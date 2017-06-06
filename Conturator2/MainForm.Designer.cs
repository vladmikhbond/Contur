namespace Conturator2
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.stepBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.infoLabel = new System.Windows.Forms.ToolStripLabel();
            this.allButton = new System.Windows.Forms.ToolStripButton();
            this.clearButton = new System.Windows.Forms.ToolStripButton();
            this.noImageButton = new System.Windows.Forms.ToolStripButton();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.messBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadButton,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.stepBox,
            this.toolStripSeparator2,
            this.infoLabel,
            this.allButton,
            this.clearButton,
            this.noImageButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1136, 45);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadButton.Image = ((System.Drawing.Image)(resources.GetObject("loadButton.Image")));
            this.loadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(73, 42);
            this.loadButton.Text = "Load Image";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 45);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(27, 42);
            this.toolStripLabel1.Text = "Cell";
            // 
            // stepBox
            // 
            this.stepBox.Name = "stepBox";
            this.stepBox.Size = new System.Drawing.Size(25, 45);
            this.stepBox.Text = "26";
            this.stepBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.stepBox.Click += new System.EventHandler(this.stepBox_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 45);
            // 
            // infoLabel
            // 
            this.infoLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.infoLabel.AutoSize = false;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(1, 1, 0, 20);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(250, 24);
            this.infoLabel.Text = "...";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // allButton
            // 
            this.allButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.allButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.allButton.Image = ((System.Drawing.Image)(resources.GetObject("allButton.Image")));
            this.allButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(44, 42);
            this.allButton.Text = "Vector";
            this.allButton.Click += new System.EventHandler(this.allButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
            this.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(38, 42);
            this.clearButton.Text = "Clear";
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // noImageButton
            // 
            this.noImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.noImageButton.Image = ((System.Drawing.Image)(resources.GetObject("noImageButton.Image")));
            this.noImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.noImageButton.Name = "noImageButton";
            this.noImageButton.Size = new System.Drawing.Size(60, 42);
            this.noImageButton.Text = "Hide Img";
            this.noImageButton.Click += new System.EventHandler(this.noImageButton_Click);
            // 
            // pBox
            // 
            this.pBox.BackColor = System.Drawing.SystemColors.Window;
            this.pBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBox.Image = ((System.Drawing.Image)(resources.GetObject("pBox.Image")));
            this.pBox.InitialImage = null;
            this.pBox.Location = new System.Drawing.Point(6, 38);
            this.pBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(240, 209);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBox.TabIndex = 3;
            this.pBox.TabStop = false;
            this.pBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Paint);
            this.pBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBox_MouseDown);
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBox_MouseMove);
            // 
            // messBox
            // 
            this.messBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messBox.Location = new System.Drawing.Point(895, 39);
            this.messBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.messBox.Multiline = true;
            this.messBox.Name = "messBox";
            this.messBox.ReadOnly = true;
            this.messBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messBox.Size = new System.Drawing.Size(115, 515);
            this.messBox.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(1014, 38);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(118, 515);
            this.textBox1.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 562);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.messBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "XForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton clearButton;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox stepBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel infoLabel;
        private System.Windows.Forms.ToolStripButton allButton;
        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox messBox;
        private System.Windows.Forms.ToolStripButton noImageButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}