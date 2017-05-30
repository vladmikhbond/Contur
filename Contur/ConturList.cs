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
        public List<System.Drawing.Point[]> List;

        // Create this from file
        //
        public ConturList(List<System.Drawing.Point[]> list)
        {
            List = list;
        }

        public ConturList(string fname)
        {
            var lines = File.ReadAllLines(fname);
            List = new List<System.Drawing.Point[]>();
            foreach (string line in lines)
            {
                var nums = line.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                var ps = nums.Select((x, i) => i % 2 == 1 ? new Point(nums[i - 1], x) : new Point(-1, -1)).Where(p => p.X != -1);
                List.Add(ps.ToArray());
            }
        }

 
        public void SaveToFile(string fname)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var contur in List)
            {
                sb.AppendLine(string.Join(",", contur.Select(c => $"{c.X},{c.Y}")));
            }
            File.WriteAllText(fname, sb.ToString());
        }


        public IEnumerable<System.Drawing.Point[]> ContursAroundPoint(System.Drawing.Point p)
        {
            return List.Where(c => IsInside(c, p)).OrderBy(c => c.Count());
        }

        public int ConturIdxAroundPoint(System.Drawing.Point p)
        {
            return List.TakeWhile(c => !IsInside(c, p)).Count();
        }

        static bool IsInside(System.Drawing.Point[] ps, System.Drawing.Point p)
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
