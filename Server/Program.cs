using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main()
        {
            //creates a new instance of the serve, with the IP and port
            //the IP is a looopback address, that loops to the same IP of the computer
            Server server = new Server("127.0.0.1", 4444);
            server.Start();
            server.Stop();
        }
    }
}