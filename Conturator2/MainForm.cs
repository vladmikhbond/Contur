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
    public partial class MainForm : Form
    {
        Image currentImage;
        YContur ycontur;

        public MainForm()
        {
            InitializeComponent();
            currentImage = pBox.Image;
        }


        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Size size1 = currentImage.Size;
                currentImage = Image.FromFile(openFileDialog1.FileName);
                Size size2 = currentImage.Size;

                pBox.Image = currentImage;
                Width += size2.Width - size1.Width;
                Height += size2.Height - size1.Height;
            }
        }


        //private void MakeWhiteEmptyImage()
        //{
        //    var bmp = new Bitmap(pBox.Width, pBox.Height);
        //    using (Graphics g = Graphics.FromImage(bmp))
        //    {
        //        g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
        //    }
        //    pBox.Image = bmp;
        //}


        private void clearButton_Click(object sender, EventArgs e)
        {
            pBox.Refresh();
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            if (pBox.Image == null)
                pBox.Image = currentImage;
            int step = Convert.ToInt32(stepBox.Text);
            ycontur = new YContur((Bitmap)pBox.Image, step, pBox.CreateGraphics(), textBox1);
            pBox.Refresh();
            // t
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Main part of work
            ycontur.Process();
                        
            // t
            stopWatch.Stop();
            string msec = stopWatch.ElapsedMilliseconds.ToString();
            
            // ordered list of conturs
            infoLabel.Text = $"Conturs = {ycontur.Conturs.Length }, t = {msec} msec";
            var xx = ycontur.Conturs.Select((c, i) => $"{i} - {c.Points.Count} / {c.Fails.Count}");
            messBox.Text = string.Join("\r\n", xx);      
        }


        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            var conturs = ycontur.ContursAroundPoint(e.Location);
            var innerContur = conturs.OrderBy(c => c.Length).FirstOrDefault();
            if (innerContur != null)
            {
                Graphics g = pBox.CreateGraphics();
                g.DrawPolygon(new Pen(Color.Yellow, 2), innerContur);
            }
        }

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"{e.X}-{e.Y}";
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            if (ycontur?.Conturs == null)
                return;


            for (int i = 0; i < ycontur.Conturs.Length; i++)
            {
                var points = ycontur.Conturs[i].Points;
                //if (contur.Count() < 2)
                //    continue;
                e.Graphics.DrawPolygon(Pens.Red, points.ToArray());
                float x = (float)points.Average(p => p.X);
                float y = (float)points.Average(p => p.Y);
                e.Graphics.DrawString(i.ToString(), Font, Brushes.Black, x, y);
            }

            
            Pen[] pens = { Pens.Red, Pens.Green, Pens.Blue, Pens.Magenta, Pens.Brown };

            int penIdx = 0;
            foreach (var contur in ycontur.Conturs)
            {
                e.Graphics.DrawPolygon(pens[penIdx], contur.Points.ToArray());
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

        private void stepBox_Click(object sender, EventArgs e)
        {

        }
    }
}
