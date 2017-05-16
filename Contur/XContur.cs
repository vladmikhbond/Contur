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
        //const int MIN_POINTS_IN_CONTUR = 8;
        Bitmap _img;
        int _step;

        int[,] dots;         // разведочная сетка : 0 - empty, 1,2,3... - contur numbers
        int dotСhrome;       // "цвет" закрашенных точек
        //List<Point> points;  // точки для рекурсивной процедуры

        public XContur(Bitmap img, int step)
        {
            _img = img;
            _step = step;
            dots = new int[_img.Width / _step + 1, _img.Height / _step + 1];
            dotСhrome = 0;
        }

        public void FludFill()
        {
            for (int xo = 0; xo < dots.GetLength(0); xo++)
                for (int yo = 0; yo < dots.GetLength(1); yo++)
                    if (dots[xo, yo] == 0)
                        WorkAround(xo, yo);

        }

        private void WorkAround(int xo, int yo)
        {
            // left neighbor is painted 
            if (xo > 0 && dots[xo - 1, yo] != 0)
            {
                if (PathToLeft(xo, yo) == -1)
                    PaintDot(xo, yo, xo - 1, yo);
            }
            // right neighbor is painted 
            else if (xo < dots.GetLength(0) - 1 && dots[xo + 1, yo] != 0)
            {
                if (PathToLeft(xo, yo) == -1)
                    PaintDot(xo, yo, xo + 1, yo);
            }
            // upper neighbor is painted 
            else if (yo > 0 && dots[xo, yo - 1] != 0)
            {
                if (PathToUp(xo, yo) == -1)
                    PaintDot(xo, yo, xo, yo - 1);
            }
            // down neighbor is painted 
            else if (yo < dots.GetLength(1) - 1 && dots[xo, yo + 1] != 0)
            {
                if (PathToDown(xo, yo) == -1)
                    PaintDot(xo, yo, xo, yo + 1);
            }
            // no neighbor is painted
            else
            {
                dotСhrome++;
                dots[xo, yo] = dotСhrome;

            }
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
            int x1 = xo * _step, x2 = (xo + 1) * _step, y = yo * _step;
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
            int y1 = yo * _step,y2 = (yo + 1) * _step, x = xo * _step;
            for (int y = y1; y < y2; y++)
                if (IsOnBoard(x, y))
                    return y;
            return -1;
        }


        private void PaintDot(int xo, int yo, int xo1, int yo1)
        {
            if (dots[xo, yo] == 0)
                dots[xo, yo] = dots[xo1, yo1];
            else
                RepaintDots(dots[xo1, yo1], dots[xo, yo]);
        }

        private void RepaintDots(int chrome1, int chrome2)
        {
            for (int xo = 0; xo < dots.GetLength(0); xo++)
                for (int yo = 0; yo < dots.GetLength(1); yo++)
                    if (dots[xo, yo] == chrome1)
                        dots[xo, yo] = chrome2;
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
