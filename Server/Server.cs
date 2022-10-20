using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Packets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    class Server
    {
        UdpClient udpListener;
        BinaryFormatter formatter;

        Thread clientThread;
        Thread udpListen;
        struct GameLobby
        {
            public Client gameHost;
            public Client GamePlayer;
            public int gameID;
        }
        GameLobby[] currentLobbies;

        protected TcpListener tcpListener;
        protected ConcurrentDictionary<int, Client> clients;
        protected ConcurrentDictionary<int, Client> gameclients;
        protected ConcurrentDictionary<int, ClientNamePacket> clientNames;

        protected Dictionary<string, List<string>> groupChat;

        //creates a server and passes an ip address and a port number
        public Server(string IpAdress, int port)
        {
            //convert the IP from a sting to and IPAddress data type
            IPAddress adress = IPAddress.Parse(IpAdress);
            //creates a new tcplistener with the IP address and the port
            tcpListener = new TcpListener(adress, port);
            udpListener = new UdpClient(port);
            formatter = new BinaryFormatter();
            udpListen = new Thread(() => { UDPListen(); });
            udpListen.Start();
            currentLobbies = new GameLobby[24];
            // loop through all the avaliable game lobbies,
            //set all of the ID' to equal -1,
            //essentially stating its an empty lobby
            for (int i = 0; i < currentLobbies.Length; i++)
            {
                currentLobbies[i] = new GameLobby();
                currentLobbies[i].gameID = -1;
            }

            Console.WriteLine("connected to IPaddress : " + IpAdress + " and port " + port);
        }

        void UDPListen()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] bytes = udpListener.Receive(ref endpoint);

                MemoryStream stream = new MemoryStream(bytes);
                Packet recievedMessage;

                recievedMessage = formatter.Deserialize(stream) as Packet;
                foreach (KeyValuePair<int, Client> client in clients)
                {
                    if (endpoint.ToString() == client.Value.endpoint.ToString())
                    {
                        udpListener.Send(bytes, bytes.Length, client.Value.endpoint);
                    }
                    if (recievedMessage.packetType == PacketType.MovePacket)
                    {
                        MovePacket sendMove = (MovePacket)recievedMessage;
                        for (int i = 0; i < currentLobbies.Length; i++)
                        {
                            if (currentLobbies[i].gameID != -1)
                            {
                                currentLobbies[i].GamePlayer.Send(sendMove);
                                currentLobbies[i].gameHost.Send(sendMove);
                            }
                        }
                    }
                }
            }
        }

        public void Start()
        {
            clients = new ConcurrentDictionary<int, Client>();
            gameclients = new ConcurrentDictionary<int, Client>();
            clientNames = new ConcurrentDictionary<int, ClientNamePacket>();

            groupChat = new Dictionary<string, List<string>>();
            int clientIndex = 0;
            //starts the tcplistener
            tcpListener.Start();

            Console.WriteLine("Waiting....");

            while (clients.Count < 5)
            {
                //creates a sockket variable that is returned by the accept socket method
                Socket socket = tcpListener.AcceptSocket();
                int index = clientIndex;
                Client client = new Client(socket);
                clientIndex++;
                clients.TryAdd(index, client);

                Console.WriteLine("Connection has been made - There are " + clients.Count + " Clients connected");

                clientThread = new Thread(() => { ClientMethod(index); } );
                clientThread.Start();
                //Console.WriteLine("new thread");
            }
            Console.WriteLine("Server is full");
        }

        public void Stop()
        {
            udpListen.Abort();
            clientThread.Abort();
            //stops the tcplistener
            tcpListener.Stop();
        }

        //method used to read and write from the client
        public void ClientMethod(int index)
        {
            //writes a message to the server
            //client.Send("Hi, i am The server, WLECOME - You may NEVER leave");
            //creates a loop that allows the client and server to be in constant communication
            //the loop will always be running until broken out off, as reader.readline() is a blocking call
            //meaning that it will pause until some data is recieved
            //creates a string used to recieve messages from the server
            ChatMessagePacket message = new ChatMessagePacket("Hi, i am The server, WLECOME - You may NEVER leave");
            clients[index].Send(message);

            Packet recievedMessage;
            try
            {
                while ((recievedMessage = clients[index].Read()) != null)
                {
                    switch (recievedMessage.packetType)
                    {
                        case PacketType.ChatMessage:
                            ChatMessagePacket chat = (ChatMessagePacket)recievedMessage;
                            foreach (KeyValuePair<int, Client> curClient in clients)
                            {
                                if (chat.message != null)
                                {
                                    curClient.Value.Send(clientNames[index]);
                                    curClient.Value.Send(chat);
                                }
                            }
                            break;
                        case PacketType.PrivateMessage:
                            PrivateMessage pMessage = (PrivateMessage)recievedMessage;
                            pMessage.sender = clients[index].GetName();
                            foreach (KeyValuePair<int, Client> curClient in clients)
                            {
                                if (curClient.Value.GetName() == pMessage.recipient)
                                {
                                    curClient.Value.Send(pMessage);
                                    pMessage.sender = null;
                                    clients[index].Send(pMessage);
                                }
                            }
                            break;
                        case PacketType.ClientName:
                            ClientNamePacket name = (ClientNamePacket)recievedMessage;
                            clients[index].SetName(name.name);
                            clientNames.TryAdd(index, name);
                            foreach (KeyValuePair<int, Client> curClient in clients)
                            {
                                foreach (KeyValuePair<int, ClientNamePacket> curClient1 in clientNames)
                                {
                                    curClient.Value.Send(curClient1.Value);
                                }
                            }
                            break;
                        case PacketType.Disconnect:
                            Disconnect disconnect = (Disconnect)recievedMessage;
                            Client client = clients[index];
                            client.Close();
                            clients.TryRemove(index, out client);
                            break;
                        case PacketType.GroupMessage:
                            GroupMessage groupMessage = (GroupMessage)recievedMessage;
                            groupMessage.sender = clients[index].GetName();
                            if (!groupChat.ContainsKey(groupMessage.GroupName))
                            {
                                groupChat.Add(groupMessage.GroupName, groupMessage.recipients);
                            }
                            //Console.WriteLine(groupMessage.recipients[0]);
                            foreach (KeyValuePair<int, Client> curClient in clients)
                            {
                                if (groupMessage.recipients.Contains(curClient.Value.GetName()))
                                {
                                    curClient.Value.Send(groupMessage);
                                }
                            }
                            foreach (KeyValuePair<string, List<string>> currentChat in groupChat)
                            {
                                if (groupMessage.GroupName == currentChat.Key)
                                {
                                    foreach (string recipient in currentChat.Value)
                                    {

                                    }
                                }
                            }

                            break;
                        case PacketType.ConnectToGame:
                            //server recieves this packet 
                            ConnectToGame connect = (ConnectToGame)recievedMessage;
                            //find the next avaliable game
                            if (connect.isHost)
                            {
                                GameLobby newGame = new GameLobby();

                                //sets the host of the lobby equal to whoever clicked the host button
                                newGame.gameHost = clients[index];
                                newGame.GamePlayer = null;
                                //sets the game ID to 1, as this is our first lobby
                                newGame.gameID = 1;

                                currentLobbies[0] = newGame;
                            }
                            //if the client doesnt click on the host button
                            else
                            {
                                //loop throough all the lobbies 
                                for (int i = 0; i < currentLobbies.Length; i++)
                                {
                                    //if the current ID of aa lobby is -1 then this lobby is empty
                                    //therefore the join game button wont allow a game to be joined
                                    if (currentLobbies[i].gameID == -1)
                                    {
                                        continue;
                                    }
                                    //if the second client clicks join and there is a game host to join
                                    if (connect.hostName == null)
                                    {
                                        Console.WriteLine("Hosted");
                                        //sets the player of the lobby to equal the client that clicked the button
                                        currentLobbies[i].GamePlayer = clients[index];
                                        //creates a new packet that passes in a random bool, and the host of the lobby
                                        ConnectToGame connectMessage = new ConnectToGame(true, currentLobbies[i].gameHost.GetName());
                                        //you know want to send an aknowledgement back to the player to say they are connected
                                        //to do this, we first fill the players name into the packet 
                                        connectMessage.playerName = currentLobbies[i].GamePlayer.GetName();
                                        //we fill the packets message to send to the host side
                                        connectMessage.message = connectMessage.playerName + " You are connected, " + connectMessage.hostName + " Is the Host!";
                                        currentLobbies[i].gameHost.Send(connectMessage);
                                        //we finally change this message, and send it to the client side
                                        connectMessage.message = "You are the host, and " + connectMessage.playerName + " is connected!";
                                        currentLobbies[i].GamePlayer.Send(connectMessage);
                                        break;
                                    }
                                }
                            }
                            Console.WriteLine(currentLobbies[0].gameHost + " " + currentLobbies[0].GamePlayer + " " + currentLobbies[0].gameID);
                            break;
                        case PacketType.LoginPacket:
                            LoginPacket login = (LoginPacket)recievedMessage;
                            clients[index].endpoint = login.endPoint;
                            break;
                        default:
                            break;
                    }


                    /*
                    Console.WriteLine("Response....");
                    //pass the recieved message into the GetReturneMessage method
                    //the returned string from this method will be the servers response
                    string response = GetReturnedMessage(recievedMessage);
                    //writes the servers response
                    client.Send(response);         

                    //if the client respond this word
                    if (recievedMessage == "end")
                    {
                        //break from the loop
                        break;
                    }
                    */
                }
            }
            catch
            {

            }
            //terminate the connection
            clients[index].Close();
        }

        public string GetReturnedMessage(string code)
        {
            switch (code)
            {
                case "hi":
                    return "Hello";
                case "When were you born?":
                    DateTime birth = new DateTime(2020, 11, 06, 16, 36, 00);
                    return "I was created at " + birth.ToString();
                case "what is my name":
                    return "You hav not given me a name to call you";
                default:
                    return "I have no response for that";
            }
        }
    }
}
