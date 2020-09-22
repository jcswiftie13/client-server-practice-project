using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.NetworkInformation;

public class ClientSocket
{
    public Socket init_socket(Socket sender, string address, int port)
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
}

public class client
{
    private static Socket sender = null;
    public static void Main(String[] args)
    {
        var socket = new ClientSocket();
        Console.WriteLine("Input IP address: ");
        string address = Console.ReadLine();
        Console.WriteLine("Input port: ");
        string port = Console.ReadLine();
        sender = socket.init_socket(sender, address, Convert.ToInt32(port));
        while (true)
        {
            byte[] bytes = new byte[1024];
            string data = null;
            Console.Write("Input command: ");
            string input = Console.ReadLine();
            //輸入指令, name和value
            byte[] msg = Encoding.ASCII.GetBytes(input);
            int bytesSent = sender.Send(msg);
            //接受回傳訊息
            int bytesRec = sender.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Console.WriteLine(data);
        }
    }
}