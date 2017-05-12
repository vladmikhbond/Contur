using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    /// <summary>
    /// 
    /// </summary>
    public class Contur
    {
        Bitmap _img;
        int _step;


        int[,] dots;         // разведочная сетка : 0 - empty, 1-visited
        List<Point> points;  // точки для рекурсивной процедуры


        private Contur(Bitmap img, int step)
        {
            _img = img;
            _step = step;
            dots = new int[_img.Width / _step + 1, _img.Height / _step + 1];
        }

        public static List<Point[]> GetAllConturs(Bitmap image, int step)
        {
            List<Point[]> conturs = new List<Point[]>();
            var contur = new Contur(image, step);
            foreach (var t in contur.GetEmptyDots())
            {
                var startPoint = new Point(t.Item1 * step, t.Item2 * step);
                var ps = contur.SetPointsOnBorder(startPoint);
                if (ps.Count > 3)
                {
                    var ps2 = contur.MakeContur(ps, thinOut: false);
                    conturs.Add(ps2.ToArray());
                }
            }
            return conturs;
        }

        private IEnumerable<Tuple<int, int>> GetEmptyDots()
        {
            for (int xo = 1; xo < dots.GetLength(0) - 1; xo++)
                for (int yo = 1; yo < dots.GetLength(1) - 1; yo++)
                    if (dots[xo, yo] == 0)
                        yield return Tuple.Create(xo, yo);
        }


        /// <summary>
        /// Создает один контур вокруг заданной точки
        /// </summary>
        /// <param name="image">битмэп</param>
        /// <param name="step">размер ячейки разведочной сетки</param>
        /// <param name="startPoint">точка, вокруг которой...</param>
        /// <returns></returns>
        public static Point[] Conturing(Bitmap image, int step, Point startPoint)
        {
            var contur = new Contur(image, step);
            var ps = contur.SetPointsOnBorder(startPoint);
            ps = contur.MakeContur(ps, thinOut: true);
            return ps.ToArray();
        }

        /// внешние имена: вход: step;    выход: points, dots
        /// <summary>
        /// Ставит точки на линии, окружающей заданную точку
        /// </summary>
        /// <param name="startPoint">заданная точка</param>
        /// <returns></returns>
        List<Point> SetPointsOnBorder(Point startPoint)
        {
            points = new List<Point>();
            int ox = startPoint.X / _step;
            int oy = startPoint.Y / _step;
            FloodFill(ox, oy);
            return points;
        }

        /// создает контур из неупорядоченного множества точек
        /// input: points     output: 
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        /// 
        List<Point> MakeContur(List<Point> points, bool thinOut=false)
        {
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
                // слишком близкие точки пропускаем
                if (!thinOut || minDist > _step * _step / 4)
                {
                    last = input[minIdx];
                    output.Add(last);
                }
                input.RemoveAt(minIdx);
            }
            return output;

        }

        private static double Dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        // Test if the point is on a board
        //
        private bool IsOnBoard(int x, int y)
        {
            Color c = _img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }

        /// Создание коллекци точек, расположенных на линии, окружающей точку (ox, oy)
        /// 
        /// красим точку в 1
        /// проверяем левого соседа. 
        /// если он существует и цвета 0, то крадемся к нему по пикселям
        /// если по пути встречаем зеленый пиксель, то создаем контурную точку на месте зеленого пикселя
        /// если зеленый пиксель не встретился, вызываем FloodFill на левом соседе.
        /// -- так же проверяем правого, верхнего и нихнего соседей
        void FloodFill(int ox, int oy)
        {
            dots[ox, oy] = 1;
            if (ox > 0 && dots[ox - 1, oy] == 0)
                SneakLeft(ox, oy);
            if (_step * (ox + 1) < _img.Width && dots[ox + 1, oy] == 0)
                SneakRight(ox, oy);
            if (oy > 0 && dots[ox, oy - 1] == 0)
                SneakUp(ox, oy);
            if (_step * (oy + 1) < _img.Height && dots[ox, oy + 1] == 0)
                SneakDown(ox, oy);
        }

        private void SneakLeft(int ox, int oy)
        {
            int x0 = _step * ox, y0 = _step * oy, x1 = x0 - _step;
            for (int x = x0; x > x1; x--)
            {
                if (IsOnBoard(x, y0))
                {
                    points.Add(new Point(x, y0));
                    return;
                }
            }
            FloodFill(ox - 1, oy);
        }

        private void SneakRight(int ox, int oy)
        {
            int x0 = _step * ox, y0 = _step * oy, x1 = x0 + _step;
            for (int x = x0; x < x1; x++)
            {
                if (IsOnBoard(x, y0))
                {
                    points.Add(new Point(x, y0));
                    return;
                }
            }
            FloodFill(ox + 1, oy);
        }

        private void SneakUp(int ox, int oy)
        {
            int x0 = _step * ox, y0 = _step * oy, y1 = y0 - _step;
            for (int y = y0; y > y1; y--)
            {
                if (IsOnBoard(x0, y))
                {
                    points.Add(new Point(x0, y));
                    return;
                }
            }
            FloodFill(ox, oy - 1);
        }

        private void SneakDown(int ox, int oy)
        {
            int x0 = _step * ox, y0 = _step * oy, y1 = y0 + _step;
            for (int y = y0; y < y1; y++)
            {
                if (IsOnBoard(x0, y))
                {
                    points.Add(new Point(x0, y));
                    return;
                }
            }
            FloodFill(ox, oy + 1);
        }

    }
}
