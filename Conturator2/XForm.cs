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


        private void claerButton_Click(object sender, EventArgs e)
        {
            int step = Convert.ToInt32(stepBox.Text);
            XContur xc = new XContur((Bitmap)pBox.Image, step);
            xc.FludFill();

        }
    }
}
