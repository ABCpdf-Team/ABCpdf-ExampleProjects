// ===========================================================================
//	©2013-2026 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product\. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.IO;
using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Atoms;
using WebSupergoo.ABCpdf14.Objects;


namespace TaggedPDF {
	/// <summary>
	/// Startup class for calling tagged pdf tests
	/// Tests include xml converter example and hardcoded tagged 
	/// pdf creation.
	/// - Simple example shows how to generate tagged pdf files using ABCPdf
	/// - Xml converter transforms simple html-like xml format to tagged pdf. 
	/// </summary>
	class TaggedPDFApp {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			string theBase = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			string srcDir = Path.Combine(theBase, "Input");
			string dstDir = Path.Combine(theBase, "Output");

			// simple example first
			SimpleExample(srcDir, dstDir);

			// then xml based example
			string[] files = Directory.GetFiles(srcDir);
			foreach (string file in files)
				XmlExample(file, dstDir);
		}

		/// <summary>
		/// Example of a simple dynamically created tagged pdf document.
		/// </summary>
		static public void SimpleExample(string srcDir, string dstDir) {
			// Create new doc and tagged content for it
			using var doc = new Doc();

			//Add a container tag
			doc.Tag.Open("Container");

			//Add a paragraph of text
			doc.Rect.Inset(40, 40);
			doc.FontSize = 20;
			doc.Tag.Open("P");
			doc.AddText("Tagged PDF (PDF 1.4) is a stylized use of PDF that builds on the logical structure framework described in Section 10.6, “Logical Structure.” It defines a set of standard structure types and attributes that allow page content (text, graphics, andimages) to be extracted and reused for other purposes.");
			doc.Tag.Close("P");

			//Add image
			doc.Rect.String = "300 100 300 100";
			using var img = XImage.FromFile(Path.Combine(srcDir, @"images\aster1.jpg"), null);
			doc.Rect.SetRect(100, 100, img.Width, img.Height);
			var figure = doc.Tag.MakeTag("Image");
			figure.Attributes = new DictAtom();
			figure.Attributes["Alt"] = new StringAtom("A flower.");
			doc.Tag.Open(figure);
			doc.AddImageObject(img, true);
			doc.Tag.Close(figure.Type);

			//Add some rotated text marked with header tag (H1)
			doc.Rect.String = doc.MediaBox.String;
			doc.Rect.Magnify(0.5, 0.5);
			doc.FontSize = 30;
			doc.Transform.Rotate(45, doc.Pos.X, doc.Pos.Y);
			doc.Tag.Open("H1");
			doc.AddText("Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.");
			doc.Transform.Reset();
			doc.Tag.Close("H1");

			//Close container
			doc.Tag.Close("Container");

			// Dump tagged content and save pdf file
			doc.Save(Path.Combine(dstDir, "Simple.pdf"));
			var st = doc.Tag.GetStructure();
			st.UpdateActualText(true, true, " ");
			File.WriteAllText(Path.Combine(dstDir, "Simple.txt"), st.ExtractStructure().ToString());
		}

		/// <summary>
		/// Example of a dynamically created tagged pdf document read from XML. 
		/// </summary>
		static public void XmlExample(string inFile, string dstDir) {
			string name = Path.GetFileNameWithoutExtension(inFile);
			using var doc = XmlConverter.Create(inFile);
			doc.Save(Path.Combine(dstDir, name + ".pdf"));
			var st = doc.Tag.GetStructure();
			st.UpdateActualText(true, true, " ");
			File.WriteAllText(Path.Combine(dstDir, name + ".txt"), st.ExtractStructure().ToString());
		}
	}
}
