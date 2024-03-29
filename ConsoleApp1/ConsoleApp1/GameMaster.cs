﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class GameMaster
    {

        private Carte map1;
        public Carte Map1
        {
            get { return map1; }
            set { map1 = value; }
        }

        private Carte map2;
        public Carte Map2
        {
            get { return map2; }
            set { map2 = value; }
        }

        /// <summary>
        /// 0:Setup
        /// 1:Playing
        /// 2:fin
        /// </summary>
        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private int tour;

        public int Tour
        {
            get { return tour; }
            set { tour = value; }
        }

        public GameMaster()
        {
            int mapSize = 5;
            mapSize = mapSize % 22 + 5;
            map1 = new Carte(mapSize);
            map2 = new Carte(mapSize);
            state = 0;
            tour = 0;
        }

        public GameMaster(int mapSize)
        {
            mapSize = mapSize % 22 + 5;
            map1 = new Carte(mapSize);
            map2 = new Carte(mapSize);
            state = 0;
            tour = 0;

        }

        /// <summary>
        /// Dessin de la partie supperieur de la fenetre
        /// </summary>
        /// <returns></returns>
        private string DrawMaps()
        {
            //Les inputs
            string retour = Map1.GetDoc();
            bool j1playing = (tour % 2 == 0) ?true:false;
            
            uint width = ((uint)Map1.Matrice.GetLength(0)*2 + 3);
            string LineM1 = "", LineM2 = "";
            int i = 0;

            LineM1 = map1.GetLine(i, j1playing);
            LineM2 = map2.GetLine(i, !j1playing);
            do
            {
                //debut du tour

                //On modifie les tailles 
                LineM1 = (LineM1 == null)?"" :LineM1;
                if (LineM1.Length > width)
                    LineM1.Substring(0, (int)width);
                else
                {
                    while(LineM1.Length < width)
                    {
                        LineM1 += " ";
                    }
                }

                LineM2 = (LineM2 == null) ? "" : LineM2;
                if (LineM2.Length > width)
                    LineM2.Substring(0, (int)width);
                else
                {
                    while (LineM2.Length < width)
                    {
                        LineM2 += " ";
                    }
                }

                retour += LineM1 + " |  " + LineM2 + "\n";
                //Prochain tour
                i++;
                LineM1 = map1.GetLine(i, j1playing);
                LineM2 = map2.GetLine(i, !j1playing);
            } while (LineM1 != null && LineM2 != null);

            return retour;
        }

        /// <summary>
        /// Le catalogue d'inputs en fonction des besoins
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string PrintInputs(int state)
        {
            string retour ="";
            switch (state)
            {
                case 0:
                    retour = "Mise en place :\n1: Ajouter un bateau\n2: Effacer\n3: Valider";
                    break;
                case 1:
                    retour = "Jeu :\n1: Tirer\n2: Passer son tour";
                    break;
                case 2:
                    retour = "Fin de partie :\n1: Recommencer\n2: Quitter";
                    break;
                case 3:
                    retour = "Initial Setup :\nEntrez la largeur de la carte (la longueur sera identique)\nLa valeur doit etre comprise entre 5 et 26";
                    break;
                default:
                    break;
            }


            return retour;
        }

        /// <summary>
        /// Retourne la map joueur
        /// </summary>
        /// <returns></returns>
        private Carte GetPlayerMap()
        {
            return (tour % 2 == 0) ? map2 : map1;
        }
       
        /// <summary>
        /// Retourne la carte enemy
        /// </summary>
        /// <returns></returns>
        private Carte GetEnemyMap()
        {
            return (tour % 2 == 0) ? map1 : map2;
        }
        
        /// <summary>
        /// Fonction qui permet de demander à l'utilisateur de rentrer quelque chose après lui avoir affiché un message.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string AskUser(string content)
        {
            Console.WriteLine(content);
            return Console.ReadLine();
        }
        
        /// <summary>
        /// Validations des coordonnées rentrées par l'utilisateur
        /// </summary>
        /// <param name="input"></param>
        /// <param name="carte"></param>
        /// <returns></returns>
        private bool TestCoords(string input, Carte m)
        {
            int x = 0,y = 0;
            string[] array = input.Split('-', StringSplitOptions.None);
            if (array.Length >= 2)
            {
                //test de la première valeur
                if (Int32.TryParse(array[0], out x))
                {

                    if (x > 0 && x <= m.Matrice.GetLength(0))
                    {
                        //test de la seconde valeur
                        if (array[1].Length > 0)
                        {
                            y = ((int)array[1].ToCharArray()[0]) - 97;
                            if (y >= 0 && y < m.Matrice.GetLength(0))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Efface la fenetre et réaffiche la carte
        /// </summary>
        public void resetView()
        {
            Console.Clear();
            Console.WriteLine(DrawMaps());
        }

        /// <summary>
        /// Structuration du code :
        /// Cette fonction gère l'ajout des bateaux dans une carte
        /// </summary>
        /// <param name="b"></param>
        /// <param name="m"></param>
        public void Setup(Bateau b,Carte m)
        {
            bool ValideBoat = false, valideInput = false, exit = false;
            string input = "";
            string[] array;
            int x = 0, y = 0;
            do
            {
                resetView();
                Console.WriteLine(@"Création d'un bateau:");
                

                //coordonnées
                do
                {
                    valideInput = false;
                    input = AskUser("Quelles sont les coordonnées de son point haut-Gauche?\n(format: '1-a')");
                   
                    if (TestCoords(input, m))
                    {
                        array = input.Split('-', StringSplitOptions.None);
                        Int32.TryParse(array[0], out x);
                        y = ((int)array[1].ToCharArray()[0]) - 96;
                        b.X = ((uint)x)-1;
                        b.Y = ((uint)y)-1;
                        valideInput = true;
                    }
                    
                    if (valideInput == false)
                    {
                        resetView();

                        Console.WriteLine("Valeur invalide (" + x + "-"  + y + "), recommencez.");
                    }
                        
                } while (!valideInput);


                //Longueur
                resetView();
                Console.WriteLine(@"Création d'un bateau:");
                do
                {
                    valideInput = false;
                    input = AskUser("Quelle est sa longueur?\n(valeur de 2 à 5)");
                    int length;
                    if (Int32.TryParse(input, out length))
                    {
                        if (length > 1 && length < 6)
                        {
                            valideInput = true;
                            b.Vies = b.Taille = length;
                        }

                    }

                    if (valideInput == false)
                    {
                        resetView();
                        Console.WriteLine("Valeur invalide, recommencez.");
                    }
                } while (!valideInput);


                //axe
                resetView();
                Console.WriteLine(@"Création d'un bateau:");
                do
                {
                    valideInput = false;
                    input = AskUser(@"Est-il horizontal?\n(format: o (oui) ou n (non))");
                    if (input == "o" || input == "n")
                    {
                        valideInput = true;
                        b.Horizontal = (input == "o") ? true : false;
                    }


                    if (valideInput == false)
                    {
                        resetView();
                        Console.WriteLine("Valeur invalide, recommencez.");
                    }
                } while (!valideInput);

                resetView();
                Console.WriteLine(@"Création d'un bateau:");

                ValideBoat = m.AddBateau(b);
                if (ValideBoat) exit = true;
                else
                {
                    do
                    {
                        valideInput = false;
                        input = AskUser("Le bateau ne peut être inséré au plateau avec ces paramêtres, voulez vous réessayer?\n(format: o(oui) ou n(non))");
                        if (input == "o" || input == "n")
                        {
                            valideInput = true;
                            exit = (input == "n") ? true : false;

                            if (!exit)
                                Console.WriteLine("\nOn recommence.");
                        }


                        if (valideInput == false)
                            Console.WriteLine(@"Valeur invalide, recommencez.");
                    } while (!valideInput);
                }

            } while (!exit);
        }


        /// <summary>
        /// Structuration du code :
        /// Cette fonction gère le tir et retourne le nombre de tir restants
        /// </summary>
        /// <returns></returns>
        public int Tirer()
        {
            bool ValideFire = false, valideInput = false, exit = false;
            string input = "";
            int retour = 0;
            string[] array;
            int x = 0, y = 0,resultat = 0;
            do
            {
                resetView();
                Console.WriteLine(@"Tir:");

                //GetInput
                do
                {
                    valideInput = false;
                    input = AskUser("Quelles sont les coordonnées de votre tir?\n(format: '1-a')");

                    if (TestCoords(input, GetEnemyMap()))
                    {
                        array = input.Split('-', StringSplitOptions.None);

                        Int32.TryParse(array[0], out x);
                        y = ((int)array[1].ToCharArray()[0]) - 96;
                        x--;
                        y--;
                        valideInput = true;
                    }
                    
                    if (valideInput == false)
                    {
                        AskUser("Valeur invalide, recommencez.");
                    }
                } while (!valideInput);

                resultat = GetEnemyMap().Tirer(x, y);
                exit = true;
                switch (resultat)
                {
                    case 0:
                        AskUser("Plouf dans l'eau");
                        break;
                    case 1:
                        AskUser("Déjà tiré ici, recommenez.");
                        exit = false;
                        break;
                    case 2:
                        AskUser("Touché");
                        retour = 1;
                        break;
                    case 3:
                        AskUser("Couler");
                        retour = (GetEnemyMap().EstFinie())?0:1;
                        break;
                    default:
                        break;
                }

            } while (!exit);
            return retour;
        }

        /// <summary>
        /// Annonce de debut de jeu
        /// </summary>
        public void JeuCommence()
        {
            Console.Clear();
            AskUser("\n\n\nLes jeux commencent !");
        }

        /// <summary>
        /// Annonce de fin de jeu
        /// </summary>
        public void Victoire()
        {
            Console.Clear();
            AskUser("\n\n\nLe joueur " + (map1.EstFinie()?"2":"1") + " a gagné!");
        }

        /// <summary>
        /// Annonce de changement de joueur et incrément du tour
        /// </summary>
        private void NextTurn()
        {
            Console.Clear();
            if (tour == 0)
                Console.WriteLine(@"Nouvelle partie!");
            tour += 1;
            int player = (tour + 1) % 2 + 1;

            AskUser("\n\n\nJoueur " + player + ", à votre tour.\nAppuyez sur entrer pour continuer.");
            Console.Clear();
        }

        /// <summary>
        /// Boucle principale de jeu
        /// </summary>
        public void Jeu()
        {

            /**/
            
            string input = "";
            bool valideInput = false;
            int setupcount, tirRestant;
            


            do //GameLoop
            {
                //Choix de taille
                do
                {
                    int size;
                    valideInput = false;
                    input = AskUser(PrintInputs(3));
                    if(Int32.TryParse(input, out size))
                    {
                        valideInput = true;
                        size -= 5;
                        if (size < 0) size = 0;
                        int mapSize = size % 22 + 5;
                        map1 = new Carte(mapSize);
                        map2 = new Carte(mapSize);
                    }
                } while (valideInput == false);

                setupcount = 0;

                //setupLoop
                do 
                {
                    NextTurn();
                    do // Bateau loop
                    {
                        resetView();
                        valideInput = false;
                        input = AskUser(PrintInputs(0));
                        switch (input)
                        {
                            case "1"://ajouter un bateau
                                Bateau b = new Bateau();
                                Setup(b, GetPlayerMap());
                                valideInput = true;
                                break;
                            case "2"://effacer
                                GetPlayerMap().Reset();
                                valideInput = true;
                                break;
                            case "3"://valider
                                valideInput = true;
                                break;
                            default:
                                break;
                        }

                        if (valideInput == false)
                        {
                            AskUser("Input inconnue, recommencez");
                        }
                            
                        if(input == "3" && GetPlayerMap().Bateaux.Count == 0)
                        {
                            valideInput = false;
                            AskUser("Vous avez besoin d'au moins 1 bateau pour valider votre setup.\nAppuyez nimporte ou pour continuer.");
                        }


                    } while (input != "3" || valideInput == false);
                    setupcount++;
                } while (setupcount < 2);
                state++;


                JeuCommence();

                //Main Action loop
                do
                {
                    NextTurn();
                    tirRestant = 1;
                    do // action Loop
                    {
                        
                        resetView();
                        valideInput = false;
                        input = AskUser(PrintInputs(1));
                        tirRestant--;
                        switch (input)
                        {
                            case "1"://tirer
                                tirRestant = Tirer();
                                valideInput = true;
                                break;
                            case "2"://passer son tour
                                valideInput = true;
                                break;
                            default:
                                break;
                        }

                        if (valideInput == false)
                            AskUser("Input inconnue, recommencez");

                        //Si on a encore des tirs, on peut recommencer
                    } while (tirRestant > 0 || valideInput == false);
                    //Ceci chacun son tour jusqu'à ce que l'une des deux cartes soit vide
                } while (!Map1.EstFinie() && !Map2.EstFinie());

                Victoire();

                // Exit Loop
                do
                {
                    resetView();
                    valideInput = false;
                    input = AskUser(PrintInputs(2));
                    switch (input)
                    {
                        case "1":
                            state = 0;
                            valideInput = true;
                            break;
                        case "2":
                            state = 2;
                            valideInput = true;
                            break;
                        default:
                            break;
                    }

                    if (valideInput == false)
                        AskUser("Input inconnue, recommencez");
                } while ( valideInput == false);

            } while (state != 2);
            
            /**/
        }

    }
}
