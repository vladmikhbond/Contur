using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


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
        // input data
        Bitmap _img;
        int _step;

        ConComp[,] dots;     // scout net : 0 - empty, 1,2,3... - contur chromes
        Point[,] tots;
        int WO, HO;          // width and height jf the dots and tots
        List<Point> points; // общая коллекция цветных точек 

        // for inspection only
        Graphics _g;
        TextBox _info;

        public YContur(Bitmap img, int step, Graphics g, TextBox info)
        {
            _img = img;
            _step = step;
            _g = g;
            _info = info;
            WO = _img.Width / _step + 1;
            HO = _img.Height / _step + 1;
        }

        public List<Point[]> Process()
        {
            FludFill();
            __DrawDots();
            __ShowConComps();

            return MakeAllConturs();
        }

        List<Point[]> MakeAllConturs()
        {
            var conturs = new List<Point[]>();

            foreach (var p in points)
            {
                int ox = p.X / _step;
                int oy = p.Y / _step;

                // точка в первый смежный контур
                dots[ox, oy].Points.Add(p);

                // точка во второй смежный контур
                if (ox * _step == p.X && oy + 1 < dots.GetLength(1))
                {
                    // точка лежит на вертикали
                    dots[ox, oy + 1].Points.Add(p);
                }
                if (oy * _step == p.Y && ox + 1 < dots.GetLength(0))
                {
                    // точка лежит на горизонтали
                    dots[ox + 1, oy].Points.Add(p);
                }
            }

            var comps = dots.OfType<ConComp>().Distinct().Where(c => c.Count > 1).ToArray();
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
            points = new List<Point>();

            // init dots array
            dots = new ConComp[WO, HO];
            ConComp zero = new ConComp(WO, HO);
            for (int xo = 0; xo < WO; xo++)
                dots[xo, 0] = zero;
            for (int yo = 0; yo < HO; yo++)
                dots[0, yo] = zero;

            InitTots(WO, HO);


            for (int yo = 1; yo < dots.GetLength(1); yo++)
            {
                for (int xo = 1; xo < dots.GetLength(0); xo++)
                {
                    Point? pUp = PathToUp(xo, yo);
                    Point? pLeft = PathToLeft(xo, yo);
                    if (pUp == null && pLeft == null)
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
                    else if (pUp == null && pLeft != null)
                    {
                        // красим в верхний цвет
                        dots[xo, yo] = dots[xo, yo - 1];
                        dots[xo, yo].Add(new Point(xo, yo));
                        // создаем точку слева
                        points.Add((Point)pLeft);
                    }
                    else if (pUp != null && pLeft == null)
                    {
                        // красим в левый цвет
                        dots[xo, yo] = dots[xo - 1, yo];
                        dots[xo, yo].Add(new Point(xo, yo));
                        // создаем точку сверху
                        points.Add((Point)pUp);
                    }
                    else  // x != -1 && y != -1
                    {
                        // новая компонента связности
                        dots[xo, yo] = new ConComp();
                        dots[xo, yo].Add(new Point(xo, yo));
                        // создаем точку слева
                        points.Add((Point)pLeft);
                        // создаем точку сверху
                        points.Add((Point)pUp);
                    }
                }
            }
        }

        // Инициализация реперных точек (контрольный вариант - без смещения)
        //
        private void InitTots0(int wo, int ho)
        {
            tots = new Point[wo, ho];
            for (int xo = 0; xo < wo; xo++)
            {
                int x = xo * _step;
                for (int yo = 0; yo < ho; yo++)
                {
                    int y = yo * _step;
                    tots[xo, yo] = new Point(x, y);
                }
            }
        }

        // Инициализация реперных точек (вариант с двумя направлениями: вверх и влево)
        //
        private void InitTots(int wo, int ho)
        {
            tots = new Point[wo, ho];
            for (int xo = 0; xo < wo; xo++)
            {
                int x = xo * _step;
                for (int yo = 0; yo < ho; yo++)
                {
                    int y = yo * _step;
                    tots[xo, yo] = new Point(x, y);
                    for (int d = 0; d < _step; d++)
                    {
                        if (!IsBlack(x - d, y))
                        {
                            tots[xo, yo] = new Point(x - d, y);
                            break;
                        }
                        if (!IsBlack(x, y - d))
                        {
                            tots[xo, yo] = new Point(x, y - d);
                            break;
                        }
                    }
                }
            }
        }

        // Инициализация реперных точек (вариант с диагональным растром)
        //
        private void InitTots1(int wo, int ho)
        {
            tots = new Point[wo, ho];
            int[,] dxy = { { 0, 0 },
                { 0, 1 }, { 1, 0 },
                { 0, 2 }, { 1, 1 }, { 2, 0 },
                { 0, 3}, { 1, 2 }, { 2, 1 }, { 3, 0 },
                { 0, 4}, { 1, 3 }, { 2, 2 }, { 3, 1 }, { 4, 0 },
                { 0, 5}, { 1, 4 }, { 2, 3 }, { 3, 2 }, { 4, 1 }, { 5, 0 },
                { 0, 6}, { 1, 5 }, { 2, 4 }, { 3, 3 }, { 4, 2 }, { 5, 1 }, { 6, 0 },
            };
            for (int xo = 0; xo < wo; xo++)
            {
                for (int yo = 0; yo < ho; yo++)
                {
                    int x = xo * _step;
                    int y = yo * _step;
                    int dx = 0, dy = 0;
                    for (int a = 0; a < dxy.Length / 2; a++)
                    {
                        dx = dxy[a, 0];
                        dy = dxy[a, 1];
                        if (!IsBlack(x - dx, y - dy))
                            break;
                    }
                    tots[xo, yo] = new Point(x - dx, y - dy);
                }
            }

        }


        // Движемся слева направо и сверху вниз. Угол наклона <= 45 град.
        // Если нет черной точки, возвращаем null
        //
        Point? LeftWay(int xo, int yo)
        {
            Point p2 = tots[xo, yo];
            Point p1 = tots[xo - 1, yo];
            int deltax = Math.Abs(p1.X - p2.X);
            int deltay = Math.Abs(p1.Y - p2.Y);

            int error = 0;
            int deltaerr = deltay;
            int y = p2.Y;
            for (int x = p2.X; x < p1.X; x++)
            {
                if (IsBlack(x, y))
                    return new Point(x, y);
                error += deltaerr;
                if (2 * error >= deltax)
                    y--;
                error = error - deltax;
            }
            return null;
        }

        // Движемся слева направо и сверху вниз. Угол наклона <= 45 град.
        // Если нет черной точки, возвращаем null
        //
        Point? UpWay(int xo, int yo)
        {
            Point p2 = tots[xo, yo];
            Point p1 = tots[xo, yo - 1];
            int deltax = Math.Abs(p1.X - p2.X);
            int deltay = Math.Abs(p1.Y - p2.Y);

            int error = 0;
            int deltaerr = deltax;
            int x = p2.X;
            for (int y = p2.Y; y < p1.Y; y++)
            {
                if (IsBlack(x, y))
                    return new Point(x, y);
                error += deltaerr;
                if (2 * error >= deltay)
                    x--;
                error = error - deltay;
            }
            return null;
        }

        private Point? PathToLeft(int xo, int yo)
        {
            Point p1 = tots[xo, yo];
            Point p2 = tots[xo - 1, yo];
            int y1 = Math.Min(p1.Y, p2.Y);
            int y2 = Math.Max(p1.Y, p2.Y);

            for (int x = p1.X; x >= p2.X; x--)
                if (IsBlack(x, y1))
                    return new Point(x, y1);
            for (int y = y1; y <= y2; y++)
                if (IsBlack(p2.X, y))
                    return new Point(p2.X, y1);
            return null;
        }

        private Point? PathToUp(int xo, int yo)
        {
            Point p1 = tots[xo, yo];
            Point p2 = tots[xo, yo - 1];
            int x1 = Math.Min(p1.X, p2.X);
            int x2 = Math.Max(p1.X, p2.X);

            for (int y = p1.Y; y > p2.Y; y--)
                if (IsBlack(x1, y))
                    return new Point(x1, y);
            for (int x = x1; x <= x2; x++)
                if (IsBlack(x, p2.Y))
                    return new Point(x, p2.Y);
            return null;
        }

        // Test if a point is on board
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
        List<Point> MakeConturFromPointSet(IEnumerable<Point> points)
        {
            int MAX_DIST = _step * _step * 4;
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

        #region Inspection

        private void __DrawDots()
        {
            var comps = dots.OfType<ConComp>().Distinct()
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
                        var p = tots[xo, yo];
                        _g.FillRectangle(Brushes.Red, p.X, p.Y, 1, 1);
                        int id = comps.TakeWhile(c => c != dots[xo, yo]).Count(); 
                        Brush brush = brushes[id % brushes.Length];
                        _g.DrawString(id.ToString(), new Font("Courier", 6), brush, xo * _step, yo * _step);

                    }
                }
            }
        }

        private void __DrawPoints()
        {
            foreach (var p in points)
                _g.FillRectangle(Brushes.Red, p.X - 1, p.Y - 1, 3, 3);
        }

        private void __ShowConComps()
        {
            var comps = dots.OfType<ConComp>().Distinct().ToArray();
            var counts = comps.Select(c => c.Count().ToString());
            _info.Text = $"All={counts.Count()}\r\n" + string.Join("\r\n", counts);      
        }

        #endregion

    }
}
