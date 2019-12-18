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

        public GameMaster()
        {
            int mapSize = 5;
            mapSize = mapSize % 22 + 5;
            map1 = new Carte(mapSize);
            map2 = new Carte(mapSize);
            state = 0;
        }

        public GameMaster(int mapSize)
        {
            mapSize = mapSize % 22 + 5;
            map1 = new Carte(mapSize);
            map2 = new Carte(mapSize);
            state = 0;
        }

        private string drawMaps(int step)
        {

            string retour = "";
            bool j1playing = (step % 2 == 0) ?true:false;
            
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


        public void Jeu()
        {


            do
            {
                Console.WriteLine(@"Nouvelle partie!

appuyez sur entrer pour commencer.");
                Console.ReadLine();
                Console.Clear();
                while (state == 0)
                {

                    
                    //Console.WriteLine(drawMaps(2));
                    Console.WriteLine(drawMaps(2));
                    state++;
                }
               

                while (state == 0)
                {

                    Console.Clear();
                    Console.WriteLine(drawMaps(2));
                }
            } while (state != 2);
        }



    }
}
