using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FogueLike
{
    public class Item
    {
        String name;
        int atk;

        public Item(String n, int a)
        {
            name = n;
            atk = a;
        }

        public int getATK()
        {
            //Random r = new Random();
            //return r.Next(atk, atk + 2);
            return atk;
        }
    }
}