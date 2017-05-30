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
    public class YContur 
    {
        
        Bitmap _img;
        int _step;
        Graphics _g;

        Comp[,] dots;   // scout net : 0 - empty, 1,2,3... - contur chromes
        List<System.Drawing.Point> ypoints; // общая коллекция цветных точек 
        System.Drawing.Point[,] pots;
 

        public YContur(Bitmap img, int step, Graphics g)
        {
            _img = img;
            _step = step;
            _g = g;
        }

        public List<System.Drawing.Point[]> MakeAllConturs()
        {
            FludFill();
            ShowDots();
            var conturs = new List<System.Drawing.Point[]>();
           // return conturs;

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
            var comps = dots.OfType<Comp>().Distinct().Where(c => c.Count > 1).ToArray();
            foreach (var comp in comps)
            {
                var ps = MakeConturFromPointSet(comp.Points);
                if (ps != null)
                    conturs.Add(ps.ToArray());
            }
            return conturs;
        }


        void FludFill()
        {
            ypoints = new List<System.Drawing.Point>();

            // init dots array
            int WO = _img.Width / _step + 1;
            int HO = _img.Height / _step + 1;
            dots = new Comp[WO, HO];
            Comp zero = new Comp(WO, HO);
            for (int xo = 0; xo < WO; xo++)
                dots[xo, 0] = zero;
            for (int yo = 0; yo < HO; yo++)
                dots[0, yo] = zero;

            // init pots array
            pots = new System.Drawing.Point[WO, HO];
            int[,] dxy = { { 0, 0 },
                { 0, 1 }, { 1, 0 },
                { 0, 2 }, { 1, 1 }, { 2, 0 },
                { 0, 3}, { 1, 2 }, { 2, 1 }, { 3, 0 },
                { 0, 4}, { 1, 3 }, { 2, 2 }, { 3, 1 }, { 4, 0 },
                { 0, 5}, { 1, 4 }, { 2, 3 }, { 3, 2 }, { 4, 1 }, { 5, 0 },
                { 0, 6}, { 1, 5 }, { 2, 4 }, { 3, 3 }, { 4, 2 }, { 5, 1 }, { 6, 0 },
            };
            for (int xo = 0; xo < WO; xo++)
            {
                for (int yo = 0; yo < HO; yo++)
                {
                    int x = xo * _step;
                    int y = yo * _step;
                    int dx = 0, dy = 0;
                    for(int a = 0; a < dxy.Length / 2; a++)
                    {
                        dx = dxy[a, 0];
                        dy = dxy[a, 1];
                        if (!IsBlack(x - dx, y - dy))
                            break;
                    }

                    pots[xo, yo] = new System.Drawing.Point(x - dx, y - dy);
                }
            }


            for (int yo = 1; yo < dots.GetLength(1); yo++)
            {
                for (int xo = 1; xo < dots.GetLength(0); xo++)
                {
                    int y = PathToUp(xo, yo);
                    int x = PathToLeft(xo, yo);
                    if (y == -1 && x == -1)
                    {
                        // красим в верхний цвет
                        dots[xo, yo] = dots[xo, yo - 1];
                        dots[xo, yo].Add(new Point(xo, yo));
                        
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
                        dots[xo, yo].Add(new Point(xo, yo));
                        // создаем точку слева
                        ypoints.Add(new System.Drawing.Point(x, yo * _step));
                    }
                    else if (x == -1 && y != -1)
                    {
                        // красим в левый цвет
                        dots[xo, yo] = dots[xo - 1, yo];
                        dots[xo, yo].Add(new Point(xo, yo));
                        // создаем точку сверху
                        var p = new System.Drawing.Point(xo * _step, y);
                        ypoints.Add(p);
                    }
                    else  // x != -1 && y != -1
                    {
                        // красим в новый цвет
                        dots[xo, yo] = new Comp();
                        dots[xo, yo].Add(new Point(xo, yo));

                        // создаем точку слева
                        var p = new System.Drawing.Point(x, yo * _step);
                        ypoints.Add(p);
                        // создаем точку сверху
                        p = new System.Drawing.Point(xo * _step, y);
                        ypoints.Add(p);

                    }
                }
            }
        }


        private int PathToLeft(int xo, int yo)
        {
            System.Drawing.Point p1 = pots[xo, yo];
            System.Drawing.Point p2 = pots[xo - 1, yo];
            int y1 = Math.Min(p1.Y, p2.Y);
            int y2 = Math.Max(p1.Y, p2.Y);

            for (int x = p1.X; x >= p2.X; x--)
                if (IsBlack(x, y1))
                    return x;
            for (int y = y1; y <= y2; y++)
                if (IsBlack(p2.X, y))
                    return p2.X;
            return -1;
        }


        private int PathToUp(int xo, int yo)
        {
            System.Drawing.Point p1 = pots[xo, yo];
            System.Drawing.Point p2 = pots[xo, yo - 1];
            int x1 = Math.Min(p1.X, p2.X);
            int x2 = Math.Max(p1.X, p2.X);

            for (int y = p1.Y; y > p2.Y; y--)
                if (IsBlack(x1, y))
                    return y;
            for (int x = x1; x <= x2; x++)
                if (IsBlack(x, p2.Y))
                    return p2.Y;
            return -1;
        }


        // Test if a point is on a board
        //
        private bool IsBlack(int x, int y)
        {
            if (x >= _img.Width || y >= _img.Height || x <= 0 || y <= 0)
                return false;
            Color c = _img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }



        /// Собирает контур из неупорядоченного множества точек
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        /// 
        List<System.Drawing.Point> MakeConturFromPointSet(IEnumerable<System.Drawing.Point> points)
        {
            int MAX_DIST = _step * _step * 4;
            var input = new List<System.Drawing.Point>(points);
            var output = new List<System.Drawing.Point>();

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

                input.RemoveAt(minIdx);
            }
            // exclude unclose conturs   //  it is a PATCH
            //if (Dist(output[0], output.Last()) >= MAX_DIST)
            //    return null;
            return output;

        }

        private static double Dist(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        #region Inspection

        private void ShowDots()
        {
            var comps = dots.OfType<Comp>().Distinct()
                //.Where(c => c.Count > 1)
                .ToArray();


            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Brown, Brushes.Orange };
            if (_step >= 10)
            {
                for (int xo = 0; xo * _step < _img.Width; xo++)
                {
                    for (int yo = 0; yo * _step < _img.Height; yo++)
                    {
                        //_g.FillRectangle(Brushes.Red, xo * _step, yo * _step, 1, 1);
                        var p = pots[xo, yo];
                        _g.FillRectangle(Brushes.Red, p.X, p.Y, 1, 1);
                        int id = comps.TakeWhile(c => c != dots[xo, yo]).Count(); 
                        Brush brush = brushes[id % brushes.Length];
                        _g.DrawString(id.ToString(), new Font("Courier", 6), brush, xo * _step, yo * _step);

                    }
                }
            }
        }

        private void ShowPoints()
        {
            foreach (var p in ypoints)
                _g.FillRectangle(Brushes.Red, p.X - 1, p.Y - 1, 3, 3);
        }

        #endregion

    }
}
