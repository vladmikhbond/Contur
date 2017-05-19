using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    // export this[int], ctor(string fname),  ctor(List<Point[]> list),  SaveToFile(fname), ContursAroundPoint(Point)
    public class ConturList
    {
        List<Point[]> _list;

        // Create this from file
        //
        public ConturList(List<Point[]> list)
        {
            this._list = list;
        }

        public ConturList(string fname)
        {
            var lines = File.ReadAllLines(fname);
            _list = new List<Point[]>();
            foreach (string line in lines)
            {
                var nums = line.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                var ps = nums.Select((x, i) => i % 2 == 1 ? new Point(nums[i - 1], x) : new Point(-1, -1)).Where(p => p.X != -1);
                _list.Add(ps.ToArray());
            }
        }

        public List<Point[]> List
        {
            get { return _list; }
        }

        public IEnumerable<Point[]> GetEnumerator()
        {
            foreach (var l in _list)
                yield return l;
        }



        public void SaveToFile(string fname)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var contur in _list)
            {
                sb.AppendLine(string.Join(",", contur.Select(c => $"{c.X},{c.Y}")));
            }
            File.WriteAllText(fname, sb.ToString());
        }


        public IEnumerable<Point[]> ContursAroundPoint(Point p)
        {
            return _list.Where(c => IsInside(c, p)).OrderBy(c => c.Count());
        }

        public int ConturIdxAroundPoint(Point p)
        {
            return _list.TakeWhile(c => !IsInside(c, p)).Count();
        }

        static bool IsInside(Point[] ps, Point p)
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
