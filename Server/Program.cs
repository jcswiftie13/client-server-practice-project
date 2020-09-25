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
            var socket = new DBServer("127.0.0.1", 11000);
            var CommandHandler = new CommandHandler();
            socket.Start();
            string dataOut = socket.HandleInput(storage);
            socket.HandleOutput(dataOut);
        }
    }
}
