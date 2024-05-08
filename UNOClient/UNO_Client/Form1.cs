using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;


namespace UNO_Client
{
    public partial class Form1 : Form
    {

        public List<Button> Deck = new List<Button>();//lista di pulsanti equivalenti alle carte nel gioco
        public mazzo carte = new mazzo();//mazzo che indica le carte(non fisiche) del gioco,cioè la carta con i suoi valori come il colore 
        carta FieldCard;
        public int drawCardCounter = 0;//serve solo per l'evento del timer per distribuire le carte ogni tot secondi
        public int DistanceLeft = 50;//distanza di una carta dal bordo sinistro della finestra
        public bool isPaused = false;//variabile che indica quando il gioco è in pausa(succede soltanto quando non è il turno di un giocatore)
        public GroupBox ColorChoose; //la groupbox per scegliere il colore in caso di una carta +4 o cambia colore
        public Client Player;// instanza della classe del client
       
        public Form1(Client c,string username,string fc,string ftu)
        {
            InitializeComponent();
            this.Text = username;//imposta il nome della form nello username inserito nella schermata iniziale
            Player = c;
            string[] fcSymbolColor = fc.Split(":");
            FirstNeutralCard(fcSymbolColor[0], fcSymbolColor[1]);//scelgo la carta iniziale
            SetBoxColor(FieldCard.Colore);//cambio il colore del riquadro
            carte.DrawCards(7);//estrazione delle carte(classe carta)
            FirstDraw();//visualizzazione delle carte(pulsanti)
            FirstTurn(ftu);//inizio del turno di un giocatore
            DrawCardsTimer.Start();//inizia il timer per la distribuzione delle carte iniziali
            ChangeCardTimer.Start();//inizia il timer per la ricezione dei dati dal server
        }
        public void Block(bool b)// blocca tutti i componenti visibili sulla form
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = b;
            }
        }
        public void FirstTurn(string cu)//funzione che decide chi inizia per primo
        {
            if (cu != this.Text )
            {
                Block(false);
            }
            Player.CurrentMessage = "";
        }
        public void FirstNeutralCard(string color,string symbol)//scelta della carta del deck esterno(in mezzo)
        {
            carta card=new carta(color,symbol);

            FieldCard = card;

            NeutralDeck.Image = Image.FromFile($"./CarteUNO/{FieldCard.ToString()}.png");
        }
        public void Redraw()//ridisegna tutte le carte del mazzo(uso questa ogni volta che uso una carta)
        {
            int DistanceLeft = 50;
            for (int i = 0; i < Deck.Count; i++)
            {
                Deck[i].Top = 300;
                Deck[i].Left = DistanceLeft;
                Deck[i].Width = 54;
                Deck[i].Height = 86;
                DistanceLeft += 30;
            }
        }
        public bool isCardCompatible(List<carta> m, carta c)//guarda se le carte sono compatibili con la carta in mezzo.
        {                                                   //Se almeno una lo è allora torna true,se no false
            for (int i = 0; i < m.Count; i++)
            {
                if ((c.Colore == m[i].Colore || c.Simbolo == m[i].Simbolo) || (m[i].Colore == "p4" || m[i].Colore == "cc"))
                    return true;
            }
            return false;
        }
        public void FirstDraw()//creazione dei pulsanti(carte fisiche)
        {
            int drawCardCounter = 0;
            while (drawCardCounter < 7)
            {
                Button b = new Button();
                this.Controls.Add(b);
                b.Image = Image.FromFile($"./CarteUNO/{carte.getMazzo()[drawCardCounter].ToString()}.png");
                b.Click += new System.EventHandler(CardSelected);
                b.Visible = false;
                b.TextAlign = ContentAlignment.MiddleRight;
                Deck.Add(b);
                drawCardCounter++;
            }
            
        }

        public void SetBoxColor(string color)//cambia il colore del riquadro
        {
            if(color.Contains("re"))
                CurrentColorBox.BackColor = Color.Red;
            if (color.Contains("y"))
                CurrentColorBox.BackColor = Color.Yellow;
            if (color.Contains("b"))
                CurrentColorBox.BackColor = Color.Blue;
            if (color.Contains("g"))
                CurrentColorBox.BackColor = Color.Green;
        }
        public bool SpecialCardsEffect(carta c)//funzione che gestisce il cambio colore delle carte cc e p4
        {
            FieldCard = c;
            if (c.Colore == "p4" || c.Colore == "cc")
            {
                ColorChoose = new GroupBox();
                RadioButton red = new RadioButton();
                RadioButton blue = new RadioButton();
                RadioButton green = new RadioButton();
                RadioButton yellow = new RadioButton();

                ColorChoose.Text = "Scegli il nuovo colore";
                ColorChoose.Width = 136;
                ColorChoose.Height = 154;
                ColorChoose.Location = new Point(378, 49);
                this.Controls.Add(ColorChoose);
                ColorChoose.Controls.Add(red);
                ColorChoose.Controls.Add(blue);
                ColorChoose.Controls.Add(green);
                ColorChoose.Controls.Add(yellow);

                foreach (Control con in this.Controls)
                {
                    if (con != ColorChoose && Deck.Contains(con) == false)
                    {
                        con.Enabled = false;
                        isPaused = true;
                    }

                }
                ColorChoose.BringToFront();

                red.Text = "Red";
                red.Location = new Point(11, 33);
                red.Name = "re";
                red.Click += new EventHandler(radioButton_CheckedChanged);

                blue.Text = "Blue";
                blue.Location = new Point(11, 58);
                blue.Name = "b";
                blue.Click += new EventHandler(radioButton_CheckedChanged);

                green.Text = "Green";
                green.Location = new Point(11, 92);
                green.Name = "g";
                green.Click += new EventHandler(radioButton_CheckedChanged);

                yellow.Text = "Yellow";
                yellow.Location = new Point(11, 117);
                yellow.Name = "y";
                yellow.Click += new EventHandler(radioButton_CheckedChanged);

                return true;
            }
            return false;
        }
        private void CardSelected(object sender, EventArgs e)//evento chiamato dal pulsante premuto
        {
            if (isPaused == false)
            {
                carta SelectedCard=new carta();
                for (int i = 0; i < carte.getLength();i++)
                {
                    if (Deck.IndexOf(sender as Button) == i)
                        SelectedCard = carte.getMazzo()[i];
                }

                if (((SelectedCard.Simbolo == FieldCard.Simbolo) || (SelectedCard.Colore == FieldCard.Colore)) || (SelectedCard.Colore == "cc" || SelectedCard.Colore == "p4"))
                {
                    if (SpecialCardsEffect(SelectedCard) == false)
                    {
                        FieldCard = SelectedCard;
                        NeutralDeck.Image = Image.FromFile($"./CarteUNO/{FieldCard.ToString()}.png");
                        carte.EliminateCard(SelectedCard);
                        Deck.Remove(sender as Button);
                        (sender as Button).Dispose();
                        SetBoxColor(SelectedCard.Colore);
                        drawCardCounter--;
                        Player.SendMessage(SelectedCard.Colore + ":" + SelectedCard.Simbolo + ";" + this.Text);
                        if (SelectedCard.Simbolo == "s" || SelectedCard.Simbolo == "r" || SelectedCard.Simbolo == "p4" || SelectedCard.Simbolo == "p2")
                            Block(true);
                        else
                            Block(false);
                    }
                    else
                    {
                        carte.EliminateCard(SelectedCard);
                        Deck.Remove(sender as Button);
                        (sender as Button).Dispose();
                    }
                }
                Redraw();
                if (carte.getLength() == 0)
                {
                    Player.SendMessage(this.Text+":"+carte.getLength().ToString()+":"+"winner");
                    Player.CurrentMessage = "";
                    MessageBox.Show("Hai vinto!!!");
                    Player.Close();
                    Close();
                }
            }

        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)//quando utilizzo una carta che cambia il colore, ci saranno dei radioButton,
        {                                                                 //quindi quando faccio la mia scelta verrà attivato questo evento
            RadioButton rb = sender as RadioButton;
            FieldCard.Colore = rb.Name;
            SetBoxColor(rb.Name);
            ColorChoose.Dispose();
            isPaused = false;

            foreach (Control con in this.Controls)
            {
                con.Enabled = true;
            }

            if (FieldCard.ToString().Contains("cc"))
            {
                NeutralDeck.Image = Image.FromFile($"./CarteUNO/cccc.png");
                Player.SendMessage($"{FieldCard.Colore}:cc;{this.Text}");
                Block(false);
            }
            else if (FieldCard.ToString().Contains("p4"))
            {
                NeutralDeck.Image = Image.FromFile($"./CarteUNO/p4p4.png");
                Player.SendMessage($"{FieldCard.Colore}:p4;{this.Text}");
                Block(true);
            }
        }

        private void DrawCardsTimer_Tick(object sender, EventArgs e)//funzione del timer che aggiunge una carta alla form all'inizio del gioco
        {
            if (drawCardCounter < 7)
            {
                Deck[drawCardCounter].Top = 300;
                Deck[drawCardCounter].Left = DistanceLeft;
                Deck[drawCardCounter].Width = 54;
                Deck[drawCardCounter].Height = 86;
                Deck[drawCardCounter].Visible = true;
                DistanceLeft += 30;
                drawCardCounter++;
            }
            else
                DrawCardsTimer.Stop();
        }

        private void ChangeCardTimer_Tick(object sender, EventArgs e)//funzione del timer per la ricezione dei messaggi
        {
            if (Player.CurrentMessage != "")
            {
                string[] data = Player.CurrentMessage.Split(";");
                if(Player.CurrentMessage.Contains(";") == true)
                {
                    if (data[1] != this.Text)// costrutto che fa determinate azione in base alla carta giocata dall'avversario
                    {
                        string[] cardCodes = data[0].Split(":");
                        carta c = new carta(cardCodes[0], cardCodes[1]);
                        FieldCard = c;

                        if (cardCodes[1] == "p4")
                        {
                            int MaxP4Number = 0,i=0;
                            NeutralDeck.Image = Image.FromFile($"./CarteUNO/p4p4.png");
                            Player.CurrentMessage = "";
                            while(i<4)
                            {
                                c = new carta();
                                c.RandomCard();
                                if (c.Simbolo != "p4")
                                {
                                    carte.AddCard(c);
                                    Button b = new Button();
                                    this.Controls.Add(b);
                                    b.Image = Image.FromFile($"./CarteUNO/{c.ToString()}.png");
                                    b.Click += new System.EventHandler(CardSelected);
                                    b.TextAlign = ContentAlignment.MiddleRight;
                                    b.Visible = true;
                                    Deck.Add(b);
                                    MaxP4Number++;
                                    i++;
                                }
                                else
                                {
                                    int y = i;
                                    i = y;
                                }
                            }
                            Redraw();
                            Block(false);
                        }
                        else if (cardCodes[1] == "p2")
                        {
                            NeutralDeck.Image = Image.FromFile($"./CarteUNO/{cardCodes[0]}{cardCodes[1]}.png");
                            Player.CurrentMessage = "";
                            for (int i = 0; i < 2; i++)
                            {
                                c = new carta();
                                c.RandomCard();
                                carte.AddCard(c);
                                Button b = new Button();
                                this.Controls.Add(b);
                                b.Image = Image.FromFile($"./CarteUNO/{c.ToString()}.png");
                                b.Click += new System.EventHandler(CardSelected);
                                b.TextAlign = ContentAlignment.MiddleRight;
                                b.Visible = true;
                                Deck.Add(b);
                            }
                            Redraw();
                            Block(false);
                        }
                        else if (cardCodes[1]=="cc")
                        {
                            NeutralDeck.Image = Image.FromFile($"./CarteUNO/cccc.png");
                            Player.CurrentMessage = "";
                            Block(true);
                        }
                        else if(cardCodes[1] == "r" || cardCodes[1] == "s")
                        {
                            Block(false);
                        }
                        else
                        {
                            NeutralDeck.Image = Image.FromFile($"./CarteUNO/{cardCodes[0]}{cardCodes[1]}.png");
                            Player.CurrentMessage = "";
                            Block(true);
                        }
            
                        SetBoxColor(cardCodes[0]);
                    }
                }
                
                if(Player.CurrentMessage.Contains("0") && Player.CurrentMessage.Contains(this.Text)==false)//costrutto che viene azionato quando ha perso
                {
                    Player.CurrentMessage = "";
                    Player.Close();
                    ChangeCardTimer.Dispose();
                    Environment.Exit(1);

                }
            }

        }

        private void HiddenDeck_Click(object sender, EventArgs e)// funzione azionata quando si clicca sulla carta ribaltata
        {
            if (isCardCompatible(carte.getMazzo(), FieldCard) == false)
            {
                carta c = new carta();
                Button b = new Button();
                this.Controls.Add(b);
                c.RandomCard();
                carte.AddCard(c);
                b.Image = Image.FromFile($"./CarteUNO/{c.ToString()}.png");
                b.Click += new System.EventHandler(CardSelected);
                b.Visible = true;
                b.TextAlign = ContentAlignment.MiddleRight;
                Deck.Add(b);
                Redraw();
            }
        }
    }

}