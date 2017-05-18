using Contur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conturator2
{
    public partial class XForm : Form
    {
        List<Point[]> conturList = new List<Point[]>();

        public XForm()
        {
            InitializeComponent();
        }


        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pBox.Image = Image.FromFile(openFileDialog1.FileName);
                Width = pBox.Width + 220;
                Height = pBox.Height + 100;
            }
        }


        private void MakeEmptyImage()
        {
            var bmp = new Bitmap(pBox.Width, pBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
            }
            pBox.Image = bmp;
        }


        private void clearButton_Click(object sender, EventArgs e)
        {
            pBox.Refresh();
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            int step = Convert.ToInt32(stepBox.Text);
            XContur xc = new XContur((Bitmap)pBox.Image, step);

            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            xc.FludFill();
            conturList = xc.MakeAllConturs();

            stopWatch.Stop();
            string msec = stopWatch.ElapsedMilliseconds.ToString();
            
            // Show results


            pBox.Refresh();
            Graphics g = pBox.CreateGraphics();
            if (step >= 10)
            {
                for (int xo = 0; xo < xc.dots.GetLength(0); xo++)
                    for (int yo = 0; yo < xc.dots.GetLength(1); yo++)
                        g.DrawString(xc.dots[xo, yo].ToString(), new Font("Courier", 6), Brushes.Black, xo * step, yo * step);
            }
            //foreach (var p in xc.cpoints)
            //    g.FillRectangle(Brushes.Red, p.P.X - 1, p.P.Y - 1, 3, 3);

            for (int i = 0; i < conturList.Count; i++)
                g.DrawPolygon(Pens.Red, conturList[i]);
            infoLabel.Text = $"Conturs = {conturList.Count()}, t = {msec} msec";

            messBox.Text = string.Join("\r\n", conturList.Select(c => c.Count().ToString()));      
        }

        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            var cs = conturList.Where(c => IsInside(c, e.Location));
            Graphics g = pBox.CreateGraphics();
            foreach (var c in cs)
            {
                g.DrawPolygon(new Pen(Color.Yellow, 2), c);
            }

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

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"{e.X}-{e.Y}";
        }
    }
}
