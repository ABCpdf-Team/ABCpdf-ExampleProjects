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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebSupergoo.ABCpdf14;

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for DeletePagesForm.
	/// </summary>
	public class DeletePagesForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox fromPageTextBox;
		private System.Windows.Forms.TextBox toPageTextBox;
		private System.Windows.Forms.Label pagesNumberLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label3;
		private Doc mCurrentDoc;

		public DeletePagesForm(Doc currentDoc) {
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
			this.label1 = new System.Windows.Forms.Label();
			this.fromPageTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.toPageTextBox = new System.Windows.Forms.TextBox();
			this.pagesNumberLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fromPageTextBox
			// 
			this.fromPageTextBox.Location = new System.Drawing.Point(64, 40);
			this.fromPageTextBox.Name = "fromPageTextBox";
			this.fromPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.fromPageTextBox.TabIndex = 1;
			this.fromPageTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(120, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toPageTextBox
			// 
			this.toPageTextBox.Location = new System.Drawing.Point(152, 40);
			this.toPageTextBox.Name = "toPageTextBox";
			this.toPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.toPageTextBox.TabIndex = 3;
			this.toPageTextBox.Text = "";
			// 
			// pagesNumberLabel
			// 
			this.pagesNumberLabel.Location = new System.Drawing.Point(200, 40);
			this.pagesNumberLabel.Name = "pagesNumberLabel";
			this.pagesNumberLabel.TabIndex = 4;
			this.pagesNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(144, 72);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 7;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(232, 72);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(272, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Delete selected range of pages from document.";
			// 
			// DeletePagesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(314, 104);
			this.ControlBox = false;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.pagesNumberLabel);
			this.Controls.Add(this.toPageTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.fromPageTextBox);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "DeletePagesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Delete Pages";
			this.Load += new System.EventHandler(this.DeletePagesForm_Load);
			this.ResumeLayout(false);

		}
		#endregion


		private void DeletePagesForm_Load(object sender, System.EventArgs e) {
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

			if (fromPage == 1 && toPage == mCurrentDoc.PageCount) {
				MessageBox.Show("You can't delete all pages.");
				return;
			}

			for (int i = 0; i < toPage - fromPage + 1; i++) {
				mCurrentDoc.PageNumber = fromPage;
				mCurrentDoc.Delete(mCurrentDoc.Page);
			}

			if (fromPage <= mCurrentDoc.PageCount)
				mCurrentDoc.PageNumber = fromPage;
			else
				mCurrentDoc.PageNumber = fromPage - 1;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
