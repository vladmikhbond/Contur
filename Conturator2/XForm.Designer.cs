namespace Conturator2
{
    partial class XForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.claerButton = new System.Windows.Forms.ToolStripButton();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.stepBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.infoLabel = new System.Windows.Forms.ToolStripLabel();
            this.allButton = new System.Windows.Forms.ToolStripButton();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.claerButton,
            this.loadButton,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.stepBox,
            this.toolStripSeparator2,
            this.infoLabel,
            this.allButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1189, 45);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // claerButton
            // 
            this.claerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.claerButton.Image = ((System.Drawing.Image)(resources.GetObject("claerButton.Image")));
            this.claerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.claerButton.Name = "claerButton";
            this.claerButton.Size = new System.Drawing.Size(47, 42);
            this.claerButton.Text = "Clear";
            this.claerButton.Click += new System.EventHandler(this.claerButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadButton.Image = ((System.Drawing.Image)(resources.GetObject("loadButton.Image")));
            this.loadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(92, 42);
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
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 42);
            this.toolStripLabel1.Text = "Cell";
            // 
            // stepBox
            // 
            this.stepBox.Name = "stepBox";
            this.stepBox.Size = new System.Drawing.Size(32, 45);
            this.stepBox.Text = "50";
            this.stepBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.infoLabel.Size = new System.Drawing.Size(120, 24);
            this.infoLabel.Text = "...";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // allButton
            // 
            this.allButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.allButton.Image = ((System.Drawing.Image)(resources.GetObject("allButton.Image")));
            this.allButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(85, 42);
            this.allButton.Text = "All Conturs";
            // 
            // pBox
            // 
            this.pBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBox.Image = ((System.Drawing.Image)(resources.GetObject("pBox.Image")));
            this.pBox.InitialImage = null;
            this.pBox.Location = new System.Drawing.Point(26, 72);
            this.pBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(871, 522);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBox.TabIndex = 3;
            this.pBox.TabStop = false;
            // 
            // XForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 715);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pBox);
            this.Name = "XForm";
            this.Text = "XForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton claerButton;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox stepBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel infoLabel;
        private System.Windows.Forms.ToolStripButton allButton;
        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}