using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Client;

namespace Client
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Input IP address: ");
            string ip = Console.ReadLine();
            Console.WriteLine("Input port: ");
            string port = Console.ReadLine();
            var socket = new DBClient(ip, Convert.ToInt32(port));
            while (true)
            {
                byte[] bytes = new byte[1024];
                socket.InputHandle(bytes);
            }
        }
    }
}
