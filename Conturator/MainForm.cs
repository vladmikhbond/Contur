﻿using Contur;
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


namespace Conturator
{
    public partial class MainForm : Form
    {
        Conturs conturs;

        public MainForm()
        {
            InitializeComponent();
            conturs = new Conturs();
        }

        Point[] points;  // last created contur's points

        private void Conturing(Point startPoint)
        {
            int step = Convert.ToInt32(stepBox.Text);
            points = Contur.Contur.Conturing((Bitmap)pBox.Image, step, startPoint);
            Graphics g = pBox.CreateGraphics();
            g.DrawPolygon(Pens.Red, points.ToArray());

            infoLabel.Text = $"Points = {points.Length}";
        }

        private void pBox_MouseDown(object sender, MouseEventArgs e)
        {
            Conturing(e.Location);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pBox.Image = Image.FromFile(openFileDialog1.FileName);
                conturs = new Conturs();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            conturs[keyBox.Text.Trim()] = points;
            conturs.Save();
            pBox.Refresh(); 
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && currentConturKey != null)
            {
                conturs.Delete(currentConturKey);
                conturs.Save();
                pBox.Refresh();
            }
        }

 
        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (var k in conturs.Keys)
            {
                e.Graphics.FillPolygon(Brushes.LightGray, conturs[k]);
            }
        }

        private void pBox_MouseMove(object sender, MouseEventArgs e)
        {
            HiLightRegion(new Point(e.X, e.Y));
        }

        private string currentConturKey = null;

        void HiLightRegion(Point p)
        {
            string selectedConturKey = conturs.GetPoligonePointInto(p);
            if (selectedConturKey != currentConturKey)
            {
                Graphics g = pBox.CreateGraphics();
                pBox.Refresh();
                if (selectedConturKey != null)
                {
                    g.DrawLines(new Pen(Color.Yellow, 2), conturs[selectedConturKey]);
                    infoLabel.Text = selectedConturKey;
                }
                currentConturKey = selectedConturKey;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int step = Convert.ToInt32(stepBox.Text);
            var conturs = Contur.Contur.GetAllConturs((Bitmap)pBox.Image, step);

            //Graphics g = pBox.CreateGraphics();
            //g.DrawPolygon(Pens.Red, points.ToArray());

            infoLabel.Text = $"Conturs = {conturs.Count()}";

        }


        //private void MakeEmptyImage()
        //{
        //    var bmp = new Bitmap(pBox.Width, pBox.Height);
        //    using (Graphics g = Graphics.FromImage(bmp))
        //    {
        //        g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
        //    }
        //    pBox.Image = bmp;
        //    conturs = new Conturs();
        //}


    }
}