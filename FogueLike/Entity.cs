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

        public Point pos;
        List<Item> inventory;
        List<Item> equipped;
        int missChance;

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

        public Entity(int x, int y)
        {
            // i know it's bad to hardcode this but i'm just trying to get stuff up and running before i worry about
            // generic stuffs.
            symbol = "g";
            hp = 20;
            missChance = 40;
            atk = 5;

            inventory = new List<Item>();
            equipped = new List<Item>();
            pos = new Point();
            pos.X = x;
            pos.Y = y;
        }

        public String GetSymbol()
        {
            return symbol;
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

        }
    }
}