using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FogueLike
{
    public class Player
    {
        struct Point
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

        public Player(int x, int y)
        {

        }
    }
}
