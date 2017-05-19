using Contur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conturator2
{
    public partial class XForm : Form
    {
        ConturList conturList;

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
            var cl = xc.MakeAllConturs();
            conturList = new ConturList(cl);

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

            for (int i = 0; i < cl.Count; i++)
                g.DrawPolygon(Pens.Red, cl[i]);
            infoLabel.Text = $"Conturs = {cl.Count()}, t = {msec} msec";

            messBox.Text = string.Join("\r\n", cl.Select(c => c.Count().ToString()));      
        }

        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            var cs = conturList.ContursAroundPoint(e.Location);
            Graphics g = pBox.CreateGraphics();
            foreach (var c in cs)
            {
                g.DrawPolygon(new Pen(Color.Yellow, 2), c);
            }
        }

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"{e.X}-{e.Y}";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            conturList.SaveToFile("xconturs.txt");
        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            conturList = new ConturList("xconturs.txt");
            pBox.Refresh();
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            if (conturList == null)
                return;
            foreach (var list in conturList.List)
                e.Graphics.DrawPolygon(Pens.Red, list);

        }
    }
}
