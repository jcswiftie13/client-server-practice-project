using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ServerSocket
{
    public Socket InitSocket(Socket listener, int port)
    {
        try
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return listener;
    }
}

public class utils
{
    private void SetDic(Dictionary<string, int> storage, string name, string value)
    {
        storage[name] = Convert.ToInt32(value);
    }
    public string CommandParser(string[] command, Dictionary<string, int> storage)
    {
        string data_out = null;
        if (command.Length > 3)
        {
            data_out = "Invalid number of arguments.";
            return data_out;
        }
        else
        {
            switch (command[0].ToLower())
            {
                case "get":
                    data_out = GetHandle(command, storage);
                    break;
                case "set":
                    data_out = SetHandle(command, storage);
                    break;
                case "ping":
                    data_out = PingHandle();
                    break;
                default:
                    data_out = "Invalid command.";
                    break;
            }
            return data_out;
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

public class Server
{
    private static Dictionary<string, int> storage = new Dictionary<string, int>();
    private static Socket handler = null;
    public static void Main()
    {
        StartServer();
    }
    public static void StartServer()
    {
        var socket = new ServerSocket();
        var util = new utils();
        handler = socket.InitSocket(handler, 11000);
        handler = handler.Accept();
        while (true)
        {
            string data_in = null;
            string data_out = null;
            string[] command = null;
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data_in += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            command = data_in.Split(" ");
            data_out = util.CommandParser(command, storage);
            byte[] msg = Encoding.ASCII.GetBytes(data_out);
            int bytesSent = handler.Send(msg);
        }
    }
}