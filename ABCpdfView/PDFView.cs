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
using System.Data;


namespace ABCpdfView {
	/// <summary>
	/// Main view windows.
	/// </summary>
	public class ABCpdfView : System.Windows.Forms.Form {
		private System.Windows.Forms.MenuItem FileMenu;
		private System.Windows.Forms.MenuItem New;
		private System.Windows.Forms.MenuItem Open;
		private System.Windows.Forms.MenuItem Save;
		private System.Windows.Forms.MenuItem SaveAs;
		private System.Windows.Forms.MenuItem PrintPreview;
		private System.Windows.Forms.MenuItem PageSetup;
		private System.Windows.Forms.MenuItem Print;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem MenuOpen;
		private System.Windows.Forms.MenuItem MenuSaveAs;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem MenuPrint;
		private System.Windows.Forms.MenuItem MenuPrintPreview;
		private System.Windows.Forms.MenuItem MenuPageSetup;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem MenuExit;
		private System.Windows.Forms.Splitter splitter1;
		private ABCpdfControls.PDFControl pdfControl1;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuDocument;
		private System.Windows.Forms.MenuItem menuAddWatermark;
		private System.Windows.Forms.MenuItem menuInsertPages;
		private System.Windows.Forms.MenuItem menuDeletePages;
		private System.Windows.Forms.MenuItem menuExtractPages;
		private System.Windows.Forms.MenuItem menuRotatePages;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ABCpdfView() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			this.FileMenu = new System.Windows.Forms.MenuItem();
			this.New = new System.Windows.Forms.MenuItem();
			this.Open = new System.Windows.Forms.MenuItem();
			this.Save = new System.Windows.Forms.MenuItem();
			this.SaveAs = new System.Windows.Forms.MenuItem();
			this.Print = new System.Windows.Forms.MenuItem();
			this.PrintPreview = new System.Windows.Forms.MenuItem();
			this.PageSetup = new System.Windows.Forms.MenuItem();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.MenuOpen = new System.Windows.Forms.MenuItem();
			this.MenuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.MenuPrint = new System.Windows.Forms.MenuItem();
			this.MenuPrintPreview = new System.Windows.Forms.MenuItem();
			this.MenuPageSetup = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.MenuExit = new System.Windows.Forms.MenuItem();
			this.menuDocument = new System.Windows.Forms.MenuItem();
			this.menuAddWatermark = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuInsertPages = new System.Windows.Forms.MenuItem();
			this.menuDeletePages = new System.Windows.Forms.MenuItem();
			this.menuExtractPages = new System.Windows.Forms.MenuItem();
			this.menuRotatePages = new System.Windows.Forms.MenuItem();
			this.pdfControl1 = new ABCpdfControls.PDFControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// FileMenu
			// 
			this.FileMenu.Index = -1;
			this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.New,
																					 this.Open,
																					 this.Save,
																					 this.SaveAs,
																					 this.Print,
																					 this.PrintPreview,
																					 this.PageSetup});
			this.FileMenu.Text = "File";
			// 
			// New
			// 
			this.New.Index = 0;
			this.New.Text = "New";
			// 
			// Open
			// 
			this.Open.Index = 1;
			this.Open.Text = "Open...";
			// 
			// Save
			// 
			this.Save.Index = 2;
			this.Save.Text = "Save";
			// 
			// SaveAs
			// 
			this.SaveAs.Index = 3;
			this.SaveAs.Text = "Save As...";
			// 
			// Print
			// 
			this.Print.Index = 4;
			this.Print.Text = "Print...";
			// 
			// PrintPreview
			// 
			this.PrintPreview.Index = 5;
			this.PrintPreview.Text = "Print Preview";
			// 
			// PageSetup
			// 
			this.PageSetup.Index = 6;
			this.PageSetup.Text = "Page Setup...";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuDocument});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.MenuOpen,
																					  this.MenuSaveAs,
																					  this.menuItem6,
																					  this.MenuPrint,
																					  this.MenuPrintPreview,
																					  this.MenuPageSetup,
																					  this.menuItem10,
																					  this.MenuExit});
			this.menuItem1.Text = "File";
			this.menuItem1.Select += new System.EventHandler(this.menuItem1_Select);
			// 
			// MenuOpen
			// 
			this.MenuOpen.Index = 0;
			this.MenuOpen.Text = "Open...";
			this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
			// 
			// MenuSaveAs
			// 
			this.MenuSaveAs.Index = 1;
			this.MenuSaveAs.Text = "Save As...";
			this.MenuSaveAs.Click += new System.EventHandler(this.MenuSaveAs_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.Text = "-";
			// 
			// MenuPrint
			// 
			this.MenuPrint.Index = 3;
			this.MenuPrint.Text = "Print...";
			this.MenuPrint.Click += new System.EventHandler(this.MenuPrint_Click);
			// 
			// MenuPrintPreview
			// 
			this.MenuPrintPreview.Index = 4;
			this.MenuPrintPreview.Text = "Print Preview";
			this.MenuPrintPreview.Click += new System.EventHandler(this.MenuPrintPreview_Click);
			// 
			// MenuPageSetup
			// 
			this.MenuPageSetup.Index = 5;
			this.MenuPageSetup.Text = "Page Setup...";
			this.MenuPageSetup.Click += new System.EventHandler(this.MenuPageSetup_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 6;
			this.menuItem10.Text = "-";
			// 
			// MenuExit
			// 
			this.MenuExit.Index = 7;
			this.MenuExit.Text = "Exit";
			this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
			// 
			// menuDocument
			// 
			this.menuDocument.Index = 1;
			this.menuDocument.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAddWatermark,
																						 this.menuItem8,
																						 this.menuInsertPages,
																						 this.menuDeletePages,
																						 this.menuExtractPages,
																						 this.menuRotatePages});
			this.menuDocument.Text = "Document";
			this.menuDocument.Select += new System.EventHandler(this.menuItem1_Select);
			// 
			// menuAddWatermark
			// 
			this.menuAddWatermark.Index = 0;
			this.menuAddWatermark.Text = "Add Watermark...";
			this.menuAddWatermark.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// menuInsertPages
			// 
			this.menuInsertPages.Index = 2;
			this.menuInsertPages.Text = "Insert Pages...";
			this.menuInsertPages.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuDeletePages
			// 
			this.menuDeletePages.Index = 3;
			this.menuDeletePages.Text = "Delete Pages...";
			this.menuDeletePages.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuExtractPages
			// 
			this.menuExtractPages.Index = 4;
			this.menuExtractPages.Text = "Extract Pages...";
			this.menuExtractPages.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuRotatePages
			// 
			this.menuRotatePages.Index = 5;
			this.menuRotatePages.Text = "Rotate Pages...";
			this.menuRotatePages.Click += new System.EventHandler(this.menuRotatePages_Click);
			// 
			// pdfControl1
			// 
			this.pdfControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pdfControl1.DotsPerInch = 72;
			this.pdfControl1.ImeMode = System.Windows.Forms.ImeMode.On;
			this.pdfControl1.Location = new System.Drawing.Point(8, 0);
			this.pdfControl1.Name = "pdfControl1";
			this.pdfControl1.PageNumber = 0;
			this.pdfControl1.Size = new System.Drawing.Size(512, 424);
			this.pdfControl1.TabIndex = 0;
			this.pdfControl1.Load += new System.EventHandler(this.pdfControl1_Load);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 433);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// ABCpdfView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 433);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pdfControl1);
			this.Menu = this.mainMenu1;
			this.Name = "ABCpdfView";
			this.Text = "ABCpdfView";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new ABCpdfView());
		}

		private void MenuOpen_Click(object sender, System.EventArgs e) {
			pdfControl1.Open();
		}

		private void MenuSave_Click(object sender, System.EventArgs e) {
			pdfControl1.Save();
		}

		private void MenuSaveAs_Click(object sender, System.EventArgs e) {
			pdfControl1.SaveAs();
		}

		private void MenuPrint_Click(object sender, System.EventArgs e) {
			pdfControl1.Print();
		}

		private void MenuPrintPreview_Click(object sender, System.EventArgs e) {
			pdfControl1.PrintPreview();
		}

		private void MenuPageSetup_Click(object sender, System.EventArgs e) {
			pdfControl1.PageSetup();
		}

		private void MenuExit_Click(object sender, System.EventArgs e) {
			Application.Exit();
		}

		private void menuItem1_Select(object sender, System.EventArgs e) {
			MenuPrint.Enabled = pdfControl1.DocumentOpened;
			MenuPrintPreview.Enabled = pdfControl1.DocumentOpened;
			MenuSaveAs.Enabled = pdfControl1.DocumentOpened;
			menuAddWatermark.Enabled = pdfControl1.DocumentOpened;
			menuInsertPages.Enabled = pdfControl1.DocumentOpened;
			menuDeletePages.Enabled = pdfControl1.DocumentOpened;
			menuExtractPages.Enabled = pdfControl1.DocumentOpened;
			menuRotatePages.Enabled = pdfControl1.DocumentOpened;
		}

		private void menuItem3_Click(object sender, System.EventArgs e) {
			pdfControl1.InsertPages();
		}

		private void menuItem4_Click(object sender, System.EventArgs e) {
			pdfControl1.DeletePages();
		}

		private void menuItem5_Click(object sender, System.EventArgs e) {
			pdfControl1.ExtractPages();
		}

		private void menuItem7_Click(object sender, System.EventArgs e) {
			pdfControl1.AddWatermark();
		}

		private void menuRotatePages_Click(object sender, System.EventArgs e) {
			pdfControl1.RotatePages();
		}

		private void pdfControl1_Load(object sender, System.EventArgs e) {
		}
	}
}
