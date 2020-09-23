using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ServerSocket
{
    public Socket init_socket(Socket listener, int port)
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
    public string SetHandle(string[] command, Socket handler, Dictionary<string, int> storage)
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
    public string GetHandle(string[] command, Socket handler, Dictionary<string, int> storage)
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
    public string PingHandle()
    {
        return "PONG";
    }
}

public class Server
{
    private static Dictionary<string, int> storage = new Dictionary<string, int>();
    private static Socket listener = null;
    public static void Main()
    {
        var util = new utils();
        var socket = new ServerSocket();
        listener = socket.init_socket(listener, 11000);
        Socket handler = listener.Accept();
        while (true)
        {
            string data_in = null;
            string data_out = null;
            string[] command = null;
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data_in += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            command = data_in.Split(" ");
            if (command.Length > 3)
            {
                byte[] msg = Encoding.ASCII.GetBytes("Invalid number of arguments");
                int bytesSent = handler.Send(msg);
            }
            else
            {
                if (command[0].ToLower() == "get")
                {
                    data_out = util.GetHandle(command, handler, storage);
                }
                else if (command[0].ToLower() == "set")
                {
                    data_out = util.SetHandle(command, handler, storage);
                }
                else if (command[0].ToLower() == "ping")
                {
                    data_out = util.PingHandle();
                }
                byte[] msg = Encoding.ASCII.GetBytes(data_out);
                int bytesSent = handler.Send(msg);
            }
        }
    }
}