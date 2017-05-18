using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Алгоритм закраски
//
// 1. Находим первую незакрашенную точку, идя слева направо, сверху вниз.
// 2. Проверяем всех закрашенных соседей.Если среди них находим смежного, красим точку в тот же цвет. 
// Если все закрашенные соседи изолированы, красим точку в новый цвет (и создаем парные цветные точки контуров в местах изоляции).
// Если есть два смежных разноцветных закрашенных соседа, перекрашиваем все точки младшего цвета в тот цвет, который старше(его номер меньше).
//
// Собираем контуры из одноцветных закрашенных контурных точек.

namespace Contur
{
    /// <summary>
    /// Класс для изготовления контуров на основе монохромного контурного изображения.
    /// 
    /// public static:  GetAllConturs(img, step), GetOneContur(img, step, point),  
    /// </summary>
    public class XContur
    {
        const int MIN_POINTS_IN_CONTUR = 8;
        Bitmap _img;
        int _step;

        public int[,] dots;         // scout net : 0 - empty, 1,2,3... - contur chromes
        public int dotСhrome;       // "цвет" закрашенных точек
        public List<CPoint> cpoints; // общая коллекция цветных точек 

        public XContur(Bitmap img, int step)
        {
            _img = img;
            _step = step;

        }

        public List<Point[]> GetAllConturs()
        {
            List<Point[]> conturs = new List<Point[]>();
            for (int chrome = 1; chrome <= dotСhrome; chrome++)
            {
                var cps = cpoints
                    .Where(p => p.Chrome == chrome)
                    .Select(p => p.P);
                if (cps.Count() >= MIN_POINTS_IN_CONTUR)
                {
                    var ps = MakeConturFromPointSet(cps);
                    if (ps != null)
                       conturs.Add(ps.ToArray());
                }
            }
            return conturs;
        }

        /// создает контур из неупорядоченного множества точек
        /// input: points     output: 
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        /// 
        List<Point> MakeConturFromPointSet(IEnumerable<Point> points)
        {
            int MIN_DIST = _step * _step / 2;
            int MAX_DIST = _step * _step * 9;
            List<Point> input = new List<Point>(points);
            List<Point> output = new List<Point>();

            var last = input[0];
            output.Add(last);
            input.RemoveAt(0);

            while (input.Count > 0)
            {
                var dists = input.Select(p => Dist(p, last));
                double minDist = dists.Min();
                int minIdx = dists.ToList().IndexOf(minDist);
                // слишком далекие точки пропускаем
                if (minDist < MAX_DIST)
                {
                    last = input[minIdx];
                    output.Add(last);
                } else
                {

                }
                input.RemoveAt(minIdx);
            }
            // exclude unclose conturs   //  it is a PATCH
            if (Dist(output[0], output.Last()) >= MAX_DIST)
                return null;
            return output;

        }

        private static double Dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }


        public void FludFill()
        {
            cpoints = new List<CPoint>();
            dots = new int[_img.Width / _step, _img.Height / _step];
            dotСhrome = 0;

            for (int xo = 0; xo < dots.GetLength(0); xo++)
                for (int yo = 0; yo < dots.GetLength(1); yo++)
                    if (dots[xo, yo] == 0)
                        WorkAround(xo, yo);
        }

        private void WorkAround(int xo, int yo)
        {
            var locals = new List<CPoint>();

            // left neighbor is painted 
            if (xo > 0 && dots[xo - 1, yo] != 0)
            {
                int x = PathToLeft(xo, yo);
                if (x == -1)
                    PaintDot(xo, yo, dots[xo - 1, yo]);
                else
                    locals.Add(new CPoint(x, yo * _step, dots[xo - 1, yo]));
            }

            // right neighbor is painted 
            if (xo < dots.GetLength(0) - 1 && dots[xo + 1, yo] != 0)
            {
                int x = PathToRight(xo, yo);
                if ( x == -1)
                    PaintDot(xo, yo, dots[xo + 1, yo]);
                else
                    locals.Add(new CPoint(x, yo * _step, dots[xo + 1, yo]));
            }

            // upper neighbor is painted 
            if (yo > 0 && dots[xo, yo - 1] != 0)
            {
                int y = PathToUp(xo, yo);
                if (y == -1)
                    PaintDot(xo, yo, dots[xo, yo - 1]);
                else
                    locals.Add(new CPoint(xo * _step, y, dots[xo, yo - 1]));
            }

            // down neighbor is painted 
            if (yo < dots.GetLength(1) - 1 && dots[xo, yo + 1] != 0)
            {
                int y = PathToDown(xo, yo);
                if (y == -1)
                    PaintDot(xo, yo, dots[xo, yo + 1]);
                else
                    locals.Add(new CPoint(xo * _step, y, dots[xo, yo + 1]));
            }

            // no neighbor is painted, paint a dot in the next chrome
            if (dots[xo, yo] == 0)
            {
                dotСhrome++;
                dots[xo, yo] = dotСhrome;
            }

            // add local cpoints to common collection
            cpoints.AddRange(locals);
            cpoints.AddRange(locals.Select(p => new CPoint(p.P, dots[xo, yo])));

        }

        private int PathToLeft(int xo, int yo)
        {
            int x1 = xo * _step, x2 = (xo - 1) * _step, y = yo * _step;
            for (int x = x1; x > x2; x--)
                if (IsOnBoard(x, y))
                    return x;
            return -1;
        }

        private int PathToRight(int xo, int yo)
        {
            int x1 = xo * _step, x2 = Math.Min((xo + 1) * _step, _img.Width - 1), y = yo * _step;
            for (int x = x1; x < x2; x++)
                if (IsOnBoard(x, y))
                    return x;
            return -1;
        }

        private int PathToUp(int xo, int yo)
        {
            int y1 = yo * _step, y2 = (yo - 1) * _step, x = xo * _step;
            for (int y = y1; y > y2; y--)
                if (IsOnBoard(x, y))
                    return y;
            return -1;
        }

        private int PathToDown(int xo, int yo)
        {
            int y1 = yo * _step, y2 = Math.Min((yo + 1) * _step, _img.Height - 1), x = xo * _step;
            for (int y = y1; y < y2; y++)
                if (IsOnBoard(x, y))
                    return y;
            return -1;
        }


        private void PaintDot(int xo, int yo, int chrome)
        {
            if (dots[xo, yo] == 0)
                dots[xo, yo] = chrome;
            else
                RepaintDots(chrome, dots[xo, yo]);
        }

        private void RepaintDots(int chrome1, int chrome2)
        {
            for (int xo = 0; xo < dots.GetLength(0); xo++)
                for (int yo = 0; yo < dots.GetLength(1); yo++)
                    if (dots[xo, yo] == chrome1)
                        dots[xo, yo] = chrome2;
            for (int i = 0; i < cpoints.Count; i++)
                if (cpoints[i].Chrome == chrome1)
                    cpoints[i].Chrome = chrome2;
        }



        // Test if the point is on a board
        //
        private bool IsOnBoard(int x, int y)
        {
            Color c = _img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }

    }
}
