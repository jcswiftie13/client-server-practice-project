using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class DBClient
    {
        private Socket sender;
        public DBClient()
        {
            Console.WriteLine("Input IP address: ");
            string ip = Console.ReadLine();
            Console.WriteLine("Input port: ");
            string port = Console.ReadLine();
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint RemoteEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(port));
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(RemoteEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void Start()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                inputHandle(bytes);
            }
        }
        private void inputHandle(byte[] bytes)
        {
            Console.Write("Input command: ");
            string input = Console.ReadLine();
            send(input, sender);
            receive(bytes, sender);
        }
        private void send(string input, Socket sender)
        {
            byte[] msg = Encoding.ASCII.GetBytes(input);
            int bytesSent = sender.Send(msg);
        }
        private void receive(byte[] bytes, Socket sender)
        {
            string dataIn = null;
            do
            {
                int bytesRec = sender.Receive(bytes);
                dataIn += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            } while (!Array.Exists(bytes, c => c == 0));
            Console.WriteLine(dataIn);
        }
    }
}