﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    public class CPoint
    {
        public int Chrome;
        public System.Drawing.Point P;

        public CPoint(int x, int y, int c)
        {
            Chrome = c;
            P = new System.Drawing.Point(x, y);
        }

        public CPoint(System.Drawing.Point p, int c)
        {
            Chrome = c;
            P = p;
        }
    }
}
