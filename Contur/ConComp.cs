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
        // valide after PointsInOrder call
        int MAX_DIST;

        // Хранилище разделительных точек контура
        public List<Point> Points = new List<Point>();

        // Точки, не вошедшие в контур
        public List<Point> Fails = new List<Point>();

        // Indicate if contur closed
        public bool Closed
        {
            get
            {
                return Dist(Points.First(), Points.Last()) < MAX_DIST;
            }
        }

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
        public void PointsInOrder(int step)
        {
            MAX_DIST = (step * step * 2) * 3 * 3; // линейный размер - 3 диагонали клетки
            var input = Points;
            var output1 = new List<Point>();
            Fails = new List<Point>();

            Point start = input[0];

            Point current = start;
           
            // a first half
            while (input.Count > 0)
            {
                var dists = input.Select(p => Dist(p, current));
                int minDist = dists.Min();
                int minIdx = dists.ToList().IndexOf(minDist);
                // прекращаем, когда нет близких точек
                if (minDist > MAX_DIST)
                    break;
                current = input[minIdx];
                output1.Add(current);
                input.RemoveAt(minIdx);
            }

            current = start;
            var output2 = new List<Point>();

            while (input.Count > 0)
            {
                var dists = input.Select(p => Dist(p, current));
                int minDist = dists.Min();
                int minIdx = dists.ToList().IndexOf(minDist);
                // слишком далекие точки перекладываем в Fails
                if (minDist <= MAX_DIST)
                {
                    current = input[minIdx];
                    output2.Add(current);
                }
                else
                {
                    Fails.Add(input[minIdx]);
                }
                input.RemoveAt(minIdx);
            }
            output2.Reverse();
            Points = output2;
            Points.Add(start);
            Points.AddRange(output1);

            JoinFails();
            InverseLoops();
        }


        //  ...i | i+1...j | j+1 ...
        void InverseLoops()
        {
            for (int i = 0; i < Points.Count - 1; i++)
            {
                Point a1 = Points[i], a2 = Points[i + 1];
                for (int j = i + 2; j < Points.Count - 1; j++)
                {
                    Point b1 = Points[j], b2 = Points[j + 1];
                    if (Intersect(a1, a2, b1, b2))
                    {
                        for (int p = i + 1, q = j; p < q; p++, q--)
                        {
                            var t = Points[p]; Points[p] = Points[q]; Points[q] = t;
                        }
                    }
                }
            }
        }

        // Найти в Fails и Points две ближайшие точки. 
        // Если расстояние между ними не велико, выстроить трек в Fails
        // Если расстояние между последней точкой трека и Points невелико, вствавить трек в Points и удалить из Fails
        // Повторять пока в Fails находятся недалекие точки
        void JoinFails()
        {

            for(;;)
            {
                if (Fails.Count == 0)
                    break;
                // Найти в Fails и Points две ближайшие точки. 
                int minDist = int.MaxValue, minI = 0, minJ = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Fails.Count; j++)
                    {
                        var dist = Dist(Points[i], Fails[j]);
                        if (dist < minDist)
                        {
                            minI = i; minJ = j; minDist = dist;
                        }
                    }
                }

                if (minDist > MAX_DIST)
                    break;
                // Если расстояние между ними не велико, выстроить трек в Fails
                Point current = Fails[minJ];
                var track = new List<Point>();

                // a first half
                while (Fails.Count > 0)
                {
                    var dists = Fails.Select(p => Dist(p, current));
                    int minDistTrack = dists.Min();
                    int minIdx = dists.ToList().IndexOf(minDistTrack);
                    // прекращаем, когда нет близких точек
                    if (minDistTrack > MAX_DIST)
                        break;
                    current = Fails[minIdx];
                    track.Add(current);
                    Fails.RemoveAt(minIdx);
                }
                // Если расстояние между последней точкой трека и тоской Points[minI+1] невелико,                
                if (Dist(track.Last(), Points[minI+1 % Points.Count]) > MAX_DIST)
                    break;
                // вствавить трек в Points после minI-й точки   
                Points.InsertRange(minI, track);
            }
        }

        #region unilities

        // Квадрат расстояния
        //
        private static int Dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        // Пересечение отрезков (a1, a2) и (b1, b2)
        //
        private static bool Intersect(Point a1, Point a2, Point b1, Point b2)
        {
            int ax1 = a1.X, ay1 = a1.Y, ax2 = a2.X, ay2 = a2.Y, 
                bx1 = b1.X, by1 = b1.Y, bx2 = b2.X, by2 = b2.Y;
            var v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
            var v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
            var v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
            var v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
            return  v1 * v2 < 0 && v3 * v4 < 0;
        }

        #endregion

    }

}
