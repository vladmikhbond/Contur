namespace Sample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.outerPanel = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.outerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outerPanel
            // 
            this.outerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outerPanel.AutoScroll = true;
            this.outerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outerPanel.Controls.Add(this.pBox);
            this.outerPanel.Location = new System.Drawing.Point(14, 13);
            this.outerPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(861, 527);
            this.outerPanel.TabIndex = 0;
            // 
            // pBox
            // 
            this.pBox.Image = ((System.Drawing.Image)(resources.GetObject("pBox.Image")));
            this.pBox.InitialImage = null;
            this.pBox.Location = new System.Drawing.Point(-2, -2);
            this.pBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(860, 510);
            this.pBox.TabIndex = 3;
            this.pBox.TabStop = false;
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBox_MouseMove);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "        Cherkasy,",
            "        Chernihiv,",
            "        Chernivtsi,",
            "        Dnipropetrovsk,",
            "        Donetsk,",
            "        Ivano_Frankivsk,",
            "        Kharkiv,",
            "        Kherson,",
            "        Khmelnytskyi,",
            "        Kirovohrad,",
            "        Kyiv,",
            "        Luhansk,",
            "        Lutsk,",
            "        Lviv,",
            "        Mykolaiv,",
            "        Odesa,",
            "        Poltava,",
            "        Rivne,",
            "        Sumy,",
            "        Ternopil,",
            "        Vinnytsia,",
            "        Volyn,",
            "        Zakarpattia,",
            "        Zaporizhzhia,",
            "        Zhytomyr,",
            "        Crimea,"});
            this.listBox1.Location = new System.Drawing.Point(899, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(331, 612);
            this.listBox1.TabIndex = 4;
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(18, 553);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(859, 68);
            this.listBox2.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 653);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.outerPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(1260, 700);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "Form1";
            this.Text = "Sample";
            this.outerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel outerPanel;
        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
    }
}

