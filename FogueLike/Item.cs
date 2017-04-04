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

        public Item(String name, int atk)
        {
            name = this.name;
            atk = this.atk;
        }

        public int getATK()
        {
            Random r = new Random();
            return r.Next(atk - 2, atk + 2);
        }
    }
}