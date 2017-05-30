﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{

    /// <summary>
    /// Компонента связности - коллекция реперных точек 
    /// </summary>
    public class ConComp : List<Point>
    {
        public List<System.Drawing.Point> Points = new List<System.Drawing.Point>();

        public ConComp() { }

        public ConComp(int len0, int len1)
        {
            for (int xo = 0; xo < len0; xo++)
                Add(new Point(xo, 0));
            for (int yo = 0; yo < len1; yo++)
                Add(new Point(0, yo));
        }
    }

}