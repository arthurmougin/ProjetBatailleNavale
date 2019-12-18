using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Carte
    {
        
        /// <summary>
        /// w:vide, H:bateau vivant, v:raté, X:touché, O:coulé
        /// </summary>
        private char[,] matrice;
        public char[,] Matrice
        {
            get { return matrice; }
            set { matrice = value; }
        }

        private List<Bateau> bateaux;
        public List<Bateau> Bateaux
        {
            get { return bateaux; }
            set { bateaux = value; }
        }

        public Carte()
        {
            Matrice = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Matrice[i, j] = 'w';
                }
            }
            Bateaux = new List<Bateau>();
        }

        public Carte(int taille)
        {
            Matrice = new char[taille, taille];
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    Matrice[i, j] = 'w';
                }
            }
            Bateaux = new List<Bateau>();
        }

        public bool AddBateau(Bateau b)
        {
            int matriceTaille = matrice.GetLength(0);

           
            if (b.Horizontal)
            {
                //Si le bateau ne sors pas de la map
                if (b.X + b.Taille > (matriceTaille - 1))
                    return false;

                //si son espace n'est pas occupé
                for (uint i = b.X; i < (b.X + b.Taille); i++)
                {
                    if(matrice[i,b.Y] != 'w')
                        return false;
                }

                //alors on le dessine
                for (uint i = b.X; i < (b.X + b.Taille); i++)
                {
                    matrice[i, b.Y] = 'H';
                }
            }
            else
            {
                //Si le bateau ne sors pas de la map
                if (b.Y + b.Taille > (matriceTaille - 1))
                    return false;

                //si son espace n'est pas occupé
                for (uint i = b.Y; i < (b.Y + b.Taille); i++)
                {
                    if (matrice[b.X, i] != 'w')
                        return false;
                }

                //alors on le dessine
                for (uint i = b.X; i < (b.X + b.Taille); i++)
                {
                    matrice[i, b.Y] = 'H';
                }
            }

            bateaux.Add(b);
            return true;
        }

        public string ToString(bool caché)
        {
            int matriceTaille = matrice.GetLength(0),marker;
            string retour = (caché)?"the enemy":"your map!";

            //titre
            for (int i = 0; i < (matriceTaille - 3); i++)
            {
                retour = "-" + retour + "-";
            }
            retour += "\n   ";

            //Ligne de repère haut
            for (int i = 0; i < matriceTaille; i++)
            {
                retour += ((char)(i+97) +" ");
            }
            retour += "\n";

            //matrice
            for (int i = 0; i < matriceTaille; i++)
            {
                //repère gauche
                marker = matriceTaille - i;
                retour += (marker < 10)?(marker + "  "): marker.ToString()+" ";
                //contenu
                for (int j = 0; j < matriceTaille; j++)
                {
                    //Si on est caché et que la case est un bateau vierge, alors on affiche de l'eau
                    retour += (caché)? ((Matrice[i, j] == 'H') ? "w ": Matrice[i, j] + " ") : Matrice[i, j]+" ";
                }
                retour += "\n";
            }


            return retour;
        }

        public string GetLine(int line,bool caché)
        {
            string retour,input = this.ToString(caché);
            string[] array = input.Split('\n', StringSplitOptions.None);
            retour = (line < array.Length-1) ? array[line] : null;
            return retour;
            
        }

        
    }
}
