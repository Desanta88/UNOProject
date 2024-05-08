using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO_Client
{
    public class carta
    {
        public string Simbolo { get; set; }
        public string Colore { get; set; }

        public carta(string c, string s)
        {
            Simbolo = s;
            Colore = c;
        }
        public carta()
        {
            Simbolo = "";
            Colore = "";
        }
        public override string ToString()
        {
            return this.Colore + this.Simbolo;
        }
        public void RandomCard()
        {
            string[] simboli = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "r", "s","p2"};
            string[] colori = new string[] { "y", "g", "b", "re", "p4", "cc" };
            Random r = new Random();
            int nSimbolo = r.Next(0, simboli.Length);
            int nColore = r.Next(0, colori.Length);
            if (colori[nColore] == "cc" || colori[nColore] == "p4")
            {
                this.Simbolo = colori[nColore];
                this.Colore= colori[nColore];
            }
            else
            {
                this.Simbolo = simboli[nSimbolo];
                this.Colore = colori[nColore];
            }
        }
    }
}
