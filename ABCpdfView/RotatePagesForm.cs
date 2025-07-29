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
	/// Summary description for RotatePagesForm.
	/// </summary>
	public class RotatePagesForm : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label pagesNumberLabel;
		private System.Windows.Forms.TextBox toPageTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox fromPageTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton allRadioButton;
		private System.Windows.Forms.RadioButton pagesRadioButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox directionComboBox;
		private Doc mDoc;

		public RotatePagesForm(Doc currentDoc) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mDoc = currentDoc;
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
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pagesNumberLabel = new System.Windows.Forms.Label();
			this.toPageTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.fromPageTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pagesRadioButton = new System.Windows.Forms.RadioButton();
			this.allRadioButton = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.directionComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(176, 176);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 7;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(264, 176);
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
			this.label3.Text = "Rotate selected range of pages.";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(80, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pagesNumberLabel
			// 
			this.pagesNumberLabel.Location = new System.Drawing.Point(256, 56);
			this.pagesNumberLabel.Name = "pagesNumberLabel";
			this.pagesNumberLabel.Size = new System.Drawing.Size(56, 23);
			this.pagesNumberLabel.TabIndex = 4;
			this.pagesNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toPageTextBox
			// 
			this.toPageTextBox.Location = new System.Drawing.Point(216, 56);
			this.toPageTextBox.Name = "toPageTextBox";
			this.toPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.toPageTextBox.TabIndex = 5;
			this.toPageTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(176, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fromPageTextBox
			// 
			this.fromPageTextBox.Location = new System.Drawing.Point(128, 56);
			this.fromPageTextBox.Name = "fromPageTextBox";
			this.fromPageTextBox.Size = new System.Drawing.Size(40, 20);
			this.fromPageTextBox.TabIndex = 4;
			this.fromPageTextBox.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pagesRadioButton);
			this.groupBox1.Controls.Add(this.allRadioButton);
			this.groupBox1.Controls.Add(this.fromPageTextBox);
			this.groupBox1.Controls.Add(this.toPageTextBox);
			this.groupBox1.Controls.Add(this.pagesNumberLabel);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(328, 88);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Page Range";
			// 
			// pagesRadioButton
			// 
			this.pagesRadioButton.Checked = true;
			this.pagesRadioButton.Location = new System.Drawing.Point(8, 56);
			this.pagesRadioButton.Name = "pagesRadioButton";
			this.pagesRadioButton.Size = new System.Drawing.Size(56, 24);
			this.pagesRadioButton.TabIndex = 3;
			this.pagesRadioButton.TabStop = true;
			this.pagesRadioButton.Text = "Pages";
			this.pagesRadioButton.CheckedChanged += new System.EventHandler(this.pagesRadioButton_CheckedChanged);
			// 
			// allRadioButton
			// 
			this.allRadioButton.Location = new System.Drawing.Point(8, 24);
			this.allRadioButton.Name = "allRadioButton";
			this.allRadioButton.Size = new System.Drawing.Size(48, 24);
			this.allRadioButton.TabIndex = 2;
			this.allRadioButton.Text = "All";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 11;
			this.label4.Text = "Rotate Direction:";
			// 
			// directionComboBox
			// 
			this.directionComboBox.Items.AddRange(new object[] {
																   "Clockwise 90 degrees",
																   "Counterclockwise 90 degrees",
																   "180 degrees"});
			this.directionComboBox.Location = new System.Drawing.Point(104, 38);
			this.directionComboBox.Name = "directionComboBox";
			this.directionComboBox.Size = new System.Drawing.Size(168, 21);
			this.directionComboBox.TabIndex = 1;
			// 
			// RotatePagesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(346, 208);
			this.ControlBox = false;
			this.Controls.Add(this.directionComboBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "RotatePagesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rotate Pages";
			this.Load += new System.EventHandler(this.RotatePagesForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void RotatePagesForm_Load(object sender, System.EventArgs e) {
			if (mDoc != null) {
				fromPageTextBox.Text = mDoc.PageNumber.ToString();
				toPageTextBox.Text = mDoc.PageNumber.ToString();
				pagesNumberLabel.Text = "of " + mDoc.PageCount.ToString();
			}
			directionComboBox.SelectedIndex = 0;
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			int fromPage = 0, toPage = 0;
			bool error = false;

			if (allRadioButton.Checked) {
				fromPage = 1;
				toPage = mDoc.PageCount;
			}
			else {
				try {
					fromPage = int.Parse(fromPageTextBox.Text);
				}
				catch {
					error = true;
				}

				if (fromPage <= 0 || fromPage > mDoc.PageCount )
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

				if (toPage <= 0 || toPage > mDoc.PageCount )
					error = true;

				if (error) {
					MessageBox.Show("There is no page numbered '" + toPageTextBox.Text + "' in this document.");
					return;
				}

				if (toPage < fromPage) {
					MessageBox.Show("The starting page number must be less then or the same as the ending page number.");
					return;
				}
			}

			int angleChange = 0;

			switch (directionComboBox.SelectedIndex) {
				case 0:
					angleChange = 90;
					break;
				case 1:
					angleChange = -90;
					break;
				case 2:
					angleChange = 180;
					break;
			}

			int savePage = mDoc.PageNumber;
			for (int i = fromPage; i <= toPage; i++) {
				mDoc.PageNumber = i;
				int currentAngle = PDFUtilities.GetPageRotation(mDoc);
				currentAngle = (currentAngle + angleChange + 360) % 360;
				PDFUtilities.SetPageRotation(mDoc, currentAngle);
			}
			mDoc.PageNumber = savePage;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void pagesRadioButton_CheckedChanged(object sender, System.EventArgs e) {
			fromPageTextBox.Enabled = pagesRadioButton.Checked;
			toPageTextBox.Enabled = pagesRadioButton.Checked;
		}
	}
}
