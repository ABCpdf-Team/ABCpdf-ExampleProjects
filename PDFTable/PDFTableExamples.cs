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
using System.IO;
using WebSupergoo.ABCpdf14;


namespace PDFTableExamples {

	public class PDFTableExamplesApp {
		[STAThread]
		static void Main() {
			InvoiceTest();
			SmallTest();
			LargeTest();
			SpaceSampleTest();
		}
		
		static private string ReadDataFromFile(string path) {
			StreamReader sr = File.OpenText(path);
			string result = sr.ReadToEnd();
			sr.Close();
			return result;
		}

		static void SmallTest() {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\TextData\\";
			
			string theText = ReadDataFromFile(theRez + "text6.txt");

			Doc theDoc = new Doc();
			// set up document
			theDoc.FontSize = 16;
			theDoc.Rect.Inset(20, 100);

			theText = theText.Trim();
			theText = theText.Replace("\r\n", "\r");
			string[] theRows = theText.Split(new char[] {'\r'});

			// set up table
			PDFTable theTable = new PDFTable(theDoc, 5);
			theTable.CellPadding = 5;

			// add some rotated text
			if (true) {
				theTable.NextRow();
				// We need to specify a height as the width that the text has
				// to be drawn into has to be fixed (much the same as table columns).
				theTable.SetRowHeight(72);
				theTable.HorizontalAlignment = 0; // because of rotation - aligned at bottom
				theDoc.TextStyle.VPos = 0.5; // because of rotation - aligned horizontally in middle
				for (int j = 0; j < 5; j++) {
					string[] names = { "One", "Two", "Three", "Four", "Five" };
					theTable.SelectCell(theTable.Row, j, true);
					theTable.SelectionRotate();
					theDoc.AddText("Vertical Column Number " + names[j]);
					theTable.SelectionReset();
				}
				theDoc.TextStyle.VPos = 0;
			}
			
			// add the content
			theTable.HorizontalAlignment = 1;
			for (int i = 0; i < theRows.Length; i++) {
				theTable.NextRow();
				string[] theCols = theRows[i].Split(new char[] {'\t'});
				theCols[0] = "<stylerun hpos=0>" + theCols[0] + "</stylerun>";
				theTable.AddTextStyled(theCols);
				if ((i % 2) == 1) 
					theTable.FillRow("220 220 220", i);
			}

			theTable.Frame();

			for (int p = 1; p <= theDoc.PageCount; p++) {
				theDoc.PageNumber = p;
				theDoc.Flatten();
			}
			theDoc.Save(Directory.GetParent(theBase).Parent.FullName + "\\SmallTable.pdf");
			theDoc.Clear();
		}

		static void LargeTest() 
		{
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\TextData\\";
			
			string theText = ReadDataFromFile(theRez + "text7.txt");

			Doc theDoc = new Doc();
			// set up document
			theDoc.FontSize = 12;
			theDoc.Rect.Inset(20, 20);

			PDFTable theTable = new PDFTable(theDoc, 6);
			theTable.SetColumnWidths(new double [] {2, 1, 3 , 2, 1, 4});
			theTable.CellPadding = 5;
			theTable.RepeatHeader = true;

			theText = theText.Trim();
			theText = theText.Replace("\r\n", "\r");
			string[] theRows = theText.Split(new char[] {'\r'});
			int thePage = 1;
			bool theShade = false;
			for (int i = 0; i < theRows.Length; i++) {
				theTable.NextRow();
				string[] theCols = theRows[i].Split(new char[] {'\t'});
				theTable.AddTextStyled(theCols);
				if (theDoc.PageNumber > thePage) {
					thePage = theDoc.PageNumber;
					theShade = true;
				}
				if (theShade)
					theTable.FillRow("200 200 200", theTable.Row);
				theShade = ! theShade;
			}

			for (int p = 1; p <= theDoc.PageCount; p++) {
				theDoc.PageNumber = p;
				theDoc.Flatten();
			}
			theDoc.Save(Directory.GetParent(theBase).Parent.FullName + "\\LargeTable.pdf");
			theDoc.Clear();
		}


		static void InvoiceTest() {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\InvoiceData\\";
		
			InvoiceData invoiceData = new InvoiceData();
			invoiceData.LoadFromFile(theRez + "travelfun.xml");

			Doc theDoc = new Doc();
			// set up document
			theDoc.FontSize = 14;
			theDoc.Rect.Inset(40, 20);
			int padding = 5;

			PDFTable layoutTable = new PDFTable(theDoc, 1);
			layoutTable.VerticalAlignment = 0;

			layoutTable.NextCell();
			PDFTable headerTable = layoutTable.AddTable(2, padding);
			headerTable.SetColumnWidth(1, 4);
			headerTable.NextCell();

			XImage logoImage = new XImage();
			logoImage.SetFile(theRez + invoiceData.Company.logo);
			headerTable.AddImage(logoImage, true);
			headerTable.NextCell();
			PDFTable companyInfoTable = headerTable.AddTable(1);
			companyInfoTable.AddTextStyled(invoiceData.GetHeaderTableData());

			layoutTable.NextRow();
			layoutTable.NextCell();

			PDFTable orderTable = layoutTable.AddTable(3);
			orderTable.SetColumnWidths(new double[] {3, 0.2, 2});
			orderTable.NextCell();

			PDFTable customerTable = orderTable.AddTable(2, padding);
			customerTable.SetColumnWidth(1, 3);
			customerTable.AddTextStyled(invoiceData.GetCustomerTableData());
			customerTable.Frame();
	
			orderTable.NextCell(2);

			PDFTable orderInfoTable = orderTable.AddTable(2, padding);
			orderInfoTable.SetColumnWidth(1, 2);
			orderInfoTable.AddTextStyled(invoiceData.GetOrderTableData());
			orderInfoTable.Frame();

			layoutTable.NextRow();
			layoutTable.NextCell();
			layoutTable.Advance(10);

			PDFTable orderItemsTable = layoutTable.AddTable(4, padding);
			orderItemsTable.RepeatHeader = true;
			orderItemsTable.FrameHeader = true;
			orderItemsTable.VerticalAlignment = 0.5;

			orderItemsTable.SetColumnWidths(new double[] {1,4,1,1});
			orderItemsTable.AddTextStyled(invoiceData.GetOrderItemsTableData());
			orderItemsTable.FrameColumns();
			orderItemsTable.FillColumn("240 240 240", 3);

			layoutTable.NextRow();
			layoutTable.NextCell();
			PDFTable footerTable = layoutTable.AddTable(3);
			footerTable.SetColumnWidths(new double[] {3,2,2});
			footerTable.NextCell();
			footerTable.Advance(10);

			PDFTable paymentTable = footerTable.AddTable(2, padding);
			paymentTable.AddTextStyled(invoiceData.GetPaymentTableData());
			paymentTable.Frame();

			footerTable.NextCell(2);
			PDFTable totalTable = footerTable.AddTable(2,padding);
			totalTable.AddTextStyled(invoiceData.GetTotalTableData());
			totalTable.FrameColumns();
			totalTable.FillColumn("240 240 240", 1);

			for (int p = 1; p <= theDoc.PageCount; p++) {
				theDoc.PageNumber = p;
				theDoc.Flatten();
			}
			theDoc.Save(Directory.GetParent(theBase).Parent.FullName + "\\Invoice.pdf");
			theDoc.Clear();
		}

		static void SpaceSampleTest() 
		{
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";
		
			SpaceSampleData spaceSampleData = new SpaceSampleData();
			spaceSampleData.LoadFromFile(theRez + "\\SpaceSampleData\\", theRez + "\\SpaceSampleData\\spaceSample.xml");

			Doc theDoc = new Doc();
			theDoc.Font = theDoc.AddFont("Helvetica");
			theDoc.FontSize = 12;
			theDoc.Rect.Inset(20, 20);

			PDFTable layoutTable = new PDFTable(theDoc, 1);
			PDFTable titleTable = layoutTable.AddTable(1);
			titleTable.CellPadding = 5;
			titleTable.NextCell();
			titleTable.AddTextStyled("<StyleRun fontsize = 30>Conquering Space</StyleRun>");
			titleTable.NextCell();
			titleTable.AddTextStyled("<StyleRun fontsize = 20>Apollo program overview </StyleRun>");
			titleTable.NextCell();
			titleTable.AddTextStyled("Images and Text Courtesy of <A href = \"http://images.jsc.nasa.gov/\"> NASA </A>");
			titleTable.Fill("230 230 230");
			layoutTable.Advance(10);

			for (int i = 0; i < spaceSampleData.Items.Count; i++) {
				SpaceSampleItem item = spaceSampleData.Items[i] as SpaceSampleItem;
				layoutTable.NextCell();
				PDFTable itemTable = layoutTable.AddTable(1);
				itemTable.NextCell();
				PDFTable headerTable = itemTable.AddTable(6, 10);
				headerTable.SetColumnWidths(new double[6] {1.2, 1, 1, 1, 1, 1});
				headerTable.AddEnumerableData(item.header);
				itemTable.NextCell();
				PDFTable infoTable = itemTable.AddTable(1, 10);
				infoTable.AddEnumerableData(item.info);
				string theColor = theDoc.Color.String;
				theDoc.Color.String = "255 255 255";
				headerTable.FrameColumns();
				infoTable.FrameRows();
				theDoc.Color.String = theColor;
				layoutTable.FillRow("230 230 230", layoutTable.Row);
				layoutTable.Advance(3);
			}

			for (int p = 1; p <= theDoc.PageCount; p++) {
				theDoc.PageNumber = p;
				theDoc.Flatten();
			}
			theDoc.Save(Directory.GetParent(theBase).Parent.FullName + "\\SpaceSample.pdf");
			theDoc.Clear();
		}
	}
}
