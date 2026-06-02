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
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Globalization;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Atoms;

namespace TaggedPDF {
	/// <summary>
	/// Parses xml files and converts them to tagged pdf
	/// </summary>
	public class XmlConverter {
		protected enum TextFormat {
			/// <summary>
			/// No format
			/// </summary>
			None,
			/// <summary>
			/// HTML styled.  Allows a single structure elements for
			/// text with many styles.  The XML should use &lt; and &gt;
			/// </summary>
			Html,
			/// <summary>
			/// Pre-formatted.  Respects new line characters
			/// </summary>
			Pre
		}
		protected sealed class BackgroundElement {
			/// <summary>
			/// Rectangle
			/// </summary>
			public string Rect;
			/// <summary>
			/// Background color
			/// </summary>
			public string BackgroundColor;
			/// <summary>
			/// Background image
			/// </summary>
			public string BackgroundImage;

			/// <summary>
			/// Constructor
			/// </summary>
			public BackgroundElement() {
			}
		}

		#region Members
		/// <summary>
		/// Base directory for resolving relative URIs
		/// </summary>
		private string mBaseDirectory;
		/// <summary>
		/// Parent document
		/// </summary>
		protected Doc mDoc;
		/// <summary>
		/// Xml reader for parsing document
		/// </summary>
		protected XmlTextReader mReader;
		/// <summary>
		/// The last page with the background set
		/// </summary>
		protected int mBackgroundSetPage;
		/// <summary>
		/// The last background set
		/// </summary>
		protected BackgroundElement mBackground;
		/// <summary>
		/// Stack of background items (e.g. body tags)
		/// </summary>
		protected Stack<BackgroundElement> mBackgroundStack = new Stack<BackgroundElement>();
		/// <summary>
		/// Stack of text formats
		/// </summary>
		protected Stack<TextFormat> mTextFormatStack = new Stack<TextFormat>();
		/// <summary>
		/// Stack of font sizes
		/// </summary>
		protected Stack<double> mFontSizeStack = new Stack<double>();
		/// <summary>
		/// Stack of font faces
		/// </summary>
		protected Stack<string> mFontFaceStack = new Stack<string>();
		/// <summary>
		/// Stack of font colors
		/// </summary>
		protected Stack<string> mFontColorStack = new Stack<string>();
		/// <summary>
		/// Stack of paragraph alignments
		/// </summary>
		protected Stack<double> mAlignStack = new Stack<double>();
		/// <summary>
		/// Stack of text styles
		/// </summary>
		protected Stack<XTextStyle> mTextStyleStack = new Stack<XTextStyle>();
		/// <summary>
		/// Height of the current line
		/// </summary>
		protected double mNewLineAdvance;
		#endregion

		#region TopLevel
		protected XmlConverter() {
			Init();
		}
		protected Doc Load(string fileName) {
			mBaseDirectory = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;

			mDoc = new Doc();
			Init();

			try {
				mReader = new XmlTextReader(fileName);
				mReader.WhitespaceHandling = WhitespaceHandling.None;

				while (mReader.Read()) {
					switch (mReader.NodeType) {
						case XmlNodeType.Element:
							string tagName = mReader.Name;
							ProcessTag();
							break;
						case XmlNodeType.Text:
							string text = mReader.Value;
							ProcessText(text);
							break;
						case XmlNodeType.EndElement:
							ProcessCloseTag(mReader);
							break;
					}
				}
			}
			finally {
				if (mReader != null)
					mReader.Close();
			}
			return mDoc;
		}
		#endregion

		#region Handlers
		/// <summary>
		/// Handle new paragraph tag P
		/// </summary>
		protected void HandleP() {
			mDoc.Pos.X = mDoc.Rect.Left;

			XTextStyle theTextStyle = new XTextStyle();
			theTextStyle.String = mTextStyleStack.Peek().String;

			string attr;
			if (GetAttribute("fontsize", out attr))
				mFontSizeStack.Push(double.Parse(attr,
					NumberFormatInfo.InvariantInfo));
			else
				mFontSizeStack.Push(mFontSizeStack.Peek());

			ExtractParagraphAttributes(theTextStyle);

			mTextStyleStack.Push(theTextStyle);

		}
		/// <summary>
		/// Handle end of the paragraph
		/// </summary>
		protected void HandleCloseP() {
			mTextFormatStack.Pop();
			mAlignStack.Pop();
			mFontSizeStack.Pop();
			XTextStyle textStyle = mTextStyleStack.Pop();
			mDoc.Pos.Y -= textStyle.ParaSpacing + mNewLineAdvance;
			mNewLineAdvance = 0;
		}
		/// <summary>
		/// Handle headers tag (H1, H2 ... H6)
		/// </summary>
		protected void HandleH() {
			mDoc.Pos.X = mDoc.Rect.Left;

			XTextStyle theTextStyle = new XTextStyle();
			theTextStyle.String = mTextStyleStack.Peek().String;

			switch (mReader.Name.ToLower()) {
				case "h1":
					mFontSizeStack.Push(28);
					break;
				case "h2":
					mFontSizeStack.Push(24);
					break;
				case "h3":
					mFontSizeStack.Push(20);
					break;
				case "h4":
					mFontSizeStack.Push(16);
					break;
				case "h5":
					mFontSizeStack.Push(10);
					break;
				case "h6":
					mFontSizeStack.Push(6);
					break;
			}
			ExtractParagraphAttributes(theTextStyle);
			mTextStyleStack.Push(theTextStyle);
		}
		/// <summary>
		/// Handle Font tag
		/// </summary>
		protected void HandleFont() {
			string attr;
			if (GetAttribute("face", out attr))
				mFontFaceStack.Push(attr);
			else
				mFontFaceStack.Push(mFontFaceStack.Peek());

			if (GetAttribute("size", out attr))
				mFontSizeStack.Push(double.Parse(attr,
					NumberFormatInfo.InvariantInfo));
			else
				mFontSizeStack.Push(mFontSizeStack.Peek());

			if (GetAttribute("color", out attr))
				mFontColorStack.Push(attr);
			else
				mFontColorStack.Push(mFontColorStack.Peek());

		}
		/// <summary>
		/// Handle end of the font tag
		/// </summary>
		protected void HandleCloseFont() {
			mFontFaceStack.Pop();
			mFontSizeStack.Pop();
			mFontColorStack.Pop();
		}
		/// <summary>
		/// Handle image tag
		/// </summary>
		protected void HandleImage() {
			string alt, src;
			GetAttribute("alt", out alt);
			if (GetAttribute("src", out src))
				ProcessImage(src, alt);
		}
		/// <summary>
		/// Handle B tag
		/// </summary>
		protected void HandleB() {
			XTextStyle theStyle = new XTextStyle();
			theStyle.String = mTextStyleStack.Peek().String;
			theStyle.Bold = true;
			mTextStyleStack.Push(theStyle);
		}
		/// <summary>
		/// Handle end of the B tag
		/// </summary>
		protected void HandleCloseB() {
			mTextStyleStack.Pop();
		}
		/// <summary>
		/// Handle I tag
		/// </summary>
		protected void HandleI() {
			XTextStyle theStyle = new XTextStyle();
			theStyle.String = mTextStyleStack.Peek().String;
			theStyle.Italic = true;
			mTextStyleStack.Push(theStyle);
		}
		/// <summary>
		/// Handle end of the I tag
		/// </summary>
		protected void HandleCloseI() {
			mTextStyleStack.Pop();
		}
		/// <summary>
		/// Handle U tag
		/// </summary>
		protected void HandleU() {
			XTextStyle theStyle = new XTextStyle();
			theStyle.String = mTextStyleStack.Peek().String;
			theStyle.Underline = true;
			mTextStyleStack.Push(theStyle);
		}
		/// <summary>
		/// Handle end of the U tag
		/// </summary>
		protected void HandleCloseU() {
			mTextStyleStack.Pop();
		}
		/// <summary>
		/// Handle Strike tag
		/// </summary>
		protected void HandleStrike() {
			XTextStyle theStyle = new XTextStyle();
			theStyle.String = mTextStyleStack.Peek().String;
			theStyle.Strike = true;
			mTextStyleStack.Push(theStyle);
		}
		/// <summary>
		/// Handle end of the Strike tag
		/// </summary>
		protected void HandleCloseStrike() {
			mTextStyleStack.Pop();
		}
		/// <summary>
		/// Handle BR tag
		/// </summary>
		protected void HandleBR() {
			mDoc.Pos.X = mDoc.Rect.Left;
			mDoc.Pos.Y -= mNewLineAdvance;
			mNewLineAdvance = 0;			
		}


		/// <summary>
		/// Handle BODY tag
		/// </summary>
		protected void HandleBody() {
			string bgcolor, background;
			GetAttribute("bgcolor", out bgcolor);
			GetAttribute("background", out background);

			string attr;
			if(GetAttribute("leftmargin", out attr))
				mDoc.Rect.Left += double.Parse(attr, NumberFormatInfo.InvariantInfo);
			if (GetAttribute("rightmargin", out attr))
				mDoc.Rect.Right -= double.Parse(attr, NumberFormatInfo.InvariantInfo);
			if (GetAttribute("topmargin", out attr))
				mDoc.Rect.Top -= double.Parse(attr, NumberFormatInfo.InvariantInfo);
			if (GetAttribute("bottommargin", out attr))
				mDoc.Rect.Bottom += double.Parse(attr, NumberFormatInfo.InvariantInfo);

			BackgroundElement bodyItem = new BackgroundElement();
			if (mBackgroundStack.Count <= 0) {
				bodyItem.BackgroundColor = bgcolor;
				bodyItem.BackgroundImage = background;
			} else {
				bodyItem.BackgroundColor = bgcolor??
					mBackgroundStack.Peek().BackgroundColor;
				bodyItem.BackgroundImage = background??
					mBackgroundStack.Peek().BackgroundImage;
			}
			mBackgroundStack.Push(bodyItem);
		}
		/// <summary>
		/// Handle end of BODY tag
		/// </summary>
		protected void HandleCloseBody() {
			mBackgroundStack.Pop();
		}

		#endregion

		#region Utilities
		/// <summary>
		/// Get the value of the attribute
		/// </summary>
		/// <param name="name">The name of the attribute</param>
		/// <param name="value">The vale of the attribute</param>
		/// <returns>Whether the result is null</returns>
		private bool GetAttribute(string name, out string value) {
			value = mReader.GetAttribute(name);
			return value != null;
		}

		/// <summary>
		/// Extract all common atrribute of p, h1, ... h6 tags
		/// </summary>
		/// <param name="textStyle">Textstyle descibed by the tag attributes</param>
		private void ExtractParagraphAttributes(XTextStyle textStyle) {
			string attr;
			if (GetAttribute("textformat", out attr)) {
				switch(attr) {
				case "none":
					mTextFormatStack.Push(TextFormat.None);
					break;
				case "html":
					mTextFormatStack.Push(TextFormat.Html);
					break;
				case "pre":
					mTextFormatStack.Push(TextFormat.Pre);
					break;
				default:
					mTextFormatStack.Push(mTextFormatStack.Peek());
					break;
				}
			}
			else
				mTextFormatStack.Push(mTextFormatStack.Peek());

			if (GetAttribute("align", out attr)) {
				switch (attr) {
				case "left":
					mAlignStack.Push(0);
					break;
				case "right":
					mAlignStack.Push(1);
					break;
				case "center":
					mAlignStack.Push(0.5);
					break;
				default:
					mAlignStack.Push(mAlignStack.Peek());
					break;
				}
			}
			else
				mAlignStack.Push(mAlignStack.Peek());

			if (GetAttribute("charspacing", out attr))
				textStyle.CharSpacing = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("wordspacing", out attr))
				textStyle.WordSpacing = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("justification", out attr))
				textStyle.Justification = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("bold", out attr))
				textStyle.Bold = bool.Parse(attr);

			if (GetAttribute("italic", out attr))
				textStyle.Italic = bool.Parse(attr);

			if (GetAttribute("underline", out attr))
				textStyle.Underline = bool.Parse(attr);

			if (GetAttribute("strike", out attr))
				textStyle.Strike = bool.Parse(attr);

			if (GetAttribute("strike2", out attr))
				textStyle.Strike2 = bool.Parse(attr);

			if (GetAttribute("outline", out attr))
				textStyle.Outline = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("linespacing", out attr))
				textStyle.LineSpacing = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("paraspacing", out attr)) {
				textStyle.ParaSpacing = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);
				mDoc.Pos.Y -= textStyle.ParaSpacing;
			}

			if (GetAttribute("leftmargin", out attr))
				textStyle.LeftMargin = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);

			if (GetAttribute("indent", out attr))
				textStyle.Indent = double.Parse(attr,
					NumberFormatInfo.InvariantInfo);
		}

		/// <summary>
		/// Process arbitrary tag
		/// </summary>
		protected void ProcessTag() {
			switch (mReader.Name.ToLower()) {
				case "p":
					HandleP();
					mDoc.Tag.Open(mReader.Name);
					break;
				case "h1":
				case "h2":
				case "h3":
				case "h4":
				case "h5":
				case "h6":
					mDoc.Tag.Open(mReader.Name);
					HandleH();
					break;
				case "font":
					HandleFont();
					break;
				case "img":
					HandleImage();
					break;
				case "b":
					HandleB();
					break;
				case "br":
					HandleBR();
					break;
				case "i":
					HandleI();
					break;
				case "u":
					HandleU();
					break;
				case "strike":
					HandleStrike();
					break;
				case "body":
					HandleBody();
					break;
			}
		}
		/// <summary>
		/// Process closing of arbitrary tag
		/// </summary>
		/// <param name="mReader">Xml reader to read data from</param>
		private void ProcessCloseTag(XmlReader mReader) {
			switch (mReader.Name.ToLower()) {
				case "p":
				case "h1":
				case "h2":
				case "h3":
				case "h4":
				case "h5":
				case "h6":
					HandleCloseP();
					mDoc.Tag.Close(mReader.Name);
					break;
				case "font":
					HandleCloseFont();
					break;
				case "b":
					HandleCloseB();
					break;
				case "i":
					HandleCloseI();
					break;
				case "u":
					HandleCloseU();
					break;
				case "strike":
					HandleCloseStrike();
					break;
				case "body":
					HandleCloseBody();
					break;
			}
		}
		/// <summary>
		/// Add block of text to the output content
		/// </summary>
		/// <param name="text">Text data</param>
		protected void ProcessText(string text) {
			SetBackground();

			mDoc.TextStyle.String = mTextStyleStack.Peek().String;
			mDoc.TextStyle.Size = mFontSizeStack.Peek();
			mDoc.Font = mDoc.AddFont(mFontFaceStack.Peek());
			mDoc.Color.String = mFontColorStack.Peek();
			mDoc.TextStyle.HPos = mAlignStack.Peek();
	
			if (mDoc.Pos.X == mDoc.Rect.Right)
				mDoc.Pos.Y -= mNewLineAdvance;

			int initialY = (int)mDoc.Pos.Y;

			int id;
			switch(mTextFormatStack.Peek()) {
			case TextFormat.Pre:
				id = mDoc.AddText(text);
				if(id == 0) {
					NewPage();
					id = mDoc.AddText(text);
				}
				break;
			case TextFormat.Html:
				id = mDoc.AddTextStyled(text);
				if(id == 0) {
					NewPage();
					id = mDoc.AddTextStyled(text);
				}
				break;
			default:
				text = HttpUtility.HtmlEncode(text);
				id = mDoc.AddTextStyled(text);
				if(id == 0) {
					NewPage();
					id = mDoc.AddTextStyled(text);
				}
				break;
			}

			while (mDoc.Chainable(id)) {
				NewPage();
				id = mDoc.AddTextStyled("", id);
			}

			if (mDoc.Pos.Y > initialY)
				mNewLineAdvance = mDoc.TextStyle.Size;
			else
				mNewLineAdvance = Math.Max(mDoc.TextStyle.Size, mNewLineAdvance);
		}
		/// <summary>
		/// Create a new page for the same tagged item.
		/// </summary>
		private void NewPage() {
			mDoc.Page = mDoc.AddPage();
			SetBackground();
		}

		/// <summary>
		/// Add image object to the output content
		/// </summary>
		/// <param name="fileName">Image file name</param>
		protected void ProcessImage(string fileName, string alt) {
			XImage image = new XImage();
			if (!Path.IsPathRooted(fileName))
				fileName = mBaseDirectory + fileName;

			if (!File.Exists(fileName))
				return;
			image.SetFile(fileName);

			HandleBR();

			SetBackground();
			if (mDoc.Pos.Y - image.Height < mDoc.Rect.Bottom)
				NewPage();

			string curRect = mDoc.Rect.String;
			double curY = mDoc.Pos.Y;
			mDoc.TextStyle.HPos = mAlignStack.Peek();

			if (image.Width < mDoc.Rect.Width)
				mDoc.Rect.Left = mDoc.Rect.Left + (mDoc.Rect.Width - image.Width) * mDoc.TextStyle.HPos;
			else
				mDoc.Rect.Left = 0;
			mDoc.Rect.Right = mDoc.Rect.Left + image.Width;
			mDoc.Rect.Top = curY;
			mDoc.Rect.Bottom = mDoc.Rect.Top - image.Height;

			var figure = mDoc.Tag.MakeTag("Image");
			if (!string.IsNullOrEmpty(alt)) {
				figure.Attributes = new DictAtom();
				figure.Attributes["Alt"] = new StringAtom(alt);
			}
			mDoc.Tag.Open(figure);
			int id = mDoc.AddImage(fileName);
			mDoc.Tag.Close(figure.Type);

			mDoc.Rect.String = curRect;
			mDoc.Pos.Y = curY;

			mDoc.Pos.X += image.Width;
			mNewLineAdvance = image.Height;
			HandleBR();
			image.Dispose();
		}

		/// <summary>
		/// Add background to the document
		/// </summary>
		/// <param name="item">background item</param>
		protected void SetBackground() {
			if (mBackgroundStack.Count <= 0)
				return;

			BackgroundElement item = mBackgroundStack.Peek();
			// Whether either color or image is set for the current page
			// If both are set, return
			bool partialSet = false;
			if (mBackgroundSetPage == mDoc.Page) {
				if (item == mBackground)
					return;
				if (mBackground != null) {
					if(mBackground.BackgroundColor != null
						&& mBackground.BackgroundImage != null)
						return;
					partialSet = true;
				}
			}
			mBackgroundSetPage = mDoc.Page;
			mBackground = item;
			string rect = item.Rect?? mDoc.MediaBox.String;
			List<int> contentIds = null;
			if (item.BackgroundColor != null
				&& (!partialSet || mBackground.BackgroundColor == null))
			{
				if (contentIds == null)
					contentIds = new List<int>();
				string curPos = mDoc.Pos.String;
				string curRect = mDoc.Rect.String;
				string curColor = mDoc.Color.String;
				mDoc.Rect.String = rect;
				mDoc.Color.String = item.BackgroundColor;
				contentIds.Add(mDoc.FillRect());
				mDoc.Rect.String = curRect;
				mDoc.Color.String = curColor;
				mDoc.Pos.String = curPos;
			}
			if (item.BackgroundImage != null
				&& (!partialSet || mBackground.BackgroundImage == null))
			{
				string fileName = item.BackgroundImage;
				if (!Path.IsPathRooted(fileName))
					fileName = mBaseDirectory + fileName;
				if (!File.Exists(fileName))
					item.BackgroundImage = null;
				else {
					if (contentIds == null)
						contentIds = new List<int>();
					XImage image = new XImage();
					image.SetFile(fileName);
					string curPos = mDoc.Pos.String;
					string curRect = mDoc.Rect.String;
					mDoc.Rect.String = rect;
					contentIds.Add(mDoc.AddImage(image));
					mDoc.Rect.String = curRect;
					mDoc.Pos.String = curPos;
				}
			}
		}

		/// <summary>
		/// Set default values for text styles and other formatting attributes
		/// </summary>
		protected void Init() {
			mBackgroundSetPage = 0;
			mBackground = null;
			mBackgroundStack.Clear();
			mTextFormatStack.Clear();
			mTextFormatStack.Push(TextFormat.None);
			mFontSizeStack.Clear();
			mFontSizeStack.Push(10);
			mFontFaceStack.Clear();
			mFontFaceStack.Push("Times-Roman");
			mFontColorStack.Clear();
			mFontColorStack.Push("black");
			mAlignStack.Clear();
			mAlignStack.Push(0);
			mNewLineAdvance = 0;
			mTextStyleStack.Clear();
			mTextStyleStack.Push(new XTextStyle());
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Convert xml file to the tagged pdf
		/// </summary>
		/// <param name="fileName">Xml file name</param>
		public static Doc Create(string fileName) {
			var conv = new XmlConverter();
			return conv.Load(fileName);
		}
		#endregion
	}
}
