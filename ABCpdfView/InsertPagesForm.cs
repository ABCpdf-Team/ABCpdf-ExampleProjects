// ===========================================================================
//	©2013-2026 WebSupergoo. All rights reserved.
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
	/// Summary description for InsertPagesForm.
	/// </summary>
	public class InsertPagesForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox positionComboBox;
		private System.Windows.Forms.TextBox pageNumberTextBox;
		private System.Windows.Forms.RadioButton pageRadioButton;
		private System.Windows.Forms.RadioButton lastRadioButton;
		private System.Windows.Forms.RadioButton firstRadioButton;
		private System.Windows.Forms.Label pagesCountLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private Doc mCurrentDoc;
		private Doc mInsertedDoc;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InsertPagesForm(Doc currentDoc, Doc insertedDoc) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mCurrentDoc = currentDoc;
			mInsertedDoc = insertedDoc;
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.positionComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pagesCountLabel = new System.Windows.Forms.Label();
			this.pageNumberTextBox = new System.Windows.Forms.TextBox();
			this.pageRadioButton = new System.Windows.Forms.RadioButton();
			this.lastRadioButton = new System.Windows.Forms.RadioButton();
			this.firstRadioButton = new System.Windows.Forms.RadioButton();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Insert Pages from File:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(128, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(224, 23);
			this.label2.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Where to insert:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// positionComboBox
			// 
			this.positionComboBox.Items.AddRange(new object[] {
																  "After",
																  "Before"});
			this.positionComboBox.Location = new System.Drawing.Point(93, 32);
			this.positionComboBox.Name = "positionComboBox";
			this.positionComboBox.Size = new System.Drawing.Size(80, 21);
			this.positionComboBox.TabIndex = 3;
			this.positionComboBox.Text = "After";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pagesCountLabel);
			this.groupBox1.Controls.Add(this.pageNumberTextBox);
			this.groupBox1.Controls.Add(this.pageRadioButton);
			this.groupBox1.Controls.Add(this.lastRadioButton);
			this.groupBox1.Controls.Add(this.firstRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(8, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(342, 128);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Page";
			// 
			// pagesCountLabel
			// 
			this.pagesCountLabel.Location = new System.Drawing.Point(128, 88);
			this.pagesCountLabel.Name = "pagesCountLabel";
			this.pagesCountLabel.Size = new System.Drawing.Size(120, 23);
			this.pagesCountLabel.TabIndex = 4;
			this.pagesCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pageNumberTextBox
			// 
			this.pageNumberTextBox.Location = new System.Drawing.Point(72, 89);
			this.pageNumberTextBox.Name = "pageNumberTextBox";
			this.pageNumberTextBox.Size = new System.Drawing.Size(48, 20);
			this.pageNumberTextBox.TabIndex = 3;
			this.pageNumberTextBox.Text = "";
			// 
			// pageRadioButton
			// 
			this.pageRadioButton.Checked = true;
			this.pageRadioButton.Location = new System.Drawing.Point(8, 87);
			this.pageRadioButton.Name = "pageRadioButton";
			this.pageRadioButton.Size = new System.Drawing.Size(64, 24);
			this.pageRadioButton.TabIndex = 2;
			this.pageRadioButton.TabStop = true;
			this.pageRadioButton.Text = "Page:";
			this.pageRadioButton.CheckedChanged += new System.EventHandler(this.pageRadioButton_CheckedChanged);
			// 
			// lastRadioButton
			// 
			this.lastRadioButton.Location = new System.Drawing.Point(8, 56);
			this.lastRadioButton.Name = "lastRadioButton";
			this.lastRadioButton.TabIndex = 1;
			this.lastRadioButton.Text = "Last";
			// 
			// firstRadioButton
			// 
			this.firstRadioButton.Location = new System.Drawing.Point(8, 24);
			this.firstRadioButton.Name = "firstRadioButton";
			this.firstRadioButton.TabIndex = 0;
			this.firstRadioButton.Text = "First";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(275, 198);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(192, 198);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// InsertPagesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(354, 226);
			this.ControlBox = false;
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.positionComboBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InsertPagesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Insert Pages";
			this.Load += new System.EventHandler(this.InsertPagesForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void InsertPagesForm_Load(object sender, System.EventArgs e) {
			if (mCurrentDoc != null) {
				pageNumberTextBox.Text = mCurrentDoc.PageNumber.ToString();
				pagesCountLabel.Text = "of " + mCurrentDoc.PageCount.ToString();
			}
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			int pageNumber = 0;

			if (firstRadioButton.Checked)
				pageNumber = 1;
			else if (lastRadioButton.Checked)
				pageNumber = mCurrentDoc.PageCount;
			else if (pageRadioButton.Checked) {

				bool error = false;

				try {
					pageNumber = int.Parse(pageNumberTextBox.Text);
				}
				catch {
					error = true;
				}

				if (pageNumber <= 0 || pageNumber > mCurrentDoc.PageCount )
					error = true;

				if (error) {
					MessageBox.Show("There is no page numbered '" + pageNumberTextBox.Text + "' in this document.");
					return;
				}
			}

			int origDocPageCount = mCurrentDoc.PageCount;
			mCurrentDoc.Append(mInsertedDoc);

			string mappingString = "";

			int before = 0;
			if (positionComboBox.Text == "Before")
				before = 1;

			for (int i = 1; i <= pageNumber - before; i++)
				mappingString += i.ToString() + " ";

			for (int i = 1; i <= mInsertedDoc.PageCount; i++)
				mappingString += (i + origDocPageCount).ToString() + " ";

			for (int i = pageNumber + 1 - before; i <= origDocPageCount; i++)
				mappingString += i.ToString() + " ";

			mCurrentDoc.RemapPages(mappingString);
			mCurrentDoc.PageNumber = pageNumber + 1 - before;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void pageRadioButton_CheckedChanged(object sender, System.EventArgs e) {
			pageNumberTextBox.Enabled = pageRadioButton.Checked;
		}
	}
}
