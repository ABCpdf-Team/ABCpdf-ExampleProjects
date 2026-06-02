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

using AdvancedGraphics;
using ExamplesProcessing;
using PDFTableExamples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
#if NETFRAMEWORK
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
#endif
using System.Xml;
using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Atoms;
using WebSupergoo.ABCpdf14.Drawing;
using WebSupergoo.ABCpdf14.Elements;
using WebSupergoo.ABCpdf14.Objects;
using WebSupergoo.ABCpdf14.Operations;
using WebSupergoo.ABCpdf14.Python;
using WebSupergoo.Annotations;
#if NETFRAMEWORK
using WPFTable;
#endif
using Bitmap = System.Drawing.Bitmap;
using ColorSpace = WebSupergoo.ABCpdf14.Objects.ColorSpace;
#if NETFRAMEWORK
using FlowDocumentPageViewer = System.Windows.Controls.FlowDocumentPageViewer;
using LogicalTreeHelper = System.Windows.LogicalTreeHelper;
#endif
using Graphics = System.Drawing.Graphics;
using Page = WebSupergoo.ABCpdf14.Objects.Page;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using PDFContent = AdvancedGraphics.PDFContent;

#pragma warning disable CS0618, CS0219, CS8321

namespace ExampleTests {
	class Tests {
		static string GetUri(string file) {
			file = Path.GetFullPath(file);
			var uri = new Uri(file);
			return uri.AbsolutePath;
		}

		// Start Tests

		// Text Flow Example
		// 
		// This example shows how to flow text from one area to another. The techniques shown
		// here are used to flow text between pages but they could equally well be applied to
		// flowing text between areas - such as columns - on the same page.
		//
		// File Start: False .\4-examples\02-textflow.htm
		public static void Ex4_examples_02_textflow() {
			// Part: 1 of 4
			int id = 0;
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt. Gallos ab Aquitanis Garumna flumen, a Belgis Matrona et Sequana dividit. Horum omnium fortissimi sunt Belgae, propterea quod a cultu atque humanitate provinciae longissime absunt, minimeque ad eos mercatores saepe commeant atque ea quae ad effeminandos animos pertinent important, proximique sunt Germanis, qui trans Rhenum incolunt, quibuscum continenter bellum gerunt. Qua de causa Helvetii quoque reliquos Gallos virtute praecedunt, quod fere cotidianis proeliis cum Germanis contendunt, cum aut suis finibus eos prohibent aut ipsi in eorum finibus bellum gerunt. [Eorum una, pars, quam Gallos obtinere dictum est, initium capit a flumine Rhodano, continetur Garumna flumine, Oceano, finibus Belgarum, attingit etiam ab Sequanis et Helvetiis flumen Rhenum, vergit ad septentriones. Belgae ab extremis Galliae finibus oriuntur, pertinent ad inferiorem partem fluminis Rheni, spectant in septentrionem et orientem solem. Aquitania a Garumna flumine ad Pyrenaeos montes et eam partem Oceani quae est ad Hispaniam pertinet; spectat inter occasum solis et septentriones.] Apud Helvetios longe nobilissimus fuit et ditissimus Orgetorix. Is M. Messala, [et P.] M. Pisone consulibus regni cupiditate inductus coniurationem nobilitatis fecit et civitati persuasit ut de finibus suis cum omnibus copiis exirent: perfacile esse, cum virtute omnibus praestarent, totius Galliae imperio potiri. Id hoc facilius iis persuasit, quod undique loci natura Helvetii continentur: una ex parte flumine Rheno latissimo atque altissimo, qui agrum Helvetium a Germanis dividit; altera ex parte monte Iura altissimo, qui est inter Sequanos et Helvetios; tertia lacu Lemanno et flumine Rhodano, qui provinciam nostram ab Helvetiis dividit. His rebus fiebat ut et minus late vagarentur et minus facile finitimis bellum inferre possent; qua ex parte homines bellandi cupidi magno dolore adficiebantur. Pro multitudine autem hominum et pro gloria belli atque fortitudinis angustos se fines habere arbitrabantur, qui in longitudinem milia passuum CCXL, in latitudinem CLXXX patebant.";
			// End Part:
			// Part: 2 of 4
			using var doc = new Doc();
			doc.Width = 4;
			doc.FontSize = 32;
			doc.TextStyle.Justification = 1;
			doc.Rect.Inset(20, 20);
			// End Part:
			// Part: 3 of 4
			doc.FrameRect();
			id = doc.AddTextStyled(text);
			while (doc.Chainable(id)) {
				doc.Page = doc.AddPage();
				doc.FrameRect();
				id = doc.AddTextStyled("", id);
			}
			// End Part:
			// Part: 4 of 4
			doc.Save("textflow.pdf");
			// End Part:
		}
		// File End:

		// Text Flow Round Image Example
		// 
		// This example shows how to flow text around an image. The techniques shown here may
		// be used in conjunction with the Text Flow example which shows how to flow text between
		// areas on the same or different pages.
		//
		// File Start: False .\4-examples\02-textflow2.htm
		public static void Ex4_examples_02_textflow2() {
			// Part: 1 of 5
			// text truncated for clarity
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes...";
			// End Part:
			// Part: 2 of 5
			using var doc = new Doc();
			doc.Width = 4;
			doc.FontSize = 32;
			doc.TextStyle.Justification = 1;
			doc.Rect.Inset(20, 20);
			// End Part:
			// Part: 3 of 5
			string saveRect = doc.Rect.String;
			using var xi = XImage.FromFile("../mypics/pic.jpg", null);
			doc.Rect.Resize(xi.Width / 2, xi.Height / 2, XRect.Corner.TopLeft);
			doc.AddImage(xi);
			// End Part:
			// Part: 4 of 5
			double padX = doc.FontSize;
			double padY = doc.FontSize / 3.0;
			string format = "<stylerun justification=\"1.0\" leftmargins=\"0 {0} {1}\">";
			string style = string.Format(format, doc.Rect.Height + padY, doc.Rect.Width + padX);
			// End Part:
			// Part: 5 of 5
			doc.Rect.String = saveRect;
			doc.FrameRect();
			int id = doc.AddTextStyled(style + text + "</stylerun>");
			doc.Save("textflowroundimage.pdf");
			// End Part:
		}
		// File End:

		// Tagged Text Example
		// 
		// This example shows how to add text to a PDF document tagged appropriately for PDF/
		// UA.
		//
		// File Start: False .\4-examples\02-texttagged.htm
		public static void Ex4_examples_02_texttagged() {
			// Part: 1 of 5
			using var doc = new Doc();
			var st = doc.Tag.GetStructure();
			st.CreateAsRequired();
			st.Title = "Tagged Document";
			st.Root.AddKid("Document").EntryLang = "en-GB";
			// End Part:
			// Part: 2 of 5
			doc.Font = doc.EmbedFont("Arial");
			doc.TextStyle.Size = 36;
			doc.Page = doc.AddPage();
			doc.Rect.Inset(72, 72);
			doc.TextStyle.AutoTag = true;
			doc.AddTextStyled("<h1 fontsize=48>Animals<h1><p>Koala<p><p>Squirrel<p>");
			// End Part:
			// Part: 3 of 5
			doc.Pos.Y -= 72;
			doc.Tag.Open("Sect", "Div", "P");
			doc.AddText("Div one paragraph one.\r\n");
			doc.Tag.CloseOpen("P");
			doc.AddText("Div one paragraph two.\r\n");
			doc.Tag.Close("P", "Div");
			doc.Tag.Open("Div", "P");
			doc.AddText("Div two paragraph one.\r\n");
			doc.Tag.CloseOpen("P");
			doc.AddText("Div two paragraph two.\r\n");
			doc.Tag.Close("P", "Div", "Sect");
			// End Part:
			// Part: 4 of 5
			doc.Tag.Roles["RubberStamp"] = "Annot";
			doc.Tag.Classes["RedBorder"] = Atom.FromString("<< /O /Layout /BorderColor [1 0 0] /BorderStyle /Solid >>");
			var graphic = doc.Tag.MakeTag("RubberStamp");
			graphic.Attributes = new DictAtom();
			graphic.Attributes["Alt"] = new StringAtom("Classification: Secret");
			graphic.Attributes["C"] = new NameAtom("RedBorder");
			graphic.Object = new StampAnnotation(doc, XRect.FromLbwh(200, 100, 200, 80), "SECRET", XColor.FromRgb(255, 0, 0));
			doc.Tag.OpenClose(graphic);
			// End Part:
			// Part: 5 of 5
			doc.Save("taggedtext.pdf");
			st.UpdateActualText(true, true);
			var txt = st.ExtractStructure();
			File.WriteAllText("taggedtext.txt", txt.ToString());
			// End Part:
		}
		// File End:

		// Multistyle Example
		// 
		// This example shows how to create multistyled text.
		//
		// File Start: False .\4-examples\03-multistyled.htm
		public static void Ex4_examples_03_multistyled() {
			// Part: 1 of 4
			string text = "<b>Gallia</b> est omnis divisa in partes tres, quarum unam incolunt <b>Belgae</b>, aliam <b>Aquitani</b>, tertiam qui ipsorum lingua <b>Celtae</b>, nostra <b>Galli</b> appellantur.";
			// End Part:
			// Part: 2 of 4
			using var doc = new Doc();
			doc.FontSize = 72;
			doc.Rect.Inset(10, 10);
			doc.FrameRect();
			int font1 = doc.EmbedFont("Verdana", LanguageType.Latin, false, true);
			int font2 = doc.EmbedFont("Verdana Bold", LanguageType.Latin, false, true);
			// End Part:
			// Part: 3 of 4
			text = "<font pid=" + font1.ToString() + ">" + text + "</font>";
			text = text.Replace("<b>", "<font pid=" + font2.ToString() + ">");
			text = text.Replace("</b>", "</font>");
			doc.AddTextStyled(text);
			// End Part:
			// Part: 4 of 4
			doc.Save("styles.pdf");
			// End Part:
		}
		// File End:

		// Image Example
		// 
		// This example shows how to create a simple PDF displaying an image.
		//
		// File Start: False .\4-examples\04-image.htm
		public static void Ex4_examples_04_image() {
			// Part: 1 of 2
			using var img = new XImage();
			img.SetFile("../mypics/pic.jpg");
			// End Part:
			// Part: 2 of 2
			using var doc = new Doc();
			doc.Rect.Left = 50;
			doc.Rect.Bottom = 25;
			doc.Rect.Width = img.Width;
			doc.Rect.Height = img.Height;
			doc.AddImageObject(img, false);
			doc.Save("image.pdf");
			// End Part:
		}
		// File End:

		// Deletion Example
		// 
		// This example shows how to delete pages from a PDF document.
		//
		// File Start: False .\4-examples\05-deletion.htm
		public static void Ex4_examples_05_deletion() {
			// Part: 1 of 3
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			int count = doc.PageCount - 1;
			// End Part:
			// Part: 2 of 3
			for (int i = 0; i < count; i++) {
				doc.PageNumber = 2;
				doc.Delete(doc.Page);
			}
			// End Part:
			// Part: 3 of 3
			doc.FontSize = 500;
			doc.Color.String = "255 0 0";
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.3;
			doc.AddText(count.ToString());
			doc.Save("deletion.pdf");
			// End Part:
		}
		// File End:

		// Headers and Footers Example
		// 
		// This example shows one method of adding headers and footers.
		//
		// File Start: False .\4-examples\06-headers.htm
		public static void Ex4_examples_06_headers() {
			// Part: 1 of 5
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt. Gallos ab Aquitanis Garumna flumen, a Belgis Matrona et Sequana dividit. Horum omnium fortissimi sunt Belgae, propterea quod a cultu atque humanitate provinciae longissime absunt, minimeque ad eos mercatores saepe commeant atque ea quae ad effeminandos animos pertinent important, proximique sunt Germanis, qui trans Rhenum incolunt, quibuscum continenter bellum gerunt. Qua de causa Helvetii quoque reliquos Gallos virtute praecedunt, quod fere cotidianis proeliis cum Germanis contendunt, cum aut suis finibus eos prohibent aut ipsi in eorum finibus bellum gerunt. [Eorum una, pars, quam Gallos obtinere dictum est, initium capit a flumine Rhodano, continetur Garumna flumine, Oceano, finibus Belgarum, attingit etiam ab Sequanis et Helvetiis flumen Rhenum, vergit ad septentriones. Belgae ab extremis Galliae finibus oriuntur, pertinent ad inferiorem partem fluminis Rheni, spectant in septentrionem et orientem solem. Aquitania a Garumna flumine ad Pyrenaeos montes et eam partem Oceani quae est ad Hispaniam pertinet; spectat inter occasum solis et septentriones.] Apud Helvetios longe nobilissimus fuit et ditissimus Orgetorix. Is M. Messala, [et P.] M. Pisone consulibus regni cupiditate inductus coniurationem nobilitatis fecit et civitati persuasit ut de finibus suis cum omnibus copiis exirent: perfacile esse, cum virtute omnibus praestarent, totius Galliae imperio potiri. Id hoc facilius iis persuasit, quod undique loci natura Helvetii continentur: una ex parte flumine Rheno latissimo atque altissimo, qui agrum Helvetium a Germanis dividit; altera ex parte monte Iura altissimo, qui est inter Sequanos et Helvetios; tertia lacu Lemanno et flumine Rhodano, qui provinciam nostram ab Helvetiis dividit. His rebus fiebat ut et minus late vagarentur et minus facile finitimis bellum inferre possent; qua ex parte homines bellandi cupidi magno dolore adficiebantur. Pro multitudine autem hominum et pro gloria belli atque fortitudinis angustos se fines habere arbitrabantur, qui in longitudinem milia passuum CCXL, in latitudinem CLXXX patebant.";
			// End Part:
			// Part: 2 of 5
			doc.Rect.String = "100 200 500 600";
			doc.Color.String = "255 0 0";
			doc.FontSize = 24;
			int id = doc.AddTextStyled(text);
			doc.FrameRect();
			while (doc.Chainable(id)) {
				doc.Page = doc.AddPage();
				id = doc.AddTextStyled("", id);
				doc.FrameRect();
			}
			int count = doc.PageCount;
			// End Part:
			// Part: 3 of 5
			doc.Rect.String = "100 650 500 750";
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.5;
			doc.Color.String = "0 255 0";
			doc.FontSize = 36;
			for (int i = 1; i <= count; i++) {
				doc.PageNumber = i;
				doc.AddText("De Bello Gallico");
				doc.FrameRect();
			}
			// End Part:
			// Part: 4 of 5
			doc.Rect.String = "100 50 500 150";
			doc.TextStyle.HPos = 1.0;
			doc.TextStyle.VPos = 0.5;
			doc.Color.String = "0 0 255";
			doc.FontSize = 36;
			for (int i = 1; i <= count; i++) {
				doc.PageNumber = i;
				doc.AddText("Page " + i.ToString() + " of " + count.ToString());
				doc.FrameRect();
			}
			// End Part:
			// Part: 5 of 5
			doc.Save("headerfooter.pdf");
			// End Part:
		}
		// File End:

		// Landscape Example
		// 
		// This example shows how to create a PDF document rotated by 90 degrees for a landscape
		// rather than portrait view.
		//
		// File Start: False .\4-examples\08-landscape.htm
		public static void Ex4_examples_08_landscape() {
			// Part: 1 of 2
			using var doc = new Doc();
			// apply a rotation transform
			double w = doc.MediaBox.Width;
			double h = doc.MediaBox.Height;
			double l = doc.MediaBox.Left;
			double b = doc.MediaBox.Bottom;
			doc.Transform.Rotate(90, l, b);
			doc.Transform.Translate(w, 0);
			
			// rotate our rectangle
			doc.Rect.Width = h;
			doc.Rect.Height = w;
			
			// add some text
			doc.Rect.Inset(50, 50);
			doc.FontSize = 96;
			doc.AddText("Landscape Orientation");
			// End Part:
			// Part: 2 of 2
			// adjust the default rotation and save
			int id = doc.GetInfoInt(doc.Root, "Pages");
			doc.SetInfo(id, "/Rotate", "90");
			doc.Save("landscape.pdf");
			// End Part:
		}
		// File End:

		// Small Table Example
		// 
		// This example shows how to draw a single page table. ABCpdf does not provide table
		// drawing routines itself so this example uses a Table Class to position the table
		// elements.
		// 
		// You can find the full project and classes under the ABCpdf menu item. The project
		// includes code for laying out a small table, a large table spreading over more than
		// one page, an invoice and a product list.
		//
		// File Start: False .\4-examples\09-table1.htm
		public static void Ex4_examples_09_table1() {
			// Part: 1 of 3
			string text = File.ReadAllText("../Rez/text6.txt");
			using var doc = new Doc();
			// set up document
			doc.FontSize = 16;
			doc.Rect.Inset(20, 20);
			// End Part:
			// Part: 2 of 3
			var table = new PDFTable(doc, 5);
			table.CellPadding = 5;
			table.HorizontalAlignment = 1;
			// End Part:
			// Part: 3 of 3
			text = text.Trim();
			text = text.Replace("\r\n", "\r");
			string[] theRows = text.Split([ '\r' ]);
			
			for (int i = 0; i < theRows.Length; i++) {
				table.NextRow();
				string[] theCols = theRows[i].Split([ '\t' ]);
				theCols[0] = "<stylerun hpos=0>" + theCols[0] + "</stylerun>";
				table.AddTextStyled(theCols);
				if ((i % 2) == 1)
					table.FillRow("220 220 220", i);
			}
			table.Frame();
			doc.Save("table1.pdf");
			// End Part:
		}
		// File End:

		// Large Table Example
		// 
		// This example shows how to draw a multi-page table. ABCpdf does not provide table
		// drawing routines itself so this example uses a Table Class to position the table
		// elements.
		// 
		// You can find the full project and classes under the ABCpdf menu item. The project
		// includes code for laying out a small table, a large table spreading over more than
		// one page, an invoice and a product list.
		//
		// File Start: False .\4-examples\10-table2.htm
		public static void Ex4_examples_10_table2() {
			// Part: 1 of 3
			string text = File.ReadAllText("../Rez/text7.txt");
			using var doc = new Doc();
			// set up document
			doc.FontSize = 12;
			doc.Rect.Inset(20, 20);
			// End Part:
			// Part: 2 of 3
			PDFTable theTable = new PDFTable(doc, 6);
			// some columns extra width
			theTable.SetColumnWidths([ 2.0, 1.0, 3.0, 2.0, 1.0, 4.0 ]);
			theTable.CellPadding = 5;
			theTable.RepeatHeader = true;
			// End Part:
			// Part: 3 of 3
			text = text.Replace("\r\n", "\r");
			string[] theRows = text.Split([ '\r' ]);
			int thePage = 1;
			bool theShade = false;
			for (int i = 0; i < theRows.Length; i++) {
				theTable.NextRow();
				string[] theCols = theRows[i].Split([ '\t' ]);
				theTable.AddTextStyled(theCols);
				if (doc.PageNumber > thePage) {
					thePage = doc.PageNumber;
					theShade = true;
				}
				if (theShade)
					theTable.FillRow("200 200 200", theTable.Row);
				theShade = !theShade;
			}
			doc.Save("table2.pdf");
			// End Part:
		}
		// File End:

		// Unicode Example
		// 
		// This example shows how to add complex scripts such as Chinese, Japanese and Korean.
		// Here we choose to embed and subset our font to ensure our document renders correctly
		// on all platforms.
		//
		// File Start: False .\4-examples\12-unicode.htm
		public static void Ex4_examples_12_unicode() {
			// Part: 1 of 5
			using var doc = new Doc();
			doc.FontSize = 32;
			// End Part:
			// Part: 2 of 5
			string path = "../Rez/Japanese2.txt";
			string text = File.ReadAllText(path);
			// End Part:
			// Part: 3 of 5
			doc.Page = doc.AddPage();
			doc.Font = doc.EmbedFont("MS PGothic", LanguageType.Unicode, false, true);
			doc.AddText("Japanese" + text);
			// End Part:
			// Part: 4 of 5
			doc.Page = doc.AddPage();
			doc.Font = doc.EmbedFont("MS PGothic", LanguageType.Unicode, true, true);
			doc.AddText("Japanese" + text);
			// End Part:
			// Part: 5 of 5
			doc.Save("unicode.pdf");
			// finished
			// End Part:
		}
		// File End:

		// Paged HTML Example
		// 
		// This example shows how to import an HTML page into a multi-page PDF document.
		//
		// File Start: False .\4-examples\13-pagedhtml.htm
		public static void Ex4_examples_13_pagedhtml() {
			// Part: 1 of 5
			using var doc = new Doc();
			doc.Rect.Inset(72, 144);
			// End Part:
			// Part: 2 of 5
			doc.HtmlOptions.Engine = EngineType.Chrome146;
			doc.HtmlOptions.UseScript = true; // enable JavaScript
			doc.HtmlOptions.Media = MediaType.Print; // Or Screen for a more screen oriented output
			doc.HtmlOptions.InitialWidth = 800; // In case we have a responsive site which is non-specific on good widths
			
			//doc.HtmlOptions.RepaintDelay = 500; // Only required if you have AJAX or animated content such as graphs
			//doc.HtmlOptions.IgnoreCertificateErrors = false; // Disabled for ease of debugging
			//doc.HtmlOptions.FireShield.Policy = XHtmlFireShield.Enforcement.Deny; // Disabled for ease of debugging
			// End Part:
			// Part: 3 of 5
			doc.Page = doc.AddPage();
			int id = doc.AddImageUrl("http://www.yahoo.com/");
			// End Part:
			// Part: 4 of 5
			while (true) {
				doc.FrameRect(); // add a black border
				if (!doc.Chainable(id))
					break;
				doc.Page = doc.AddPage();
				id = doc.AddImageToChain(id);
			}
			// End Part:
			// Part: 5 of 5
			doc.Save("pagedhtml.pdf");
			// End Part:
		}
		// File End:

		// eForm Fields Example
		// 
		// This example shows how to change the values of eForm fields. In this example we simply
		// replace each of the fields in a form with the name of that field.
		//
		// File Start: False .\4-examples\15-eform1.htm
		public static void Ex4_examples_15_eform1() {
			// Part: 1 of 3
			using var doc = new Doc();
			doc.Read("../mypics/form.pdf");
			
			doc.Form.NeedAppearances = false; // for PDF 2.0
			// End Part:
			// Part: 2 of 3
			string[] names = doc.Form.GetFieldNames();
			foreach (string theName in names) {
				var field = doc.Form[theName];
				field.Value = field.Name;
			}
			// End Part:
			// Part: 3 of 3
			doc.Save("eformfields.pdf");
			// End Part:
		}
		// File End:

		// eForm Placeholder Example
		// 
		// This example shows how to use eForm fields as placeholders for the insertion of text.
		// In this example we simply replace each of the fields in a form with the name of that
		// field.
		//
		// File Start: False .\4-examples\15-eform2.htm
		public static void Ex4_examples_15_eform2() {
			// Part: 1 of 3
			using var doc = new Doc();
			doc.Read("../mypics/form.pdf");
			doc.Form.NeedAppearances = false; // for PDF 2.0
			doc.Font = doc.AddFont("Helvetica-Bold");
			doc.FontSize = 16;
			doc.Rect.Pin = XRect.Corner.TopLeft;
			// End Part:
			// Part: 2 of 3
			var names = doc.Form.GetFieldNames();
			foreach (string name in names) {
				Field theField = doc.Form[name];
				theField.Focus();
				doc.Color.String = "240 240 255";
				doc.FillRect();
				doc.Rect.Height = 16;
				doc.Color.String = "220 0 0";
				doc.AddText(theField.Name);
				doc.Delete(theField.ID);
			}
			// End Part:
			// Part: 3 of 3
			doc.Save("eform.pdf");
			// End Part:
		}
		// File End:

		// eForm Stamp Example
		// 
		// This example shows how to stamp eForm fields into a document so that the values are
		// indelibly marked
		//
		// File Start: False .\4-examples\15-eform3.htm
		public static void Ex4_examples_15_eform3() {
			// Part: 1 of 3
			using var doc = new Doc();
			doc.Read("../mypics/form.pdf");
			doc.Form.NeedAppearances = false; // for PDF 2.0
			doc.Font = doc.AddFont("Helvetica-Bold");
			// End Part:
			// Part: 2 of 3
			doc.Form["Day"].Value = "23";
			doc.Form["Month"].Value = "February";
			doc.Form["Year"].Value = "2005";
			doc.Form["State"].Value = "Arizona";
			doc.Form.Stamp();
			// End Part:
			// Part: 3 of 3
			doc.Save("eformstamp.pdf");
			// End Part:
		}
		// File End:

		// eForm FDF Example
		// 
		// This example shows how to extract Unicode annotation values from an eForm FDF file.
		//
		// File Start: False .\4-examples\16-eformfdf.htm
		public static void Ex4_examples_16_eformfdf() {
			// Part: 1 of 4
			using var fdf = new Doc();
			fdf.Read("../Rez/form.fdf");
			// End Part:
			// Part: 2 of 4
			string theValues = "";
			int lastID = Convert.ToInt32(fdf.GetInfo(0, "Count"));
			// End Part:
			// Part: 3 of 4
			// extract annotation values (for insertion into PDF)
			for (int i = 0; i <= lastID; i++) {
				string theType = fdf.GetInfo(i, "Type");
				if (theType == "anno") {
					if (fdf.GetInfo(i, "SubType") == "Text") {
						string theCont;
						theCont = fdf.GetInfo(i, "Contents");
						theValues = theValues + theCont + "\r\n\r\n";
					}
				}
			}
			// extract field values (for demonstration purposes)
			for (int i = 0; i <= lastID; i++) {
				int theN = fdf.GetInfoInt(i, "/FDF*/Fields*:Count");
				for (int j = 0; j < theN; j++) {
					string name = fdf.GetInfo(i, "/FDF*/Fields*[" + j + "]*/T:Text");
					string value = fdf.GetInfo(i, "/FDF*/Fields*[" + j + "]*/V:Text");
					// here we would do something with the field value we've found
				}
			}
			// End Part:
			// Part: 4 of 4
			using var doc = new Doc();
			doc.Font = doc.EmbedFont("Arial", LanguageType.Unicode, false, true);
			doc.FontSize = 96;
			doc.Rect.Inset(10, 10);
			doc.AddText(theValues);
			doc.Save("fdf.pdf");
			// End Part:
		}
		// File End:

		// Advanced Graphics Example
		//
		// File Start: False .\4-examples\17-advancedgraphics.htm
		public static void Ex4_examples_17_advancedgraphics() {
			// Part: 1 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(30);
				theContent.SetLineJoin(2);
				theContent.Move(124, 158);
				theContent.Line(300, 700);
				theContent.Line(476, 158);
				theContent.Line(15, 493);
				theContent.Line(585, 493);
				theContent.Close();
				theContent.Stroke();
				theContent.RestoreState();
				theContent.AddToDoc();
				doc.Save("adv_star_draw.pdf");
			}
			// End Part:
			// Part: 2 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(30);
				theContent.SetLineJoin(2);
				theContent.Move(124, 158);
				theContent.Line(300, 700);
				theContent.Line(476, 158);
				theContent.Line(15, 493);
				theContent.Line(585, 493);
				theContent.Close();
				theContent.Fill();
				theContent.RestoreState();
				theContent.AddToDoc();
				doc.Save("adv_star_fill.pdf");
			}
			// End Part:
			// Part: 3 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(30);
				theContent.Move(100, 50);
				theContent.Bezier(200, 650, 400, 550, 500, 250);
				theContent.Stroke();
				theContent.RestoreState();
			
				// annotate Bezier curve in red
				doc.Color.String = "255 0 0";
				doc.Width = 20;
				doc.FontSize = 30;
				doc.Pos.String = "100 50";
				doc.AddText("p0 (current point)");
				doc.Pos.String = "200 650";
				doc.Pos.Y = doc.Pos.Y + doc.FontSize;
				doc.AddText("p1 (x1, y1)");
				doc.Pos.String = "400 550";
				doc.Pos.Y = doc.Pos.Y + doc.FontSize;
				doc.AddText("p2 (x2, y2)");
				doc.Pos.String = "500 250";
				doc.Pos.X = doc.Pos.X - doc.FontSize;
				doc.AddText("p3 (x3, y3)");
				doc.AddLine(100, 50, 200, 650);
				doc.AddLine(400, 550, 500, 250);
				theContent.AddToDoc();
				doc.Save("adv_bezier.pdf");
			}
			// End Part:
			// Part: 4 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(30);
				theContent.SetLineJoin(2);
				theContent.Move(124, 158);
				theContent.Line(300, 700);
				theContent.Line(476, 158);
				theContent.Line(15, 493);
				theContent.Line(585, 493);
				theContent.Clip();
				theContent.Rect(100, 200, 400, 400);
				theContent.Fill();
				theContent.RestoreState();
				theContent.AddToDoc();
				doc.Save("adv_star_clip.pdf");
			}
			// End Part:
			// Part: 5 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(100);
				theContent.SetLineCap(0);
				theContent.Move(100, 600);
				theContent.Line(500, 600); // line
				theContent.Stroke();
			
				theContent.SetLineCap(1); // round cap
				theContent.Move(100, 400);
				theContent.Line(500, 400);
				theContent.Stroke();
			
				theContent.SetLineCap(2);
				theContent.Move(100, 200);
				theContent.Line(500, 200);
				theContent.Stroke();
			
				// add capped lines
				theContent.AddToDoc();
			
				// annotate capped lines
				doc.FontSize = 48;
				doc.Pos.String = "50 720";
				doc.AddText("0 - Butt Cap");
				doc.Pos.String = "50 520";
				doc.AddText("1 - Round Cap");
				doc.Pos.String = "50 320";
				int id = doc.AddText("2 - Projecting Square Cap");
				doc.Width = 20;
			
				doc.Color.String = "255 255 255";
				doc.AddLine(100, 200, 500, 200);
				doc.Rect.String = "80 180 120 220";
				doc.FillRect(20, 20);
				doc.Rect.String = "480 180 520 220";
				doc.FillRect(20, 20);
				doc.AddLine(100, 400, 500, 400);
				doc.Rect.String = "80 380 120 420";
				doc.FillRect(20, 20);
				doc.Rect.String = "480 380 520 420";
				doc.FillRect(20, 20);
				doc.AddLine(100, 600, 500, 600);
				doc.Rect.String = "80 580 120 620";
				doc.FillRect(20, 20);
				doc.Rect.String = "480 580 520 620";
				doc.FillRect(20, 20);
				doc.Color.String = "0 0 0";
				doc.Save("adv_linecap.pdf");
			}
			// End Part:
			// Part: 6 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SetLineWidth(50);
				theContent.SetLineJoin(0);
				theContent.Move(300, 500);
				theContent.Line(400, 700);
				theContent.Line(500, 500);
				theContent.Stroke();
			
				theContent.SetLineJoin(1);
				theContent.Move(300, 300);
				theContent.Line(400, 500);
				theContent.Line(500, 300);
				theContent.Stroke();
			
				theContent.SetLineJoin(2);
				theContent.Move(300, 100);
				theContent.Line(400, 300);
				theContent.Line(500, 100);
				theContent.Stroke();
				theContent.AddToDoc();
			
				doc.FontSize = 48;
				doc.Pos.String = "50 700";
				doc.AddText("0 - Miter");
				doc.Pos.String = "50 500";
				doc.AddText("1 - Round ");
				doc.Pos.String = "50 300";
				doc.AddText("2 - Bevel");
				doc.Width = 10;
				doc.Color.String = "255 255 255";
				doc.AddLine(300, 500, 400, 700);
				doc.AddLine(400, 700, 500, 500);
				doc.Rect.String = "390 690 410 710";
				doc.FillRect(10, 10);
				doc.AddLine(300, 300, 400, 500);
				doc.AddLine(400, 500, 500, 300);
				doc.Rect.String = "390 490 410 510";
				doc.FillRect(10, 10);
				doc.AddLine(300, 100, 400, 300);
				doc.AddLine(400, 300, 500, 100);
				doc.Rect.String = "390 290 410 310";
				doc.FillRect(10, 10);
				doc.Color.String = "0 0 0";
				doc.Save("adv_linejoin.pdf");
			}
			// End Part:
			// Part: 7 of 8
			using (var doc = new Doc()) {
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(20);
				theContent.LineDash("[ ] 0");
				theContent.Move(100, 650);
				theContent.Line(500, 650);
				theContent.Stroke();
			
				theContent.LineDash("[ 90 ] 0");
				theContent.Move(100, 500);
				theContent.Line(500, 500);
				theContent.Stroke();
			
				theContent.LineDash("[ 60 ] 30");
				theContent.Move(100, 350);
				theContent.Line(500, 350);
				theContent.Stroke();
			
				theContent.LineDash("[ 60 30 ] 0");
				theContent.Move(100, 200);
				theContent.Line(500, 200);
				theContent.Stroke();
				theContent.RestoreState();
			
				// annotate dashed lines
				doc.Color.String = "0 0 0";
				doc.FontSize = 36;
				doc.Pos.String = "50 710";
				doc.AddText("[ ] 0 - no dashes");
				doc.Pos.String = "50 560";
				doc.AddText("[ 90 ] 0 - 90 on, 90 off...");
				doc.Pos.String = "50 410";
				doc.AddText("[ 60 ] 30 - 30 on, 60 off, 60 on...");
				doc.Pos.String = "50 260";
				doc.AddText("[ 60 30 ] 0 - 60 on, 30 off, 60 on...");
			
				// add dashed lines
				theContent.AddToDoc();
				doc.Save("adv_dashes.pdf");
			}
			// End Part:
			// Part: 8 of 8
			using (var doc = new Doc()) {
				var star = new PDFContent(doc);
				star.Move(124, 108);
				star.Line(300, 650);
				star.Line(476, 108);
				star.Line(15, 443);
				star.Line(585, 443);
				star.Close();
				star.Stroke();
			
				var theContent = new PDFContent(doc);
				theContent.SaveState();
				theContent.SetLineWidth(30);
				theContent.SetLineJoin(2);
				theContent.AddContent(star);
				theContent.SetRGBStrokeColor(1, 0, 0);
				theContent.Transform(0.7, 0.7, -0.7, 0.7, 0, 0);
				theContent.AddContent(star);
				theContent.RestoreState();
				theContent.AddToDoc();
				doc.Save("adv_star_rotate.pdf");
			}
			// End Part:
		}
		// File End:

		// Fields, Markup and Movies Example
		//
		// File Start: False .\4-examples\18-annotations.htm
		public static void Ex4_examples_18_annotations() {
			// Part: 1 of 4
			using (var doc = new Doc()) {
				doc.Font = doc.AddFont("Helvetica");
				doc.FontSize = 36;
			
				var cat = doc.ObjectSoup.Catalog;
			
				var fileTree = new EmbeddedFileTree(doc);
				fileTree.EmbedFile("MyFile1", "../Rez/ABCpdf.swf", "attachment without annotation");
				doc.SetInfo(doc.Root, "/PageMode:Name", "UseAttachments");
			
				doc.Pos.X = 40;
				doc.Pos.Y = doc.MediaBox.Top - 40;
				doc.AddText("Interactive Form annotations");
			
				// Create interactive form
				var form = doc.Form;
				int fontID = doc.AddFont("Times-Roman", LanguageType.Latin);
				string fontName = form.AddResource(doc.ObjectSoup[fontID], "Font", "TimesRoman");
			
				// Radio buttons
				var radio = form.AddRadioButtonGroup(new XRect[] { new XRect("40 610 80 650"), new XRect("40 660 80 700") }, "RadioGroupField", 0);
				doc.Pos.String = "100 696";
				doc.AddText("RadioButton 1");
				doc.Pos.String = "100 646";
				doc.AddText("RadioButton 2");
			
				// Text fields
				var text = form.AddTextField(new XRect("40 530 300 580"), "TextField1", "Hello World!");
				var textE = new FieldElement(text);
				var textW = new WidgetAnnotationElement(text);
				textE.EntryDA = $"/{fontName} 36 Tf 0 0 1 rg";
				textW.EntryMK = new AppearanceCharacteristicsElement(textE);
				textW.EntryMK.EntryBC = [ 0.0, 0.0, 0.0 ];
				textW.EntryMK.EntryBG = [ 220.0 / 255.0, 220.0 / 255.0, 220.0 / 255.0 ];
				textE.EntryQ = 0; // Left alignment
			
				text = form.AddTextField(new XRect("40 460 300 510"), "TextField2", "Text Field");
				textE = new FieldElement(text);
				textW = new WidgetAnnotationElement(text);
				textW.EntryMK = new AppearanceCharacteristicsElement(textE);
				textW.EntryMK.EntryBC = [ 0.0, 0.0, 0.0 ];
				textE.EntryDA = $"/{fontName} 36 Tf 0 0 1 rg";
				textE.EntryQ = 0; // Left alignment
				textE.EntryFf |= (int)Field.FieldFlags.Password;
			
				text = form.AddTextField(new XRect("320 460 370 580"), "TextField3", "Vertical");
				textE = new FieldElement(text);
				textW = new WidgetAnnotationElement(text);
				textW.EntryMK = new AppearanceCharacteristicsElement(textE);
				textW.EntryMK.EntryBC = [ 0.0, 0.0, 0.0 ];
				textE.EntryDA = $"/{fontName} 36 Tf 0 0 0 rg";
				textW.EntryMK.EntryR = 90; // Rotation
			
				// Combobox field
				var combo = form.AddComboBoxField(new XRect("40 390 300 440"), "ComboBoxField");
				var comboE = new FieldElement(combo);
				comboE.EntryDA = $"/{fontName} 24 Tf 0 0 0 rg";
				combo.Options = [ "ComboBox Item 1", "ComboBox Item 2", "ComboBox Item 3" ];
			
				// Listbox field
				var listbox = form.AddListBoxField(new XRect("40 280 300 370"), "ListBoxField");
				var listboxE = new FieldElement(listbox);
				listboxE.EntryDA = $"/{fontName} 24 Tf 0 0 0 rg";
				listbox.Options = [ "ListBox Item 1", "ListBox Item 2", "ListBox Item 3" ];
			
				// Checkbox field
				form.AddCheckbox(new XRect("40 220 80 260"), "CheckBoxField", true);
				doc.Pos.String = "100 256";
				doc.AddText("Check Box");
			
				// Pushbutton field
				var button = form.AddButton(new XRect("40 160 200 200"), "ButtonField", "Button");
				var buttonW = new WidgetAnnotationElement(button);
				buttonW.EntryMK = new AppearanceCharacteristicsElement(buttonW);
				buttonW.EntryMK.EntryBC = [ 0.0, 0.0, 0.0 ];
				buttonW.EntryBS = new BorderStyleElement(buttonW);
				buttonW.EntryBS.EntryS = "B"; // beveled
			
				// Signature field
				var sig1 = form.AddSignature(new XRect("40 100 240 150"), "Signature1");
				doc.Save("annotations1.pdf");
			}
			// End Part:
			// Part: 2 of 4
			using (var doc = new Doc()) {
				//Markup annotations
				doc.Page = doc.AddPage();
				doc.Pos.X = 40;
				doc.Pos.Y = doc.MediaBox.Top - 40;
				doc.AddText("Markup annotations");
				var cat = doc.ObjectSoup.Catalog;
			
				var square = new SquareAnnotation(doc, new XRect("40 560 300 670"), XColor.FromRgb(255, 0, 0), XColor.FromRgb(0, 0, 255));
				square.SquareElement.EntryBS = new BorderStyleElement(square.SquareElement);
				square.SquareElement.EntryBS.EntryW = 8;
			
				var line = new LineAnnotation(doc, new XPoint("100 565"), new XPoint("220 665"), XColor.FromRgb(255, 0, 0));
				line.LineElement.EntryBS = new BorderStyleElement(line.LineElement);
				line.LineElement.EntryBS.EntryW = 12;
				line.RichTextCaption = "<span style= \"font-size:36pt; color:#FF0000\">Line</span>";
			
				doc.FontSize = 24;
				int fontID = doc.AddFont("Times-Roman", LanguageType.Latin);
				doc.Pos.String = "400 670";
				int id = doc.AddText("Underline");
				var markup = new TextMarkupAnnotation(doc, fontID, TextMarkupType.Underline, XColor.FromRgb(0, 255, 0));
			
				doc.Pos.String = "400 640";
				fontID = doc.AddText("Highlight");
				markup = new TextMarkupAnnotation(doc, fontID, TextMarkupType.Highlight, XColor.FromRgb(255, 255, 0));
			
				doc.Pos.String = "400 610";
				fontID = doc.AddText("StrikeOut");
				markup = new TextMarkupAnnotation(doc, fontID, TextMarkupType.StrikeOut, XColor.FromRgb(255, 0, 0));
			
				doc.Pos.String = "400 580";
				fontID = doc.AddText("Squiggly");
				markup = new TextMarkupAnnotation(doc, fontID, TextMarkupType.Squiggly, XColor.FromRgb(0, 0, 255));
			
				var circle = new CircleAnnotation(doc, new XRect("80 320 285 525"), XColor.FromRgb(255, 255, 0), XColor.FromRgb(255, 128, 0));
				circle.CircleElement.EntryBS = new BorderStyleElement(circle.CircleElement);
				circle.CircleElement.EntryBS.EntryW = 20;
				circle.CircleElement.EntryBS.EntryS = "D"; // dashed
				circle.CircleElement.EntryBS.EntryD = new ArrayElement<Element>(Atom.FromString("[3 2]"), cat);
			
				var arrowLine = new LineAnnotation(doc, new XPoint("385 330"), new XPoint("540 520"), XColor.FromRgb(255, 0, 0));
				arrowLine.LineEndingsStyle = "ClosedArrow ClosedArrow";
				arrowLine.LineElement.EntryBS = new BorderStyleElement(arrowLine.LineElement);
				arrowLine.LineElement.EntryBS.EntryW = 6;
				arrowLine.FillColor = XColor.FromRgb(255, 0, 0);
			
				var v1 = new double[] { 100, 70, 50, 120, 50, 220, 100, 270, 200, 270, 250, 220, 250, 120, 200, 70 };
				var polygon = new PolygonAnnotation(doc, v1, XColor.FromRgb(255, 0, 0), XColor.FromRgb(0, 255, 0));
				var v2 = new double[] { 400, 70, 350, 120, 350, 220, 400, 270, 500, 270, 550, 220, 550, 120, 500, 70 };
				var cloudyPolygon = new PolygonAnnotation(doc, v2, XColor.FromRgb(255, 0, 0), XColor.FromRgb(64, 85, 255));
				cloudyPolygon.CloudyEffect = 1;
				doc.Save("annotations2.pdf");
			}
			// End Part:
			// Part: 3 of 4
			using (var doc = new Doc()) {
				//Movie annotations
				//WMV is courtesy of NASA - http://www.nasa.gov/wmv/30873main_cardiovascular_300.wmv
				doc.Page = doc.AddPage();
				doc.Pos.X = 40;
				doc.Pos.Y = doc.MediaBox.Top - 40;
				doc.AddText("Multimedia features");
			
				doc.FontSize = 24;
			
				doc.Pos.String = "40 690";
				doc.AddText("Flash movie:");
				var movie1 = new ScreenAnnotation(doc, new XRect("40 420 300 650"), "../Rez/ABCpdf.swf");
			
				doc.Pos.String = "312 690";
				doc.AddText("Flash rich media:");
				var media1 = new RichMediaAnnotation(doc, new XRect("312 420 572 650"), "../Rez/ABCpdf.swf", "Flash");
				doc.Save("annotations3.pdf");
			}
			// End Part:
			// Part: 4 of 4
			using (var doc = new Doc()) {
				doc.Page = doc.AddPage();
				doc.FontSize = 36;
				doc.Pos.X = 40;
				doc.Pos.Y = doc.MediaBox.Top - 40;
				doc.AddText("Other types of annotations");
			
				//Sticky note annotation
				doc.FontSize = 24;
				doc.Pos.String = "40 680";
				doc.AddText("Text annotation");
				var textAnnotation = new TextAnnotation(doc, new XRect("340 660 360 680"), new XRect("550 650 600 750"), "6 sets of 13 pages. Trim to 5X7.");
			
				//File attachment annotation
				doc.Pos.String = "40 640";
				doc.AddText("File Attachment annotation");
				var fileAttachment = new FileAttachmentAnnotation(doc, new XRect("340 620 360 640"), "../Rez/video.WMV");
			
				//StampAnnotations
				doc.Pos.String = "40 600";
				doc.AddText("Stamp annotations");
				var stamp1 = new StampAnnotation(doc, new XRect("340 560 540 600"), "DRAFT", XColor.FromRgb(0, 0, 128));
				var stamp2 = new StampAnnotation(doc, new XRect("340 505 540 545"), "FINAL", XColor.FromRgb(0, 128, 0));
				var stamp3 = new StampAnnotation(doc, new XRect("340 450 540 490"), "NOT APPROVED", XColor.FromRgb(128, 0, 0));
				doc.Save("annotations4.pdf");
			}
			// End Part:
		}
		// File End:

		// PDF Rendering Example
		// 
		// This example shows how to render a PDF document.
		// 
		// For an example of how to render a PDF direct to screen and how to print a PDF see
		// the ABCpdfView project and classes under the ABCpdf menu item.
		//
		// File Start: False .\4-examples\19-rendering.htm
		public static void Ex4_examples_19_rendering() {
			// Part: 1 of 3
			using Doc doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			// End Part:
			// Part: 2 of 3
			doc.Rendering.DotsPerInch = 36;
			// End Part:
			// Part: 3 of 3
			for (int i = 1; i <= 4; i++) {
				doc.PageNumber = i;
				doc.Rect.String = doc.CropBox.String;
				doc.Rendering.Save("shuttle_p" + i.ToString() + ".png");
			}
			// End Part:
		}
		// File End:

		// System.Drawing Example
		//
		// File Start: False .\4-examples\20-systemdrawing.htm
		public static void Ex4_examples_20_systemdrawing() {
			// Part: 1 of 5
			//using WebSupergoo.ABCpdf14.Drawing;
			//using WebSupergoo.ABCpdf14.Drawing.Drawing2D;
			//using WebSupergoo.ABCpdf14.Drawing.Text;
			// End Part:
			// Part: 2 of 5
			//using System;
			//using System.IO;
			//using System.Reflection;
			
			//using WebSupergoo.ABCpdf14.Drawing;
			//using WebSupergoo.ABCpdf14.Drawing.Drawing2D;
			//using WebSupergoo.ABCpdf14.Drawing.Text;
			//using Rectangle = System.Drawing.Rectangle;
			//using RectangleF = System.Drawing.RectangleF;
			//using Point = System.Drawing.Point;
			//using PointF = System.Drawing.PointF;
			// End Part:
			// Part: 3 of 5
			// create a canvas for painting on
			var doc = new PDFDocument();
			var pg = doc.AddPage((int)(8.5 * 300), (int)(11 * 300));
			var gr = pg.Graphics;
			// End Part:
			// Part: 4 of 5
			// clear the canvas to white
			var pgRect = new Rectangle(0, 0, pg.Width, pg.Height);
			var solidWhite = new SolidBrush(Color.White);
			gr.FillRectangle(solidWhite, pgRect);
			// load a new image and draw it centered on our canvas
			using var stm = File.OpenRead("../mypics/pic1.jpg");
			using var img = Image.FromStream(stm);
			int w = img.Width;
			int h = img.Height;
			Rectangle rc = new Rectangle((pg.Width - w) / 2, (pg.Height - h) / 2, w, h);
			gr.DrawImage(img, rc);
			// frame the image with a black border
			gr.DrawRectangle(new Pen(Color.Black, 4), rc);
			// add some text at the top left of the canvas
			Font fn = new Font("Comic Sans MS", 300);
			var solidBlack = new SolidBrush(Color.Black);
			gr.DrawString("My Picture", fn, solidBlack, (int)(pg.Width * 0.1), (int)(pg.Height * 0.1));
			// End Part:
			// Part: 5 of 5
			// save the output
			doc.Save("abcpdf.drawing.pdf");
			// End Part:
		}
		// File End:

		// WPF Tables Example
		// 
		// This example shows how to import Windows Presentation Foundation (WPF) Extensible
		// Application Markup Language (XAML) tables into a PDF document. Each table has been
		// specified in a XAML file.
		// 
		// You can find the full project and classes under the ABCpdf menu item. The project
		// includes code for laying out four different types of tables. Two get their input
		// data from text files and two from XML files.
		// 
		// The tables are intentionally very similar to those in the Small Table Example and
		// Large Table Example to allow you to compare the two different layout methods.
		// 
		// <table border="0" cellpadding="10" class="backgrounder"> <b>WPF Limitations.</b>
		// The WPF Table component does not support the following features:
		// 
		// WPF supports row and column backgrounds and table and cell borders. However, it
		// does not support row or column borders.
		// 
		// WPF does not support automatic header and footer repetition when a table is split
		// across pages. Some people appear to have extended the DocumentPaginator class to
		// add headers manually in code during the XPS serialization process. However, our code
		// does not demonstrate this technique.
		// 
		// WPF does not directly support list data binding. By this, we mean that there is
		// no way to define a row template and then have the table automatically add a row for
		// each data item in the data provider. We achieve this functionality by manipulating
		// the XAML code in memory to add additional rows.
		//
		// File Start: False .\4-examples\21-wpftables.htm
		public static void Ex4_examples_21_wpftables() {
			// Part: 1 of 3
			#if NETFRAMEWORK // ignore
			MemoryStream ModifyXamlUsingTextProvider(string inDataProvider, string inXamlFile, string inTableXamlLocation, string inTableName) {
				var dataProvider = new TextDataProvider(inDataProvider);
				var xamlDoc = new XmlDocument();
				using var xamlFile = new FileStream(inXamlFile, FileMode.Open);
				xamlDoc.Load(xamlFile);
			
				var nsmgr = new XmlNamespaceManager(xamlDoc.NameTable);
				nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			
				var itemsTable = xamlDoc.DocumentElement.SelectSingleNode(inTableXamlLocation + "[@Name='" + inTableName + "']", nsmgr);
			
				for (int i = 1; i < dataProvider.Count; i++) {
					var rowGroup = itemsTable.LastChild;
					var newRowGroup = rowGroup.Clone();
					string bindingText = newRowGroup.Attributes["DataContext"].Value;
					bindingText = bindingText.Remove(bindingText.LastIndexOf('[')) + "[" + i + "] }";
					newRowGroup.Attributes["DataContext"].Value = bindingText;
					newRowGroup.Attributes["Background"].Value = (i % 2) == 0 ? "White" : "LightGray";
					itemsTable.InsertAfter(newRowGroup, rowGroup);
				}
			
				var memStream = new MemoryStream();
				xamlDoc.Save(memStream);
				return memStream;
			}
			// End Part:
			// Part: 2 of 3
			string dataProvider = null, xamlFile = null, tableXamlLocation = null, tableName = null;
			// ... user code to provide paths for these strings
			if (dataProvider == null || xamlFile == null || tableXamlLocation == null || tableName == null)
				return; // not provided
			using var stm = ModifyXamlUsingTextProvider(dataProvider, xamlFile, tableXamlLocation, tableName);
			var page = XamlReader.Load(stm) as System.Windows.Controls.Page;
			var docViewer = LogicalTreeHelper.FindLogicalNode(page, "DocViewer") as FlowDocumentPageViewer;
			page.Content = null;
			// End Part:
			// Part: 3 of 3
			void SaveToXps(Stream fileStream, FlowDocumentPageViewer viewer) {
				using var package = Package.Open(fileStream, FileMode.Create, FileAccess.ReadWrite);
				using var doc = new XpsDocument(package);
				var writer = XpsDocument.CreateXpsDocumentWriter(doc);
				var document = viewer.Document;
				writer.Write(document.DocumentPaginator);
			}
			
			void SaveToPdf(string pdfFileName, FlowDocumentPageViewer viewer) {
				using  var memStream = new MemoryStream();
				SaveToXps(memStream, viewer);
				using var pdfDoc = new Doc();
				pdfDoc.Read(memStream, new XReadOptions() { FileExtension = ".xps" });
				pdfDoc.Save(pdfFileName);
			}
			#endif // ignore
			// End Part:
		}
		// File End:

		// Example code for AddArc Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds an arc to a document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addarc.htm
		public static void Ex5_abcpdf_docaddarc() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 24;
			doc.Color.String = "120 0 0";
			doc.AddArc(0, 270, 300, 400, 200, 300);
			doc.Save("docaddarc.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddBookmark Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a sequence of pages with a nested sequence of bookmarks.
		// The image shows the appearance of the document outline. Note that none of the subject
		// pages are visible because the chapter pages were added in a collapsed state.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addbookmark.htm
		public static void Ex5_abcpdf_docaddbookmark() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 64;
			for (int i = 1; i < 4; i++) {
				doc.Page = doc.AddPage();
				string theSection = $"{i} Section";
				doc.AddText(theSection);
				doc.AddBookmark(theSection, true);
				for (int j = 1; j < 6; j++) {
					doc.Page = doc.AddPage();
					string theChapter = $"{theSection}\\{j} Chapter";
					doc.AddText(theChapter);
					doc.AddBookmark(theChapter, false);
					for (int k = 1; k < 7; k++) {
						doc.Page = doc.AddPage();
						string theSubject = $"{theChapter}\\{k} Subject";
						doc.AddText(theSubject);
						doc.AddBookmark(theSubject, true);
					}
				}
			}
			doc.Save("docaddbookmark.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddColorSpaceFile Function of Doc Class for ABCpdf .NET
		// 
		// In this example we add some CMYK text defined in an ICC based color space.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addcolorspacefile.htm
		public static void Ex5_abcpdf_docaddcolorspacefile() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.FontSize = 96;
			string path = "../mypics/cmyk.icc";
			doc.ColorSpace = doc.AddColorSpaceFile(path);
			doc.Color.String = "200 20 20 20";
			doc.AddText(text);
			doc.Save("docaddcolorspacefile.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddColorSpaceSpot Function of Doc Class for ABCpdf .NET
		// 
		// In this example we define a colorant called Gold and add some text using varying
		// amounts of our colorant.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addcolorspacespot.htm
		public static void Ex5_abcpdf_docaddcolorspacespot() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(20, 20);
			doc.FontSize = 300;
			doc.ColorSpace = doc.AddColorSpaceSpot("GOLD", "0 0 100 0");
			for (int i = 1; i <= 10; i++) {
				doc.Color.Gray = 255 / i;
				doc.AddText(doc.Color.Gray.ToString());
				doc.Rect.Move(25, -50);
			}
			doc.Save("docaddcolorspacespot.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddFont Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds two pieces of text to a document. The first piece is in Times-
		// Roman and the second in Helvetica-Bold.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addfont.htm
		public static void Ex5_abcpdf_docaddfont() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 48;
			string font = "Times-Roman ";
			doc.Font = doc.AddFont(font);
			doc.AddText(font);
			font = "Helvetica-Bold";
			doc.Font = doc.AddFont(font);
			doc.AddText(font);
			doc.Save("docaddfont.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddGrid Function of Doc Class for ABCpdf .NET
		// 
		// The following code modifies the page transform and then adds a grid to show how the
		// transform has affected the page.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addgrid.htm
		public static void Ex5_abcpdf_docaddgrid() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Page = doc.AddPage();
			doc.Transform.Rotate(20, 100, 100);
			doc.AddGrid();
			doc.Save("docaddgrid.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageBitmap Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a transparent PNG to a document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimagebitmap.htm
		public static void Ex5_abcpdf_docaddimagebitmap() {
			// Part: 1 of 1
			using var doc = new Doc();
			string path = "../mypics/mypic.png";
			using var bm = new Bitmap(path);
			doc.Rect.Inset(20, 20);
			doc.Color.String = "0 0 200";
			doc.FillRect();
			doc.AddImageBitmap(bm, true);
			doc.Save("docaddimagebitmap.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageCopy Function of Doc Class for ABCpdf .NET
		// 
		// This example shows how to read an existing PDF document and insert a background image
		// into every page.
		// 
		// We start by reading our template PDF document and finding out core information we
		// will need to reference each page.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimagecopy.htm
		public static void Ex5_abcpdf_docaddimagecopy() {
			// Part: 1 of 2
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			int count = doc.PageCount;
			// End Part:
			// Part: 2 of 2
			int id = 0;
			for (int i = 1; i <= count; i++) {
				doc.PageNumber = i;
				doc.Layer = doc.LayerCount + 1;
				if (i == 1) {
					string path = "../mypics/light.jpg";
					id = doc.AddImageFile(path, 1);
				}
				else
					doc.AddImageCopy(id);
			}
			doc.Save("watermark.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageDoc Function of Doc Class for ABCpdf .NET
		// 
		// This example shows how to draw one PDF into another. It takes a PDF document and
		// creates a 'four-up' summary document by drawing four pages on each page of the new
		// document.
		// 
		//  First we create an ABCpdf Doc object and read in our source document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimagedoc.htm
		public static void Ex5_abcpdf_docaddimagedoc() {
			// Part: 1 of 4
			using var src = new Doc();
			src.Read("../Rez/spaceshuttle.pdf");
			int count = src.PageCount;
			// End Part:
			// Part: 2 of 4
			using var dst = new Doc();
			dst.MediaBox.String = src.MediaBox.String;
			dst.Rect.String = dst.MediaBox.String;
			dst.Rect.Magnify(0.5, 0.5);
			dst.Rect.Inset(10, 10);
			double x = dst.MediaBox.Width / 2;
			double y = dst.MediaBox.Height / 2;
			// End Part:
			// Part: 3 of 4
			for (int i = 1; i <= count; i++) {
				switch (i % 4) {
					case 1:
						dst.Page = dst.AddPage();
						dst.Rect.Position(10, y + 10);
						break;
					case 2:
						dst.Rect.Position(x + 10, y + 10);
						break;
					case 3:
						dst.Rect.Position(10, 10);
						break;
					case 0:
						dst.Rect.Position(x + 10, 10);
						break;
				}
				dst.AddImageDoc(src, i, null);
				dst.FrameRect();
			}
			// End Part:
			// Part: 4 of 4
			dst.Save("fourup.pdf");
			// finished
			// End Part:
		}
		// File End:

		// Example code for AddImageFile Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds an image to the current page positioned at the bottom left.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimagefile.htm
		public static void Ex5_abcpdf_docaddimagefile() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.String = "0 0 510 638";
			string path = "../mypics/pic.jpg";
			doc.AddImageFile(path, 1);
			doc.Save("docaddimage.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageObject 
		// Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a transparent GIF against a gray background.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimageobject.htm
		public static void Ex5_abcpdf_docaddimageobject() {
			// Part: 1 of 1
			using var img = new XImage();
			img.SetFile("../mypics/mypic.gif");
			using Doc doc = new Doc();
			doc.Color.String = "200 200 200";
			doc.FillRect();
			doc.Rect.String = "0 0 480 640";
			doc.AddImageObject(img, true);
			doc.Save("docaddimageobject.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageToChain Function of Doc Class for ABCpdf .NET
		// 
		// This example shows how to import an HTML page into a multi-page PDF document.
		// 
		// We first create a Doc object and inset the edges a little so that the HTML will
		// appear in the middle of the page.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimagetochain.htm
		public static void Ex5_abcpdf_docaddimagetochain() {
			// Part: 1 of 4
			using var doc = new Doc();
			doc.Rect.Inset(72, 144);
			// End Part:
			// Part: 2 of 4
			int id = doc.AddImageUrl("http://www.yahoo.com/");
			// End Part:
			// Part: 3 of 4
			while (true) {
				doc.FrameRect();
				if (!doc.Chainable(id))
					break;
				doc.Page = doc.AddPage();
				id = doc.AddImageToChain(id);
			}
			// End Part:
			// Part: 4 of 4
			doc.Save("paged_html.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddImageUrl Function of Doc Class for ABCpdf .NET
		// 
		// We create an ABCpdf Doc object, add our URL and save. That's it!
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addimageurl.htm
		public static void Ex5_abcpdf_docaddimageurl() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.AddImageUrl("http://www.google.com/");
			doc.Save("htmlimport.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddLine Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds two horizontal lines to a document. The first is blue and
		// the second is green.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addline.htm
		public static void Ex5_abcpdf_docaddline() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 24;
			doc.Color.String = "0 0 255";
			doc.AddLine(-50, 100, 999, 100);
			doc.Color.String = "0 255 0";
			doc.AddLine(-50, 400, 999, 400);
			doc.Save("docaddline.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddObject Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a document information section to an existing PDF document.
		// First it adds an empty dictionary and references it from the document trailer. Then
		// it adds an Author, Title and Subject before saving.
		// 
		// There are multiple places that metadata can be put into a PDF. The most commonly
		// used are the Info entry of the Trailer and the Metadata entry of the Catalog. The
		// Info entry is the older and most widely recognized location. The Metadata entry is
		// a more recent XML based store. It is important that information within these stores
		// is consistent. If the information is inconsistent then you'll find that the metadata
		// reported by different applications is different.
		// 
		// To try and reduce the amount of confusion caused by multiple metadata entries, PDF
		// 2.0 deprecated all Info entries apart from the CreationDate and ModDate. To be PDF
		// 2.0 compliant you should put all metadata into the Metadata entry rather than the
		// Info one. The code example below will ensure backwards compatibility with legacy
		// applications, but in general for metadata, you should be using the kind of code you
		// see in the Catalog.Metadata example.
		// 
		// In this example, to ensure that the data is consistent we're going to delete any
		// XML Metadata entry that may be present. That way we force applications to report
		// the Info store. However it wouldn't be a difficult matter to load up any XML in the
		// Metadata entry and modify that as well as the Info entry. For details see the Catalog.
		// Metadata example.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addobject.htm
		public static void Ex5_abcpdf_docaddobject() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			if (doc.GetInfo(-1, "/Info") == "")
				doc.SetInfo(-1, "/Info:Ref", doc.AddObject("<< >>").ToString());
			doc.SetInfo(-1, "/Info*/Author:Text", "Arthur Dent");
			doc.SetInfo(-1, "/Info*/Title:Text", "Musings on Life");
			doc.SetInfo(-1, "/Info*/Subject:Text", "Philosophy");
			doc.SetInfo(doc.Root, "/Metadata:Del", "");
			doc.Save("docaddobject.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddOval Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds two ovals to a document. The outline oval is semi-transparent.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addoval.htm
		public static void Ex5_abcpdf_docaddoval() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Rect.Inset(50, 50);
			doc.Color.String = "255 0 0";
			doc.AddOval(true);
			doc.Color.String = "0 255 0 128";
			doc.AddOval(false);
			doc.Save("docaddoval.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddPage Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds three pages to a document. Each page is marked with the page
		// number and page Object ID.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addpage.htm
		public static void Ex5_abcpdf_docaddpage() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96; // big text
			doc.TextStyle.HPos = 0.5; // centered
			doc.TextStyle.VPos = 0.5; // ...
			for (int i = 1; i <= 3; i++) {
				doc.Page = doc.AddPage();
				string txt = $"Page {i}, ID={doc.Page}";
				doc.AddText(txt);
			}
			doc.Save("docaddpage.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddPie Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds two pie slices to a document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addpie.htm
		public static void Ex5_abcpdf_docaddpie() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Rect.Inset(50, 50);
			doc.Color.String = "255 0 0";
			doc.AddPie(0, 90, true);
			doc.Color.String = "0 255 0";
			doc.AddPie(180, 270, false);
			doc.Save("docaddpie.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddPoly Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a transparent green outlined star over the top of a red filled
		// star.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addpoly.htm
		public static void Ex5_abcpdf_docaddpoly() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Color.String = "255 0 0";
			doc.AddPoly("124 158 300 700 476 158 15 493 585 493 124 158", true);
			doc.Color.String = "0 255 0 a128";
			doc.AddPoly("124 158 300 700 476 158 15 493 585 493 124 158", false);
			doc.Save("docaddpoly.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddText Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a number of chunks of text to a document. Each chunk is in
		// a different style. This sample makes use of the fact that the Pos is updated to point
		// to the next text insertion point after adding a piece of text. However note that
		// when inserting muti-styled text it is generally more efficient to use the AddTextStyled
		// method.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addtext.htm
		public static void Ex5_abcpdf_docaddtext() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Page = doc.AddPage();
			doc.FontSize = 48;
			int font1 = doc.AddFont("Times-Roman");
			int font2 = doc.AddFont("Times-Bold");
			doc.Font = font1;
			doc.AddText("Gallia est omnis ");
			doc.Font = font2;
			doc.AddText("tertiam Galli appellantur ");
			doc.Font = font1;
			doc.AddText("divisa in partes tres, ");
			doc.Font = font2;
			doc.AddText("quarum unam incolunt ");
			doc.Font = font1;
			doc.AddText("Belgae, aliam Aquitani. ");
			doc.Font = font2;
			doc.AddText("tertiam Galli appellantur");
			doc.Save("docaddtext.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddTextStyled Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds some styled text to a document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addtextstyled.htm
		public static void Ex5_abcpdf_docaddtextstyled() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 72;
			doc.AddTextStyled("<b>Gallia</b> est omnis divisa in partes tres, quarum unam incolunt <b>Belgae</b>, aliam <b>Aquitani</b>, tertiam qui ipsorum lingua <b>Celtae</b>, nostra <b>Galli</b> appellantur.");
			doc.Save("docaddhtml.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddXObject Function of Doc Class for ABCpdf .NET
		// 
		// This example shows how to load an image into a PixMap and then draw it on the current
		// page using the AddXObject method.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\addxobject.htm
		public static void Ex5_abcpdf_docaddxobject() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(50, 50);
			doc.Transform.Rotate(20, 200, 200);
			doc.Color.SetRgb(200, 200, 255);
			doc.FillRect();
			var pm = new PixMap(doc.ObjectSoup);
			using var img = (Bitmap)Bitmap.FromFile("../mypics/mypic.png");
			pm.SetBitmap(img, true);
			doc.AddXObject(pm);
			doc.Save("examplePixMapBitmap.pdf");
			// End Part:
		}
		// File End:

		// Example code for Append Function of Doc Class for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might join two PDF documents together.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\append.htm
		public static void Ex5_abcpdf_docappend() {
			// Part: 1 of 1
			using var doc1 = new Doc();
			doc1.FontSize = 192;
			doc1.TextStyle.HPos = 0.5;
			doc1.TextStyle.VPos = 0.5;
			doc1.AddText("Hello");
			using var doc2 = new Doc();
			doc2.FontSize = 192;
			doc2.TextStyle.HPos = 0.5;
			doc2.TextStyle.VPos = 0.5;
			doc2.AddText("World");
			doc1.Append(doc2);
			doc1.Save("docjoin.pdf");
			// End Part:
		}
		// File End:

		// Example code for Delete Function of Doc Class for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might add an image and then delete
		// it if the image color space is CMYK.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\delete.htm
		public static void Ex5_abcpdf_docdelete() {
			// Part: 1 of 1
			using var doc = new Doc();
			string path = "../mypics/mypic.jpg";
			int id1 = doc.AddImageFile(path, 1);
			int id2 = doc.GetInfoInt(id1, "XObject");
			int comps = doc.GetInfoInt(id2, "Components");
			if (comps == 4) doc.Delete(id1);
			doc.Save("docdelete.pdf");
			// End Part:
		}
		// File End:

		// Example code for EmbedFont Function of Doc Class for ABCpdf .NET
		// 
		// The following code embeds the font 'Comic Sans MS' into the document and then adds
		// some text.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\embedfont.htm
		public static void Ex5_abcpdf_docembedfont() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 216;
			string font = "Comic Sans MS";
			doc.Font = doc.EmbedFont(font);
			doc.AddText(font);
			doc.Save("docembedfont.pdf");
			// End Part:
		}
		// File End:

		// Example code for FillRect Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a blue filled rectangle to a document. The frame is inset
		// from the edges of the document by 200 points horizontally and 100 points vertically.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\fillrect.htm
		public static void Ex5_abcpdf_docfillrect() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(200, 100);
			doc.Color.Blue = 255;
			doc.FillRect();
			doc.Save("docfillrect.pdf");
			// End Part:
		}
		// File End:

		// Example code for FrameRect Function of Doc Class for ABCpdf .NET
		// 
		// The following code adds a black frame to a document. The frame is inset from the
		// edges of the document by 50 points horizontally and 100 points vertically.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\framerect.htm
		public static void Ex5_abcpdf_docframerect() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(50, 100);
			doc.FrameRect();
			doc.Save("docframerect.pdf");
			// End Part:
		}
		// File End:

		// Example code for GetInfo Function of Doc Class for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might report the natural dimensions
		// of an image.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\getinfo.htm
		public static void Ex5_abcpdf_docgetinfo() {
			// Part: 1 of 1
			using var doc = new Doc();
			string path = "../mypics/mypic.jpg";
			int id1 = doc.AddImageFile(path, 1);
			int id2 = doc.GetInfoInt(id1, "XObject");
			string theWidth = doc.GetInfo(id2, "Width");
			string theHeight = doc.GetInfo(id2, "Height");
			Response.Write($"Width {theWidth}< br>");
			Response.Write($"Height {theHeight}< br>");
			// End Part:
		}
		// File End:

		// Example code for Read Function of Doc Class for ABCpdf .NET
		// 
		// The following illustrates how one might add a large red number to every page of a
		// PDF document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\read.htm
		public static void Ex5_abcpdf_docread() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			doc.FontSize = 500;
			doc.Color.String = "255 0 0";
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.3;
			int count = doc.PageCount;
			for (int i = 1; i <= count; i++) {
				doc.PageNumber = i;
				doc.AddText(i.ToString());
			}
			doc.Save("docread.pdf");
			// End Part:
		}
		// File End:

		// Example code for RemapPages Method of Doc Class for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might reverse all the pages in a document.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\remappages.htm
		public static void Ex5_abcpdf_docremappages() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			doc.FontSize = 500;
			doc.Color.String = "255 0 0";
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.3;
			int count = doc.PageCount;
			var pages = new List<int>();
			for (int i = 1; i <= count; i++) {
				doc.PageNumber = i;
				doc.AddText(i.ToString());
				pages.Add(count - i + 1);
			}
			doc.RemapPages(pages.ToArray());
			doc.Save("docremappages.pdf");
			// End Part:
		}
		// File End:

		// Example code for Save Function of Doc Class for ABCpdf .NET
		// 
		// The following code illustrates how one might add text to a PDF and then save it out.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\save.htm
		public static void Ex5_abcpdf_docsave() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.AddText("Hello World");
			doc.Save("docsave.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetInfo Function of Doc Class for ABCpdf .NET
		// 
		// The following shows how to modify the document catalog to ensure that the PDF opens
		// onto the second page rather than the first.
		//
		// File Start: True .\5-abcpdf\doc\1-methods\setinfo.htm
		public static void Ex5_abcpdf_docsetinfo() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			int pages = doc.GetInfoInt(doc.Root, "Pages");
			int page2 = doc.GetInfoInt(pages, "Page 2");
			string action = $"[ {page2} 0 R /Fit ]";
			doc.SetInfo(doc.Root, "/OpenAction", action);
			doc.Save("docsetinfo.pdf");
			// End Part:
		}
		// File End:

		// Example code for Color Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document with a red background.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\color.htm
		public static void Ex5_abcpdf_doccolor() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Color.String = "255 0 0";
			doc.FillRect();
			doc.Save("doccolor.pdf");
			// End Part:
		}
		// File End:

		// Example code for ColorSpace Property of Doc Class for ABCpdf .NET
		// 
		// The following code shows how to colorize an image. It adds a base image to the current
		// page and converts it to grayscale. Then it creates a new spot color space and assigns
		// the new color space to the image.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\colorspace.htm
		public static void Ex5_abcpdf_doccolorspace() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(20, 20);
			
			using var img = new XImage();
			img.SetFile("../mypics/mypic.jpg");
			int id = doc.AddImageObject(img, false);
			
			id = doc.GetInfoInt(id, "XObject");
			doc.SetInfo(id, "Grayscale", "");
			
			int theCS = doc.AddColorSpaceSpot("MAGENTA", "0 100 0 0");
			doc.SetInfo(id, "/ColorSpace:Ref", theCS.ToString());
			doc.SetInfo(id, "/Decode", "[1 0]");
			
			doc.Save("doccolorspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for Encryption Property of Doc Class for ABCpdf .NET
		// 
		// The following code saves a simple PDF document using a 128 bit encryption key. It
		// applies a copy-protection permission to stop people copying text out of the document.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\encryption.htm
		public static void Ex5_abcpdf_docencryption() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.AddText("Hello World!");
			doc.Encryption.Type = 5;
			doc.Encryption.SetCryptMethods(CryptMethodType.AESV3);
			doc.Encryption.CanCopy = false;
			doc.Encryption.OwnerPassword = "owner";
			doc.Save("docencrypt.pdf");
			// End Part:
		}
		// File End:

		// Example code for Font Property of Doc Class for ABCpdf .NET
		// 
		// The following example adds two blocks of styled text to a document. The first block
		// is in Helvetica and the second in Courier.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\font.htm
		public static void Ex5_abcpdf_docfont() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96; // big text
			doc.Font = doc.AddFont("Helvetica");
			doc.AddText("Helvetica Text.");
			doc.Font = doc.AddFont("Courier");
			doc.AddText("Courier Text.");
			doc.Save("docfont.pdf");
			// End Part:
		}
		// File End:

		// Example code for FontSize Property of Doc Class for ABCpdf .NET
		// 
		// The following example adds two blocks of styled text to a document. The first block
		// is in 96 point type and the second is in 192 point type.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\fontsize.htm
		public static void Ex5_abcpdf_docfontsize() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.AddText("Small ");
			doc.FontSize = 192;
			doc.AddText("Big");
			doc.Save("docfontsize.pdf");
			// End Part:
		}
		// File End:

		// Example code for MediaBox Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document containing three different pages each with
		// a different size.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\mediabox.htm
		public static void Ex5_abcpdf_docmediabox() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Page = doc.AddPage();
			doc.MediaBox.String = "A4";
			doc.Page = doc.AddPage();
			doc.MediaBox.String = "B5";
			doc.Page = doc.AddPage();
			doc.Save("docmediabox.pdf");
			// End Part:
		}
		// File End:

		// Example code for Options Property of Doc Class for ABCpdf .NET
		// 
		// The following code adds an arc to a document. It uses the options parameter to make
		// the line dashed rather than solid.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\options.htm
		public static void Ex5_abcpdf_docoptions() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 24;
			doc.Color.String = "0 120 0";
			doc.Options = "[6 10] 6 d";
			doc.AddArc(0, 270, 300, 400, 200, 300);
			doc.Save("docoptions.pdf");
			// End Part:
		}
		// File End:

		// Example code for Page Property of Doc Class for ABCpdf .NET
		// 
		// The following example creates a document with two pages and adds text to each of
		// the pages in turn.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\page.htm
		public static void Ex5_abcpdf_docpage() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96; // big text
			doc.Page = doc.AddPage();
			doc.AddText("Page One");
			doc.Page = doc.AddPage();
			doc.AddText("Page Two");
			doc.Save("docpage.pdf");
			// End Part:
		}
		// File End:

		// Example code for Pos Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document with text positioned at a number of different
		// points within it.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\pos.htm
		public static void Ex5_abcpdf_docpos() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 48;
			for (int i = 1; i <= 8; i++) {
				doc.Pos.X = i * 40;
				doc.Pos.Y = i * 80;
				doc.AddText($"Pos = {doc.Pos}");
			}
			doc.Save("docpos.pdf");
			// End Part:
		}
		// File End:

		// Example code for Rect Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document containing a number of concentric frames.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\rect.htm
		public static void Ex5_abcpdf_docrect() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.String = "50 50 550 550";
			for (int i = 1; i <= 20; i++) {
				doc.FrameRect();
				doc.Rect.Inset(20, 20);
			}
			doc.Save("docrect.pdf");
			// End Part:
		}
		// File End:

		// Example code for Root Property of Doc Class for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might find some information about
		// a PDF document.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\root.htm
		public static void Ex5_abcpdf_docroot() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/mydoc.pdf");
			string vers = doc.GetInfo(doc.Root, "Version");
			string names = doc.GetInfo(doc.Root, "/Names");
			string pages = doc.GetInfo(doc.Root, "pages");
			string outlines = doc.GetInfo(doc.Root, "outlines");
			Response.Write($"Version {vers}&lt;br&gt;");
			Response.Write($"Names {names}&lt;br&gt;");
			Response.Write($"Pages ID {pages}&lt;br&gt;");
			Response.Write($"Outlines ID {outlines}&lt;br&gt;");
			// End Part:
		}
		// File End:

		// Example code for String Property of Doc Class for ABCpdf .NET
		// 
		// In this example we show how to use the String property to implement a graphics state
		// stack with Push and Pop operators.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\string.htm
		public static void Ex5_abcpdf_docstring() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 64;
			doc.Rect.Inset(20, 20);
			doc.Font = doc.AddFont("Helvetica");
			var state = new Stack<string>();
			state.Push(doc.String);
			doc.AddText("Black Helvetica\r\n\r\n");
			doc.Color.SetRgb(255, 0, 0);
			doc.Font = doc.AddFont("Helvetica-Oblique");
			doc.AddText("Red Helvetica-Oblique\r\n\r\n");
			doc.String = state.Pop();
			doc.AddText("Black Helvetica again\r\n\r\n");
			doc.Save("savestate.pdf");
			// End Part:
		}
		// File End:

		// Example code for TextStyle Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document and adds some text using a number of the
		// text style properties to control formatting.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\textstyle.htm
		public static void Ex5_abcpdf_doctextstyle() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt. Gallos ab Aquitanis Garumna flumen, a Belgis Matrona et Sequana dividit.";
			text = text + "\r\n" + text + "\r\n" + text + "\r\n";
			doc.Rect.Inset(20, 20);
			doc.TextStyle.Size = 32;
			doc.TextStyle.Justification = 1;
			doc.TextStyle.Indent = 64;
			doc.TextStyle.ParaSpacing = 32;
			doc.AddText(text);
			doc.Save("doctextstyle.pdf");
			// End Part:
		}
		// File End:

		// Example code for TopDown Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document and adds a grid measured in inches.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\topdown.htm
		public static void Ex5_abcpdf_doctopdown() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Units = UnitType.Inches;
			doc.TopDown = true;
			doc.Width = 1.0 / 8.0;
			doc.FontSize = 1;
			doc.Rect.Pin = XRect.Corner.TopLeft;
			for (int i = 0; i <= 12; i += 2) {
				doc.AddLine(0, i, 12, i);
				doc.Rect.Position(0, i);
				doc.AddText(i.ToString());
				doc.AddLine(i, 0, i, 12);
				doc.Rect.Position(i, 0);
				doc.AddText(i.ToString());
			}
			doc.Save("doctopdown.pdf");
			// End Part:
		}
		// File End:

		// Example code for Transform Property of Doc Class for ABCpdf .NET
		// 
		// The following code creates a PDF document and adds some text and a rectangle rotated
		// at 45 degrees anti-clockwise around the middle of the document.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\transform.htm
		public static void Ex5_abcpdf_doctransform() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt. Gallos ab Aquitanis Garumna flumen, a Belgis Matrona et Sequana dividit.";
			text = text + "\r\n" + text + "\r\n" + text + "\r\n";
			doc.Rect.Magnify(0.5, 0.5);
			doc.Rect.Position(151, 198);
			doc.FrameRect();
			doc.Transform.Rotate(45, 302, 396);
			doc.FrameRect();
			doc.FontSize = 24;
			doc.AddText(text);
			doc.Save("doctransform.pdf");
			// End Part:
		}
		// File End:

		// Example code for Width Property of Doc Class for ABCpdf .NET
		// 
		// The following code adds two lines to a document. The first line has a width of ten
		// points and the second has a width of twenty points.
		//
		// File Start: True .\5-abcpdf\doc\2-properties\width.htm
		public static void Ex5_abcpdf_docwidth() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 10;
			doc.AddLine(10, 10, 300, 300);
			doc.Width = 20;
			doc.AddLine(10, 300, 300, 10);
			doc.Save("docwidth.pdf");
			// End Part:
		}
		// File End:

		// Example code for Alpha Property of XColor Class for ABCpdf .NET
		// 
		// Here we create a PDF document showing how different values of alpha result in different
		// levels of transparency.
		//
		// File Start: True .\5-abcpdf\xcolor\2-properties\alpha.htm
		public static void Ex5_abcpdf_xcoloralpha() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(20, 20);
			doc.FontSize = 300;
			for (int i = 1; i <= 10; i++) {
				doc.Color.Alpha = 255 / i;
				doc.AddText(doc.Color.Alpha.ToString());
				doc.Rect.Move(25, -50);
			}
			doc.Save("coloralpha.pdf");
			// End Part:
		}
		// File End:

		// Example code for Components Property of XColor Class for ABCpdf .NET
		// 
		// In the following example we demonstrate how to use generic color components to draw
		// in the Lab color space.
		//
		// File Start: True .\5-abcpdf\xcolor\2-properties\components.htm
		public static void Ex5_abcpdf_xcolorcomponents() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Rect.Inset(50, 50);
			var cs = new ColorSpace(doc.ObjectSoup, ColorSpaceType.Lab);
			doc.ColorSpace = cs.ID;
			// This Lab color is a deep green
			doc.Color.ColorSpace = ColorOperatorType.ColorSpace;
			doc.Color.Components[0] = 50; // L range is 0 to +100
			doc.Color.Components[1] = -50; // a range is -100 to +100
			doc.Color.Components[2] = +50; // B range is -100 to +100
			doc.AddOval(true);
			doc.Save("examplelabcolorspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetCryptMethods Function of XEncryption Object for ABCpdf .NET
		// 
		// Here we use 128-bit AES encryption.
		//
		// File Start: True .\5-abcpdf\xencryption\1-methods\setcryptmethods.htm
		public static void Ex5_abcpdf_xencryptionsetcryptmethods() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.AddText("Hello World!");
			doc.Encryption.Type = 5;
			doc.Encryption.SetCryptMethods(CryptMethodType.AESV3);
			doc.Save("setcryptmethods.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddDocTimestamp Function of XForm Object for ABCpdf .NET
		// 
		// See the Annotations example project for a full example.
		// 
		//  However the following is a code snippet showing how this might be used.
		//
		// File Start: True .\5-abcpdf\xform\1-methods\adddoctimestamp.htm
		public static void Ex5_abcpdf_xformadddoctimestamp() {
			// Part: 1 of 1
			using var doc = new Doc();
			// ... set up the doc perhaps by reading a PDF
			if (doc.PageCount == 0)
				doc.Page = doc.AddPage();
			var sig = doc.Form.AddDocTimestamp("Timestamp");
			var uri = new Uri("http://timestamp.digicert.com");
			var oid = new Oid(CryptoConfig.MapNameToOID("SHA256"));
			sig.TimestampServiceUrl = uri;
			sig.Timestamp(oid, 0);
			// End Part:
		}
		// File End:

		// Example code for GetTagRects Function of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\1-methods\gettagrects.htm
		public static void Ex5_abcpdf_xhtmloptionsgettagrects() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(100, 100);
			doc.Rect.Top = 700;
			doc.HtmlOptions.Engine = EngineType.Chrome146;
			doc.HtmlOptions.AddTags = true;
			// The ABCGecko and MSHTML tagging format uses styles.
			string html1 = "<!DOCTYPE html><html><head>" +
				"<style type='text/css'>" +
				".tag-visible { abcpdf-tag-visible:true; outline: 1px solid transparent; font-size: 72pt; }" +
				"</style>" +
				"</head><body>" +
				"<p id='p1' class='tag-visible'>Gallia est omnis divisa in partes tres.</p>" +
				"</body></html>";
			// The ABCChrome tagging format uses attributes.
			string html2 = "<!DOCTYPE html><html><head>" +
				"<style type='text/css'>" +
				"p { font-size: 72pt; }" +
				"</style>" +
				"</head><body>" +
				"<p id='p1' abcpdf-tag-visible>Gallia est omnis divisa in partes tres.</p>" +
				"</body></html>";
			string html = doc.HtmlOptions.Engine != EngineType.Chrome146 ? html1 : html2;
			int id = doc.AddImageHtml(html);
			// Frame location of the tagged element
			var tagRects = doc.HtmlOptions.GetTagRects(id);
			foreach (var rect in tagRects) {
				doc.Rect.String = rect.ToString();
				doc.FrameRect();
			}
			// Output tag ID
			var tagIds = doc.HtmlOptions.GetTagIDs(id);
			doc.Rect.String = doc.MediaBox.String;
			doc.Rect.Inset(20, 20);
			doc.FontSize = 64;
			doc.Color.String = "255 0 0";
			doc.AddText($"Tag ID \"{tagIds[0]}\":");
			// Save the document
			doc.Save("HtmlOptionsGetTagRects.pdf");
			// End Part:
		}
		// File End:

		// Example code for LinkDestinations Method of XHtmlOptions Object for ABCpdf .NET
		// 
		// This example shows how to import an HTML page which uses named destinations.
		// 
		// We first create a Doc object and inset the edges a little so that the HTML will
		// appear in the middle of the page. We assign the appropriate HTML options so that
		// links will be rendered live.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\1-methods\linkdestinations.htm
		public static void Ex5_abcpdf_xhtmloptionslinkdestinations() {
			// Part: 1 of 4
			using var doc = new Doc();
			doc.Rect.Inset(18, 18);
			doc.HtmlOptions.AddLinks = true;
			// End Part:
			// Part: 2 of 4
			var theList = new List<int>();
			int id = doc.AddImageUrl("http://www.websupergoo.com/support.htm");
			while (true) {
				theList.Add(id);
				if (!doc.Chainable(id))
					break;
				doc.Page = doc.AddPage();
				id = doc.AddImageToChain(id);
			}
			// End Part:
			// Part: 3 of 4
			doc.HtmlOptions.LinkDestinations(theList);
			// End Part:
			// Part: 4 of 4
			doc.Save("linkdestinations.pdf");
			// End Part:
		}
		// File End:

		// Example code for LinkPages Method of XHtmlOptions Object for ABCpdf .NET
		// 
		// This example shows how to import an HTML page which uses named destinations.
		// 
		// We first create a Doc object and inset the edges a little so that the HTML will
		// appear in the middle of the page. We assign the appropriate HTML options so that
		// links will be rendered live.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\1-methods\linkpages.htm
		public static void Ex5_abcpdf_xhtmloptionslinkpages() {
			// Part: 1 of 4
			using var doc = new Doc();
			doc.Rect.Inset(18, 18);
			doc.HtmlOptions.AddLinks = true;
			// End Part:
			// Part: 2 of 4
			int id = doc.AddImageUrl("http://www.websupergoo.com/support.htm");
			while (true) {
				if (!doc.Chainable(id))
					break;
				doc.Page = doc.AddPage();
				id = doc.AddImageToChain(id);
			}
			// End Part:
			// Part: 3 of 4
			doc.HtmlOptions.LinkPages();
			// End Part:
			// Part: 4 of 4
			doc.Save("linkpages.pdf");
			// End Part:
		}
		// File End:

		// Example code for ForChrome Property of XHtmlOptions Object for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\2-forchrome.htm
		public static void Ex5_abcpdf_xhtmloptions2_forchrome() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.HtmlOptions.Engine = EngineType.Chrome146;
			doc.HtmlOptions.ForChrome.AddLinks = true;
			
			// You can store a reference to the filter to reduce code repetition
			var options = doc.HtmlOptions.ForChrome;
			
			options.UseScript = false;
			options.AddTags = true;
			
			doc.AddImageUrl("http://www.websupergoo.com");
			doc.Save("wsg1.pdf");
			// End Part:
		}
		// File End:

		// Example code for ForGecko Property of XHtmlOptions Object for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\2-forgecko.htm
		public static void Ex5_abcpdf_xhtmloptions2_forgecko() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.HtmlOptions.Engine = EngineType.Gecko;
			doc.HtmlOptions.ForGecko.AddLinks = true;
			
			// You can store a reference to the filter to reduce code repetition
			var options = doc.HtmlOptions.ForGecko;
			
			options.AddLinks = true;
			
			doc.AddImageUrl("http://www.websupergoo.com");
			doc.Save("wsg2.pdf");
			// End Part:
		}
		// File End:

		// Example code for ForMSHtml Property of XHtmlOptions Object for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\2-formshtml.htm
		public static void Ex5_abcpdf_xhtmloptions2_formshtml() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.HtmlOptions.Engine = EngineType.MSHtml;
			doc.HtmlOptions.ForMSHtml.AddLinks = true;
			
			// You can store a reference to the filter to reduce code repetition
			var options = doc.HtmlOptions.ForMSHtml;
			
			options.UseActiveX = true;
			options.AutoTruncate = true;
			
			doc.AddImageUrl("http://www.websupergoo.com");
			doc.Save("wsg3.pdf");
			// End Part:
		}
		// File End:

		// Example code for ForWebKit Property of XHtmlOptions Object for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\2-forwebkit.htm
		public static void Ex5_abcpdf_xhtmloptions2_forwebkit() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.HtmlOptions.Engine = EngineType.WebKit;
			doc.HtmlOptions.ForWebKit.AddLinks = true;
			
			// You can store a reference to the filter to reduce code repetition
			var options = doc.HtmlOptions.ForWebKit;
			
			options.UseScript = false;
			
			doc.AddImageUrl("http://www.websupergoo.com");
			doc.Save("wsg4.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddForms Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\addforms.htm
		public static void Ex5_abcpdf_xhtmloptionsaddforms() {
			// Part: 1 of 1
			using var doc = new Doc();
			// Covert html form fields to the pdf form fields in the output file
			doc.HtmlOptions.AddForms = true;
			int id = doc.AddImageUrl("https://www.nasa.gov/forms/submit-a-question-for-nasa/");
			// Save the document
			doc.Save("HtmlOptionsAddForms.pdf");
			// End Part:
		}
		// File End:

		// Example code for BrowserWidth Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\browserwidth.htm
		public static void Ex5_abcpdf_xhtmloptionsbrowserwidth() {
			// Part: 1 of 1
			using var doc = new Doc();
			string url = "https://photojournal.jpl.nasa.gov/catalog/PIA24312";
			// Render html page with default browser width
			doc.AddImageUrl(url);
			// Save the document
			doc.Save("HtmlOptionsBrowserWidth0.pdf");
			doc.Clear();
			// Render html page with browser width = 300
			doc.HtmlOptions.BrowserWidth = 300;
			doc.AddImageUrl(url);
			// Save the document
			doc.Save("HtmlOptionsBrowserWidth300.pdf");
			// End Part:
		}
		// File End:

		// Example code for FireShield Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following code snippet illustrates how one might add a rule to allow access to
		// an audio driver or similar.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\fireshield.htm
		public static void Ex5_abcpdf_xhtmloptionsfireshield() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.HtmlOptions.Engine = EngineType.Chrome146;
			doc.HtmlOptions.FireShield.Rules.Add(new XHtmlFireShield.PathRule(@"C:\Windows\*.drv", XHtmlFireShield.PathRule.AccessType.Allow));
			int id = doc.AddImageUrl("https://www.google.com/");
			// ...
			// End Part:
		}
		// File End:

		// Example code for HideBackground Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\hidebackground.htm
		public static void Ex5_abcpdf_xhtmloptionshidebackground() {
			// Part: 1 of 1
			using var doc = new Doc();
			// Please note that the URL below is included for demonstration purposes only.
			// In your code you should use your own URL. The site at the URL below
			// may include other opaque elements which may obscure our blue rectangle.
			string url = "https://www.websupergoo.com/";
			// Add some content
			doc.Color.String = "0 255 255"; // light blue
			doc.FillRect(200, 200);
			// Hide the background of the HTML page so content shows through
			doc.HtmlOptions.HideBackground = true;
			doc.AddImageUrl(url);
			// Save the document
			doc.Save("HtmlOptionsHideBackground.pdf");
			// End Part:
		}
		// File End:

		// Example code for HtmlCallback Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\htmlcallback.htm
		public static void Ex5_abcpdf_xhtmloptionshtmlcallback() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "https://photojournal.jpl.nasa.gov/catalog/PIA24312";
			// Set up the callback
			var theLog = new StringBuilder();
			doc.HtmlOptions.HtmlCallback = (string stage, object page) => theLog.AppendLine(stage);
			// Render html page
			doc.AddImageUrl(uri);
			// Add log over the top of the content
			doc.Rect.Inset(100, 100);
			doc.Color.String = "255 0 0";
			doc.FontSize = 96;
			doc.AddText(theLog.ToString());
			// Save the document
			doc.Save("HtmlOptionsCallback.pdf");
			// End Part:
		}
		// File End:

		// Example code for HtmlEmbedCallback Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\htmlembedcallback.htm
		public static void Ex5_abcpdf_xhtmloptionshtmlembedcallback() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "https://www.websupergoo.com/";
			// Set up the callback
			doc.HtmlOptions.HtmlEmbedCallback = (Doc d, HtmlEmbedInfo info) => {
				if (info.EmbedType == HtmlEmbedType.Swf) {
					HtmlParameter param;
					if (info.Parameters.TryGetValue("dataURL", out param)) {
						info.Parameters.Remove("dataURL");
						param.Conversion = HtmlParameterConversionType.UrlToTextFileContent;
						info.Parameters["dataXML"] = param;
					}
				}
			};
			// Render html page
			doc.AddImageUrl(uri);
			// Save the document
			doc.Save("HtmlOptionsEmbedCallback.pdf");
			// End Part:
		}
		// File End:

		// Example code for HttpAdditionalHeaders Property of XHtmlOptions Object for ABCpdf
		// .NET
		// 
		// The following example shows how this property may be used.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\httpadditionalheaders.htm
		public static void Ex5_abcpdf_xhtmloptionshttpadditionalheaders() {
			// Part: 1 of 1
			using var doc = new Doc();
			string url = "https://www.websupergoo.com/"; // assign appropriate URL
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.CookieContainer = new CookieContainer(); // required for HttpWebResponse.Cookies
			request.Credentials = null; // assign appropriate credentials
			using (var resp = request.GetResponse()) {
				// cookieless Forms Authentication adds authentication ticket to the URL
				url = resp.ResponseUri.AbsoluteUri;
				var response = (HttpWebResponse)resp;
				if (response.Cookies.Count > 0) { // includes ASP.NET_SessionId
					bool needsCookie2 = false;
					var builder = new StringBuilder("Cookie: ");
					for (int i = 0; i < response.Cookies.Count; ++i) {
						var cookie = response.Cookies[i];
						if (!needsCookie2 && cookie.Version != 1)
							needsCookie2 = true;
						if (i > 0)
							builder.Append("; ");
						builder.Append(cookie.ToString());
					}
					builder.Append(!needsCookie2 ? "\r\n" : "\r\nCookie2: $Version=1\r\n");
					doc.HtmlOptions.HttpAdditionalHeaders = builder.ToString();
				}
			}
			doc.HtmlOptions.Engine = EngineType.MSHtml;
			doc.HtmlOptions.NoCookie = true;
			doc.HtmlOptions.PageLoadMethod = PageLoadMethodType.MonikerForHtml;
			int id = doc.AddImageUrl(url);
			doc.Save("HttpHeaders.pdf");
			// End Part:
		}
		// File End:

		// Example code for ImageQuality Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\imagequality.htm
		public static void Ex5_abcpdf_xhtmloptionsimagequality() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "http://www.nasa.gov/multimedia/imagegallery/image_feature_313.html";
			// Set low image quality for HTML rendering
			doc.HtmlOptions.ImageQuality = 5;
			doc.AddImageUrl(uri);
			// Save the document
			doc.Save("HtmlOptionsImageQuality5.pdf");
			doc.Clear();
			// Set lossless image quality for HTML rendering
			doc.HtmlOptions.ImageQuality = 101;
			doc.AddImageUrl(uri);
			// Save the document
			doc.Save("HtmlOptionsImageQuality101.pdf");
			// End Part:
		}
		// File End:

		// Example code for LogonName Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows this property may be used.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\logonname.htm
		public static void Ex5_abcpdf_xhtmloptionslogonname() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "https://www.websupergoo.com/";
			// Assign name and password
			doc.HtmlOptions.Engine = EngineType.Gecko;
			doc.HtmlOptions.LogonName = "Steve";
			doc.HtmlOptions.LogonPassword = "stevepassword";
			// Add HTML page
			doc.AddImageUrl(uri);
			// Save the document
			doc.Save("HtmlOptionsLogon.pdf");
			// End Part:
		}
		// File End:

		// Example code for RetryCount Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on HTML rendering.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\retrycount.htm
		public static void Ex5_abcpdf_xhtmloptionsretrycount() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "https://photojournal.jpl.nasa.gov/catalog/PIA24312";
			// Set minimum number of items a page of HTML should contain.
			// Otherwise the page will be assumed to be invalid.
			doc.HtmlOptions.ContentCount = 20;
			// Try to obtain html page up to 11 times
			doc.HtmlOptions.RetryCount = 10;
			// The page must be obtained in less then 10 seconds
			doc.HtmlOptions.Timeout = 10000;
			try {
				doc.AddImageUrl(uri);
			}
			catch {
				// Page couldn't be loaded
			}
			// Save the document
			doc.Save("HtmlOptionsRetryCount.pdf");
			// End Part:
		}
		// File End:

		// Example code for UseScript Property of XHtmlOptions Object for ABCpdf .NET
		// 
		// The following example shows one method of crawling and transferring an entire site
		// to PDF. Here, we use JavaScript to determine the links present on the page. However,
		// you could equally well use the HtmlCallback to do the same thing.
		//
		// File Start: True .\5-abcpdf\xhtmloptions\2-properties\usescript.htm
		public static void Ex5_abcpdf_xhtmloptionsusescript() {
			// Part: 1 of 1
			using var doc = new Doc();
			string uri = "https://photojournal.jpl.nasa.gov/gallery/snt";
			// Set HTML options
			doc.HtmlOptions.AddLinks = true;
			doc.HtmlOptions.UseScript = true;
			doc.HtmlOptions.PageCacheEnabled = false;
			// JavaScript is used to extract all links from the page
			doc.HtmlOptions.OnLoadScript = "var hrefCollection = document.all ? document.all.tags(\"a\") : document.getElementsByTagName(\"a\");" +
				"var allLinks = \"\";" +
				"for(i = 0; i < hrefCollection.length; ++i) {" +
				"if (i > 0)" +
				"	allLinks += \",\";" +
				"allLinks += hrefCollection.item(i).href;" +
				"};" +
				"document.documentElement.abcpdf = allLinks;";
			// Array of links - start with base URL
			var links = new ArrayList();
			links.Add(uri);
			for (int i = 0; i < links.Count; i++) {
				// Stop if we render more than 20 pages
				if (doc.PageCount > 20)
					break;
				// Add page
				doc.Page = doc.AddPage();
				int id = doc.AddImageUrl(links[i] as string);
				// Links from the rendered page
				string allLinks = doc.HtmlOptions.GetScriptReturn(id);
				string[] newLinks = allLinks.Split([ ',' ]);
				foreach (string link in newLinks) {
					// Check to see if we allready rendered this page
					if (links.BinarySearch(link) < 0) {
						// Skip links inside the page
						int pos = link.IndexOf("#");
						if (!(pos > 0 && links.BinarySearch(link.Substring(0, pos)) >= 0)) {
							if (link.StartsWith(uri)) {
								links.Add(link);
							}
						}
					}
				}
				// Add other pages
				while (true) {
					doc.FrameRect();
					if (!doc.Chainable(id))
						break;
					doc.Page = doc.AddPage();
					id = doc.AddImageToChain(id);
				}
			}
			// Link pages together
			doc.HtmlOptions.LinkPages();
			// Save the document
			doc.Save("HtmlOptionsJavaScript.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetData Function of XImage 
		// Class for ABCpdf .NET
		// 
		// Here we read a TIFF file and present the data to the XImage object. We then add the
		// image to our document and then save the PDF.
		//
		// File Start: True .\5-abcpdf\ximage\1-methods\setdata.htm
		public static void Ex5_abcpdf_ximagesetdata() {
			// Part: 1 of 1
			using var img = new XImage();
			using var doc = new Doc();
			// read the data from a file
			string path = "../mypics/mypic.jpg";
			using var stream = File.OpenRead(path);
			byte[] theData = new byte[stream.Length];
			stream.Read(theData, 0, (int)stream.Length);
			// place the data into the image
			img.SetData(theData);
			doc.Rect.Inset(20, 20);
			doc.AddImageObject(img, false);
			doc.Save("imagesetdata.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetFile Function of XImage 
		// Class for ABCpdf .NET
		// 
		// Here we open a TIFF file using the XImage object. After we've opened the file we
		// add the image to our document and then save the PDF.
		//
		// File Start: True .\5-abcpdf\ximage\1-methods\setfile.htm
		public static void Ex5_abcpdf_ximagesetfile() {
			// Part: 1 of 1
			using var img = new XImage();
			using var doc = new Doc();
			img.SetFile("../mypics/mypic.jpg");
			doc.Rect.Inset(20, 20);
			doc.AddImageObject(img, false);
			doc.Save("imagesetfile.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetMask Function of XImage 
		// Class for ABCpdf .NET
		// 
		// Here we read a TIFF file and present the data to the Image object. We then read a
		// mask image and assign that to our Image. Finally we add the image to our document
		// and then save the PDF.
		//
		// File Start: True .\5-abcpdf\ximage\1-methods\setmask.htm
		public static void Ex5_abcpdf_ximagesetmask() {
			// Part: 1 of 1
			using var doc = new Doc();
			using var img = new XImage();
			using var msk = new XImage();
			img.SetFile("../mypics/mypic.jpg");
			msk.SetFile("../mypics/mymask.jpg");
			img.SetMask(msk, true);
			doc.Color.String = "0 0 0";
			doc.FillRect();
			doc.Rect.Inset(20, 20);
			doc.AddImageObject(img, true);
			doc.Save("imagesetmask.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetStream Function of XImage 
		// Class for ABCpdf .NET
		// 
		// Here we read a TIFF file and present the data to the XImage object. We then add the
		// image to our document and then save the PDF.
		//
		// File Start: True .\5-abcpdf\ximage\1-methods\setstream.htm
		public static void Ex5_abcpdf_ximagesetstream() {
			// Part: 1 of 1
			using var img = new XImage();
			string path = "../mypics/mypic.jpg";
			using var stream = File.OpenRead(path);
			img.SetStream(stream);
			using var doc = new Doc();
			doc.Rect.Inset(20, 20);
			doc.AddImageObject(img, false);
			doc.Save("imagesetstream.pdf");
			// End Part:
		}
		// File End:

		// Example code for Frame Property of XImage 
		// Class for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\ximage\2-properties\frame.htm
		public static void Ex5_abcpdf_ximageframe() {
			// Part: 1 of 1
			using var img = new XImage();
			using var doc = new Doc();
			img.SetFile("../mypics/multipage.tif");
			for (int i = 1; i <= img.FrameCount; i++) {
				img.Frame = i;
				doc.Page = doc.AddPage();
				doc.AddImageObject(img, false);
			}
			doc.Save("imageframe.pdf");
			// End Part:
		}
		// File End:

		// Example code for Selection Property of XImage 
		// Class for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\ximage\2-properties\selection.htm
		public static void Ex5_abcpdf_ximageselection() {
			// Part: 1 of 1
			using var img = new XImage();
			using var doc = new Doc();
			img.SetFile("../mypics/mypic.jpg");
			doc.Rect.String = img.Selection.String;
			doc.Rect.Magnify(0.5, 0.5);
			doc.Rect.Position(100, 30);
			doc.AddImageObject(img, false);
			img.Selection.Inset(100, 200);
			doc.Rect.String = img.Selection.String;
			doc.Rect.Position(170, 400);
			doc.AddImageObject(img, false);
			doc.Save("imageselect.pdf");
			// End Part:
		}
		// File End:

		// Example code for Point Property of XPoint Class for ABCpdf .NET
		// 
		// The following code adds three words to a document. The positioning is done using
		// standard .NET Points.
		//
		// File Start: True .\5-abcpdf\xpoint\2-properties\point.htm
		public static void Ex5_abcpdf_xpointpoint() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			Point pt = doc.Pos.Point;
			pt.Offset(100, 150);
			doc.Pos.Point = pt;
			doc.AddText("One");
			pt.Offset(100, 150);
			doc.Pos.Point = pt;
			doc.AddText("Two");
			pt.Offset(100, 150);
			doc.Pos.Point = pt;
			doc.AddText("Three");
			doc.Save("xptpt.pdf");
			// End Part:
		}
		// File End:

		// Example code for String Property of XPoint Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xpoint\2-properties\string.htm
		public static void Ex5_abcpdf_xpointstring() {
			// Part: 1 of 1
			var pt = new XPoint();
			pt.String = "20 10";
			Response.Write($"X = {pt.X}");
			Response.Write("<br>");
			Response.Write($"Y = {pt.Y}");
			// End Part:
		}
		// File End:

		// Example code for Inset Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\inset.htm
		public static void Ex5_abcpdf_xrectinset() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "0 0 200 100";
			Response.Write($"Rect = {rc}");
			Response.Write("&lt;br&gt;");
			rc.Inset(10, 20);
			Response.Write("Inset = " + rc.String);
			// End Part:
		}
		// File End:

		// Example code for Magnify Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\magnify.htm
		public static void Ex5_abcpdf_xrectmagnify() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "20 20 220 120";
			Response.Write($"Rect = {rc}");
			Response.Write("&lt;br&gt;");
			rc.Magnify(0.5, 0.5);
			Response.Write($"Scale = {rc}");
			// End Part:
		}
		// File End:

		// Example code for Move Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\move.htm
		public static void Ex5_abcpdf_xrectmove() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "20 20 220 120";
			Response.Write($"Rect = {rc}");
			rc.Move(50, 50);
			Response.Write("&lt;br&gt;");
			Response.Write($"Move = {rc}");
			// End Part:
		}
		// File End:

		// Example code for Position Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\position.htm
		public static void Ex5_abcpdf_xrectposition() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "20 20 220 120";
			Response.Write($"Rect = {rc}");
			Response.Write("&lt;br&gt;");
			rc.Position(50, 50);
			Response.Write($"Pos. = {rc}");
			// End Part:
		}
		// File End:

		// Example code for Resize Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\resize.htm
		public static void Ex5_abcpdf_xrectresize() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "20 20 220 120";
			Response.Write($"Rect = {rc}");
			Response.Write("&lt;br&gt;");
			rc.Resize(50, 150);
			Response.Write($"Pos. = {rc}");
			// End Part:
		}
		// File End:

		// Example code for SetRect Function of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\1-methods\setrect.htm
		public static void Ex5_abcpdf_xrectsetrect() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "20 20 220 120";
			Response.Write($"Rect = {rc}");
			Response.Write("&lt;br&gt;");
			rc.SetRect(20, 40, 50, 150);
			Response.Write("Pos. = {rc}");
			// End Part:
		}
		// File End:

		// Example code for Rectangle Property of XRect Class for ABCpdf .NET
		// 
		// The following code adds two blocks of text to a document. The positioning is done
		// using standard .NET Rectangles.
		//
		// File Start: True .\5-abcpdf\xrect\2-properties\rectangle.htm
		public static void Ex5_abcpdf_xrectrectangle() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			var rc = doc.MediaBox.Rectangle;
			rc.Inflate(-50, -50);
			rc.Height = 250;
			doc.Rect.Rectangle = rc;
			doc.FrameRect();
			doc.AddText("First Rectangle...");
			rc.Offset(0, 300);
			doc.Rect.Rectangle = rc;
			doc.FrameRect();
			doc.AddText("Second Rectangle...");
			doc.Save("xrectrectangle.pdf");
			// End Part:
		}
		// File End:

		// Example code for String Property of XRect Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xrect\2-properties\string.htm
		public static void Ex5_abcpdf_xrectstring() {
			// Part: 1 of 1
			var rc = new XRect();
			rc.String = "10 10 200 100";
			Response.Write($"Width = {rc.Width}");
			Response.Write("&lt;br&gt;");
			Response.Write($"Height = {rc.Height}");
			// End Part:
		}
		// File End:

		// Example code for AntiAliasImages Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\antialiasimages.htm
		public static void Ex5_abcpdf_xrenderingantialiasimages() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/HyperX.pdf");
			doc.Rect.Inset(200, 200);
			// Render document with AntiAliasImages = true (default)
			doc.Rendering.Save("RenderingAntiAliasImagesTrue.png");
			// Render document with AntiAliasImages = false
			doc.Rendering.AntiAliasImages = false;
			// Save the image
			doc.Rendering.Save("RenderingAntiAliasImagesFalse.png");
			// End Part:
		}
		// File End:

		// Example code for AntiAliasPolygons Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\antialiaspolygons.htm
		public static void Ex5_abcpdf_xrenderingantialiaspolygons() {
			// Part: 1 of 1
			using var doc = new Doc();
			// Add a polygon
			doc.Color.String = "255 0 0";
			doc.AddPoly("32 650 50 704 68 650 22 683 79 683 32 650", true);
			doc.Rect.String = "20 650 80 704";
			// Render the drawn area with AntiAliasPolygons (default)
			using var antiAliasedBitmap = doc.Rendering.GetBitmap();
			// Render the drawn area without AntiAliasPolygons
			doc.Rendering.AntiAliasPolygons = false;
			using var aliasedBitmap = doc.Rendering.GetBitmap();
			// Add magnified aliased image
			doc.Rect.String = "5 20 605 560";
			doc.AddImageBitmap(aliasedBitmap, false);
			// Anotate
			doc.Color.String = "black";
			doc.FontSize = 30;
			doc.Pos.String = "20 750";
			doc.AddText("Original path:");
			doc.Pos.String = "20 620";
			doc.AddText("Magnified rendered image:");
			// Render the document with aliased image
			doc.Rendering.DotsPerInch = 36;
			doc.Rect.String = doc.MediaBox.String;
			doc.Rendering.Save("RenderingAntiAliasPolygonsFalse.png");
			// Add magnified antialiased image
			doc.Rect.String = "5 20 605 560";
			doc.AddImageBitmap(antiAliasedBitmap, false);
			// Render the document with antialiased image
			doc.Rendering.DotsPerInch = 36;
			doc.Rect.String = doc.MediaBox.String;
			doc.Rendering.Save("RenderingAntiAliasPolygonsTrue.png");
			// End Part:
		}
		// File End:

		// Example code for AntiAliasText Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\antialiastext.htm
		public static void Ex5_abcpdf_xrenderingantialiastext() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(50, 50);
			// Add some text
			doc.FontSize = 48;
			doc.Pos.String = "50 690";
			int id = doc.AddText("Abc");
			// Render images
			doc.Rect.String = doc.GetInfo(id, "rect");
			doc.Rendering.AntiAliasText = true;
			using var antialiasedBitmap = doc.Rendering.GetBitmap();
			doc.Rendering.AntiAliasText = false;
			using var aliasedBitmap = doc.Rendering.GetBitmap();
			// Add enlarged images to the pdf file
			doc.Rect.Magnify(5, 5);
			doc.Rect.Move(0, -300);
			doc.AddImageBitmap(aliasedBitmap, false);
			doc.Rect.Move(0, -300);
			doc.AddImageBitmap(antialiasedBitmap, false);
			// Annotate
			doc.Rect.String = doc.MediaBox.String;
			doc.Color.String = "255 0 0";
			doc.FontSize = 36;
			doc.Pos.String = "50 740";
			doc.AddText("Original text:");
			doc.Pos.String = "50 620";
			doc.AddText("Magnified aliased image:");
			doc.Pos.String = "50 320";
			doc.AddText("Magnified antialiased image:");
			// Save render of pdf files
			doc.Rendering.AntiAliasText = true;
			doc.Rendering.DotsPerInch = 36;
			doc.Rendering.Save("RenderingAntiAliasText.png");
			// End Part:
		}
		// File End:

		// Example code for ColorSpace Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\colorspace.htm
		public static void Ex5_abcpdf_xrenderingcolorspace() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.AddImage("../mypics/Shuttle.jpg");
			doc.Rect.String = doc.MediaBox.String;
			// Render document in Gray colorspace
			doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Gray;
			doc.Rendering.DotsPerInch = 36;
			doc.Rendering.Save("RenderingColorSpace.png");
			// End Part:
		}
		// File End:

		// Example code for DefaultHalftone Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\defaulthalftone.htm
		public static void Ex5_abcpdf_xrenderingdefaulthalftone() {
			// Part: 1 of 1
			using var doc = new Doc();
			using var image = new XImage();
			image.SetFile("../mypics/Shuttle.jpg");
			doc.Rect.String = image.Selection.String;
			doc.AddImage(image);
			// Save rendered image as black and white picture using Line spot function
			doc.Rendering.UseEmbeddedHalftone = false;
			doc.Rendering.DotsPerInch = 50;
			doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Gray;
			doc.Rendering.BitsPerChannel = 1;
			doc.Rendering.DefaultHalftone = "Spot,30,100,Line";
			doc.Rendering.Save("RenderingHalftoneLine.png");
			// Save rendered image as black and white picture using Diamond spot function
			doc.Rendering.DefaultHalftone = "Spot,0,100,Diamond";
			doc.Rendering.Save("RenderingHalftoneDiamond.png");
			// End Part:
		}
		// File End:

		// Example code for DrawAnnotations Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\drawannotations.htm
		public static void Ex5_abcpdf_xrenderingdrawannotations() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/Annotations.pdf");
			doc.Rect.Pin = XRect.Corner.TopLeft;
			doc.Rect.Height = 300;
			// Render document with DrawAnnotations (default)
			doc.Rendering.Save("RenderingDrawAnnotationsTrue.png");
			// Render document without DrawAnnotations
			doc.Rendering.DrawAnnotations = false;
			doc.Rendering.Save("RenderingDrawAnnotationsFalse.png");
			// End Part:
		}
		// File End:

		// Example code for IccCmyk Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\icccmyk.htm
		public static void Ex5_abcpdf_xrenderingicccmyk() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(20, 40);
			doc.FontSize = 96;
			// Add CMYK Content
			doc.Color.String = "200 20 20 20";
			doc.AddText("Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.");
			doc.Rect.String = doc.MediaBox.String;
			doc.Rendering.DotsPerInch = 36;
			doc.Rendering.IccCmyk = "../mypics/cmyk.icc";
			doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Rgb;
			// Save the image
			doc.Rendering.Save("RenderingIccCmyk.png");
			// End Part:
		}
		// File End:

		// Example code for Overprint Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\overprint.htm
		public static void Ex5_abcpdf_xrenderingoverprint() {
			// Part: 1 of 1
			using var doc = new Doc();
			// Open document with overprint
			doc.Read("../mypics/Overprint.pdf");
			// Render the document (we need to go to CMYK for overprint)
			doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Cmyk;
			doc.Rendering.Overprint = true;
			doc.Rendering.IccRgb = "device";
			doc.Rendering.IccGray = "device";
			doc.Rendering.IccCmyk = "device";
			doc.Rendering.IccOutput = "device";
			// Put image back into another PDF and render the document as RGB
			// (so that we can see the effect of the overprint in RGB)
			using var theRgb = new Doc();
			theRgb.AddImageData(doc.Rendering.GetData(".tif"), 1);
			theRgb.Rendering.DotsPerInch = 36;
			theRgb.Rendering.ColorSpace = XRendering.ColorSpaceType.Rgb;
			theRgb.Rendering.Save("RenderingOverprint.png");
			// End Part:
		}
		// File End:

		// Example code for SaveAlpha Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		// We create a PDF with some text on it. We then render the PDF with an alpha channel
		// and add the transparent image into a new PDF with a blue background. The blue background
		// shows through where the image is transparent.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\savealpha.htm
		public static void Ex5_abcpdf_xrenderingsavealpha() {
			// Part: 1 of 1
			// Add some text
			Doc doc = new Doc();
			doc.FontSize = 196;
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.3;
			doc.AddText("Hello World");
			// Render the PDF with alpha
			doc.Rendering.SaveAlpha = true;
			using var alphaBitmap = doc.Rendering.GetBitmap();
			// Create a blue PDF
			doc = new Doc();
			doc.Color.String = "0 0 255";
			doc.FillRect();
			// Add the transparent Bitmap into the PDF
			// so that the underlying blue can show through
			doc.AddImageBitmap(alphaBitmap, true);
			// Save render of pdf
			doc.Rendering.Save("RenderingSaveAlpha.png");
			// End Part:
		}
		// File End:

		// Example code for SaveCompression Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows how to render a PDF into a multipage G4 compressed Fax
		// TIFF using different vertical and horizontal resolutions.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\savecompression.htm
		public static void Ex5_abcpdf_xrenderingsavecompression() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			// set up the rendering parameters
			doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Gray;
			doc.Rendering.BitsPerChannel = 1;
			doc.Rendering.DotsPerInchX = 200;
			doc.Rendering.DotsPerInchY = 400;
			// loop through the pages
			int n = doc.PageCount;
			for (int i = 1; i <= n; i++) {
				doc.PageNumber = i;
				doc.Rect.String = doc.CropBox.String;
				doc.Rendering.SaveAppend = (i != 1);
				doc.Rendering.SaveCompression = XRendering.Compression.G4;
				doc.Rendering.Save("fax.tif");
			}
			// End Part:
		}
		// File End:

		// Example code for SaveQuality Property of XRendering Object for ABCpdf .NET
		// 
		// The following example shows the effect that this parameter has on PDF rendering.
		//
		// File Start: True .\5-abcpdf\xrendering\2-properties\savequality.htm
		public static void Ex5_abcpdf_xrenderingsavequality() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/SpaceShuttlePage6.pdf");
			doc.Rendering.DotsPerInch = 36;
			// Save at low quality
			doc.Rendering.SaveQuality = 5;
			doc.Rendering.Save("RenderingQuality5.jpg");
			// Save at high quality
			doc.Rendering.SaveQuality = 75;
			doc.Rendering.Save("RenderingQuality75.jpg");
			// End Part:
		}
		// File End:

		// Example code for Template Property of XSaveOptions Object for ABCpdf .NET
		// 
		// See the example project for how to use a SWF template file.
		// 
		//  Here we specify one page per frame and 2 frames per second for SWF format.
		//
		// File Start: True .\5-abcpdf\xsaveoptions\2-properties\template.htm
		public static void Ex5_abcpdf_xsaveoptionstemplate() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			doc.SaveOptions.Template = XSaveTemplateData.Template_OnePagePerFrame + "\0FrameRate:512";
			doc.Save("swfsave_fr.swf");
			// End Part:
		}
		// File End:

		// Example code for WritePageSeparator Property of XSaveOptions Object for ABCpdf .NET
		// 
		// The following example shows how to customize the page separator.
		//
		// File Start: True .\5-abcpdf\xsaveoptions\2-properties\writepageseparator.htm
		public static void Ex5_abcpdf_xsaveoptionswritepageseparator() {
			// Part: 1 of 1
			#if NETFRAMEWORK // ignore
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			doc.SaveOptions.WritePageSeparator = delegate (int pageNum, XSaveOptions.ExportArgs e) {
				var writer = (XmlWriter)e.Writer;
				if (pageNum > 1) {
					writer.WriteStartElement("hr");
					writer.WriteEndElement();
				}
				writer.WriteStartElement("div");
				writer.WriteAttributeString("align", "right");
				writer.WriteString(string.Format("Page {0}", pageNum));
				writer.WriteFullEndElement();
			};
			doc.Save("PageSeparator.htm");
			#endif // ignore
			// End Part:
		}
		// File End:

		// Example code for SetMeasureResolution Function of XSaveTemplateData Class for ABCpdf
		// .NET
		// 
		// Here we use 72 DPI so that 1 inch in PDF becomes 72 pixels in SWF.
		//
		// File Start: True .\5-abcpdf\xsavetemplatedata\1-methods\setmeasureresolution.htm
		public static void Ex5_abcpdf_xsavetemplatedatasetmeasureresolution() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			doc.SaveOptions.TemplateData = new XSaveTemplateData();
			doc.SaveOptions.TemplateData.SetMeasureResolution(72);
			doc.Save("swfsave_mr.swf");
			// End Part:
		}
		// File End:

		// Example code for Open Function of XTagging Class for ABCpdf .NET
		// 
		// The following code adds a list of animals. Normally this list would go directly into
		// the Document root level tag. However here we first open a Section, Div and P (paragraph)
		// so that the list will go inside those elements instead.
		//
		// File Start: True .\5-abcpdf\xtagging\1-methods\04-open.htm
		public static void Ex5_abcpdf_xtagging04_open() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Font = doc.EmbedFont("Arial");
			doc.TextStyle.Size = 48;
			doc.Rect.Inset(50, 50);
			string[] animals = ["Panda", "Koala", "Red panda", "Otter", "Kitten",
				"Puppy", "Bunny", "Hedgehog", "Penguin", "Dolphin", "Seal", "Fox",
				"Squirrel", "Baby elephant", "Fawn", "Chick", "Hamster",
				"Guinea pig", "Ferret", "Quokka"];
			var sb = new StringBuilder();
			sb.AppendLine("<ul>");
			foreach (var animal in animals)
				sb.AppendLine($"<li>{animal}</li>");
			sb.AppendLine("</ul>");
			doc.TextStyle.AutoTag = true;
			doc.Tag.Open("Sect", "Div", "P");
			int id = doc.AddTextStyled(sb.ToString());
			doc.FrameRect();
			while (true) {
				var tl = (TextLayer)doc.ObjectSoup[id];
				if (!tl.Truncated)
					break;
				doc.Page = doc.AddPage();
				id = doc.AddTextStyled("", id);
				doc.FrameRect();
			}
			doc.Tag.Close("P", "Div", "Sect");
			doc.Save("doctagopen.pdf");
			doc.Read("doctagopen.pdf");
			var ts = doc.Tag.GetStructure();
			var str = ts.ExtractStructure();
			File.WriteAllText("doctagopen.txt", str.ToString());
			// End Part:
		}
		// File End:

		// Example code for Close Function of XTagging Class for ABCpdf .NET
		// 
		// This example shows how to add tagged content to a document which is already tagged.
		//
		// File Start: True .\5-abcpdf\xtagging\1-methods\05-close.htm
		public static void Ex5_abcpdf_xtagging05_close() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spacex_nasa_dragon.pdf");
			doc.Font = doc.EmbedFont("Arial", LanguageType.Unicode, false, true, false);
			doc.TextStyle.Size = 18;
			doc.Rect.SetRect(0, 0, doc.MediaBox.Width, 100);
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.Bold = true;
			doc.Color.SetRgb(200, 00, 0);
			doc.Tag.Open("P", "Span");
			doc.AddText("This is a NASA document\r\n");
			doc.Tag.CloseOpen("Span");
			doc.AddText("National Aeronautics and Space Administration");
			doc.Tag.Close("Span", "P");
			doc.Save("addtagstotaggeddoc.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddFocus Function of XTagging Class for ABCpdf .NET
		// 
		// The following code adds sequence of tagged areas to a page. We first delete any existing
		// structure so we have a clean slate. The first Artifact covers the entire page so
		// that anything which is not tagged later will become an artifact. The next tags create
		// a nested sequence of H1, H2 and P elements.
		//
		// File Start: True .\5-abcpdf\xtagging\1-methods\08-addfocus.htm
		public static void Ex5_abcpdf_xtagging08_addfocus() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spacex_nasa_dragon.pdf");
			Atom.RemoveItem(doc.ObjectSoup.Catalog.Atom, "StructTreeRoot");
			var st = doc.Tag.GetStructure();
			st.Detag();
			st.Title = "SpaceX NASA Dragon";
			st.CreateAsRequired();
			var div = st.Root.AddKid("Document").AddKid("Div");
			var artifact = new StructureElementElement(div) { EntryS = "Artifact" };
			doc.Tag.AddFocus(artifact, doc.Rect).AddMcids = false;
			var h1 = doc.Tag.AddFocus(div.AddKid("H1"), XRect.FromSides(0, 650, 600, 710)).Tag;
			var h2 = doc.Tag.AddFocus(div.AddKid("H2"), XRect.FromSides(0, 620, 600, 650)).Tag;
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 580, 600, 620));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 500, 600, 580));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 440, 600, 500));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 360, 600, 440));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 270, 600, 360));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 200, 600, 270));
			doc.Tag.AddFocus(h2.AddKid("P"), XRect.FromSides(0, 100, 600, 200));
			var focus = doc.Tag.AddFocus(div.AddKid("Figure"), XRect.FromSides(360, 480, 600, 650));
			focus.Tag.EntryAlt = "Spacecraft in orbit over earth.";
			// in later versions, instead of the next five lines, you can just use ...
			var layout = new StandardLayoutAttributesElement(focus.Tag);
			layout.EntryO = "Layout";
			layout.EntryBBox = new RectangleElement(ArrayAtom.FromXRect(focus.Bounds), focus.Tag.Host);
			focus.Tag.EntryA = new ArrayElement<Element>(focus.Tag);
			focus.Tag.EntryA.Add(layout);
			// ... focus.Tag.SetBBox(focus.Bounds);
			doc.Tag.MakePdfUAConformant = true;
			doc.Save("taggedarea.pdf");
			st.UpdateActualText(true, true);
			var txt = st.ExtractStructure();
			File.WriteAllText("taggedarea.txt", txt.ToString());
			// End Part:
		}
		// File End:

		// Example code for OpaqueTypes Property of Focus Class for ABCpdf .NET
		// 
		// The following code detags the document leaving Artifacts behind. It then tags the
		// entire pag as a Sect. When the output is exported, items previously marked as Artifacts
		// - some images, the header, footer and page number- are not part of the Sect.
		//
		// File Start: True .\5-abcpdf\xtagging.focus\2-properties\opaquetypes.htm
		public static void Ex5_abcpdf_xtagging_focusopaquetypes() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spacex_nasa_dragon.pdf");
			var st = doc.Tag.GetStructure();
			st.Detag(); // Artifacts will be left
			st.CreateAsRequired();
			var div = st.Root.AddKid("Document").AddKid("Div");
			var focus = doc.Tag.AddFocus(div.AddKid("Sect"), doc.Rect);
			focus.OpaqueTypes = new HashSet<string>(["Artifact"]);
			doc.Save("opaquetags.pdf");
			st.UpdateActualText(true, true);
			st.MarkupStructure();
			doc.Save("opaquetags.pdf");
			// End Part:
		}
		// File End:

		// Example code for AutoTag Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following code creates a simple tagged document containg text and an image.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\autotag.htm
		public static void Ex5_abcpdf_xtextstyleautotag() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "From the attic window, snow-draped rooftops stretch into the distance, chimneys smoking softly. Icicles glitter in the pale sun. A quiet, frozen world.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.AutoTag = true;
			doc.TextStyle.Size = 36;
			doc.AddTextStyled($"<p>{text}</p>");
			using var img = XImage.FromFile("../mypics/mypic.jpg", null);
			doc.Rect.SetRect(100, 100, img.Width / 2, img.Height / 2);
			var figure = doc.Tag.MakeTag("Figure");
			figure.Attributes = new DictAtom();
			figure.Attributes["Alt"] = new StringAtom("Snowy rooftops.");
			doc.Tag.Open(figure);
			doc.AddImageObject(img, true);
			doc.Tag.Close(figure.Type);
			doc.Save("simpletags.pdf");
			// End Part:
		}
		// File End:

		// Example code for Bold Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some bold text to a document.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\bold.htm
		public static void Ex5_abcpdf_xtextstylebold() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 96;
			doc.TextStyle.Bold = true;
			doc.AddText(text);
			doc.Save("stylebold.pdf");
			// End Part:
		}
		// File End:

		// Example code for CharSpacing Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add three blocks of text to a document. The first block uses the
		// default spacing. The second block uses a positive value to stretch out the text.
		// The last block uses a negative value to condense the text.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\charspacing.htm
		public static void Ex5_abcpdf_xtextstylecharspacing() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.TextStyle.Size = 96;
			doc.AddText("Zero CharSpacing");
			doc.Rect.Move(0, -300);
			doc.TextStyle.CharSpacing = 10;
			doc.AddText("Positive CharSpacing");
			doc.Rect.Move(0, -300);
			doc.TextStyle.CharSpacing = -10;
			doc.AddText("Negative CharSpacing");
			doc.Save("stylecspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for HPos Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following code adds two blocks of text to a document. The first block is left
		// aligned and the second is right aligned. Before adding the text we change the current
		// rectangle and frame it so that you can see how the text is aligned.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\hpos.htm
		public static void Ex5_abcpdf_xtextstylehpos() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.Rect.Magnify(1.0, 0.5);
			doc.Rect.Inset(40, 40);
			doc.FrameRect();
			doc.AddText("Left justified text...");
			doc.Rect.Move(0, doc.Rect.Height + 80);
			doc.FrameRect();
			doc.TextStyle.HPos = 1.0;
			doc.AddText("Right justified text...");
			doc.Save("dochpos.pdf");
			// End Part:
		}
		// File End:

		// Example code for Indent Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add a block of text to a document. We specify a ParaSpacing value
		// to space out the paragraphs and an Indent value to indent the first line of each
		// paragraph.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\indent.htm
		public static void Ex5_abcpdf_xtextstyleindent() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt.";
			text = text + text;
			text = text + "\r\n" + text + "\r\n";
			text = text + text + text + text;
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 16;
			doc.TextStyle.ParaSpacing = 16;
			doc.TextStyle.Indent = 48;
			doc.AddText(text);
			doc.Save("styleindent.pdf");
			// End Part:
		}
		// File End:

		// Example code for Italic Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some italic text to a document.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\italic.htm
		public static void Ex5_abcpdf_xtextstyleitalic() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text;
			text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 96;
			doc.TextStyle.Italic = true;
			doc.AddText(text);
			doc.Save("styleitalic.pdf");
			// End Part:
		}
		// File End:

		// Example code for Justification Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add two blocks of text to a document. The first is added with
		// no justification and the second is added with a justification factor of one.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\justification.htm
		public static void Ex5_abcpdf_xtextstylejustification() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 48;
			doc.AddText(text);
			doc.Rect.Move(0, -350);
			doc.TextStyle.Justification = 1.0;
			doc.AddText(text);
			doc.Save("stylejustification.pdf");
			// End Part:
		}
		// File End:

		// Example code for Kerning Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following shows how to insert a table of contents while disabling kerning.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\kerning.htm
		public static void Ex5_abcpdf_xtextstylekerning() {
			// Part: 1 of 1
			string text = File.ReadAllText("../Rez/tableofcontents.txt");
			text = text.Replace("\r", "<br>"); // make our carriage returns into breaks
			text = text.Replace(" ", "		 "); // make our indent at start of line into nbsp
			using var doc = new Doc();
			doc.TextStyle.Size = 36;
			doc.TextStyle.Kerning = XTextStyle.KerningType.None;
			doc.Rect.Inset(10, 10);
			doc.Page = doc.AddPage();
			doc.AddTextStyled(text.Replace(" ~", "<leader>.</leader>"));
			doc.Save("TableOfContentsWithLeaders.pdf");
			// End Part:
		}
		// File End:

		// Example code for LeftMargin Property of XTextStyle Class for ABCpdf .NET
		// 
		// In the following example we add three blocks of text to a document. The first block
		// uses the default left margin. The subsequent blocks use different left margin settings
		// to indent the text.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\leftmargin.htm
		public static void Ex5_abcpdf_xtextstyleleftmargin() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani...";
			doc.TextStyle.Size = 48;
			doc.AddText(text);
			doc.Rect.Move(0, -250);
			doc.TextStyle.LeftMargin = 100;
			doc.AddText(text);
			doc.Rect.Move(0, -250);
			doc.TextStyle.LeftMargin = 200;
			doc.AddText(text);
			doc.Save("stylemargin.pdf");
			// End Part:
		}
		// File End:

		// Example code for LineSpacing Property of XTextStyle Class for ABCpdf .NET
		// 
		// In the following example we add three blocks of text to a document. The first block
		// uses the default line spacing. The second block uses a positive value to space out
		// the lines. The last block uses a negative value to shift the lines together.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\linespacing.htm
		public static void Ex5_abcpdf_xtextstylelinespacing() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani...";
			doc.TextStyle.Size = 48;
			doc.AddText(text);
			doc.Rect.Move(0, -250);
			doc.TextStyle.LineSpacing = 20;
			doc.AddText(text);
			doc.Rect.Move(0, -350);
			doc.TextStyle.LineSpacing = -20;
			doc.AddText(text);
			doc.Save("stylelspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for Outline Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some text to a document varying the outline style to show
		// how different values affect the final result.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\outline.htm
		public static void Ex5_abcpdf_xtextstyleoutline() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.TextStyle.Size = 144;
			doc.AddText("Outline 0");
			doc.Rect.Move(0, -300);
			doc.TextStyle.Outline = 4;
			doc.AddText("Outline 4");
			doc.Rect.Move(0, -300);
			doc.TextStyle.Outline = 10;
			doc.AddText("Outline 10");
			doc.Save("styleoutline.pdf");
			// End Part:
		}
		// File End:

		// Example code for ParaSpacing Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add two blocks of text to a document. The first block uses the
		// default paragraph spacing. The second block uses a positive value to space out the
		// paragraphs.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\paraspacing.htm
		public static void Ex5_abcpdf_xtextstyleparaspacing() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur. Hi omnes lingua, institutis, legibus inter se differunt.";
			text = text + "\r\n" + text + "\r\n" + text + "\r\n" + text;
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 16;
			doc.AddText(text);
			doc.Rect.Move(0, -350);
			doc.TextStyle.ParaSpacing = 20;
			doc.AddText(text);
			doc.Save("stylepspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for Size Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following example adds two blocks of styled text to a document. The first block
		// is in 96.5 point type and the second is in 192.5 point type.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\size.htm
		public static void Ex5_abcpdf_xtextstylesize() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.TextStyle.Size = 96.5;
			doc.AddText("Small ");
			doc.TextStyle.Size = 192.5;
			doc.AddText("Big");
			doc.Save("stylesize.pdf");
			// End Part:
		}
		// File End:

		// Example code for Strike Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some strikethrough styled text to a document.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\strike.htm
		public static void Ex5_abcpdf_xtextstylestrike() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 96;
			doc.TextStyle.Strike = true;
			doc.AddText(text);
			doc.Save("stylestrike.pdf");
			// End Part:
		}
		// File End:

		// Example code for Strike2 Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some double strikethrough styled text to a document.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\strike2.htm
		public static void Ex5_abcpdf_xtextstylestrike2() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 96;
			doc.TextStyle.Strike2 = true;
			doc.AddText(text);
			doc.Save("stylestrike2.pdf");
			// End Part:
		}
		// File End:

		// Example code for String Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following code.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\string.htm
		public static void Ex5_abcpdf_xtextstylestring() {
			// Part: 1 of 1
			var ts = new XTextStyle();
			ts.String = "24.5 10 0 0 0 0 0";
			Response.Write($"Size = {ts.Size}&lt;br&gt;");
			Response.Write($"Indent = {ts.Indent}");
			// End Part:
		}
		// File End:

		// Example code for Underline Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add some underlined text to a document.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\underline.htm
		public static void Ex5_abcpdf_xtextstyleunderline() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.";
			doc.Rect.Inset(20, 40);
			doc.TextStyle.Size = 96;
			doc.TextStyle.Underline = true;
			doc.AddText(text);
			doc.Save("styleunderline.pdf");
			// End Part:
		}
		// File End:

		// Example code for VPos Property of XTextStyle Class for ABCpdf .NET
		// 
		// The following code adds two blocks of text to a document. The first block is bottom
		// aligned and the second is top aligned. Before adding the text we change the current
		// rectangle and frame it so that you can see how the text is aligned.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\vpos.htm
		public static void Ex5_abcpdf_xtextstylevpos() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.Rect.Magnify(1.0, 0.5);
			doc.Rect.Inset(40, 40);
			doc.FrameRect();
			doc.AddText("Top aligned text...");
			doc.Rect.Move(0, doc.Rect.Height + 80);
			doc.FrameRect();
			doc.TextStyle.VPos = 1.0;
			doc.AddText("Bottom aligned text...");
			doc.Save("docvpos.pdf");
			// End Part:
		}
		// File End:

		// Example code for WordSpacing Property of XTextStyle Class for ABCpdf .NET
		// 
		// In this example we add three blocks of text to a document. The first block uses the
		// default spacing. The second block uses a positive value to stretch out the text.
		// The last block uses a negative value to condense the text.
		//
		// File Start: True .\5-abcpdf\xtextstyle\2-properties\wordspacing.htm
		public static void Ex5_abcpdf_xtextstylewordspacing() {
			// Part: 1 of 1
			using var doc = new Doc();
			string text = "This is an example of word spacing.";
			doc.TextStyle.Size = 72;
			doc.AddText(text);
			doc.Rect.Move(0, -300);
			doc.TextStyle.WordSpacing = 20;
			doc.AddText(text);
			doc.Rect.Move(0, -300);
			doc.TextStyle.WordSpacing = -20;
			doc.AddText(text);
			doc.Save("stylewspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for Invert Function of XTransform Class for ABCpdf .NET
		// 
		// Here we add some text rotated at 45 degrees anti-clockwise around the middle of the
		// document. We then invert the transform and draw some more text. Because the transform
		// has been inverted the text now appears rotated 45 degrees clockwise.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\invert.htm
		public static void Ex5_abcpdf_xtransforminvert() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 72;
			doc.Rect.String = "0 0 999 999";
			doc.Pos.String = "302 396";
			doc.Transform.Rotate(45, 302, 396);
			doc.AddText("45 Degrees");
			doc.Pos.String = "302 396";
			doc.Transform.Invert();
			doc.AddText("Inverted");
			doc.Save("transforminvert.pdf");
			// End Part:
		}
		// File End:

		// Example code for Magnify Function of XTransform Class for ABCpdf .NET
		// 
		// Here we add two chunks of text. The default text is added in black and the magnified
		// text is drawn in red. We specify the middle of the document as the anchor point which
		// means that all scaling is relative to the middle of the document. Our horizontal
		// scale factor is larger than our text has been stretched horizontally somewhat.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\magnify.htm
		public static void Ex5_abcpdf_xtransformmagnify() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(200, 200);
			doc.FontSize = 48;
			doc.AddText("Normal");
			doc.FrameRect();
			doc.Rect.Move(0, -100);
			doc.Color.String = "255 0 0";
			doc.Transform.Magnify(2, 1.5, 302, 396);
			doc.AddText("Magnified");
			doc.FrameRect();
			doc.Save("transformmagnify.pdf");
			// End Part:
		}
		// File End:

		// Example code for Reset Function of XTransform Class for ABCpdf .NET
		// 
		// Here we add some text rotated at 60 degrees around the middle of the document. We
		// then reset the transform and draw some more text. This text is drawn with no rotation
		// because the transform has been reset.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\reset.htm
		public static void Ex5_abcpdf_xtransformreset() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(10, 10);
			doc.FontSize = 96;
			doc.Transform.Rotate(60, 302, 396);
			doc.Pos.String = "302 396";
			doc.AddText("Angled");
			doc.FrameRect();
			doc.Transform.Reset();
			doc.Pos.String = "302 396";
			doc.AddText("Reset");
			doc.FrameRect();
			doc.Save("transformreset.pdf");
			// End Part:
		}
		// File End:

		// Example code for Rotate Function of XTransform Class for ABCpdf .NET
		// 
		// Here we add a number of chunks of text rotated at different angles about the middle
		// of the document.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\rotate.htm
		public static void Ex5_abcpdf_xtransformrotate() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 48;
			doc.TextStyle.Indent = 48;
			for (int i = 1; i <= 8; i++) {
				int angle = i * 45;
				doc.Pos.String = "302 396";
				doc.Transform.Reset();
				doc.Transform.Rotate(angle, 302, 396);
				doc.AddText($"Rotated {angle}");
			}
			doc.Save("rotate.pdf");
			// End Part:
		}
		// File End:

		// Example code for Skew Function of XTransform Class for ABCpdf .NET
		// 
		// Here we draw two rectangles into our document. The black rectangle is drawn before
		// the skew operation and the red one is drawn after it.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\skew.htm
		public static void Ex5_abcpdf_xtransformskew() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Width = 200;
			doc.Rect.Height = 250;
			doc.Rect.Position(20, 20);
			doc.Width = 20;
			doc.FrameRect();
			doc.Transform.Skew(1.5, 1.5, 20, 20);
			doc.Color.String = "255 0 0"; // red
			doc.FrameRect();
			doc.Save("transformskew.pdf");
			// End Part:
		}
		// File End:

		// Example code for Translate Function of XTransform Class for ABCpdf .NET
		// 
		// Here we draw two rectangles into our document. The black rectangle is drawn before
		// the translation operation and the red one is drawn after it.
		//
		// File Start: True .\5-abcpdf\xtransform\1-methods\translate.htm
		public static void Ex5_abcpdf_xtransformtranslate() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Width = 200;
			doc.Rect.Height = 250;
			doc.Rect.Position(100, 100);
			doc.Width = 20;
			doc.FrameRect();
			doc.Transform.Translate(200, 200);
			doc.Color.String = "255 0 0"; // red
			doc.FrameRect();
			doc.Save("transformtranslate.pdf");
			// End Part:
		}
		// File End:

		// Example code for AngleUnit Property of XTransform Class for ABCpdf .NET
		//
		// File Start: True .\5-abcpdf\xtransform\2-properties\angleunit.htm
		public static void Ex5_abcpdf_xtransformangleunit() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.TextStyle.HPos = 0.5;
			doc.TextStyle.VPos = 0.5;
			doc.Transform.Rotate(90, doc.Rect.Width / 2, doc.Rect.Height / 2);
			doc.TextStyle.Underline = true;
			doc.AddText("Hello World rotated by 90 degrees");
			doc.Page = doc.AddPage();
			doc.Transform.AngleUnit = WebSupergoo.ABCpdf14.XTransform.AngleUnitType.Radians;
			doc.Transform.Rotate(-1 * Math.PI / 2, doc.Rect.Width / 2, doc.Rect.Height / 2);
			doc.AddText("Hello World rotated back by PI/2 radians");
			doc.Save("transformrotate.pdf");
			// End Part:
		}
		// File End:

		// Example code for Load Function of Eof Class for ABCpdf .NET
		// 
		// The following code tests to see if any of the revisions in a document are hybrid.
		//
		// File Start: True .\6-abcpdf.objects\2-objectsoup.eof\1-methods\01-load.htm
		public static void Ex6_abcpdf_objects_2_objectsoup_eof01_load() {
			// Part: 1 of 1
			bool isHybrid = false;
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			using (var eof = new ObjectSoup.Eof()) {
				eof.Load(doc.ObjectSoup);
				var xref = eof.XRef;
				while (xref != null) {
					if (xref.Type == ObjectSoup.XRefType.Hybrid) {
						isHybrid = true;
						break;
					}
					xref = xref.Prev;
				}
			}
			// do something with isHybrid
			// End Part:
		}
		// File End:

		// Example code for GetEmbeddedFiles Function of Catalog Class for ABCpdf .NET
		// 
		// The example below show how to extract all the files embedded in a PDF portfolio.
		// See the FileSpecification constructor for the creation of portfolios.
		//
		// File Start: True .\6-abcpdf.objects\catalog\1-methods\getembeddedfiles.htm
		public static void Ex6_abcpdf_objects_cataloggetembeddedfiles() {
			// Part: 1 of 1
			using var doc = new Doc();
			var ro = new XReadOptions();
			ro.OpenPortfolios = false;
			doc.Read("../Rez/Portfolio1.pdf", ro);
			var files = doc.ObjectSoup.Catalog.GetEmbeddedFiles();
			foreach (var pair in files) {
				var fileSpec = pair.Value;
				fileSpec.Rationalize();
				var file = fileSpec.EmbeddedFile;
				if ((file != null) && (file.Decompress())) {
					string name = "Portfolio_" + fileSpec.Uri;
					string path = name;
					File.WriteAllBytes(path, file.GetData());
				}
			}
			// End Part:
		}
		// File End:

		// Example code for Metadata Property of Catalog Class for ABCpdf .NET
		// 
		// This example shows how to insert document properties. Document properties can be
		// viewed from Acrobat Reader and most commonly provide information on the document
		// origin.
		// 
		// Complex XMP metadata can be constructed using the Adobe XMP Toolkit. However in
		// most cases you will only want simple metadata so you can use the use the standard
		// Metadata object properties.
		// 
		//  Here we load an existing PDF and set the Title and Author.
		//
		// File Start: True .\6-abcpdf.objects\catalog\2-properties\metadata.htm
		public static void Ex6_abcpdf_objects_catalogmetadata() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/book.pdf");
			var md = doc.ObjectSoup.Catalog.Metadata;
			if (md == null) {
				md = new Metadata(doc.ObjectSoup);
				doc.ObjectSoup.Catalog.Metadata = md;
			}
			md.InfoSubject = "Finn Family Moomintroll";
			md.InfoAuthor = "Tove Jansson";
			doc.Save("metadata.pdf");
			// End Part:
		}
		// File End:

		// Example code for Gamma Property of ColorSpace Class for ABCpdf .NET
		// 
		// In this example we show how to use the Gamma property with a calibrated grayscale
		// color space.
		//
		// File Start: True .\6-abcpdf.objects\colorspace\2-properties\gamma.htm
		public static void Ex6_abcpdf_objects_colorspacegamma() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Rect.Inset(50, 50);
			var cs = new ColorSpace(doc.ObjectSoup, ColorSpaceType.CalGray);
			((NumAtom)cs.Gamma).Real = 1.2;
			doc.ColorSpace = cs.ID;
			doc.Color.SetComponents(0.9); // gray
			doc.AddOval(true);
			doc.Save("examplecalgraycolorspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for WhitePoint Property of ColorSpace Class for ABCpdf .NET
		// 
		// In this example we show how to use the WhitePoint and BlackPoint with a calibrated
		// RGB color space.
		//
		// File Start: True .\6-abcpdf.objects\colorspace\2-properties\whitepoint.htm
		public static void Ex6_abcpdf_objects_colorspacewhitepoint() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Width = 80;
			doc.Rect.Inset(50, 50);
			var cs = new ColorSpace(doc.ObjectSoup, ColorSpaceType.CalRGB);
			cs.WhitePoint.SetComponents(0.9, 1.0, 1.1);
			cs.BlackPoint = XColor.FromComponents(0.1, 0.1, 0.1);
			doc.ColorSpace = cs.ID;
			doc.Color.SetComponents(0.9, 0.1, 0.1); // red
			doc.AddOval(true);
			doc.Save("examplecalrgbcolorspace.pdf");
			// End Part:
		}
		// File End:

		// Example code for FileSpecification Function of FileSpecification Class for ABCpdf
		// .NET
		// 
		// The example below shows how to combine a set of PDF documents into a portfolio. See
		// the Catalog.GetEmbeddedFiles function for how to extract files from a portfolio.
		//
		// File Start: True .\6-abcpdf.objects\filespecification\1-methods\filespecification.htm
		public static void Ex6_abcpdf_objects_filespecificationfilespecification() {
			// Part: 1 of 1
			string[] files = {
				"../Rez/SignedDocument.pdf",
				"../Rez/spaceshuttle.pdf",
				"../Rez/Authorization.pdf",
				"../Rez/Portfolio1.pdf",
			};
			using var doc = new Doc();
			var fileSpecs = new List<Tuple<string, FileSpecification>>();
			foreach (string file in files) {
				byte[] data = null;
				using (var subDoc = new Doc()) {
					subDoc.Read(file);
					data = subDoc.GetData();
				}
				var embedFile = new EmbeddedFile(doc.ObjectSoup, data);
				embedFile.CompressFlate();
				var fileSpec = new FileSpecification(doc.ObjectSoup);
				fileSpec.EmbeddedFile = embedFile;
				fileSpec.Uri = file;
				string name = Path.GetFileName(file);
				fileSpecs.Add(new Tuple<string, FileSpecification>(name, fileSpec));
			}
			fileSpecs.Sort((a, b) => a.Item1.CompareTo(b.Item1));
			// we do not really need to delete existing files but we do so as an example
			doc.SetInfo(doc.Root, "/Names*/EmbeddedFiles*/Names:Del", "");
			foreach (var fileSpec in fileSpecs) {
				doc.SetInfo(doc.Root, "/Names*/EmbeddedFiles*/Names*[]:Text", fileSpec.Item1);
				doc.SetInfo(doc.Root, "/Names*/EmbeddedFiles*/Names*[]:Ref", fileSpec.Item2.ID);
			}
			// add placeholder text
			doc.AddText("This is a Adobe Acrobat Portfolio file. Please use Adobe Acrobat software to open it.");
			// save
			doc.ObjectSoup.Catalog.Version = 17;
			doc.SaveOptions.Linearize = false;
			doc.SetInfo(doc.Root, "/Collection*/D:Text", files[0]);
			doc.Save("createportfolio.pdf");
			// End Part:
		}
		// File End:

		// Example code for RuneWidths Property of FontObject Class for ABCpdf .NET
		// 
		// The example below shows how to add text on a curve and text flowing round a circle.
		//
		// File Start: True .\6-abcpdf.objects\fontobject\2-properties\runewidths.htm
		public static void Ex6_abcpdf_objects_fontobjectrunewidths() {
			// Part: 1 of 2
			string font = "Comic Sans MS";
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae...";
			string theTitle = "Commentarii de Bello Gallico";
			using var doc = new Doc();
			doc.FontSize = 36;
			doc.TextStyle.Kerning = XTextStyle.KerningType.None;
			doc.Font = doc.EmbedFont(font, LanguageType.Latin, false, true, false);
			// add some radial text in the top middle of the page
			double cx = doc.MediaBox.Width * 0.5;
			double cy = doc.MediaBox.Height * 0.6;
			double r = doc.MediaBox.Width * 0.3;
			CurvedText.AddRadial(doc, text, cx, cy, r, 225, true, false);
			// add some curved text to a rectangle
			double width = doc.MeasureText(theTitle);
			doc.Rect.SetRect(100, 100, width, doc.FontSize * 1.5);
			doc.FrameRect();
			CurvedText2.AddCurved(doc, theTitle);
			// save
			doc.Save("ExampleCurvedText2.pdf");
			// End Part:
		}
		// Part: 2 of 2
		class CurvedText2 {
			public static void AddCurved(Doc doc, string text) {
				double halfWidth = doc.Rect.Width / 2;
				double height = doc.Rect.Height - doc.TextStyle.Size;
				double radius = ((halfWidth * halfWidth) + (height * height)) / (2 * height);
				double centerX = doc.Rect.Left + halfWidth;
				double centerY = doc.Rect.Bottom + radius + doc.TextStyle.Size;
				double alpha = Math.Asin(halfWidth / radius) - Math.PI;
				AddRadial(doc, text, centerX, centerY, radius, RadiansToDegrees(alpha), true, false);
			}
		
			public static void AddRadial(Doc doc, string text, double centerX, double centerY, double radius, double startAngleDegrees, bool inside, bool clockwise) {
				var font = doc.ObjectSoup[doc.Font] as FontObject;
				var widths = font.RuneWidths;
				int n = text.Length;
				double a = DegreesToRadians(startAngleDegrees);
				double fontWidthToRadians = doc.TextStyle.Size / (radius * 1000);
				doc.Rect.String = doc.MediaBox.String;
				for (int i = 0; i < n; i++) {
					// work out position
					double x = centerX + (Math.Sin(a) * radius);
					double y = centerY + (Math.Cos(a) * radius);
					// add a character
					doc.Pos.X = x;
					doc.Pos.Y = y;
					doc.Transform.Reset();
					double charRotation = inside ? RadiansToDegrees(-a) + 180 : RadiansToDegrees(-a);
					doc.Transform.Rotate(charRotation, x, y);
					doc.AddText(text[i].ToString());
					// increment angle
					var rune = new XRune(text[i]);
					double da = (double)widths[rune] * fontWidthToRadians;
					a += clockwise ? da : -da;
				}
				doc.Transform.Reset();
			}
		
			private static double DegreesToRadians(double degrees) {
				return degrees * Math.PI / 180;
			}
		
			private static double RadiansToDegrees(double radians) {
				return radians * 180 / Math.PI;
			}
		}
		// End Part:
		// File End:

		// Example code for Widths Property of FontObject Class for ABCpdf .NET
		// 
		// The example below shows how to add text on a curve and text flowing round a circle.
		//
		// File Start: True .\6-abcpdf.objects\fontobject\2-properties\widths.htm
		public static void Ex6_abcpdf_objects_fontobjectwidths() {
			// Part: 1 of 2
			string font = "Comic Sans MS";
			string text = "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae...";
			string theTitle = "Commentarii de Bello Gallico";
			using var doc = new Doc();
			doc.FontSize = 36;
			doc.TextStyle.Kerning = XTextStyle.KerningType.None;
			doc.Font = doc.EmbedFont(font, LanguageType.Latin, false, true, false);
			// add some radial text in the top middle of the page
			double cx = doc.MediaBox.Width * 0.5;
			double cy = doc.MediaBox.Height * 0.6;
			double r = doc.MediaBox.Width * 0.3;
			CurvedText.AddRadial(doc, text, cx, cy, r, 225, true, false);
			// add some curved text to a rectangle
			double width = doc.MeasureText(theTitle);
			doc.Rect.SetRect(100, 100, width, doc.FontSize * 1.5);
			doc.FrameRect();
			CurvedText.AddCurved(doc, theTitle);
			// save
			doc.Save("ExampleCurvedText.pdf");
			// End Part:
		}
		// Part: 2 of 2
		class CurvedText {
			public static void AddCurved(Doc doc, string text) {
				double halfWidth = doc.Rect.Width / 2;
				double height = doc.Rect.Height - doc.TextStyle.Size;
				double radius = ((halfWidth * halfWidth) + (height * height)) / (2 * height);
				double centerX = doc.Rect.Left + halfWidth;
				double centerY = doc.Rect.Bottom + radius + doc.TextStyle.Size;
				double alpha = Math.Asin(halfWidth / radius) - Math.PI;
				AddRadial(doc, text, centerX, centerY, radius, RadiansToDegrees(alpha), true, false);
			}
		
			public static void AddRadial(Doc doc, string text, double centerX, double centerY, double radius, double startAngleDegrees, bool inside, bool clockwise) {
				var font = doc.ObjectSoup[doc.Font] as FontObject;
				var widths = font.Widths;
				int n = text.Length;
				double a = DegreesToRadians(startAngleDegrees);
				double fontWidthToRadians = doc.TextStyle.Size / (radius * 1000);
				doc.Rect.String = doc.MediaBox.String;
				for (int i = 0; i < n; i++) {
					// work out position
					double x = centerX + (Math.Sin(a) * radius);
					double y = centerY + (Math.Cos(a) * radius);
					// add a character
					doc.Pos.X = x;
					doc.Pos.Y = y;
					doc.Transform.Reset();
					double charRotation = inside ? RadiansToDegrees(-a) + 180 : RadiansToDegrees(-a);
					doc.Transform.Rotate(charRotation, x, y);
					doc.AddText(text[i].ToString());
					// increment angle
					double da = Convert.ToDouble(widths[text[i]]) * fontWidthToRadians;
					a += clockwise ? da : -da;
				}
				doc.Transform.Reset();
			}
		
			private static double DegreesToRadians(double degrees) {
				return degrees * Math.PI / 180;
			}
		
			private static double RadiansToDegrees(double radians) {
				return radians * 180 / Math.PI;
			}
		}
		// End Part:
		// File End:

		// Example code for GetBitmap Function of Page Class for ABCpdf .NET
		// 
		// The following example shows how to use this method to generate various types of drop
		// shadows.
		//
		// File Start: True .\6-abcpdf.objects\page\1-methods\getbitmap.htm
		public static void Ex6_abcpdf_objects_pagegetbitmap() {
			// Part: 1 of 2
			using var doc = new Doc();
			// light blue background
			doc.Color.SetCmyk(50, 0, 0, 0);
			doc.FillRect();
			doc.Color.SetRgb(0, 0, 0);
			doc.Rect.Inset(20, 20);
			doc.Rect.Pin = XRect.Corner.TopLeft;
			doc.Rect.Height = doc.Rect.Height / 5;
			// set up styles
			doc.TextStyle.Size = 72;
			double shift = doc.TextStyle.Size * 0.1;
			var pink = new XColor();
			pink.SetRgb(255, 128, 128);
			var gray = new XColor();
			gray.SetRgb(128, 128, 128);
			var blue = new XColor();
			blue.SetRgb(0, 0, 255);
			// add text content
			AddDropShadow(doc, doc.AddText("Sharp Shadow"), 0, shift, -shift, gray);
			doc.Rect.Move(0, -doc.Rect.Height);
			AddDropShadow(doc, doc.AddText("Blurred Shadow"), 1, shift, -shift, gray);
			doc.Rect.Move(0, -doc.Rect.Height);
			AddDropShadow(doc, doc.AddText("Pink Shadow"), 1, shift, -shift, pink);
			doc.Rect.Move(0, -doc.Rect.Height);
			// add drawn content
			doc.Transform.Magnify(0.5, 0.5, 0, 0);
			doc.Transform.Translate(50, 0);
			string star = "124 158 300 700 476 158 15 493 585 493 124 158";
			doc.Width = 20;
			doc.Color.String = "255 0 0";
			AddDropShadow(doc, doc.AddPoly(star, false), 3, shift, -shift, blue);
			doc.Save("dropshadows.pdf");
			// End Part:
			// Part: 2 of 2
			void AddDropShadow(Doc doc, int id, double gaussianBlurRadius, double shadowHorizontalShift, double shadowVerticalShift, XColor shadowColor) {
				string rect = doc.Rect.String;
				string transform = doc.Transform.String;
				string color = doc.Color.String;
				double dpiX = doc.Rendering.DotsPerInchX;
				double dpiY = doc.Rendering.DotsPerInchY;
				bool saveAlpha = doc.Rendering.SaveAlpha;
				int docLayer = doc.Layer;
				try {
					doc.Rendering.DotsPerInch = 72;
					doc.Rendering.SaveAlpha = true;
					var layer = doc.ObjectSoup[id] as Layer;
					var page = doc.ObjectSoup[doc.Page] as Page;
					var bm = page.GetBitmap([ layer ]);
					// expand image if blur may move content off edges
					int border = 0;
					if (gaussianBlurRadius > 0) {
						// we extend our border out two standard deviations
						border = (int)Math.Round(Math.Abs(gaussianBlurRadius)) * 2;
						int borders = border * 2;
						var larger = new Bitmap(bm.Width + borders, bm.Height + borders, bm.PixelFormat);
						larger.SetResolution(bm.HorizontalResolution, bm.VerticalResolution);
						using (var graphics = Graphics.FromImage(larger)) {
							graphics.DrawImage(bm, new Point(border, border));
							if (bm != null)
								bm.Dispose();
							bm = larger;
						}
					}
					doc.Transform.Reset();
					doc.Rect.String = layer.Rect.String;
					doc.Rect.Inset(-border, -border);
					doc.Rect.Move(shadowHorizontalShift, shadowVerticalShift);
					doc.Layer = docLayer + 1;
					int pid = doc.AddImageBitmap(bm, true);
					bm.Dispose();
					// Here we set the base image to be one pixel of an appropriate color.
					// This is what will determine the shadow color.
					var img = doc.ObjectSoup[pid] as ImageLayer;
					var pm = img.PixMap;
					pm.ClearData(); // this will remove any compression settings
					pm.SetData([ (byte)shadowColor.Red, (byte)shadowColor.Green, (byte)shadowColor.Blue ]);
					pm.Width = 1;
					pm.Height = 1;
					// The alpha channel is held as a separate soft mask and this is what will
					// determine the shape of the shadow. If required we blur it to give it
					// soft edges.
					var alpha = pm.SMask;
					if (gaussianBlurRadius > 0) {
						using (EffectOperation effect = new EffectOperation("Gaussian Blur")) {
							effect.Parameters["Radius"].Value = gaussianBlurRadius;
							effect.Apply(alpha);
						}
					}
				}
				finally {
					doc.Rect.String = rect;
					doc.Transform.String = transform;
					doc.Color.String = color;
					doc.Rendering.DotsPerInchX = dpiX;
					doc.Rendering.DotsPerInchY = dpiY;
					doc.Rendering.SaveAlpha = saveAlpha;
					doc.Layer = docLayer;
				}
			}
			// End Part:
		}
		// File End:

		// Example code for MakeFormXObject Function of Page Class for ABCpdf .NET
		// 
		// This example shows how to take a page, convert it into a separate drawing object
		// and then draw it, scaled, onto the page it came from.
		//
		// File Start: True .\6-abcpdf.objects\page\1-methods\makeformxobject.htm
		public static void Ex6_abcpdf_objects_pagemakeformxobject() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/HyperX.pdf");
			var page1 = doc.ObjectSoup[doc.Page] as Page;
			var form = page1.MakeFormXObject();
			doc.Transform.Magnify(0.5, 0.5, 0, 0);
			doc.Page = doc.AddPage();
			var page2 = doc.ObjectSoup[doc.Page] as Page;
			string name = page2.AddResource(form, "XObject", "Iabc");
			// Here we create our own layer for the purposes of the demonstration.
			// However a simpler approach would be to use Doc.AddXObject.
			var layer = new StreamObject(doc.ObjectSoup);
			layer.SetText(String.Format("q {0} cm /{1} Do Q ", doc.Transform.ToString(), name));
			page2.AddLayer(layer);
			doc.Save("exampleformxobject.pdf");
			// End Part:
		}
		// File End:

		// Example code for VectorizeText Function of Page Class for ABCpdf .NET
		//
		// File Start: True .\6-abcpdf.objects\page\1-methods\vectorizetext.htm
		public static void Ex6_abcpdf_objects_pagevectorizetext() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.FontSize = 96;
			doc.AddText("Hello World");
			foreach (var page in doc.ObjectSoup.Catalog.Pages.GetPageArrayAll())
				page.VectorizeText();
			doc.Save("VectorizedText.pdf");
			// End Part:
		}
		// File End:

		// Example code for Rotation Property of Page Class for ABCpdf .NET
		// 
		// This example shows how to use the Rotation property to determine how to add a PDF
		// page - which may be rotated - to a portrait PDF page.
		//
		// File Start: True .\6-abcpdf.objects\page\2-properties\rotation.htm
		public static void Ex6_abcpdf_objects_pagerotation() {
			// Part: 1 of 1
			using var doc = new Doc();
			using var src = new Doc();
			src.Read("landscape.pdf");
			int rotation = ((Page)src.ObjectSoup[src.Page]).Rotation;
			bool landscape = src.MediaBox.Width > src.MediaBox.Height;
			doc.Page = doc.AddPage();   // output is always in portrait
			if (landscape) {
				switch (rotation) {
			
					case 0:
					case 90:
						doc.Transform.Rotate(270, 0, 0);
						doc.Transform.Translate(0, doc.MediaBox.Height);
						break;
					case 180:
					case 270:
						doc.Transform.Rotate(90, 0, 0);
						doc.Transform.Translate(doc.MediaBox.Width, 0);
						break;
				}
				doc.Rect.SetRect(0, 0, doc.MediaBox.Height, doc.MediaBox.Width);
			}
			else {
				switch (rotation) {
					case 90:
					case 180:
						doc.Transform.Rotate(180, 0, 0);
						doc.Transform.Translate(doc.MediaBox.Width, doc.MediaBox.Height);
						break;
				}
			}
			doc.AddImageDoc(src, 1, null);
			doc.Save("addtoportrait.pdf");
			// End Part:
		}
		// File End:

		// Example code for Thumbnail Property of Page Class for ABCpdf .NET
		// 
		// This example shows how to create and embed thumbnails in a PDF document.
		//
		// File Start: True .\6-abcpdf.objects\page\2-properties\thumbnail.htm
		public static void Ex6_abcpdf_objects_pagethumbnail() {
			// Part: 1 of 1
			using var doc = new Doc();
			using var src = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			doc.Rendering.DotsPerInch = 18;
			var pages = doc.ObjectSoup.Catalog.Pages.GetPageArrayAll();
			foreach (var page in pages) {
				doc.Page = page.ID;
				using (var xi = XImage.FromData(doc.Rendering.GetData(".jpg"), null))
					page.Thumbnail = PixMap.FromXImage(doc.ObjectSoup, xi);
			}
			doc.Save("embedthumbnails.pdf");
			// End Part:
		}
		// File End:

		// Example code for GetBitmap Function of PixMap Class for ABCpdf .NET
		// 
		// This example shows how to extract all the page thumbnails (if they exist) from a
		// document. See also the PDFSurgeon example project for another example.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\getbitmap.htm
		public static void Ex6_abcpdf_objects_pixmapgetbitmap() {
			// Part: 1 of 1
			using var doc = new Doc();
			using var src = new Doc();
			doc.Read("../Rez/embedthumbnails.pdf");
			doc.Rendering.DotsPerInch = 18;
			var pages = doc.ObjectSoup.Catalog.Pages.GetPageArrayAll();
			foreach (var page in pages) {
				if (page.Thumbnail == null)
					continue;
				using var bm = page.Thumbnail.GetBitmap();
				bm.Save($"embedthumbnails{page.Thumbnail.ID}.jpg");
			}
			// End Part:
		}
		// File End:

		// Example code for Recolor Function of PixMap Class for ABCpdf .NET
		// 
		// Here we change all the images in a document to CMYK.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\recolor.htm
		public static void Ex6_abcpdf_objects_pixmaprecolor() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			List<PixMap> theList = new List<PixMap>();
			// find all the PixMap objects in the soup
			foreach (IndirectObject obj in doc.ObjectSoup) {
				var p = obj as PixMap;
				if (p != null)
					theList.Add(p);
			}
			// add our destination color space
			var cs = new ColorSpace(doc.ObjectSoup);
			cs.IccProfile = new IccProfile(doc.ObjectSoup, "../Rez/abccmyk.icc");
			// convert images to our color space
			for (int i = 0; i < theList.Count; i++) {
				var p = theList[i];
				p.Recolor(cs, RenderingIntent.Perceptual);
				p.CompressJpeg(75);
			}
			doc.Save("pixmaprecolor.pdf");
			// End Part:
		}
		// File End:

		// Example code for Resize Function of PixMap Class for ABCpdf .NET
		// 
		// Here we resize all the images in a document to a quarter of their previous resolution.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\resize.htm
		public static void Ex6_abcpdf_objects_pixmapresize() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			foreach (IndirectObject io in doc.ObjectSoup) {
				if (io is PixMap) {
					var pm = (PixMap)io;
					pm.Realize(); // eliminate indexed color images
					pm.Resize(pm.Width / 4, pm.Height / 4);
				}
			}
			doc.Save("pixmapresize.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetAlpha Function of PixMap Class for ABCpdf .NET
		// 
		// Here we add an image without transparency and then, at a position down and to the
		// right, with 50% transparency.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\setalpha.htm
		public static void Ex6_abcpdf_objects_pixmapsetalpha() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Pin = XRect.Corner.TopLeft;
			doc.Rect.Magnify(0.5, 0.5);
			string path = "../mypics/mypic.jpg";
			doc.AddImageFile(path, 1);
			doc.Rect.Move(doc.Rect.Width, -doc.Rect.Height);
			int i = doc.AddImageFile(path, 1);
			var im = (ImageLayer)doc.ObjectSoup[i];
			im.PixMap.SetAlpha(128);
			doc.Save("pixmapsetalpha.pdf");
			// End Part:
		}
		// File End:

		// Example code for SetChromakey Function of PixMap Class for ABCpdf .NET
		// 
		// Here we add an image over the top of a green background. We use a chromakey to make
		// black and near-black colors transparent.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\setchromakey.htm
		public static void Ex6_abcpdf_objects_pixmapsetchromakey() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Inset(20, 20);
			doc.Color.String = "0 255 0";
			doc.FillRect();
			string path = "../mypics/mypic.jpg";
			int i = doc.AddImageFile(path, 1);
			var im = (ImageLayer)doc.ObjectSoup[i];
			im.PixMap.SetChromakey("0 50 0 50 0 50");
			doc.Save("pixmapsetchromakey.pdf");
			// End Part:
		}
		// File End:

		// Example code for ToGrayscale Function of PixMap Class for ABCpdf .NET
		// 
		// Here we add an image in its natural color space and then, at a position down and
		// to the right, converted to grayscale.
		//
		// File Start: True .\6-abcpdf.objects\pixmap\1-methods\tograyscale.htm
		public static void Ex6_abcpdf_objects_pixmaptograyscale() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Rect.Pin = XRect.Corner.TopLeft;
			doc.Rect.Magnify(0.5, 0.5);
			string path = "../mypics/mypic.jpg";
			doc.AddImageFile(path, 1);
			doc.Rect.Move(doc.Rect.Width, -doc.Rect.Height);
			int i = doc.AddImageFile(path, 1);
			var im = (ImageLayer)doc.ObjectSoup[i];
			im.PixMap.ToGrayscale();
			doc.Save("pixmaptograyscale.pdf");
			// End Part:
		}
		// File End:

		// Example code for AddLTV Function of Signature Class for ABCpdf .NET
		// 
		// In this example, for simplicity, we use the plain text password overload. However
		// to improve application security you may wish to use the SecureString overload.
		//
		// File Start: True .\6-abcpdf.objects\signature\1-methods\addltv.htm
		public static void Ex6_abcpdf_objects_signatureaddltv() {
			// Part: 1 of 1
			using var doc = new Doc();
			X509Certificate2 cert = Certificates.GetFromStore(); // User-defined function
			if (cert == null)
				return; // no certificate found
			doc.Page = doc.AddPage();
			var sig = doc.Form.AddSignature(new XRect("340 160 540 220"), "Signature");
			sig.Sign(cert, false);
			sig.Commit();
			sig = (Signature)doc.Form.Fields["Signature"];
			sig.TimestampServiceUrl = new Uri("http://timestamp.comodoca.com");
			sig.AddLTV(new Oid(CryptoConfig.MapNameToOID("SHA256")));
			doc.Save("SignedLTV.pdf");
			// End Part:
		}
		// File End:

		// Example code for Sign Method of Signature Class for ABCpdf .NET
		// 
		// If you would like to make your signature compliant to a specific compliance level
		// see Compliance.
		// 
		// Read a document and sign a signature field embedded within that document. Before
		// signing, we specify a location and a reason why the document is being digitally signed.
		// 
		// In this example, for simplicity, we use the plain text password overload. However
		// to improve application security you may wish to use the SecureString overload.
		//
		// File Start: True .\6-abcpdf.objects\signature\1-methods\sign.htm
		public static void Ex6_abcpdf_objects_signaturesign() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/Authorization.pdf");
			Signature theSig = (Signature)doc.Form["Signature"];
			theSig.Location = "Washington";
			theSig.Reason = "Schedule Agreed";
			theSig.Sign("../Rez/JohnSmith.pfx", "1234");
			doc.Save("Signed.pdf");
			// End Part:
		}
		// File End:

		// Example code for Validate Function of Signature Class for ABCpdf .NET
		//
		// File Start: True .\6-abcpdf.objects\signature\1-methods\validate.htm
		public static void Ex6_abcpdf_objects_signaturevalidate() {
			// Part: 1 of 2
			// Validate using certificate files
			using (var doc = new Doc()) {
				doc.Read("../Rez/SignedDocument.pdf");
				var theCerts = "../Rez/JohnSmith.cer".Split([ ';' ]);
				var theSig = (Signature)doc.Form["Signature"];
				if ((theSig.Validate(theCerts)) && (!theSig.IsModified))
					doc.AddText($"Signature valid at {DateTime.Now}");
				doc.Save("SignedAndValidated1.pdf");
			}
			// End Part:
			// Part: 2 of 2
			// Validate using the Windows Certificate Store
			using (var doc = new Doc()) {
				doc.Read("../Rez/SignedDocument.pdf");
				using var theStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
				theStore.Open(OpenFlags.ReadOnly);
				var theSig = (Signature)doc.Form["Signature"];
				if ((theSig.Validate(theStore.Certificates)) && (!theSig.IsModified))
					doc.AddText($"Signature valid at {DateTime.Now}");
				doc.Save("SignedAndValidated2.pdf");
			}
			// End Part:
		}
		// File End:

		// Example code for CompliancePades Property of Signature Class for ABCpdf .NET
		// 
		// The following example shows how to sign a PAdES_B_LTA signature with a certificate
		// with its private key on a Hardware Security Module (HSM).
		// 
		// Usually when the HSM is inserted any certificates are automatically inserted into
		// the Windows Certificate Store. The signing certificate can then be obtained via the
		// .NET X509Store interface.
		// 
		// To silently sign using a certificate on an HSM you will need to provide the password
		// as a SecureString.
		//
		// File Start: True .\6-abcpdf.objects\signature\2-properties\compliancepades.htm
		public static void Ex6_abcpdf_objects_signaturecompliancepades() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/BlankSignature.pdf");
			var sig = (Signature)doc.Form.Fields["Signature1"];
			sig.Reason = "Final Version";
			sig.Location = "New York";
			sig.TimestampServiceUrl = new Uri("http://timestamp.digicert.com");
			sig.CompliancePades = Signature.PadesLevel.PAdES_B_LTA;
			X509Certificate2 cert = Certificates.GetFromStore(); // User-defined function
			if (cert == null)
				return; // no certificate found
			var pwd = new SecureString();
			foreach (char c in "password".ToCharArray())
				pwd.AppendChar(c);
			sig.Sign(cert, pwd, new Oid(CryptoConfig.MapNameToOID("SHA256")));
			doc.Save("SignedDoc.pdf");
			// End Part:
		}
		// File End:

		// Example code for CustomSigner Property of Signature Class for ABCpdf .NET
		// 
		// The following example shows how an external delegate might be used.
		//
		// File Start: True .\6-abcpdf.objects\signature\2-properties\customsigner.htm
		public static void Ex6_abcpdf_objects_signaturecustomsigner() {
			// Part: 1 of 2
			X509Certificate2 cert = FindCertificate();
			if (cert == null)
				return; // no certificate found
			using var doc = new Doc();
			doc.Read("../mypics/BlankSignature.pdf");
			var sig = (Signature)doc.Form.Fields["Signature1"];
			sig.CustomSigner = ExternalSigner;
			sig.Reason = "Test External Signing";
			// Just use public certificate from file - i.e. do not obtain from registry
			var gs = new X509Certificate2("GlobalSign.cer");
			sig.Sign(gs, true, new Oid(CryptoConfig.MapNameToOID("SHA512")), X509IncludeOption.EndCertOnly);
			cert = FindCertificate();
			// here we commit and validate to ensure the data is correct
			sig = (Signature)doc.Form.Fields["Signature1"]; // signature must be re-retrieved after a Commit/Save
			if (!sig.Validate())
				throw new Exception("Signing failed!");
			doc.Save("SignedDoc.pdf");
			// End Part:
			// Part: 2 of 2
			byte[] ExternalSigner(byte[] data) {
				var password = new SecureString(); // needs value
				var rsa = (RSACryptoServiceProvider)cert.PrivateKey;
				var cspParams = new CspParameters(1, rsa.CspKeyContainerInfo.ProviderName,
					rsa.CspKeyContainerInfo.UniqueKeyContainerName) {
					KeyPassword = password,
					Flags = CspProviderFlags.NoPrompt
				};
				var service = new RSACryptoServiceProvider(cspParams);
				return service.SignData(data, CryptoConfig.MapNameToOID("SHA512"));
			}
			X509Certificate2 FindCertificate() {
				string serial = "10 20 30 10 40 10 40 50 60 10 20 30"; // needs value
				X509Certificate2 cert = null;
				using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly | OpenFlags.MaxAllowed);
				var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false);
				if (certs.Count == 1) {
					cert = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false)[0];
					if (cert.PrivateKey is RSACryptoServiceProvider == false)
						cert = null;
				}
				return cert;
			}
			// End Part:
		}
		// File End:

		// Example code for CustomSigner2 Property of Signature Class for ABCpdf .NET
		// 
		// The following example shows how an external delegate might be used.
		//
		// File Start: True .\6-abcpdf.objects\signature\2-properties\customsigner2.htm
		public static void Ex6_abcpdf_objects_signaturecustomsigner2() {
			// Part: 1 of 2
			X509Certificate2 cert = FindCertificate();
			if (cert == null)
				return; // no certificate found
			using var doc = new Doc();
			doc.Read("../mypics/BlankSignature.pdf");
			var sig = (Signature)doc.Form.Fields["Signature1"];
			sig.CustomSigner2 = ExternalSigner;
			sig.Reason = "Test External Signing";
			// Just use public certificate from file - i.e. do not obtain from registry
			var gs = new X509Certificate2("GlobalSign.cer");
			sig.Sign(gs, true, new Oid(CryptoConfig.MapNameToOID("SHA512")), X509IncludeOption.EndCertOnly);
			// here we commit and validate to ensure the data is correct
			sig.Commit();
			sig = (Signature)doc.Form.Fields["Signature1"]; // signature must be re-retrieved after a Commit/Save
			if (!sig.Validate())
				throw new Exception("Signing failed!");
			doc.Save("SignedDoc.pdf");
			// End Part:
			// Part: 2 of 2
			byte[] ExternalSigner(byte[] data, Signature.State state) {
				var password = new SecureString(); // needs value
				var rsa = (RSACryptoServiceProvider)cert.PrivateKey;
				var cspParams = new CspParameters(1, rsa.CspKeyContainerInfo.ProviderName,
					rsa.CspKeyContainerInfo.UniqueKeyContainerName) {
					KeyPassword = password,
					Flags = CspProviderFlags.NoPrompt
				};
				var service = new RSACryptoServiceProvider(cspParams);
				return service.SignData(data, CryptoConfig.MapNameToOID("SHA512"));
			}
			X509Certificate2 FindCertificate() {
				string serial = "10 20 30 10 40 10 40 50 60 10 20 30"; // needs value
				X509Certificate2 cert = null;
				using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly | OpenFlags.MaxAllowed);
				var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false);
				if (certs.Count == 1) {
					cert = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false)[0];
					if (cert.PrivateKey is RSACryptoServiceProvider == false)
						cert = null;
				}
				return cert;
			}
			// End Part:
		}
		// File End:

		// Example code for FromContentStream Function of ArrayAtom Class for ABCpdf .NET
		// 
		// This example shows how to use the FromContentStream function to parse and display
		// a PDF content stream.
		//
		// File Start: True .\7-abcpdf.atoms\arrayatom\1-methods\fromcontentstream.htm
		public static void Ex7_abcpdf_atoms_arrayatomfromcontentstream() {
			// Part: 1 of 1
			var sb = new StringBuilder();
			using (var doc = new Doc()) {
				doc.Read("../Rez/spaceshuttle.pdf");
				var page = doc.ObjectSoup[doc.Page] as Page;
				var array = ArrayAtom.FromContentStream(page.GetContentData());
				int indent = 0;
				var indentPlus = new HashSet<string>([ "q", "BT" ]);
				var indentMinus = new HashSet<string>(["Q", "ET" ]);
				var items = OpAtom.Find(array);
				int index = 0;
				foreach (var pair in items) {
					string op = ((OpAtom)array[pair.Item2]).Text;
					// add indent to code
					if (indentMinus.Contains(op))
						indent--;
					for (int i = 0; i < indent; i++)
						sb.Append(" ");
					// write out the operators
					for (int i = index; i <= pair.Item2; i++) {
						if (i != index)
							sb.Append(" ");
						var item = array[i];
						// we write arrays out individually so that
						// we can override default cr lf behavior
						var itemArray = item as ArrayAtom;
						if (itemArray != null) {
							int n = itemArray.Count;
							sb.Append("[");
							for (int j = 0; j < n; j++) {
								sb.Append(itemArray[j].ToString());
								if (j != n - 1)
									sb.Append(" ");
							}
							sb.Append("]");
						}
						else {
							sb.Append(item.ToString());
						}
					}
					sb.AppendLine();
					if (indentPlus.Contains(op))
						indent++;
					index = pair.Item2 + 1;
				}
				// write out any atoms that are left over
				for (int i = index; i < array.Count; i++) {
					sb.Append(" ");
					sb.Append(array[i].ToString());
				}
			}
			using (var doc = new Doc()) {
				doc.Font = doc.AddFont("Courier");
				doc.Rect.Inset(20, 20);
				doc.AddText(sb.ToString());
				doc.Save("PageContents.pdf");
			}
			// End Part:
		}
		// File End:

		// Example code for Find Function of OpAtom Class for ABCpdf .NET
		// 
		// This example shows how to use the Array.FromContentStream function to parse a content
		// stream and then use the Find method to search for certain types of color operators
		// and replace them with other color operators. In this example this has the effect
		// of converting some black parts of the PDF to red. For a more comprehensive approach
		// one would use the ContentStreamOperation class to determine all content streams associated
		// with the pages in question.
		//
		// File Start: True .\7-abcpdf.atoms\opatom\1-methods\find.htm
		public static void Ex7_abcpdf_atoms_opatomfind() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			doc.RemapPages(new int[] { 1, 1 });
			doc.PageNumber = 2;
			var page = doc.ObjectSoup[doc.Page] as Page;
			var layers = page.GetLayers();
			using var st = new MemoryStream();
			foreach (var layer in layers) {
				if (!layer.Decompress())
					throw new Exception("Unable to decompress stream.");
				byte[] data = layer.GetData();
				st.Write(data, 0, data.Length);
				layer.CompressFlate();
			}
			var array = ArrayAtom.FromContentStream(st.ToArray());
			if (true) {
				var items = OpAtom.Find(array, [ "k" ]);
				foreach (var pair in items) { // make red
					var args = OpAtom.GetParameters(array, pair.Item2);
					if (args != null) {
						((NumAtom)args[0]).Real = 0;
						((NumAtom)args[1]).Real = 1;
						((NumAtom)args[2]).Real = 1;
						((NumAtom)args[3]).Real = 0;
					}
				}
			}
			if (true) {
				var items = OpAtom.Find(array, [ "rg" ]);
				foreach (var pair in items) { // make green
					var args = OpAtom.GetParameters(array, pair.Item2);
					if (args != null) {
						((NumAtom)args[0]).Real = 0;
						((NumAtom)args[1]).Real = 1;
						((NumAtom)args[2]).Real = 0;
					}
				}
			}
			byte[] arrayData = array.GetData();
			var so = new StreamObject(doc.ObjectSoup);
			so.SetData(arrayData, 1, arrayData.Length - 2);
			doc.SetInfo(page.ID, "/Contents:Del", "");
			page.AddLayer(so);
			doc.Save("ReplaceColors.pdf");
			// End Part:
		}
		// File End:

		// Example code for FrameNumber Property of ProcessingInfo Class for ABCpdf .NET
		// 
		// See the SwfImportOperation.Import method.
		// 
		//   
		// 
		//  Here we import the frame at 3.5 seconds.
		//
		// File Start: True .\8-abcpdf.operations\2-processinginfo\2-properties\framenumber.htm
		public static void Ex8_abcpdf_operations_2_processinginfoframenumber() {
			// Part: 1 of 1
			using var doc = new Doc();
			using (var operation = new SwfImportOperation()) {
				operation.Doc = doc;
				operation.ContentAlign = ContentAlign.Top;
				operation.ContentScaleMode = ContentScaleMode.ShowAll;
				operation.ProcessingObject += delegate (object sender, ProcessingObjectEventArgs e) {
					if (e.Info.SourceType == ProcessingSourceType.MultiFrameImage && e.Info.FrameNumber.HasValue)
						e.Info.FrameNumber = 1 + (long)(e.Info.FrameRate.Value * 3.5);
				};
				operation.Import("../Rez/ABCpdf.swf");
			}
			doc.Save("swf.pdf");
			// End Part:
		}
		// File End:

		// Example code for Recolor Function of RecolorOperation Class for ABCpdf .NET
		// 
		// Here we recolor one page out of a document. We pick up the ProcessingObject events
		// so that we can store the source color space for all the PixMap objects which are
		// processed. We do not want to convert CMYK pixmaps so we set the Cancel property to
		// true if we find these.
		// 
		// We then pick up the ProcessedObject events so that we can recompress the PixMap
		// objects after they have been recolored. We vary the recompression method used dependent
		// on the source color space and the size of the image.
		// 
		// Here we use standard delegates for backwards compatibility with older code. However
		// anonymous delegates will provide a more compact solution.
		//
		// File Start: True .\8-abcpdf.operations\3-recoloroperation\1-methods\recolor.htm
		public static void Ex8_abcpdf_operations_3_recoloroperationrecolor() {
			// Part: 1 of 2
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			MyOp.Recolor(doc, (Page)doc.ObjectSoup[doc.Page]);
			doc.Save("RecolorOperation.pdf");
			// End Part:
		}
		// Part: 2 of 2
		class MyOp {
			public static void Recolor(Doc doc, Page page) {
				var op = new RecolorOperation();
				op.DestinationColorSpace = new ColorSpace(doc.ObjectSoup, ColorSpaceType.DeviceGray);
				op.ConvertAnnotations = false;
				op.ProcessingObject += Recoloring;
				op.ProcessedObject += Recolored;
				op.Recolor(page);
			}
		
			public static void Recoloring(object sender, ProcessingObjectEventArgs e) {
				var pm = e.Object as PixMap;
				if (pm != null) {
					ColorSpaceType cs = pm.ColorSpaceType;
					if (cs == ColorSpaceType.DeviceCMYK)
						e.Cancel = true;
					e.Tag = cs;
				}
			}
		
			public static void Recolored(object sender, ProcessedObjectEventArgs e) {
				if (e.Successful) {
					var pm = e.Object as PixMap;
					if (pm != null) {
						ColorSpaceType cs = (ColorSpaceType)e.Tag;
						if (pm.Width > 1000)
							pm.CompressJpx(30);
						else if (cs == ColorSpaceType.DeviceRGB)
							pm.CompressJpeg(30);
						else
							pm.Compress(); // Flate
					}
				}
			}
		}
		// End Part:
		// File End:

		// Example code for IccCmyk Property of RecolorOperation Class for ABCpdf .NET
		// 
		// The following example illustrates how one could convert the first page of a PDF document
		// so all colors and color spaces are in Lab format
		// 
		// Any DeviceCMYK color values are run though the CoatedGRACoL2006 ICC color profile
		// for conversion to Lab.
		// 
		// We assign 96 to the Type0SampleCount which means that continuous shading functions
		// are converted to Type 0 (sample based) functions with 96 samples.
		//
		// File Start: True .\8-abcpdf.operations\3-recoloroperation\2-properties\icccmyk.htm
		public static void Ex8_abcpdf_operations_3_recoloroperationicccmyk() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/SpaceShuttlePage6.pdf");
			var cs = new ColorSpace(doc.ObjectSoup, ColorSpaceType.Lab);
			using (var op = new RecolorOperation()) {
				op.DestinationColorSpace = cs;
				op.IccCmyk = "../Rez/EuroscaleCoated.icc";
				op.Type0SampleCount = 96;
				op.RenderingIntent = RenderingIntent.Perceptual;
				op.Recolor((Page)doc.ObjectSoup[doc.Page]);
			}
			doc.Save("RecolorToLab.pdf");
			// End Part:
		}
		// File End:

		// Example code for Import Function of XpsImportOperation Class for ABCpdf .NET
		// 
		// Here we import the pages with odd page numbers of an XPS document up to page 8, 2
		// pages per sheet in landscape. The aspect ratio is preserved and a border is also
		// drawn.
		//
		// File Start: True .\8-abcpdf.operations\4-xpsimportoperation\1-methods\import.htm
		public static void Ex8_abcpdf_operations_4_xpsimportoperationimport() {
			// Part: 1 of 2
			using var doc = new Doc();
			var importOp = new MyImportOperation(doc);
			importOp.Import("../mypics/AdvancedGraphicsExamples.xps");
			doc.Save("xps.pdf");
			// End Part:
		}
		// Part: 2 of 2
		class MyImportOperation {
			private Doc _doc = null;
			private double _margin = 10;
			private int _pagesAdded = 0;
		
			public MyImportOperation(Doc doc) {
				_doc = doc;
		
				_doc.Transform.Rotate(90, _doc.MediaBox.Left, _doc.MediaBox.Bottom);
				_doc.Transform.Translate(_doc.MediaBox.Width, 0);
		
				int id = _doc.GetInfoInt(_doc.Root, "Pages");
				_doc.SetInfo(id, "/Rotate", "90");
			}
		
			public void Import(string inPath) {
				using (XpsImportOperation op = new XpsImportOperation()) {
					op.ProcessingObject += Processing;
					op.ProcessedObject += Processed;
					op.Import(_doc, inPath);
				}
			}
		
			public void Processing(object sender, ProcessingObjectEventArgs e) {
				if (e.Info.SourceType == ProcessingSourceType.Page && e.Info.PageNumber != null) {
					if ((e.Info.PageNumber % 2) == 0)
						e.Info.PageNumber++;
		
					if (e.Info.PageNumber >= 8)
						e.Info.PageNumber = null;
		
					e.Tag = e.Info.PageNumber;
				}
				else if (e.Info.SourceType == ProcessingSourceType.PageContent) {
					if ((_pagesAdded % 2) == 0)
						_doc.Page = _doc.AddPage();
		
					double width = _doc.MediaBox.Height;
					double height = _doc.MediaBox.Width;
					double scale = Math.Min((width - 4 * _margin) / (2 * e.Info.Width.Value),
						(height - 2 * _margin) / e.Info.Height.Value);
		
					double rectWidth = scale * e.Info.Width.Value;
					double rectHeight = scale * e.Info.Height.Value;
		
					double distanceX = (width - 2 * rectWidth) / 3;
					double distanceY = (height - rectHeight) / 2;
		
					_doc.Rect.SetRect(distanceX + (_pagesAdded % 2) * (distanceX + rectWidth),
						distanceY, rectWidth, rectHeight);
		
					e.Info.Handled = true;
					_pagesAdded++;
				}
			}
		
			public void Processed(object sender, ProcessedObjectEventArgs e) {
				if (e.Successful) {
					var pixmap = e.Object as PixMap;
					if (pixmap != null)
						pixmap.Compress();
		
					var graphic = e.Object as GraphicLayer;
					if (graphic != null) {
						_doc.FrameRect();
		
						int pageNumber = (int)e.Tag;
						_doc.FontSize = 16;
						_doc.TextStyle.HPos = 0.5;
						_doc.Rect.Top = _doc.Rect.Bottom - _margin;
						_doc.Rect.Bottom = _doc.MediaBox.Bottom;
						_doc.AddText(string.Format("Page {0}", pageNumber));
					}
				}
			}
		}
		// End Part:
		// File End:

		// Example code for Import Function of SwfImportOperation Class for ABCpdf .NET
		// 
		// See the SwfParameters.FlashVars property or the ProcessingInfo.FrameNumber property
		// for simple examples.
		// 
		//   
		// 
		// Here we import 24 frames from a Flash movie alternately into two PDF documents.
		// The frames are 0.2 seconds apart. Doc.Rect is set so that the outputs are 6-up and
		// preserve the aspect ratio. For the fifth to the eighth imported frames in each document,
		// the background is suppressed and a transparent oval in the same color is drawn instead.
		//
		// File Start: True .\8-abcpdf.operations\5-swfimportoperation\1-methods\import.htm
		public static void Ex8_abcpdf_operations_5_swfimportoperationimport() {
			// Part: 1 of 1
			using Doc doc1 = new Doc();
			using Doc doc2 = new Doc();
			using (var operation = new SwfImportOperation()) {
				const int fontSize = 20;
				int k = 0;
				bool failed = false;
				operation.ProcessingObject += delegate (object sender, ProcessingObjectEventArgs e) {
					switch (e.Info.SourceType) {
						case ProcessingSourceType.MultiFrameImage:
							if (failed || k >= 24)
								e.Info.FrameNumber = null;
							else
								e.Info.FrameNumber = 1 + Convert.ToInt64(0.2 * k * e.Info.FrameRate.Value);
							e.Tag = ProcessingSourceType.MultiFrameImage;
							break;
						case ProcessingSourceType.ImageFrame: {
								SwfImportOperation op = (SwfImportOperation)sender;
								op.Doc = op.Doc == doc1 ? doc2 : doc1;
								const int distance = 20;
								const int margin = 30;
								double width = op.Doc.MediaBox.Width - 2 * margin;
								double height = op.Doc.MediaBox.Height - 2 * margin;
								double scale = Math.Min((width - distance) / (2 * e.Info.Width.Value),
									(height - 2 * distance - 3 * fontSize) / (3 * e.Info.Height.Value));
			
								int p = k / 2;
								double rectWidth = scale * e.Info.Width.Value;
								double rectHeight = scale * e.Info.Height.Value;
								op.Doc.Rect.SetRect(margin + (width + distance) * (p % 2) / 2,
									margin + height - rectHeight - (height + 2 * distance) * (p / 2 % 3) / 3,
									rectWidth, rectHeight);
								if (p % 6 == 0)
									op.Doc.Page = op.Doc.AddPage();
								if (p >= 4 && p < 8 && e.Info.BackgroundColor != null) {
									op.BackgroundRegion = null;
									op.Doc.Color.String = e.Info.BackgroundColor.String;
									op.Doc.Color.Alpha = 127;
									op.Doc.AddOval(true);
								}
							}
							break;
					}
				};
				operation.ProcessedObject += delegate (object sender, ProcessedObjectEventArgs e) {
					if (!e.Successful) {
						failed = true;
						return;
					}
					if (e.Tag is ProcessingSourceType
						&& (ProcessingSourceType)e.Tag == ProcessingSourceType.MultiFrameImage) {
						SwfImportOperation op = (SwfImportOperation)sender;
						op.Doc.Color.Gray = 0;
						op.Doc.Color.Alpha = 255;
						op.Doc.FontSize = fontSize;
						op.Doc.TextStyle.HPos = 0.5;
						op.Doc.Rect.Top = op.Doc.Rect.Bottom;
						op.Doc.Rect.Bottom = op.Doc.MediaBox.Bottom;
						op.Doc.AddText(string.Format("{0} secs", 0.2 * k));
						++k;
					}
					PixMap pixmap = e.Object as PixMap;
					if (pixmap != null)
						pixmap.Compress();
				};
				operation.Import("../Rez/ABCpdf.swf");
			}
			doc1.Save("swf1.pdf");
			doc2.Save("swf2.pdf");
			// End Part:
		}
		// File End:

		// Example code for Save Function of RenderOperation Class for ABCpdf .NET
		// 
		// Here we render all the pages of the doc using 10 threads at a time. We alternate
		// rendering format between jpg and tiff. We also alternate resolution between 150 and
		// 300 dpi. Note how the RenderingOperation is created in the constructor of TheRenderingWorker.
		// This is because at this point a copy of the rendering options is made. Had we created
		// the RenderingOperation in DoWork, we would have picked up only the last doc.Rendering.
		// DotsPerInch, because the threads are started in the following loop. Also note how
		// we dispose the operation in DoWork, to release resources stored on the native side
		// (the copy of the rendering options basically).
		//
		// File Start: True .\8-abcpdf.operations\6-renderoperation\1-methods\save.htm
		public static void Ex8_abcpdf_operations_6_renderoperationsave() {
			// Part: 1 of 2
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			string[] xts = { ".jpg", ".tif" };
			int[] dpis = { 150, 300 };
			var threads = new Thread[10];
			int pageNum = 1, pageCount = doc.PageCount;
			while (pageNum <= pageCount) {
				int count = 0;
				while (count < threads.Length && pageNum <= pageCount) {
					doc.Rendering.DotsPerInch = dpis[(pageNum - 1) % 2];
					doc.PageNumber = pageNum;
					string path = $"ABCpdf{pageNum}{xts[(pageNum - 1) % 2]}";
			
			
					threads[count] = new Thread(new RenderingWorker(doc, path).DoWork);
					++count;
					++pageNum;
				}
				for (int i = 0; i < count; ++i)
					threads[i].Start();
				for (int i = 0; i < count; ++i)
					threads[i].Join();
			}
			// End Part:
		}
		// Part: 2 of 2
		class RenderingWorker {
			private string mPath;
			private RenderOperation mOp;
		
			public RenderingWorker(Doc inDoc, string inPath) {
				mPath = inPath;
				mOp = new RenderOperation(inDoc);
			}
		
			public void DoWork() {
				mOp.Save(mPath);
				mOp.Dispose();
			}
		}
		// End Part:
		// File End:

		// Example code for Save Function of PdfConformityOperation Class for ABCpdf .NET
		// 
		// Here we save a document in PDF/A-1b format.
		//
		// File Start: True .\8-abcpdf.operations\7-pdfconformityoperation\1-methods\save.htm
		public static void Ex8_abcpdf_operations_7_pdfconformityoperationsave() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/Acrobat.pdf");
			string path = "pdfa_save.pdf";
			using (var theOperation = new PdfConformityOperation()) {
				theOperation.Conformance = PdfConformance.PdfA1b;
				theOperation.Save(doc, path);
			
				if (theOperation.Errors.Count > 0) {
					Console.WriteLine("Errors:");
					for (int i = 0; i < theOperation.Errors.Count; ++i)
						Console.WriteLine(theOperation.Errors[i]);
				}
			}
			// End Part:
		}
		// File End:

		// Example code for Read Function of PdfValidationOperation Class for ABCpdf .NET
		// 
		// Here we validate a document against PDF/A-1b format.
		//
		// File Start: True .\8-abcpdf.operations\7-pdfvalidationoperation\1-methods\read.htm
		public static void Ex8_abcpdf_operations_7_pdfvalidationoperationread() {
			// Part: 1 of 1
			string path = "../Rez/pdfa.pdf";
			using (var op = new PdfValidationOperation()) {
				op.Conformance = PdfConformance.PdfA1b;
				using var doc = op.Read(path, null);
			
				if (op.Errors.Count > 0) {
					Console.WriteLine("Errors:");
					for (int i = 0; i < op.Errors.Count; ++i)
						Console.WriteLine(op.Errors[i]);
				}
				if (op.Warnings.Count > 0) {
					if (op.Errors.Count > 0)
						Console.WriteLine();
					Console.WriteLine("Warnings:");
					for (int i = 0; i < op.Warnings.Count; ++i)
						Console.WriteLine(op.Warnings[i]);
				}
			}
			// End Part:
		}
		// File End:

		// Example code for Group Function of TextOperation Class for ABCpdf .NET
		// 
		// Here we highlight a set of words in a source document by drawing a rectangle around
		// each one.
		//
		// File Start: True .\8-abcpdf.operations\8-textoperation\1-methods\group.htm
		public static void Ex8_abcpdf_operations_8_textoperationgroup() {
			// Part: 1 of 1
			string src = "../mypics/Acrobat.pdf";
			string dst = "HighlightedText.pdf";
			string searchString = "Acrobat";
			using var doc = new Doc();
			doc.Read(src);
			TextOperation op = new TextOperation(doc);
			op.PageContents.AddPages();
			string text = op.GetText();
			int pos = 0;
			while (true) {
				pos = text.IndexOf(searchString, pos, StringComparison.CurrentCultureIgnoreCase);
				if (pos < 0)
					break;
				var selection = op.Select(pos, searchString.Length);
				var groups = op.Group(selection);
				foreach (var group in groups) {
					doc.Rect.String = group.Rect.String;
					doc.FrameRect();
				}
				pos += searchString.Length;
			}
			doc.Save(dst);
			// End Part:
		}
		// File End:

		// Example code for GetImageProperties Function of ImageOperation Class for ABCpdf .
		// NET
		// 
		// Here we highlight a set of images in a source document by drawing a red rectangle
		// around each one.
		//
		// File Start: True .\8-abcpdf.operations\9-imageoperation\1-methods\getimageproperties.htm
		public static void Ex8_abcpdf_operations_9_imageoperationgetimageproperties() {
			// Part: 1 of 1
			string src = "../mypics/Acrobat.pdf";
			string dst = "HighlightedImages.pdf";
			using var doc = new Doc();
			doc.Read(src);
			doc.Color.SetRgb(255, 0, 0);
			doc.Width = 0.1;
			var op = new ImageOperation(doc);
			op.PageContents.AddPages();
			var images = op.GetImageProperties();
			foreach (var img in images) {
				foreach (var rend in img.Renditions) {
					rend.Focus();
					doc.FrameRect();
				}
			}
			doc.Save(dst);
			// End Part:
		}
		// File End:

		// Example code for Flatten Function of FlattenTransparencyOperation Class for ABCpdf
		// .NET
		// 
		// Here we <span lang="en-us">flatten all the transparent objects in </span>a document.
		//
		// File Start: True .\8-abcpdf.operations\A-flattentransparency\1-methods\flatten.htm
		public static void Ex8_abcpdf_operations_A_flattentransparencyflatten() {
			// Part: 1 of 1
			var op = new FlattenTransparencyOperation();
			op.DotsPerInch = 144;
			op.ColorSpace = XRendering.ColorSpaceType.Rgb;
			
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			op.Flatten(doc);
			doc.Save("Flattened.pdf");
			// End Part:
		}
		// File End:

		// Example code for Compact Function of ReduceSizeOperation Class for ABCpdf .NET
		// 
		// The following example shows how to compress a document.
		//
		// File Start: True .\8-abcpdf.operations\B-reducesizeoperation\1-methods\compact.htm
		public static void Ex8_abcpdf_operations_B_reducesizeoperationcompact() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../mypics/sample.pdf");
			using (var op = new ReduceSizeOperation(doc))
				op.Compact(true);
			doc.Save("ReduceSizeOperation.pdf");
			// End Part:
		}
		// File End:

		// Example code for AccessibilityOperation Constructor of AccessibilityOperation Class
		// for ABCpdf .NET
		// 
		// Here we read an existing PDF and make it accessible. 
		// 
		// We produce output conformant to the PDF specification and compliant with use within
		// Acrobat. However please note that Accessibility is not well supported outside Acrobat.
		// 
		// In particular some versions of software such as NVDAccess do not necessarily handle
		// all constructs correctly. In particular they have problems with form XObjects and
		// so one needs to avoid this type of construct.
		// 
		// The code below calls Page.StampFormXObjects on all pages in the document to ensure
		// that all form XObjects are removed before the document is made accessible.
		// 
		// The process of stamping form XObjects will not normally make much difference. However
		// in some cases you may find there is a level of expansion in size and very occasionally
		// you may see subtle differences in transparency blending. Nevertheless if you want
		// compatibility, there is no other choice.
		//
		// File Start: True .\8-abcpdf.operations\C-accessibilityoperation\1-methods\1-accessibilityoperation.htm
		public static void Ex8_abcpdf_operations_C_accessibilityoperation1_accessibilityoperation() {
			// Part: 1 of 1
			using var doc = new Doc();
			doc.Read("../Rez/spaceshuttle.pdf");
			var pages = doc.ObjectSoup.Catalog.Pages.GetPageArrayAll();
			foreach (var page in pages)
				page.StampFormXObjects(true); // see notes on NVDA above
			var op = new AccessibilityOperation(doc);
			op.PageContents.AddPages();
			op.MakeAccessible();
			doc.Save("accessible.pdf");
			// End Part:
		}
		// File End:

		// Example code for Tag Function of AccessibilityOperationAI Class for ABCpdf .NET
		// 
		// This example shows a simple document tagging operation.
		//
		// File Start: True .\8-abcpdf.operations\C-accessibilityoperationai\1-methods\01-tag.htm
		public static void Ex8_abcpdf_operations_C_accessibilityoperationai01_tag() {
			// Part: 1 of 1
			var options = new PythonOptions();
			options.Initialize(PythonEnvironment.Current, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope);
			var p = Environment.OSVersion.Platform;
			bool isWindows = p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows;
			if (!isWindows)
				scope.Exec("import os\nos.environ['NO_COLOR'] = '1'\n");
			var op = new AccessibilityOperationAI();
			using var doc = op.Tag(scope, "../Rez/spacex_nasa_dragon.pdf");
			doc.Save("accessibility_ai.pdf");
			// End Part:
		}
		// File End:

		// Example code for ConversionOptions Property of AccessibilityOperationAI Class for
		// ABCpdf .NET
		// 
		// Older Nvidia graphics cards may cause Python on Linux to crash. To use CPU instead
		// of GPU and also to disable ANSI color in stdout output on Linux, you can use code
		// of this form.
		//
		// File Start: True .\8-abcpdf.operations\C-accessibilityoperationai\2-properties\01-conversionoptions.htm
		public static void Ex8_abcpdf_operations_C_accessibilityoperationai01_conversionoptions() {
			// Part: 1 of 1
			string venvPath = null;
			var options = new PythonOptions { SetNoSiteFlag = !string.IsNullOrEmpty(venvPath) };
			options.Initialize(PythonEnvironment.Current, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope);
			var p = Environment.OSVersion.Platform;
			bool isWindows = p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows;
			scope.Exec((isWindows ? "" : "import os\nos.environ['NO_COLOR'] = '1'\n") +
				"from docling.datamodel.accelerator_options import AcceleratorDevice, AcceleratorOptions\n" +
				"from docling.datamodel.base_models import InputFormat\n" +
				"from docling.datamodel.pipeline_options import ThreadedPdfPipelineOptions\n" +
				"from docling.document_converter import PdfFormatOption\n");
			var op = new AccessibilityOperationAI();
			op.ConversionOptions = "{InputFormat.PDF: PdfFormatOption(pipeline_options=ThreadedPdfPipelineOptions(do_ocr=False, accelerator_options=AcceleratorOptions(device=AcceleratorDevice.CPU)))}";
			using var doc = op.Tag(scope, "../Rez/spacex_nasa_dragon.pdf");
			doc.Save("accessibility_ai_cpu.pdf");
			// End Part:
		}
		// File End:

		// Example code for Vectorize Method of VectorizeTextOperation Class for ABCpdf .NET
		// 
		// Here we vectorize all the text in the document.
		//
		// File Start: True .\8-abcpdf.operations\L-vectorizetextoperation\1-methods\vectorize.htm
		public static void Ex8_abcpdf_operations_L_vectorizetextoperationvectorize() {
			// Part: 1 of 3
			static void VectorizeDocText1(string inDocName) {
				var op = new VectorizeTextOperation();
				using var doc = new Doc();
				doc.Read(inDocName);
				op.Vectorize(doc);
				doc.Save("VectorizedSample.pdf");
			}
			// End Part:
			// Part: 2 of 3
			static void Vectorizing2(object sender, ProcessingObjectEventArgs e) {
				var font = e.Object as FontObject;
				if (font != null && font.BaseFont.Contains("Bold"))
					e.Cancel = true;
			}
			static void VectorizeDocText2(string inDocName) {
				var op = new VectorizeTextOperation();
				Doc doc = new Doc();
				doc.Read(inDocName);
				// Use the 'Vectorizing' method to decide which fonts to vectorize
				op.ProcessingObject += new ProcessingObjectEventHandler(Vectorizing2);
				op.Vectorize(doc);
				doc.Save("VectorizedSample.pdf");
			}
			// End Part:
			// Part: 3 of 3
			static void Vectorizing3(object sender, ProcessingObjectEventArgs e) {
				var font = e.Object as FontObject;
				if (font != null && font.EmbeddedFont != null)
					e.Cancel = true;
			}
			static void VectorizeDocText3(string inDocName) {
				var op = new VectorizeTextOperation();
				Doc doc = new Doc();
				doc.Read(inDocName);
				// Use the 'Vectorizing' method to decide which fonts to vectorize
				op.ProcessingObject += new ProcessingObjectEventHandler(Vectorizing3);
				op.Vectorize(doc);
				doc.Save("VectorizedSample.pdf");
			}
			// End Part:
		}
		// File End:

		// Example code for StartOcr Function of CloudVisionOperation Class for ABCpdf .NET
		// 
		// Here we read a TIFF into a Doc, OCR it and then save the PDF. 
		// 
		// The text in this document is indivisible which means you can select and copy it,
		// but it does not obscure the image in the background.
		// 
		// To run this code you will need to replace the Google API key with one of your own.
		//
		// File Start: True .\8-abcpdf.operations\M-cloudvisionoperation\1-methods\04-startocr.htm
		public static void Ex8_abcpdf_operations_M_cloudvisionoperation04_startocr() {
			// Part: 1 of 1
			#if !NETFRAMEWORK // ignore
			return; // ignore
			string theSrc = "../mypics/multipage.tif";
			string theDst = "CloudOCR.pdf";
			using (Doc doc = new Doc()) {
				doc.Read(theSrc);
			
				var op = new CloudVisionOperation(doc);
				op.AddPages();
				op.ApiKey = "AIzAUlBcTPDwxUNn5uQvSHONpsTpAqVJHbfOa";
				op.StartOcr();
				op.WaitAll();
				doc.Save(theDst);
			}
			#endif // ignore
			// End Part:
		}
		// File End:

		// Example code for Doc Property of WebPageOperation Class for ABCpdf .NET
		// 
		// The following code creates a PDF document from HTML and outlines any tagged areas.
		//
		// File Start: True .\8-abcpdf.operations\Q-webpageoperation\2-properties\01-doc.htm
		public static void Ex8_abcpdf_operations_Q_webpageoperation01_doc() {
			// Part: 1 of 1
			var op = new WebPageOperation();
			using (op.Doc) {
				op.Doc.Rect.Inset(72, 72);
				op.Doc.HtmlOptions.AddTags = true;
				op.Doc.HtmlOptions.RetryCount = 0;
				op.Tagged = true;
				op.Outline = true;
				string template = "<html><body><div style=\"width:100%;font-size:14pt;text-align:center;\">*</div></body></html>";
				op.HeaderHtml = template.Replace("*", "Commentarii de Bello Gallico");
				op.FooterHtml = template.Replace("*", "<span class=pageNumber></span> of <span class=totalPages></span>");
				op.ReadUrl(GetUri("../Rez/commentarii.htm"));
				var tagIDs = op.Doc.HtmlOptions.GetTagIDs(op.Doc.Page);
				var tagRects = op.Doc.HtmlOptions.GetTagRects(op.Doc.Page);
				for (int i = 0; i < tagIDs.Length; ++i) {
					op.Doc.Rect.String = tagRects[i].String;
					op.Doc.FrameRect();
					op.Doc.FontSize = (int)(0.9 * op.Doc.Rect.Height);
					op.Doc.AddText(tagIDs[i]);
				}
				op.Doc.Save("webpageop.pdf");
			}
			// End Part:
		}
		// File End:

		// Example code for CustomFont Function of CustomFont Class for ABCpdf .NET
		// 
		// This example shows how to create a font Doc from a set of SVG files for use as the
		// basis of a custom font. For an example showing how to use the emojifont.pdf document
		// see the Embed method.
		//
		// File Start: True .\8-abcpdf.operations\R-customfont\1-methods\01-customfont.htm
		public static void Ex8_abcpdf_operations_R_customfont01_customfont() {
			// Part: 1 of 2
			using var doc = new Doc();
			string icons = "../Rez/icons";
			CustomFontMaker.Append(doc, icons, "happy-svgrepo-com.svg", 0x1F600);
			CustomFontMaker.Append(doc, icons, "smiling-svgrepo-com.svg", 0x1F601);
			CustomFontMaker.Append(doc, icons, "in-love-svgrepo-com.svg", 0x1F60D);
			CustomFontMaker.Append(doc, icons, "sad-svgrepo-com.svg", 0x1F614);
			CustomFontMaker.Append(doc, icons, "angry-svgrepo-com.svg", 0x1F620);
			CustomFontMaker.Append(doc, icons, "crying-svgrepo-com.svg", 0x1F62F);
			CustomFontMaker.Baseline(doc, 0.2);
			doc.Save("emojifont.pdf");
			// End Part:
		}
		// Part: 2 of 2
		class CustomFontMaker {
			public static void Append(Doc doc, string dir, string file, int unicode) {
				using var icon = new Doc();
				icon.Read(Path.Combine(dir, file));
				doc.Append(icon);
				doc.PageNumber = doc.PageCount;
				doc.AddBookmark($"0x{unicode:X}", false);
			}
			public static void Baseline(Doc doc, double baseline) {
				var pages = doc.ObjectSoup.Catalog.Pages.GetPageArrayAll();
				var bbox = new XRect();
				foreach (var page in pages)
					bbox.Union(page.MediaBox);
				var scale = new XTransform(1, 0, 0, 1, 0, -bbox.Top * baseline);
				foreach (var page in pages) {
					doc.Page = page.ID;
					var layers = page.GetLayers();
					if (layers.Length > 0) {
						// scale contents
						var contents = (ArrayAtom)page.Resolve(Atom.GetItem(page.Atom, "Contents"));
						var restore = new StreamObject(doc.ObjectSoup, Encoding.ASCII.GetBytes($" Q"));
						contents.Insert(layers.Length, new RefAtom(restore));
						var save = new StreamObject(doc.ObjectSoup, Encoding.ASCII.GetBytes($"q {scale.String} cm "));
						contents.Insert(0, new RefAtom(save));
						// update page boundaries
						page.MediaBox.Union(XRect.FromPoints(scale.TransformPoints(page.MediaBox.GetCorners())));
						if (page.CropBox != null)
							page.CropBox.Union(XRect.FromPoints(scale.TransformPoints(page.CropBox.GetCorners())));
						if (page.ArtBox != null)
							page.ArtBox.Union(XRect.FromPoints(scale.TransformPoints(page.ArtBox.GetCorners())));
						if (page.BleedBox != null)
							page.BleedBox.Union(XRect.FromPoints(scale.TransformPoints(page.BleedBox.GetCorners())));
					}
				}
			}
		}
		// End Part:
		// File End:

		// Example code for Embed Function of CustomFont Class for ABCpdf .NET
		// 
		// This example shows how you might embed and use a custom font. For an example showing
		// how to create the emojifont.pdf document see the CustomFont constructor.
		//
		// File Start: True .\8-abcpdf.operations\R-customfont\1-methods\02-embed.htm
		public static void Ex8_abcpdf_operations_R_customfont02_embed() {
			// Part: 1 of 1
			using var icons = new Doc();
			icons.Read("../Rez/emojifont.pdf");
			var emojis = new CustomFont(icons, 'a');
			using var doc = new Doc();
			doc.Page = doc.AddPage();
			doc.FontSize = 42;
			doc.Rect.Inset(72, 72);
			int id = emojis.Embed(doc, true, 0.1);
			doc.AddTextStyled($"Hello World: <stylerun pid={id}>abcdef</stylerun>...");
			doc.FrameRect();
			doc.Save("emojis.pdf");
			// End Part:
		}
		// File End:

		// Example code for Version Property of PythonEngine Struct for ABCpdf .NET
		// 
		// The following code reports the engine version.
		//
		// File Start: True .\9-abcpdf.python\03-pythonengine\2-properties\01-version.htm
		public static void Ex9_abcpdf_python_03_pythonengine01_version() {
			// Part: 1 of 1
			var env = PythonEnvironment.Current;
			var options = new PythonOptions();
			options.Initialize(env, null);
			var engine = new PythonEngine(env);
			Console.WriteLine(engine.Version);
			// End Part:
		}
		// File End:

		// Example code for AutoInitializeRuntime Function of PythonRuntime Class for ABCpdf
		// .NET
		// 
		// To use a virtual environment, you need to initialize the Python engine with PythonOptions.
		// SetNoSiteFlag true and to provide the path to the directory of the virtual environment.
		// This example shows how you might do this.
		//
		// File Start: True .\9-abcpdf.python\04-pythonruntime\1-methods\02-autoinitializeruntime.htm
		public static void Ex9_abcpdf_python_04_pythonruntime02_autoinitializeruntime() {
			// Part: 1 of 1
			string venvPath = @"C:\MyVenv";
			var env = PythonEnvironment.Current;
			var options = new PythonOptions { SetNoSiteFlag = !string.IsNullOrEmpty(venvPath) };
			options.Initialize(env, null);
			using var rt = env.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope, venvPath);
			// ... do work
			// End Part:
		}
		// File End:

		// Example code for SetAfterEval Function of PythonScope Class for ABCpdf .NET
		// 
		// You can re-use a complex object using code of the following form.
		//
		// File Start: True .\9-abcpdf.python\06-pythonscope\1-methods\10-setaftereval.htm
		public static void Ex9_abcpdf_python_06_pythonscope10_setaftereval() {
			// Part: 1 of 1
			var options = new PythonOptions();
			options.Initialize(PythonEnvironment.Current, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope);
			scope.Exec(
				"from docling.datamodel.accelerator_options import AcceleratorDevice, AcceleratorOptions\n" +
				"from docling.datamodel.base_models import InputFormat\n" +
				"from docling.datamodel.pipeline_options import ThreadedPdfPipelineOptions\n" +
				"from docling.document_converter import PdfFormatOption\n"
			);
			scope.SetAfterEval("myFormatOptions", "{InputFormat.PDF: PdfFormatOption(pipeline_options=ThreadedPdfPipelineOptions(do_ocr=False, accelerator_options=AcceleratorOptions(device=AcceleratorDevice.CPU)))}");
			var op = new AccessibilityOperationAI();
			op.ConversionOptions = "myFormatOptions";
			// ... do work
			// End Part:
		}
		// File End:

		// Example code for EvalString Function of PythonScope Class for ABCpdf .NET
		// 
		// For example to get the version of Docling you might use code of the following form.
		//
		// File Start: True .\9-abcpdf.python\06-pythonscope\1-methods\11-evalstring.htm
		public static void Ex9_abcpdf_python_06_pythonscope11_evalstring() {
			// Part: 1 of 1
			var options = new PythonOptions();
			options.Initialize(PythonEnvironment.Current, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope);
			var result = scope.EvalString("__import__('importlib.metadata', fromlist=['metadata']).version('docling')\n");
			Console.WriteLine(result);
			// End Part:
		}
		// File End:

		// Example code for Exec Function of PythonScope Class for ABCpdf .NET
		// 
		// To disable ANSI color in stdout output on Linux you might use code of the following
		// form.
		//
		// File Start: True .\9-abcpdf.python\06-pythonscope\1-methods\18-exec.htm
		public static void Ex9_abcpdf_python_06_pythonscope18_exec() {
			// Part: 1 of 1
			var options = new PythonOptions();
			options.Initialize(PythonEnvironment.Current, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			rt.StdOutWriter = Console.Out;
			rt.StdErrWriter = Console.Error;
			rt.AutoInitializeRuntime(scope);
			scope.Exec("import os\nos.environ['NO_COLOR'] = '1'\n");
			// ... do work
			// End Part:
		}
		// File End:

		// Example code for InitializeRuntime Function of PythonRuntimeOptions Class for ABCpdf
		// .NET
		// 
		// To disable ANSI color in stdout output on Linux you might use code of the following
		// form.
		//
		// File Start: True .\9-abcpdf.python\07-pythonruntimeoptions\1-methods\02-initializeruntime.htm
		public static void Ex9_abcpdf_python_07_pythonruntimeoptions02_initializeruntime() {
			// Part: 1 of 1
			string venvPath = null;
			var options = new PythonOptions { SetNoSiteFlag = !string.IsNullOrEmpty(venvPath) };
			var env = PythonEnvironment.Current;
			options.Initialize(env, null);
			using var rt = PythonEnvironment.Current.GetRuntime();
			using var scope = rt.CreateScope();
			var rtOptions = new PythonRuntimeOptions() {
			    VEnvPath = venvPath,
			    StdOutCallback = Console.Out.Write,
			    StdErrCallback = Console.Error.Write
			};
			rtOptions.InitializeRuntime(scope);
			env.RuntimeIsInitialized = RuntimeInitializationState.UserInitialized;
			// ... do work
			// End Part:
		}
		// File End:


		public static Dictionary<string, Action> GetAll() {
			var exc = new Dictionary<string, Action>();
			exc["4_examples_02_textflow"] = Ex4_examples_02_textflow;
			exc["4_examples_02_textflow2"] = Ex4_examples_02_textflow2;
			exc["4_examples_02_texttagged"] = Ex4_examples_02_texttagged;
			exc["4_examples_03_multistyled"] = Ex4_examples_03_multistyled;
			exc["4_examples_04_image"] = Ex4_examples_04_image;
			exc["4_examples_05_deletion"] = Ex4_examples_05_deletion;
			exc["4_examples_06_headers"] = Ex4_examples_06_headers;
			exc["4_examples_08_landscape"] = Ex4_examples_08_landscape;
			exc["4_examples_09_table1"] = Ex4_examples_09_table1;
			exc["4_examples_10_table2"] = Ex4_examples_10_table2;
			exc["4_examples_12_unicode"] = Ex4_examples_12_unicode;
			exc["4_examples_13_pagedhtml"] = Ex4_examples_13_pagedhtml;
			exc["4_examples_15_eform1"] = Ex4_examples_15_eform1;
			exc["4_examples_15_eform2"] = Ex4_examples_15_eform2;
			exc["4_examples_15_eform3"] = Ex4_examples_15_eform3;
			exc["4_examples_16_eformfdf"] = Ex4_examples_16_eformfdf;
			exc["4_examples_17_advancedgraphics"] = Ex4_examples_17_advancedgraphics;
			exc["4_examples_18_annotations"] = Ex4_examples_18_annotations;
			exc["4_examples_19_rendering"] = Ex4_examples_19_rendering;
			exc["4_examples_20_systemdrawing"] = Ex4_examples_20_systemdrawing;
			exc["4_examples_21_wpftables"] = Ex4_examples_21_wpftables;
			exc["5_abcpdf_docaddarc"] = Ex5_abcpdf_docaddarc;
			exc["5_abcpdf_docaddbookmark"] = Ex5_abcpdf_docaddbookmark;
			exc["5_abcpdf_docaddcolorspacefile"] = Ex5_abcpdf_docaddcolorspacefile;
			exc["5_abcpdf_docaddcolorspacespot"] = Ex5_abcpdf_docaddcolorspacespot;
			exc["5_abcpdf_docaddfont"] = Ex5_abcpdf_docaddfont;
			exc["5_abcpdf_docaddgrid"] = Ex5_abcpdf_docaddgrid;
			exc["5_abcpdf_docaddimagebitmap"] = Ex5_abcpdf_docaddimagebitmap;
			exc["5_abcpdf_docaddimagecopy"] = Ex5_abcpdf_docaddimagecopy;
			exc["5_abcpdf_docaddimagedoc"] = Ex5_abcpdf_docaddimagedoc;
			exc["5_abcpdf_docaddimagefile"] = Ex5_abcpdf_docaddimagefile;
			exc["5_abcpdf_docaddimageobject"] = Ex5_abcpdf_docaddimageobject;
			exc["5_abcpdf_docaddimagetochain"] = Ex5_abcpdf_docaddimagetochain;
			exc["5_abcpdf_docaddimageurl"] = Ex5_abcpdf_docaddimageurl;
			exc["5_abcpdf_docaddline"] = Ex5_abcpdf_docaddline;
			exc["5_abcpdf_docaddobject"] = Ex5_abcpdf_docaddobject;
			exc["5_abcpdf_docaddoval"] = Ex5_abcpdf_docaddoval;
			exc["5_abcpdf_docaddpage"] = Ex5_abcpdf_docaddpage;
			exc["5_abcpdf_docaddpie"] = Ex5_abcpdf_docaddpie;
			exc["5_abcpdf_docaddpoly"] = Ex5_abcpdf_docaddpoly;
			exc["5_abcpdf_docaddtext"] = Ex5_abcpdf_docaddtext;
			exc["5_abcpdf_docaddtextstyled"] = Ex5_abcpdf_docaddtextstyled;
			exc["5_abcpdf_docaddxobject"] = Ex5_abcpdf_docaddxobject;
			exc["5_abcpdf_docappend"] = Ex5_abcpdf_docappend;
			exc["5_abcpdf_docdelete"] = Ex5_abcpdf_docdelete;
			exc["5_abcpdf_docembedfont"] = Ex5_abcpdf_docembedfont;
			exc["5_abcpdf_docfillrect"] = Ex5_abcpdf_docfillrect;
			exc["5_abcpdf_docframerect"] = Ex5_abcpdf_docframerect;
			exc["5_abcpdf_docgetinfo"] = Ex5_abcpdf_docgetinfo;
			exc["5_abcpdf_docread"] = Ex5_abcpdf_docread;
			exc["5_abcpdf_docremappages"] = Ex5_abcpdf_docremappages;
			exc["5_abcpdf_docsave"] = Ex5_abcpdf_docsave;
			exc["5_abcpdf_docsetinfo"] = Ex5_abcpdf_docsetinfo;
			exc["5_abcpdf_doccolor"] = Ex5_abcpdf_doccolor;
			exc["5_abcpdf_doccolorspace"] = Ex5_abcpdf_doccolorspace;
			exc["5_abcpdf_docencryption"] = Ex5_abcpdf_docencryption;
			exc["5_abcpdf_docfont"] = Ex5_abcpdf_docfont;
			exc["5_abcpdf_docfontsize"] = Ex5_abcpdf_docfontsize;
			exc["5_abcpdf_docmediabox"] = Ex5_abcpdf_docmediabox;
			exc["5_abcpdf_docoptions"] = Ex5_abcpdf_docoptions;
			exc["5_abcpdf_docpage"] = Ex5_abcpdf_docpage;
			exc["5_abcpdf_docpos"] = Ex5_abcpdf_docpos;
			exc["5_abcpdf_docrect"] = Ex5_abcpdf_docrect;
			exc["5_abcpdf_docroot"] = Ex5_abcpdf_docroot;
			exc["5_abcpdf_docstring"] = Ex5_abcpdf_docstring;
			exc["5_abcpdf_doctextstyle"] = Ex5_abcpdf_doctextstyle;
			exc["5_abcpdf_doctopdown"] = Ex5_abcpdf_doctopdown;
			exc["5_abcpdf_doctransform"] = Ex5_abcpdf_doctransform;
			exc["5_abcpdf_docwidth"] = Ex5_abcpdf_docwidth;
			exc["5_abcpdf_xcoloralpha"] = Ex5_abcpdf_xcoloralpha;
			exc["5_abcpdf_xcolorcomponents"] = Ex5_abcpdf_xcolorcomponents;
			exc["5_abcpdf_xencryptionsetcryptmethods"] = Ex5_abcpdf_xencryptionsetcryptmethods;
			exc["5_abcpdf_xformadddoctimestamp"] = Ex5_abcpdf_xformadddoctimestamp;
			exc["5_abcpdf_xhtmloptionsgettagrects"] = Ex5_abcpdf_xhtmloptionsgettagrects;
			exc["5_abcpdf_xhtmloptionslinkdestinations"] = Ex5_abcpdf_xhtmloptionslinkdestinations;
			exc["5_abcpdf_xhtmloptionslinkpages"] = Ex5_abcpdf_xhtmloptionslinkpages;
			exc["5_abcpdf_xhtmloptions2_forchrome"] = Ex5_abcpdf_xhtmloptions2_forchrome;
			exc["5_abcpdf_xhtmloptions2_forgecko"] = Ex5_abcpdf_xhtmloptions2_forgecko;
			exc["5_abcpdf_xhtmloptions2_formshtml"] = Ex5_abcpdf_xhtmloptions2_formshtml;
			exc["5_abcpdf_xhtmloptions2_forwebkit"] = Ex5_abcpdf_xhtmloptions2_forwebkit;
			exc["5_abcpdf_xhtmloptionsaddforms"] = Ex5_abcpdf_xhtmloptionsaddforms;
			exc["5_abcpdf_xhtmloptionsbrowserwidth"] = Ex5_abcpdf_xhtmloptionsbrowserwidth;
			exc["5_abcpdf_xhtmloptionsfireshield"] = Ex5_abcpdf_xhtmloptionsfireshield;
			exc["5_abcpdf_xhtmloptionshidebackground"] = Ex5_abcpdf_xhtmloptionshidebackground;
			exc["5_abcpdf_xhtmloptionshtmlcallback"] = Ex5_abcpdf_xhtmloptionshtmlcallback;
			exc["5_abcpdf_xhtmloptionshtmlembedcallback"] = Ex5_abcpdf_xhtmloptionshtmlembedcallback;
			exc["5_abcpdf_xhtmloptionshttpadditionalheaders"] = Ex5_abcpdf_xhtmloptionshttpadditionalheaders;
			exc["5_abcpdf_xhtmloptionsimagequality"] = Ex5_abcpdf_xhtmloptionsimagequality;
			exc["5_abcpdf_xhtmloptionslogonname"] = Ex5_abcpdf_xhtmloptionslogonname;
			exc["5_abcpdf_xhtmloptionsretrycount"] = Ex5_abcpdf_xhtmloptionsretrycount;
			exc["5_abcpdf_xhtmloptionsusescript"] = Ex5_abcpdf_xhtmloptionsusescript;
			exc["5_abcpdf_ximagesetdata"] = Ex5_abcpdf_ximagesetdata;
			exc["5_abcpdf_ximagesetfile"] = Ex5_abcpdf_ximagesetfile;
			exc["5_abcpdf_ximagesetmask"] = Ex5_abcpdf_ximagesetmask;
			exc["5_abcpdf_ximagesetstream"] = Ex5_abcpdf_ximagesetstream;
			exc["5_abcpdf_ximageframe"] = Ex5_abcpdf_ximageframe;
			exc["5_abcpdf_ximageselection"] = Ex5_abcpdf_ximageselection;
			exc["5_abcpdf_xpointpoint"] = Ex5_abcpdf_xpointpoint;
			exc["5_abcpdf_xpointstring"] = Ex5_abcpdf_xpointstring;
			exc["5_abcpdf_xrectinset"] = Ex5_abcpdf_xrectinset;
			exc["5_abcpdf_xrectmagnify"] = Ex5_abcpdf_xrectmagnify;
			exc["5_abcpdf_xrectmove"] = Ex5_abcpdf_xrectmove;
			exc["5_abcpdf_xrectposition"] = Ex5_abcpdf_xrectposition;
			exc["5_abcpdf_xrectresize"] = Ex5_abcpdf_xrectresize;
			exc["5_abcpdf_xrectsetrect"] = Ex5_abcpdf_xrectsetrect;
			exc["5_abcpdf_xrectrectangle"] = Ex5_abcpdf_xrectrectangle;
			exc["5_abcpdf_xrectstring"] = Ex5_abcpdf_xrectstring;
			exc["5_abcpdf_xrenderingantialiasimages"] = Ex5_abcpdf_xrenderingantialiasimages;
			exc["5_abcpdf_xrenderingantialiaspolygons"] = Ex5_abcpdf_xrenderingantialiaspolygons;
			exc["5_abcpdf_xrenderingantialiastext"] = Ex5_abcpdf_xrenderingantialiastext;
			exc["5_abcpdf_xrenderingcolorspace"] = Ex5_abcpdf_xrenderingcolorspace;
			exc["5_abcpdf_xrenderingdefaulthalftone"] = Ex5_abcpdf_xrenderingdefaulthalftone;
			exc["5_abcpdf_xrenderingdrawannotations"] = Ex5_abcpdf_xrenderingdrawannotations;
			exc["5_abcpdf_xrenderingicccmyk"] = Ex5_abcpdf_xrenderingicccmyk;
			exc["5_abcpdf_xrenderingoverprint"] = Ex5_abcpdf_xrenderingoverprint;
			exc["5_abcpdf_xrenderingsavealpha"] = Ex5_abcpdf_xrenderingsavealpha;
			exc["5_abcpdf_xrenderingsavecompression"] = Ex5_abcpdf_xrenderingsavecompression;
			exc["5_abcpdf_xrenderingsavequality"] = Ex5_abcpdf_xrenderingsavequality;
			exc["5_abcpdf_xsaveoptionstemplate"] = Ex5_abcpdf_xsaveoptionstemplate;
			exc["5_abcpdf_xsaveoptionswritepageseparator"] = Ex5_abcpdf_xsaveoptionswritepageseparator;
			exc["5_abcpdf_xsavetemplatedatasetmeasureresolution"] = Ex5_abcpdf_xsavetemplatedatasetmeasureresolution;
			exc["5_abcpdf_xtagging04_open"] = Ex5_abcpdf_xtagging04_open;
			exc["5_abcpdf_xtagging05_close"] = Ex5_abcpdf_xtagging05_close;
			exc["5_abcpdf_xtagging08_addfocus"] = Ex5_abcpdf_xtagging08_addfocus;
			exc["5_abcpdf_xtagging_focusopaquetypes"] = Ex5_abcpdf_xtagging_focusopaquetypes;
			exc["5_abcpdf_xtextstyleautotag"] = Ex5_abcpdf_xtextstyleautotag;
			exc["5_abcpdf_xtextstylebold"] = Ex5_abcpdf_xtextstylebold;
			exc["5_abcpdf_xtextstylecharspacing"] = Ex5_abcpdf_xtextstylecharspacing;
			exc["5_abcpdf_xtextstylehpos"] = Ex5_abcpdf_xtextstylehpos;
			exc["5_abcpdf_xtextstyleindent"] = Ex5_abcpdf_xtextstyleindent;
			exc["5_abcpdf_xtextstyleitalic"] = Ex5_abcpdf_xtextstyleitalic;
			exc["5_abcpdf_xtextstylejustification"] = Ex5_abcpdf_xtextstylejustification;
			exc["5_abcpdf_xtextstylekerning"] = Ex5_abcpdf_xtextstylekerning;
			exc["5_abcpdf_xtextstyleleftmargin"] = Ex5_abcpdf_xtextstyleleftmargin;
			exc["5_abcpdf_xtextstylelinespacing"] = Ex5_abcpdf_xtextstylelinespacing;
			exc["5_abcpdf_xtextstyleoutline"] = Ex5_abcpdf_xtextstyleoutline;
			exc["5_abcpdf_xtextstyleparaspacing"] = Ex5_abcpdf_xtextstyleparaspacing;
			exc["5_abcpdf_xtextstylesize"] = Ex5_abcpdf_xtextstylesize;
			exc["5_abcpdf_xtextstylestrike"] = Ex5_abcpdf_xtextstylestrike;
			exc["5_abcpdf_xtextstylestrike2"] = Ex5_abcpdf_xtextstylestrike2;
			exc["5_abcpdf_xtextstylestring"] = Ex5_abcpdf_xtextstylestring;
			exc["5_abcpdf_xtextstyleunderline"] = Ex5_abcpdf_xtextstyleunderline;
			exc["5_abcpdf_xtextstylevpos"] = Ex5_abcpdf_xtextstylevpos;
			exc["5_abcpdf_xtextstylewordspacing"] = Ex5_abcpdf_xtextstylewordspacing;
			exc["5_abcpdf_xtransforminvert"] = Ex5_abcpdf_xtransforminvert;
			exc["5_abcpdf_xtransformmagnify"] = Ex5_abcpdf_xtransformmagnify;
			exc["5_abcpdf_xtransformreset"] = Ex5_abcpdf_xtransformreset;
			exc["5_abcpdf_xtransformrotate"] = Ex5_abcpdf_xtransformrotate;
			exc["5_abcpdf_xtransformskew"] = Ex5_abcpdf_xtransformskew;
			exc["5_abcpdf_xtransformtranslate"] = Ex5_abcpdf_xtransformtranslate;
			exc["5_abcpdf_xtransformangleunit"] = Ex5_abcpdf_xtransformangleunit;
			exc["6_abcpdf_objects_2_objectsoup_eof01_load"] = Ex6_abcpdf_objects_2_objectsoup_eof01_load;
			exc["6_abcpdf_objects_cataloggetembeddedfiles"] = Ex6_abcpdf_objects_cataloggetembeddedfiles;
			exc["6_abcpdf_objects_catalogmetadata"] = Ex6_abcpdf_objects_catalogmetadata;
			exc["6_abcpdf_objects_colorspacegamma"] = Ex6_abcpdf_objects_colorspacegamma;
			exc["6_abcpdf_objects_colorspacewhitepoint"] = Ex6_abcpdf_objects_colorspacewhitepoint;
			exc["6_abcpdf_objects_filespecificationfilespecification"] = Ex6_abcpdf_objects_filespecificationfilespecification;
			exc["6_abcpdf_objects_fontobjectrunewidths"] = Ex6_abcpdf_objects_fontobjectrunewidths;
			exc["6_abcpdf_objects_fontobjectwidths"] = Ex6_abcpdf_objects_fontobjectwidths;
			exc["6_abcpdf_objects_pagegetbitmap"] = Ex6_abcpdf_objects_pagegetbitmap;
			exc["6_abcpdf_objects_pagemakeformxobject"] = Ex6_abcpdf_objects_pagemakeformxobject;
			exc["6_abcpdf_objects_pagevectorizetext"] = Ex6_abcpdf_objects_pagevectorizetext;
			exc["6_abcpdf_objects_pagerotation"] = Ex6_abcpdf_objects_pagerotation;
			exc["6_abcpdf_objects_pagethumbnail"] = Ex6_abcpdf_objects_pagethumbnail;
			exc["6_abcpdf_objects_pixmapgetbitmap"] = Ex6_abcpdf_objects_pixmapgetbitmap;
			exc["6_abcpdf_objects_pixmaprecolor"] = Ex6_abcpdf_objects_pixmaprecolor;
			exc["6_abcpdf_objects_pixmapresize"] = Ex6_abcpdf_objects_pixmapresize;
			exc["6_abcpdf_objects_pixmapsetalpha"] = Ex6_abcpdf_objects_pixmapsetalpha;
			exc["6_abcpdf_objects_pixmapsetchromakey"] = Ex6_abcpdf_objects_pixmapsetchromakey;
			exc["6_abcpdf_objects_pixmaptograyscale"] = Ex6_abcpdf_objects_pixmaptograyscale;
			exc["6_abcpdf_objects_signatureaddltv"] = Ex6_abcpdf_objects_signatureaddltv;
			exc["6_abcpdf_objects_signaturesign"] = Ex6_abcpdf_objects_signaturesign;
			exc["6_abcpdf_objects_signaturevalidate"] = Ex6_abcpdf_objects_signaturevalidate;
			exc["6_abcpdf_objects_signaturecompliancepades"] = Ex6_abcpdf_objects_signaturecompliancepades;
			exc["6_abcpdf_objects_signaturecustomsigner"] = Ex6_abcpdf_objects_signaturecustomsigner;
			exc["6_abcpdf_objects_signaturecustomsigner2"] = Ex6_abcpdf_objects_signaturecustomsigner2;
			exc["7_abcpdf_atoms_arrayatomfromcontentstream"] = Ex7_abcpdf_atoms_arrayatomfromcontentstream;
			exc["7_abcpdf_atoms_opatomfind"] = Ex7_abcpdf_atoms_opatomfind;
			exc["8_abcpdf_operations_2_processinginfoframenumber"] = Ex8_abcpdf_operations_2_processinginfoframenumber;
			exc["8_abcpdf_operations_3_recoloroperationrecolor"] = Ex8_abcpdf_operations_3_recoloroperationrecolor;
			exc["8_abcpdf_operations_3_recoloroperationicccmyk"] = Ex8_abcpdf_operations_3_recoloroperationicccmyk;
			exc["8_abcpdf_operations_4_xpsimportoperationimport"] = Ex8_abcpdf_operations_4_xpsimportoperationimport;
			exc["8_abcpdf_operations_5_swfimportoperationimport"] = Ex8_abcpdf_operations_5_swfimportoperationimport;
			exc["8_abcpdf_operations_6_renderoperationsave"] = Ex8_abcpdf_operations_6_renderoperationsave;
			exc["8_abcpdf_operations_7_pdfconformityoperationsave"] = Ex8_abcpdf_operations_7_pdfconformityoperationsave;
			exc["8_abcpdf_operations_7_pdfvalidationoperationread"] = Ex8_abcpdf_operations_7_pdfvalidationoperationread;
			exc["8_abcpdf_operations_8_textoperationgroup"] = Ex8_abcpdf_operations_8_textoperationgroup;
			exc["8_abcpdf_operations_9_imageoperationgetimageproperties"] = Ex8_abcpdf_operations_9_imageoperationgetimageproperties;
			exc["8_abcpdf_operations_A_flattentransparencyflatten"] = Ex8_abcpdf_operations_A_flattentransparencyflatten;
			exc["8_abcpdf_operations_B_reducesizeoperationcompact"] = Ex8_abcpdf_operations_B_reducesizeoperationcompact;
			exc["8_abcpdf_operations_C_accessibilityoperation1_accessibilityoperation"] = Ex8_abcpdf_operations_C_accessibilityoperation1_accessibilityoperation;
			exc["8_abcpdf_operations_C_accessibilityoperationai01_tag"] = Ex8_abcpdf_operations_C_accessibilityoperationai01_tag;
			exc["8_abcpdf_operations_C_accessibilityoperationai01_conversionoptions"] = Ex8_abcpdf_operations_C_accessibilityoperationai01_conversionoptions;
			exc["8_abcpdf_operations_L_vectorizetextoperationvectorize"] = Ex8_abcpdf_operations_L_vectorizetextoperationvectorize;
			exc["8_abcpdf_operations_M_cloudvisionoperation04_startocr"] = Ex8_abcpdf_operations_M_cloudvisionoperation04_startocr;
			exc["8_abcpdf_operations_Q_webpageoperation01_doc"] = Ex8_abcpdf_operations_Q_webpageoperation01_doc;
			exc["8_abcpdf_operations_R_customfont01_customfont"] = Ex8_abcpdf_operations_R_customfont01_customfont;
			exc["8_abcpdf_operations_R_customfont02_embed"] = Ex8_abcpdf_operations_R_customfont02_embed;
			exc["9_abcpdf_python_03_pythonengine01_version"] = Ex9_abcpdf_python_03_pythonengine01_version;
			exc["9_abcpdf_python_04_pythonruntime02_autoinitializeruntime"] = Ex9_abcpdf_python_04_pythonruntime02_autoinitializeruntime;
			exc["9_abcpdf_python_06_pythonscope10_setaftereval"] = Ex9_abcpdf_python_06_pythonscope10_setaftereval;
			exc["9_abcpdf_python_06_pythonscope11_evalstring"] = Ex9_abcpdf_python_06_pythonscope11_evalstring;
			exc["9_abcpdf_python_06_pythonscope18_exec"] = Ex9_abcpdf_python_06_pythonscope18_exec;
			exc["9_abcpdf_python_07_pythonruntimeoptions02_initializeruntime"] = Ex9_abcpdf_python_07_pythonruntimeoptions02_initializeruntime;
			return exc;
		}

// End Tests
	}
}
