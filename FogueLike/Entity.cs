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
        String id;
        String symbol;
        int hp;
        int atk;
        int speed;

        public Point pos;
        List<Item> inventory;
        List<Item> equipped;
        int missChance;

        string tempSpot;

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
            tempSpot = t;

            inventory = new List<Item>();
            equipped = new List<Item>();

            equipped.Add(new Item("sword", 5));

            pos = new Point();
            pos.X = x;
            pos.Y = y;
        }

        public void Decide(Player p, String[,] map)
        {
            if (map[pos.Y, pos.X - 1].Equals("@") || map[pos.Y, pos.X + 1].Equals("@") || map[pos.Y - 1, pos.X].Equals("@") || map[pos.Y + 1, pos.X].Equals("@"))
            {
                Attack(p);
            }
            if (CanSeePlayer(p, map))
            {
                int startX = pos.X;
                int startY = pos.Y;
                int endX = p.position.X;
                int endY = p.position.Y;
                int difX; int difY;
                if (startX > endX && map[pos.Y, pos.X - 1].Equals(".")) // TODO: Reprint the entity locations. Figure out what is going on here.
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(tempSpot);
                    tempSpot = map[pos.Y, pos.X - 1];
                    Console.SetCursorPosition(pos.X - 1, pos.Y);
                    pos.X -= 1;
                    Console.Write(symbol);
                }
                else if (map[pos.Y, pos.X + 1].Equals("."))
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(tempSpot);
                    tempSpot = map[pos.Y, pos.X + 1];
                    Console.SetCursorPosition(pos.X + 1, pos.Y);
                    pos.X += 1;
                    Console.Write(symbol);
                }
                else if (startY > endY && map[pos.Y - 1, pos.X].Equals("."))
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(tempSpot);
                    tempSpot = map[pos.Y - 1, pos.X];
                    Console.SetCursorPosition(pos.X, pos.Y - 1);
                    pos.Y -= 1;
                    Console.Write(symbol);
                }
                else if (map[pos.Y + 1, pos.X].Equals("."))
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(tempSpot);
                    tempSpot = map[pos.Y + 1, pos.X];
                    Console.SetCursorPosition(pos.X, pos.Y + 1);
                    pos.Y += 1;
                    Console.Write(symbol);
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
                    if (!(map[j, i].Equals("."))) // TODO: Make sure we are using j/i in the right order.
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void decHP(Item i) // TODO: Make it where we are decreasing HP by all equipped items by a thing instead of just 1 item.
        {
            hp -= i.getATK();
            if (hp <= 0)
            {
                symbol = "x";
            }
        }

        public void Attack(Player p)
        {
            Random r = new Random();
            if (r.Next(0, 100) > missChance)
            {
                p.DecHP(atk);
            }
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