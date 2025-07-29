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
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using WebSupergoo.ABCpdf13;

namespace WebSupergoo.ABCpdf13.Drawing {
	/// <summary>Rectangle in double precision.</summary>
	[StructLayout(LayoutKind.Auto)]
	internal struct PDFRect {
		/// <summary>The entire plane.</summary>
		public static PDFRect Infinity {
			get {
				return new PDFRect(double.MinValue, double.MinValue,
					double.MaxValue, double.MaxValue);
			}
		}

		public double X, Y, Width, Height;

		public PDFRect(double x, double y, double width, double height) {
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public double Right {
			get { return double.IsInfinity(Width)? Width: X+Width; }
		}
		public double Top {
			get { return double.IsInfinity(Height)? Height: Y+Height; }
		}

		/// <summary>The string representation as used in PDF and XRect.String.</summary>
		public override string ToString() {
			const string format = PDFContent.mFloatFormat;
			return string.Format(NumberFormatInfo.InvariantInfo,
				"{0:" + format + "} {1:" + format
				+ "} {2:" + format + "} {3:" + format + "}",
				X, Y, Right, Top);
		}

		public void Union(double x, double y, double width, double height) {
			if(width>0) {
				if(!(Width>0)) {
					Width = width;
					X = x;
				} else if(x<X) {
					Width += X-x;
					X = x;
					if(width>Width)
						Width = width;
				} else {
					double right = Right;
					double rectRight = double.IsInfinity(width)? width: x+width;
					if(rectRight>right)
						Width = rectRight-X;
				}
			}
			if(height>0) {
				if(!(Height>0)) {
					Height = height;
					Y = y;
				} else if(y<Y) {
					Height += Y-y;
					Y = y;
					if(height>Height)
						Height = height;
				} else {
					double top = Top;
					double rectTop = double.IsInfinity(height)? height: y+height;
					if(rectTop>top)
						Height = rectTop-Y;
				}
			}
		}

		public void Intersect(PDFRect rect) {
			if(X<rect.X) {
				Width -= rect.X-X;
				X = rect.X;
			}
			if(rect.Width<double.PositiveInfinity
				&& (!(Width<double.PositiveInfinity) || X+Width>rect.X+rect.Width))
				Width = rect.X+rect.Width-X;

			if(Y<rect.Y) {
				Height -= rect.Y-Y;
				Y = rect.Y;
			}
			if(rect.Height<double.PositiveInfinity
				&& (!(Height<double.PositiveInfinity) || Y+Height>rect.Y+rect.Height))
				Height = rect.Y+rect.Height-Y;
		}

		internal static void GetLineBounds(double x0, double y0,
			double x1, double y1, out double outX, out double outY,
			out double outWidth, out double outHeight)
		{
			if(x0<=x1) {
				outWidth = x1-x0;
				outX = x0;
			} else {
				outWidth = x0-x1;
				outX = x1;
			}
			if(y0<=y1) {
				outHeight = y1-y0;
				outY = y0;
			} else {
				outHeight = y0-y1;
				outY = y1;
			}
		}

		internal static void GetBezierBounds(double start, double control0,
			double control1, double end, out double outX, out double outWidth)
		{
			double midControl = (control0+control1)/2;
			double quadControl0 = (start+control0)/2;
			double quadControl1 = (quadControl0+midControl)/2;
			quadControl0 = (start+quadControl0*3)/4;
			double quadControl3 = (end+control1)/2;
			double quadControl2 = (quadControl3+midControl)/2;
			quadControl3 = (end+quadControl3*3)/4;
			double mid = quadControl1+quadControl2;
			quadControl1 = (quadControl1*6+mid)/8;
			quadControl2 = (quadControl2*6+mid)/8;
			double x;
			if(start<=end)
				x = start;
			else {
				x = end;
				end = start;
			}
			if(quadControl0<x)
				x = quadControl0;
			else if(quadControl0>end)
				end = quadControl0;
			if(quadControl1<x)
				x = quadControl1;
			else if(quadControl1>end)
				end = quadControl1;
			if(quadControl2<x)
				x = quadControl2;
			else if(quadControl2>end)
				end = quadControl2;
			if(quadControl3<x)
				x = quadControl3;
			else if(quadControl3>end)
				end = quadControl3;
			outX = x;
			outWidth = end-x;
		}
	}

	/// <summary>Matrix in double precision.</summary>
	[StructLayout(LayoutKind.Auto)]
	internal struct PDFMatrix {
		public static PDFMatrix Identity { get { return new PDFMatrix(1, 0, 0, 1, 0, 0); } }

		public double M00, M01, M10, M11, M20, M21;

		public PDFMatrix(double m00, double m01, double m10, double m11,
			double m20, double m21)
		{
			M00 = m00;
			M01 = m01;
			M10 = m10;
			M11 = m11;
			M20 = m20;
			M21 = m21;
		}

		public PDFMatrix(PDFMatrix v, double m00, double m01, double m10, double m11,
			double m20, double m21)
		{
			M00 = v.M00*m00+v.M10*m01;
			M01 = v.M01*m00+v.M11*m01;
			M10 = v.M00*m10+v.M10*m11;
			M11 = v.M01*m10+v.M11*m11;
			M20 = v.M00*m20+v.M10*m21+v.M20;
			M21 = v.M01*m20+v.M11*m21+v.M21;
		}

		public PDFMatrix(ref PDFMatrix v, double m00, double m01, double m10, double m11,
			double m20, double m21)
		{
			M00 = v.M00*m00+v.M10*m01;
			M01 = v.M01*m00+v.M11*m01;
			M10 = v.M00*m10+v.M10*m11;
			M11 = v.M01*m10+v.M11*m11;
			M20 = v.M00*m20+v.M10*m21+v.M20;
			M21 = v.M01*m20+v.M11*m21+v.M21;
		}

		public PDFMatrix(PDFMatrix v0, PDFMatrix v1) : this(ref v0, ref v1) { }
		public PDFMatrix(ref PDFMatrix v0, ref PDFMatrix v1) {
			M00 = v0.M00*v1.M00+v0.M10*v1.M01;
			M01 = v0.M01*v1.M00+v0.M11*v1.M01;
			M10 = v0.M00*v1.M10+v0.M10*v1.M11;
			M11 = v0.M01*v1.M10+v0.M11*v1.M11;
			M20 = v0.M00*v1.M20+v0.M10*v1.M21+v0.M20;
			M21 = v0.M01*v1.M20+v0.M11*v1.M21+v0.M21;
		}

		public double MultiplierXSquare { get { return M00*M00+M10*M10; } }
		public double MultiplierYSquare { get { return M01*M01+M11*M11; } }

		public void TransformPoint(double x, double y,
			out double outX, out double outY)
		{
			outX = x*M00+y*M10+M20;
			outY = x*M01+y*M11+M21;
		}

		public void Translate(double offsetX, double offsetY) {
			TransformPoint(offsetX, offsetY, out M20, out M21);
		}

		public void Scale(double scaleX, double scaleY) {
			M00 *= scaleX;
			M01 *= scaleX;
			M10 *= scaleY;
			M11 *= scaleY;
		}

		public void GetRectBounds(double x, double y,
			double width, double height, out double outX, out double outY,
			out double outWidth, out double outHeight)
		{
			TransformPoint(x, y, out x, out y);
			double widthX = width*M00;
			double heightX = height*M10;
			if(widthX>=heightX) {
				if(widthX<0) {
					widthX = -widthX-heightX;
					x -= widthX;
				} else if(heightX<0) {
					widthX -= heightX;
					x += heightX;
				} else
					widthX += heightX;
			} else {
				if(heightX<0) {
					widthX = -widthX-heightX;
					x -= widthX;
				} else if(widthX<0) {
					x += widthX;
					widthX = heightX-widthX;
				} else
					widthX += heightX;
			}
			outX = x;
			outWidth = widthX;
			double heightY = height*M11;
			double widthY = width*M01;
			if(heightY>=widthY) {
				if(heightY<0) {
					heightY = -heightY-widthY;
					y -= heightY;
				} else if(widthY<0) {
					heightY -= widthY;
					y += widthY;
				} else
					heightY += widthY;
			} else {
				if(widthY<0) {
					heightY = -heightY-widthY;
					y -= heightY;
				} else if(heightY<0) {
					y += heightY;
					heightY = widthY-heightY;
				} else
					heightY += widthY;
			}
			outY = y;
			outHeight = heightY;
		}

		internal void GetBezierBounds(double startX, double startY,
			double controlX0, double controlY0,
			double controlX1, double controlY1, double endX, double endY,
			out double outX, out double outY,
			out double outWidth, out double outHeight)
		{
			// startX and startY are already transformed
			//TransformPoint(startX, startY, out startX, out startY);
			TransformPoint(controlX0, controlY0, out controlX0, out controlY0);
			TransformPoint(controlX1, controlY1, out controlX1, out controlY1);
			TransformPoint(endX, endY, out endX, out endY);
			PDFRect.GetBezierBounds(startX, controlX0, controlX1, endX,
				out outX, out outWidth);
			PDFRect.GetBezierBounds(startY, controlY0, controlY1, endY,
				out outY, out outHeight);
		}
	}

	/// <summary>Form XObject ID and bounds.</summary>
	[StructLayout(LayoutKind.Auto)]
	internal struct PDFFormXObjectID {
		public int ID;
		public PDFRect Bounds;

		public PDFFormXObjectID(int id, PDFRect bounds) {
			ID = id;
			Bounds = bounds;
		}
	}

	/// <summary>PDF content with bounds for the construction of Form XObjects.</summary>
	/// <example>This example shows how to use PDFBoundedContent with a main PDFContent.</example>
	/// <code>
	/// PDFBoundedContent theContent = new PDFBoundedContent(theDoc);
	/// ... // add some content to theContent
	/// PDFFormXObjectID theXObjID = theContent.WriteToFormXObject();
	/// theDoc.GetInfo(theXObjID.ID, "Compress");
	/// // PDFFormXObjectID may be reused with PDFContent.DoFormXObject
	/// // many times and/or on different main PDFContent's as long as
	/// // they are in the same document
	/// theMainContent.DoFormXObject(theXObjID);
	/// </code>
	internal class PDFBoundedContent: PDFContent {
		/// <summary>The graphics state for the calculation of the bounds.</summary>
		protected class PDFGraphicsState {
			public PDFRect ClipBounds;
			public PDFMatrix Matrix;
			public double LineWidth;
			public double MiterLimit;
			public LineJoin LineJoin;
			public LineCap LineCap;
			public int FontID;
			public double FontSize;
			public double CharSpacing;
			public double WordSpacing;
			public double HorizontalTextScale;
			public double TextLeading;
			public TextRenderingMode TextRender;
			public double TextRise;

			public PDFGraphicsState() {
				ClipBounds = PDFRect.Infinity;
				Matrix = PDFMatrix.Identity;
				LineWidth = 1;
				MiterLimit = 10;
				LineJoin = PDFContent.LineJoin.Miter;
				LineCap = PDFContent.LineCap.Butt;
				HorizontalTextScale = 100;
				TextRender = TextRenderingMode.FillText;
			}

			private PDFGraphicsState(PDFGraphicsState v) {
				ClipBounds = v.ClipBounds;
				Matrix = v.Matrix;
				LineWidth = v.LineWidth;
				MiterLimit = v.MiterLimit;
				LineJoin = v.LineJoin;
				LineCap = v.LineCap;
				FontID = v.FontID;
				FontSize = v.FontSize;
				CharSpacing = v.CharSpacing;
				WordSpacing = v.WordSpacing;
				HorizontalTextScale = v.HorizontalTextScale;
				TextLeading = v.TextLeading;
				TextRender = v.TextRender;
				TextRise = v.TextRise;
			}

			public PDFGraphicsState Clone() { return CloneState(); }
			protected virtual PDFGraphicsState CloneState() {
				return new PDFGraphicsState(this);
			}
		}

		/// <summary>The Form XObject ID.</summary>
		private int _xobjID;
		/// <summary>The stack of graphics states.</summary>
		private Stack<PDFGraphicsState> _stateStack;
		/// <summary>The current graphics state.</summary>
		private PDFGraphicsState _state;
		/// <summary>The current drawing position.</summary>
		private double _x, _y;
		/// <summary>The bounds of the content.</summary>
		private PDFRect _bounds;
		/// <summary>The bounds of path being constructed.</summary>
		private PDFRect _pathBounds;
		/// <summary>The text line matrix.</summary>
		private PDFMatrix _textMatrix;
		/// <summary>The ID of the font whose information is loaded/cached.</summary>
		private int _fontInfoID;
		/// <summary>The font glyph bounding box.</summary>
		private XRect _fontBBox;
		/// <summary>Whether the font is multibyte.</summary>
		private bool _fontIsMultibyte;

		public PDFBoundedContent(Doc doc, DocumentCache docCache) : this(doc, docCache,
			doc.AddObject("<</Type /XObject /Subtype /Form"
			+" /Resources <</ProcSet[/PDF /Text /ImageB /ImageC /ImageI]>>>>stream\nendstream\n"))
		{ }

		public PDFBoundedContent(Doc doc, DocumentCache docCache, int xobjID) : base(doc, docCache) {
			_xobjID = xobjID;
			_stateStack = new Stack<PDFGraphicsState>();
			_state = new PDFGraphicsState();
			_textMatrix = PDFMatrix.Identity;
			_fontBBox = new XRect();
		}

		protected override int ResourcesObjectID { get { return _xobjID; } }

		private void UnionBounds(PDFRect rect) {
			rect.Intersect(_state.ClipBounds);
			_bounds.Union(rect.X, rect.Y, rect.Width, rect.Height);
		}
		private void UnionPathBounds(double lineWidth) {
			double x = _pathBounds.X;
			double y = _pathBounds.Y;
			if(lineWidth>0) {
				lineWidth /= 2;
				double halfWidthX = lineWidth*Math.Sqrt(_state.Matrix.MultiplierXSquare);
				double halfWidthY = lineWidth*Math.Sqrt(_state.Matrix.MultiplierYSquare);
				_pathBounds.X -= halfWidthX;
				_pathBounds.Y -= halfWidthY;
				_pathBounds.Width += 2*halfWidthX;
				_pathBounds.Height += 2*halfWidthY;
			}
			UnionBounds(_pathBounds);
			_pathBounds.X = x;
			_pathBounds.Y = y;
			_pathBounds.Width = 0;
			_pathBounds.Height = 0;
		}

		public override void Move(double x, double y) {
			base.Move(x, y);
			_state.Matrix.TransformPoint(x, y, out _x, out _y);
		}

		public override void Line(double x, double y) {
			base.Line(x, y);
			double x0 = _x, y0 = _y;
			_state.Matrix.TransformPoint(x, y, out _x, out _y);
			PDFRect.GetLineBounds(x0, y0, _x, _y, out x0, out y0, out x, out y);
			_pathBounds.Union(x0, y0, x, y);
		}

		public override void Rect(double x, double y, double w, double h) {
			base.Rect(x, y, w, h);
			_state.Matrix.TransformPoint(x, y, out _x, out _y);
			_state.Matrix.GetRectBounds(x, y, w, h, out x, out y, out w, out h);
			_pathBounds.Union(x, y, w, h);
		}

		public override void Bezier(double x1, double y1, double x2, double y2,
			double x3, double y3)
		{
			base.Bezier(x1, y1, x2, y2, x3, y3);
			double x0 = _x, y0 = _y;
			_state.Matrix.TransformPoint(x3, y3, out _x, out _y);
			_state.Matrix.GetBezierBounds(x0, y0, x1, y1, x2, y2, x3, y3,
				out x0, out y0, out x1, out y1);
			_pathBounds.Union(x0, y0, x1, y1);
		}

		public override void Stroke() {
			base.Stroke();
			UnionPathBounds(_state.LineWidth);
		}

		public override void Fill() {
			base.Fill();
			UnionPathBounds(0);
		}

		public override void FillEvenOddRule() {
			base.FillEvenOddRule();
			UnionPathBounds(0);
		}

		public override void Clip() {
			base.Clip();
			_state.ClipBounds.Intersect(_pathBounds);
			_pathBounds.Width = 0;
			_pathBounds.Height = 0;
		}

		public override void ClipEvenOddRule() {
			base.ClipEvenOddRule();
			_state.ClipBounds.Intersect(_pathBounds);
			_pathBounds.Width = 0;
			_pathBounds.Height = 0;
		}

		public override void SaveState() {
			base.SaveState();
			_stateStack.Push(_state.Clone());
		}

		public override void RestoreState() {
			base.RestoreState();
			_state = _stateStack.Pop();
		}

		public override void SetLineWidth(double v) {
			base.SetLineWidth(v);
			_state.LineWidth = Math.Abs(v);
		}

		public override void SetLineCap(int v) {
			base.SetLineCap(v);
			_state.LineCap = (LineCap)v;
		}

		public override void SetLineJoin(int v) {
			base.SetLineJoin(v);
			_state.LineJoin = (LineJoin)v;
		}

		public override void SetMiterLimit(double v) {
			base.SetMiterLimit(v);
			_state.MiterLimit = Math.Abs(v);
		}

		public override void Transform(double a, double b, double c, double d,
			double h, double v)
		{
			base.Transform(a, b, c, d, h, v);
			_state.Matrix = new PDFMatrix(ref _state.Matrix, a, b, c, d, h, v);
		}

		public override void AddContent(PDFContent theContent) {
			throw new NotSupportedException("PDFBoundedContent does not support AddContent.");
		}

		public override bool AddToDoc() {
			throw new InvalidOperationException("PDFBoundedContent does not allow AddToDoc.");
		}

		/// <summary>Write the contents to the Form XObject.</summary>
		public PDFFormXObjectID WriteToFormXObject() {
			WriteAllGStates();

			Doc.SetInfo(_xobjID, "stream", Contents.ToString());
			Doc.SetInfo(_xobjID, "/BBox:Rect", _bounds.ToString());
			ShowErrors();
			return new PDFFormXObjectID(_xobjID, _bounds);
		}

		public override void DoImage(int imageID) {
			base.DoImage(imageID);
			PDFRect rect;
			_state.Matrix.GetRectBounds(0, 0, 1, 1,
				out rect.X, out rect.Y, out rect.Width, out rect.Height);
			UnionBounds(rect);
		}

		public override void DoFormXObject(PDFFormXObjectID xobjID) {
			base.DoFormXObject(xobjID);
			PDFRect rect;
			_state.Matrix.GetRectBounds(xobjID.Bounds.X, xobjID.Bounds.Y,
				xobjID.Bounds.Width, xobjID.Bounds.Height,
				out rect.X, out rect.Y, out rect.Width, out rect.Height);
			UnionBounds(rect);
		}

		public override void BeginText() {
			base.BeginText();
			_textMatrix = PDFMatrix.Identity;
		}

		public override void SetTextRenderingMode(TextRenderingMode mode) {
			base.SetTextRenderingMode(mode);
			_state.TextRender = mode;
		}

		public override void SetCharacterSpacing(double tc) {
			base.SetCharacterSpacing(tc);
			_state.CharSpacing = tc;
		}

		public override void SetWordSpacing(double tw) {
			base.SetWordSpacing(tw);
			_state.WordSpacing = tw;
		}

		public override void SetHorizontalScaling(double th) {
			base.SetHorizontalScaling(th);
			_state.HorizontalTextScale = th;
		}

		public override void SetTextLeading(double tl) {
			base.SetTextLeading(tl);
			_state.TextLeading = tl;
		}

		public override void SetTextRise(double ts) {
			base.SetTextRise(ts);
			_state.TextRise = ts;
		}

		public override void ShowTextString(string text) {
			base.ShowTextString(text);
			if(text.Length<=0 || _state.FontID==0 || !(_state.FontSize>0))
				return;

			string s = Doc.GetInfo(_state.FontID, "Widths "+text);
			if(s.Length<=0)
				return;

			if(_fontInfoID!=_state.FontID) {
				_fontIsMultibyte = Doc.GetInfo(_state.FontID, "/Subtype:Name")=="Type0";
				_fontBBox.String = Doc.GetInfo(_state.FontID, "FontBBox");
				_fontInfoID = _state.FontID;
			}

			long width = 0;
			int i = 0, count = 0;
			for(int j; (j = s.IndexOf(',', i))>=i; i = j+1, ++count) {
				width += int.Parse(s.Substring(i, j-i), NumberFormatInfo.InvariantInfo);
			}
			i = int.Parse(s.Substring(i), NumberFormatInfo.InvariantInfo);
			double right = width + Math.Max(_fontBBox.Right, i);
			width += i;
			++count;

			double scale = _state.FontSize/1000;
			right *= scale;
			double advance = width*scale;
			int wordSpaceCount = 0;
			if(!_fontIsMultibyte && _state.WordSpacing!=0) {
				for(i = 0; i<count; ++i) {
					if(text[i]==' ')
						++wordSpaceCount;
				}
			}
			if(wordSpaceCount>0) {
				double spacing = _state.WordSpacing*wordSpaceCount;
				right += text[count-1]!=' '? spacing: spacing-_state.WordSpacing;
				advance += spacing;
			}
			if(_state.CharSpacing!=0) {
				double spacing = _state.CharSpacing*count;
				if(count>1)
					right += spacing-_state.CharSpacing;
				advance += spacing;
			}

			PDFMatrix matrix = new PDFMatrix(ref _state.Matrix, ref _textMatrix);
			if(_state.TextRise!=0)
				matrix.Translate(0, _state.TextRise);
			if(_state.HorizontalTextScale!=100) {
				double hscale = _state.HorizontalTextScale/100;
				matrix.Scale(hscale, 1);
				advance *= hscale;
			}

			_textMatrix.Translate(advance, 0);

			double left = _fontBBox.Left*scale;
			PDFRect rect;
			matrix.GetRectBounds(left, _fontBBox.Bottom*scale,
				right-left, _fontBBox.Height*scale,
				out rect.X, out rect.Y, out rect.Width, out rect.Height);
			if(_state.LineWidth>0) {
				switch(_state.TextRender) {
				case TextRenderingMode.StrokeText:
				case TextRenderingMode.FillThenStrokeText:
				case TextRenderingMode.StrokeTextAndAddForClipping:
				case TextRenderingMode.FillThenStrokeTextAndAddForClipping:
					double halfWidth = _state.LineWidth/2;
					double halfWidthX = halfWidth*Math.Sqrt(_state.Matrix.MultiplierXSquare);
					double halfWidthY = halfWidth*Math.Sqrt(_state.Matrix.MultiplierYSquare);
					rect.X -= halfWidthX;
					rect.Y -= halfWidthY;
					rect.Width += 2*halfWidthX;
					rect.Height += 2*halfWidthY;
					break;
				}
			}
			UnionBounds(rect);
		}

		public override void SetFont(int fontObjID, double fontSize) {
			base.SetFont(fontObjID, fontSize);
			_state.FontID = fontObjID;
			_state.FontSize = Math.Abs(fontSize);
		}

		public override void SetTextMatrix(double a, double b, double c, double d, double e, double f) {
			base.SetTextMatrix(a, b, c, d, e, f);
			_textMatrix.M00 = a;
			_textMatrix.M01 = b;
			_textMatrix.M10 = c;
			_textMatrix.M11 = d;
			_textMatrix.M20 = e;
			_textMatrix.M21 = f;
		}

		public override void NextLine() {
			base.NextLine();
			if(_state.TextLeading!=0)
				_textMatrix.Translate(0, -_state.TextLeading);
		}

		public override void TextMove(double tx, double ty) {
			base.TextMove(tx, ty);
			_textMatrix.Translate(tx, ty);
		}
	}
}
