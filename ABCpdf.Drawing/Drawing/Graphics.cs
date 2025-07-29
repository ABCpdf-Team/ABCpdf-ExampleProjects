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
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Atoms;
using WebSupergoo.ABCpdf13.Objects;

using TextState = WebSupergoo.ABCpdf13.Drawing.TextState;
using FontStyle = WebSupergoo.ABCpdf13.Drawing.FontStyle;
using FontInfoConvert = WebSupergoo.ABCpdf13.Drawing.FontInfoConvert;
using Font = WebSupergoo.ABCpdf13.Drawing.Font;
using BuiltInFont = WebSupergoo.ABCpdf13.Drawing.BuiltInFont;

namespace WebSupergoo.ABCpdf13.Drawing {
	public enum ColorSpace { GrayScale, RGB, CMYK }

	#region Drawing2D
	namespace Drawing2D {
		/// <summary>The line join style specifies the shape to be used at
		/// the corners of paths that are stroked.</summary>
		public enum LineJoin {
			/// <summary>Miter join. The outer edges of the strokes for the two
			/// segments are extended until they meet at an angle. If the segments
			/// meet at too sharp an angle, a bevel join is used instead.</summary>
			Miter,
			/// <summary>Round join. An arc of a circle with a diameter equal to
			/// the line width is drawn around the point where the two segments
			/// meet, connecting the outer edges of the strokes for the two segments.
			/// This pieslice-shaped figure is filled in, producing a rounded corner.</summary>
			Round,
			/// <summary>Bevel join. The two segments are finished with butt caps
			/// and the resulting notch beyond the ends of the segments is filled
			/// with a triangle.</summary>
			Bevel
		}

		/// <summary>The line cap style specifies the shape to be used at the
		/// ends of open subpaths (and dashes, if any) when they are stroked.</summary>
		public enum LineCap {
			/// <summary>Flat cap. The stroke is squared off at the endpoint of
			/// the path. There is no projection beyond the end of the path.</summary>
			Flat,
			/// <summary>Round cap. A semicircular arc with a diameter equal to
			/// the line width is drawn around the endpoint and filled in.</summary>
			Round,
			/// <summary>Projecting square cap. The stroke continues beyond the
			/// endpoint of the path for a distance equal to half the line width
			/// and is squared off.</summary>
			Square
		}

		/// <summary>Specifies the style of dashed lines drawn with a Pen object.</summary>
		public enum DashStyle {
			/// <summary>Specifies a user-defined custom dash style.</summary>
			Custom,
			/// <summary>Specifies a line consisting of dashes.</summary>
			Dash,
			/// <summary>Specifies a line consisting of a repeating pattern of dash-dot.</summary>
			DashDot,
			/// <summary>Specifies a line consisting of a repeating pattern of dash-dot-dot.</summary>
			DashDotDot,
			/// <summary>Specifies a line consisting of dots.</summary>
			Dot,
			/// <summary>Specifies a solid line.</summary>
			Solid
		}

		/// <summary>Specifies how the interior of a closed path is filled.</summary>
		public enum FillMode {
			/// <summary>The even-odd rule rule determines whether a point is
			/// inside a path by drawing a ray from that point in any direction
			/// and simply counting the number of path segments that cross the
			/// ray, regardless of direction.</summary>
			Alternate,
			/// <summary>The nonzero winding number rule determines whether a
			/// given point is inside a path by conceptually drawing a ray from
			/// that point to infinity in any direction and then examining the
			/// places where a segment of the path crosses the ray.</summary>
			Winding
		}
		
		public enum PathPointType {
			Start = 1, 
			Line = 2, 
			CloseSubPath = 4 
		}


		public enum MatrixOrder { Prepend, Append }

		/// <summary>Specifies a PDF transformation matrix.</summary>
		public sealed class Matrix {
			private double _a = 1;
			private double _b = 0;
			private double _c = 0;
			private double _d = 1;
			private double _e = 0;
			private double _f = 0;

			public double A {
				get { return _a; }
				set { _a = value; }
			}
			
			public double B {
				get { return _b; }
				set { _b = value; }
			}
			
			public double C {
				get { return _c; }
				set { _c = value; }
			}
			
			public double D {
				get { return _d; }
				set { _d = value; }
			}
			
			public double E {
				get { return _e; }
				set { _e = value; }
			}

			public double F {
				get { return _f; }
				set { _f = value; }
			}

			public Matrix() { }
			
			public Matrix(double a, double b, double c, double d, double e, double f) {
				_a = a;
				_b = b;
				_c = c;
				_d = d;
				_e = e;
				_f = f;
			}

			public void Multiply(double a, double b, double c, double d, double e, double f) {
				Multiply(a, b, c, d, e, f, MatrixOrder.Prepend);
			}

			public void Multiply(double a, double b, double c, double d, double e, double f, MatrixOrder order) {
				double ra, rb, rc, rd, re, rf;
				
				if (order == MatrixOrder.Prepend) {
					ra = a * _a + b * _c;
					rb = a * _b + b * _d;
					rc = c * _a + d * _c;
					rd = c * _b + d * _d;
					re = e * _a + f * _c + _e ;
					rf = e * _b + f * _d + _f ;
				}
				else {
					ra = _a * a + _b * c;
					rb = _a * b + _b * d;
					rc = _c * a + _d * c;
					rd = _c * b + _d * d;
					re = _e * a + _f * c + e ;
					rf = _e * b + _f * d + f ;
				}

				_a = ra;
				_b = rb;
				_c = rc;
				_d = rd;
				_e = re;
				_f = rf;
			}


			public void Multiply(Matrix m, MatrixOrder order) {
				Multiply(m.A, m.B, m.C, m.D, m.E, m.F, order);
			}

			public void Multiply(Matrix m) {
				Multiply(m, MatrixOrder.Prepend);
			}

			public void Rotate(double angle) {
				double cos = Math.Cos(angle / 180 * Math.PI);
				double sin = Math.Sin(angle / 180 * Math.PI);
				Multiply(cos, sin, -sin, cos, 0, 0, MatrixOrder.Prepend);
			}

			public void Scale(double width, double height) {
				Multiply(width, 0, 0, height, 0, 0, MatrixOrder.Prepend);
			}

			internal double TransformX(System.Drawing.SizeF size) {
				return size.Width * _a + size.Height * _c + _e;
			}

			internal double TransformY(System.Drawing.SizeF size) {
				return size.Width * _b + size.Height * _d + _f;
			}


			public override bool Equals(object obj) {
				Matrix m = obj as Matrix;
				if (m == null)
					return false;
				return (A == m.A && B == m.B && C == m.C && D == m.D && E == m.E && F == m.F); 
			}

			public override int GetHashCode() {
				return base.GetHashCode ();
			}

			public override string ToString() {
				return string.Format(NumberFormatInfo.InvariantInfo,
					"{0:0.#####} {1:0.#####} {2:0.#####} {3:0.#####} {4:0.#####} {5:0.#####}",
					A, B, C, D, E, F);
			}


			internal Matrix Clone() {
				return new Matrix(A, B, C, D, E, F);
			}


		}
	}
	#endregion

	#region Graphics
	/// <summary>Encapsulates a PDF drawing surface. This class cannot be inherited.</summary>
	public sealed class Graphics {

		#region Declare variables
		internal readonly PDFContent Content;
		private System.Drawing.Rectangle _clip;
		private Drawing2D.Matrix _curMatrix = new Drawing2D.Matrix();
		private ColorSpace _colorSpace;
		private float _resolution = 96;
		#endregion

		#region Properties
		/// <summary>Defining the bounds of the visible content.</summary>
		public System.Drawing.Rectangle Rect {
			get { return Content.Doc.Rect.Rectangle; }
			set { Content.Doc.Rect.Rectangle = value; }
		} 

		/// <summary>Determines the current world space transform.</summary>
		public Drawing2D.Matrix Transform {
			get { return _curMatrix; }
			set { _curMatrix = value; }
		}
		
		/// <summary>Gets or sets a Rectangle that limits the drawing region of this Graphics.</summary>
		public System.Drawing.Rectangle Clip {
			get { return _clip; }
			set { _clip = value; }
		}

		/// <summary>Gets a RectangleF structure that bounds the clipping region of this Graphics.</summary>
		/// <value>A RectangleF structure that represents a bounding rectangle for the clipping region of this Graphics.</value>
		public System.Drawing.RectangleF ClipBounds{ get{ return _clip; } }

		#endregion

		#region Constructors
		/// <summary>Initializes a new instance of the Graphics class with
		/// the specified Page parent.</summary>
		internal Graphics(Page page) {
			Content = page.Document.NewContent();
			_clip = Content.Doc.Rect.Rectangle;
			_colorSpace = page.Document.ColorSpace;
			_resolution = page.Document.Resolution;
		}
		#endregion

		/// <summary>Write content to the doc.</summary>
		public bool Realize() {
			return Content.AddToDoc();	// idempotent
		}

		#region Line methods
		/// <summary>Draws a line connecting two Point structures.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="pt1">The point at the start of the line.</param>
		/// <param name="pt2">The point at the end of the line.</param>
		public void DrawLine(Pen pen, System.Drawing.Point pt1, System.Drawing.Point pt2) {
			DrawLines(pen, new System.Drawing.PointF[] { new System.Drawing.PointF(pt1.X, pt1.Y), new System.Drawing.PointF(pt2.X, pt2.Y) });
		}

		/// <summary>Draws a line connecting two System.Drawing.PointF structures.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="pt1">The point at the start of the line.</param>
		/// <param name="pt2">The point at the end of the line.</param>
		public void DrawLine(Pen pen, System.Drawing.PointF pt1, System.Drawing.PointF pt2) {
			DrawLines(pen, new System.Drawing.PointF[] { pt1, pt2 });
		}

		/// <summary>Draws a line connecting the two points specified by coordinate pairs.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="x1">x-coordinate of the first point.</param>
		/// <param name="y1">y-coordinate of the first point.</param>
		/// <param name="x2">x-coordinate of the second point.</param>
		/// <param name="y2">y-coordinate of the second point.</param>
		public void DrawLine(Pen pen, double x1, double y1, double x2, double y2) {
			DrawLines(pen, new System.Drawing.PointF[] { new System.Drawing.PointF((float)x1, (float)y1), new System.Drawing.PointF((float)x2, (float)y2) });
		}

		/// <summary>Draws a series of line segments that connect an array of points.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="points">The points to be connected.</param>
		public void DrawLines(Pen pen, System.Drawing.Point[] points) {
			DrawLines(pen, ToPointFArray(points));
		}

		/// <summary>Draws a series of line segments that connect an array of System.Drawing.PointF structures.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="points">The points to be connected.</param>
		public void DrawLines(Pen pen, System.Drawing.PointF[] points) {
			DrawLines(pen, points, false);
		}

		/// <summary>Draws a series of line segments that connect an array of System.Drawing.PointF structures.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="points">The points to be connected.</param>
		/// <param name="closePath">Close the current subpath by appending a straight line segment from the current point to the starting point of the subpath.</param>
		private void DrawLines(Pen pen, System.Drawing.PointF[] points, bool closePath) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);

			Content.Move(points[0].X, Rect.Height - points[0].Y);
			
			for (int i = 1; i < points.Length; i++) {
				Content.Line(points[i].X, Rect.Height - points[i].Y);
			}
			
			if (closePath)
				Content.Close();
			
			Content.Stroke();
			RestoreState();
		}
		#endregion

		#region Polygon methods
		/// <summary>Draws a polygon defined by an array of points.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon.</param>
		public void DrawPolygon(Pen pen, System.Drawing.Point[] points) {
			DrawLines(pen, ToPointFArray(points), true);
		}

		/// <summary>Draws a polygon defined by an array of System.Drawing.PointF structures.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon.</param>
		public void DrawPolygon(Pen pen, System.Drawing.PointF[] points) {
			DrawLines(pen, points, true);
		}


		/// <summary>Fills the interior of a polygon defined by an array of points specified by points using the specified fill mode.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon to be filled.</param>
		/// <param name="fillMode">The style of the fill.</param>
		public void FillPolygon(Brush brush, System.Drawing.Point[] points, Drawing2D.FillMode fillMode) {
			FillPolygon(brush, ToPointFArray(points), fillMode);		
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by points using the specified fill mode.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon to fill.</param>
		public void FillPolygon(Brush brush, System.Drawing.Point[] points) {
			FillPolygon(brush, points, Drawing2D.FillMode.Alternate);		
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by System.Drawing.PointF structures using the specified fill mode.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon to be filled.</param>
		/// <param name="fillMode">The style of the fill.</param>
		public void FillPolygon(Brush brush, System.Drawing.PointF[] points, Drawing2D.FillMode fillMode) {
			SaveState();
			SetCoordinateSystems();
			SetBrushStyle(brush);
			Content.Move(points[0].X, Rect.Height - points[0].Y);
			
			for (int i = 1; i < points.Length; i++) {
				Content.Line(points[i].X, Rect.Height - points[i].Y);
			}
			
			if (fillMode == Drawing2D.FillMode.Winding) {
				Content.Fill();
			}
			else if (fillMode == Drawing2D.FillMode.Alternate) {
				Content.FillEvenOddRule();
			}

			RestoreState();
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by System.Drawing.PointF structures using the specified fill mode.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="points">Array of points that represent the vertices of the polygon to be filled.</param>
		public void FillPolygon(Brush brush, System.Drawing.PointF[] points) {
			FillPolygon(brush, points, Drawing2D.FillMode.Alternate);
		}
		#endregion

		#region Rectangle methods
		/// <summary>Draws a rectangle specified by a coordinate pair, a width, and a height.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the rectangle.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="width">Width of the rectangle to draw.</param>
		/// <param name="height">Height of the rectangle to draw.</param>
		public void DrawRectangle(Pen pen, double x, double y, double width, double height) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			Content.Rect(x, Rect.Height - y - height, width, height);
			Content.Stroke();
			RestoreState();
		}
		
		/// <summary>Draws a rectangle.</summary>
		/// <param name="pen">Pen object that determines the color, width, and style of the rectangle.</param>
		/// <param name="rect">The rectangle to be drawn.</param>
		public void DrawRectangle(Pen pen, System.Drawing.Rectangle rect) {
			DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		

		/// <summary>Draws a series of rectangles.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the rectangles.</param>
		/// <param name="rects">Array of rectangles to be drawn.</param>
		public void DrawRectangles(Pen pen, System.Drawing.Rectangle[] rects) {
			DrawRectangles(pen, ToRectangleFArray(rects));
		}
		
		/// <summary>Draws a series of rectangles.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the rectangles.</param>
		/// <param name="rects">Array of rectangles to be drawn.</param>
		public void DrawRectangles(Pen pen, System.Drawing.RectangleF[] rects) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			
			foreach (System.Drawing.RectangleF rect in rects) {
				Content.Rect(rect.X, Rect.Height - rect.Y - rect.Height, rect.Width, rect.Height);
				Content.Stroke();
			}
			
			RestoreState();
		}


		/// <summary>Fills the interior of a rectangle.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rect">The rectangle to be filled.</param>
		public void FillRectangle(Brush brush, System.Drawing.Rectangle rect) {
			FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of a rectangle.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rect">The rectangle to be filled.</param>
		public void FillRectangle(Brush brush, System.Drawing.RectangleF rect) {
			FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of a rectangle.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">Width of the rectangle.</param>
		/// <param name="height">Height of the rectangle.</param>
		public void FillRectangle(Brush brush, double x, double y, double width, double height) {
			SaveState();
			SetCoordinateSystems();
			SetBrushStyle(brush);
			Content.Rect(x, Rect.Height - y - height, width, height);
			Content.Fill();
			RestoreState();
		}


		/// <summary>Fills the interiors of a series of rectangles.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rects">The rectangles to be filled.</param>
		public void FillRectangles(Brush brush, System.Drawing.Rectangle[] rects) {
			FillRectangles(brush, ToRectangleFArray(rects));
		}

		/// <summary>Fills the interiors of a series of rectangles.</summary>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rects">The rectangles to be filled.</param>
		public void FillRectangles(Brush brush, System.Drawing.RectangleF[] rects) {
			SaveState();
			SetCoordinateSystems();
			SetBrushStyle(brush);
			
			foreach (System.Drawing.RectangleF rect in rects) {
				Content.Rect(rect.X, rect.Y, rect.Width, rect.Height);
				Content.Fill();
			}
			
			RestoreState();
		}
		#endregion

		#region Arc methods

		private void ContentArc(double x, double y, double width, double height,
			double startAngle, double sweepAngle, bool inMove) {
			Content.Arc(-startAngle, -(startAngle + sweepAngle),
				x + (width / 2), Rect.Height - y - (height / 2),
				width / 2, height / 2, 0, inMove);
		}

		/// <summary>Draws an arc representing a portion of an ellipse.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the arc.</param>
		/// <param name="rect">The rectangle that defines the boundaries of the ellipse.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to ending point of the arc.</param>
		public void DrawArc(Pen pen, System.Drawing.Rectangle rect, double startAngle, double sweepAngle) {
			DrawArc(pen, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws an arc representing a portion of an ellipse.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the arc.</param>
		/// <param name="rect">The rectangle that defines the boundaries of the ellipse.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to ending point of the arc.</param>
		public void DrawArc(Pen pen, System.Drawing.RectangleF rect, double startAngle, double sweepAngle) {
			DrawArc(pen, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws an arc representing a portion of an ellipse.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the arc.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the rectangle that defines the ellipse.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to ending point of the arc.</param>
		public void DrawArc(Pen pen, double x, double y, double width, double height,
			double startAngle, double sweepAngle) {
			DrawArc(pen, x, y, width, height, startAngle, sweepAngle, false);
		}

		private void DrawArc(Pen pen, double x, double y, double width, double height,
			double startAngle, double sweepAngle, bool closePath) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			ContentArc(x, y, width, height, startAngle, sweepAngle, true);
			
			if (closePath)
				Content.Close();
			
			Content.Stroke();
			RestoreState();
		}


		/// <summary>Draws a pie shape defined by an ellipse and two radial lines.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the pie shape.</param>
		/// <param name="rect">The rectangle that represents the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie shape.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie shape.</param>
		public void DrawPie(Pen pen, System.Drawing.Rectangle rect, double startAngle, double sweepAngle) {
			DrawPie(pen, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws a pie shape defined by an ellipse and two radial lines.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the pie shape.</param>
		/// <param name="rect">The rectangle that represents the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie shape.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie shape.</param>
		public void DrawPie(Pen pen, System.Drawing.RectangleF rect, double startAngle, double sweepAngle) {
			DrawPie(pen, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws a pie shape defined by an ellipse specified by a coordinate pair, a width, a height, and two radial lines.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the pie shape.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie shape.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie shape.</param>
		public void DrawPie(Pen pen, double x, double y, double width, double height,
			double startAngle, double sweepAngle) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			Content.Move(x + (width / 2), Rect.Height - y - (height / 2));
			ContentArc(x, y, width, height, startAngle, sweepAngle, true);
			Content.Line(x + (width / 2), Rect.Height - y - (height / 2));
			Content.Close();
			Content.Stroke();
			RestoreState();
		}


		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a rectangle and two radial lines.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="rect">The rectangle that represents the bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie section.</param>
		public void FillPie(Brush brush, System.Drawing.Rectangle rect, double startAngle, double sweepAngle) {
			FillPie(brush, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a rectangle and two radial lines.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="rect">The bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie section.</param>
		public void FillPie(Brush brush, System.Drawing.RectangleF rect, double startAngle, double sweepAngle) {
			FillPie(brush, rect.Left, rect.Top, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a pair of coordinates, a width, a height, and two radial lines.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle"/> parameter to the second side of the pie section.</param>
		public void FillPie(Brush brush, double x, double y, double width, double height,
			double startAngle, double sweepAngle) {
			SaveState();
			SetCoordinateSystems();
			SetBrushStyle(brush);
			Content.Move(x + (width / 2), Rect.Height - y - (height / 2));
			ContentArc(x, y, width, height, startAngle, sweepAngle, true);
			Content.Line(x + (width / 2), Rect.Height - y - (height / 2));
			Content.Close();

			Drawing2D.FillMode fillMode = Drawing2D.FillMode.Alternate;

			if (fillMode == Drawing2D.FillMode.Winding)
				Content.Fill();
			else if (fillMode == Drawing2D.FillMode.Alternate)
				Content.FillEvenOddRule();
			
			RestoreState();
		}


		/// <summary>Draws an ellipse defined by a bounding rectangle.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the ellipse.</param>
		/// <param name="rect">The rectangle that defines the boundaries of the ellipse.</param>
		public void DrawEllipse(Pen pen, System.Drawing.Rectangle rect){
			DrawArc(pen, rect, 0, 360);
		}

		/// <summary>Draws an ellipse defined by a bounding rectangle.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the ellipse.</param>
		/// <param name="rect">The rectangle that defines the boundaries of the ellipse.</param>
		public void DrawEllipse(Pen pen, System.Drawing.RectangleF rect){
			DrawArc(pen, rect, 0, 360);
		}

		/// <summary>Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the ellipse.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
		public void DrawEllipse(Pen pen, double x, double y, double width, double height){
			DrawArc(pen, x, y, width, height, 0, 360);
		}


		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="rect">The bounding rectangle that defines the ellipse.</param>
		public void FillEllipse(Brush brush, System.Drawing.Rectangle rect){
			FillPie(brush, rect, 0, 360);
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="rect">The bounding rectangle that defines the ellipse.</param>
		public void FillEllipse(Brush brush, System.Drawing.RectangleF rect){
			FillPie(brush, rect, 0, 360);
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
		public void FillEllipse(Brush brush, double x, double y, double width, double height){
			FillPie(brush, x, y, width, height, 0, 360);
		}

		#endregion

		#region Bezier methods
		/// <summary>Draws a Bezier spline defined by four points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="pt1">The starting point of the curve.</param>
		/// <param name="pt2">The first control point for the curve.</param>
		/// <param name="pt3">The second control point for the curve.</param>
		/// <param name="pt4">The ending point of the curve.</param>
		public void DrawBezier(Pen pen, System.Drawing.Point pt1, System.Drawing.Point pt2, System.Drawing.Point pt3, System.Drawing.Point pt4) {
			DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>Draws a Bezier spline defined by four points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="pt1">The starting point of the curve.</param>
		/// <param name="pt2">The first control point for the curve.</param>
		/// <param name="pt3">The second control point for the curve.</param>
		/// <param name="pt4">The ending point of the curve.</param>
		public void DrawBezier(Pen pen, System.Drawing.PointF pt1, System.Drawing.PointF pt2, System.Drawing.PointF pt3, System.Drawing.PointF pt4) {
			DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>Draws a Bezier spline defined by four ordered pairs of coordinates that represent points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="x1">x-coordinate of the starting point of the curve.</param>
		/// <param name="y1">y-coordinate of the starting point of the curve.</param>
		/// <param name="x2">x-coordinate of the first control point of the curve.</param>
		/// <param name="y2">y-coordinate of the first control point of the curve.</param>
		/// <param name="x3">x-coordinate of the second control point of the curve.</param>
		/// <param name="y3">y-coordinate of the second control point of the curve.</param>
		/// <param name="x4">x-coordinate of the ending point of the curve.</param>
		/// <param name="y4">y-coordinate of the ending point of the curve.</param>
		public void DrawBezier(Pen pen, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4) {
			DrawBezier(pen, x1, y1, x2, y2, x3, y3, x4, y4, false);
		}

		/// <summary>Draws a Bezier spline defined by four ordered pairs of coordinates that represent points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="x1">x-coordinate of the starting point of the curve.</param>
		/// <param name="y1">y-coordinate of the starting point of the curve.</param>
		/// <param name="x2">x-coordinate of the first control point of the curve.</param>
		/// <param name="y2">y-coordinate of the first control point of the curve.</param>
		/// <param name="x3">x-coordinate of the second control point of the curve.</param>
		/// <param name="y3">y-coordinate of the second control point of the curve.</param>
		/// <param name="x4">x-coordinate of the ending point of the curve.</param>
		/// <param name="y4">y-coordinate of the ending point of the curve.</param>
		/// <param name="closePath">Close the current subpath by appending a straight line segment from the current point to the starting point of the subpath.</param>
		private void DrawBezier(Pen pen, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, bool closePath) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			Content.Move(x1, Rect.Height - y1);
			Content.Bezier(x2, Rect.Height - y2, x3, Rect.Height - y3, x4, Rect.Height - y4);
			
			if (closePath) {
				Content.Close();
			}
			
			Content.Stroke();
			RestoreState();
		}


		/// <summary>Draws a series of Bezier splines from an array of points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="points">Array of points that represent the points that determine the curve.</param>
		public void DrawBeziers(Pen pen, System.Drawing.Point[] points) {
			DrawBeziers(pen , ToPointFArray(points));
		}

		/// <summary>Draws a series of Bezier splines from an array of points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="points">Array of System.Drawing.PointF structures that represent the points that determine the curve.</param>
		public void DrawBeziers(Pen pen, System.Drawing.PointF[] points) {
			DrawBeziers(pen, points, false);
		}

		/// <summary>Draws a series of Bezier splines from an array of points.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the curve.</param>
		/// <param name="points">Array of System.Drawing.PointF structures that represent the points that determine the curve.</param>
		/// <param name="closePath">Close the current subpath by appending a straight line segment from the current point to the starting point of the subpath.</param>
		private void DrawBeziers(Pen pen, System.Drawing.PointF[] points, bool closePath) {
			if(points.Length <= 0)
				return;

			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			Content.Move(points[0].X, Rect.Height - points[0].Y);
			
			for(int i = 0; i < points.Length - 3; i += 3){
				Content.Bezier(points[i + 1].X, Rect.Height - points[i + 1].Y,
					points[i + 2].X, Rect.Height - points[i + 2].Y,
					points[i + 3].X, Rect.Height - points[i + 3].Y);
			}

			if (closePath) {
				Content.Close();
			}
			
			Content.Stroke();
			RestoreState();
		}
		#endregion

		#region Path methods
		/// <summary>Draws a Drawing2D.GraphicsPath.</summary>
		/// <param name="pen">Pen that determines the color, width, and style of the path.</param>
		/// <param name="path">The path to draw.</param>
		public void DrawPath(Pen pen, Drawing2D.GraphicsPath path) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			Content.Contents.Append(path.Content.Contents.ToString());
			Content.Stroke();
			RestoreState();
		}

		/// <summary>Fills the interior of a Drawing2D.GraphicsPath.</summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="path">The path to fill.</param>
		public void FillPath(Brush brush, Drawing2D.GraphicsPath path) {
			SaveState();
			SetCoordinateSystems();
			SetBrushStyle(brush);
			Content.Contents.Append(path.Content.Contents.ToString());
			//Content.Close();
			if (path.FillMode == Drawing2D.FillMode.Winding) {
				Content.Fill();
			}
			else if (path.FillMode == Drawing2D.FillMode.Alternate) {
				Content.FillEvenOddRule();
			}
			RestoreState();
		}
		#endregion

		#region Text methods
		/// <summary>Draws the specified text string at the specified location with the specified Brush and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
		public void DrawString(string s, Font font, Brush brush, double x, double y) {
			DrawString(s, font, new Pen(brush.Color), brush, x, y, new TextState());
		}

		/// <summary>Draws the specified text string at the specified location with the specified Brush and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="point">System.Drawing.PointF structure that specifies the upper-left corner of the drawn text.</param>
		public void DrawString(string s, Font font, Brush brush, System.Drawing.PointF point) {
			DrawString(s, font, new Pen(brush.Color), brush, point, new TextState());
		}

		/// <summary>Draws the specified text string in the specified rectangle with the specified Brush and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="layoutRectangle">The rectangle that specifies the location of the drawn text.</param>
		public void DrawString(string s, Font font, Brush brush,
			System.Drawing.RectangleF layoutRectangle) {
			DrawString(s, font, new Pen(brush.Color), brush, layoutRectangle, new TextState());
		}

		/// <summary>Draws the specified text string at the specified location with the specified Pen, Brush, and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush, double x, double y) {
			DrawString(s, font, pen, brush, x, y, new TextState());
		}

		/// <summary>Draws the specified text string at the specified location with the specified Pen, Brush, and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="point">The upper-left corner of the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush,
			System.Drawing.PointF point) {
			DrawString(s, font, pen, brush, point, new TextState());
		}

		/// <summary>Draws the specified text string in the specified rectangle with the specified Pen, Brush, and Font objects.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="layoutRectangle">The rectangle that specifies the location of the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush,
			System.Drawing.RectangleF layoutRectangle) {
			DrawString(s, font, pen, brush, layoutRectangle, new TextState());
		}
		
		/// <summary>Draws the specified text string at the specified location with the specified Pen, Brush, and Font objects using the styling attributes of the specified TextState.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
		/// <param name="state">TextState that specifies styling attributes that are applied to the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush,
			double x, double y, TextState state) {
			DrawString(s, font, pen, brush, x, y, 1000000, 1000000, state);
		}

		/// <summary>Draws the specified text string at the specified location with the specified Pen, Brush, and Font objects using the styling attributes of the specified TextState.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="point">The upper-left corner of the drawn text.</param>
		/// <param name="state">TextState that specifies styling attributes that are applied to the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush,
			System.Drawing.PointF point, TextState state) {
			DrawString(s, font, pen, brush, point.X, point.Y, state);
		}

		/// <summary>Draws the specified text string in the specified rectangle with the specified Pen, Brush, and Font objects using the styling attributes of the specified TextState.</summary>
		/// <param name="s">String to draw</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="pen">Pen that determines the color, width, and style of the outlines of the drawn text.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="layoutRectangle">The rectangle that specifies the location of the drawn text.</param>
		/// <param name="state">TextState that specifies styling attributes that are applied to the drawn text.</param>
		public void DrawString(string s, Font font, Pen pen, Brush brush,
			System.Drawing.RectangleF layoutRectangle, TextState state) {
			DrawString(s, font, pen, brush,
				layoutRectangle.Left, layoutRectangle.Top,
				layoutRectangle.Width, layoutRectangle.Height, state);
		}

		private void DrawString(string s, Font font, Pen pen, Brush brush,
			double x, double y, double width, double height, TextState state) {
			SaveState();
			SetCoordinateSystems();
			SetPenStyle(pen);
			SetBrushStyle(brush);

			SetFontAndTextStyle(font, state);

			string oldRect = Content.Doc.Rect.String;
			string oldTransform = Content.Doc.Transform.String;

			double hscale = state.HorizontalScaling / 100;
			Content.Doc.Rect.Left = x;
			Content.Doc.Rect.Width = width / hscale;
			Content.Doc.Rect.Top = Content.Doc.MediaBox.Top - y; // - Content.mDoc.TextStyle.LineSpacing;
			Content.Doc.Rect.Bottom = Content.Doc.Rect.Top - height;
			Content.Doc.Transform.Magnify(hscale, 1,
				Content.Doc.Rect.Left, Content.Doc.Rect.Top);

			string renderingModeStr;
			switch (state.RenderingMode) {
				case TextRenderingMode.FillThenStrokeText :
				case TextRenderingMode.FillThenStrokeTextAndAddForClipping :
				case TextRenderingMode.StrokeText :
				case TextRenderingMode.StrokeTextAndAddForClipping :
					renderingModeStr = string.Format("rendering-mode=\"{0}\" outline=\"1\"",
						(int)state.RenderingMode);
					break;
				default :
					renderingModeStr = string.Format("rendering-mode=\"{0}\"",
						(int)state.RenderingMode);
					break;
			}
			string colorFillStr = string.Format(
				"color-fill=\"#{0:x2}{1:x2}{2:x2}\"",
				(int)(brush.Color.R * 255),
				(int)(brush.Color.G * 255),
				(int)(brush.Color.B * 255));
			string colorStrokeStr = string.Format(
				"color-stroke=\"#{0:x2}{1:x2}{2:x2}\"",
				(int)(pen.Color.R * 255),
				(int)(pen.Color.G * 255),
				(int)(pen.Color.B * 255));
			int id = Content.Doc.AddTextStyled(string.Format("<Font {1} {2} {3}><Pre>{0}</Pre></Font>",
				System.Web.HttpUtility.HtmlEncode(s),
				renderingModeStr, colorFillStr, colorStrokeStr));
			string stream = Content.Doc.GetInfo(id, "stream");
			Objects.Page page = (Objects.Page)Content.Doc.ObjectSoup[Content.Doc.Page];
			page.Detach(Content.Doc.ObjectSoup[id] as StreamObject);
			Content.Contents.Append(" ").Append(stream).Append(" ");

			Content.Doc.Transform.String = oldTransform;
			Content.Doc.Rect.String = oldRect;
			RestoreState();
		}

		/// <summary>Measures the specified string when drawn with the specified Font.</summary>
		/// <param name="s">String to measure</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <returns>The size, in points, of the string specified by the <paramref name="s"/> parameter as drawn with the <paramref name="font"/> parameter.</returns>
		public System.Drawing.SizeF MeasureString(string s, Font font) {
			return MeasureString(s, font, new TextState());
		}

		/// <summary>Measures the specified string when drawn with the specified Font using the styling attributes of the specified TextState.</summary>
		/// <param name="s">String to measure</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="state">The styling attributes that are applied to the drawn text.</param>
		/// <returns>The size, in points, of the string specified by the <paramref name="s"/> parameter as drawn with the <paramref name="font"/> parameter.</returns>
		public System.Drawing.SizeF MeasureString(string s, Font font, TextState state) {
			FontStyle leftoutFontStyle;
			int fontId = SetFontAndTextStyle(font, state, out leftoutFontStyle);

			string widths = Content.Doc.GetInfo(fontId, "widths " + s);
			float width = 0;
			foreach(string widthStr in widths.Split(',')) {
				if(widthStr != string.Empty) {
					try {
						width += float.Parse(widthStr, NumberFormatInfo.InvariantInfo);
					}
					catch{}
				}
			}

			double adjustItalic = 0;
			double adjustBold = 0;

			if(s.Length > 0){
				if(font.Italic)
					adjustItalic = 0.25 - 0.1;

				if(FontInfoConvert.TestFlag(leftoutFontStyle, FontStyle.Bold))
					adjustBold = 0.03;
			}
			int ascent = font.FontFamily.GetCellAscent(font.Style);
			int descent = font.FontFamily.GetCellDescent(font.Style);
			int emHeight = font.FontFamily.GetEmHeight(font.Style);
			float sizeInPixels = (font.SizeInPoints * _resolution) / 72;

			return new System.Drawing.SizeF(
				(float)(sizeInPixels
				* (width / 1000d + adjustBold + adjustItalic * ascent / emHeight)),
				(float)(sizeInPixels * (ascent + descent) / emHeight));
		}
		#endregion

		#region Image methods
		/// <summary>Draws the specified Image, using its original physical size, at the specified location.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="point">The location of the upper-left corner of the drawn image.</param>
		public void DrawImage(Image image, System.Drawing.Point point) {
			DrawImage(image, point.X, point.Y, double.NaN, double.NaN);
		}

		/// <summary>Draws the specified Image, using its original physical size, at the specified location.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="point">The location of the upper-left corner of the drawn image.</param>
		public void DrawImage(Image image, System.Drawing.PointF point) {
			DrawImage(image, point.X, point.Y, double.NaN, double.NaN);
		}

		/// <summary>Draws the specified Image at the specified location and with the specified size.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="rect">The rectangle into which the image should be drawn.</param>
		public void DrawImage(Image image, System.Drawing.Rectangle rect) {
			DrawImage(image, rect.Left, rect.Top, rect.Width, rect.Height);
		}

		/// <summary>Draws the specified Image at the specified location and with the specified size.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="rect">The rectangle into which the image should be drawn.</param>
		public void DrawImage(Image image, System.Drawing.RectangleF rect) {
			DrawImage(image, rect.Left, rect.Top, rect.Width, rect.Height);
		}

		/// <summary>Draws the specified Image, using its original physical size, at the specified location.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image.</param>
		public void DrawImage(Image image, double x, double y) {
			DrawImage(image, x, y, double.NaN, double.NaN);
		}

		/// <summary>Draws the specified Image at the specified location and with the specified size.</summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="width">Width of the drawn image.</param>
		/// <param name="height">Height of the drawn image.</param>
		public void DrawImage(Image image, double x, double y, double width, double height) {
			if (double.IsNaN(width)) {
				if (double.IsNaN(height)) {
					width = image.Width / image.xImage.HRes * 72;
					height = image.Height / image.xImage.VRes * 72;
				}
				else {
					width = image.Width * height / image.Height;
				}
			}
			else if (double.IsNaN(height)) {
				height = image.Height * width / image.Width;
			}

			Drawing2D.Matrix m = GetTransformMatrix();
			m.Multiply(width, 0, 0, height, x, Rect.Height - y - height);

			int imageID = Content.GetReferencedImageID(image);
			if (imageID != 0) {
				Content.SaveState();
				Content.Transform(m.A, m.B, m.C, m.D, m.E, m.F);
				Content.DoImage(imageID);
				Content.RestoreState();
			}
			else {
				string oldTransform = Content.Doc.Transform.String;
				System.Drawing.Rectangle oldRect = Content.Doc.Rect.Rectangle;
				Content.Doc.Transform.SetTransform(m.A, m.B, m.C, m.D, m.E, m.F);
				Content.Doc.Rect.SetRect(0, 0, 1, 1);

				imageID = Content.Doc.AddImageObject(image.xImage, false);
				Content.AddReferencedImage(image, imageID);
				string stream = Content.Doc.GetInfo(imageID, "stream");
				Objects.Page page = (Objects.Page)Content.Doc.ObjectSoup[Content.Doc.Page];
				page.Detach(Content.Doc.ObjectSoup[imageID] as StreamObject);
				Content.Contents.Append(" ").Append(stream).Append(" ");

				Content.Doc.Rect.Rectangle = oldRect;
				Content.Doc.Transform.String = oldTransform;
			}
		}

		#endregion

		#region Private methods

		private System.Drawing.PointF[] ToPointFArray(System.Drawing.Point[] points){
			System.Drawing.PointF[] arr = new System.Drawing.PointF[points.Length];

			for(int i = 0; i < points.Length; ++i)
				arr[i] = points[i];

			return arr;
		}

		private System.Drawing.RectangleF[] ToRectangleFArray(System.Drawing.Rectangle[] rects){
			System.Drawing.RectangleF[] arr = new System.Drawing.RectangleF[rects.Length];

			for(int i = 0; i < rects.Length; ++i)
				arr[i] = rects[i];

			return arr;
		}

		private int SetFontAndTextStyle(Font font, TextState state){
			FontStyle leftoutFontStyle;
			return SetFontAndTextStyle(font, state, out leftoutFontStyle);
		}

		private int SetFontAndTextStyle(Font font, TextState state, out FontStyle leftoutFontStyle) {
			XTextStyle textStyle = Content.Doc.TextStyle;
			textStyle.Size = (font.SizeInPoints * _resolution) / 72;
			textStyle.CharSpacing = state.CharacterSpacing;
			textStyle.WordSpacing = state.WordSpacing;
			textStyle.LineSpacing = state.Leading + (font.GetHeight(_resolution) - textStyle.Size);

			string embeddedFontName = font.GetEmbeddedFontName(out leftoutFontStyle);

			textStyle.Bold = FontInfoConvert.TestFlag(leftoutFontStyle, FontStyle.Bold);
			textStyle.Italic = FontInfoConvert.TestFlag(leftoutFontStyle, FontStyle.Italic);
			textStyle.Underline = FontInfoConvert.TestFlag(leftoutFontStyle, FontStyle.Underline);
			textStyle.Strike = FontInfoConvert.TestFlag(leftoutFontStyle, FontStyle.Strikeout);

			int fontId = 0;
			if (font is BuiltInFont)
				fontId = Content.Doc.AddFont(embeddedFontName);
			if (fontId == 0)
				fontId = Content.Doc.EmbedFont(embeddedFontName, LanguageType.Unicode, false, true);
			if (fontId == 0)
				fontId = Content.Doc.AddFont(embeddedFontName);
			#if DEBUG
			if (fontId == 0)
				throw new ApplicationException("Unable to find font \"" + embeddedFontName + "\".");
			#endif

			Content.Doc.Font = fontId;
			return fontId;
		}

		private Drawing2D.Matrix GetTransformMatrix() {
			Drawing2D.Matrix transform = Transform;
			double rectHeight = Rect.Height;
			return new Drawing2D.Matrix(transform.A, -transform.B, -transform.C, transform.D,
				transform.E + transform.C * rectHeight,
				rectHeight - transform.F - transform.D * rectHeight);
		}

		private void SetCoordinateSystems() {
			if (!_curMatrix.Equals(new Drawing2D.Matrix())) {
				Drawing2D.Matrix m = GetTransformMatrix();
				Content.Transform(m.A, m.B, m.C, m.D, m.E, m.F);
			}
			
			if (!Content.Doc.Rect.Rectangle.Equals(_clip)) {
				Content.SetNonStrokeAlpha(0);
				Content.Clip();
				Content.Rect(_clip.X, Rect.Height - _clip.Y - _clip.Height, _clip.Width, _clip.Height);
				Content.Fill();
			}
		}

		private void SetPenStyle(Pen pen) {
			Content.SetLineWidth(pen.Width);
			Content.SetLineJoin((int)pen.LineJoin);	
			Content.SetLineCap((int)pen.LineCap);
			Content.SetStrokeAlpha(pen.Color.A);
			
			if (_colorSpace == ColorSpace.RGB) {
				Content.SetRGBStrokeColor(pen.Color.R, pen.Color.G, pen.Color.B);
			}
			else if (_colorSpace == ColorSpace.CMYK) {
				Content.SetCMYKStrokeColor(pen.Color.C, pen.Color.M, pen.Color.Y, pen.Color.K);
			}
			else if (_colorSpace == ColorSpace.GrayScale) {
				Content.SetGrayStrokeColor(pen.Color.GrayScale);
			}

			Content.SetMiterLimit(pen.MiterLimit);

			StringBuilder sb = new StringBuilder();
			switch (pen.DashStyle) {
				case Drawing2D.DashStyle.Custom:
					sb.Append("[ ");
					foreach (double p in pen.DashPattern)
						sb.Append(p).Append(" ");
					sb.Append("] ");
					break;
				case Drawing2D.DashStyle.Dash:
					sb.Append("[ 10 5 ] ");
					break;
				case Drawing2D.DashStyle.DashDot:
					sb.Append("[ 10 5 3 5 ] ");
					break;
				case Drawing2D.DashStyle.DashDotDot:
					sb.Append("[ 10 5 3 5 3 5 ] ");
					break;
				case Drawing2D.DashStyle.Dot:
					sb.Append("[ 3 10 ] ");
					break;
				case Drawing2D.DashStyle.Solid:
				default:
					sb.Append("[ ] ");
					break;
			}

			Content.LineDash(sb.Append(pen.DashOffset).ToString());
		}

		private void SaveState() {
			if (!_curMatrix.Equals(new Drawing2D.Matrix()) || !Content.Doc.Rect.Rectangle.Equals(_clip))
				Content.SaveState();
		}

		private void RestoreState() {
			if (!_curMatrix.Equals(new Drawing2D.Matrix()) || !Content.Doc.Rect.Rectangle.Equals(_clip))
				Content.RestoreState();
		}

		private void SetBrushStyle(Brush brush) {
			Content.SetNonStrokeAlpha(brush.Color.A);
		
			if (_colorSpace == ColorSpace.RGB) {
				Content.SetRGBNonStrokeColor(brush.Color.R, brush.Color.G, brush.Color.B);
			}
			else if (_colorSpace == ColorSpace.CMYK) {
				Content.SetCMYKNonStrokeColor(brush.Color.C, brush.Color.M, brush.Color.Y, brush.Color.K);
			}
			else if (_colorSpace == ColorSpace.GrayScale) {
				Content.SetGrayNonStrokeColor(brush.Color.GrayScale);
			}
		}
		#endregion
	}
	#endregion
}
