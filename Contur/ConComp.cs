using System;
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
        // Хранилище разделительных точек контура
        public List<Point> Points = new List<Point>();

        // Для создания новой компоненты из единственной точки.
        //
        public ConComp(Point p)
        {
            Add(p);
        }

        // Для создания начальной компоненты, в которую включаются 
        // все реперные точки на верхней и левой границах изображения.
        //
        public ConComp(int width, int height)
        {
            for (int xo = 0; xo < width; xo++)
                Add(new Point(xo, 0));
            for (int yo = 0; yo < height; yo++)
                Add(new Point(0, yo));
        }
    }

}
