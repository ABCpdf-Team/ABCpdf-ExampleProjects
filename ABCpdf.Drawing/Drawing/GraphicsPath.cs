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

namespace WebSupergoo.ABCpdf13.Drawing.Drawing2D {
	#region GraphicsPath
	/// <summary>Represents a series of connected lines and curves. This class cannot be inherited.</summary>
	public sealed class GraphicsPath {
		#region Declare variables
		internal readonly PDFContent Content = new PDFContent();
		private FillMode _fillMode = FillMode.Alternate;
		private System.Drawing.PointF[] _pathPoints = new System.Drawing.PointF[0];
		private System.Drawing.Rectangle _rect = new System.Drawing.Rectangle(0, 0, 0, 0);
		#endregion

		#region Properties
		/// <summary>Gets the points in the path.</summary>
		public System.Drawing.PointF[] PathPoints {
			get { return _pathPoints; }
		}

		/// <summary>Gets the number of elements in the path</summary>
		public int PointCount {
			get { return _pathPoints.Length; }
		}

		/// <summary>Gets or sets a FillMode enumeration that determines how
		/// the interiors of shapes in this path object are filled.</summary>
		public FillMode FillMode {
			get { return _fillMode; }
			set { _fillMode = value; }
		}
		#endregion

		#region Constructors
		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics) {
			_rect = graphics.Rect;
		}

		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics, FillMode fillMode) {
			_rect = graphics.Rect;
			_fillMode = fillMode;
		}
		
		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics, System.Drawing.Point[] pts, byte[] types) {
			System.Drawing.PointF[] ar = new System.Drawing.PointF[pts.Length];
			for (int i = 0; i < pts.Length; ++i)
				ar[i] = pts[i];
			_rect = graphics.Rect;
			CreateGraphicsPath(graphics, ar, types);
		}

		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics, System.Drawing.PointF[] pts, byte[] types) {
			_rect = graphics.Rect;
			CreateGraphicsPath(graphics, pts, types);
		}

		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics, System.Drawing.Point[] pts, byte[] types, FillMode fillMode) {
			System.Drawing.PointF[] ar = new System.Drawing.PointF[pts.Length];
			for (int i = 0; i < pts.Length; ++i) {
				ar[i] = pts[i];
			}
			_rect = graphics.Rect;
			_fillMode = fillMode;
			CreateGraphicsPath(graphics, ar, types);
		}

		/// <summary>GraphicsPath constructor.</summary>
		public GraphicsPath(Graphics graphics, System.Drawing.PointF[] pts, byte[] types, FillMode fillMode) {
			_rect = graphics.Rect;
			_fillMode = fillMode;
			CreateGraphicsPath(graphics, pts, types);
		}
		#endregion

		#region Line methods
		/// <summary>Move the pen to a particular location.</summary>
		/// <param name="start">The x coordinate of the location.</param>
		/// <param name="start">The y coordinate of the location.</param>
		public void MoveTo(double x, double y) {
			Content.Move(x, _rect.Height - y);
		}

		/// <summary>Us the pen to draw a line to a particular location.</summary>
		/// <param name="start">The x coordinate of the location.</param>
		/// <param name="start">The y coordinate of the location.</param>
		public void LineTo(double x, double y) {
			Content.Line(x, _rect.Height - y);
		}
		#endregion

		#region Arc methods
		/// <summary>Draws an arc.</summary>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
		/// <param name="endAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to ending point of the arc.</param>
		/// <param name="cx">Horizontal center of the arc</param>
		/// <param name="cy">Vertical center of the arc</param>
		/// <param name="rx">Horizontal radius of the arc</param>
		/// <param name="ry">Vertical radius of the arc</param>
		/// <param name="angle">Rotate angle</param>
		/// <param name="inMove">If true, move to the first point of the arc</param>
		public void AddArc(double startAngle, double endAngle, double cx, double cy, double rx, double ry, double angle) {
			Content.Arc(360 - startAngle, 360 - endAngle, cx, _rect.Height - cy, rx, ry, angle, false);
		}

		public void AddArc(double startAngle, double endAngle, double cx, double cy, double rx, double ry, double angle, bool move) {
			Content.Arc(360 - startAngle, 360 - endAngle, cx, _rect.Height - cy, rx, ry, angle, move);
		}

		public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle) {
			double endAngle = startAngle + sweepAngle;
			Content.Arc(360 - startAngle, 360 - endAngle, x + (width / 2), _rect.Height - (y - (height / 2)), width / 2, height / 2, 0, Content.Contents.Length == 0);
		}

		public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle, bool move) {
			double endAngle = startAngle + sweepAngle;
			Content.Arc(360 - startAngle, 360 - endAngle, x + (width / 2), _rect.Height - (y - (height / 2)), width / 2, height / 2, 0, move);
		}

		public void AddArc(System.Drawing.RectangleF rect, double startAngle, double sweepAngle) {
			AddArc(rect.Left, _rect.Height - rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void AddArc(System.Drawing.RectangleF rect, double startAngle, double sweepAngle, bool move) {
			AddArc(rect.Left, _rect.Height - rect.Top, rect.Width, rect.Height, startAngle, sweepAngle, move);
		}
		#endregion

		#region Polygon methods
		/// <summary>Adds a polygon to this path.</summary>
		/// <param name="points">An array of points that defines the polygon to add.</param>
		public void AddPolygon(System.Drawing.Point[] points) {
			System.Drawing.PointF[] ar = new System.Drawing.PointF[points.Length];
			for (int i = 0; i < points.Length; ++i) {
				ar[i] = points[i];
			}
			AddPolygon(ar);
		}

		/// <summary>Adds a polygon to this path.</summary>
		/// <param name="points">An array of points that defines the polygon to add.</param>
		public void AddPolygon(System.Drawing.PointF[] points) {
			Content.Move(points[0].X, points[0].Y);
			for (int i = 1; i < points.Length; i++) {
				Content.Line(points[i].X, _rect.Height - points[i].Y);
			}
		}
		#endregion

		#region Rectangle methods
		/// <summary>Adds a rectangle to this path.</summary>
		/// <param name="rect">The rectangle to be added.</param>
		public void AddRectangle(System.Drawing.Rectangle rect) {
			AddRectangle(new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
		}

		/// <summary>Adds a rectangle to this path.</summary>
		/// <param name="rect">The rectangle to be added.</param>
		public void AddRectangle(System.Drawing.RectangleF rect) {
			Content.Rect(rect.X, _rect.Height - rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Adds a series of rectangles to this path.</summary>
		/// <param name="rect">The rectangles to be added.</param>
		public void AddRectangles(System.Drawing.Rectangle[] rects) {
			foreach (System.Drawing.Rectangle rect in rects) {
				AddRectangle(rect);
			}
		}

		/// <summary>Adds a series of rectangles to this path.</summary>
		/// <param name="rect">The rectangles to be added.</param>
		public void AddRectangles(System.Drawing.RectangleF[] rects) {
			foreach (System.Drawing.RectangleF rect in rects) {
				AddRectangle(rect);
			}
		}
		#endregion

		#region Path methods

		/// <summary>Appends the specified GraphicsPath to this path.</summary>
		/// <param name="rect">The path to be added.</param>
		public void AddPath(GraphicsPath addingPath) {
			Content.Contents.Append(addingPath.Content.Contents.ToString());
		}
		#endregion

		#region Private methods
		private void CreateGraphicsPath(Graphics graphics, System.Drawing.PointF[] pts, byte[] types) {
			if (pts.Length != types.Length)
				throw new ArgumentException("CreateGraphicsPath");

			_pathPoints = pts;
			double docHeight = graphics.Rect.Height;

			for (int i = 0; i < _pathPoints.Length; i++) {
				System.Drawing.PointF pt = _pathPoints[i];
				PathPointType type = (PathPointType)types[i];

				if ((type & PathPointType.Start) == PathPointType.Start) {
					Content.Move(pt.X, docHeight - pt.Y);
				}
				
				if ((type & PathPointType.Line) == PathPointType.Line) {
					Content.Line(pt.X, docHeight - pt.Y);
				}
				
				if ((type & PathPointType.CloseSubPath) == PathPointType.CloseSubPath) {
					Content.Close();
				}
			}
		}
		#endregion
	}
	#endregion
}
