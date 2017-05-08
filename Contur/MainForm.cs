using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contur
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
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

        List<Point> points;  // last created contur's points

        private void Conturing(Point startPoint)
        {
            int step = Convert.ToInt32(stepBox.Text);
            points = Contur.Conturing((Bitmap)pBox.Image, step, startPoint);
            Graphics g = pBox.CreateGraphics();
            g.DrawPolygon(Pens.Red, points.ToArray());

            infoLabel.Text = $"Points = {points.Count}";
        }


        #region Drawing 

        bool isDrowing = false;
        Point p0;

        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Conturing(e.Location);
            }
            else
            {
                isDrowing = true;
                p0 = e.Location;
            }
        }

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrowing)
            {
                Pen pen = new Pen(Color.Gray, 1);
                Graphics g = pBox.CreateGraphics();
                Graphics g2 = Graphics.FromImage(pBox.Image);
                g.DrawLine(pen, p0, e.Location);
                g2.DrawLine(pen, p0, e.Location);

                p0 = e.Location;
            }
        }

        private void pBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDrowing = false;
        }

        #endregion

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pBox.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void emtyButton_Click(object sender, EventArgs e)
        {
            MakeEmptyImage();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pBox.Refresh();
        }

        // save contur as a line like "key : 12,23,...,34,45,"
        private void saveButton_Click(object sender, EventArgs e)
        {
            string coords = new string(points.SelectMany(p => p.X + "," + p.Y + ",").ToArray());
            string text = $"{keyBox.Text} : {coords} \r\n ";
            File.AppendAllText("conturs.txt", text);
        }
    }
}
