using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UNO_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int porta = 8000;
            int nClient = 0;
            string username = "";
            TcpClient Client = new TcpClient();
            Server server = new Server(porta);
            server.Start();

        }
    }
}
