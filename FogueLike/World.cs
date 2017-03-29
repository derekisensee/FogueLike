using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class World
    {
        String[,] Map;
        List<Point> UsedPoints;
        struct Point
        {
            private int x;
            private int y;
            public int X
            {
                get
                {
                    return x;
                }
                set
                {
                    x = value;
                }
            }
            public int Y
            {
                get
                {
                    return y;
                }
                set
                {
                    y = value;
                }
            }
        }
        Random r = new Random();

        public World()
        {
            Map = new String[100, 100];
            #region "World" Generation
            // fill entire map
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = " ";
                }
            }

            // random rooms..
            for (int i = 0; i < 10; i++)
            {
                PlaceObject(RoomGen(), r.Next(0, 100), r.Next(0, 100));
            }

            // bounds of whole map
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                Map[0, i] = "X";
                Map[Map.GetLength(0) - 1, i] = "X";
            }

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                Map[i, 0] = "X";
                Map[i, Map.GetLength(1) - 1] = "X";
            }
            #endregion
            HallGen();
            // print board...
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        void HallGen()
        {
            int XDifference = 0;
            int YDifference = 0;
            int XStart = 0;
            int YStart = 0;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[i, j].Equals("-")) // if we find a upper/lower wall, search any other type of wall
                    {
                        for (int a = i + 1; a < Map.GetLength(0); a++)
                        {
                            for (int b = j + 1; b < Map.GetLength(1); b++)
                            {
                                if (Map[a, j].Equals("-"))
                                {
                                    XDifference = a - i;
                                    YDifference = b - j;
                                    XStart = a;
                                    YStart = b;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < XDifference; i++)
            {
                for (int j = 0; j < YDifference; j++)
                {

                }
            }
            Console.WriteLine(XDifference + " " + YDifference + "\n" + XStart + " " + YStart);
        }

        String[,] RoomGen()
        {
            int YSize = r.Next(3, 30); int XSize = r.Next(3, 30);
            String[,] Room = new String[YSize, XSize];

            // fill room with stuff
            for (int i = 0; i < Room.GetLength(0); i++)
            {
                for (int j = 0; j < Room.GetLength(1); j++)
                {
                    Room[i, j] = ".";
                }
            }

            // generate bounds of the room
            for (int i = 0; i < Room.GetLength(1); i++)
            {
                Room[0, i] = "-";
                Room[Room.GetLength(0) - 1, i] = "-";
            }

            for (int i = 0; i < Room.GetLength(0); i++)
            {
                Room[i, 0] = "|";
                Room[i, Room.GetLength(1) - 1] = "|";
            }
            return Room;
        }

        public void PlaceObject(String[,] structure, int x, int y)
        {
            int startX = x;
            int startY = y;
            for (int i = 0; i < structure.GetLength(0); i++)
            {
                for (int j = 0; j < structure.GetLength(1); j++)
                {
                    if (y < Map.GetLength(1) && x < Map.GetLength(0))
                    {
                        Map[y, x++] = structure[i, j];
                    }
                }
                x = startX;
                y++;
            }
        }
    }
}