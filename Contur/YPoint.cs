using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    public class Dot
    {
        public Comp Parent;

        public Dot(Comp comp)
        {
            Parent = comp;
        }
    }

    public class Comp : List<Dot>
    {
    }

    public class YPoint
    {
        public Dot Dot1, Dot2;
        public Point Point;

        public YPoint(Point p, Dot dot1, Dot dot2)
        {
            Point = p;
            Dot1 = dot1;
            Dot2 = dot2;
        }
    }
}
