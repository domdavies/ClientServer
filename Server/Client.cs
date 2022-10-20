using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    class Client
    {
        public IPEndPoint endpoint;
        Socket socket;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter formatter;
        object readlock;
        object writelock;
        string name;

        public Client(Socket _socket)
        {
            readlock = new object();
            writelock = new object();
            formatter = new BinaryFormatter();
            socket = _socket;
            //creates a network stream that allows data to be sent over a network
            stream = new NetworkStream(socket);
            //creates a streamreader that reads messages sent to the server
            reader = new BinaryReader(stream, Encoding.UTF8);
            //creates a streamwriter that writes to the server
            writer = new BinaryWriter(stream, Encoding.UTF8);
            formatter = new BinaryFormatter();
        }

        public void SetName(string _name)
        {
            name = _name;
        }

        public string GetName()
        {
            return name;
        }

        public Packets.Packet Read()
        {
            lock (readlock)
            {
                int numberOfBytes;
                try
                {
                    if ((numberOfBytes = reader.ReadInt32()) != -1)
                    {
                        //read the correct amount of bytes that were sent over the stream
                        byte[] buffer = reader.ReadBytes(numberOfBytes);
                        //create a new memory stream and pass in the amount of data in the buffer
                        MemoryStream stream = new MemoryStream(buffer);
                        //returns a deserialized packet
                        return formatter.Deserialize(stream) as Packets.Packet;
                    }
                }
                catch
                {

                }
                return null;
                //return reader.ReadLine();
            }
        }

        public void Send(Packets.Packet message)
        {
            lock (writelock)
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
            }
        }

        public void Close()
        {
            stream.Close();
            reader.Close();
            writer.Close();
            socket.Close();
        }
    }
}
