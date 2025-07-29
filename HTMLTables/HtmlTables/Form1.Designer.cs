namespace HtmlTables {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.outline = new System.Windows.Forms.CheckBox();
			this.deleteContent = new System.Windows.Forms.CheckBox();
			this.deleteShift = new System.Windows.Forms.CheckBox();
			this.addFrame = new System.Windows.Forms.CheckBox();
			this.addBackground = new System.Windows.Forms.CheckBox();
			this.maxImageWidth = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.maxImageHeight = new System.Windows.Forms.NumericUpDown();
			this.button4 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.maxImageWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.maxImageHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(118, 43);
			this.button1.TabIndex = 0;
			this.button1.Text = "Nested Table";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 61);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(118, 43);
			this.button2.TabIndex = 1;
			this.button2.Text = "Paging Table";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(12, 110);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(118, 43);
			this.button3.TabIndex = 2;
			this.button3.Text = "Invoice";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// outline
			// 
			this.outline.AutoSize = true;
			this.outline.Location = new System.Drawing.Point(175, 12);
			this.outline.Name = "outline";
			this.outline.Size = new System.Drawing.Size(99, 17);
			this.outline.TabIndex = 5;
			this.outline.Text = "Outline Content";
			this.outline.UseVisualStyleBackColor = true;
			// 
			// deleteContent
			// 
			this.deleteContent.AutoSize = true;
			this.deleteContent.Location = new System.Drawing.Point(175, 35);
			this.deleteContent.Name = "deleteContent";
			this.deleteContent.Size = new System.Drawing.Size(97, 17);
			this.deleteContent.TabIndex = 6;
			this.deleteContent.Text = "Delete Content";
			this.deleteContent.UseVisualStyleBackColor = true;
			// 
			// deleteShift
			// 
			this.deleteShift.AutoSize = true;
			this.deleteShift.Location = new System.Drawing.Point(175, 58);
			this.deleteShift.Name = "deleteShift";
			this.deleteShift.Size = new System.Drawing.Size(185, 17);
			this.deleteShift.TabIndex = 7;
			this.deleteShift.Text = "Delete, Shift and Redraw Content";
			this.deleteShift.UseVisualStyleBackColor = true;
			// 
			// addFrame
			// 
			this.addFrame.AutoSize = true;
			this.addFrame.Location = new System.Drawing.Point(175, 81);
			this.addFrame.Name = "addFrame";
			this.addFrame.Size = new System.Drawing.Size(77, 17);
			this.addFrame.TabIndex = 8;
			this.addFrame.Text = "Add Frame";
			this.addFrame.UseVisualStyleBackColor = true;
			// 
			// addBackground
			// 
			this.addBackground.AutoSize = true;
			this.addBackground.Location = new System.Drawing.Point(175, 104);
			this.addBackground.Name = "addBackground";
			this.addBackground.Size = new System.Drawing.Size(106, 17);
			this.addBackground.TabIndex = 9;
			this.addBackground.Text = "Add Background";
			this.addBackground.UseVisualStyleBackColor = true;
			// 
			// maxImageWidth
			// 
			this.maxImageWidth.Location = new System.Drawing.Point(175, 181);
			this.maxImageWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.maxImageWidth.Name = "maxImageWidth";
			this.maxImageWidth.Size = new System.Drawing.Size(56, 20);
			this.maxImageWidth.TabIndex = 10;
			this.maxImageWidth.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(172, 165);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Max Image Size";
			// 
			// maxImageHeight
			// 
			this.maxImageHeight.Location = new System.Drawing.Point(237, 181);
			this.maxImageHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.maxImageHeight.Name = "maxImageHeight";
			this.maxImageHeight.Size = new System.Drawing.Size(56, 20);
			this.maxImageHeight.TabIndex = 12;
			this.maxImageHeight.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(12, 181);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(118, 43);
			this.button4.TabIndex = 3;
			this.button4.Text = "Other Tests";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(371, 236);
			this.Controls.Add(this.maxImageHeight);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.maxImageWidth);
			this.Controls.Add(this.addBackground);
			this.Controls.Add(this.addFrame);
			this.Controls.Add(this.deleteShift);
			this.Controls.Add(this.deleteContent);
			this.Controls.Add(this.outline);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.maxImageWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.maxImageHeight)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.CheckBox outline;
		private System.Windows.Forms.CheckBox deleteContent;
		private System.Windows.Forms.CheckBox deleteShift;
		private System.Windows.Forms.CheckBox addFrame;
		private System.Windows.Forms.CheckBox addBackground;
        private System.Windows.Forms.NumericUpDown maxImageWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown maxImageHeight;
		private System.Windows.Forms.Button button4;
    }
}

