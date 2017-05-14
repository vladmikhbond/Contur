using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contur
{
    /// <summary>
    /// Класс для работы с готовым словарем контуров.
    /// 
    /// public:  ctor, this[string key], Save(), GetPoligonePointInto()
    /// </summary>
    public class Conturs
    {
        const string FILE_NAME = "conturs.txt";

        Dictionary<string, Point[]> dict;

        public Conturs()
        {
            Load();
        }

        public Point[] this[string key]
        {
            get { return dict[key]; }
            set { dict[key] = value; }
        }

        public IEnumerable<string> Keys
        {
            get { return dict.Keys; }
        }

        public void Delete(string key)
        {
            dict.Remove(key);
        }

        void Load()
        {
            if (!File.Exists(FILE_NAME))
            {
                dict = new Dictionary<string, Point[]>();
            }
            else
            {
                dict = File.ReadAllLines(FILE_NAME)
                    .Select(line => line.Split(':'))
                    .Where(ss => ss.Length == 2)
                    .Select(ss => new { key = ss[0].Trim(), val = TextToPointArray(ss[1]) })
                    .ToDictionary(v => v.key, v => v.val);
            }
        }

        // Save poligones dictionary in the file FILE_NAME
        // format is:   "key" : 12,34,56,...
        public void Save()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in dict)
            {
                string coords = new string(pair.Value.SelectMany(p => p.X + "," + p.Y + ",").ToArray());
                sb.Append($"{pair.Key} : {coords} \r\n ");
            }
            File.WriteAllText(FILE_NAME, sb.ToString());
        }



        static Point[] TextToPointArray(string text, int m = 1)
        {
            // text is a string like "22,22,33,33,44,44,"

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

        
        // Get poligone containing the point. 
        // Return dict key as a string.
        //
        public string GetPoligonePointInto(Point p)
        {
            foreach (var pair in dict)
            {
                if (Contur.IsInside(pair.Value, p))
                    return pair.Key;
            }
            return null;
        }



    }
}
