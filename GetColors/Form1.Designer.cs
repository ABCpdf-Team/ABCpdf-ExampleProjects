namespace GetColors {
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.maxPages = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.maxPages)).BeginInit();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(15, 42);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(549, 395);
			this.textBox1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(570, 42);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(110, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "Get Colors";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(15, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(549, 21);
			this.comboBox1.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(570, 80);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(110, 32);
			this.button2.TabIndex = 3;
			this.button2.Text = "Save SVG";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(570, 118);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(110, 32);
			this.button3.TabIndex = 4;
			this.button3.Text = "Save All SVG";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// maxPages
			// 
			this.maxPages.Location = new System.Drawing.Point(633, 13);
			this.maxPages.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.maxPages.Name = "maxPages";
			this.maxPages.Size = new System.Drawing.Size(44, 20);
			this.maxPages.TabIndex = 5;
			this.maxPages.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(573, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Max Pages";
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 451);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(665, 22);
			this.progressBar1.TabIndex = 7;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(689, 485);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.maxPages);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Activated += new System.EventHandler(this.Form1_Activated);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.maxPages)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.NumericUpDown maxPages;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}

