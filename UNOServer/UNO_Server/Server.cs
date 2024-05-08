using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace UNO_Server
{
    public class Server
    {
        private TcpListener listener;
        private Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        Dictionary<int, string> Usernames = new Dictionary<int, string>();
        private int clientCount;
        private string CurrentCard;
        private List<Thread> listT = new List<Thread>();

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            clientCount = 0;
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Server in ascolto");

            while (clientCount<2)
            {
                TcpClient client = listener.AcceptTcpClient();
                Byte[] ReceiveBytes = new Byte[client.ReceiveBufferSize];
                clientCount++;
                clients.Add(clientCount,client);
                NetworkStream ns = client.GetStream();
                ns.Read(ReceiveBytes, 0, client.ReceiveBufferSize);
                string username = Encoding.ASCII.GetString(ReceiveBytes).Replace("\0", "");
                Usernames.Add(clientCount, username);
                Console.WriteLine("Client connesso con username "+username);
                Byte[] SendBytes = Encoding.ASCII.GetBytes("sei connesso");
                ns.Write(SendBytes, 0, SendBytes.Length);

                Thread clientThread = new Thread(() => HandleClient(client));
                listT.Add(clientThread);
                clientThread.Start();
            }
            if (clientCount == 2)
            {
                Random rId = new Random();
                int PrimoTurno = rId.Next(1, 3);
                string user = Usernames[PrimoTurno];
                CurrentCard = GetNeutralCard();
                Console.WriteLine("Inizio partita");
                BroadcastMessage($"start;{CurrentCard};{user}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Ricevuto dal client({receivedMessage.Split(';')[1]}): {receivedMessage}");
                    if(receivedMessage.Contains("winner")==true)
                    {
                        Console.WriteLine($"il giocatore {receivedMessage.Split(':')[0]} ha vinto!");
                        BroadcastMessage(receivedMessage);
                        EndThreads();
                        listener.Stop();
                        Environment.Exit(0);
                    }
                    else
                        BroadcastMessage(receivedMessage);
                    
                }
            }
        }
        private void BroadcastMessage(string message)
        {
            foreach (TcpClient client in clients.Values)
            {
                NetworkStream stream = client.GetStream();
                Byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, message.Length);
            }
        }
        private void EndThreads()
        {
            for(int i = 0; i < listT.Count; i++)
            {
                listT[i].Abort();
            }
        }
        public int GetClientCount
        {
            get { return clientCount; }
        }
        public string GetCardCodes(int nc)
        {
            string[] simboli = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "r", "s","p2"};
            string[] colori = new string[] { "y", "g", "b", "re", "cc","p4" };
            string codes = "";
            Random r = new Random();
            for (int i = 0; i < nc; i++)
            {
                int nSimbolo = r.Next(0, simboli.Length);
                int nColore = r.Next(0, colori.Length);
                if (colori[nColore] == "cc" || colori[nColore] == "p4")
                {
                    codes += colori[nColore] + ":" + colori[nColore];
                    if (i < nc - 1)
                    {
                        codes += ";";
                    }
                }
                else
                {
                    codes += colori[nColore] + ":" + simboli[nSimbolo];
                    if (i < nc - 1)
                    {
                        codes += ";";
                    }
                }
            }
            return codes;
        }
        public string GetNeutralCard()
        {
            bool hasColor = false;
            string card = "";
            while (hasColor == false)
            {
                card = GetCardCodes(1);
                if (card.Contains("p4") == false && card.Contains("cc") == false)
                    hasColor = true;
            }

            return card;
        }
    }
}
