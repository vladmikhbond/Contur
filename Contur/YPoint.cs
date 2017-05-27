using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    public struct Dot
    {
        public int X, Y;

        public Dot(int x, int y)
        {
            X = x; Y = y;
        }
    }

    public class Comp : List<Dot>
    {
        public List<Point> Points = new List<Point>();
        public Comp() { }

        public Comp(int len0, int len1)
        {
            for (int xo = 0; xo < len0; xo++)
                Add(new Contur.Dot(xo, 0));
            for (int yo = 0; yo < len1; yo++)
                Add(new Contur.Dot(0, yo));
        }
    }

}
