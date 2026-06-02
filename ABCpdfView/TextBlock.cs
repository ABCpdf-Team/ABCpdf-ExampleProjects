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
using System.Drawing.Drawing2D;
using System.Xml;

namespace ABCpdfControls
{
	/// <summary>
	/// Text block
	/// </summary>
	internal class TextBlock
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="inText">Text string</param>
		/// <param name="points">Vertices of the bounding box</param>
		/// <param name="inNode">Xml node from the text content description</param>
		public TextBlock(string inText, GraphicsPath path, XmlNode inNode)
		{
			Text = inText;
			Path = path;
			Node = inNode;
		}

		/// <summary>
		/// Determines if point is inside the bounding box of a text block
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool ContainsPoint(PointF p)
		{
			if (Path.PointCount < 4)
				return false;

			return (ClassifyPoint(Path.PathPoints[0], Path.PathPoints[1], p) <= 0) &&
				(ClassifyPoint(Path.PathPoints[1], Path.PathPoints[2], p) <= 0) &&
				(ClassifyPoint(Path.PathPoints[2], Path.PathPoints[3], p) <= 0) &&
				(ClassifyPoint(Path.PathPoints[3], Path.PathPoints[0], p) <= 0);
		}

		/// <summary>
		/// Check the position of the point relative to the line p0 - p1
		/// </summary>
		/// <param name="p0">Point of the line</param>
		/// <param name="p1">Point of the line</param>
		/// <param name="p">Point</param>
		/// <returns> 1 if point is on the right side from the line
		/// 0 if point is on the line itself
		/// -1 if point is on the left side from the line</returns>
		private int ClassifyPoint(PointF p0, PointF p1, PointF p)
		{
			PointF a = new PointF(p1.X - p0.X, p1.Y - p0.Y);
			PointF b = new PointF(p.X - p0.X, p.Y - p0.Y);
			double sa = a. X * b.Y - b.X * a.Y;
			if (sa > 0.0)
				return -1;
			if (sa < 0.0)
				return 1;

			return 0;
		}

		public GraphicsPath GetGraphics(Matrix viewTransform)
		{
			GraphicsPath thePath = (GraphicsPath)Path.Clone();
			thePath.Transform(viewTransform);
			return thePath;
		}

		public GraphicsPath Path;
		public XmlNode Node;
		public int StreamId = 0;
		public int StreamOffset = 0;
		public int Length = 0;
		public bool Selected;
		public Matrix TextRenderingMatrix;

		public string Text;
		public Color TextColor;
		public string TextColorOperator;
		public float FontSize;
		public string FontFamily;
		public string FontOperator;
		public string PDFOperator;

		public float TextRise = 0;
		public float WordSpacing = 0;
		public float CharacterSpacing = 0;
		public float TextLeading = 0;
		public float HorizontalScaling = 100;
	}
}
