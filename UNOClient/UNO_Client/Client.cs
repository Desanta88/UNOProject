using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UNO_Client
{
    public class Client
    {
        private TcpClient client;
        public NetworkStream stream;
        Thread receiveThread;
        public string CurrentMessage;

        public Client(string serverIP, int serverPort)
        {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();

            Console.WriteLine("Connected to server.");

            receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }

        public void SendMessage(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        private void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    stream.Read(buffer, 0, buffer.Length);
                    string receivedMessage = Encoding.ASCII.GetString(buffer);
                    receivedMessage = receivedMessage.Replace("\0", "");
                    CurrentMessage = receivedMessage;
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Server disconnected.");
            }
        }

        public void Close()
        {
            client.Close();
        }
        public TcpClient GetClient
        {
            get { return client; }
        }

    }
}
