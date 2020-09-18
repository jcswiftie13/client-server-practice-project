using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketListener
{
    private static Dictionary<string, int> storage = new Dictionary<string, int>();
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }
    private static void SetDic(string name, string value)
    {
        storage.Add(name, Convert.ToInt32(value));
    }
    public static void StartServer()
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
        try
        {  
            Socket listener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Waiting for connection...");
            Socket handler = listener.Accept();

            while (true)
            {
                string data = null;
                string[] command = null;
                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                command = data.Split(" ");
                if(command[0].ToLower() == "set")
                {
                    try
                    {
                        SetDic(command[1], command[2]);
                        byte[] msg = Encoding.ASCII.GetBytes($"SET {command[1]} OK");
                        int bytesSent = handler.Send(msg);
                    }
                    catch(Exception e)
                    {
                        byte[] msg = Encoding.ASCII.GetBytes(e.ToString());
                        int bytesSent = handler.Send(msg);
                    }
                }
                else if(command[0].ToLower() == "get")
                {
                    try
                    {
                        if (storage.ContainsKey(command[1]))
                        {
                            byte[] msg = Encoding.ASCII.GetBytes($"GET {command[1]} {storage[command[1]]}");
                            int bytesSent = handler.Send(msg);
                        }
                        else
                        {
                            byte[] msg = Encoding.ASCII.GetBytes($"Did not set this name.");
                            int bytesSent = handler.Send(msg);
                        }
                    }
                    catch(Exception e)
                    {
                        byte[] msg = Encoding.ASCII.GetBytes(e.ToString());
                        int bytesSent = handler.Send(msg);
                    }
                }
                else if((command[0].ToLower() != "quit()") && (command[0].ToLower() != "ping"))
                {
                    byte[] msg = Encoding.ASCII.GetBytes($"Invalid input \"{command[0]}\"...please reenter your command.");
                    int bytesSent = handler.Send(msg);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }
}