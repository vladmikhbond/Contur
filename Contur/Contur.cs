using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    class Contur
    {
        Bitmap img;
        Point startPoint;
        int step;


        int[,] dots;
        public List<Point> points;


        public Contur(Bitmap img, int step, Point startPoint)
        {
            this.img = img;
            this.step = step;
            this.startPoint = startPoint;
        }


        public static List<Point> Conturing(Bitmap image, int step, Point startPoint)
        {
            var contur = new Contur(image, step, startPoint);
            contur.PointsOnBorder();
            var ps = contur.MakeRegion();
            return ps;
        }


        /// input: step;    output: points, dots
        public List<Point> PointsOnBorder()
        {
            dots = new int[img.Width / step + 1, img.Height / step + 1];
            points = new List<Point>();
            int ox = startPoint.X / step;
            int oy = startPoint.Y / step;
            FloodFill(ox, oy);
            return points;
        }

        /// input: points     output: 
        /// 
        /// берем первую точку из входного массива и переносим в выходной
        /// находим во входном массиве ближайшую  к последней перенесенной и переносим ее в выходной
        /// продолжаем переносить, пока точки во входном массве не закончатся
        
        public List<Point> MakeRegion()
        {
            List<Point> input = new List<Point>(points);
            List<Point> output = new List<Point>();

            var last = input[0];
            output.Add(last);
            input.RemoveAt(0);

            while (input.Count > 0)
            {
                var dists = input.Select(p => dist(p, last));
                double minDist = dists.Min();
                int minIdx = dists.ToList().IndexOf(minDist);
                // слишком близкие точки пропускаем
                if (minDist > step * step / 4)
                {
                    last = input[minIdx];
                    output.Add(last);
                }
                input.RemoveAt(minIdx);
            }
            return output;

        }

        private static double dist(Point p1, Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }


        private bool IsOnBoard(int x, int y)
        {
            Color c = img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }

        /// Создание коллекци  контурных точек
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
            if (step * (ox + 1) < img.Width && dots[ox + 1, oy] == 0)
                SneakRight(ox, oy);
            if (oy > 0 && dots[ox, oy - 1] == 0)
                SneakUp(ox, oy);
            if (step * (oy + 1) < img.Height && dots[ox, oy + 1] == 0)
                SneakDown(ox, oy);
        }

        private void SneakLeft(int ox, int oy)
        {
            int x0 = step * ox, y0 = step * oy, x1 = x0 - step;
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
            int x0 = step * ox, y0 = step * oy, x1 = x0 + step;
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
            int x0 = step * ox, y0 = step * oy, y1 = y0 - step;
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
            int x0 = step * ox, y0 = step * oy, y1 = y0 + step;
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
