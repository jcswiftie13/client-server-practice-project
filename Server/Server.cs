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
        }

        public string HandleInput(Dictionary<string, int> storage, byte[] bytes)
        {
            int bytesLeft;
            string dataIn = null;
            //接收訊息大小
            int bytesRec = handler.Receive(bytes);
            bytesLeft = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, bytesRec));
            //接收訊息
            while (bytesLeft > 0)
            {
                bytesRec = handler.Receive(bytes);
                dataIn += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                bytesLeft -= bytesRec;
            }
            return HandleCommand(dataIn, storage);
        }
        public void HandleOutput(string dataOut)
        {
            string length;
            byte[] msg = Encoding.ASCII.GetBytes(dataOut);
            //取得訊息大小
            length = Convert.ToString(msg.Length);
            //傳送訊息大小
            byte[] byteLength = Encoding.ASCII.GetBytes(length);
            int bytesSent = handler.Send(byteLength);
            //傳送訊息
            bytesSent = handler.Send(msg);
        }
        private void SetDic(Dictionary<string, int> storage, string name, string value)
        {
            storage[name] = Convert.ToInt32(value);
        }
        public string HandleCommand(string command, Dictionary<string, int> storage)
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
                        return GetHandle(splitted, storage);
                    case "set":
                        return SetHandle(splitted, storage);
                    case "ping":
                        return PingHandle();
                    default:
                        return "Invalid command.";
                }
            }
        }
        private string SetHandle(string[] command, Dictionary<string, int> storage)
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
        private string GetHandle(string[] command, Dictionary<string, int> storage)
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
        private string PingHandle()
        {
            return "PONG";
        }
    }
}