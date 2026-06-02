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
using System.Xml;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Atoms;
using WebSupergoo.ABCpdf14.Objects;


namespace ABCpdfControls {
	/// <summary>
	/// Annotated page content.
	/// </summary>
	internal class AnnotatedContent {
		private Doc mDoc;
		private NumberFormatInfo mFormatProvider;
		private const string kDefaultTextProperties = "0 Tc 0 Tw 100 Tz 0 TL 0 Ts";

		public AnnotatedContent(Doc inDoc) {
			mDoc = inDoc;
			mFormatProvider = new NumberFormatInfo();
			mFormatProvider.NumberDecimalSeparator = ".";
			mFormatProvider.NumberDecimalDigits = 5;
		}
		/// <summary>
		/// Extract text block information from the xml decription
		/// </summary>
		/// <param name="node">Xml node corresonding to the text block</param>
		/// <returns>Text Block</returns>
		private TextBlock ParseTextBlock(XmlNode node) {
			NumberFormatInfo theFormatProvider = new NumberFormatInfo();
			theFormatProvider.NumberDecimalSeparator = ".";
			XmlNode attrNode = null;

			float fontSize = 0;
			if ((attrNode = node.Attributes["font-size"]) != null) {
				fontSize = float.Parse(attrNode.Value, mFormatProvider);
				// SVG does not allow negative font size
				// so special leading '+' sign indicates negative font size
				if (attrNode.Value.Length > 0 && attrNode.Value[0] == '+')
					fontSize = -fontSize;
			}

			string text = "";
			float w = 0;
			if (node.ChildNodes.Count == 0 || (node.ChildNodes.Count > 0 && node.ChildNodes[0].Name != "tspan")) {
				text = node.InnerText;
				string lengthStr = node.Attributes["pdf_w1000"].Value;
				w = float.Parse(lengthStr, theFormatProvider) / 1000;
			}
			else {
				for (int j = 0; j < node.ChildNodes.Count; j++) {
					XmlNode childNode = node.ChildNodes.Item(j);
					if (childNode.Name == "tspan") {
						text += childNode.InnerText;
						string lengthStr = childNode.Attributes["pdf_w1000"].Value;
						w += float.Parse(lengthStr, theFormatProvider) / 1000;
						if ((attrNode = childNode.Attributes["dx"]) != null)
							w += float.Parse(attrNode.Value, theFormatProvider) / fontSize;
					}
				}
			}

			if (text == "" || w == 0)
				return null;

			float h = 1;
			GraphicsPath thePath = new GraphicsPath();
			thePath.AddRectangle(new RectangleF(0, -(float)0.2*h, w, h));

			string textMatrix = node.Attributes["pdf_Trm"].Value;
			string[] valuesStr = textMatrix.Split(' ');
			Matrix theMatrix = new Matrix(float.Parse(valuesStr[0], theFormatProvider), float.Parse(valuesStr[1], theFormatProvider), float.Parse(valuesStr[2], theFormatProvider), float.Parse(valuesStr[3], theFormatProvider), float.Parse(valuesStr[4], theFormatProvider), float.Parse(valuesStr[5], theFormatProvider));
			thePath.Transform(theMatrix);

			TextBlock theBlock = new TextBlock(text, thePath, node);
			if ((attrNode = node.Attributes["pdf_StreamID"]) != null)
				theBlock.StreamId = int.Parse(attrNode.Value);

			if ((attrNode = node.Attributes["pdf_StreamOffset"]) != null)
				theBlock.StreamOffset = int.Parse(attrNode.Value);

			if ((attrNode = node.Attributes["pdf_StreamLength"]) != null)
				theBlock.Length = int.Parse(attrNode.Value);

			if ((attrNode = node.Attributes["fill"]) != null) {
				string colorStr = attrNode.Value;
				colorStr = colorStr.Replace("rgb(", "");
				colorStr = colorStr.Replace(")", "");
				XColor theColor = new XColor();
				theColor.String = colorStr;
				theBlock.TextColor = theColor.Color;
			}
			else
				theBlock.TextColor = Color.Black;

			if ((attrNode = node.Attributes["font-family"]) != null)
				theBlock.FontFamily = attrNode.Value;

			theBlock.FontSize = fontSize;

			if ((attrNode = node.Attributes["pdf_Op"]) != null)
				theBlock.PDFOperator = attrNode.Value;

			if ((attrNode = node.Attributes["pdf_Tf"]) != null)
				theBlock.FontOperator = attrNode.Value;

			if ((attrNode = node.Attributes["pdf_Ts"]) != null)
				theBlock.TextRise = float.Parse(attrNode.Value, mFormatProvider);

			if ((attrNode = node.Attributes["pdf_Tw"]) != null)
				theBlock.WordSpacing = float.Parse(attrNode.Value, mFormatProvider);

			if ((attrNode = node.Attributes["pdf_Tc"]) != null)
				theBlock.CharacterSpacing = float.Parse(attrNode.Value, mFormatProvider);

			if ((attrNode = node.Attributes["pdf_TL"]) != null)
				theBlock.TextLeading = float.Parse(attrNode.Value, mFormatProvider);

			if ((attrNode = node.Attributes["pdf_Tz"]) != null)
				theBlock.HorizontalScaling = float.Parse(attrNode.Value, mFormatProvider);

			if ((attrNode = node.Attributes["pdf_nonStroke"]) != null)
				theBlock.TextColorOperator = attrNode.Value;

			theBlock.TextRenderingMatrix = theMatrix;

			return theBlock;
		}

		/// <summary>
		/// Returns Text Blocks from the page
		/// </summary>
		/// <param name="mDoc">Document</param>
		/// <returns>ArrayList containing text blocks</returns>
		public List<TextBlock> Parse(Doc mDoc) {
			List<TextBlock> theBlocks = new List<TextBlock>();
			mDoc.Flatten();
			string content = mDoc.GetText("SVG+");

			StringReader xmlReader = new StringReader(content);
			XmlDocument xmlDoc = new XmlDocument();

			try {
				xmlDoc.Load(xmlReader);
			}
			catch (Exception e) {
				MessageBox.Show("Error deconstructing SVG: " + e.Message);
			}

			theBlocks.Clear();
			XmlNode svg = xmlDoc["svg"];
			XmlNode node = svg != null && svg.FirstChild != null? svg.FirstChild: svg;
			while (node!=svg) {
				if (node.Name == "text") {
					TextBlock theBlock = ParseTextBlock(node);
					if (theBlock != null)
						theBlocks.Add(theBlock);
				}
				if (node.FirstChild != null)
					node = node.FirstChild;
				else if (node.NextSibling != null)
					node = node.NextSibling;
				else {
					do {
						node = node.ParentNode;
					} while (node != svg && node.NextSibling == null);
					if (node != svg)
						node = node.NextSibling;
				}
			}
			return theBlocks;
		}

		public void UpdateTextBlock(TextBlock inBlock, string inText) {
			if (inBlock != null && inBlock.StreamId != 0 && inBlock.StreamOffset > 0 && inBlock.Length > 0) {
				StreamObject streamObject = (StreamObject)mDoc.ObjectSoup[inBlock.StreamId];
				streamObject.Decompress();

				string stream = streamObject.GetText();

				stream = stream.Remove(inBlock.StreamOffset, inBlock.Length);

				string oldFontPath = "/Resources/Font/"+inBlock.FontOperator+":Ref";
				int oldFontId = mDoc.GetInfoInt(inBlock.StreamId, oldFontPath);
				if(oldFontId == 0)
					oldFontId = mDoc.GetInfoInt(mDoc.Page, oldFontPath);

				int fontId;
				string updatedString = UnicodeToFontEncoding(mDoc, oldFontId,
					inBlock.FontFamily?? "", inText, out fontId) + " Tj";
				string fontOp = fontId==0? "": GetFontOperator(inBlock.StreamId, fontId);
				if (fontOp != "")
					updatedString = string.Format(mFormatProvider,
						" /{0} {1:0.#####} Tf\r\n{2}\r\n/{3} {1:0.#####} Tf",
						fontOp, inBlock.FontSize, updatedString, inBlock.FontOperator);

				stream = stream.Insert(inBlock.StreamOffset, updatedString);
				streamObject.SetText(stream);
				inBlock.Length = updatedString.Length;
			}
		}

		private string UnicodeToFontEncoding(Doc inDoc, int inFontId, string inFont,
			string inText, out int outFontId)
		{
			string theText;
			FontObject fontObj = inFontId==0? null: inDoc.ObjectSoup[inFontId] as FontObject;
			if (fontObj != null) {
				int count;
				theText = fontObj.EncodeText(inText,
					FontObject.EncodingSupport.NoneIfAnyNotSupported, out count);
				if (count >= inText.Length) {
					outFontId = 0;
					return theText;
				}
				if (inFont != "") {
					inFontId = mDoc.AddFont(inFont);
					fontObj = inFontId==0? null: inDoc.ObjectSoup[inFontId] as FontObject;
					if (fontObj != null) {
						theText = fontObj.EncodeText(inText,
							FontObject.EncodingSupport.NoneIfAnyNotSupported, out count);
						if (count >= inText.Length) {
							outFontId = inFontId;
							return theText;
						}
					}
					mDoc.Delete(inFontId);
				}
			}
			outFontId = 0;
			theText = inText.Replace("\\", "\\\\");
			theText = theText.Replace("(", "\\(");
			theText = theText.Replace(")", "\\)");
			return theText;
		}

		public string GetFontOperator(int resObjId, int fontID) {
			string fontCommand = "";
			string fontStr = mDoc.GetInfo(resObjId, "/Resources/Font*");
			DictAtom fontDict = Atom.FromString(fontStr) as DictAtom;
			if(fontDict==null) {
				resObjId = mDoc.Page;
				fontStr = mDoc.GetInfo(resObjId, "/Resources/Font*");
				fontDict = Atom.FromString(fontStr) as DictAtom;
			}
			if (fontDict != null) {
				foreach (KeyValuePair<string, Atom> pair in fontDict) {
					RefAtom refAtom = pair.Value as RefAtom;
					if (refAtom != null && refAtom.ID == fontID) {
						fontCommand = pair.Key;
						break;
					}
				}
			}
			if (fontCommand == "") {
				if (fontDict == null)
					fontDict = new DictAtom();

				Atom fontRef = Atom.FromString(fontID.ToString()+" 0 R");
				fontCommand = "Fabc"+fontID.ToString();
				fontDict.Add(fontCommand, fontRef);
				mDoc.SetInfo(resObjId, "/Resources*/Font", fontDict.ToString());
			}

			return fontCommand;
		}

		public void UpdateTextBlock(TextBlock inBlock, string inText, Color inColor, string font) {
			if (inBlock != null && inBlock.StreamId != 0 && inBlock.StreamOffset > 0 && inBlock.Length > 0) {
				StreamObject streamObject = (StreamObject)mDoc.ObjectSoup[inBlock.StreamId];
				streamObject.Decompress();
				string stream = streamObject.GetText();
				stream = stream.Remove(inBlock.StreamOffset, inBlock.Length);

				int fontId = 0;
				string fontOp = "";
				if (font != "") {
					fontId = mDoc.AddFont(font);
					if (fontId != 0)
						fontOp = GetFontOperator(inBlock.StreamId, fontId);
				}

				string updatedString = inBlock.PDFOperator;
				bool sameText = inBlock.Text == inText;
				if (!sameText || fontId != 0) {
					string oldFontPath = "/Resources/Font/"+inBlock.FontOperator+":Ref";
					int oldFontId = mDoc.GetInfoInt(inBlock.StreamId, oldFontPath);
					if (oldFontId == 0)
						oldFontId = mDoc.GetInfoInt(mDoc.Page, oldFontPath);
					string oldEncoding = "";
					if (sameText && oldFontId != 0)
						oldEncoding = mDoc.GetInfo(oldFontId, "/Encoding*:Name");
					if (!sameText || oldEncoding == ""
						|| oldEncoding != mDoc.GetInfo(fontId, "/Encoding*:Name"))
					{
						updatedString = UnicodeToFontEncoding(mDoc,
							fontId==0? oldFontId: fontId,
							fontId!=0 || inBlock.FontFamily==null? "": inBlock.FontFamily,
							inText, out fontId) + " Tj";
						if (fontId != 0)
							fontOp = GetFontOperator(inBlock.StreamId, fontId);
					}
				}

				if (inBlock.TextColor != inColor) {
					string oldColorOP = inBlock.TextColorOperator?? string.Format(mFormatProvider,
						"{0:0.#####} {1:0.#####} {2:0.#####} rg",
						inBlock.TextColor.R / 255.0, inBlock.TextColor.G / 255.0, inBlock.TextColor.B / 255.0);
					updatedString = string.Format(mFormatProvider,
						" {0:0.#####} {1:0.#####} {2:0.#####} rg\r\n{3}\r\n{4}",
						inColor.R / 255.0, inColor.G / 255.0, inColor.B / 255.0, updatedString, oldColorOP);
				}

				if (fontOp != "")
					updatedString = string.Format(mFormatProvider,
						" /{0} {1:0.#####} Tf\r\n{2}\r\n/{3} {1:0.#####} Tf",
						fontOp, inBlock.FontSize, updatedString, inBlock.FontOperator);

				stream = stream.Insert(inBlock.StreamOffset, updatedString);
				streamObject.SetText(stream);
				inBlock.Length = updatedString.Length;
			}
		}

		//public string GetTextPropertiesString(TextBlock inBlock) {
		//	return " " + inBlock.TextRise.ToString(mFormatProvider) + " Ts " + 
		//		inBlock.WordSpacing.ToString(mFormatProvider) + " Tw " +
		//		inBlock.CharacterSpacing.ToString(mFormatProvider) + " Tc " +
		//		inBlock.TextLeading.ToString(mFormatProvider) + " TL " +
		//		inBlock.HorizontalScaling.ToString(mFormatProvider) + " Tz ";
		//}

		public void DeleteTextBlock(TextBlock inBlock) {
			if (inBlock != null && inBlock.StreamId != 0 && inBlock.StreamOffset > 0 && inBlock.Length > 0) {
				StreamObject streamObject = (StreamObject)mDoc.ObjectSoup[inBlock.StreamId];
				streamObject.Decompress();
				string stream = streamObject.GetText();

				if (inBlock.StreamOffset + inBlock.Length < stream.Length){
					stream = stream.Remove(inBlock.StreamOffset, inBlock.Length);
					streamObject.SetText(stream);
					inBlock.Length = 0;
				}
			}
		}

		public void MoveTextBlock(TextBlock inBlock, int xOffset, int yOffset) {
			Matrix moveMatrix = new Matrix(1, 0, 0, 1, xOffset, yOffset);
			TransformTextBlock(inBlock, moveMatrix);
		}


		public void TransformTextBlock(TextBlock inBlock, Matrix ctm) {
			if (inBlock != null && inBlock.StreamId != 0 && inBlock.StreamOffset > 0 && inBlock.Length > 0) {
				DeleteTextBlock(inBlock);
				string colorOP = inBlock.TextColorOperator?? string.Format(mFormatProvider,
					"{0:0.#####} {1:0.#####} {2:0.#####} rg",
					inBlock.TextColor.R / 255.0, inBlock.TextColor.G / 255.0, inBlock.TextColor.B / 255.0);
				inBlock.TextRenderingMatrix.Multiply(ctm, MatrixOrder.Append);
				float[] elements = inBlock.TextRenderingMatrix.Elements;
				// not using GetTextPropertiesString(inBlock)
				string textBlockStr = string.Format(mFormatProvider,
					"q\r\nBT\r\n"+kDefaultTextProperties
					+ "\r\n{0}\r\n{1:0.#####} {2:0.#####} {3:0.#####} {4:0.#####} {5:0.#####} {6:0.#####} Tm"
					+ "\r\n/{7} 1 Tf\r\n{8}\r\nET\r\nQ",
					colorOP, elements[0], elements[1], elements[2], elements[3], elements[4], elements[5],
					inBlock.FontOperator, inBlock.PDFOperator);
				mDoc.SetInfo(mDoc.FrameRect(), "stream", textBlockStr);
			}
		}
	}
}
