using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace Server
{
    class Program
    {
        public static void Main()
        {
            var socket = new DBServer("127.0.0.1", 11000);
            socket.Start();
        }
    }
}
