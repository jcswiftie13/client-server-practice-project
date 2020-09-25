using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    public void InputHandle(byte[] bytes, string data, Socket sender)
    {
        int bytesSent;
        int bytesRec;
        Console.Write("Input command: ");
        string input = Console.ReadLine();
        do
        {
            byte[] msg = Encoding.ASCII.GetBytes(input);
            bytesSent = sender.Send(msg);
        } while (bytesSent > 0);
        do
        {
            bytesRec = sender.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        } while (bytesRec > 0);
        Console.WriteLine(data);
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
            string data = null;
            socket.InputHandle(bytes, data, sender);
        }
    }
}