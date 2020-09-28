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
            byte[] msg = Encoding.ASCII.GetBytes(input);
            send(msg, sender);
            receive(bytes, sender);
        }
        private void send(byte[] msg, Socket sender)
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
        private void receive(byte[] bytes, Socket sender)
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
}
