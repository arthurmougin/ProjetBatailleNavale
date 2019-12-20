using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Carte
    {
        
        /// <summary>
        /// -:vide, H:bateau vivant, v:raté, X:touché, O:coulé
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
                    Matrice[i, j] = '-';
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
                    Matrice[i, j] = '-';
                }
            }
            Bateaux = new List<Bateau>();
        }

        /// <summary>
        /// Essaie d'ajouter le bateau mis en paramêtre. S'il y arrive, retourne Vrai, sinon Faux
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool AddBateau(Bateau b)
        {
            int matriceTaille = matrice.GetLength(0);


            if (b.Horizontal)
            {
                //Si le bateau ne sors pas de la map
                if (b.Y < 0 || b.Y + b.Taille-1 > (matriceTaille - 1))
                    return false;
                if (b.X < 0 || b.X > (matriceTaille - 1))
                    return false;

                //si son espace n'est pas occupé
                for (uint i = b.Y; i < (b.Y + b.Taille); i++)
                {
                    if (matrice[b.X, i] != '-')
                        return false;
                }

                //alors on le dessine
                for (uint i = b.Y; i < (b.Y + b.Taille); i++)
                {
                    matrice[b.X, i] = 'H';
                }

            }
            else
            {
                //Si le bateau ne sors pas de la map
                if (b.X < 0 || b.X + b.Taille-1 > (matriceTaille - 1))
                    return false;
                if (b.Y < 0 || b.Y > (matriceTaille - 1))
                    return false;

                //si son espace n'est pas occupé
                for (uint i = b.X; i < (b.X + b.Taille); i++)
                {
                    if (matrice[i, b.Y] != '-')
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

        /// <summary>
        /// Génère automatiquement un affichage complet de la carte en chaine de caractères
        /// </summary>
        /// <param name="caché"></param>
        /// <returns></returns>
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
                marker = i + 1;
                retour += (marker < 10)?(marker + "  "): marker.ToString()+" ";
                //contenu
                for (int j = 0; j < matriceTaille; j++)
                {
                    //Si on est caché et que la case est un bateau vierge, alors on affiche de l'eau
                    retour += (caché)? ((Matrice[i, j] == 'H') ? '-'+" ": Matrice[i, j] + " ") : Matrice[i, j]+" ";
                }
                retour += "\n";
            }
            retour += "\nBateaux:" + bateaux.Count+"\n";

            return retour;
        }

        /// <summary>
        /// Retourne une ligne specifique de toString sur demande
        /// </summary>
        /// <param name="line"></param>
        /// <param name="caché"></param>
        /// <returns></returns>
        public string GetLine(int line,bool caché)
        {
            string retour,input = this.ToString(caché);
            string[] array = input.Split('\n', StringSplitOptions.None);
            retour = (line < array.Length-1) ? array[line] : null;
            return retour;
            
        }

        /// <summary>
        /// Retourne une chaine de caractères décrivant les symboles de la matrice
        /// </summary>
        /// <returns></returns>
        public string GetDoc()
        {
            return "'-':vide, 'H':bateau, 'v':raté, 'X':touché, 'O':coulé\n\n\n";
        }

        /// <summary>
        /// Efface la carte et la liste de bateaux
        /// </summary>
        public void Reset()
        {
            bateaux.Clear();
            int taille = Matrice.GetLength(0);
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    Matrice[i, j] = '-';
                }
            }
        }

        /// <summary>
        /// Retourne le bateau présent aux coordonnées souhaitées
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Bateau GetBateauByCoords(int x, int y)
        {
            if(Matrice[x,y] == '-' || Matrice[x, y] == 'v')
                return null;

            //On teste pour chaque bateaux s'ils possèdent cette case
            for (int i = 0; i < bateaux.Count; i++)
            {
                if (bateaux[i].X == x && bateaux[i].Y == y) return bateaux[i];

                if (bateaux[i].Horizontal)
                {
                    if(bateaux[i].X == x && (x - bateaux[i].X <= bateaux[i].Taille)) return bateaux[i];
                }
                else
                {
                    if (bateaux[i].Y == y && (y - bateaux[i].Y <= bateaux[i].Taille)) return bateaux[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Retourne un booléen indiquant si cette carte n'a plus de bateaux en vie
        /// </summary>
        /// <returns></returns>
        public bool EstFinie()
        {
            int viesRestantes = 0;
            for (int i = 0; i < bateaux.Count; i++)
            {
                viesRestantes += bateaux[i].Vies;
            }
            return viesRestantes == 0;
        }

        /// <summary>
        /// Fonction utilisée en interne pour remplacer les bateaux coulés par le marqueur adapté
        /// </summary>
        /// <param name="b"></param>
        private void BateauCoulé(Bateau b)
        {
            if (b.Horizontal)
            {
                //alors on le dessine
                for (uint i = b.Y; i < (b.Y + b.Taille); i++)
                {
                    matrice[b.X, i] = 'O';
                }

            }
            else
            {
                //alors on le dessine
                for (uint i = b.X; i < (b.X + b.Taille); i++)
                {
                    matrice[i, b.Y] = 'O';
                }
            }
        }

        /// <summary>
        /// Gère le tir sur la matrice et retourne un indicateur de ce qui s'est passé
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Tirer(int x,int y)
        {
            int retour = 0;//0:raté, 1:déjàtiré, 2:touché, 3:couler
            char maCase = matrice[x, y];
            if(maCase == '-')//tir dans l'eau
            {
                retour = 0;
                matrice[x, y] = 'v';
            }
            else if (maCase == 'v' || maCase == 'X' || maCase == 'O')//tir déjà effectué
            {
                retour = 1;
            }
            if (maCase == 'H')//tir sur bateau
            {
                retour = 2;
                matrice[x, y] = 'X';

                Bateau b = GetBateauByCoords(x, y);
                b.Vies--;
                if(b.Vies == 0)
                {
                    retour = 3;
                    BateauCoulé(b);
                }
            }

            return retour;
        }
    }
}
