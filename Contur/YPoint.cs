using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    public class Chromic
    {
        public int C;
    }


    public class YPoint
    {
        public Chromic Chromic1;
        public Chromic Chromic2;
        public Point Point;

        public YPoint(Point p, Chromic chromic1, Chromic chromic2)
        {
            Point = p;
            Chromic1 = chromic1;
            Chromic2 = chromic2;
        }
    }
}
