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
        List<Dictionary<String, Entity>> entities;
        List<string> log;

        int worldNum;
        int entID;
        Random r = new Random();

        public List<string> Passable { get => passable; set => passable = value; }
        public List<string> Symbols { get => symbols; set => symbols = value; }
        public List<string[,]> CurrentWorld { get => currentWorld; set => currentWorld = value; }
        //public Dictionary<string, Entity> Entities { get => entities; set => entities = value; }
        //public string[,] Map { get => map; set => map = value; }
        public Player P { get => p; set => p = value; }
        public int WorldNum { get => worldNum; set => worldNum = value; }

        public World(int y)
        {
            Map = new String[y, 150];
            p = new Player();
            currentWorld = new List<String[,]>();
            log = new List<string>();
            entities = new List<Dictionary<string, Entity>>();
            worldNum = 0;
            entID = 0;

            passable = new List<String>();
            passable.Add("."); passable.Add("x"); passable.Add("/");
            symbols = new List<String>();
            symbols.Add("g");

            Dictionary<String, Entity> floorEnts = new Dictionary<String, Entity>();
            entities.Add(floorEnts);
            entities.Add(floorEnts);
            entities.Add(floorEnts);
            entities.Add(floorEnts);
            entities.Add(floorEnts);

            int floors = -1;
            while (floors++ < 4) {
                CurrentWorld.Add(WorldGen(y, floors));
            }

            Map = CurrentWorld[worldNum];
            SpawnPlayer();
            PrintMap();
            
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
                    WorldStep();
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
                    WorldStep();
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
                    WorldStep();
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
                    WorldStep();
                }
                if (c.Key == ConsoleKey.OemPeriod)
                {
                    WorldStep();
                }
                
                Console.SetCursorPosition(0, 0);
                #endregion
                #region Attack Stuff
                if (c.Key == ConsoleKey.RightArrow && symbols.Contains(Map[p.position.Y, p.position.X + 1]))
                {
                    foreach (Entity s in entities[worldNum].Values)
                    {
                        if (s.pos.X == p.position.X + 1 && s.pos.Y == p.position.Y)
                        {
                            if (!(s.Symbol.Equals("x"))) // basically i don't want entities attacking back right away after player attacks, instead they should be attacking whenever WorldTurn deems it's okay for them to.
                            {
                                Console.SetCursorPosition(0, Map.GetLength(0) + 1);
                                int entDmg = s.decHP(p.Equipped[0]);
                                log.Add("You hit for " + entDmg + " damage!");
                                Console.Write("You hit for " + entDmg + " damage! ");

                                int pDmg = s.Attack(p);
                                string l = "The " + s.Symbol + " hits you for " + pDmg + " damage!";
                                log.Add(l);
                                Console.WriteLine(l);
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
                    foreach (Entity s in entities[worldNum].Values)
                    {
                        if (s.pos.X == p.position.X - 1 && s.pos.Y == p.position.Y)
                        {                            
                            if (!(s.Symbol.Equals("x")))
                            {
                                Console.SetCursorPosition(0, Map.GetLength(0) + 1);
                                int entDmg = s.decHP(p.Equipped[0]);
                                log.Add("You hit for " + entDmg + " damage!");
                                Console.Write("You hit for " + entDmg + " damage! ");

                                int pDmg = s.Attack(p);
                                string l = "The " + s.Symbol + " hits you for " + pDmg + " damage!";
                                log.Add(l);
                                Console.WriteLine(l);
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
                    foreach (Entity s in entities[worldNum].Values)
                    {
                        if (s.pos.X == p.position.X && s.pos.Y == p.position.Y + 1)
                        {
                            if (!(s.Symbol.Equals("x")))
                            {
                                Console.SetCursorPosition(0, Map.GetLength(0) + 1);
                                int entDmg = s.decHP(p.Equipped[0]);
                                log.Add("You hit for " + entDmg + " damage!");
                                Console.Write("You hit for " + entDmg + " damage! ");

                                int pDmg = s.Attack(p);
                                string l = "The " + s.Symbol + " hits you for " + pDmg + " damage!";
                                log.Add(l);
                                Console.WriteLine(l);
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
                    foreach (Entity s in entities[worldNum].Values)
                    {
                        if (s.pos.X == p.position.X && s.pos.Y == p.position.Y - 1)
                        {                            
                            if (!(s.Symbol.Equals("x")))
                            {
                                Console.SetCursorPosition(0, Map.GetLength(0) + 1);
                                int entDmg = s.decHP(p.Equipped[0]);
                                log.Add("You hit for " + entDmg + " damage! ");
                                Console.Write("You hit for " + entDmg + " damage!");

                                int pDmg = s.Attack(p);
                                string l = "The " + s.Symbol + " hits you for " + pDmg + " damage!";
                                log.Add(l);
                                Console.WriteLine(l);
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
                #region Inventory Management
                if (c.Key == ConsoleKey.I)
                {
                    // This is for the maximum selected item index spot we can choose.
                    int equippedItems = 0;
                    int inventoryItems = 0;

                    Console.Clear();
                    Console.WriteLine("\nEQUIPPED\nNAME\tATK\tDEF");
                    foreach (Item i in p.Equipped)
                    {
                        Console.WriteLine(equippedItems + " " + i.Symbol + " " + i.Name + "\t" + i.Atk + "\t" + i.Def);
                        equippedItems++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("INVENTORY\nNAME\tATK\tDEF");
                    foreach (Item i in p.Inventory)
                    {
                        Console.WriteLine(inventoryItems + " " + i.Symbol + " " + i.Name + "\t" + i.Atk + "\t" + i.Def);
                        inventoryItems++;
                    }

                    Boolean equipSwitch = false;
                    int selectedInv = 0;
                    int selectedEqu = 0;

                    Console.SetCursorPosition(0, 0);
                    Console.Write("EQUIPPED SELECTED " + selectedEqu);
                    
                    ConsoleKeyInfo a;
                    do
                    {
                        a = Console.ReadKey();
                        if (equipSwitch)
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("INVENTORY SELECTED " + selectedInv);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("EQUIPPED SELECTED " + selectedEqu);
                        }
                        if (a.Key == ConsoleKey.Tab)
                        {
                            equipSwitch = !equipSwitch;
                            if (equipSwitch)
                            {
                                Console.SetCursorPosition(0, 0);
                                Console.Write("INVENTORY SELECTED " + selectedInv);
                            }
                            else
                            {
                                Console.SetCursorPosition(0, 0);
                                Console.Write("EQUIPPED SELECTED " + selectedEqu);
                            }
                        }
                        if (a.Key == ConsoleKey.DownArrow)
                        {
                            if (equipSwitch && selectedInv - 1 >= 0)
                            {
                                selectedInv--;
                                Console.SetCursorPosition(0, 0);
                                Console.Write("INVENTORY SELECTED " + selectedInv);
                            }
                            else if (selectedEqu - 1 >= 0)
                            {
                                selectedEqu--;
                                Console.SetCursorPosition(0, 0);
                                Console.Write("EQUIPPED SELECTED " + selectedEqu);
                            }
                        }
                        if (a.Key == ConsoleKey.UpArrow)
                        {
                            if (equipSwitch && selectedInv + 1 < inventoryItems)
                            {
                                selectedInv++;
                                Console.SetCursorPosition(0, 0);
                                Console.Write("INVENTORY SELECTED " + selectedInv);
                            }
                            else if (selectedEqu + 1 < equippedItems)
                            {
                                selectedEqu++;
                                Console.SetCursorPosition(0, 0);
                                Console.Write("EQUIPPED SELECTED " + selectedEqu);
                            }
                        }
                        if (a.Key == ConsoleKey.Enter)
                        {
                            if (equipSwitch)
                            {
                                Item toSwitch = p.Inventory[selectedInv];
                                p.Inventory.Remove(toSwitch);
                                p.Equipped.Add(toSwitch);
                            }
                            else
                            {
                                Item toSwitch = p.Equipped[selectedEqu];
                                p.Equipped.Remove(toSwitch);
                                p.Inventory.Add(toSwitch);
                            }
                            equippedItems = 0;
                            inventoryItems = 0;

                            Console.Clear();
                            Console.WriteLine("\nEQUIPPED\nNAME\tATK\tDEF");
                            foreach (Item i in p.Equipped)
                            {
                                Console.WriteLine(equippedItems + " " + i.Symbol + " " + i.Name + "\t" + i.Atk + "\t" + i.Def);
                                equippedItems++;
                            }
                            Console.WriteLine();
                            Console.WriteLine("INVENTORY\nNAME\tATK\tDEF");
                            foreach (Item i in p.Inventory)
                            {
                                Console.WriteLine(inventoryItems + " " + i.Symbol + " " + i.Name + "\t" + i.Atk + "\t" + i.Def);
                                inventoryItems++;
                            }

                            equipSwitch = false;
                            selectedInv = 0;
                            selectedEqu = 0;

                            Console.SetCursorPosition(0, 0);
                            Console.Write("EQUIPPED SELECTED " + selectedEqu);
                        }
                    } while (a.Key != ConsoleKey.Escape);
                    Console.Clear();
                    PrintMap();
                }
                #endregion

                if (c.Key == ConsoleKey.U)
                {
                    Console.Clear();
                    Console.WriteLine("UNITS");
                    foreach (Entity e in entities[worldNum].Values)
                    {
                        Console.WriteLine(e.Symbol + "\t" + e.pos.X + " " + e.pos.Y + "\t" + worldNum);
                    }
                    ConsoleKeyInfo a = Console.ReadKey();
                    do
                    {

                    } while (a.Key != ConsoleKey.Escape);
                    PrintMap();
                }

                if (c.Key == ConsoleKey.L)
                {
                    Console.Clear();
                    Console.WriteLine("LOG");
                    foreach (string s in log)
                    {
                        Console.WriteLine(s);
                    }
                    ConsoleKeyInfo a = Console.ReadKey();
                    do
                    {

                    } while (a.Key != ConsoleKey.Escape);
                    PrintMap();
                }
                //PrintMap(); // This is more for debugging purposes, performance is too poor when this is uncommented.

                Console.SetCursorPosition(0, Map.GetLength(0));
                Console.Write("HP:" + p.CurrentHP + "/" + p.MaxHP);
            } while (c.Key != ConsoleKey.Escape);
        }

        public void SpawnPlayer() 
        {
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
        String[,] WorldGen(int y, int floor)
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
            int numEnts = r.Next(2, 8);
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
                        entities[floor].Add(entID + "", e);
                        tempMap[yVal, xVal] = entities[floor][entID + ""].Symbol; // I think we might be getting issues here?
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
            foreach (Entity e in entities[worldNum].Values)
            {
                e.Decide(p, Map);
                Map[e.tempPos.Y, e.tempPos.X] = e.TempSpot;
                Map[e.pos.Y, e.pos.X] = "g";
            }
            // HACK: DIRTY HACK
            #region HACK
            // Since I couldn't figure out what was making "ghost" symbols I made us go through the whole board, looking for "g"'s, and if we find one see if it's position is in the entity list, if not then delete.
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j].Equals("g"))
                    {
                        Boolean exists = false;
                        foreach (Entity e in entities[worldNum].Values)
                        {
                            if (e.pos.X == j && e.pos.Y == i)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (exists == false)
                        {
                            Map[i, j] = ".";
                            Console.SetCursorPosition(j, i);
                            Console.Write(".");
                        }
                    }
                }
            }
            #endregion
        }
    }
}