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
using System.IO;
using WebSupergoo.ABCpdf13;

namespace AdvancedGraphics
{
	class AdvancedGraphicsApp
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{	
			Doc theDoc = new Doc();
			StarStroke(theDoc);
			StarFill(theDoc);
			FillEvenOddRule(theDoc);
			StarClip(theDoc);
			BezierCurve(theDoc);
			SetLineJoin(theDoc);
			SetLineCap(theDoc);
			Dash(theDoc);
			Rotate(theDoc);
			Hexahedron(theDoc);
			Transparency(theDoc);
			BlendModes(theDoc);
			TextExamples(theDoc);
			FormXObject(theDoc);
			ClippedImages(theDoc);

			string theBase = Directory.GetCurrentDirectory();
			for(int i = 1, count = theDoc.PageCount; i<=count; ++i) {
				theDoc.PageNumber = i;
				theDoc.Flatten();
			}
			theDoc.Save(Directory.GetParent(theBase).Parent.FullName +"\\AdvancedGraphicsExamples.pdf");
			theDoc.Clear(); 
		}
		/// <summary>
		/// Add example description on the top of the page
		/// </summary>
		/// <param name="theDoc"></param>
		/// <param name="description"></param>
		static void AddDescription(Doc theDoc, string description)
		{
			PDFContent theContent = new PDFContent(theDoc);
			theContent.SaveState();
			theContent.BeginText();
			int id = theDoc.AddFont("Times-Italic");
			theContent.SetFont(id, 30);
			theContent.SetTextMatrix(1, 0, 0, 1, 30, 740);
			theContent.ShowTextString(description);
			theContent.EndText();
			theContent.RestoreState();
			theContent.AddToDoc();
		}
		/// <summary>
		/// Start new page, set active rect to MediaBox, set active color to black
		/// </summary>
		/// <param name="theDoc"></param>
		static void StartNewPage(Doc theDoc)
		{
			theDoc.Page = theDoc.AddPage();
			theDoc.Rect.String = theDoc.MediaBox.String;
			theDoc.Color.String = "0 0 0";
		}
		/// <summary>
		/// Stroke operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void StarStroke(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Stroke path example");
			PDFContent theContent = new PDFContent(theDoc);
			
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
		}
		/// <summary>
		/// Fill operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void StarFill(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Fill path example");
			PDFContent theContent = new PDFContent(theDoc);
			
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
		}
		/// <summary>
		/// Fill using Even-odd filling rule operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void FillEvenOddRule(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Even-Odd Fillling rule example");
			PDFContent theContent = new PDFContent(theDoc);
			
			double size =100;
			double centerX = size;
			double centerY = size;

			theContent.Transform(2, 0, 0, 2, 100, 200);
			theContent.Rect(0, 0 , size, size);
			theContent.Rect(size, size , size, size);

			for (int i = 0; i < 4; i++)
			{
				theContent.Arc(0, 360, centerX - size/2, centerY ,size/2, size/2, 0, true);
				theContent.Arc(0, 360, centerX + size/2, centerY ,size/2, size/2, 0, true);
				theContent.Arc(0, 360, centerX, centerY + size/2 ,size/2, size/2, 0, true);
				theContent.Arc(0, 360, centerX, centerY - size/2, size/2 , size/2, 0, true);
				size = size / 2;
			}
			theContent.FillEvenOddRule();
			theContent.AddToDoc();
		}

		/// <summary>
		/// Bezier curve operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void BezierCurve(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Bezier curve example");
			PDFContent	theContent = new PDFContent(theDoc);

			theContent.SaveState();
			theContent.SetLineWidth(30);
			theContent.Move(100, 50);
			theContent.Bezier(200, 650, 400, 550, 500, 250);
			theContent.Stroke();
			theContent.RestoreState();

			// annotate Bezier curve in red
			theDoc.Color.String = "255 0 0";
			theDoc.Width = 20;
			theDoc.FontSize = 30;
			theDoc.Pos.String = "100 50";
			theDoc.AddText("p0 (current point)");
			theDoc.Pos.String = "200 650";
			theDoc.Pos.Y = theDoc.Pos.Y + theDoc.FontSize;
			theDoc.AddText("p1 (x1, y1)");
			theDoc.Pos.String = "400 550";
			theDoc.Pos.Y = theDoc.Pos.Y + theDoc.FontSize;
			theDoc.AddText("p2 (x2, y2)");
			theDoc.Pos.String = "500 250";
			theDoc.Pos.X = theDoc.Pos.X - theDoc.FontSize;
			theDoc.AddText("p3 (x3, y3)");
			theDoc.AddLine(100, 50, 200, 650);
			theDoc.AddLine(400, 550, 500, 250);
			theContent.AddToDoc();
		}

		/// <summary>
		/// Clipping operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void StarClip(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Clipping path example");
			PDFContent theContent = new PDFContent(theDoc);
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
		}
		
		/// <summary>
		/// Line cap operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void SetLineCap(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Line cap example");

			PDFContent theContent = new PDFContent(theDoc);
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
			theDoc.FontSize = 48;
			theDoc.Pos.String = "50 720";
			theDoc.AddText("0 - Butt Cap");
			theDoc.Pos.String = "50 520";
			theDoc.AddText("1 - Round Cap");
			theDoc.Pos.String = "50 320";
			int id = theDoc.AddText("2 - Projecting Square Cap");
			theDoc.Width = 20;

			theDoc.Color.String = "255 255 255";
			theDoc.AddLine(100, 200, 500, 200);
			theDoc.Rect.String = "80 180 120 220";
			theDoc.FillRect(20, 20);
			theDoc.Rect.String = "480 180 520 220";
			theDoc.FillRect(20, 20);
			theDoc.AddLine(100, 400, 500, 400);
			theDoc.Rect.String = "80 380 120 420";
			theDoc.FillRect(20, 20);
			theDoc.Rect.String = "480 380 520 420";
			theDoc.FillRect(20, 20);
			theDoc.AddLine(100, 600, 500, 600);
			theDoc.Rect.String = "80 580 120 620";
			theDoc.FillRect(20, 20);
			theDoc.Rect.String = "480 580 520 620";
			theDoc.FillRect(20, 20);
			theDoc.Color.String = "0 0 0";
		}

		/// <summary>
		/// Line join operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void SetLineJoin(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Line join example");
			PDFContent theContent = new PDFContent(theDoc);

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

			theDoc.FontSize = 48;
			theDoc.Pos.String = "50 700";
			theDoc.AddText("0 - Miter");
			theDoc.Pos.String = "50 500";
			theDoc.AddText("1 - Round ");
			theDoc.Pos.String = "50 300";
			theDoc.AddText("2 - Bevel");
			theDoc.Width = 10;
			theDoc.Color.String = "255 255 255";
			theDoc.AddLine(300, 500, 400, 700);
			theDoc.AddLine(400, 700, 500, 500);
			theDoc.Rect.String = "390 690 410 710";
			theDoc.FillRect(10, 10);
			theDoc.AddLine(300, 300, 400, 500);
			theDoc.AddLine(400, 500, 500, 300);
			theDoc.Rect.String = "390 490 410 510";
			theDoc.FillRect(10, 10);
			theDoc.AddLine(300, 100, 400, 300);
			theDoc.AddLine(400, 300, 500, 100);
			theDoc.Rect.String = "390 290 410 310";
			theDoc.FillRect(10, 10);
			theDoc.Color.String = "0 0 0";
		}

		/// <summary>
		/// Dash pattern operator example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void Dash(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Dash pattern example");
			PDFContent theContent = new PDFContent(theDoc);
			
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
			theDoc.Color.String = "0 0 0";
			theDoc.FontSize = 36;
			theDoc.Pos.String = "50 710";
			theDoc.AddText("[ ] 0 - no dashes");
			theDoc.Pos.String = "50 560";
			theDoc.AddText("[ 90 ] 0 - 90 on, 90 off...");
			theDoc.Pos.String = "50 410";
			theDoc.AddText("[ 60 ] 30 - 30 on, 60 off, 60 on...");
			theDoc.Pos.String = "50 260";
			theDoc.AddText("[ 60 30 ] 0 - 60 on, 30 off, 60 on...");
			
			// add dashed lines
			theContent.AddToDoc();
		}
		/// <summary>
		/// Example of cordinate space transformation
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void Rotate(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Coordinate transformation example");
			
			PDFContent star = new PDFContent(theDoc);
			star.Move(124, 108);
			star.Line(300, 650);
			star.Line(476, 108);
			star.Line(15, 443);
			star.Line(585, 443);
			star.Close();
			star.Stroke();

			PDFContent theContent = new PDFContent(theDoc);
			theContent.SaveState();
			theContent.SetLineWidth(30);
			theContent.SetLineJoin(2);
			theContent.AddContent(star);
			theContent.SetRGBStrokeColor(1, 0, 0);
			theContent.Transform(0.7, 0.7, -0.7, 0.7, 0, 0);
			theContent.AddContent(star);
			theContent.RestoreState();
			theContent.AddToDoc();
		}
		/// <summary>
		/// Example of cordinate space transformation
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void Hexahedron(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Coordinate transformation example");
			double side = 200;
			double offset = side*Math.Sqrt(3)/2;
			XPoint center = new XPoint();
			center.X = 0;
			center.Y = 0;
			//Construct hexahedron figure
			PDFContent hexahedron = new PDFContent(theDoc);
			hexahedron.Move(center.X - side/2, center.Y - offset);
			hexahedron.Line(center.X + side/2, center.Y - offset);
			hexahedron.Line(center.X + side, center.Y);
			hexahedron.Line(center.X + side/2, center.Y + offset);
			hexahedron.Line(center.X - side/2, center.Y + offset);
			hexahedron.Line(center.X - side, center.Y);
			hexahedron.Close();
			hexahedron.Fill();

			PDFContent theContent = new PDFContent(theDoc);

			const double pi = 3.141592;

			double scale = 1.5*Math.Sqrt(3)/2;
			double angle = 15;

			for (int i = 1; i < 8; i++ )
			{
				theContent.SaveState();
				theContent.SetRGBNonStrokeColor(0.1*i, 0.1*i, 0.1*i);
				//Rotate hexahedron
				theContent.Transform(Math.Cos(angle*i*pi/180), Math.Sin(angle*i*pi/180), - Math.Sin(angle*i*pi/180), Math.Cos(angle*i*pi/180), 300, 400);
				//Scale hexahedron
				theContent.Transform(scale, 0, 0, scale, 0, 0);
				scale *= 0.9 * Math.Sqrt(3)/2;
				theContent.AddContent(hexahedron);
				theContent.RestoreState();
			}
			theContent.AddToDoc();
		}
		/// <summary>
		/// Example of usage of different text operators
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void TextExamples(Doc theDoc)
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Text examples");
			PDFContent theContent = new PDFContent(theDoc);
			theContent.Transform(1, 0, 0, 1, 0, 620);

			//Character spacing
			theContent.SaveState();
			theContent.BeginText();
			int HelveticaBoldID = theDoc.AddFont("Helvetica-Bold");
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Character spacing:");
			int HelveticaID = theDoc.AddFont("Helvetica");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Tc = 0 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Character");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Tc = 5");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetCharacterSpacing(5);
			theContent.ShowTextString("Character");
			theContent.EndText();
			theContent.RestoreState();

			//Word spacing
			theContent.Transform(1, 0, 0, 1, 0, -110);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Word spacing:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Tw = 0 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Word Space");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Tw = 10");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetWordSpacing(10);
			theContent.ShowTextString("Word Space");
			theContent.EndText();
			theContent.RestoreState();

			//Horizontal scaling
			theContent.Transform(1, 0, 0, 1, 0, -110);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Horizontal scaling:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Th = 100 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Word");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Th = 50");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetHorizontalScaling(50);
			theContent.ShowTextString("WordWord");
			theContent.EndText();
			theContent.RestoreState();

			//Text rise
			theContent.Transform(1, 0, 0, 1, 0, -80);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Text rise:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("This text is");
			theContent.SetTextRise(5);
			theContent.ShowTextString("superscripted");
			theContent.EndText();
			theContent.RestoreState();

			//Text rendering modes
			theContent.Transform(1, 0, 0, 1, 0, -90);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 40);
			theContent.ShowTextString("Text rendering modes:");

			theContent.SetFont(HelveticaID, 32);
			for (int i = 0; i < 8; i++ )
			{
				theContent.SetTextRenderingMode((PDFContent.TextRenderingMode) i );
				theContent.SetTextMatrix(1, 0, 0, 1, 50 * (i+1), 0);
				theContent.ShowTextString("R");
			}
			theContent.EndText();
			theContent.RestoreState();
			theContent.AddToDoc();
		}
		/// <summary>
		/// Example of transparent objects
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void Transparency( Doc theDoc )
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Transparency example");

			PDFContent theContent = new PDFContent(theDoc);
			theContent.SaveState();
			theContent.SetRGBNonStrokeColor(0.9, 0.9, 1);
			//Add background rect
			theContent.Rect(130, 200, 350, 350);
			theContent.Fill();

			theContent.SetRGBNonStrokeColor(0, 0, 1);
			theContent.SetNonStrokeAlpha(0.6);

			//Add transparent circles
			theContent.Arc(0, 360, 305, 300, 100, 100, 0, true);
			theContent.Fill();
			theContent.SetRGBNonStrokeColor(1, 0, 0);
			theContent.Arc(0, 360, 230, 450, 100, 100, 0, true);
			theContent.Fill();
			theContent.SetRGBNonStrokeColor(0, 1, 0);
			theContent.Arc(0, 360, 380, 450, 100, 100, 0, true);
			theContent.Fill();
			theContent.RestoreState();
			theContent.AddToDoc();
		}
		/// <summary>
		/// Different blend modes example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void BlendModes( Doc theDoc )
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Different blending modes example");

			//Add background
			PDFContent background = new PDFContent(theDoc);
			background.SaveState();
			background.SetRGBNonStrokeColor(0.5, 0.5, 1);
			background.Rect(20, 120, theDoc.MediaBox.Width - 40 , theDoc.MediaBox.Height -230);
			background.Fill();
			background.RestoreState();
			background.AddToDoc();

			string theBase = Directory.GetCurrentDirectory();
			XImage sampleImage = new XImage();
			sampleImage.SetFile((Directory.GetParent(theBase).Parent.FullName +"\\pic.png"));
			double width = sampleImage.Width*2/3;
			double height = sampleImage.Height*2/3;

			theDoc.Rect.Position(23, 500);
			theDoc.Rect.Width = width;
			theDoc.Rect.Height = height;
			int imageID = theDoc.AddImage(sampleImage);
			PDFContent theContent = new PDFContent(theDoc);


			string[] blendModes = {"Normal", "Multiply", "Screen", "Overlay", "Darken",
									"Lighten", "ColorDodge", "ColorBurn", "HardLight", "Difference", "Exclusion",
									"Hue", "Saturation", "Color", "Luminosity"};

			int HelveticaID = theDoc.AddFont("Helvetica");

			//Draw images with different blend modes
			for (int i = 0; i < blendModes.Length; i++)
			{
				if (i > 0) // Don't show 1st image, because we already have done it with Doc.AddImage
				{
					theContent.SaveState();
					theContent.Transform(width, 0, 0, height, 23 + (width+10)*(i % 5), 500- (height+40) * (i / 5));
					theContent.SetBlendMode(blendModes[i]);
					theContent.DoImage(imageID);
					theContent.RestoreState();
				}
				//Print blend mode name
				theContent.SaveState();
				theContent.Transform(1, 0, 0, 1, 23 + (width+10)*(i % 5), 500- (height+40) * (i / 5) + height + 10);
				theContent.BeginText();
				theContent.SetFont(HelveticaID, 16);
				theContent.ShowTextString(blendModes[i]);
				theContent.EndText();
				theContent.RestoreState();
			}
			theContent.AddToDoc();
		}
		/// <summary>
		/// Form XObject example
		/// </summary>
		/// <param name="theDoc">Output doc</param>

		static void FormXObject(Doc theDoc )
		{
			StartNewPage(theDoc);
			AddDescription(theDoc, "Form XObject example");

			PDFBoundedContent theContent = new PDFBoundedContent(theDoc);
			theContent.Transform(1, 0, 0, 1, 0, 620);

			//Character spacing
			theContent.SaveState();
			theContent.BeginText();
			int HelveticaBoldID = theDoc.AddFont("Helvetica-Bold");
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Character spacing:");
			int HelveticaID = theDoc.AddFont("Helvetica");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Tc = 0 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Character");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Tc = 5");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetCharacterSpacing(5);
			theContent.ShowTextString("Character");
			theContent.EndText();
			theContent.RestoreState();

			//Word spacing
			theContent.Transform(1, 0, 0, 1, 0, -110);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Word spacing:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Tw = 0 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Word Space");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Tw = 10");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetWordSpacing(10);
			theContent.ShowTextString("Word Space");
			theContent.EndText();
			theContent.RestoreState();

			//Horizontal scaling
			theContent.Transform(1, 0, 0, 1, 0, -110);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 60);
			theContent.ShowTextString("Horizontal scaling:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Th = 100 (default)");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 30);
			theContent.SetFont(HelveticaID, 32);
			theContent.ShowTextString("Word");

			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("Th = 50");
			theContent.SetTextMatrix(1, 0, 0, 1, 250, 0);
			theContent.SetFont(HelveticaID, 32);
			theContent.SetHorizontalScaling(50);
			theContent.ShowTextString("WordWord");
			theContent.EndText();
			theContent.RestoreState();

			//Text rise
			theContent.Transform(1, 0, 0, 1, 0, -80);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 30);
			theContent.ShowTextString("Text rise:");
			theContent.SetFont(HelveticaID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 0);
			theContent.ShowTextString("This text is");
			theContent.SetTextRise(5);
			theContent.ShowTextString("superscripted");
			theContent.EndText();
			theContent.RestoreState();

			//Text rendering modes
			theContent.Transform(1, 0, 0, 1, 0, -90);
			theContent.SaveState();
			theContent.BeginText();
			theContent.SetFont(HelveticaBoldID, 16);
			theContent.SetTextMatrix(1, 0, 0, 1, 50, 40);
			theContent.ShowTextString("Text rendering modes:");

			theContent.SetFont(HelveticaID, 32);
			for (int i = 0; i < 8; i++ )
			{
				theContent.SetTextRenderingMode((PDFContent.TextRenderingMode) i );
				theContent.SetTextMatrix(1, 0, 0, 1, 50 * (i+1), 0);
				theContent.ShowTextString("R");
			}
			theContent.EndText();
			theContent.RestoreState();
			PDFFormXObjectID theTextID = theContent.WriteToFormXObject();
			theDoc.GetInfo(theTextID.ID, "Compress");

			theContent = new PDFBoundedContent(theDoc);
			theContent.SaveState();
			theContent.Transform(0.5, 0, 0.25, 0.5, 0, 310);
			theContent.DoFormXObject(theTextID);
			theContent.RestoreState();

			theContent.SaveState();
			theContent.SetNonStrokeAlpha(0.1);
			theContent.SetRGBNonStrokeColor(0, 0, 1);
			//Add background rect
			theContent.Rect(130, 200, 350, 350);
			theContent.Fill();

			theContent.SetRGBNonStrokeColor(0, 0, 1);
			theContent.SetNonStrokeAlpha(0.6);

			//Add transparent circles
			theContent.Arc(0, 360, 305, 300, 100, 100, 0, true);
			theContent.Fill();
			theContent.SetRGBNonStrokeColor(1, 0, 0);
			theContent.Arc(0, 360, 230, 450, 100, 100, 0, true);
			theContent.Fill();
			theContent.SetRGBNonStrokeColor(0, 1, 0);
			theContent.Arc(0, 360, 380, 450, 100, 100, 0, true);
			theContent.Fill();
			theContent.RestoreState();
			PDFFormXObjectID theTransparencyID = theContent.WriteToFormXObject();
			theDoc.GetInfo(theTransparencyID.ID, "Compress");

			PDFContent thePageContent = new PDFContent(theDoc);
			thePageContent.SaveState();
			thePageContent.Transform(0.5, 0, 0, 0.5, 0, 310);
			thePageContent.Rect(theTextID.Bounds.X, theTextID.Bounds.Y,
				theTextID.Bounds.Width, theTextID.Bounds.Height);
			thePageContent.Stroke();
			thePageContent.DoFormXObject(theTextID);
			thePageContent.RestoreState();
			thePageContent.SaveState();
			thePageContent.Transform(0.5, 0, 0, 0.5, 300, 310);
			thePageContent.DoFormXObject(theTextID);
			thePageContent.RestoreState();

			thePageContent.SaveState();
			thePageContent.Transform(0.5, 0, 0, 0.5, 0, 0);
			thePageContent.Rect(theTransparencyID.Bounds.X, theTransparencyID.Bounds.Y,
				theTransparencyID.Bounds.Width, theTransparencyID.Bounds.Height);
			thePageContent.Stroke();
			thePageContent.DoFormXObject(theTransparencyID);
			thePageContent.RestoreState();
			thePageContent.SaveState();
			thePageContent.Transform(0.5, 0, 0, 0.5, 300, 0);
			thePageContent.DoFormXObject(theTransparencyID);
			thePageContent.RestoreState();
			thePageContent.AddToDoc();
		}

		/// <summary>
		/// Clipped images example
		/// </summary>
		/// <param name="theDoc">Output doc</param>
		static void ClippedImages(Doc theDoc) {
			StartNewPage(theDoc);
			AddDescription(theDoc, "Clipped images example");

			string theBase = Directory.GetCurrentDirectory();
			using (XImage image = new XImage()) {
				image.SetFile((Directory.GetParent(theBase).Parent.FullName + "\\pic.png"));
				double width = image.Width, height = image.Height;
				double x = (theDoc.MediaBox.Width - width) / 2;
				double y = (theDoc.MediaBox.Height - height) / 2;

				PDFContent theContent = new PDFContent(theDoc);
				string name = theContent.AddImage(image);
				theContent.SaveState();
				theContent.Transform(width, 0, 0, height, x, y);
				theContent.Arc(0, 360, 0.5, 0.5, 0.5, 0.5, 0, true);
				theContent.Close();
				theContent.Clip();
				theContent.DoImage(name);
				theContent.RestoreState();

				theContent.AddToDoc();
			}
		}
	}
}


