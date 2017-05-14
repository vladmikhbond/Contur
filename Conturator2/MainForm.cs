using Contur;
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


namespace Conturator2
{
    public partial class MainForm : Form
    {
        List<Point[]> conturList = new List<Point[]>();

        public MainForm()
        {
            InitializeComponent();
            MakeEmptyImage();
        }

        Point[] points;  // last created contur points

        #region Mouse Events

        private Point[] currentContur = null;
        Pen pen = null; //  new Pen(Color.Black, 2);
        Point mouseLocation;

        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GetOneContur(e.Location);
            }
            else
            {
                pen = new Pen(Color.Black, 2);
                mouseLocation = e.Location;
            }
        }

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (pen == null)
            {
                HiLightRegion(new Point(e.X, e.Y));
            }
            else
            {
                Graphics g = Graphics.FromImage(pBox.Image);
                g.DrawLine(pen, mouseLocation, e.Location);
                mouseLocation = e.Location;
                pBox.Refresh();
            }
        }

        private void pBox_MouseUp(object sender, MouseEventArgs e)
        {
            pen = null;
        }

        #endregion

        #region ToolStrip Events

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pBox.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            GetAllConturs();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (conturList != null && e.KeyCode == Keys.Delete && currentContur != null)
            {
                conturList.Remove(currentContur);
                pBox.Refresh();
            }
        }

        #endregion

        private void GetAllConturs()
        {
            int step = Convert.ToInt32(stepBox.Text);
            conturList = Contur.Contur.GetAllConturs((Bitmap)pBox.Image, step);
            Graphics g = pBox.CreateGraphics();
            for (int i = 0; i < conturList.Count; i++)
                g.DrawPolygon(Pens.Red, conturList[i]);
            infoLabel.Text = $"Conturs = {conturList.Count()}";

            string info = string.Join("\n", conturList.Select(c => c.Count().ToString()));
            MessageBox.Show(info, infoLabel.Text);
        }

        private void GetOneContur(Point startPoint)
        {
            int step = Convert.ToInt32(stepBox.Text);
            points = Contur.Contur.GetOneContur((Bitmap)pBox.Image, step, startPoint);
            if (points != null)
            {
                Graphics g = pBox.CreateGraphics();
                g.DrawPolygon(Pens.Red, points.ToArray());
                foreach (var p in points)
                    g.FillRectangle(Brushes.Red, p.X - 1, p.Y - 1, 3, 3);
                var p0 = points[0];
                g.FillRectangle(Brushes.Red, p0.X - 3, p0.Y - 3, 7, 7);
                infoLabel.Text = $"Points = {points.Length}";
            }
            else
            {
                infoLabel.Text = "No contur";
            }
        }


        void HiLightRegion(Point point)
        {
            var selectedContur = conturList.FirstOrDefault(x => Contur.Contur.IsInside(x, point));


            if (selectedContur != currentContur)
            {
                Graphics g = pBox.CreateGraphics();
                pBox.Refresh();
                if (selectedContur != null)
                {
                    g.DrawLines(new Pen(Color.Yellow, 2), selectedContur);
                    foreach (var p in selectedContur)
                        g.FillRectangle(Brushes.Red, p.X - 1, p.Y - 1, 3, 3);

                    infoLabel.Text = selectedContur.Length.ToString();
                }
                currentContur = selectedContur;
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
            conturList = new List<Point[]>();
        }

        private void claerButton_Click(object sender, EventArgs e)
        {
            MakeEmptyImage();
        }
    }
}
