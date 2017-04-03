using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class Item
    {
        String name;
        int atk;

        public Item(String name)
        {
            name = this.name;
            atk = 5;
        }

        public int getATK()
        {
            Random r = new Random();
            return r.Next(atk - 2, atk + 2);
        }
    }
}