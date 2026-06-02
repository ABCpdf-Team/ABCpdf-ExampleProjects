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
using WebSupergoo.ABCpdf14;

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for PDFUtilities.
	/// </summary>
	public static class PDFUtilities {
		public static int GetPageRotation(Doc theDoc) {
			return (theDoc != null) ? theDoc.GetInfoInt(theDoc.Page, "Rotate") : 0;
		}

		public static void SetPageRotation(Doc theDoc, int theAngle) {
			if (theDoc == null)
				return;
			theDoc.SetInfo(theDoc.Page, "/Rotate:Num", theAngle.ToString());
		}

		public static bool AddTextBlock(Doc theDoc, string text, XRect rect, int fontSize, Color fontColor, bool fitToRect) {
			int saveFontSize = theDoc.FontSize;
			string saveFontColor = theDoc.Color.String;
			string saveFontRect = theDoc.Rect.String;

			theDoc.Rect.String = rect.String;
			theDoc.Color.Color = fontColor;

			if (fitToRect)
				FitContents(theDoc, text);
			else {
				theDoc.FontSize = fontSize;
				theDoc.AddText(text);
			}


			theDoc.FontSize = saveFontSize;
			theDoc.Color.String = saveFontColor;
			theDoc.Rect.String = saveFontRect;
			return true;
		}

		private static int FitContents(Doc theDoc, string contents) {
			double oldTextSize = theDoc.TextStyle.Size;
			string oldPos = theDoc.Pos.String;
			int max = 200;
			int min = 1;
			double precision = 1;

			while (Math.Abs(max - min) > precision) {
				theDoc.TextStyle.Size = (max + min) / 2;
				theDoc.Rect.String = theDoc.Rect.String;
				int id = theDoc.AddText(contents);

				if ( id == 0 || theDoc.Chainable(id) )
					max = (int)theDoc.TextStyle.Size;
				else
					min = (int)theDoc.TextStyle.Size;
				theDoc.Delete(id);
			}
			theDoc.TextStyle.Size = min;
			theDoc.Rect.String = theDoc.Rect.String;
			int id_final = theDoc.AddText(contents);
			theDoc.TextStyle.Size = oldTextSize;
			return id_final;
		}
	}
}
