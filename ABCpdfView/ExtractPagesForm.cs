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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebSupergoo.ABCpdf13;


namespace ABCpdfControls {
	/// <summary>
	/// Summary description for ExtractPagesForm.
	/// </summary>
	public class ExtractPagesForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label pagesNumberLabel;
		private System.Windows.Forms.TextBox toPageTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox fromPageTextBox;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label3;
		private Doc mCurrentDoc;

		public ExtractPagesForm(Doc currentDoc) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mCurrentDoc = currentDoc;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.pagesNumberLabel = new System.Windows.Forms.Label();
			this.toPageTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.fromPageTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(144, 80);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 9;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(232, 80);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "Cancel";
			// 
			// pagesNumberLabel
			// 
			this.pagesNumberLabel.Location = new System.Drawing.Point(192, 48);
			this.pagesNumberLabel.Name = "pagesNumberLabel";
			this.pagesNumberLabel.TabIndex = 15;
			this.pagesNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toPageTextBox
			// 
			this.toPageTextBox.Location = new System.Drawing.Point(144, 48);
			this.toPageTextBox.Name = "toPageTextBox";
			this.toPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.toPageTextBox.TabIndex = 14;
			this.toPageTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(112, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 23);
			this.label2.TabIndex = 13;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fromPageTextBox
			// 
			this.fromPageTextBox.Location = new System.Drawing.Point(56, 48);
			this.fromPageTextBox.Name = "fromPageTextBox";
			this.fromPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.fromPageTextBox.TabIndex = 12;
			this.fromPageTextBox.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 11;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(296, 32);
			this.label3.TabIndex = 16;
			this.label3.Text = "Extract a portion of a PDF. All the pages outside the selected portion will be de" +
				"leted.";
			// 
			// ExtractPagesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(312, 112);
			this.ControlBox = false;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.pagesNumberLabel);
			this.Controls.Add(this.toPageTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.fromPageTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ExtractPagesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Extract Pages";
			this.Load += new System.EventHandler(this.ExtractPagesForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ExtractPagesForm_Load(object sender, System.EventArgs e) {
			if (mCurrentDoc != null) {
				fromPageTextBox.Text = mCurrentDoc.PageNumber.ToString();
				toPageTextBox.Text = mCurrentDoc.PageNumber.ToString();
				pagesNumberLabel.Text = "of " + mCurrentDoc.PageCount.ToString();
			}
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			int fromPage = 0, toPage = 0;
			bool error = false;

			try {
				fromPage = int.Parse(fromPageTextBox.Text);
			}
			catch {
				error = true;
			}

			if (fromPage <= 0 || fromPage > mCurrentDoc.PageCount )
				error = true;

			if (error) {
				MessageBox.Show("There is no page numbered '" + fromPageTextBox.Text + "' in this document.");
				return;
			}

			try {
				toPage = int.Parse(toPageTextBox.Text);
			}
			catch {
				error = true;
			}

			if (toPage <= 0 || toPage > mCurrentDoc.PageCount )
				error = true;

			if (error) {
				MessageBox.Show("There is no page numbered '" + toPageTextBox.Text + "' in this document.");
				return;
			}

			if (toPage < fromPage) {
				MessageBox.Show("The starting page number must be less then or the same as the ending page number.");
				return;
			}

			
			int origPageCount = mCurrentDoc.PageCount;

			for (int i = toPage + 1; i <= origPageCount; i++) {
				mCurrentDoc.PageNumber = toPage + 1;
				mCurrentDoc.Delete(mCurrentDoc.Page);
			}

			for (int i = 1; i < fromPage; i++) {
				mCurrentDoc.PageNumber = 1;
				mCurrentDoc.Delete(mCurrentDoc.Page);
			}

			mCurrentDoc.PageNumber = 1;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
