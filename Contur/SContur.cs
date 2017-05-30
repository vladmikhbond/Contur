using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    public class SContur 
    {
        Bitmap _img;
        int _step;
        List<System.Drawing.Point> _points;

        public SContur(Bitmap img, int step)
        {
            _img = img;
            _step = step;
        }

        public System.Drawing.Point[] Points
        {
            get { return _points.ToArray(); }
        }

        public List<System.Drawing.Point[]> MakeAllConturs()
        {        
            CreatePoints();
            var conturs = new List<System.Drawing.Point[]>();

            int MAX_DIST = _step * _step * 2;
            var input = new List<System.Drawing.Point>(_points);
            System.Drawing.Point[] contur;
            while ((contur = GetOneContur(input)) != null && input.Count > 0)
                conturs.Add(contur);
            return conturs;
        }

        private System.Drawing.Point[] GetOneContur(List<System.Drawing.Point> input)
        {
            int MAX_DIST = _step * _step * 2;
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
                else
                {
                    break;
                }
                input.RemoveAt(minIdx);
            }
            return output.Count > 0 ? output.ToArray() : null;
        }

        private void CreatePoints()
        {
            _points = new List<System.Drawing.Point>();

            for (int y = 0; y < _img.Height; y += _step)
                for (int x = 0; x < _img.Width; x++)
                    if (IsOnBoard(x, y))
                    {
                        _points.Add(new System.Drawing.Point(x, y));
                        x += _step;
                    }

            for (int x = 0; x < _img.Width; x += _step)
                for (int y = 0; y < _img.Height; y++)
                    if (IsOnBoard(x, y))
                    {
                        _points.Add(new System.Drawing.Point(x, y));
                        y += _step;
                    }
        }

        private static double Dist(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            int dx = p1.X - p2.X, dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        // Test if the point is on a board
        //
        private bool IsOnBoard(int x, int y)
        {
            if (x >= _img.Width || y >= _img.Height)
                return false;
            Color c = _img.GetPixel(x, y);
            return c.R < 255 && c.G < 255 && c.B < 255;
        }


    }
}
