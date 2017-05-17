using Contur;
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
    public partial class XForm : Form
    {
        public XForm()
        {
            InitializeComponent();
        }


        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pBox.Image = Image.FromFile(openFileDialog1.FileName);
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
            xc.FludFill();
            var conturList = xc.GetAllConturs();

            Graphics g = pBox.CreateGraphics();
            //for (int xo = 0; xo < xc.dots.GetLength(0); xo++)
            //    for (int yo = 0; yo < xc.dots.GetLength(1); yo++)
            //        g.DrawString(xc.dots[xo, yo].ToString(), new Font("Courier", 6), Brushes.Black, xo * step, yo * step);

            //foreach (var p in xc.cpoints)
            //    g.FillRectangle(Brushes.Red, p.P.X - 1, p.P.Y - 1, 3, 3);

            for (int i = 0; i < conturList.Count; i++)
                g.DrawPolygon(Pens.Red, conturList[i]);
            infoLabel.Text = $"Conturs = {conturList.Count()}";

            messBox.Text = string.Join("\r\n", conturList.Select(c => c.Count().ToString()));      
        }
    }
}
