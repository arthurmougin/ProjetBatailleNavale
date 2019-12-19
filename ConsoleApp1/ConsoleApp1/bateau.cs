using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Bateau
    {
        private int taille;

        public int Taille
        {
            get { return taille; }
            set { taille = value; }
        }

        /// <summary>
        /// Point à gauche du bateau
        /// </summary>
        private uint x;
        public uint X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Point en haut du bateau
        /// </summary>
        private uint y;
        public uint Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Axe du bateau
        /// </summary>
        private bool horizontal;
        public bool Horizontal
        {
            get { return horizontal; }
            set { horizontal = value; }
        }

        /// <summary>
        /// Nombre de vies d'un bateau. Initialisé à sa taille
        /// </summary>
        private int vies;
        public int Vies
        {
            get { return vies; }
            set { vies = value; }
        }

        public Bateau()
        {
            this.x = 0;
            this.y = 0;
            this.taille = 2;
            this.horizontal = true;
            this.vies = this.taille;
        }

        public Bateau(uint x, uint y, int taille, bool horizontal)
        {
            this.x = x;
            this.y = y;
            this.taille = taille;
            this.horizontal = horizontal;
            this.vies = taille;
        }

        public bool isAlive()
        {
            return vies > 0;
        }

        public override string ToString()
        {
            return "Bateau positionné en " + (x+1) + "-" + (char)(y + 96 +1)+ ", de longueur "+ taille + " et "+((horizontal)?"horizontal":"vertical");
        }
    }
}
