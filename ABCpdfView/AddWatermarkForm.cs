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
using System.IO;
using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for AddWatermarkForm.
	/// </summary>
	public class AddWatermarkForm : System.Windows.Forms.Form {
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.PictureBox pagePreviewPictureBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox watermarkTextBox;
		private System.Windows.Forms.NumericUpDown verticalOffsetUpDown;
		private System.Windows.Forms.NumericUpDown horizontalOffsetUpDown;
		private System.Windows.Forms.RadioButton imageWatermarkRadioButton;
		private System.Windows.Forms.RadioButton textWatermarkRadioButton;
		private System.Windows.Forms.ComboBox fontNameComboBox;
		private System.Windows.Forms.NumericUpDown fontSizeNumericUpDown;
		private System.Windows.Forms.Button fontColorButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar opacityPercentTrackBar;
		private System.Windows.Forms.NumericUpDown opacityPercentNumericUpDown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button selectImageButton;
		private System.Windows.Forms.TextBox imageFileTextBox;

		private Doc mDoc;
		private int m_iTextId = -1;
		private int m_iImageId = -1;
		private int m_iImageWitdth;
		private int m_iImageHeight;
		private System.Windows.Forms.CheckBox allPagesCheckBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown scaleNumericUpDown;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown rotateNumericUpDown;
		private System.Windows.Forms.GroupBox positionGroupBox;
		private System.Windows.Forms.GroupBox appearanceGroupBox;
		private Color mColor = Color.Black;

		public AddWatermarkForm(Doc currentDoc) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mDoc = currentDoc;
			fontNameComboBox.SelectedIndex = 0;
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pagePreviewPictureBox = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.fontSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.fontColorButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.fontNameComboBox = new System.Windows.Forms.ComboBox();
			this.selectImageButton = new System.Windows.Forms.Button();
			this.imageFileTextBox = new System.Windows.Forms.TextBox();
			this.imageWatermarkRadioButton = new System.Windows.Forms.RadioButton();
			this.textWatermarkRadioButton = new System.Windows.Forms.RadioButton();
			this.watermarkTextBox = new System.Windows.Forms.TextBox();
			this.positionGroupBox = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.rotateNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.scaleNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.horizontalOffsetUpDown = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.verticalOffsetUpDown = new System.Windows.Forms.NumericUpDown();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.appearanceGroupBox = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.opacityPercentNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.opacityPercentTrackBar = new System.Windows.Forms.TrackBar();
			this.allPagesCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).BeginInit();
			this.positionGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rotateNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.horizontalOffsetUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.verticalOffsetUpDown)).BeginInit();
			this.appearanceGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.opacityPercentNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.opacityPercentTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pagePreviewPictureBox);
			this.groupBox1.Location = new System.Drawing.Point(296, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(264, 312);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// pagePreviewPictureBox
			// 
			this.pagePreviewPictureBox.Location = new System.Drawing.Point(16, 24);
			this.pagePreviewPictureBox.Name = "pagePreviewPictureBox";
			this.pagePreviewPictureBox.Size = new System.Drawing.Size(240, 280);
			this.pagePreviewPictureBox.TabIndex = 0;
			this.pagePreviewPictureBox.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.fontSizeNumericUpDown);
			this.groupBox2.Controls.Add(this.fontColorButton);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.fontNameComboBox);
			this.groupBox2.Controls.Add(this.selectImageButton);
			this.groupBox2.Controls.Add(this.imageFileTextBox);
			this.groupBox2.Controls.Add(this.imageWatermarkRadioButton);
			this.groupBox2.Controls.Add(this.textWatermarkRadioButton);
			this.groupBox2.Controls.Add(this.watermarkTextBox);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(280, 176);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Source";
			// 
			// fontSizeNumericUpDown
			// 
			this.fontSizeNumericUpDown.Location = new System.Drawing.Point(192, 80);
			this.fontSizeNumericUpDown.Maximum = new System.Decimal(new int[] {
																				  1000,
																				  0,
																				  0,
																				  0});
			this.fontSizeNumericUpDown.Minimum = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.fontSizeNumericUpDown.Name = "fontSizeNumericUpDown";
			this.fontSizeNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.fontSizeNumericUpDown.TabIndex = 9;
			this.fontSizeNumericUpDown.Value = new System.Decimal(new int[] {
																				100,
																				0,
																				0,
																				0});
			this.fontSizeNumericUpDown.ValueChanged += new System.EventHandler(this.fontSizeNumericUpDown_ValueChanged);
			// 
			// fontColorButton
			// 
			this.fontColorButton.Location = new System.Drawing.Point(248, 80);
			this.fontColorButton.Name = "fontColorButton";
			this.fontColorButton.Size = new System.Drawing.Size(24, 20);
			this.fontColorButton.TabIndex = 8;
			this.fontColorButton.Click += new System.EventHandler(this.fontColorButton_Click);
			this.fontColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.fontColorButton_Paint);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Font:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fontNameComboBox
			// 
			this.fontNameComboBox.DisplayMember = "Arial";
			this.fontNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
			this.fontNameComboBox.Location = new System.Drawing.Point(64, 80);
			this.fontNameComboBox.Name = "fontNameComboBox";
			this.fontNameComboBox.Size = new System.Drawing.Size(112, 21);
			this.fontNameComboBox.TabIndex = 5;
			this.fontNameComboBox.SelectedIndexChanged += new System.EventHandler(this.fontNameComboBox_SelectedIndexChanged);
			// 
			// selectImageButton
			// 
			this.selectImageButton.Enabled = false;
			this.selectImageButton.Location = new System.Drawing.Point(248, 144);
			this.selectImageButton.Name = "selectImageButton";
			this.selectImageButton.Size = new System.Drawing.Size(24, 20);
			this.selectImageButton.TabIndex = 4;
			this.selectImageButton.Text = "...";
			this.selectImageButton.Click += new System.EventHandler(this.selectImageButton_Click);
			// 
			// imageFileTextBox
			// 
			this.imageFileTextBox.Enabled = false;
			this.imageFileTextBox.Location = new System.Drawing.Point(32, 144);
			this.imageFileTextBox.Name = "imageFileTextBox";
			this.imageFileTextBox.Size = new System.Drawing.Size(208, 20);
			this.imageFileTextBox.TabIndex = 3;
			this.imageFileTextBox.Text = "";
			this.imageFileTextBox.TextChanged += new System.EventHandler(this.imageFileTextBox_TextChanged);
			// 
			// imageWatermarkRadioButton
			// 
			this.imageWatermarkRadioButton.Location = new System.Drawing.Point(16, 112);
			this.imageWatermarkRadioButton.Name = "imageWatermarkRadioButton";
			this.imageWatermarkRadioButton.Size = new System.Drawing.Size(80, 24);
			this.imageWatermarkRadioButton.TabIndex = 2;
			this.imageWatermarkRadioButton.Text = "Image file:";
			this.imageWatermarkRadioButton.CheckedChanged += new System.EventHandler(this.imageWatermarkRadioButton_CheckedChanged);
			// 
			// textWatermarkRadioButton
			// 
			this.textWatermarkRadioButton.Checked = true;
			this.textWatermarkRadioButton.Location = new System.Drawing.Point(16, 24);
			this.textWatermarkRadioButton.Name = "textWatermarkRadioButton";
			this.textWatermarkRadioButton.Size = new System.Drawing.Size(48, 24);
			this.textWatermarkRadioButton.TabIndex = 1;
			this.textWatermarkRadioButton.TabStop = true;
			this.textWatermarkRadioButton.Text = "Text:";
			// 
			// watermarkTextBox
			// 
			this.watermarkTextBox.AcceptsReturn = true;
			this.watermarkTextBox.Location = new System.Drawing.Point(72, 16);
			this.watermarkTextBox.Multiline = true;
			this.watermarkTextBox.Name = "watermarkTextBox";
			this.watermarkTextBox.Size = new System.Drawing.Size(200, 48);
			this.watermarkTextBox.TabIndex = 0;
			this.watermarkTextBox.Text = "";
			this.watermarkTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.watermarkTextBox_KeyDown);
			this.watermarkTextBox.Leave += new System.EventHandler(this.watermarkTextBox_Leave);
			// 
			// positionGroupBox
			// 
			this.positionGroupBox.Controls.Add(this.label11);
			this.positionGroupBox.Controls.Add(this.rotateNumericUpDown);
			this.positionGroupBox.Controls.Add(this.label10);
			this.positionGroupBox.Controls.Add(this.label9);
			this.positionGroupBox.Controls.Add(this.scaleNumericUpDown);
			this.positionGroupBox.Controls.Add(this.label8);
			this.positionGroupBox.Controls.Add(this.label7);
			this.positionGroupBox.Controls.Add(this.label6);
			this.positionGroupBox.Controls.Add(this.horizontalOffsetUpDown);
			this.positionGroupBox.Controls.Add(this.label3);
			this.positionGroupBox.Controls.Add(this.label2);
			this.positionGroupBox.Controls.Add(this.verticalOffsetUpDown);
			this.positionGroupBox.Enabled = false;
			this.positionGroupBox.Location = new System.Drawing.Point(8, 192);
			this.positionGroupBox.Name = "positionGroupBox";
			this.positionGroupBox.Size = new System.Drawing.Size(280, 160);
			this.positionGroupBox.TabIndex = 2;
			this.positionGroupBox.TabStop = false;
			this.positionGroupBox.Text = "Position";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(128, 128);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(148, 16);
			this.label11.TabIndex = 11;
			this.label11.Text = "deggrees (counterclockwise)";
			// 
			// rotateNumericUpDown
			// 
			this.rotateNumericUpDown.Increment = new System.Decimal(new int[] {
																				  10,
																				  0,
																				  0,
																				  0});
			this.rotateNumericUpDown.Location = new System.Drawing.Point(64, 128);
			this.rotateNumericUpDown.Maximum = new System.Decimal(new int[] {
																				360,
																				0,
																				0,
																				0});
			this.rotateNumericUpDown.Name = "rotateNumericUpDown";
			this.rotateNumericUpDown.Size = new System.Drawing.Size(56, 20);
			this.rotateNumericUpDown.TabIndex = 10;
			this.rotateNumericUpDown.ValueChanged += new System.EventHandler(this.rotateNumericUpDown_ValueChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 128);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 16);
			this.label10.TabIndex = 9;
			this.label10.Text = "Rotation:";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(128, 96);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(16, 16);
			this.label9.TabIndex = 8;
			this.label9.Text = "%";
			// 
			// scaleNumericUpDown
			// 
			this.scaleNumericUpDown.Increment = new System.Decimal(new int[] {
																				 10,
																				 0,
																				 0,
																				 0});
			this.scaleNumericUpDown.Location = new System.Drawing.Point(64, 94);
			this.scaleNumericUpDown.Maximum = new System.Decimal(new int[] {
																			   1000,
																			   0,
																			   0,
																			   0});
			this.scaleNumericUpDown.Name = "scaleNumericUpDown";
			this.scaleNumericUpDown.Size = new System.Drawing.Size(56, 20);
			this.scaleNumericUpDown.TabIndex = 7;
			this.scaleNumericUpDown.Value = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.scaleNumericUpDown.ValueChanged += new System.EventHandler(this.scaleNumericUpDown_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 96);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 6;
			this.label8.Text = "Scale:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(160, 60);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(16, 16);
			this.label7.TabIndex = 5;
			this.label7.Text = "pt";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(160, 26);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 4;
			this.label6.Text = "pt";
			// 
			// horizontalOffsetUpDown
			// 
			this.horizontalOffsetUpDown.Increment = new System.Decimal(new int[] {
																					 36,
																					 0,
																					 0,
																					 0});
			this.horizontalOffsetUpDown.Location = new System.Drawing.Point(96, 24);
			this.horizontalOffsetUpDown.Maximum = new System.Decimal(new int[] {
																				   10000000,
																				   0,
																				   0,
																				   0});
			this.horizontalOffsetUpDown.Name = "horizontalOffsetUpDown";
			this.horizontalOffsetUpDown.Size = new System.Drawing.Size(56, 20);
			this.horizontalOffsetUpDown.TabIndex = 3;
			this.horizontalOffsetUpDown.ValueChanged += new System.EventHandler(this.horizontalOffsetUpDown_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Horizontal offset:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Vertical offset:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// verticalOffsetUpDown
			// 
			this.verticalOffsetUpDown.Increment = new System.Decimal(new int[] {
																				   36,
																				   0,
																				   0,
																				   0});
			this.verticalOffsetUpDown.Location = new System.Drawing.Point(96, 58);
			this.verticalOffsetUpDown.Maximum = new System.Decimal(new int[] {
																				 10000000,
																				 0,
																				 0,
																				 0});
			this.verticalOffsetUpDown.Name = "verticalOffsetUpDown";
			this.verticalOffsetUpDown.Size = new System.Drawing.Size(56, 20);
			this.verticalOffsetUpDown.TabIndex = 0;
			this.verticalOffsetUpDown.Value = new System.Decimal(new int[] {
																			   792,
																			   0,
																			   0,
																			   0});
			this.verticalOffsetUpDown.ValueChanged += new System.EventHandler(this.verticalOffsetUpDown_ValueChanged);
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(400, 409);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 9;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(488, 409);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// appearanceGroupBox
			// 
			this.appearanceGroupBox.Controls.Add(this.label5);
			this.appearanceGroupBox.Controls.Add(this.opacityPercentNumericUpDown);
			this.appearanceGroupBox.Controls.Add(this.label4);
			this.appearanceGroupBox.Controls.Add(this.opacityPercentTrackBar);
			this.appearanceGroupBox.Enabled = false;
			this.appearanceGroupBox.Location = new System.Drawing.Point(8, 360);
			this.appearanceGroupBox.Name = "appearanceGroupBox";
			this.appearanceGroupBox.Size = new System.Drawing.Size(280, 72);
			this.appearanceGroupBox.TabIndex = 11;
			this.appearanceGroupBox.TabStop = false;
			this.appearanceGroupBox.Text = "Appearance";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(262, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 23);
			this.label5.TabIndex = 3;
			this.label5.Text = "%";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// opacityPercentNumericUpDown
			// 
			this.opacityPercentNumericUpDown.Location = new System.Drawing.Point(222, 22);
			this.opacityPercentNumericUpDown.Name = "opacityPercentNumericUpDown";
			this.opacityPercentNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.opacityPercentNumericUpDown.TabIndex = 2;
			this.opacityPercentNumericUpDown.Value = new System.Decimal(new int[] {
																					  100,
																					  0,
																					  0,
																					  0});
			this.opacityPercentNumericUpDown.ValueChanged += new System.EventHandler(this.opacityPercentNumericUpDown_ValueChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Opacity:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// opacityPercentTrackBar
			// 
			this.opacityPercentTrackBar.Location = new System.Drawing.Point(64, 20);
			this.opacityPercentTrackBar.Maximum = 100;
			this.opacityPercentTrackBar.Name = "opacityPercentTrackBar";
			this.opacityPercentTrackBar.Size = new System.Drawing.Size(152, 45);
			this.opacityPercentTrackBar.TabIndex = 1;
			this.opacityPercentTrackBar.TickFrequency = 0;
			this.opacityPercentTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.opacityPercentTrackBar.Value = 100;
			this.opacityPercentTrackBar.Scroll += new System.EventHandler(this.opacityPercentTrackBar_Scroll);
			// 
			// allPagesCheckBox
			// 
			this.allPagesCheckBox.Location = new System.Drawing.Point(296, 336);
			this.allPagesCheckBox.Name = "allPagesCheckBox";
			this.allPagesCheckBox.Size = new System.Drawing.Size(200, 24);
			this.allPagesCheckBox.TabIndex = 12;
			this.allPagesCheckBox.Text = "Apply to all pages in the document";
			// 
			// AddWatermarkForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 440);
			this.ControlBox = false;
			this.Controls.Add(this.appearanceGroupBox);
			this.Controls.Add(this.allPagesCheckBox);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.positionGroupBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddWatermarkForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Watermark";
			this.Load += new System.EventHandler(this.AddWatermarkForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).EndInit();
			this.positionGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.rotateNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.horizontalOffsetUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.verticalOffsetUpDown)).EndInit();
			this.appearanceGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.opacityPercentNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.opacityPercentTrackBar)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void UpdatePreview() {
			if (mDoc == null || mDoc.PageCount == 0 || mDoc.Page == 0)
				return;

			double sw = pagePreviewPictureBox.Width - 4;
			double sh = pagePreviewPictureBox.Height - 4;

			double resX = (sw * 72) / mDoc.Rect.Width;
			double resY = (sh * 72) / mDoc.Rect.Height;
			double res = resX > resY ? resY : resX;
			double width = mDoc.Rect.Width * res / 72.0;
			double height = mDoc.Rect.Height * res / 72.0;
			double x = (pagePreviewPictureBox.Width - width) / 2;
			double y = (pagePreviewPictureBox.Height - height) / 2;
			RectangleF theRect = new RectangleF((float)x, (float)y, (float)width, (float)height);
			
			double saveDPI = mDoc.Rendering.DotsPerInch;
			mDoc.Rendering.DotsPerInch = res;
			mDoc.Rendering.ColorSpace = XRendering.ColorSpaceType.Rgb;
			mDoc.Rendering.BitsPerChannel = 8;
			Bitmap bm = mDoc.Rendering.GetBitmap();
			pagePreviewPictureBox.Image = bm;
			mDoc.Rendering.DotsPerInch = saveDPI;
		}

		bool m_bFirstTime = true;

		private bool FirstTime {
			get {
				return m_bFirstTime;
			}
			set {
				if( value == false) {
					positionGroupBox.Enabled = true;
					appearanceGroupBox.Enabled = true;
				}
				m_bFirstTime = value;
			}
		}

		private void UpdateWatermark(bool deleteOld) {
			if (deleteOld && m_iTextId > 0)
				mDoc.Delete(m_iTextId);
			
			int rotate = (int)rotateNumericUpDown.Value;
			double scale = (double)scaleNumericUpDown.Value / 100;
			mDoc.Transform.Reset();

			if (textWatermarkRadioButton.Checked) {
				if (m_iTextId <= 0 && watermarkTextBox.Text == "")
					return;

				string savePos = mDoc.Pos.String;
				string saveRect = mDoc.Rect.String;
				mDoc.Pos.X = (double)horizontalOffsetUpDown.Value;
				mDoc.Rect.Left = mDoc.Pos.X;
				mDoc.Pos.Y = (double)verticalOffsetUpDown.Value;
				mDoc.Font = mDoc.AddFont(fontNameComboBox.Text);
				mDoc.FontSize = (int)fontSizeNumericUpDown.Value;
				mDoc.Color.Color = mColor;
				mDoc.Color.Alpha = GetAlpha();
				m_iTextId = mDoc.AddText(watermarkTextBox.Text);

				XRect theRect = new XRect();
				theRect.String = mDoc.GetInfo(m_iTextId, "Rect");
				mDoc.Delete(m_iTextId);
				
				mDoc.Transform.Rotate(rotate, (theRect.Left + theRect.Right)/2, (theRect.Bottom + theRect.Top)/2);
				mDoc.Transform.Magnify(scale, scale, (theRect.Left + theRect.Right)/2, (theRect.Bottom + theRect.Top)/2);

				if (FirstTime) {
					mDoc.Pos.X = (mDoc.CropBox.Width - theRect.Width) / 2;
					mDoc.Pos.Y = (mDoc.CropBox.Height + theRect.Height) / 2;
					horizontalOffsetUpDown.Value = (decimal)mDoc.Pos.X;
					verticalOffsetUpDown.Value = (decimal)mDoc.Pos.Y;
					FirstTime = false;
				}
				else {
					mDoc.Pos.X = (double)horizontalOffsetUpDown.Value;
					mDoc.Pos.Y = (double)verticalOffsetUpDown.Value;
				}
				m_iTextId = mDoc.AddText(watermarkTextBox.Text);

				mDoc.Pos.String = savePos;
				mDoc.Rect.String = saveRect;
			}
			else if (imageWatermarkRadioButton.Checked) {
				if (File.Exists(imageFileTextBox.Text)) {

					bool bNewImage = (m_iImageId <= 0);

					XImage theImage = null;

					if (bNewImage) {
						try {
							theImage  = new XImage();
							theImage.SetFile(imageFileTextBox.Text);
							m_iImageWitdth = theImage.Width;
							m_iImageHeight = theImage.Height;
						}
						catch {
							MessageBox.Show("Can't add image. Image file is invalid.");
							return;
						}
					}

					string saveRect = mDoc.Rect.String;

					if (FirstTime) {
						mDoc.Rect.Bottom = (mDoc.CropBox.Height - m_iImageHeight)/2;
						mDoc.Rect.Left = (mDoc.CropBox.Width - m_iImageWitdth)/2;
						verticalOffsetUpDown.Value = (decimal)mDoc.Rect.Bottom;
						horizontalOffsetUpDown.Value = (decimal)mDoc.Rect.Left;
						FirstTime = false;
					}
					else {
						mDoc.Rect.Bottom = (double)verticalOffsetUpDown.Value;
						mDoc.Rect.Left = (double)horizontalOffsetUpDown.Value;
					}

					mDoc.Rect.Width = m_iImageWitdth;
					mDoc.Rect.Height = m_iImageHeight;

					mDoc.Transform.Rotate(rotate, (mDoc.Rect.Left + mDoc.Rect.Right)/2, (mDoc.Rect.Bottom + mDoc.Rect.Top)/2);
					mDoc.Transform.Magnify(scale, scale, (mDoc.Rect.Left + mDoc.Rect.Right)/2, (mDoc.Rect.Bottom + mDoc.Rect.Top)/2);

					if (bNewImage) 
						m_iImageId = mDoc.AddImageFile(imageFileTextBox.Text, 1);
					else {
						int newImage = mDoc.AddImageCopy(m_iImageId);
						if (deleteOld)
							mDoc.Delete(m_iImageId);
						m_iImageId = newImage;
					}

					ImageLayer im = (ImageLayer)mDoc.ObjectSoup[m_iImageId];
					im.PixMap.SetAlpha(GetAlpha());

					mDoc.Rect.String = saveRect;
				}
			}
			if (deleteOld)
				UpdatePreview();
		}

		private void AddWatermarkForm_Load(object sender, System.EventArgs e) {
			UpdatePreview();
		}


		private void watermarkTextBox_Leave(object sender, System.EventArgs e) {
			UpdateWatermark(true);
		}

		private void watermarkTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				UpdateWatermark(true);
			}
		}

		private void verticalOffsetUpDown_ValueChanged(object sender, System.EventArgs e) {
			if (!FirstTime)
				UpdateWatermark(true);
		}

		private void horizontalOffsetUpDown_ValueChanged(object sender, System.EventArgs e) {
			if (!FirstTime)
				UpdateWatermark(true);
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			if (allPagesCheckBox.Checked) {
				int currentPageNumber = mDoc.PageNumber;
				for (int i = 1; i <= mDoc.PageCount; i++) {
					if ( i != currentPageNumber) {
						mDoc.PageNumber = i;
						UpdateWatermark(false);
					}
				}
				mDoc.PageNumber = currentPageNumber;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void fontNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e) {
			UpdateWatermark(true);
		}

		private void fontSizeNumericUpDown_ValueChanged(object sender, System.EventArgs e) {
			UpdateWatermark(true);
		}

		private void fontColorButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
			Graphics g = e.Graphics;
			g.FillRectangle(new SolidBrush(mColor), new Rectangle(4, 4, fontColorButton.Size.Width - 8, fontColorButton.Size.Height - 8));
		}

		private void fontColorButton_Click(object sender, System.EventArgs e) {
			ColorDialog theDialog = new ColorDialog();
			theDialog.Color = mColor;
			DialogResult res = theDialog.ShowDialog();
			if( res == DialogResult.OK ) {
				mColor = theDialog.Color;
				fontColorButton.Refresh();
				UpdateWatermark(true);
			}
		}

		private void opacityPercentNumericUpDown_ValueChanged(object sender, System.EventArgs e) {
			opacityPercentTrackBar.Value = (int)opacityPercentNumericUpDown.Value;
			UpdateWatermark(true);
		}

		private void opacityPercentTrackBar_Scroll(object sender, System.EventArgs e) {
			opacityPercentNumericUpDown.Value = opacityPercentTrackBar.Value;
		}

		private int GetAlpha() {
			return 255 * (int)opacityPercentNumericUpDown.Value /100;
		}

		private void selectImageButton_Click(object sender, System.EventArgs e) {
			OpenFileDialog od = new OpenFileDialog();
			od.Filter = "Image files (*.jpg;*.bmp;*.png;*.tif)|*.jpg;*.bmp;*.png;*.tif|All files (*.*)|*.*" ;
			od.FilterIndex = 1;

			if (od.ShowDialog() == DialogResult.OK) {
				imageFileTextBox.Text = od.FileName;
			}
		}

		private void imageWatermarkRadioButton_CheckedChanged(object sender, System.EventArgs e) {
			//Text watermark section
			fontNameComboBox.Enabled = !imageWatermarkRadioButton.Checked;
			fontSizeNumericUpDown.Enabled = !imageWatermarkRadioButton.Checked;
			fontColorButton.Enabled = !imageWatermarkRadioButton.Checked;
			watermarkTextBox.Enabled = !imageWatermarkRadioButton.Checked;

			//Image watermark section
			imageFileTextBox.Enabled = imageWatermarkRadioButton.Checked;
			selectImageButton.Enabled = imageWatermarkRadioButton.Checked;

			if (m_iImageId > 0 && !imageWatermarkRadioButton.Checked) {
				mDoc.Delete(m_iImageId);
				m_iImageId = 0;
			}
			if (m_iTextId > 0 && imageWatermarkRadioButton.Checked) {
				mDoc.Delete(m_iTextId);
				m_iTextId = 0;
			}

			UpdateWatermark(true);
		}

		private void imageFileTextBox_TextChanged(object sender, System.EventArgs e) {
			if (m_iImageId > 0) {
				mDoc.Delete(m_iImageId);
				m_iImageId = 0;
			}
			UpdateWatermark(true);
		}

		private void cancelButton_Click(object sender, System.EventArgs e) {
			if (m_iImageId > 0)
				mDoc.Delete(m_iImageId);
			if (m_iTextId > 0)
				mDoc.Delete(m_iTextId);
		
		}

		private void scaleNumericUpDown_ValueChanged(object sender, System.EventArgs e) {
			UpdateWatermark(true);
		}

		private void rotateNumericUpDown_ValueChanged(object sender, System.EventArgs e) {
			UpdateWatermark(true);
		}
	}
}
