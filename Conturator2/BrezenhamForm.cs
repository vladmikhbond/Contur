using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conturator2
{
    public partial class BrezenhamForm : Form
    {
        public BrezenhamForm()
        {
            InitializeComponent();

        }


        Point? BrezenhamPath(Point p0, Point p1)
        {
            int deltax = Math.Abs(p1.X - p0.X);
            int deltay = Math.Abs(p1.Y - p0.Y);

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
            for (int x = p0.X; x != p1.X; x += dx)
            {
                if (b45)
                {
                    if (IsBlack(y, x))
                        plot(new Point(y, x)); // return new Point(y, x);
                }
                else
                {
                    if (IsBlack(x, y))
                        plot(new Point(x, y)); // return new Point(x, y);                    }
                }
                error += deltaerr;
                if (2 * error >= deltax)
                {
                    y += dy;
                    error -= deltax;
                }
            }
            return null;
        }

        private bool IsBlack(int y, int x)
        {
            return true;
        }

        Graphics _g;

        void plot(Point p, Brush brush = null)
        {
            int d = 10;
            if (brush == null)
                brush = Brushes.Gray;
            _g.FillRectangle(brush, p.X * d, p.Y * d, d, d);  
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
            _g = CreateGraphics();
            string[] ss = textBox1.Text.Split(',');
            int x0 = Convert.ToInt32(ss[0].Trim());
            int y0 = Convert.ToInt32(ss[1].Trim());
            int x1 = Convert.ToInt32(ss[2].Trim());
            int y1 = Convert.ToInt32(ss[3].Trim());
            BrezenhamPath(new Point(x0, y0), new Point(x1, y1));
            plot(new Point(x0, y0), Brushes.Red);
            plot(new Point(x1, y1), Brushes.Blue);
            // 
        }
    }
}
