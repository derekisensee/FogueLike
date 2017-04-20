// This file is for enemies.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class Entity
    {
        String symbol;
        int hp;
        int atk;
        int speed;

        Random r = new Random();

        public Point pos;
        public Point tempPos;

        List<Item> inventory;
        List<Item> equipped;
        List<String> passable;
        int missChance;

        string tempSpot;

        private int wait;

        public string Symbol { get => symbol; set => symbol = value; }
        public int Hp { get => hp; set => hp = value; }
        public string TempSpot { get => tempSpot; set => tempSpot = value; }

        public struct Point
        {
            private int x; private int y;
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

        public Entity(int x, int y, String t)
        {
            // i know it's bad to hardcode this but i'm just trying to get stuff up and running before i worry about
            // generic stuffs.
            symbol = "g";
            hp = 20;
            missChance = 50;
            atk = 5;
            speed = 30;
            tempSpot = ".";
            tempPos.X = x; tempPos.Y = y;

            wait = 0;

            r = new Random();

            passable = new List<String>();
            passable.Add("."); passable.Add("x");

            inventory = new List<Item>();
            equipped = new List<Item>();

            equipped.Add(new Item("/", "sword", 5, 0));

            pos = new Point();
            pos.X = x;
            pos.Y = y;
        }

        public void Decide(Player p, String[,] map)
        {
            if (!(symbol.Equals("x")) && wait-- <= 0)
            {
                if (map[pos.Y, pos.X - 1].Equals("@") || map[pos.Y, pos.X + 1].Equals("@") || map[pos.Y - 1, pos.X].Equals("@") || map[pos.Y + 1, pos.X].Equals("@"))
                {
                    Attack(p);
                    wait += 3;
                }
                else if (CanSeePlayer(p, map))
                {
                    int startX = pos.X;
                    int startY = pos.Y;
                    int endX = p.position.X;
                    int endY = p.position.Y;
                    if (startX > endX && passable.Contains(map[pos.Y, pos.X - 1]))
                    {
                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(tempSpot);

                        map[pos.Y, pos.X] = tempSpot;
                        tempSpot = map[pos.Y, pos.X - 1];
                        map[pos.Y, pos.X - 1] = symbol;

                        Console.SetCursorPosition(pos.X - 1, pos.Y);
                        pos.X -= 1;
                        Console.Write(symbol);
                    }
                    else if (passable.Contains(map[pos.Y, pos.X + 1]))
                    {
                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(tempSpot);
                        map[pos.Y, pos.X] = tempSpot;
                        tempSpot = map[pos.Y, pos.X + 1];
                        map[pos.Y, pos.X + 1] = symbol;
                        Console.SetCursorPosition(pos.X + 1, pos.Y);
                        pos.X += 1;
                        Console.Write(symbol);
                    }
                    else if (startY > endY && passable.Contains(map[pos.Y - 1, pos.X]))
                    {
                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(tempSpot);
                        map[pos.Y, pos.X] = tempSpot;
                        tempSpot = map[pos.Y - 1, pos.X];
                        map[pos.Y - 1, pos.X] = symbol;
                        Console.SetCursorPosition(pos.X, pos.Y - 1);
                        pos.Y -= 1;
                        Console.Write(symbol);
                    }
                    else if (passable.Contains(map[pos.Y + 1, pos.X]))
                    {
                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(tempSpot);
                        map[pos.Y, pos.X] = tempSpot;
                        tempSpot = map[pos.Y + 1, pos.X];
                        map[pos.Y + 1, pos.X] = symbol;
                        Console.SetCursorPosition(pos.X, pos.Y + 1);
                        pos.Y += 1;
                        Console.Write(symbol);
                    }
                }
                else
                {
                    RandomMove(map);
                }
            }            
        }

        Boolean CanSeePlayer(Player p, String[,] map)
        {
            int startX = pos.X;
            int startY = pos.Y;
            int endX = p.position.X;
            int endY = p.position.Y;

            int difX; int difY;

            if (startX > endX)
            {
                difX = startX - endX;
            }
            else
            {
                difX = endX - startX;
            }
            if (startY > endY)
            {
                difY = startY - endY;
            }
            else
            {
                difY = endY - startY;
            }

            for (int i = difX; i > p.position.X; i--) // TODO: Finish calculated if we have LOS to player. This is incomplete.
            {
                if (!(map[pos.Y, i].Equals(".")))
                {
                    return false;
                }
            }

            for (int i = difY; i > p.position.Y; i--) 
            {
                if (!(map[i, pos.X].Equals(".")))
                {
                    return false;
                }
            }

            for (int j = difY; j > 0; j--)
            {
                for (int i = difX; i > 0; i--)
                {
                    if (!(map[j, i].Equals("."))) // TODO: Make sure we are using j/i in the right order. Pretty sure we are.
                    {
                        return false;
                    }
                }
            }

            return true;
        } // TODO: We can condense this into the above if statement where this function is called.

        void RandomMove(String[,] m)
        {
            int n = r.Next(0, 7);
            if (n == 3 && passable.Contains(m[pos.Y, pos.X + 1])) // TODO: The position of entities is not being updated when we change it here. I think.
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(tempSpot);

                tempSpot = m[pos.Y, pos.X + 1];
                tempPos.X = pos.X;
                pos.X += 1;

                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(symbol);
            }
            if (n == 4 && passable.Contains(m[pos.Y, pos.X - 1]))
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(tempSpot);

                tempSpot = m[pos.Y, pos.X - 1];
                tempPos.X = pos.X;
                pos.X -= 1;

                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(symbol);
            }
            if (n == 5 && passable.Contains(m[pos.Y + 1, pos.X]))
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(tempSpot);

                tempSpot = m[pos.Y + 1, pos.X];
                tempPos.Y = pos.Y;
                pos.Y += 1;
                
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(symbol);
            }
            if (n == 6 && passable.Contains(m[pos.Y - 1, pos.X]))
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(tempSpot);

                tempSpot = m[pos.Y - 1, pos.X];
                tempPos.Y = pos.Y;
                pos.Y -= 1;
                
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(symbol);
            }
        }

        public int decHP(Item i) // TODO: Make it where we are decreasing HP by all equipped items by a thing instead of just 1 item.
        {
            int hitFor = i.getATK();
            hp -= hitFor;
            if (hp <= 0)
            {
                symbol = "x";
            }
            return hitFor;
        }

        public int Attack(Player p)
        {
            Random r = new Random();
            int hit = 0;
            if (r.Next(0, 100) > missChance)
            {
                hit = p.DecHP(atk);
            }
            return hit;
        }

        public void Attack(Entity e)
        {
            Random r = new Random();
            if (r.Next(0, 100) > missChance)
            {
                e.decHP(equipped[0]);
            }
        }
    }
}