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
        public Point pos;
        List<Item> inventory;
        List<Item> equipped;

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
            symbol = "g";
            hp = 20;
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

        public void decHP(Item i)
        {
            hp -= i.getATK();
            if (hp <= 0)
            {
                symbol = "x";
            }
        }
    }
}