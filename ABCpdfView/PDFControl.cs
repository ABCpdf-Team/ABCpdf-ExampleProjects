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

//#define DISPLAY_TEXT_RECTANGLES
//#define DISPLAY_PRINT_AREA
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Drawing.Drawing2D;
using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for PDFControl.
	/// </summary>
	public class PDFControl : System.Windows.Forms.UserControl {
		#region Private types and members
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button nextPageButton;
		private System.Windows.Forms.Button prevPageButton;
		private System.Windows.Forms.NumericUpDown dpiNumericUpDown;
		private System.Windows.Forms.TextBox pageNumberTextBox;
		private System.Windows.Forms.Label dpiLabel;
		private System.Windows.Forms.ImageList iconsImageList;
		private System.Windows.Forms.ContextMenu zoomContextMenu;
		private System.Windows.Forms.MenuItem zoomInMenuItem;
		private System.Windows.Forms.MenuItem zoomOutMenuItem;
		private System.Windows.Forms.ToolBarButton openToolBarButton;
		private System.Windows.Forms.ToolBarButton saveToolBarButton;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton printToolBarButton;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton textToolBarButton;
		private System.Windows.Forms.PictureBox renderedDocPictureBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel textEditingPanel;
		private System.Windows.Forms.TextBox selectedBlockTextBox;
		private System.Windows.Forms.Panel layoutPanel;
		private System.Windows.Forms.Panel imagePanel;
		private System.Windows.Forms.Splitter panelsSplitter;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.Button addTextButton;
		private System.Windows.Forms.GroupBox textStyleGroupBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox fitToSelectionCheckBox;
		private System.Windows.Forms.NumericUpDown fontSizeNumericUpDown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button fontColorButton;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox fontNameComboBox;
		private System.Windows.Forms.ToolBarButton selectToolBarButton;
		private System.Windows.Forms.ToolBarButton zoomInToolBarButton;
		private System.Windows.Forms.ToolBarButton zoomOutToolBarButton;

		/// <summary>
		/// Type of the tool
		/// </summary>
		private enum ToolType {
			HandTool,
			ZoomInTool,
			ZoomOutTool,
			TextTool,
			TextAddTool,
			SelectTool
		}
		private Doc				mDoc = null;
		private AnnotatedContent mAnnotatedContent = null;
		private string			mPath = "";
		private PrintDocument	mPrint = null;
		private	int				mPage = 0;
		private	int				mDotsPerInch = 0;
		private int				mPageSaved = 0;
		private int				mCopiesNumber;
		private ToolType		mCurrentTool = ToolType.HandTool;
		private Cursor mZoomInCursor;
		private Cursor mZoomOutCursor;
		private bool mZoomToolReverted;
		private const int kZoomIncrement = 72;
		private const int kGrabBarSize = 4;
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFControl));
			this.dpiLabel = new System.Windows.Forms.Label();
			this.dpiNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.nextPageButton = new System.Windows.Forms.Button();
			this.prevPageButton = new System.Windows.Forms.Button();
			this.pageNumberTextBox = new System.Windows.Forms.TextBox();
			this.zoomContextMenu = new System.Windows.Forms.ContextMenu();
			this.zoomInMenuItem = new System.Windows.Forms.MenuItem();
			this.zoomOutMenuItem = new System.Windows.Forms.MenuItem();
			this.iconsImageList = new System.Windows.Forms.ImageList(this.components);
			this.openToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.saveToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.printToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.zoomInToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.textToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.zoomOutToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.selectToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.imagePanel = new System.Windows.Forms.Panel();
			this.renderedDocPictureBox = new System.Windows.Forms.PictureBox();
			this.panelsSplitter = new System.Windows.Forms.Splitter();
			this.textEditingPanel = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.textStyleGroupBox = new System.Windows.Forms.GroupBox();
			this.fontNameComboBox = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.fontColorButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.fitToSelectionCheckBox = new System.Windows.Forms.CheckBox();
			this.fontSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.addTextButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.selectedBlockTextBox = new System.Windows.Forms.TextBox();
			this.layoutPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.dpiNumericUpDown)).BeginInit();
			this.imagePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.renderedDocPictureBox)).BeginInit();
			this.textEditingPanel.SuspendLayout();
			this.textStyleGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).BeginInit();
			this.layoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// dpiLabel
			// 
			this.dpiLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.dpiLabel.Location = new System.Drawing.Point(28, 423);
			this.dpiLabel.Name = "dpiLabel";
			this.dpiLabel.Size = new System.Drawing.Size(80, 23);
			this.dpiLabel.TabIndex = 11;
			this.dpiLabel.Text = "Dots per inch:";
			this.dpiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.dpiLabel.Visible = false;
			// 
			// dpiNumericUpDown
			// 
			this.dpiNumericUpDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.dpiNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dpiNumericUpDown.Increment = new decimal(new int[] {
            72,
            0,
            0,
            0});
			this.dpiNumericUpDown.Location = new System.Drawing.Point(108, 424);
			this.dpiNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.dpiNumericUpDown.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.dpiNumericUpDown.Name = "dpiNumericUpDown";
			this.dpiNumericUpDown.Size = new System.Drawing.Size(48, 20);
			this.dpiNumericUpDown.TabIndex = 10;
			this.dpiNumericUpDown.Value = new decimal(new int[] {
            72,
            0,
            0,
            0});
			this.dpiNumericUpDown.Visible = false;
			this.dpiNumericUpDown.ValueChanged += new System.EventHandler(this.dpiNumericUpDown_ValueChanged);
			// 
			// nextPageButton
			// 
			this.nextPageButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.nextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nextPageButton.Location = new System.Drawing.Point(288, 424);
			this.nextPageButton.Name = "nextPageButton";
			this.nextPageButton.Size = new System.Drawing.Size(24, 20);
			this.nextPageButton.TabIndex = 9;
			this.nextPageButton.Text = ">";
			this.nextPageButton.Visible = false;
			this.nextPageButton.Click += new System.EventHandler(this.nextPageButton_Click);
			// 
			// prevPageButton
			// 
			this.prevPageButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.prevPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.prevPageButton.Location = new System.Drawing.Point(186, 424);
			this.prevPageButton.Name = "prevPageButton";
			this.prevPageButton.Size = new System.Drawing.Size(24, 20);
			this.prevPageButton.TabIndex = 8;
			this.prevPageButton.Text = "<";
			this.prevPageButton.Visible = false;
			this.prevPageButton.Click += new System.EventHandler(this.prevPageButton_Click);
			// 
			// pageNumberTextBox
			// 
			this.pageNumberTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.pageNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pageNumberTextBox.Location = new System.Drawing.Point(216, 424);
			this.pageNumberTextBox.Name = "pageNumberTextBox";
			this.pageNumberTextBox.Size = new System.Drawing.Size(66, 20);
			this.pageNumberTextBox.TabIndex = 7;
			this.pageNumberTextBox.Text = "1 of 1";
			this.pageNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pageNumberTextBox.Visible = false;
			this.pageNumberTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pageNumberTextBox_KeyDown);
			// 
			// zoomContextMenu
			// 
			this.zoomContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.zoomInMenuItem,
            this.zoomOutMenuItem});
			// 
			// zoomInMenuItem
			// 
			this.zoomInMenuItem.Checked = true;
			this.zoomInMenuItem.Index = 0;
			this.zoomInMenuItem.Text = "Zoom in";
			this.zoomInMenuItem.Click += new System.EventHandler(this.zoomInMenuItem_Click);
			// 
			// zoomOutMenuItem
			// 
			this.zoomOutMenuItem.Index = 1;
			this.zoomOutMenuItem.Text = "Zoom out";
			this.zoomOutMenuItem.Click += new System.EventHandler(this.zoomOutMenuItem_Click);
			// 
			// iconsImageList
			// 
			this.iconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsImageList.ImageStream")));
			this.iconsImageList.TransparentColor = System.Drawing.Color.White;
			this.iconsImageList.Images.SetKeyName(0, "");
			this.iconsImageList.Images.SetKeyName(1, "");
			this.iconsImageList.Images.SetKeyName(2, "");
			this.iconsImageList.Images.SetKeyName(3, "");
			this.iconsImageList.Images.SetKeyName(4, "");
			this.iconsImageList.Images.SetKeyName(5, "");
			this.iconsImageList.Images.SetKeyName(6, "");
			// 
			// openToolBarButton
			// 
			this.openToolBarButton.ImageIndex = 0;
			this.openToolBarButton.Name = "openToolBarButton";
			// 
			// saveToolBarButton
			// 
			this.saveToolBarButton.ImageIndex = 1;
			this.saveToolBarButton.Name = "saveToolBarButton";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Name = "toolBarButton1";
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// printToolBarButton
			// 
			this.printToolBarButton.ImageIndex = 2;
			this.printToolBarButton.Name = "printToolBarButton";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Name = "toolBarButton2";
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// zoomInToolBarButton
			// 
			this.zoomInToolBarButton.ImageIndex = 3;
			this.zoomInToolBarButton.Name = "zoomInToolBarButton";
			this.zoomInToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Name = "toolBarButton3";
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// textToolBarButton
			// 
			this.textToolBarButton.ImageIndex = 5;
			this.textToolBarButton.Name = "textToolBarButton";
			this.textToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.openToolBarButton,
            this.saveToolBarButton,
            this.toolBarButton1,
            this.printToolBarButton,
            this.toolBarButton2,
            this.zoomInToolBarButton,
            this.zoomOutToolBarButton,
            this.toolBarButton3,
            this.textToolBarButton,
            this.selectToolBarButton});
			this.toolBar1.ButtonSize = new System.Drawing.Size(24, 24);
			this.toolBar1.Divider = false;
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.iconsImageList;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(496, 34);
			this.toolBar1.TabIndex = 12;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// zoomOutToolBarButton
			// 
			this.zoomOutToolBarButton.ImageIndex = 4;
			this.zoomOutToolBarButton.Name = "zoomOutToolBarButton";
			this.zoomOutToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			// 
			// selectToolBarButton
			// 
			this.selectToolBarButton.ImageIndex = 6;
			this.selectToolBarButton.Name = "selectToolBarButton";
			this.selectToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			// 
			// imagePanel
			// 
			this.imagePanel.AutoScroll = true;
			this.imagePanel.Controls.Add(this.renderedDocPictureBox);
			this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imagePanel.Location = new System.Drawing.Point(168, 0);
			this.imagePanel.Name = "imagePanel";
			this.imagePanel.Size = new System.Drawing.Size(328, 384);
			this.imagePanel.TabIndex = 2;
			// 
			// renderedDocPictureBox
			// 
			this.renderedDocPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.renderedDocPictureBox.Location = new System.Drawing.Point(144, 32);
			this.renderedDocPictureBox.Name = "renderedDocPictureBox";
			this.renderedDocPictureBox.Size = new System.Drawing.Size(184, 233);
			this.renderedDocPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.renderedDocPictureBox.TabIndex = 1;
			this.renderedDocPictureBox.TabStop = false;
			this.renderedDocPictureBox.DoubleClick += new System.EventHandler(this.renderedDocPictureBox_DoubleClick);
			this.renderedDocPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderedDocPictureBox_MouseMove);
			this.renderedDocPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renderedDocPictureBox_MouseDown);
			this.renderedDocPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.renderedDocPictureBox_Paint);
			this.renderedDocPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.renderedDocPictureBox_MouseUp);
			// 
			// panelsSplitter
			// 
			this.panelsSplitter.Location = new System.Drawing.Point(168, 0);
			this.panelsSplitter.MinExtra = 0;
			this.panelsSplitter.MinSize = 0;
			this.panelsSplitter.Name = "panelsSplitter";
			this.panelsSplitter.Size = new System.Drawing.Size(3, 384);
			this.panelsSplitter.TabIndex = 4;
			this.panelsSplitter.TabStop = false;
			this.panelsSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.panelsSplitter_SplitterMoved);
			// 
			// textEditingPanel
			// 
			this.textEditingPanel.Controls.Add(this.label4);
			this.textEditingPanel.Controls.Add(this.textStyleGroupBox);
			this.textEditingPanel.Controls.Add(this.addTextButton);
			this.textEditingPanel.Controls.Add(this.label1);
			this.textEditingPanel.Controls.Add(this.selectedBlockTextBox);
			this.textEditingPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.textEditingPanel.Location = new System.Drawing.Point(0, 0);
			this.textEditingPanel.Name = "textEditingPanel";
			this.textEditingPanel.Size = new System.Drawing.Size(168, 384);
			this.textEditingPanel.TabIndex = 3;
			this.textEditingPanel.Visible = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(160, 40);
			this.label4.TabIndex = 5;
			this.label4.Text = "Drag a rectangle to add a new text block.";
			// 
			// textStyleGroupBox
			// 
			this.textStyleGroupBox.Controls.Add(this.fontNameComboBox);
			this.textStyleGroupBox.Controls.Add(this.label6);
			this.textStyleGroupBox.Controls.Add(this.fontColorButton);
			this.textStyleGroupBox.Controls.Add(this.label5);
			this.textStyleGroupBox.Controls.Add(this.fitToSelectionCheckBox);
			this.textStyleGroupBox.Controls.Add(this.fontSizeNumericUpDown);
			this.textStyleGroupBox.Controls.Add(this.label2);
			this.textStyleGroupBox.Location = new System.Drawing.Point(0, 176);
			this.textStyleGroupBox.Name = "textStyleGroupBox";
			this.textStyleGroupBox.Size = new System.Drawing.Size(160, 128);
			this.textStyleGroupBox.TabIndex = 3;
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
			this.fontNameComboBox.Location = new System.Drawing.Point(48, 96);
			this.fontNameComboBox.Name = "fontNameComboBox";
			this.fontNameComboBox.Size = new System.Drawing.Size(104, 21);
			this.fontNameComboBox.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 96);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 23);
			this.label6.TabIndex = 10;
			this.label6.Text = "Font:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fontColorButton
			// 
			this.fontColorButton.Location = new System.Drawing.Point(88, 72);
			this.fontColorButton.Name = "fontColorButton";
			this.fontColorButton.Size = new System.Drawing.Size(24, 20);
			this.fontColorButton.TabIndex = 9;
			this.fontColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.fontColorButton_Paint);
			this.fontColorButton.Click += new System.EventHandler(this.fontColorButton_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Text color:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fitToSelectionCheckBox
			// 
			this.fitToSelectionCheckBox.Checked = true;
			this.fitToSelectionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fitToSelectionCheckBox.Location = new System.Drawing.Point(8, 24);
			this.fitToSelectionCheckBox.Name = "fitToSelectionCheckBox";
			this.fitToSelectionCheckBox.Size = new System.Drawing.Size(146, 16);
			this.fitToSelectionCheckBox.TabIndex = 2;
			this.fitToSelectionCheckBox.Text = "Fit to selection rectangle";
			this.fitToSelectionCheckBox.CheckedChanged += new System.EventHandler(this.fitToSelectionCheckBox_CheckedChanged);
			// 
			// fontSizeNumericUpDown
			// 
			this.fontSizeNumericUpDown.Enabled = false;
			this.fontSizeNumericUpDown.Location = new System.Drawing.Point(88, 48);
			this.fontSizeNumericUpDown.Name = "fontSizeNumericUpDown";
			this.fontSizeNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.fontSizeNumericUpDown.TabIndex = 1;
			this.fontSizeNumericUpDown.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Font size:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// addTextButton
			// 
			this.addTextButton.Enabled = false;
			this.addTextButton.Location = new System.Drawing.Point(0, 136);
			this.addTextButton.Name = "addTextButton";
			this.addTextButton.Size = new System.Drawing.Size(75, 23);
			this.addTextButton.TabIndex = 2;
			this.addTextButton.Text = "Add text";
			this.addTextButton.Click += new System.EventHandler(this.addTextButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Text:";
			// 
			// selectedBlockTextBox
			// 
			this.selectedBlockTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.selectedBlockTextBox.Enabled = false;
			this.selectedBlockTextBox.Location = new System.Drawing.Point(0, 64);
			this.selectedBlockTextBox.Multiline = true;
			this.selectedBlockTextBox.Name = "selectedBlockTextBox";
			this.selectedBlockTextBox.Size = new System.Drawing.Size(160, 64);
			this.selectedBlockTextBox.TabIndex = 0;
			// 
			// layoutPanel
			// 
			this.layoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.layoutPanel.Controls.Add(this.panelsSplitter);
			this.layoutPanel.Controls.Add(this.imagePanel);
			this.layoutPanel.Controls.Add(this.textEditingPanel);
			this.layoutPanel.Location = new System.Drawing.Point(0, 32);
			this.layoutPanel.Name = "layoutPanel";
			this.layoutPanel.Size = new System.Drawing.Size(496, 384);
			this.layoutPanel.TabIndex = 13;
			// 
			// PDFControl
			// 
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.dpiLabel);
			this.Controls.Add(this.dpiNumericUpDown);
			this.Controls.Add(this.nextPageButton);
			this.Controls.Add(this.prevPageButton);
			this.Controls.Add(this.pageNumberTextBox);
			this.Controls.Add(this.layoutPanel);
			this.Name = "PDFControl";
			this.Size = new System.Drawing.Size(496, 448);
			this.Load += new System.EventHandler(this.PDFControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.dpiNumericUpDown)).EndInit();
			this.imagePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.renderedDocPictureBox)).EndInit();
			this.textEditingPanel.ResumeLayout(false);
			this.textEditingPanel.PerformLayout();
			this.textStyleGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fontSizeNumericUpDown)).EndInit();
			this.layoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Initialization

		public PDFControl() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Activates double buffering
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true); 

			dragPen.DashStyle = DashStyle.Custom;
			dragPen.DashPattern = mDashPattern;

		}

		private void PDFControl_Load(object sender, System.EventArgs e) {
			mPrint = new PrintDocument();
			mPrint.BeginPrint += new PrintEventHandler(this.DoBeginPrint);
			mPrint.EndPrint += new PrintEventHandler(this.DoEndPrint);
			mPrint.PrintPage += new PrintPageEventHandler(this.DoPrintPage);

			mDotsPerInch = (int)dpiNumericUpDown.Value;
			fontNameComboBox.SelectedIndex = 0;
			
			mZoomInCursor =	new Cursor(this.GetType(), "zoomIn.cur");
			mZoomOutCursor = new Cursor(this.GetType(), "zoomOut.cur");
			imagePanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PDFControl_KeyDown);
			imagePanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PDFControl_KeyUp);

			selectToolBarButton.Pushed = true;
			CurrentTool = ToolType.SelectTool;
		}
		#endregion

		#region Finalizing
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#endregion

		#region PDF view layout

		/// <summary>
		/// Adsjust the position of image panel
		/// </summary>
		private void SetLayout() {
			if (renderedDocPictureBox.Image == null)
				return;

			int width = renderedDocPictureBox.Image.Width;
			int height = renderedDocPictureBox.Image.Height;
			int left = (imagePanel.Width - width)/2;
			if (left < 0)
				left = 0;
			left += imagePanel.AutoScrollPosition.X+5;
			int top = (imagePanel.Height - height)/2;
			if (top < 0)
				top = 0;
			top += imagePanel.AutoScrollPosition.Y;
			renderedDocPictureBox.SetBounds(left, top, width, height);
			
			imagePanel.Focus();
		}

		/// <summary>
		/// Render page of the document
		/// </summary>
		private void UpdateView() {
			Cursor saveCursor = renderedDocPictureBox.Cursor;
			renderedDocPictureBox.Cursor = Cursors.WaitCursor;
			renderedDocPictureBox.Image = mDoc.Rendering.GetBitmap();
			renderedDocPictureBox.Cursor = saveCursor;
			Refresh();
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			SetLayout();
			Refresh();
		}

		private void panelsSplitter_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e) {
			SetLayout();
		}
		#endregion
		
		#region Open/Save dialogs
		/// <summary>
		/// Show open file dialog and load selected pdf file
		/// </summary>
		public void Open() {
			OpenFileDialog od = new OpenFileDialog();
			od.Filter = "PDF Files (*.pdf;*.ai)|*.pdf;*ai|All Files (*.*)|*.*" ;
			od.FilterIndex = 1;
			if (od.ShowDialog() == DialogResult.OK) 
				Open(od.FileName);
		}
		
		/// <summary>
		/// Open pdf file
		/// </summary>
		/// <param name="inPath">File path</param>
		public void Open(string inPath) {
			if (mDoc == null) {
				mDoc = new Doc();
				pageNumberTextBox.Visible = true;
				prevPageButton.Visible = true;
				nextPageButton.Visible = true;
				dpiNumericUpDown.Visible = true;
				dpiLabel.Visible = true;
			}

			mDoc.Read(inPath);

			mDoc.Rect.String = mDoc.CropBox.String;
			mPath = inPath;
			FileInfo fileInfo = new FileInfo(mPath);
			mPrint.DocumentName = fileInfo.Name;

			double sw = imagePanel.Width;
			double sh = imagePanel.Height;
			double resX = (sw * 72) / mDoc.Rect.Width;
			double resY = (sh * 72) / mDoc.Rect.Height;
			int res = (int)(resX > resY ? resY : resX);
			int oldDPI = (int)dpiNumericUpDown.Value;
			if (dpiNumericUpDown.Minimum >= res / 2) {
				dpiNumericUpDown.Minimum = res / 2;
				dpiNumericUpDown.Value = res;
			} else {
				dpiNumericUpDown.Value = res;
				dpiNumericUpDown.Minimum = res / 2;
			}

			mAnnotatedContent = new AnnotatedContent(mDoc);
			mTextBlocks = mAnnotatedContent.Parse(mDoc);

			string thePermissions = "";
			if (!mDoc.Encryption.CanAssemble)
				thePermissions += " - page assembly\r\n";
			if (!mDoc.Encryption.CanChange)
				thePermissions += " - page changes\r\n";
			if (!mDoc.Encryption.CanCopy)
				thePermissions += " - copying from pages\r\n";
			if (!mDoc.Encryption.CanEdit)
				thePermissions += " - editing pages\r\n";
			if (!mDoc.Encryption.CanExtract)
				thePermissions += " - content extraction\r\n";
			if (!mDoc.Encryption.CanFillForms)
				thePermissions += " - form filling\r\n ";
			if (!mDoc.Encryption.CanPrint)
				thePermissions += " - printing\r\n";
			if (!mDoc.Encryption.CanPrintHi)
				thePermissions += " - high resolution printing\r\n";
			if (thePermissions != "")
				MessageBox.Show("This document does not allow:\r\n" + thePermissions);

			renderedDocPictureBox.BorderStyle = BorderStyle.FixedSingle;
			UpdatePageNumberTextBox();

			imagePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(renderedDocPictureBox_MouseUp);
			saveToolBarButton.Enabled = true;
			printToolBarButton.Enabled = true;
			zoomInToolBarButton.Enabled = true;
			zoomOutToolBarButton.Enabled = true;
			textToolBarButton.Enabled = mDoc.Encryption.CanChange;
			selectToolBarButton.Enabled = true;
		}

		/// <summary>
		/// Save file to disc
		/// </summary>
		public void Save() {
			if (mDoc == null)
				return;

			if (mPath == "")
				SaveAs();
			else
				Save(mPath);
		}
		
		/// <summary>
		/// Show "Save as" dialog and save file to the specified location on the disc
		/// </summary>
		public void SaveAs() {
			if (mDoc == null)
				return;

			SaveFileDialog sd = new SaveFileDialog();
			sd.Filter = "PDF Files (*.pdf;*.ai)|*.pdf;*ai|All Files (*.*)|*.*" ;
			sd.FilterIndex = 1;
			sd.RestoreDirectory = true;
			if (sd.ShowDialog() == DialogResult.OK) {
				mPath = sd.FileName;
				Save(mPath);
			}
		}
		
		/// <summary>
		/// Save file to the specified location on the disc
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path) {
			if (mDoc == null)
				return;

			if (path != "")
				mDoc.Save(path);
		}
		#endregion

		#region Page manipulations
		public void InsertPages() {
			if (!mDoc.Encryption.CanAssemble) {
				MessageBox.Show("This document does not permit page assembly.");
				return;
			}

			OpenFileDialog od = new OpenFileDialog();
			od.Filter = "PDF Files (*.pdf;*.ai)|*.pdf;*ai|All Files (*.*)|*.*" ;
			od.FilterIndex = 1;
			od.Title = "Select File To Insert";

			if (od.ShowDialog() == DialogResult.OK) {
				Doc theDoc = new Doc();
				try {
					theDoc.Read(od.FileName);
				}
				catch {
					MessageBox.Show("Can't open " + od.FileName + ".");
					InsertPages();
					return;
				}
				InsertPagesForm theDialog = new InsertPagesForm(mDoc, theDoc);
				if (theDialog.ShowDialog() == DialogResult.OK) {
					UpdateView();
					UpdatePageNumberTextBox();
				}
			}
		}

		public void DeletePages() {
			if (!mDoc.Encryption.CanAssemble) {
				MessageBox.Show("This document does not permit page assembly.");
				return;
			}

			DeletePagesForm theDialog = new DeletePagesForm(mDoc);
			if (theDialog.ShowDialog() == DialogResult.OK) {
				UpdateView();
				UpdatePageNumberTextBox();
			}
		}

		public void ExtractPages() {
			if (!mDoc.Encryption.CanAssemble) {
				MessageBox.Show("This document does not permit page assembly.");
				return;
			}

			ExtractPagesForm theDialog = new ExtractPagesForm(mDoc);
			if (theDialog.ShowDialog() == DialogResult.OK) {
				UpdateView();
				UpdatePageNumberTextBox();
			}
		}

		public void AddWatermark() {
			if (!mDoc.Encryption.CanChange) {
				MessageBox.Show("This document does not permit page changes.");
				return;
			}

			AddWatermarkForm theDialog = new AddWatermarkForm(mDoc);
			if (theDialog.ShowDialog() == DialogResult.OK) {
				UpdateView();
				UpdatePageNumberTextBox();
			}
		}
		public void RotatePages() {
			if (!mDoc.Encryption.CanAssemble) {
				MessageBox.Show("This document does not permit page assembly.");
				return;
			}

			RotatePagesForm theDialog = new RotatePagesForm(mDoc);
			if (theDialog.ShowDialog() == DialogResult.OK) {
				UpdateView();
				SetLayout();
			}
		}
		#endregion

		#region Page navigation
		/// <summary>
		/// Update text in the page number text box
		/// </summary>
		private void UpdatePageNumberTextBox() {
			pageNumberTextBox.Text = PageNumber.ToString() + " of " + PageCount.ToString();
			prevPageButton.Enabled = PageNumber > 1;
			nextPageButton.Enabled = PageNumber < PageCount;
		}

		private void nextPageButton_Click(object sender, System.EventArgs e) {
			PageNumber++;
			UpdatePageNumberTextBox();
			imagePanel.Focus();
		}

		private void prevPageButton_Click(object sender, System.EventArgs e) {
			PageNumber--;
			UpdatePageNumberTextBox();
			imagePanel.Focus();
		}

		private void pageNumberTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				string text = ((TextBox)sender).Text;
				Regex regExp = new Regex("^[0-9]*");
				string pageNumberString = regExp.Match(text).Value;

				if (pageNumberString != "") {
					PageNumber = int.Parse(pageNumberString);
				}
				UpdatePageNumberTextBox();
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Current page number
		/// </summary>
		public int PageNumber {
			get {
				if (mDoc == null)
					return 0;
				else
					return mDoc.PageNumber;
			}
			set {
				if (mDoc != null && value <= mDoc.PageCount && value != mDoc.PageNumber) {
					mDoc.PageNumber = value;
					mDoc.Rect.String = mDoc.CropBox.String;
					Cursor saveCursor = renderedDocPictureBox.Cursor;
					renderedDocPictureBox.Cursor = Cursors.WaitCursor;
					renderedDocPictureBox.Image = mDoc.Rendering.GetBitmap();
					renderedDocPictureBox.Cursor = saveCursor;
					SetLayout();
					mTextBlocks = mAnnotatedContent.Parse(mDoc);
				}
			}
		}

		/// <summary>
		/// Number of pages in the opened document
		/// </summary>
		public int PageCount {
			get {
				if (mDoc != null)
					return mDoc.PageCount;
				else
					return 0;
			}
		}

		/// <summary>
		/// Rendering resolution
		/// </summary>
		public int DotsPerInch {
			get {
				return mDotsPerInch;
			}
			set {
				if (mDoc != null) {
					mDotsPerInch = value;
					mDoc.Rendering.DotsPerInch = mDotsPerInch;
					Cursor saveCursor = renderedDocPictureBox.Cursor;
					renderedDocPictureBox.Cursor = Cursors.WaitCursor;
					renderedDocPictureBox.Image = mDoc.Rendering.GetBitmap();
					renderedDocPictureBox.Cursor = saveCursor;
					SetLayout();
				}
			}
		}

		/// <summary>
		/// Is any document opened
		/// </summary>
		public bool DocumentOpened {
			get {
				return mDoc != null;
			}
		}


		/// <summary>
		/// Document name
		/// </summary>
		public string DocumentName {
			get {
				return mPrint.DocumentName;
			}
		}
		#endregion
		
		#region Printing

		/// <summary>
		/// Show page setup dialog
		/// </summary>
		public void PageSetup() {
			PageSetupDialog p = new PageSetupDialog();
			p.Document = mPrint;
			// we hardwire the margins because of .NET bug http://support.microsoft.com/kb/814355
			mPrint.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
			p.AllowMargins = false;
			//p.AllowOrientation = false;
			//p.AllowPaper = false;
			//p.AllowPrinter = false;
			p.ShowDialog();
		}

		/// <summary>
		/// Show print dialog and print document
		/// </summary>
		public void Print() {
			if (mDoc == null)
				return;

			if (!mDoc.Encryption.CanPrint) {
				MessageBox.Show("This document does not permit printing.");
				return;
			}
			PrintDialog p = new PrintDialog();
			p.Document = mPrint;

			// Note: on some AMD64 machines the dialog will not show
			// unless this is set to true.
			p.UseEXDialog = true;

			mPrint.PrinterSettings.FromPage = 1;
			mPrint.PrinterSettings.ToPage = PageCount;
			mPrint.PrinterSettings.MinimumPage = 1;
			mPrint.PrinterSettings.MaximumPage = PageCount;
			p.AllowSomePages = true;

			if (p.ShowDialog() == DialogResult.OK) 
				mPrint.Print();
		}
		
		/// <summary>
		/// Show print preview
		/// </summary>
		public void PrintPreview() {
			if (mDoc == null)
				return;

			if (!mDoc.Encryption.CanPrint) {
				MessageBox.Show("This document does not permit printing.");
				return;
			}
			PrintPreviewDialog p = new PrintPreviewDialog();
			p.Document = mPrint;
			p.WindowState = FormWindowState.Maximized;
			p.ShowDialog();
		}

		private void DoBeginPrint(object sender, PrintEventArgs e) {
			mPage = mPrint.PrinterSettings.FromPage;
			mPageSaved = mDoc.PageNumber;
			mCopiesNumber = 1;
		}

		private void DoEndPrint(object sender, PrintEventArgs e) {
			mDoc.PageNumber = mPageSaved;
			mDoc.Rect.String = mDoc.CropBox.String;
		}

		private void DoPrintPage(object sender, PrintPageEventArgs e) {
			mDoc.PageNumber = mPage;
			if (!mPrint.PrinterSettings.Collate) {
				if (mCopiesNumber++ >= mPrint.PrinterSettings.Copies) {
					mPage++;
					mCopiesNumber = 1;
				}

				e.HasMorePages = mPage <= mPrint.PrinterSettings.ToPage;
			}
			else {
				mPage++;
				e.HasMorePages = mPage <= mPrint.PrinterSettings.ToPage;

				if (!e.HasMorePages && mCopiesNumber < mPrint.PrinterSettings.Copies) {
					mCopiesNumber++;
					mPage = mPrint.PrinterSettings.FromPage;
					e.HasMorePages = mPage <= mPrint.PrinterSettings.ToPage;
				}
			}

			Graphics g = e.Graphics;
			if (mDoc.PageCount == 0) return;
			if (mDoc.Page == 0) return;

			XRect cropBox = mDoc.CropBox;
			double srcWidth = (cropBox.Width / 72) * 100;
			double srcHeight = (cropBox.Height / 72) * 100;
			double pageWidth = e.PageBounds.Width;
			double pageHeight = e.PageBounds.Height;
			double marginX = e.PageSettings.HardMarginX;
			double marginY = e.PageSettings.HardMarginY;
			double dstWidth = pageWidth - (marginX * 2);
			double dstHeight = pageHeight - (marginY * 2);

			const bool autoRotate = true;
			int rotate = 0;
			if (autoRotate && srcWidth!=srcHeight && dstWidth!=dstHeight
				&& (srcWidth>srcHeight)!=(dstWidth>dstHeight))
			{
				double temp = pageWidth;
				pageWidth = pageHeight;
				pageHeight = temp;
				temp = marginX;
				marginX = marginY;
				marginY = temp;
				temp = dstWidth;
				dstWidth = dstHeight;
				dstHeight = temp;
				rotate = PDFUtilities.GetPageRotation(mDoc) % 360;
				if (rotate <= -180)
					rotate += 360;
				else if (rotate > 180)
					rotate -= 360;
				rotate = rotate > 0? 90: -90;	// default to -90
				// Use -90 because we want the staple to be at a top corner
				// of the page.  Assuming a rotation of "rotate" (0 or 180 degrees)
				// produces upright contents, a rotation of -90 or 90 degrees
				// (respectively) produces outputs whose top is at the
				// left edge of the portrait page.  The staple at the top-left
				// corner of the portrait page will be at the top-right corner of the
				// contents.
			} else {
				rotate = PDFUtilities.GetPageRotation(mDoc) % 360;
				if (rotate != 180 && rotate != -180)
					rotate = 0;
			}

			// if source bigger than destination then scale
			if ((srcWidth > dstWidth) || (srcHeight > dstHeight)) {
				double sx = dstWidth / srcWidth;
				double sy = dstHeight / srcHeight;
				double s = Math.Min(sx, sy);
				srcWidth *= s;
				srcHeight *= s;
			}

			// now center
			double x = (pageWidth - srcWidth) / 2;
			double y = (pageHeight - srcHeight) / 2;

			// adjust to work around bug in .NET
			// http://social.msdn.microsoft.com/forums/en-US/vbgeneral/thread/c4b3408f-5b4f-4608-a36d-dd74f69ecdbd/
			if (!mPrint.PrintController.IsPreview) {
				x -= marginX;
				y -= marginY;
			}

			// save state
			double saveDotsPerInch = mDoc.Rendering.DotsPerInch;
			mDoc.Rendering.AutoRotate = false;

			RectangleF theRect = new RectangleF((float)x, (float)y, (float)srcWidth, (float)srcHeight);
			
			int theRez = e.PageSettings.PrinterResolution.X;
			if (e.PageSettings.Color) // color is generally CMYK so to translate from dpi to ppi we divide by four
				theRez /= 4;
			if (theRez <= 0) // Invalid printer resolution - use the default value
				theRez = 72;

			// draw content
			mDoc.Rect.SetRect(cropBox);
			Matrix oldTransform = null;
			if (rotate != 0) {
				oldTransform = g.Transform;
				switch (rotate) {
				case 90:
					using (Matrix matrix = new Matrix(0, 1, -1, 0,
						(float)(2*y+srcHeight), 0))
					{
						g.MultiplyTransform(matrix);
					}
					break;
				case -90:
					using (Matrix matrix = new Matrix(0, -1, 1, 0,
						0, (float)(2*x+srcWidth)))
					{
						g.MultiplyTransform(matrix);
					}
					break;
				case 180: case -180:
					using (Matrix matrix = new Matrix(-1, 0, 0, -1,
						(float)(2*x+srcWidth), (float)(2*y+srcHeight)))
					{
						g.MultiplyTransform(matrix);
					}
					break;
				}
			}
#if !DISPLAY_PRINT_AREA
			g.SetClip(theRect);
#endif
			if (!mDoc.Encryption.CanPrintHi) {
				mDoc.Rendering.DotsPerInch = 72;
				using (Bitmap bm = mDoc.Rendering.GetBitmap()) {
					g.DrawImage(bm, theRect);
				}
			}
			else {
				mDoc.Rendering.DotsPerInch = theRez;
				mDoc.Rendering.ColorSpace = XRendering.ColorSpaceType.Rgb;
				mDoc.Rendering.BitsPerChannel = 8;
				byte[] theData = mDoc.Rendering.GetData(".emf");
				MemoryStream theStream = new MemoryStream(theData);
				using (Metafile theEMF = new Metafile(theStream)) {
					g.DrawImage(theEMF, theRect);
				}
			}
			if (oldTransform != null) {
				g.Transform = oldTransform;
				oldTransform.Dispose();
			}
			// restore state
			mDoc.Rendering.DotsPerInch = saveDotsPerInch;
			mDoc.Rendering.AutoRotate = true;
#if DISPLAY_PRINT_AREA
			g.DrawRectangle(new Pen(Color.Red),
				theRect.X, theRect.Y, theRect.Width, theRect.Height);
#endif
		}
		#endregion

		#region Dragging

		/// <summary>
		/// True when user drags a new selection rectangle or selected text block
		/// </summary>
		private bool mbDragging = false;
		/// <summary>
		/// Tracks the original point where mbDragging was started.
		/// </summary>
		private Point mDragStart;
		/// <summary>
		/// Tracks the point where the drag ended.
		/// </summary>
		private Point mDragEnd;

		private Single[] mDashPattern = new Single[]{5,2,5,2}; /* The dash pattern used to draw the zoom box. */
		private Pen dragPen = new Pen(Brushes.Black, 1); /* The pen used to draw the zoom box. */

		//Drag selected rect
		private Point mCurrentDragPosition;
		private GraphicsPath mCurrentDragPath;

		//Drag grab bar
		private bool mbGrabBarDragging = false;
		private int mDragBarPointIndex = -1;

		private Rectangle mTextAreaSelection;
		private Rectangle TextAreaSelection {
			get {
				return mTextAreaSelection;
			}
			set {
				if (mTextAreaSelection.IsEmpty && ! value.IsEmpty)
					SelectBlock(null);
				mTextAreaSelection = value;

				fitToSelectionCheckBox.Enabled = ! value.IsEmpty;
				fontColorButton.Enabled = ! value.IsEmpty;
				fontSizeNumericUpDown.Enabled = ! value.IsEmpty && !fitToSelectionCheckBox.Enabled;
				fontNameComboBox.Enabled = ! value.IsEmpty;
				selectedBlockTextBox.Enabled = ! value.IsEmpty;
				addTextButton.Enabled = ! value.IsEmpty;
			}
		}


		private void DrawResizingRect(Point dragCurrent) {
			if (dragCurrent != mDragStart) {
				renderedDocPictureBox.Refresh();

				DraggingUtilities.DragWindowInfo dwi = DraggingUtilities.GetDragWindowSize(dragCurrent, mDragStart);

				renderedDocPictureBox.CreateGraphics().DrawRectangle(dragPen, dwi.TopLeft.X, dwi.TopLeft.Y,
					dwi.WindowSize.Width, dwi.WindowSize.Height);
			}
		}

		private void DrawDraggedRect(Point dragNewPoint) {
			if (mCurrentDragPosition != dragNewPoint) {
				Matrix translateMatrix = new Matrix();
				translateMatrix.Translate(dragNewPoint.X - mCurrentDragPosition.X, dragNewPoint.Y - mCurrentDragPosition.Y);
				mCurrentDragPath.Transform(translateMatrix);
				mCurrentDragPosition = dragNewPoint;

				renderedDocPictureBox.Refresh();

				renderedDocPictureBox.CreateGraphics().DrawPath(dragPen, mCurrentDragPath);
			}
		}

		private void  DrawDraggedRectWithGrabBar(Point dragNewPoint) {
			if (mDragStart != dragNewPoint) {
				GraphicsPath thePath = (GraphicsPath)mCurrentDragPath.Clone();

				Matrix theMatrix = DraggingUtilities.GetDraggingMatrix(dragNewPoint, thePath, mDragBarPointIndex);

				thePath.Transform(theMatrix);

				renderedDocPictureBox.Refresh();

				renderedDocPictureBox.CreateGraphics().DrawPath(dragPen, thePath);
			}
		}


		#endregion

		#region Mouse and Keyboard Events


		private void renderedDocPictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			base.OnMouseDown(e);

			if (CurrentTool == ToolType.TextTool) {
				if (e.Button == MouseButtons.Left) {
					mDragStart = new Point(e.X, e.Y);
					mTextAreaSelection = new Rectangle();
					mbDragging = true;
				}
			}
			if (CurrentTool == ToolType.SelectTool && SelectedBlock != null) {
				if (e.Button == MouseButtons.Left) {
					PointF[] points = new PointF[1];
					points[0] = new PointF(e.X, e.Y);
					Matrix theMatrix = ViewTransform;
					theMatrix.Invert();
					theMatrix.TransformPoints(points);

					//Drag Grab bar
					int pointIndex;
					GraphicsPath thePath = SelectedBlock.GetGraphics(ViewTransform);

					if (DraggingUtilities.IsOverGrabBars(new Point(e.X, e.Y), thePath, kGrabBarSize, out pointIndex)) {
						mDragBarPointIndex = pointIndex;
						mDragStart = new Point(e.X, e.Y);
						mCurrentDragPath = thePath;
						mbGrabBarDragging = true;
					}
					else // Drag selected rect
						if (SelectedBlock.ContainsPoint(points[0])) {
						mCurrentDragPath = SelectedBlock.GetGraphics(ViewTransform);

						mCurrentDragPosition = new Point(e.X, e.Y);
						mDragStart = mCurrentDragPosition;
						mbDragging = true;
					}
				}
			}
		}

		private void renderedDocPictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			base.OnMouseMove(e);

			//Drag text area 
			if (CurrentTool == ToolType.TextTool && mbDragging)
				DrawResizingRect(new Point(e.X, e.Y));

			if (CurrentTool == ToolType.SelectTool) {
				if (mbGrabBarDragging) { //Drag the rect with grab bar
					DrawDraggedRectWithGrabBar(new Point(e.X, e.Y));
					return;
				}
				else
					if (mbDragging) { //Drag the rect
					DrawDraggedRect(new Point(e.X, e.Y));
					return;
				}
			}

			bool bDefaultCursor = true;

			//Check if the mouse is over the grab bars.
			TextBlock theBlock = SelectedBlock;
			if (theBlock != null) {
				int pointIndex;
				GraphicsPath thePath = theBlock.GetGraphics(ViewTransform);

				if (DraggingUtilities.IsOverGrabBars(new Point(e.X, e.Y), thePath, kGrabBarSize, out pointIndex)) {
					renderedDocPictureBox.Cursor = DraggingUtilities.GetGrabBarCursor(pointIndex, thePath);
					bDefaultCursor = false;
				}
			}

			if (bDefaultCursor)
				renderedDocPictureBox.Cursor = CurrentCursor;

		}


		private void renderedDocPictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			int x = e.X;
			int y = e.Y;

			if (CurrentTool == ToolType.ZoomInTool || CurrentTool == ToolType.ZoomOutTool) {
				int dpi;

				if (CurrentTool == ToolType.ZoomInTool)
					dpi = (mDotsPerInch > kZoomIncrement)? mDotsPerInch + kZoomIncrement : mDotsPerInch * 2;
				else {
					dpi = (mDotsPerInch / 2> kZoomIncrement)? mDotsPerInch - kZoomIncrement: mDotsPerInch / 2;
					if (dpi < dpiNumericUpDown.Minimum)
						dpi = (int)dpiNumericUpDown.Minimum;
				}

				dpiNumericUpDown.Value = dpi;
			}
			else if (CurrentTool == ToolType.TextTool) {
				if (mbDragging) {
					if (e.Button == MouseButtons.Left) {
					 	// A drag has just ended and now we need to perform the actual zoom.
						mbDragging = false;
						if (mDragEnd == mDragStart)
							return;

						mDragEnd = new Point(e.X, e.Y);

						if (DraggingUtilities.CheckDragBoxSize(mDragStart, mDragEnd)) {
							Rectangle rect = DraggingUtilities.GetDragWindowSize(mDragEnd, mDragStart).GetRectangle();
							Matrix theMatrix = ViewTransform;
							theMatrix.Invert();
							GraphicsPath thePath = new GraphicsPath();
							thePath.AddRectangle(rect);
							thePath.Transform(theMatrix);
							RectangleF updatedRect = thePath.GetBounds();
							TextAreaSelection = new Rectangle((int)updatedRect.Left, (int)updatedRect.Top, (int)updatedRect.Width, (int)updatedRect.Height);
						}
						else {
							TextAreaSelection = new Rectangle();
							renderedDocPictureBox.Refresh();
						}
					}
				}
				else {
				}
			}
			else if (CurrentTool == ToolType.SelectTool) {
				if (mbDragging) {
					mbDragging = false;
					if (e.Button == MouseButtons.Left) {
					 	// A drag has just ended and now we need to perform the actual zoom.
						mDragEnd = new Point(e.X, e.Y);

						if (mDragStart != mDragEnd) {
							Point[] points = new Point[2];
							points[0] = mDragStart;
							points[1] = mDragEnd;
							Matrix theMatrix = ViewTransform;
							theMatrix.Invert();

							theMatrix.TransformPoints(points);

							mAnnotatedContent.MoveTextBlock(SelectedBlock, points[1].X - points[0].X, points[1].Y - points[0].Y);
							mTextBlocks = mAnnotatedContent.Parse(mDoc);
							UpdateView();
						}
					}
				}
				else if (mbGrabBarDragging) {
					TextBlock theBlock = SelectedBlock;
					mbGrabBarDragging = false;
					Matrix theMatrix = DraggingUtilities.GetDraggingMatrix(new Point(e.X, e.Y), mCurrentDragPath, mDragBarPointIndex);
					Matrix invertedViewMatrix = ViewTransform.Clone();
					invertedViewMatrix.Invert();

					Matrix newMatrix = ViewTransform.Clone();
					newMatrix.Multiply(theMatrix, MatrixOrder.Append);
					newMatrix.Multiply(invertedViewMatrix, MatrixOrder.Append);

					mAnnotatedContent.TransformTextBlock(theBlock, newMatrix);

					mTextBlocks = mAnnotatedContent.Parse(mDoc);
					UpdateView();

				}
				else {
					SelectBlock(null);
					Rectangle updateRect = TextAreaSelection;
					updateRect.Inflate(2,2);
					TextAreaSelection = new Rectangle();
					renderedDocPictureBox.Invalidate(updateRect);

					if (mTextBlocks.Count>0) {
						//float scale = 72 / (float)mDotsPerInch;
						Matrix theMatrix = ViewTransform;
						theMatrix.Invert();
						PointF[] points = { new PointF(x, y) };
						theMatrix.TransformPoints(points);

						for (int i = 0; i < mTextBlocks.Count; i++) {
							TextBlock textBlock = mTextBlocks[i];
							if (textBlock.ContainsPoint(points[0])) {
								SelectBlock(textBlock);
								imagePanel.Focus();
								break;
							}
						}
					}
				}
			}

			imagePanel.Focus();
		}

		private void renderedDocPictureBox_DoubleClick(object sender, System.EventArgs e) {
			if (SelectedBlock != null && (CurrentTool == ToolType.SelectTool || CurrentTool == ToolType.TextTool)) {
				TextPropertiesForm theDialog = new TextPropertiesForm(SelectedBlock, mAnnotatedContent);
				theDialog.CanChange = mDoc.Encryption.CanChange;
				theDialog.CanCopy = mDoc.Encryption.CanCopy;
				DialogResult res = theDialog.ShowDialog();
				if (res == DialogResult.OK) {
					mTextBlocks = mAnnotatedContent.Parse(mDoc);
					UpdateView();
				}
			}
		}

		private void PDFControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.ControlKey)
				UpdateToolCursors();
			else
				if (e.KeyCode == Keys.Delete) {
				TextBlock theBlock = SelectedBlock;
				if (theBlock != null) {
					mAnnotatedContent.DeleteTextBlock(theBlock);
					mTextBlocks = mAnnotatedContent.Parse(mDoc);
					UpdateView();
				}
			}
		}

		private void PDFControl_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.ControlKey)
				UpdateToolCursors();
		}


		#endregion

		#region Toolbar

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e) {
			if (e.Button == openToolBarButton) {
				Open();
			}
			else if (e.Button == saveToolBarButton) {
				SaveAs();
			}
			else if (e.Button == printToolBarButton) {
				Print();
			}
			else if (e.Button == zoomInToolBarButton) {
				CurrentTool = (zoomInToolBarButton.Pushed) ? ToolType.ZoomInTool : ToolType.HandTool;
			}
			else if (e.Button == zoomOutToolBarButton) {
				CurrentTool = (zoomOutToolBarButton.Pushed) ? ToolType.ZoomOutTool : ToolType.HandTool;
			}
			else if (e.Button == textToolBarButton) {
				CurrentTool = (textToolBarButton.Pushed) ? ToolType.TextTool : ToolType.HandTool;
				TextAreaSelection = new Rectangle();
			}
			else if (e.Button == selectToolBarButton) {
				CurrentTool = (selectToolBarButton.Pushed) ? ToolType.SelectTool : ToolType.HandTool;
			}
		}

		/// <summary>
		/// Currently active tool
		/// </summary>
		private ToolType CurrentTool {
			get {
				return mCurrentTool;
			}
			set {
				mCurrentTool = value;
				switch (mCurrentTool) {
					case ToolType.ZoomInTool:
						renderedDocPictureBox.Cursor = mZoomInCursor;
						break;
					case ToolType.ZoomOutTool:
						renderedDocPictureBox.Cursor = mZoomOutCursor;
						break;
					case ToolType.HandTool:
						renderedDocPictureBox.Cursor = Cursors.Default;
						break;
					case ToolType.TextTool:
						renderedDocPictureBox.Cursor = Cursors.Cross;
						ShowTextEditPane = true;
						break;
					case ToolType.SelectTool:
						renderedDocPictureBox.Cursor = Cursors.Arrow;
						break;
					default:
						break;
				}

				if (mCurrentTool != ToolType.TextTool && ShowTextEditPane) {
					ShowTextEditPane = false;
					textToolBarButton.Pushed = false;
				}

				if (mCurrentTool != ToolType.SelectTool)
					selectToolBarButton.Pushed = false;

				if (mCurrentTool !=  ToolType.ZoomOutTool)
					zoomOutToolBarButton.Pushed = false;

				if (mCurrentTool !=  ToolType.ZoomInTool)
					zoomInToolBarButton.Pushed = false;
			}
		}

		private Cursor CurrentCursor {
			get {
				switch (CurrentTool) {
					case ToolType.ZoomInTool:
						return mZoomInCursor;
					case ToolType.ZoomOutTool:
						return mZoomOutCursor;
					case ToolType.HandTool:
						return Cursors.Default;
					case ToolType.TextTool:
						return Cursors.Cross;
					case ToolType.SelectTool:
						return Cursors.Arrow;
					default:
						return Cursors.Default;
				}
			}
		}

		#endregion

		#region Zooming tools

		private void dpiNumericUpDown_ValueChanged(object sender, System.EventArgs e) {
			int x = imagePanel.Width / 2 - imagePanel.AutoScrollPosition.X ;
			int y = imagePanel.Height /2 - imagePanel.AutoScrollPosition.Y;

			int oldDPI = mDotsPerInch;

			DotsPerInch = (int)((NumericUpDown)sender).Value;

			x = x * mDotsPerInch / oldDPI - imagePanel.Width / 2;
			y = y * mDotsPerInch / oldDPI - imagePanel.Height /2;

			imagePanel.AutoScrollPosition = new Point(x , y);
			imagePanel.Focus();

			dpiNumericUpDown.Increment = (dpiNumericUpDown.Value < kZoomIncrement)? dpiNumericUpDown.Value : kZoomIncrement;
			dpiNumericUpDown.Minimum = ((int)dpiNumericUpDown.Value + 1) / 2;
			Refresh();
		}

		private void zoomInMenuItem_Click(object sender, System.EventArgs e) {
		}

		private void zoomOutMenuItem_Click(object sender, System.EventArgs e) {
		}

		private void UpdateToolCursors() {
			imagePanel.Focus();
			if (CurrentTool == ToolType.ZoomInTool || CurrentTool == ToolType.ZoomOutTool) {
				if (!mZoomToolReverted && Control.ModifierKeys == Keys.Control) { //switch zooming tool if control is pressed
					CurrentTool = (CurrentTool == ToolType.ZoomInTool)? ToolType.ZoomOutTool: ToolType.ZoomInTool;
					mZoomToolReverted = true;
				}
				else
					if (mZoomToolReverted && Control.ModifierKeys != Keys.Control) {
					CurrentTool = (CurrentTool == ToolType.ZoomInTool)? ToolType.ZoomOutTool: ToolType.ZoomInTool;
					mZoomToolReverted = false;
				}
			}
		}

		#endregion

		#region Text editing

		private Pen BlackPen = new Pen(Brushes.Black, 1);
		private Pen RedPen = new Pen(Brushes.Red, 1);
		private Color mTextColor = Color.Black;
		private List<TextBlock> mTextBlocks = new List<TextBlock>();

		/// <summary>
		/// Select text block
		/// </summary>
		/// <param name="selectedBlock">Text block</param>
		private void SelectBlock(TextBlock selectedBlock) {
			for (int i = 0; i < mTextBlocks.Count; i++) {
				TextBlock theBlock = mTextBlocks[i];

				GraphicsPath thePath = theBlock.GetGraphics(ViewTransform);
				thePath.Widen(new Pen(Brushes.Black, 2 + kGrabBarSize));

				if (theBlock == selectedBlock) {
					theBlock.Selected = true;
					renderedDocPictureBox.Invalidate(new Region(thePath));
					selectedBlockTextBox.Text = theBlock.Text;
				}
				else
					if (theBlock.Selected) {
					theBlock.Selected = false;
					renderedDocPictureBox.Invalidate(new Region(thePath));
				}
			}
			if (selectedBlock == null)
				selectedBlockTextBox.Text = "";

			//Enable edit box and button
			selectedBlockTextBox.Enabled = selectedBlock != null;
			addTextButton.Enabled = selectedBlock != null;

		}

		/// <summary>
		/// Selected text block
		/// </summary>
		private TextBlock SelectedBlock {
			get {
				for (int i = 0; i < mTextBlocks.Count; i++) {
					TextBlock theBlock = mTextBlocks[i];
					if (theBlock.Selected)
						return theBlock;
				}
				return null;
			}
		}

		/// <summary>
		/// Show text editing pane
		/// </summary>
		private bool ShowTextEditPane {
			set {
				textEditingPanel.Visible = value;
				panelsSplitter.Visible = value;
				SetLayout();
			}
			get {
				return textEditingPanel.Visible;
			}
		}

		/// <summary>
		/// Transformation from pdf coordinates to view coordinates
		/// </summary>
		private Matrix ViewTransform {
			get {
				float scale = mDotsPerInch / (float)72;
				Matrix theMatrix = null;

				int angle = PDFUtilities.GetPageRotation(mDoc);
				switch (angle) {
					case 0:
						theMatrix = new Matrix(scale, 0, 0, -scale, 0, (float)mDoc.MediaBox.Height*scale);
						theMatrix.Translate( (float)(mDoc.MediaBox.Left - mDoc.CropBox.Left), (float)(mDoc.MediaBox.Top - mDoc.CropBox.Top), MatrixOrder.Prepend);
						break;
					case 90:
						theMatrix = new Matrix(0, scale, scale, 0, 0, 0);
						theMatrix.Translate( (float)(mDoc.MediaBox.Left - mDoc.CropBox.Left), (float)(mDoc.MediaBox.Bottom - mDoc.CropBox.Bottom), MatrixOrder.Prepend);
						break;
					case 180:
						theMatrix = new Matrix(-scale, 0, 0, scale, (float)mDoc.MediaBox.Width*scale, 0);
						theMatrix.Translate( (float)(mDoc.MediaBox.Right - mDoc.CropBox.Right), (float)(mDoc.MediaBox.Bottom - mDoc.CropBox.Bottom), MatrixOrder.Prepend);
						break;
					case 270:
						theMatrix = new Matrix(0, -scale, -scale, 0, (float)mDoc.MediaBox.Height*scale, (float)mDoc.MediaBox.Width*scale);
						theMatrix.Translate( (float)(mDoc.MediaBox.Right - mDoc.CropBox.Right), (float)(mDoc.MediaBox.Top - mDoc.CropBox.Top), MatrixOrder.Prepend);
						break;
					default:
						break;
				}
				
				return theMatrix;
			}
		}

		private void renderedDocPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
#if DISPLAY_TEXT_RECTANGLES
			for (int i = 0; i < mTextBlocks.Count; i++)
			{
				TextBlock textBlock = mTextBlocks[i];
				GraphicsPath thePath = textBlock.GetGraphics(ViewTransform);

				if (textBlock.Selected)
					e.Graphics.DrawPath(RedPen, thePath);			
				else
					e.Graphics.DrawPath(BlackPen, thePath);			
			}
#else
			TextBlock textBlock = SelectedBlock;
			if (textBlock != null) {
				GraphicsPath thePath = textBlock.GetGraphics(ViewTransform);
				e.Graphics.DrawPath(BlackPen, thePath);
				for (int i = 0; i < thePath.PathPoints.Length; i++) {
					e.Graphics.FillRectangle(new SolidBrush(Color.White), thePath.PathPoints[i].X - 2, thePath.PathPoints[i].Y - 2, 4, 4);
					e.Graphics.DrawRectangle(BlackPen, thePath.PathPoints[i].X - kGrabBarSize / 2, thePath.PathPoints[i].Y - kGrabBarSize / 2, kGrabBarSize, kGrabBarSize);
				}

			}
#endif
			
			if (!TextAreaSelection.IsEmpty && CurrentTool == ToolType.TextTool) {
				GraphicsPath thePath = new GraphicsPath();
				thePath.AddRectangle(TextAreaSelection);
				thePath.Transform(ViewTransform);
				e.Graphics.DrawPath(dragPen, thePath);
			}
		}

		private void addTextButton_Click(object sender, System.EventArgs e) {
			if (!mDoc.Encryption.CanChange) {
				MessageBox.Show("This document does not permit page changes.");
				return;
			}

			if (!TextAreaSelection.IsEmpty) {
				imagePanel.Focus();
				XRect theRect = new XRect();
				theRect.Bottom = TextAreaSelection.Bottom;
				theRect.Left = TextAreaSelection.Left;
				theRect.Top = TextAreaSelection.Top;
				theRect.Right = TextAreaSelection.Right;

				PDFUtilities.AddTextBlock(mDoc, selectedBlockTextBox.Text, theRect, (int)fontSizeNumericUpDown.Value, mTextColor, fitToSelectionCheckBox.Checked);

				TextAreaSelection = new Rectangle();
				mTextBlocks = mAnnotatedContent.Parse(mDoc);
				UpdateView();
				imagePanel.Focus();
			}
			else {
				imagePanel.Focus();

				TextBlock theBlock = SelectedBlock;
				if (theBlock != null) {
					mAnnotatedContent.UpdateTextBlock(theBlock, selectedBlockTextBox.Text);
					mTextBlocks = mAnnotatedContent.Parse(mDoc);
					UpdateView();
				}
			}

		}
		private void fitToSelectionCheckBox_CheckedChanged(object sender, System.EventArgs e) {
			fontSizeNumericUpDown.Enabled = !fitToSelectionCheckBox.Checked;		
		}

		private void fontColorButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
			Graphics g = e.Graphics;
			g.FillRectangle(new SolidBrush(mTextColor), new Rectangle(4, 4, fontColorButton.Size.Width - 8, fontColorButton.Size.Height - 8));
		}

		private void fontColorButton_Click(object sender, System.EventArgs e) {
			ColorDialog theDialog = new ColorDialog();
			theDialog.Color = mTextColor;
			DialogResult res = theDialog.ShowDialog();
			if (res == DialogResult.OK) {
				mTextColor = theDialog.Color;
				fontColorButton.Refresh();
			}
		}
		#endregion
	}
}
