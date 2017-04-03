﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class Player
    {
        public List<Item> inventory;
        public List<Item> equipped;

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

        public Player ()
        {
            inventory = new List<Item>();
            equipped = new List<Item>();

            equipped.Add(new Item("fist"));

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
    }
}
