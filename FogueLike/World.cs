using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace FogueLike
{
    public class World
    {
        Player p;
        public String[,] Map; // Current map.
        List<String[,]> currentWorld; // List of maps.
        List<String> symbols; // This is for things we can attack.
        List<String> passable; // This is for things we can walk over.
        Dictionary<String, Entity> entities;
        int worldNum;
        int entID;
        Random r = new Random();

        public List<string> Passable { get => passable; set => passable = value; }
        public List<string> Symbols { get => symbols; set => symbols = value; }
        public List<string[,]> CurrentWorld { get => currentWorld; set => currentWorld = value; }
        public Dictionary<string, Entity> Entities { get => entities; set => entities = value; }
        //public string[,] Map { get => map; set => map = value; }
        public Player P { get => p; set => p = value; }
        public int WorldNum { get => worldNum; set => worldNum = value; }

        public World(int y)
        {
            Map = new String[y, 150];
            p = new Player();
            currentWorld = new List<String[,]>();
            entities = new Dictionary<string, Entity>();
            worldNum = 0;
            entID = 0;

            passable = new List<String>();
            passable.Add("."); passable.Add("x");
            symbols = new List<String>();
            symbols.Add("g");

            int floors = 5;
            while (floors-- > 0) {
                CurrentWorld.Add(WorldGen(y));
            }
            Map = CurrentWorld[worldNum];
            SpawnPlayer();
            PrintMap();

            Console.SetCursorPosition(0, Map.GetLength(0) + 1);
            Console.WriteLine("Press J to travel up/down stairs. Press ESC to quit.");
            ConsoleKeyInfo c;
            String tempSpot = "."; // holds the place of the last thing we step on.

            // This loop is where the gameplay takes place.
            do
            {
                c = Console.ReadKey();
                #region Movement Controls
                if (c.Key == ConsoleKey.UpArrow && (passable.Contains(Map[p.position.Y - 1, p.position.X]) || Map[p.position.Y - 1, p.position.X].Equals(">") || Map[p.position.Y - 1, p.position.X].Equals("<")))
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
                if (c.Key == ConsoleKey.DownArrow && (passable.Contains(Map[p.position.Y + 1, p.position.X]) || Map[p.position.Y + 1, p.position.X].Equals(">") || Map[p.position.Y + 1, p.position.X].Equals("<")))
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
                if (c.Key == ConsoleKey.LeftArrow && (passable.Contains(Map[p.position.Y, p.position.X - 1]) || Map[p.position.Y, p.position.X - 1].Equals(">") || Map[p.position.Y, p.position.X - 1].Equals("<")))
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
                if (c.Key == ConsoleKey.RightArrow && (passable.Contains(Map[p.position.Y, p.position.X + 1]) || Map[p.position.Y, p.position.X + 1].Equals(">") || Map[p.position.Y, p.position.X + 1].Equals("<")))
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
                #region Attack Stuff
                if (c.Key == ConsoleKey.RightArrow && symbols.Contains(Map[p.position.Y, p.position.X + 1]))
                {
                    foreach (Entity s in entities.Values)
                    {
                        if (s.pos.X == p.position.X + 1 && s.pos.Y == p.position.Y)
                        {
                            s.decHP(p.equipped[0]);
                            if (!(s.Symbol.Equals("x")))
                            {
                                s.Attack(p);
                            }
                            else
                            {
                                Map[p.position.Y, p.position.X + 1] = s.Symbol;
                                Console.SetCursorPosition(p.position.X + 1, p.position.Y);
                                Console.Write(Map[p.position.Y, p.position.X + 1]);
                            }
                        }
                    }
                }
                if (c.Key == ConsoleKey.LeftArrow && symbols.Contains(Map[p.position.Y, p.position.X - 1]))
                {
                    foreach (Entity s in entities.Values)
                    {
                        if (s.pos.X == p.position.X - 1 && s.pos.Y == p.position.Y)
                        {
                            s.decHP(p.equipped[0]);
                            if (!(s.Symbol.Equals("x")))
                            {
                                s.Attack(p);
                            }
                            else
                            {
                                Map[p.position.Y, p.position.X - 1] = s.Symbol;
                                Console.SetCursorPosition(p.position.X - 1, p.position.Y);
                                Console.Write(Map[p.position.Y, p.position.X - 1]);
                            }
                        }
                    }
                }
                if (c.Key == ConsoleKey.DownArrow && symbols.Contains(Map[p.position.Y + 1, p.position.X]))
                {
                    foreach (Entity s in entities.Values)
                    {
                        if (s.pos.X == p.position.X && s.pos.Y == p.position.Y + 1)
                        {
                            s.decHP(p.equipped[0]);
                            if (!(s.Symbol.Equals("x")))
                            {
                                s.Attack(p);
                            }
                            else
                            {
                                Map[p.position.Y + 1, p.position.X] = s.Symbol;
                                Console.SetCursorPosition(p.position.X, p.position.Y + 1);
                                Console.Write(Map[p.position.Y + 1, p.position.X]);
                            }
                        }
                    }
                }
                if (c.Key == ConsoleKey.UpArrow && symbols.Contains(Map[p.position.Y - 1, p.position.X]))
                {
                    foreach (Entity s in entities.Values)
                    {
                        if (s.pos.X == p.position.X && s.pos.Y == p.position.Y - 1)
                        {
                            s.decHP(p.equipped[0]);
                            if (!(s.Symbol.Equals("x")))
                            {
                                s.Attack(p);
                            }
                            else
                            {
                                Map[p.position.Y - 1, p.position.X] = s.Symbol;
                                Console.SetCursorPosition(p.position.X, p.position.Y - 1);
                                Console.Write(Map[p.position.Y - 1, p.position.X]);
                            }
                        }
                    }
                }
                #endregion
                #region Stair Stuff
                // moving down a floor.
                if (c.Key == ConsoleKey.J && tempSpot.Equals(">"))
                {
                    Console.Clear();
                    // This prevents the stairs from becoming the player if we return to this floor.
                    Map[p.position.Y, p.position.X] = ">";
                    currentWorld[worldNum] = Map;

                    // Load the next map
                    Map = currentWorld[++worldNum];
                    Map[p.upStairPositions[worldNum].Y, p.upStairPositions[worldNum].X] = "@";
                    p.position.X = p.upStairPositions[worldNum].X; p.position.Y = p.upStairPositions[worldNum].Y;
                    tempSpot = ".";
                    PrintMap();
                }
                // moving up a floor.
                if (c.Key == ConsoleKey.J && tempSpot.Equals("<"))
                {
                    Console.Clear();
                    Map[p.position.Y, p.position.X] = "<";

                    Map = currentWorld[--worldNum];
                    Map[p.downStairPositions[worldNum].Y, p.downStairPositions[worldNum].X] = "@";
                    p.position.X = p.downStairPositions[worldNum].X; p.position.Y = p.downStairPositions[worldNum].Y;
                    tempSpot = ".";
                    PrintMap();
                }
                #endregion

                foreach (Entity e in entities.Values)
                {
                    e.Decide(p, Map);
                }

                Console.SetCursorPosition(0, Map.GetLength(0));
                Console.Write("HP:" + p.GetCurrentHP() + "/" + p.GetMaxHP());
            } while (c.Key != ConsoleKey.Escape);
        }

        public void SpawnPlayer() // TODO: Split this up, have seperate method that initally places player, and make this one the one that spawns entities.
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
                    playerPlaced = true;
                }
            } while (playerPlaced == false);
        }

        public void PlacePlayer()
        {
            Map[p.position.Y, p.position.X] = "@";
        }

        #region World Generation Stuffs
        String[,] WorldGen(int y)
        {
            String[,] tempMap = new String[y, 150];
            // Fill the entire map.
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    tempMap[i, j] = " ";
                }
            }

            // random rooms..
            for (int i = 0; i < 15; i++)
            {
                PlaceObject(tempMap, RoomGen(), r.Next(1, 148), r.Next(1, 70));
            }
            tempMap = HallGen(tempMap, 10);
            // bounds of whole map
            for (int i = 0; i < tempMap.GetLength(1); i++)
            {
                tempMap[0, i] = "X";
                tempMap[tempMap.GetLength(0) - 1, i] = "X";
            }

            for (int i = 0; i < tempMap.GetLength(0); i++)
            {
                tempMap[i, 0] = "X";
                tempMap[i, Map.GetLength(1) - 1] = "X";
            }
            ///////
            Boolean downStairPlaced = false;
            Boolean upStairPlaced = false;
            do
            {
                int XStair = r.Next(1, tempMap.GetLength(1) - 1);
                int YStair = r.Next(1, tempMap.GetLength(0) - 1);
                if (tempMap[YStair, XStair].Equals("."))
                {
                    tempMap[YStair, XStair] = ">";
                    if (tempMap[YStair, XStair + 1].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair + 1; backPoint.Y = YStair;
                        p.downStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair, XStair - 1].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair - 1; backPoint.Y = YStair;
                        p.downStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair - 1, XStair].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair; backPoint.Y = YStair - 1;
                        p.downStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair + 1, XStair].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair; backPoint.Y = YStair + 1;
                        p.downStairPositions.Add(backPoint);
                    }


                    downStairPlaced = true;
                }
            } while (downStairPlaced == false);

            do
            {
                int XStair = r.Next(1, tempMap.GetLength(1) - 1);
                int YStair = r.Next(1, tempMap.GetLength(0) - 1);
                if (tempMap[YStair, XStair].Equals("."))
                {
                    tempMap[YStair, XStair] = "<";
                    if (tempMap[YStair, XStair - 1].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair - 1; backPoint.Y = YStair;
                        p.upStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair, XStair + 1].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair + 1; backPoint.Y = YStair;
                        p.upStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair - 1, XStair].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair; backPoint.Y = YStair - 1;
                        p.upStairPositions.Add(backPoint);
                    }
                    else if (tempMap[YStair + 1, XStair].Equals("."))
                    {
                        Player.Point backPoint = new Player.Point();
                        backPoint.X = XStair; backPoint.Y = YStair + 1;
                        p.upStairPositions.Add(backPoint);
                    }
                    upStairPlaced = true;
                }
            } while (upStairPlaced == false);

            #region Entity Spawning
            int numEnts = r.Next(8, 10);
            while (numEnts-- > 0)
            {
                Boolean placed = false;
                do
                {
                    int xVal = r.Next(1, Map.GetLength(1) - 1);
                    int yVal = r.Next(1, Map.GetLength(0) - 1);
                    if (tempMap[yVal, xVal].Equals("."))
                    {
                        placed = true;
                        Entity e = new Entity(xVal, yVal, Map[yVal, xVal]);
                        Entities.Add(entID + "", e);
                        tempMap[yVal, xVal] = Entities[entID + ""].Symbol;
                        entID++;
                    }
                } while (placed == false);
                
            }
            #endregion
            return tempMap;
        }

        String[,] HallGen(String[,] givenMap, int halls)
        {
            while (halls-- > 0)
            {
                int XStart = r.Next(1, givenMap.GetLength(1));
                int YStart = r.Next(1, givenMap.GetLength(0));
                int XEnd = r.Next(1, givenMap.GetLength(1));
                int YEnd = r.Next(1, givenMap.GetLength(0));

                givenMap[YStart, XStart] = ".";
                givenMap[YEnd, XEnd] = ".";

                int XDif; int YDif;
                if (XStart > XEnd)
                {
                    XDif = XStart - XEnd;
                    for (; XDif > 0; XDif--)
                    {
                        givenMap[YStart, XStart--] = ".";
                    }
                }
                else
                {
                    XDif = XEnd - XStart;
                    for (; XDif > 0; XDif--)
                    {
                        givenMap[YStart, XStart++] = ".";
                    }
                }
                if (YStart > YEnd)
                {
                    YDif = YStart - YEnd;
                    for (; YDif > 0; YDif--)
                    {
                        givenMap[YStart--, XEnd] = ".";
                    }
                }
                else
                {
                    YDif = YEnd - YStart;
                    for (; YDif > 0; YDif--)
                    {
                        givenMap[YStart++, XEnd] = ".";
                    }
                }
            }
            return givenMap;
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
        

        public String[,] PlaceObject(String[,] givenMap, String[,] structure, int x, int y)
        {
            int startX = x;
            int startY = y;
            for (int i = 0; i < structure.GetLength(0); i++)
            {
                for (int j = 0; j < structure.GetLength(1); j++)
                {
                    if ((y < givenMap.GetLength(0) && x < givenMap.GetLength(1)) && (i < structure.GetLength(0) && j < structure.GetLength(1)))
                    {
                        givenMap[y, x++] = structure[i, j];
                    }
                    else
                    {
                        break;
                    }
                }
                x = startX;
                y++;
            }
            return givenMap;
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
        #endregion // TODO: Improve this because it's trash.

        void WorldStep()
        {
            foreach (Entity s in entities.Values)
            {
                s.Decide(p, Map);
            }
        }
    }
}