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
        Image currentImage;


        public XForm()
        {
            InitializeComponent();
        }


        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentImage = Image.FromFile(openFileDialog1.FileName);
                pBox.Image = currentImage;
                Width = pBox.Width + 220;
                Height = pBox.Height + 100;
            }
        }


        private void MakeWhiteEmptyImage()
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
            if (pBox.Image == null)
                pBox.Image = currentImage;
            int step = Convert.ToInt32(stepBox.Text);
            IContur xc = new SContur((Bitmap)pBox.Image, step);
            pBox.Refresh();

            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            List<Point[]> cl = xc.MakeAllConturs();
            conturList = new ConturList(cl);

            stopWatch.Stop();
            string msec = stopWatch.ElapsedMilliseconds.ToString();
            
            // Show results


            Graphics g = pBox.CreateGraphics();
            //if (step >= 10)
            //{
            //    for (int xo = 0; xo < xc.dots.GetLength(0); xo++)
            //        for (int yo = 0; yo < xc.dots.GetLength(1); yo++)
            //            g.DrawString(xc.dots[xo, yo].ToString(), new Font("Courier", 6), Brushes.Black, xo * step, yo * step);
            //}
            foreach (var p in xc.Points)
                g.FillRectangle(Brushes.Red, p.X - 1, p.Y - 1, 3, 3);

            //for (int i = 0; i < cl.Count; i++)
            //    g.DrawPolygon(Pens.Red, cl[i]);

            //pBox.Refresh();


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
            infoLabel.Text = conturList.ConturIdxAroundPoint(e.Location).ToString();
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


            for (int i = 0; i < conturList.List.Count; i++)
            {
                var contur = conturList.List[i];
                if (contur.Count() < 2)
                    continue;
                e.Graphics.DrawPolygon(Pens.Red, contur);
                float x = (float)contur.Average(p => p.X);
                float y = (float)contur.Average(p => p.Y);
                e.Graphics.DrawString(i.ToString(), Font, Brushes.Black, x, y);
            }

            
            Pen[] pens = { Pens.Black, Pens.Red, Pens.Green, Pens.Blue, Pens.Magenta, Pens.Brown };

            int penIdx = 0;
            foreach (var list in conturList.List)
            {
                if (list.Count() < 2)
                    continue;
                e.Graphics.DrawPolygon(pens[penIdx], list);
                penIdx = (penIdx + 1) % pens.Length;
            }

        }

        private void noImageButton_Click(object sender, EventArgs e)
        {
            if (pBox.Image == null)
                pBox.Image = currentImage;
            else
                pBox.Image = null;
        }
    }
}
