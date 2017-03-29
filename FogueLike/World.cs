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
        Random r = new Random();

        public World()
        {
            Map = new String[80, 150];
            Player p = new Player();

            WorldGen();
            Console.WriteLine("Press V to generate a new map. Press ESC to quit.");
            ConsoleKeyInfo c;

            do
            {
                c = Console.ReadKey();
                // movement controllers
                if (c.Key == ConsoleKey.UpArrow)
                {

                }

                if (c.Key == ConsoleKey.V)
                {
                    Console.Clear();
                    WorldGen();
                    Console.WriteLine("Press V to generate a new map. Press ESC to quit.");
                }
            } while (c.Key != ConsoleKey.Escape);
        }

        void WorldGen()
        {
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
            for (int i = 0; i < 15; i++)
            {
                PlaceObject(RoomGen(), r.Next(1, 148), r.Next(1, 70));
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
            HallGen(10);
            #endregion
            // prints board
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        void HallGen(int halls)
        {
            while (halls-- > 0)
            {
                int XStart = r.Next(1, Map.GetLength(1));
                int YStart = r.Next(1, Map.GetLength(0));
                int XEnd = r.Next(1, Map.GetLength(1));
                int YEnd = r.Next(1, Map.GetLength(0));

                Map[YStart, XStart] = ".";
                Map[YEnd, XEnd] = ".";

                int XDif; int YDif;
                if (XStart > XEnd)
                {
                    XDif = XStart - XEnd;
                    for (; XDif > 0; XDif--)
                    {
                        Map[YStart, XStart--] = ".";
                    }
                }
                else
                {
                    XDif = XEnd - XStart;
                    for (; XDif > 0; XDif--)
                    {
                        Map[YStart, XStart++] = ".";
                    }
                }
                if (YStart > YEnd)
                {
                    YDif = YStart - YEnd;
                    for (; YDif > 0; YDif--)
                    {
                        Map[YStart--, XEnd] = ".";
                    }
                }
                else
                {
                    YDif = YEnd - YStart;
                    for (; YDif > 0; YDif--)
                    {
                        Map[YStart++, XEnd] = ".";
                    }
                } 
            }
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
                    if ((y < Map.GetLength(0) && x < Map.GetLength(1)) && (i < structure.GetLength(0) && j < structure.GetLength(1)))
                    {
                        Map[y, x++] = structure[i, j];
                    }
                    else
                    {
                        break;
                    }
                }
                x = startX;
                y++;
            }
        }
    }
}