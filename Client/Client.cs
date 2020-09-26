using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public class ClientSocket
{
    public Socket InitSocket(Socket sender, string address, int port)
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint RemoteEndPoint = new IPEndPoint(ipAddress, port);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(RemoteEndPoint);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return sender;
    }
    public void InputHandle(byte[] bytes, Socket sender)
    {
        string length;
        Console.Write("Input command: ");
        string input = Console.ReadLine();
        byte[] msg = Encoding.ASCII.GetBytes(input);
        Send(msg, sender);
        Receive(bytes, sender);
    }
    private void Send(byte[] msg, Socket sender)
    {
        string length;
        //取得訊息大小
        length = Convert.ToString(msg.Length);
        byte[] byteLength = Encoding.ASCII.GetBytes(length);
        //傳送訊息大小
        int bytesSent = sender.Send(byteLength);
        //傳送訊息
        bytesSent = sender.Send(msg);
    }
    private void Receive(byte[] bytes, Socket sender)
    {
        int bytesLeft;
        string dataIn = null;
        int bytesRec = sender.Receive(bytes);
        bytesLeft = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        while (bytesLeft > 0)
        {
            bytesRec = sender.Receive(bytes);
            dataIn += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            bytesLeft -= bytesRec;
        }
        Console.WriteLine(dataIn);
    }
}

public class client
{
    private static Socket sender = null;
    public static void Main(String[] args)
    {
        StartClient();
    }
    private static void StartClient()
    {
        var socket = new ClientSocket();
        Console.WriteLine("Input IP address: ");
        string address = Console.ReadLine();
        Console.WriteLine("Input port: ");
        string port = Console.ReadLine();
        sender = socket.InitSocket(sender, address, Convert.ToInt32(port));
        while (true)
        {
            byte[] bytes = new byte[1024];
            socket.InputHandle(bytes, sender);
        }
    }
}