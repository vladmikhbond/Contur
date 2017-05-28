using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


// Алгоритм закраски
//
// 1. Находим первую незакрашенную точку Т, идя слева направо, сверху вниз.
// 2. Проверяем первого закрашенного соседа. Если он смежный, красим точку Т в его цвет.
// 3. Проверяем следующего закрашенного соседа. Если он смежный, а точка Т еще не закрашена, красим точку Т в его цвет.
// 4. Если он смежный, а точка Т уже закрашена, перекрашиваем все точки соседнего цвета в цвет точки Т.
// 5. Если среди закрашенных соседей нет смежных (т.е. они все изолированы), 
//    красим точку Т в новый цвет и создаем парные цветные точки контуров в местах изоляции.
//
// Собираем контуры из одноцветных закрашенных контурных точек.

namespace Contur
{

    /// <summary>
    /// Класс для изготовления контуров на основе монохромного контурного изображения.
    /// 
    /// public ctor,  MakeAllConturs(), ToString
    /// 
    /// note: internal access level for debugging only
    /// </summary>
    public class YContur: IContur
    {
        
        Bitmap _img;
        int _step;

        Comp[,] dots;   // scout net : 0 - empty, 1,2,3... - contur chromes
        List<Point> ypoints; // общая коллекция цветных точек 

        //int dotСhrome;        // "цвет" закрашенных точек
 

        // for inspectin
        public Point[] Points
        {
            get { return ypoints.ToArray(); }
        }

        public int this[int x, int y]
        {
            get
            {
                return dots[x, y].GetHashCode() % 100;
            }
        }

        public int diagnostic_lostPoints;

        public YContur(Bitmap img, int step)
        {
            _img = img;
            _step = step;
        }

        void FludFill()
        {
            ypoints = new List<Point>();
            // init dots array
            dots = new Comp[_img.Width / _step + 1, _img.Height / _step + 1];

            Comp zero = new Comp(dots.GetLength(0), dots.GetLength(1));
            for (int xo = 0; xo < dots.GetLength(0); xo++)
                dots[xo, 0] = zero;
            for (int yo = 0; yo < dots.GetLength(1); yo++)
                dots[0, yo] = zero;

            for (int xo = 1; xo < dots.GetLength(0); xo++)
            {
                for (int yo = 1; yo < dots.GetLength(1); yo++)
                {
                    int y = PathToUp(xo, yo);
                    int x = PathToLeft(xo, yo);
                    if (y == -1 && x == -1)
                    {
                        // красим в верхний цвет
                        dots[xo, yo] = dots[xo, yo - 1];
                        dots[xo, yo].Add(new Dot(xo, yo));
                        
                        var up = dots[xo, yo - 1];
                        var left = dots[xo - 1, yo];
                        // если компоненты разные, вливаем в верхнюю компоненту левую
                        if (up != left)
                        {
                            for (int i = 0; i < left.Count; i++)
                            {
                                dots[left[i].X, left[i].Y] = up;
                            }
                            up.AddRange(left);
                        }                            
                    }
                    else if (y == -1 && x != -1)
                    {
                        // красим в верхний цвет
                        dots[xo, yo] = dots[xo, yo - 1];
                        dots[xo, yo].Add(new Dot(xo, yo));
                        // создаем точку слева
                        ypoints.Add(new Point(x, yo * _step));
                    }
                    else if (x == -1 && y != -1)
                    {
                        // красим в левый цвет
                        dots[xo, yo] = dots[xo - 1, yo];
                        dots[xo, yo].Add(new Dot(xo, yo));
                        // создаем точку сверху
                        var p = new Point(xo * _step, y);
                        ypoints.Add(p);
                    }
                    else  // x != -1 && y != -1
                    {
                        // красим в новый цвет
                        dots[xo, yo] = new Comp();
                        dots[xo, yo].Add(new Dot(xo, yo));

                        // создаем точку слева
                        var p = new Point(x, yo * _step);
                        ypoints.Add(p);
                        // создаем точку сверху
                        p = new Point(xo * _step, y);
                        ypoints.Add(p);
                    }
                }
            }
        }


        private int PathToLeft(int xo, int yo)
        {
            int x1 = xo * _step, x2 = (xo - 1) * _step, y1 = yo * _step;
            for (int x = x1; x > x2; x--)
                if (IsBlack(x, y1) != IsBlack(x1, y1))
                //if (IsBlack(x, y))
                    return x;
            return -1;
        }


        private int PathToUp(int xo, int yo)
        {
            int y1 = yo * _step, y2 = (yo - 1) * _step, x1 = xo * _step;
            for (int y = y1; y > y2; y--)
                if (IsBlack(x1, y) != IsBlack(x1, y1))
                    //if (IsBlack(x, y))
                    return y;
            return -1;
        }


        // Test if the point is on a board
        //
        private bool IsBlack(int x, int y)
        {
            if (x >= _img.Width || y >= _img.Height)
                return false;
            Color c = _img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }


        public List<Point[]> MakeAllConturs()
        {
            FludFill();

            for (int xo = 1; xo < dots.GetLength(0); xo++)
            {
                for (int yo = 1; yo < dots.GetLength(1); yo++)
                {
                    if (dots[xo, yo].Count == 1)
                    {
                        //if (dots[xo - 1, yo].Count > 1)
                        //{
                        //    dots[xo - 1, yo].Add(new Dot(xo, yo));
                        //    dots[xo, yo] = dots[xo - 1, yo];
                        //}
                        //if (dots[xo, yo - 1].Count > 1)
                        //{
                        //    dots[xo, yo - 1].Add(new Dot(xo, yo));
                        //    dots[xo, yo] = dots[xo, yo - 1];
                        //}
                        if (xo + 1 < dots.GetLength(0))
                        {
                            ////
                        }



                    }
                }
            }


            var conturs = new List<Point[]>();
            foreach (var p in ypoints)
            {
                int ox = p.X / _step;
                int oy = p.Y / _step;
                dots[ox, oy].Points.Add(p);
                // если точка лежит на вертикали
                if (ox * _step == p.X && oy + 1 < dots.GetLength(1))
                {
                    dots[ox, oy + 1].Points.Add(p);
                }
                // если точка лежит на горизонтали
                if (oy * _step == p.Y && ox + 1 < dots.GetLength(0))
                {
                    dots[ox + 1, oy].Points.Add(p);
                }
            }
            var conmps = dots.OfType<Comp>().Distinct().Where(c => c.Count > 1).ToArray();
            foreach (var comp in conmps)
            {
                var ps = MakeConturFromPointSet(comp.Points);
                if (ps != null)
                    conturs.Add(ps.ToArray());
            }
            return conturs;
        }


        /// Собирает контур из неупорядоченного множества точек
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        /// 
        List<Point> MakeConturFromPointSet(IEnumerable<Point> points)
        {
            int MAX_DIST = _step * _step * 8;
            var input = new List<Point>(points);
            var output = new List<Point>();

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
                } else
                {
                    diagnostic_lostPoints++;
                }
                input.RemoveAt(minIdx);
            }
            // exclude unclose conturs   //  it is a PATCH
            //if (Dist(output[0], output.Last()) >= MAX_DIST)
            //    return null;
            return output;

        }

        private static double Dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }


 
    }
}
