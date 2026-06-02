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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WebSupergoo.ABCpdf14;

namespace HtmlTables {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

		private string basePath;
        private void Form1_Load(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			basePath = Directory.GetParent(theBase).Parent.Parent.FullName + "\\";
		}

        private void button1_Click(object sender, EventArgs e) {
			HtmlToPdf(basePath + @"TestFiles\Nested.htm", basePath + @"TestFiles\Nested.pdf");
        }

		private void button2_Click(object sender, EventArgs e) {
			HtmlToPdf(basePath + @"TestFiles\Paging.htm", basePath + @"TestFiles\Paging.pdf");
		}

		private void button3_Click(object sender, EventArgs e) {
			HtmlToPdf(basePath + @"TestFiles\Invoice.htm", basePath + @"TestFiles\Invoice.pdf");
		}

		private void button4_Click(object sender, EventArgs e) {
			string[] files = Directory.GetFiles(basePath, "*.htm");
			foreach (string file in files) 
				HtmlToPdf(file, file.Replace(".htm", ".pdf"));
		}

		/// <summary>
		/// Convert an HTML file into a PDF one.
		/// </summary>
		/// <param name="inPath">The source HTML.</param>
		/// <param name="outPath">The output PDF.</param>
		void HtmlToPdf(string inPath, string outPath) {
			if (!File.Exists(inPath))
				return;

			Doc doc = new Doc();
			doc.Rect.Inset(100, 100);

			int page = 1;
			HtmlDoc h = new HtmlDoc(doc);
            h.MaxImageSize = new Size((int)maxImageWidth.Value, (int)maxImageHeight.Value);
            h.SetFile(inPath);
			while (!h.Drawn) {
				while (page > doc.PageNumber)
					doc.Page = doc.AddPage();
				if (addBackground.Checked) {
					string save = doc.Rect.String;
					doc.Rect.String = doc.MediaBox.String;
					doc.Color.String = "255 128 128";
					doc.FillRect();
					doc.Color.String = "0 0 0";
					doc.Rect.String = save;
				}
				XRect xr = h.Draw();
				if (outline.Checked)
					DrawContentOutlines(doc, h, xr);
                if (addFrame.Checked) {
                    string save = doc.Color.String;
                    doc.Width = 0.25;
                    doc.Color.String = "0 255 255"; // cyan
                    doc.FrameRect();
                    doc.Color.String = save;
                }
				if (deleteContent.Checked) {
					h.Delete();
					break;
				}
				if (deleteShift.Checked) {
					h.Delete();
					doc.Rect.Move(0, -200);
					h.Draw();
				}
				if (doc.PageCount > 200)
					break;
				page++;
			}

			for (int i = 1; i <= doc.PageCount; i++) {
				doc.PageNumber = i;
				doc.Flatten();
			}

			doc.Save(outPath);
			System.Diagnostics.Process.Start(outPath);
		}

		/// <summary>
		/// Draw outlines round content just added to the PDF to
		/// illustrate use of the GetContentArea method.
		/// </summary>
		/// <param name="doc">The Doc into which content has been added</param>
		/// <param name="h">The HtmlDoc which was used to add the content</param>
		void DrawContentOutlines(Doc doc, HtmlDoc html, XRect totalArea) {
			string saveRect = doc.Rect.String;
			doc.Color.String = "0 0 255"; // blue
			doc.Width = 1.0;
			// frame individual items of content
			List<XRect> rects = html.GetContentArea(true);
			foreach (XRect xr in rects) {
				doc.Rect.String = xr.String;
				doc.FrameRect();
			}
			// frame overall content area
			rects = html.GetContentArea(false);
            doc.Width = 0.75;
			doc.Color.String = "0 255 0"; // green
			doc.Rect.String = DeriveTotalContentArea(rects).String;
			doc.FrameRect();
			// frame overall table area
            doc.Width = 0.50;
			doc.Color.String = "255 0 0"; // red
			doc.Rect.String = totalArea.String;
			doc.FrameRect();
			doc.Rect.String = saveRect;
		}

		/// <summary>
		/// Convert a set of XRect areas into one union of the areas.
		/// </summary>
		/// <param name="rects">The list of XRects to combine.</param>
		/// <returns>The union of the list of XRects.</returns>
		public XRect DeriveTotalContentArea(List<XRect> rects) {
			if ((rects == null) || (rects.Count == 0))
				return null;
			double l = Double.MaxValue, r = Double.MinValue;
			double t = Double.MinValue, b = Double.MaxValue;
			foreach (XRect xr in rects) {
				l = Math.Min(l, xr.Left);
				r = Math.Max(r, xr.Right);
				t = Math.Max(t, xr.Top);
				b = Math.Min(b, xr.Bottom);
			}
			XRect xr2 = new XRect();
			xr2.SetRect(l, b, r - l, t - b);
			return xr2;
		}
    }
}