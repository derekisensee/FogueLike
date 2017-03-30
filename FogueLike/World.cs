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
        Player p;
        String[,] Map;
        List<String[,]> currentWorld;
        int worldNum;

        Random r = new Random();
        
        public World(int y)
        {
            Map = new String[y, 150];
            p = new Player(30, 30);
            currentWorld = new List<String[,]>();
            worldNum = 0;
            WorldGen();
            SpawnChars();
            currentWorld.Add(Map);
            PrintMap();
            
            Console.WriteLine("Press V to generate a new map. Press ESC to quit.");
            ConsoleKeyInfo c;
            String tempSpot = "."; // holds the place of the last thing we step on.

            do
            {
                c = Console.ReadKey();
                #region Movement Controls
                if (c.Key == ConsoleKey.UpArrow && (Map[p.position.Y - 1, p.position.X].Equals(".") || Map[p.position.Y - 1, p.position.X].Equals(">")))
                {
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write(tempSpot);
                    Map[p.position.Y, p.position.X] = tempSpot;
                    p.position.Y -= 1;
                    tempSpot = Map[p.position.Y, p.position.X];
                    PlacePlayer();
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.DownArrow && (Map[p.position.Y + 1, p.position.X].Equals(".") || Map[p.position.Y + 1, p.position.X].Equals(">")))
                {
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write(tempSpot);
                    Map[p.position.Y, p.position.X] = tempSpot;
                    p.position.Y += 1;
                    tempSpot = Map[p.position.Y, p.position.X];
                    PlacePlayer();
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.LeftArrow && (Map[p.position.Y, p.position.X - 1].Equals(".") || Map[p.position.Y, p.position.X - 1].Equals(">")))
                {
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write(tempSpot);
                    Map[p.position.Y, p.position.X] = tempSpot;
                    p.position.X -= 1;
                    tempSpot = Map[p.position.Y, p.position.X];
                    PlacePlayer();
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.RightArrow && (Map[p.position.Y, p.position.X + 1].Equals(".") || Map[p.position.Y, p.position.X + 1].Equals(">")))
                {
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write(tempSpot);
                    Map[p.position.Y, p.position.X] = tempSpot;
                    p.position.X += 1;
                    tempSpot = Map[p.position.Y, p.position.X];
                    PlacePlayer();
                    Console.SetCursorPosition(p.position.X, p.position.Y);
                    Console.Write("@");
                }
                Console.SetCursorPosition(0, 0);
                #endregion
                #region Stair Stuff
                if (c.Key == ConsoleKey.J && tempSpot.Equals(">"))
                {
                    worldNum++;
                    WorldGen();
                    SpawnChars();
                    currentWorld.Add(Map);
                    tempSpot = ".";
                    PrintMap();
                }
                #endregion

                if (c.Key == ConsoleKey.V)
                {
                    Console.Clear();
                    WorldGen();
                    SpawnChars();
                    PrintMap();
                    Console.WriteLine("Press V to generate a new map. Press ESC to quit.");
                }
            } while (c.Key != ConsoleKey.Escape);
        }

        public void SpawnChars()
        {
            // Place the player.
            Boolean playerPlaced = false;
            do
            {
                int XSpawn = r.Next(1, Map.GetLength(1) - 1);
                int YSpawn = r.Next(1, Map.GetLength(0) - 1);
                if (Map[YSpawn, XSpawn].Equals("."))
                {
                    Map[YSpawn, XSpawn] = "@";
                    p.position.X = XSpawn;
                    p.position.Y = YSpawn;
                    PlacePlayer();
                    if (Map[YSpawn + 1, XSpawn].Equals("."))
                    {
                        Map[YSpawn + 1, XSpawn] = "<";
                    }
                    else if (Map[YSpawn - 1, XSpawn].Equals("."))
                    {
                        Map[YSpawn - 1, XSpawn] = "<";
                    }
                    else if (Map[YSpawn, XSpawn + 1].Equals("."))
                    {
                        Map[YSpawn, XSpawn + 1] = "<";
                    }
                    else if (Map[YSpawn, XSpawn - 1].Equals("."))
                    {
                        Map[YSpawn, XSpawn - 1] = "<";
                    }
                    playerPlaced = true;
                }
            } while (playerPlaced == false);
        }

        public void PlacePlayer()
        {
            Map[p.position.Y, p.position.X] = "@";
        }

        #region World Generation Stuffs
        void WorldGen()
        {
            // Fill the entire map.
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
            HallGen(10);
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
            // /////
            Boolean stairPlaced = false;
            do
            {
                int XStair = r.Next(1, Map.GetLength(1) - 1);
                int YStair = r.Next(1, Map.GetLength(0) - 1);
                if (Map[YStair, XStair].Equals("."))
                {
                    Map[YStair, XStair] = ">";
                    stairPlaced = true;
                }
            } while (stairPlaced == false);
            
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
        #endregion // TODO: Improve this because it's trash.

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

        public void PrintMap()
        {
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
    }
}