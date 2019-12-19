using System;
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

        private string DrawMaps()
        {

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

        private void NextTurn()
        {
            Console.Clear();
            if (tour == 0)
                Console.WriteLine(@"Nouvelle partie!");
            tour += 1;
            int player = (tour + 1)%2+1;
          
            AskUser("\n\n\nJoueur " + player + ", à votre tour.\nAppuyez sur entrer pour continuer.");
            Console.Clear();
        }

        private string PrintInputs(int state)
        {
            string retour ="";
            if(state == 0)
            {
                retour = @"Mise en place :
1: Ajouter un bateau
2: Effacer
3: Valider";
            }
            else if (state == 1)
            {
                retour = @"Jeu :
1: Tirer
2: Passer son tour";
            }
            else if (state == 2)
            {
                retour = @"Fin de partie :
1: Recommencer
2: Quitter";
            }
            return retour;
        }

        private Carte GetMapByTurn()
        {
            return (tour % 2 == 0) ? map2 : map1;
        }

        private string AskUser(string content)
        {
            Console.WriteLine(content);
            return Console.ReadLine();
        }
        
        public void SetupBateau(Bateau b,Carte m)
        {
            bool ValideBoat = false, valideInput = false, exit = false;
            string input = "";
            do
            {
                resetView();
                Console.WriteLine(@"Création d'un bateau:");


                //coordonnées
                do
                {
                    valideInput = false;
                    input = AskUser("Quelles sont les coordonnées de son point haut-Gauche?\n(format: '1-a')");
                    string[] array = input.Split('-', StringSplitOptions.None);
                    int x = 0,y = 0;


                    //test de la première valeur
                    if (Int32.TryParse(array[0], out x))
                    {
                        x--;

                        if (x < m.Matrice.GetLength(0))
                        {
                            //test de la seconde valeur
                            y = ((int)array[1].ToCharArray()[0]) - 97;
                            if (y >= 0 && y < m.Matrice.GetLength(0))
                            {
                                valideInput = true;
                                b.X = (uint)x;
                                b.Y = (uint)y;
                            }
                        }
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

        public void resetView()
        {
            Console.Clear();
            Console.WriteLine(DrawMaps());
        }

        public void Jeu()
        {

            /**/
            
            string input = "";
            bool valideInput = false;

            

            do //GameLoop
            {
                int setupcount = 0;
                do //setupLoop
                {
                    NextTurn();
                    do // Bateau loop
                    {
                        resetView();
                        valideInput = false;
                        input = AskUser(PrintInputs(0));
                        if (input == "1")//ajouter un bateau
                        {
                            Bateau b = new Bateau();
                            SetupBateau(b,GetMapByTurn());
                            valideInput = true;
                        }
                        else if (input == "2")//effacer
                        {
                            GetMapByTurn().Reset();
                            valideInput = true;
                        }
                        else if (input == "3")//valider
                        {
                            valideInput = true;
                        }

                        if (valideInput == false)
                        {
                            
                            AskUser("Input inconnue, recommencez");
                        }
                            
                       
                        if(input == "3" && GetMapByTurn().Bateaux.Count == 0)
                        {
                            valideInput = false;
                            AskUser("Vous avez besoin d'au moins 1 bateau pour valider votre setup.\nAppuyez nimporte ou pour continuer.");
                        }


                    } while (input != "3" || valideInput == false);
                    setupcount++;
                } while (setupcount < 2);
                state++;

                do //Main Action loop
                {
                    NextTurn();
                    do // action Loop
                    {
                        resetView();
                        valideInput = false;
                        input = AskUser(PrintInputs(1));


                    } while (input != "3" || valideInput == false);

                } while (!Map1.EstFinie() && !Map2.EstFinie());



                do // Exit Loop
                {
                    resetView();
                    valideInput = false;
                    input = AskUser(PrintInputs(2));
                    if (input == "1")//recommencer
                    {
                        state = 0;
                        valideInput = true;

                    }
                    else if (input == "2")//finir
                    {
                        state = 2;
                        valideInput = true;
                    }

                    if (valideInput == false)
                        Console.WriteLine("Input inconnue, recommencez");
                } while ( valideInput == false);

            } while (state != 2);
            



            /**/
        }

    }
}
