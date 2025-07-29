// ===========================================================================
//	©2013-2024 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;

namespace testapp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private TextBox filePath;
		private Button button2;
		private TextBox textBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
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
			this.filePath = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(380, 42);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Print";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// filePath
			// 
			this.filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filePath.Location = new System.Drawing.Point(12, 16);
			this.filePath.Name = "filePath";
			this.filePath.Size = new System.Drawing.Size(560, 20);
			this.filePath.TabIndex = 1;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(492, 42);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(80, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Print Silently";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(12, 83);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(560, 296);
			this.textBox1.TabIndex = 3;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 391);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.filePath);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";
			foreach (string file in Directory.GetFiles(theRez, "*.pdf")) {
				filePath.Text = file;
				break;
			}
			textBox1.Text = File.ReadAllText(Path.Combine(theRez, "ReadMe.txt"));
		}

		private void button1_Click(object sender, System.EventArgs e) {
			TimeSpan span = TimeSpan.MinValue;
			string file = filePath.Text;

			using (Doc doc = new Doc()) {
				doc.Read(file);
				PrintDialog dialog = new PrintDialog();
				try {
					// NB: on some AMD64 machines dialog will not show unless UseEXDialog is set to true.
					dialog.UseEXDialog = true;
					dialog.PrinterSettings.FromPage = 1;
					dialog.PrinterSettings.ToPage = doc.PageCount;
					dialog.PrinterSettings.MinimumPage = 1;
					dialog.PrinterSettings.MaximumPage = doc.PageCount;
					dialog.AllowSomePages = true;
					if (dialog.ShowDialog() != DialogResult.OK)
						return;

					DateTime start = DateTime.Now;
					ABCpdfPrinting.NativePrint.Print(doc, Path.GetFileName(file), dialog.PrinterSettings);
					span = DateTime.Now.Subtract(start);
				}
				catch (Exception ex) {
					MessageBox.Show(GetExceptionMessage(ex));
					return;
				}
				finally {
					dialog.Dispose();
				}
			}

			int ms = (int)span.TotalMilliseconds;
			MessageBox.Show("Finished in " + ms.ToString() + " ms");
		}

		private void button2_Click(object sender, EventArgs e) {
			TimeSpan span = TimeSpan.MinValue;
			string file = filePath.Text;

			using (Doc doc = new Doc()) {
				doc.Read(file);
				try {
					PrinterSettings settings = new PrinterSettings(); // default printer
					settings.FromPage = 1;
					settings.ToPage = doc.PageCount;
					DateTime start = DateTime.Now;
					ABCpdfPrinting.NativePrint.Print(doc, Path.GetFileName(file), settings);
					span = DateTime.Now.Subtract(start);
				}
				catch (Exception ex) {
					MessageBox.Show(GetExceptionMessage(ex));
					return;
				}
			}

			int ms = (int)span.TotalMilliseconds;
			MessageBox.Show("Finished in " + ms.ToString() + " ms");
		}

		private static string GetExceptionMessage(Exception exc) {
			return GetExceptionMessage(exc, "\r\n");
		}

		private static string GetExceptionMessage(Exception exc, string separator) {
			string msg = exc.Message;
			if (exc.InnerException != null) {
				msg += separator;
				msg += exc.InnerException.Message;
			}
			return msg;
		}
	}
}
