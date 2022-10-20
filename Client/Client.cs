using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;

namespace Client
{ 
    public class Client
    {
        public Gameform gameForm;
        public bool isHost;
        TcpClient tcpClient;
        NetworkStream stream;
        BinaryWriter writer;
        BinaryReader reader;
        BinaryFormatter formatter;
        UdpClient udpClient;

        Thread gameThread;
        Thread UDPThread;
        Thread TCPthread;

        private ClientForm clientForm;
        public Client()
        {
            clientForm = new ClientForm(this);
            clientForm.ShowDialog();
            Close();
        }

        public void Login()
        {
            IPEndPoint endpoint = (IPEndPoint)udpClient.Client.LocalEndPoint;
            LoginPacket login = new LoginPacket(endpoint);
            SendMessage(login);
        }

        public void UDPSendMessage(Packet _packet)
        {
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, _packet);
            //store the byte array of the stream into a variable
            byte[] buffer = stream.GetBuffer();
            udpClient.Send(buffer, buffer.Length);
        }

        void UDPProcessServerResponce() 
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    byte[] bytes = udpClient.Receive(ref endpoint);

                    MemoryStream stream = new MemoryStream(bytes);
                    Packet recievedMessage;

                    recievedMessage = formatter.Deserialize(stream) as Packet;
                    switch (recievedMessage.packetType)
                    {
                        case PacketType.MovePacket:
                            MovePacket move = (MovePacket)recievedMessage;
                            gameForm.game1.MovePiece((int)move.x, (int)move.y);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
            }
        }

        public void StartGame()
        {
            gameForm = new Gameform(this);

            gameThread = new Thread(() => { gameForm.ShowDialog(); });
            gameThread.Start();
        }

        public void SendMessage(Packet message)
        {
            //create a new memory stream
            MemoryStream stream = new MemoryStream();
            //serialize the data passed in and store it into the stream
            formatter.Serialize(stream, message);
            //store the byte array of the stream into a variable
            byte[] buffer = stream.GetBuffer();
            //write the length of the array to the writer
            writer.Write(buffer.Length);
            //write the bye array to the writer
            writer.Write(buffer);
            //writer.WriteLine(message);
            writer.Flush();

            //writer.Write(message);
            //writer.Flush();
        }

        public bool Connect(string address, int port)
        {
            try
            {
                IPAddress IP = IPAddress.Parse(address);
                //trys to connect to the server that is passed
                tcpClient = new TcpClient();
                udpClient = new UdpClient();
                tcpClient.Connect(IP, port);

                udpClient.Connect(IP, port);
                //sets the network stream = stream
                stream = tcpClient.GetStream();
                //creates a streamreader that reads messages sent to the client
                reader = new BinaryReader(stream, Encoding.UTF8);
                //creates a streamwriter that writes to the client
                writer = new BinaryWriter(stream, Encoding.UTF8);
                formatter = new BinaryFormatter();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                return false;
            }
        }

        public void Close()
        {
            TCPthread.Abort();
            UDPThread.Abort();
            stream.Close();
            tcpClient.Close();
            udpClient.Close();
        }

        public void Run()
        {
            TCPthread = new Thread(() => { ProcessServerResponse(); });
            TCPthread.Start();

            UDPThread = new Thread(() => { UDPProcessServerResponce(); });
            UDPThread.Start();
            Login();
        }

        private void ProcessServerResponse()
        {
            int numberOfBytes;
            try
            {
                while ((numberOfBytes = reader.ReadInt32()) != -1)
                {
                    //read the correct amount of bytes that were sent over the stream
                    byte[] buffer = reader.ReadBytes(numberOfBytes);
                    //create a new memory stream and pass in the amount of data in the buffer
                    MemoryStream stream = new MemoryStream(buffer);
                    Packet recievedMessage;
                    //returns a deserialized packet
                    recievedMessage = formatter.Deserialize(stream) as Packet;
                    switch (recievedMessage.packetType)
                    {
                        //-----------------Chat Messaging System-----------------------
                        //server sends out a response to the relevent clients
                        case PacketType.ChatMessage:
                            ChatResponse(recievedMessage);
                            break;
                        case PacketType.PrivateMessage:
                            PrivateResponse(recievedMessage);
                            break;
                        case PacketType.ClientName:
                            ClientNameResponse(recievedMessage);
                            break;
                        case PacketType.CreateGroup:
                            CreateGroup(recievedMessage);
                            break;
                        case PacketType.GroupMessage:
                            GroupMessage(recievedMessage);
                            break;
                        //-------------------------------------------------------------
                        //-------------------------------------------------------------


                        //-----------------Game Messaging System-----------------------
                        case PacketType.ConnectToGame:
                            ConnectToGame connectPacket = (ConnectToGame)recievedMessage;
                            StartGame();
                            gameForm.UpdateGameWindow(connectPacket.message);
                            break;
                        case PacketType.MovePacket:
                            MovePacket move = (MovePacket)recievedMessage;
                            gameForm.game1.MovePiece((int)move.x, (int)move.y);
                            break;
                        default:
                            break;
                        //-------------------------------------------------------------
                        //-------------------------------------------------------------
                    }
                }
            }
            catch
            {

            }
        }

        private void ChatResponse(Packet recieved)
        {
            ChatMessagePacket chat = (ChatMessagePacket)recieved;
            clientForm.UpdateChatWindow(chat.sender + " - ALL :     " + chat.message);
        }

        private void PrivateResponse(Packet recieved)
        {
            PrivateMessage pMessage = (PrivateMessage)recieved;
            if (pMessage.sender != null)
            {
                clientForm.UpdateChatWindow(pMessage.sender + " - YOU :     " + pMessage.message);
            }
            else
                clientForm.UpdateChatWindow("YOU - " + pMessage.recipient + ":     " + pMessage.message);
        }

        private void ClientNameResponse(Packet recieved)
        {
            ClientNamePacket name = (ClientNamePacket)recieved;
            if (!clientForm.listBox1.Items.Contains(name.name))
            {
                clientForm.UpdateClientNameWindow(name.name);
            }
        }

        private void CreateGroup(Packet recieved)
        {
            CreateGroup groupMessage = (CreateGroup)recieved;
            //clientForm.UpdateChatWindow(groupMessage.sender + " - " + groupMessage.GroupName + ":     " + groupMessage.message);
            clientForm.CreateGroupChat(groupMessage.GroupName);
        }

        private void GroupMessage(Packet recieved)
        {
            GroupMessage groupMessage = (GroupMessage)recieved;
            clientForm.UpdateChatWindow(groupMessage.sender + " - " + groupMessage.GroupName + ":     " + groupMessage.message);
        }
    }
}
