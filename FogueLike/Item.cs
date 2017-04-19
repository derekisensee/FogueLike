using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FogueLike
{
    public class Item
    {
        string name;
        string symbol;
        int atk;
        int def;

        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public int Atk { get => atk; set => atk = value; }
        public int Def { get => def; set => def = value; }

        public Item(String s, String n, int a, int d)
        {
            symbol = s;
            name = n;
            atk = a;
            def = d;
        }

        public int getATK()
        {
            //Random r = new Random();
            //return r.Next(atk, atk + 2);
            return atk;
        }
    }
}