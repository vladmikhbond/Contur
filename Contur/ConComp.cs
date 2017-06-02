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

        // Точки, не вошедшие в контур
        public List<Point> Fails = new List<Point>();

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

        /// Собирает контур из неупорядоченного множества точек
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        /// 
        public void OrderPoints(int step)
        {
            int MAX_DIST = (step * step * 2) * 3 * 3; // линейный размер - 3 диагонали клетки
            var input = Points;
            var output = new List<Point>();
            Fails = new List<Point>();

            var last = input[0];
            output.Add(last);
            input.RemoveAt(0);

            while (input.Count > 0)
            {
                var dists = input.Select(p => Dist(p, last));
                double minDist = dists.Min();
                int minIdx = dists.ToList().IndexOf(minDist);
                // слишком далекие точки пропускаем
                if (minDist <= MAX_DIST)
                {
                    last = input[minIdx];
                    output.Add(last);
                }
                else
                {
                    Fails.Add(input[minIdx]);
                }
                input.RemoveAt(minIdx);
            }
            Points = output;
        }

        private static double Dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

    }

}
