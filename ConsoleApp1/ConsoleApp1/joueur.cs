using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Joueur
    {
        private int vies;

        public int Vies
        {
            get { return vies; }
            set { vies = value; }
        }

        public Joueur()
        {
            vies = 0;
        }

        public Joueur(List<Bateau> Bateaux)
        {
            this.vies = 0;
        }
    }
}
