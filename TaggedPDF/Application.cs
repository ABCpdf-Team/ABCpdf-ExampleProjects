// ===========================================================================
//	©2013-2024 WebSupergoo. All rights reserved.
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
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;


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
			string theIn = theBase + @"\Input\";
			string theOut = theBase + @"\Output\";

			// simple example first
			SimpleExample(theIn, theOut);

			// then xml based example
			string[] theFiles = Directory.GetFiles(theIn);
			foreach (string inFile in theFiles) {
				string outFile = theOut + Path.GetFileNameWithoutExtension(inFile) + ".pdf";
				XmlConverter converter = new XmlConverter();
				converter.Convert(inFile, outFile);
			}
		}

		/// <summary>
		/// Example of a dynamically created tagged pdf document  
		/// </summary>
		static public void SimpleExample(string inputDirectory, string outputDirectory) {
			// Create new doc and tagged content for it
			Doc theDoc = new Doc();
			TaggedContent theContent = new TaggedContent(theDoc);

			//Add a container tag
			theContent.BeginTaggedItem("Container");

			//Add a paragraph of text
			theDoc.Rect.Inset(40, 40);
			theDoc.FontSize = 20;
			theContent.AddTaggedText("P", "Tagged PDF (PDF 1.4) is a stylized use of PDF that builds on the logical structure framework described in Section 10.6, “Logical Structure.” It defines a set of standard structure types and attributes that allow page content (text, graphics, andimages) to be extracted and reused for other purposes.");

			//Add image
			theDoc.Rect.String = "300 100 300 100";
			theContent.AddTaggedImage("Image", inputDirectory + @"images\aster1.jpg");

			//Add some rotated text marked with header tag (H1)
			theDoc.Rect.String = theDoc.MediaBox.String;
			theDoc.Rect.Magnify(0.5, 0.5);
			theDoc.FontSize = 30;
			theDoc.Transform.Rotate(45, theDoc.Pos.X, theDoc.Pos.Y);
			theContent.AddTaggedText("H1", "Gallia est omnis divisa in partes tres, quarum unam incolunt Belgae, aliam Aquitani, tertiam qui ipsorum lingua Celtae, nostra Galli appellantur.");
			theDoc.Transform.Reset();

			//Close container
			theContent.EndTaggedItem();

			// Dump tagged content and save pdf file
			theContent.AddToDoc();
			theDoc.Save(outputDirectory + "Simple.pdf");
		}
	}
}
