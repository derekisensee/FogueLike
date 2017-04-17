using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class Player
    {
        int currentHP;
        int maxHP;
        private List<Item> inventory;
        private List<Item> equipped;

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
        } // player's location
        public Point position;
        public List<Point> downStairPositions;
        public List<Point> upStairPositions;

        public int CurrentHP { get => currentHP; set => currentHP = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }
        public List<Item> Inventory { get => inventory; set => inventory = value; }
        public List<Item> Equipped { get => equipped; set => equipped = value; }

        public Player()
        {
            maxHP = 400;
            currentHP = maxHP;
            inventory = new List<Item>();
            equipped = new List<Item>();

            Item fist = new Item("\"", "fist", 5, 0);
            
            equipped.Add(fist);

            downStairPositions = new List<Point>();
            upStairPositions = new List<Point>();
            position = new Point();
        }

        public Player(int x, int y)
        {
            position = new Point();
            position.X = 30;
            position.Y = 20;
        }

        public int DecHP(int atk)
        {
            currentHP -= atk;
            return atk;
        }
    }
}
