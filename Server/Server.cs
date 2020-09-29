using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class DBServer
    {
        public Socket listener;
        public Socket handler;
        private Dictionary<string, string> storage = new Dictionary<string, string>();
        public DBServer(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
        }

        public void Start()
        {
            listener.Listen(10);
            handler = listener.Accept();
            while (true)
            {
                byte[] bytes = new byte[1024];
                string dataOut = handleInput(storage, bytes);
                handleOutput(dataOut);
            }
        }

        private string handleInput(Dictionary<string, string> storage, byte[] bytes)
        {
            string dataIn = null;
            do
            {
                int bytesRec = handler.Receive(bytes);
                dataIn += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            } while (!Array.Exists(bytes, c => c == 0));
            return handleCommand(dataIn, storage);
        }
        private void handleOutput(string dataOut)
        {
            byte[] msg = Encoding.ASCII.GetBytes(dataOut);
            int bytesSent = handler.Send(msg);
        }
        private void SetDic(Dictionary<string, string> storage, string name, string value)
        {
            storage[name] = value;
        }
        private string handleCommand(string command, Dictionary<string, string> storage)
        {
            string[] splitted = command.Split(" ");
            if (splitted.Length > 3)
            {
                return "Invalid number of arguments.";
            }
            else
            {
                switch (splitted[0].ToLower())
                {
                    case "get":
                        return getHandle(splitted, storage);
                    case "set":
                        return setHandle(splitted, storage);
                    case "ping":
                        return pingHandle();
                    default:
                        return "Invalid command.";
                }
            }
        }
        private string setHandle(string[] command, Dictionary<string, string> storage)
        {
            if (command.Length == 3)
            {
                SetDic(storage, command[1], command[2]);
                return $"SET {command[1]} OK";
            }
            else
            {
                return "Missing name or value.";
            }
        }
        private string getHandle(string[] command, Dictionary<string, string> storage)
        {
            if (command.Length == 2)
            {
                if (storage.ContainsKey(command[1]))
                {
                    return $"GET {command[1]} {storage[command[1]]}";
                }
                else
                {
                    return "Did not set this name.";
                }
            }
            else
            {
                return "Missing name.";
            }
        }
        private string pingHandle()
        {
            return "PONG";
        }
    }
}