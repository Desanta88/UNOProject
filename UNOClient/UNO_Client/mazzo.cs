using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO_Client
{
    public class mazzo
    {
        private List<carta> Carte;

        public mazzo()
        {
            Carte = new List<carta>();
        }

        public void DrawCards(int nc)
        {
            for (int i = 0; i < nc; i++)
            {
                carta c = new carta();
                c.RandomCard();
                Carte.Add(c);
            }
        }
        public int getLength()
        {
            return Carte.Count;
        }
        public List<carta> getMazzo()
        {
            return Carte;
        }
        public void EliminateCard(carta c)
        {
            Carte.Remove(c);
        }
        public void AddCard(carta c)
        {
            Carte.Add(c);
        }
    }
}
