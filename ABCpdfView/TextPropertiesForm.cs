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

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for TextPropertiesForm.
	/// </summary>
	public class TextPropertiesForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox selectedBlockTextBox;
		private System.Windows.Forms.GroupBox textStyleGroupBox;
		private System.Windows.Forms.ComboBox fontNameComboBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button fontColorButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TextBlock mBlock;
		private Color mTextColor;
		private AnnotatedContent mAnnotatedContent;

		internal TextPropertiesForm(TextBlock block, AnnotatedContent annotatedContent) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			mBlock = block;
			selectedBlockTextBox.Text = block.Text;
			mTextColor = block.TextColor;
			mAnnotatedContent = annotatedContent;

			string fontFamily = block.FontFamily;
			if (!fontNameComboBox.Items.Contains(fontFamily))
				fontNameComboBox.SelectedIndex = fontNameComboBox.Items.Add(fontFamily);
			else
				fontNameComboBox.SelectedIndex = fontNameComboBox.Items.IndexOf(fontFamily);
		}

		internal bool CanChange {
			set {
				okButton.Enabled = value;
				selectedBlockTextBox.ReadOnly = !value;
			}
		}

		internal bool CanCopy {
			set {
				selectedBlockTextBox.Enabled = value;
			}
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
			this.selectedBlockTextBox = new System.Windows.Forms.TextBox();
			this.textStyleGroupBox = new System.Windows.Forms.GroupBox();
			this.fontNameComboBox = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.fontColorButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.textStyleGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Text:";
			// 
			// selectedBlockTextBox
			// 
			this.selectedBlockTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.selectedBlockTextBox.Location = new System.Drawing.Point(56, 8);
			this.selectedBlockTextBox.Multiline = true;
			this.selectedBlockTextBox.Name = "selectedBlockTextBox";
			this.selectedBlockTextBox.Size = new System.Drawing.Size(240, 48);
			this.selectedBlockTextBox.TabIndex = 4;
			this.selectedBlockTextBox.Text = "";
			// 
			// textStyleGroupBox
			// 
			this.textStyleGroupBox.Controls.Add(this.fontNameComboBox);
			this.textStyleGroupBox.Controls.Add(this.label6);
			this.textStyleGroupBox.Controls.Add(this.fontColorButton);
			this.textStyleGroupBox.Controls.Add(this.label5);
			this.textStyleGroupBox.Location = new System.Drawing.Point(8, 72);
			this.textStyleGroupBox.Name = "textStyleGroupBox";
			this.textStyleGroupBox.Size = new System.Drawing.Size(288, 88);
			this.textStyleGroupBox.TabIndex = 7;
			this.textStyleGroupBox.TabStop = false;
			this.textStyleGroupBox.Text = "Text Style";
			// 
			// fontNameComboBox
			// 
			this.fontNameComboBox.Items.AddRange(new object[] {
																  "Arial",
																  "Arial Black",
																  "Arial Black Italic",
																  "Times-Roman",
																  "Times-Bold",
																  "Times-Italic",
																  "Times-BoldItalic",
																  "Helvetica",
																  "Helvetica-Bold",
																  "Helvetica-Oblique",
																  "Helvetica-BoldOblique",
																  "Courier",
																  "Courier-Bold",
																  "Courier-Oblique",
																  "Courier-BoldOblique",
																  "Symbol",
																  "ZapfDingbats"});
			this.fontNameComboBox.Location = new System.Drawing.Point(72, 56);
			this.fontNameComboBox.Name = "fontNameComboBox";
			this.fontNameComboBox.Size = new System.Drawing.Size(208, 21);
			this.fontNameComboBox.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 55);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 23);
			this.label6.TabIndex = 10;
			this.label6.Text = "Font:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fontColorButton
			// 
			this.fontColorButton.Location = new System.Drawing.Point(72, 24);
			this.fontColorButton.Name = "fontColorButton";
			this.fontColorButton.Size = new System.Drawing.Size(24, 20);
			this.fontColorButton.TabIndex = 9;
			this.fontColorButton.Click += new System.EventHandler(this.fontColorButton_Click);
			this.fontColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.fontColorButton_Paint);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Text color:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(136, 168);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 9;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(221, 168);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "Cancel";
			// 
			// TextPropertiesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(304, 200);
			this.ControlBox = false;
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.selectedBlockTextBox);
			this.Controls.Add(this.textStyleGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MinimizeBox = false;
			this.Name = "TextPropertiesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Text Properties";
			this.textStyleGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void fontColorButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
			Graphics g = e.Graphics;
			g.FillRectangle(new SolidBrush(mTextColor), new Rectangle(4, 4, fontColorButton.Size.Width - 8, fontColorButton.Size.Height - 8));
		
		}

		private void fontColorButton_Click(object sender, System.EventArgs e) {
			ColorDialog theDialog = new ColorDialog();
			theDialog.Color = mTextColor;
			DialogResult res = theDialog.ShowDialog();
			if( res == DialogResult.OK ) {
				mTextColor = theDialog.Color;
				fontColorButton.Refresh();
			}
	
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			string fontName = "";
			if (fontNameComboBox.Text != mBlock.FontFamily)
				fontName = fontNameComboBox.Text;

			mAnnotatedContent.UpdateTextBlock(mBlock, selectedBlockTextBox.Text, mTextColor, fontName);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
