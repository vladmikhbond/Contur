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

        // for inspection only
        Graphics _g;
        TextBox _info;

        // output data (valide after call MakeAllConturs() )
        public ConComp[] Conturs { get; private set; }

        public YContur(Bitmap img, int step, Graphics g, TextBox info)
        {
            _img = img;
            _step = step;
            _g = g;
            _info = info;
            WO = _img.Width / _step + 1;
            HO = _img.Height / _step + 1;
        }

        public void Process()
        {
            InitDots();
            InitTots();
            FludFill();
            __DrawDots();
            WalkAroundConturs();

            //MakePoints();

            //__DrawPoints();

            //MakeAllConturs();
            //__DrawFails();
            //__ShowConComps();
        }

        private void WalkAroundConturs()
        {
            Conturs = dots.OfType<ConComp>()
                .Distinct()
                .Where(c => c != dots[0,0])
                .Where(c => c.Count > 1).ToArray();

            foreach (var comp in Conturs)
                WalkAroundContur(comp);                       
        }


        private void WalkAroundContur(ConComp comp)
        {
            // Found start point
            var startDot = comp.FirstOrDefault(p => p.X > 0 && p.Y > 0 && dots[p.X - 1, p.Y] != comp);
            int xo = startDot.X, yo = startDot.Y;
            int curDir = 0;
            do
            {
                comp.Points.Add(new Point(xo * _step, yo * _step));  // temp

                // try turn right
                if (dots[xo + 1, yo] == comp && dots[xo, yo - 1] != comp)
                {
                    xo = xo + 1;
                    yo = yo;
                    curDir = (curDir + 1) % 4;
                }
                // try go ahead
                else if (dots[xo, yo - 1] == comp && dots[xo - 1, yo] != comp)
                {
                    xo = xo;
                    yo = yo - 1;
                }
                // try turn left
                else if (dots[xo - 1, yo] == comp && dots[xo - 1, yo + 1] != comp)
                {
                    xo = xo - 1;
                    yo = yo;
                }


            } while (xo != startDot.X || yo != startDot.Y);

              
        }

        // Создает разделительные точки, одновременно распределяя их по контурам.
        //
        private void MakePoints()
        {
            for (int yo = 1; yo < dots.GetLength(1); yo++)
            {
                for (int xo = 1; xo < dots.GetLength(0); xo++)
                {
                    Point? pLeft = BrezenhamPath(tots[xo, yo], tots[xo - 1, yo]);
                    if (pLeft != null)
                    {
                        // создаем точки между реперами dots[xo, yo] и dots[xo - 1, yo]
                        dots[xo, yo].Points.Add((Point)pLeft);
                        dots[xo - 1, yo].Points.Add((Point)pLeft);
                    }
                    Point? pUp = BrezenhamPath(tots[xo, yo], tots[xo, yo - 1]);
                    if (pUp != null)
                    {
                        // создаем точки между реперами dots[xo, yo] и dots[xo, yo - 1]
                        dots[xo, yo].Points.Add((Point)pUp);
                        dots[xo, yo - 1].Points.Add((Point)pUp);
                    }
                }
            }
        }

        // Сборка контуров
        //
        List<Point[]> MakeAllConturs()
        {
            Conturs = dots.OfType<ConComp>().Distinct().Where(c => c.Count > 1).ToArray();

            var conturs = new List<Point[]>();
            foreach (var comp in Conturs)
            {
                comp.PointsInOrder(_step);              
                conturs.Add(comp.Points.ToArray());
            }
            return conturs;
        }

        // Определение компонент связности путем заливки
        //
        void FludFill()
        {
            for (int yo = 1; yo < dots.GetLength(1); yo++)
            {
                for (int xo = 1; xo < dots.GetLength(0); xo++)
                {
                    Point? pLeft = BrezenhamPath(tots[xo, yo], tots[xo - 1, yo]);
                    Point? pUp = BrezenhamPath(tots[xo, yo], tots[xo, yo - 1]);
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
                    }
                    else if (pUp != null && pLeft == null)
                    {
                        // красим в левый цвет
                        dots[xo, yo] = dots[xo - 1, yo];
                        dots[xo, yo].Add(new Point(xo, yo));
                    }
                    else  // x != -1 && y != -1
                    {
                        // новая компонента связности
                        dots[xo, yo] = new ConComp(new Point(xo, yo));
                    }
                }
            }
        }

        // init dots array
        //
        private void InitDots()
        {
            dots = new ConComp[WO, HO];
            ConComp zero = new ConComp(WO, HO);
            for (int xo = 0; xo < WO; xo++)
                dots[xo, 0] = zero;
            for (int yo = 0; yo < HO; yo++)
                dots[0, yo] = zero;
        }

        // Коррекция реперных точек (вариант с двумя направлениями: вверх и влево)
        //
        private void InitTots()
        {
            tots = new Point[WO, HO];
            for (int xo = 0; xo < WO; xo++)
            {
                int x = xo * _step;
                for (int yo = 0; yo < HO; yo++)
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

        // На отрезке, соединяющем точки p0 и p1, находим усредненный черный пиксель
        // или null, если черных пикселей нет
        //
        Point? BrezenhamPath(Point p0, Point p1)
        {
            int deltax = Math.Abs(p1.X - p0.X);
            int deltay = Math.Abs(p1.Y - p0.Y);

            // change X and Y to make horizontal longer then vertical
            bool b45 = deltax < deltay;
            if (b45)
            {
                int t = p0.X; p0.X = p0.Y; p0.Y = t;
                t = p1.X; p1.X = p1.Y; p1.Y = t;
                t = deltax; deltax = deltay; deltay = t;
            };

            int error = 0;
            int deltaerr = deltay;
            int y = p0.Y;
            int dx = Math.Sign(p1.X - p0.X);
            int dy = Math.Sign(p1.Y - p0.Y);

            // для усреднения черных точек 
            int blackSumX = 0;
            int blackSumY = 0;
            int blackCount = 0;

            for (int x = p0.X; x != p1.X; x += dx)
            {
                if (b45)
                {
                    if (IsBlack(y, x))
                    {
                        //return new Point(y, x);
                        blackSumX += y;
                        blackSumY += x;
                        blackCount++;
                    }
                }
                else
                {
                    if (IsBlack(x, y))
                    {
                        //return new Point(x, y);
                        blackSumX += x;
                        blackSumY += y;
                        blackCount++;
                    }
                }
                error += deltaerr;
                if (2 * error >= deltax)
                {
                    y += dy;
                    error -= deltax;
                }
            }
            if (blackCount > 0)
                return new Point(blackSumX / blackCount, blackSumY / blackCount);
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

        public IEnumerable<Point[]> ContursAroundPoint(Point p)
        {
            return Conturs
                .Where(c => IsInside(c.Points, p))
                .OrderBy(c => c.Count())
                .Select(c => c.Points.ToArray());
        }

        static bool IsInside(IList<Point> ps, Point p)
        {
            bool result = false;
            int j = ps.Count() - 1;
            for (int i = 0; i < ps.Count(); i++)
            {
                if (ps[i].Y < p.Y && ps[j].Y >= p.Y || ps[j].Y < p.Y && ps[i].Y >= p.Y)
                {
                    if (ps[i].X + (p.Y - ps[i].Y) / (ps[j].Y - ps[i].Y) * (ps[j].X - ps[i].X) < p.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }


        #region For Inspection

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
            var points = dots.OfType<ConComp>().Distinct().SelectMany(c => c.Points).Distinct();
            foreach (var p in points)
                _g.FillRectangle(Brushes.Yellow, p.X, p.Y, 1, 1);
        }

        private void __DrawFails()
        {
            var points = dots.OfType<ConComp>().Distinct().SelectMany(c => c.Fails).Distinct();
            foreach (var p in points)
                _g.DrawArc(Pens.Blue, p.X-10, p.Y-10, 20, 20, 0, 360);
        }

        private void __ShowConComps()
        {
            var comps = dots.OfType<ConComp>().Distinct().ToArray();
            var counts = comps.Select(c => $"{c.Count()} / {c.Fails.Count}");
            _info.Text = $"All={counts.Count()}\r\n" + string.Join("\r\n", counts);      
        }

        #endregion

    }
}
