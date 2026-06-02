// ===========================================================================
//	©2013-2025 WebSupergoo. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;
using WebSupergoo.ABCpdf14.Atoms;
using WebSupergoo.ABCpdf14.Operations;
using WebSupergoo.ABCpdf14.Elements;

namespace ValidatePDF {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		private Button button1;
		private CheckBox checkBoxOrphans;
		private CheckBox checkBoxCoverage;
		private CheckBox checkBoxObjects;
		private CheckBox checkBoxContentStreams;
		private CheckBox checkBoxSyntax;

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
			this.checkBoxOrphans = new System.Windows.Forms.CheckBox();
			this.checkBoxCoverage = new System.Windows.Forms.CheckBox();
			this.checkBoxObjects = new System.Windows.Forms.CheckBox();
			this.checkBoxContentStreams = new System.Windows.Forms.CheckBox();
			this.checkBoxSyntax = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(21, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(260, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Validate PDFs in ToValidate Folder";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBoxOrphans
			// 
			this.checkBoxOrphans.AutoSize = true;
			this.checkBoxOrphans.Location = new System.Drawing.Point(57, 93);
			this.checkBoxOrphans.Name = "checkBoxOrphans";
			this.checkBoxOrphans.Size = new System.Drawing.Size(152, 17);
			this.checkBoxOrphans.TabIndex = 4;
			this.checkBoxOrphans.Text = "Report orphaned Elements";
			this.checkBoxOrphans.UseVisualStyleBackColor = true;
			// 
			// checkBoxCoverage
			// 
			this.checkBoxCoverage.AutoSize = true;
			this.checkBoxCoverage.Location = new System.Drawing.Point(57, 116);
			this.checkBoxCoverage.Name = "checkBoxCoverage";
			this.checkBoxCoverage.Size = new System.Drawing.Size(170, 17);
			this.checkBoxCoverage.TabIndex = 5;
			this.checkBoxCoverage.Text = "Report Element type coverage";
			this.checkBoxCoverage.UseVisualStyleBackColor = true;
			// 
			// checkBoxObjects
			// 
			this.checkBoxObjects.AutoSize = true;
			this.checkBoxObjects.Checked = true;
			this.checkBoxObjects.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxObjects.Location = new System.Drawing.Point(39, 70);
			this.checkBoxObjects.Name = "checkBoxObjects";
			this.checkBoxObjects.Size = new System.Drawing.Size(120, 17);
			this.checkBoxObjects.TabIndex = 6;
			this.checkBoxObjects.Text = "Check PDF Objects";
			this.checkBoxObjects.UseVisualStyleBackColor = true;
			// 
			// checkBoxContentStreams
			// 
			this.checkBoxContentStreams.AutoSize = true;
			this.checkBoxContentStreams.Checked = true;
			this.checkBoxContentStreams.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxContentStreams.Location = new System.Drawing.Point(39, 139);
			this.checkBoxContentStreams.Name = "checkBoxContentStreams";
			this.checkBoxContentStreams.Size = new System.Drawing.Size(138, 17);
			this.checkBoxContentStreams.TabIndex = 7;
			this.checkBoxContentStreams.Text = "Check Content Streams";
			this.checkBoxContentStreams.UseVisualStyleBackColor = true;
			// 
			// checkBoxSyntax
			// 
			this.checkBoxSyntax.AutoSize = true;
			this.checkBoxSyntax.Checked = true;
			this.checkBoxSyntax.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSyntax.Location = new System.Drawing.Point(39, 162);
			this.checkBoxSyntax.Name = "checkBoxSyntax";
			this.checkBoxSyntax.Size = new System.Drawing.Size(116, 17);
			this.checkBoxSyntax.TabIndex = 9;
			this.checkBoxSyntax.Text = "Check PDF Syntax";
			this.checkBoxSyntax.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(306, 204);
			this.Controls.Add(this.checkBoxSyntax);
			this.Controls.Add(this.checkBoxContentStreams);
			this.Controls.Add(this.checkBoxObjects);
			this.Controls.Add(this.checkBoxCoverage);
			this.Controls.Add(this.checkBoxOrphans);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e) {
			string folder = null;
			DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
			while (true) {
				folder = Path.Combine(dir.FullName, "ToValidate");
				if (Directory.Exists(folder))
					break;
				dir = dir.Parent;
				if (dir == null)
					throw new Exception("Unable to find validation folder.");
			}
			ValidationUtilities.Validate(folder, checkBoxObjects.Checked, checkBoxOrphans.Checked, checkBoxCoverage.Checked, checkBoxContentStreams.Checked, false, checkBoxSyntax.Checked);
		}
	}
}



