using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class ConturHelper
    {
        Dictionary<string, Point[]> Conturs;

        public ConturHelper(string fileName)
        {
            Conturs = LoadConturs(fileName);
        }

        public Point[] this[string key]
        {
            get { return Conturs[key];  }
        }

        public static  Dictionary<string, Point[]> LoadConturs(string pointsFileName)
        {
            try
            {
                return File.ReadAllLines(pointsFileName)
                    .Select(line => line.Split(':'))
                    .Where(ss => ss.Length == 2)
                    .Select(ss => new { key = ss[0].Trim(), val = TextToPointArray(ss[1]) })
                    .ToDictionary(v => v.key, v => v.val);
            }
            catch
            {
                return new Dictionary<string, Point[]>();
            }
        }

        // text is a string like "22,22,33,33,44,44,"
        static Point[] TextToPointArray(string text, int m = 1)
        {
            var ss = text.Split(',');
            var v = new List<Point>();

            for (int i = 0; i < ss.Length - 1; i += 2)
            {
                int x = Convert.ToInt32(ss[i]);
                int y = Convert.ToInt32(ss[i + 1]);
                v.Add(new Point(x * m, y * m));
            }
            return v.ToArray();
        }

        public string GetPoligonePointInto(Point p)
        {
            foreach (var pair in Conturs)
            {
                if (IsInside(pair.Value, p))
                    return pair.Key;
            }
            return null;
        }

        public static bool IsInside(Point[] ps, Point p)
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


    }
}
