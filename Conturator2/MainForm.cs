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
        ConturList conturList;
        Image currentImage;


        public MainForm()
        {
            InitializeComponent();
            currentImage = pBox.Image;
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
            conturList = null;
            pBox.Refresh();
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            if (pBox.Image == null)
                pBox.Image = currentImage;
            int step = Convert.ToInt32(stepBox.Text);
            YContur xc = new YContur((Bitmap)pBox.Image, step, pBox.CreateGraphics(), textBox1);
            pBox.Refresh();
            // t
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Main part of work
            List<System.Drawing.Point[]> cl = xc.Process();
            conturList = new ConturList(cl);
            
            // t
            stopWatch.Stop();
            string msec = stopWatch.ElapsedMilliseconds.ToString();
            

            infoLabel.Text = $"Conturs = {cl.Count()}, t = {msec} msec";
            messBox.Text = string.Join("\r\n", cl.Select(c => c.Count().ToString()));      
        }


        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            var conturs = conturList.ContursAroundPoint(e.Location);
            var innerContur = conturs.OrderBy(c => c.Length).First();

            Graphics g = pBox.CreateGraphics();
            g.DrawPolygon(new Pen(Color.Yellow, 2), innerContur);
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


            for (int i = 0; i < conturList.Conturs.Count; i++)
            {
                var contur = conturList.Conturs[i];
                if (contur.Count() < 2)
                    continue;
                e.Graphics.DrawPolygon(Pens.Red, contur);
                float x = (float)contur.Average(p => p.X);
                float y = (float)contur.Average(p => p.Y);
                e.Graphics.DrawString(i.ToString(), Font, Brushes.Black, x, y);
            }

            
            Pen[] pens = { Pens.Black, Pens.Red, Pens.Green, Pens.Blue, Pens.Magenta, Pens.Brown };

            int penIdx = 0;
            foreach (var list in conturList.Conturs)
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

        private void stepBox_Click(object sender, EventArgs e)
        {

        }
    }
}
