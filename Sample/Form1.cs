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

namespace Sample
{
    public partial class Form1 : Form
    {
        int M = 1;
        Conturs conturs = new Conturs();

        public Form1()
        {
            InitializeComponent();            
        }


        private string current = null;

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            HiLightRegion(new Point(e.X / M, e.Y / M));
        }

        void HiLightRegion(Point p)
        {
            string selected = conturs.GetPoligonePointInto(p);
            if (selected != current)
            {
                Graphics g = pBox.CreateGraphics();
                pBox.Refresh();
                if (selected != null)
                {
                   // var ps = points.GetScaledPoligone(selected, M);
                    g.DrawLines(new Pen(Color.Red, M), conturs[selected]);
                }
                current = selected;
            }

        }

    }
}
