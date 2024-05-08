using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace UNO_Client
{
    public partial class Start : Form
    {
        string server = "127.0.0.1";
        int porta = 8000;
        Client client;
        string receivedData;
        Label l = new Label();
        int secondi = 5;

        public Start()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)//comunicazione con il server
        {
            if (textBox1.Text != "")
            {
                client = new Client(server, porta);
                client.SendMessage(textBox1.Text);
                receivedData = client.CurrentMessage;
                labelNotice.Visible = true;
                timerReady.Start();
            }
            else
                MessageBox.Show("inserire il username");
        }

        private void timerReady_Tick(object sender, EventArgs e)//timer azionato quando il client riceve l'ok dal server
        {
            receivedData = client.CurrentMessage.Replace("\0", "");
            if (receivedData.Contains("start"))
            {
                timerReady.Stop();
                labelNotice.Text = $"la partità inizierà tra {secondi} secondi";
                labelNotice.Location = new Point(250, 373);
                timerStart.Start();
            }
        }

        private void timerStart_Tick(object sender, EventArgs e)// timer per il countdown
        {
            secondi--;
            labelNotice.Text = $"la partità inizierà tra {secondi} secondi";
            if (secondi == 0)
            {
                string[] data = client.CurrentMessage.Split(";");
                string fieldCard = data[1];
                Form1 form1 = new Form1(client, textBox1.Text, fieldCard, data[2]);
                timerStart.Stop();
                this.Hide();
                form1.ShowDialog();

            }
        }
    }
}
