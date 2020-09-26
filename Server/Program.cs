using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace Server
{
    class Program
    {
        private static Dictionary<string, int> storage = new Dictionary<string, int>();
        public static void Main()
        {
            byte[] bytes = new byte[1024];
            var socket = new DBServer("127.0.0.1", 11000);
            socket.Start();
            while (true)
            {
                string dataOut = socket.HandleInput(storage, bytes);
                socket.HandleOutput(dataOut);
            }
        }
    }
}
