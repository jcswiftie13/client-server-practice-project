using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.NetworkInformation;

public class SocketClient
{
    public static int Main(String[] args)
    {
        StartClient();
        return 0;
    }


    public static void StartClient()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[1];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

        Socket sender = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
        sender.ReceiveTimeout = 1000;
        sender.SendTimeout = 1000;

        try
        {
            sender.Connect(remoteEP);
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

            while (true)
            {
                byte[] bytes = new byte[1024];
                string data = null;
                Console.Write("Input: ");
                string input = Console.ReadLine();
                if (input == "PING")
                {
                    using (var ping = new Ping())
                    {
                        var pingReply = ping.Send(ipAddress);

                        if (pingReply.Status == IPStatus.Success)
                        {
                            Console.WriteLine("Ping to " + "[" + ipAddress + "]" + " Successful"
                   + " Response delay = " + pingReply.RoundtripTime.ToString() + " ms" + "\n");
                        }
                        else
                        {
                            Console.WriteLine(pingReply.Status);
                        }
                        ((IDisposable)ping).Dispose();
                    }
                    continue;
                }
                //輸入指令, name和value
                byte[] msg = Encoding.ASCII.GetBytes(input);
                int bytesSent = sender.Send(msg);
                //接受回傳訊息
                int bytesRec = sender.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine(data);
                if (input == "quit()")
                {
                    break;
                }
            }
            
            // Release the socket.    
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

        }
        catch (ArgumentNullException ane)
        {
            Console.WriteLine(ane.ToString());
        }

    }
}