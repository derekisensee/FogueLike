using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class TurnHandler
    {
        World w;

        public TurnHandler()
        {
            w = new World(55);

            Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
            Console.WriteLine("Press J to travel up/down stairs. Press ESC to quit.");
            ConsoleKeyInfo c;
            String tempSpot = "."; // holds the place of the last thing we step on.

            do
            {
                c = Console.ReadKey();
                #region Movement Controls
                if (c.Key == ConsoleKey.UpArrow && (w.Passable.Contains(w.Map[w.P.position.Y - 1, w.P.position.X]) || w.Map[w.P.position.Y - 1, w.P.position.X].Equals(">") || w.Map[w.P.position.Y - 1, w.P.position.X].Equals("<")))
                {
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write(tempSpot);
                    w.Map[w.P.position.Y, w.P.position.X] = tempSpot;
                    w.P.position.Y -= 1;
                    tempSpot = w.Map[w.P.position.Y, w.P.position.X];
                    w.PlacePlayer();
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.DownArrow && (w.Passable.Contains(w.Map[w.P.position.Y + 1, w.P.position.X]) || w.Map[w.P.position.Y + 1, w.P.position.X].Equals(">") || w.Map[w.P.position.Y + 1, w.P.position.X].Equals("<")))
                {
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write(tempSpot);
                    w.Map[w.P.position.Y, w.P.position.X] = tempSpot;
                    w.P.position.Y += 1;
                    tempSpot = w.Map[w.P.position.Y, w.P.position.X];
                    w.PlacePlayer();
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.LeftArrow && (w.Passable.Contains(w.Map[w.P.position.Y, w.P.position.X - 1]) || w.Map[w.P.position.Y, w.P.position.X - 1].Equals(">") || w.Map[w.P.position.Y, w.P.position.X - 1].Equals("<")))
                {
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write(tempSpot);
                    w.Map[w.P.position.Y, w.P.position.X] = tempSpot;
                    w.P.position.X -= 1;
                    tempSpot = w.Map[w.P.position.Y, w.P.position.X];
                    w.PlacePlayer();
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write("@");
                }
                if (c.Key == ConsoleKey.RightArrow && (w.Passable.Contains(w.Map[w.P.position.Y, w.P.position.X + 1]) || w.Map[w.P.position.Y, w.P.position.X + 1].Equals(">") || w.Map[w.P.position.Y, w.P.position.X + 1].Equals("<")))
                {
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write(tempSpot);
                    w.Map[w.P.position.Y, w.P.position.X] = tempSpot;
                    w.P.position.X += 1;
                    tempSpot = w.Map[w.P.position.Y, w.P.position.X];
                    w.PlacePlayer();
                    Console.SetCursorPosition(w.P.position.X, w.P.position.Y);
                    Console.Write("@");
                }
                Console.SetCursorPosition(0, 0);
                #endregion
                #region Attack Stuff
                if (c.Key == ConsoleKey.RightArrow && w.Symbols.Contains(w.Map[w.P.position.Y, w.P.position.X + 1]))
                {
                    foreach (Entity s in w.Entities.Values)
                    {
                        if (s.pos.X == w.P.position.X + 1 && s.pos.Y == w.P.position.Y)
                        {
                            s.decHP(w.P.equipped[0]); // TODO: We'll have to change this to whatever the player's inventory atk power adds up to.
                            w.Map[w.P.position.Y, w.P.position.X + 1] = s.GetSymbol();
                            s.Attack(w.P);
                            Console.SetCursorPosition(w.P.position.X + 1, w.P.position.Y);
                            Console.Write(w.Map[w.P.position.Y, w.P.position.X + 1]);
                            Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
                            Console.Write("You attack the " + s.GetSymbol() + " for some damage!");
                        }
                    }
                }
                if (c.Key == ConsoleKey.LeftArrow && w.Symbols.Contains(w.Map[w.P.position.Y, w.P.position.X - 1]))
                {
                    foreach (Entity s in w.Entities.Values)
                    {
                        if (s.pos.X == w.P.position.X - 1 && s.pos.Y == w.P.position.Y)
                        {
                            s.decHP(w.P.equipped[0]);
                            w.Map[w.P.position.Y, w.P.position.X - 1] = s.GetSymbol();
                            Console.SetCursorPosition(w.P.position.X - 1, w.P.position.Y);
                            Console.Write(w.Map[w.P.position.Y, w.P.position.X - 1]);
                            Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
                            Console.Write("You attack the " + s.GetSymbol() + " for some damage!");
                        }
                    }
                }
                if (c.Key == ConsoleKey.DownArrow && w.Symbols.Contains(w.Map[w.P.position.Y + 1, w.P.position.X]))
                {
                    foreach (Entity s in w.Entities.Values)
                    {
                        if (s.pos.X == w.P.position.X && s.pos.Y == w.P.position.Y + 1)
                        {
                            s.decHP(w.P.equipped[0]);
                            w.Map[w.P.position.Y + 1, w.P.position.X] = s.GetSymbol();
                            Console.SetCursorPosition(w.P.position.X, w.P.position.Y + 1);
                            Console.Write(w.Map[w.P.position.Y + 1, w.P.position.X]);
                            Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
                            Console.Write("You attack the " + s.GetSymbol() + " for some damage!");
                        }
                    }
                }
                if (c.Key == ConsoleKey.UpArrow && w.Symbols.Contains(w.Map[w.P.position.Y - 1, w.P.position.X]))
                {
                    foreach (Entity s in w.Entities.Values)
                    {
                        s.
                        if (s.pos.X == w.P.position.X && s.pos.Y == w.P.position.Y - 1)
                        {
                            s.decHP(w.P.equipped[0]);
                            w.Map[w.P.position.Y - 1, w.P.position.X] = s.GetSymbol();
                            Console.SetCursorPosition(w.P.position.X, w.P.position.Y - 1);
                            Console.Write(w.Map[w.P.position.Y - 1, w.P.position.X]);
                            Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
                            Console.Write("You attack the " + s.GetSymbol() + " for some damage!");
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
                    w.Map[w.P.position.Y, w.P.position.X] = ">";
                    w.CurrentWorld[w.WorldNum] = w.Map;

                    // Load the next map
                    w.Map = w.CurrentWorld[++w.WorldNum];
                    w.Map[w.P.upStairPositions[w.WorldNum].Y, w.P.upStairPositions[w.WorldNum].X] = "@";
                    w.P.position.X = w.P.upStairPositions[w.WorldNum].X; w.P.position.Y = w.P.upStairPositions[w.WorldNum].Y;
                    tempSpot = ".";
                    w.PrintMap();
                }
                // moving up a floor.
                if (c.Key == ConsoleKey.J && tempSpot.Equals("<"))
                {
                    Console.Clear();
                    w.Map[w.P.position.Y, w.P.position.X] = "<";

                    w.Map = w.CurrentWorld[--w.WorldNum];
                    w.Map[w.P.downStairPositions[w.WorldNum].Y, w.P.downStairPositions[w.WorldNum].X] = "@";
                    w.P.position.X = w.P.downStairPositions[w.WorldNum].X; w.P.position.Y = w.P.downStairPositions[w.WorldNum].Y;
                    tempSpot = ".";
                    w.PrintMap();
                }
                #endregion

                Console.SetCursorPosition(0, w.Map.GetLength(0));
                Console.Write("HP:" + w.P.GetCurrentHP() + "/" + w.P.GetMaxHP());
                Console.SetCursorPosition(0, w.Map.GetLength(0) + 1);
                Console.Write("                              ");
            } while (c.Key != ConsoleKey.Escape);
        }
    }
}