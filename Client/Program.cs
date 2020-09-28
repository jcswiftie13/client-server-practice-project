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
            var socket = new DBClient();
            socket.Start();
        }
    }
}
