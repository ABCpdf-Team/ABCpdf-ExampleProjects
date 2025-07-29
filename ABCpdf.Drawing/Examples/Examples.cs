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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Drawing;
using WebSupergoo.ABCpdf13.Drawing.Text;
using WebSupergoo.ABCpdf13.Drawing.Drawing2D;


namespace Drawing.Examples {
	class Drawing {
		[STAThread]
		static void Main(string[] args) {
			// examples to demonstrate functionality
			using (PDFDocument doc = new PDFDocument()) {
				BaseSample bs = new BaseSample();
				bs.Run(doc);
				doc.Save("../../examples.pdf", true);
				//OpenDoc("../../examples.pdf");
			}
		}

		private static void OpenDoc(string file) {
			if (File.Exists(file)) {
				Process process = new Process();
				process.StartInfo.FileName = file;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process.Start();
			}
		}
	}

	internal class BaseSample {
		private System.Drawing.PointF[] _starPoints;
		private System.Drawing.PointF[] StarPoints {
			get {
				if (_starPoints == null) {
					const double angle = Math.PI * 2 / 5;
					const double radius = 250;
					const double centerX = 300, centerY = 420;
					_starPoints = new System.Drawing.PointF[5];
					int j = 0;
					for (int i = 0; i < _starPoints.Length; ++i) {
						_starPoints[i] = new System.Drawing.PointF(
							(float)(centerX + radius * Math.Sin(j * angle)),
							(float)(centerY - radius * Math.Cos(j * angle)));
						j = (j + 2) % 5;
					}
				}
				return _starPoints;
			}
		}

		public void Run(PDFDocument doc) {
			StrokeExample(doc);
			FillExample(doc);
			AlternateFillModeExample(doc);
			HexagonExample(doc);
			RotateExample(doc);
			BezierCurveExample(doc);
			LineJoinExample(doc);
			LineCapExample(doc);
			ClipExample(doc);
			DashExample(doc);
			TransparencyExamples(doc);
			TextExample(doc);
			TextExample2(doc);
			ImageExample(doc);
			ImageExample2(doc);
		}

		private void TextExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Text examples (built-in fonts)");
			Pen pen = new Pen(Color.Red, 1);
			Brush brush = new SolidBrush(Color.Black);

			System.Drawing.SizeF size = pg.Graphics.MeasureString("Helvetica", new BuiltInFont("Helvetica", 30));
			pg.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(System.Drawing.KnownColor.LightGray)),
				20, 100, size.Width, size.Height);
			pg.Graphics.DrawString("Helvetica", new BuiltInFont("Helvetica", 30),
				pen, brush, 20, 100);

			size = pg.Graphics.MeasureString("Helvetica-BoldOblique",
				new BuiltInFont("Helvetica", 30, FontStyle.Italic | FontStyle.Bold));
			pg.Graphics.FillRectangle(new SolidBrush(Color.LightGray), 20, 150, size.Width, size.Height);
			pg.Graphics.DrawString("Helvetica-BoldOblique", new BuiltInFont("Helvetica", 30,
				FontStyle.Italic | FontStyle.Bold),
				pen, brush, 20, 150, new TextState(TextRenderingMode.StrokeText));

			pg.Graphics.DrawString("Courier", new BuiltInFont("Courier", 30),
				pen, brush, 20, 200, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Times-Roman", new BuiltInFont("Times Roman", 30),
				pen, brush, 20, 250, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Times-BoldItalic",
				new BuiltInFont("Times Roman", 30, FontStyle.Italic | FontStyle.Bold),
				pen, brush, 20, 300, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Symbol", new BuiltInFont("Symbol", 30),
				pen, brush, 20, 350);

			pg.Graphics.DrawString("ZapfDingbats", new BuiltInFont("Zapf Dingbats", 30),
				pen, brush, 20, 400, new TextState(TextRenderingMode.FillText));

			Font transformFont = new BuiltInFont("Helvetica", 21, FontStyle.Italic | FontStyle.Bold);
			pg.Graphics.DrawString("Horizontal scaling = 50", transformFont,
				pen, brush, 20, 450, new TextState(0, 0, 50, 0, TextRenderingMode.FillText));

			pg.Graphics.DrawString("Word space = 15", transformFont,
				pen, brush, 20, 500, new TextState(0, 15, 100, 0, TextRenderingMode.FillText));

			pg.Graphics.DrawString("Character spacing = 5", transformFont,
				pen, brush, 20, 550, new TextState(5, 0, 100, 0, TextRenderingMode.FillText));

			pg.Graphics.DrawString("Underline",
				new BuiltInFont("Helvetica", 30, FontStyle.Italic | FontStyle.Underline),
				pen, brush, 20, 600);
			pg.Graphics.DrawString("Strikeout",
				new BuiltInFont("Helvetica", 30, FontStyle.Italic | FontStyle.Strikeout),
				pen, brush, 250, 600);
			pg.Graphics.DrawString("Underline Strikeout",
				new BuiltInFont("Helvetica", 30, FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout),
				pen, brush, 20, 650);
		}

		private void TextExample2(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Text examples (embedded fonts)");
			Pen pen = new Pen(Color.Red, 1);
			Brush brush = new SolidBrush(Color.Black);

			System.Drawing.SizeF size = pg.Graphics.MeasureString("Arial", new Font("Arial", 30));
			pg.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(System.Drawing.KnownColor.LightGray)),
				20, 100, size.Width, size.Height);
			pg.Graphics.DrawString("Arial", new Font("Arial", 30),
				pen, brush, 20, 100);

			size = pg.Graphics.MeasureString("Arial Bold Italic",
				new Font("Arial", 30, FontStyle.Italic | FontStyle.Bold));
			pg.Graphics.FillRectangle(new SolidBrush(Color.LightGray), 20, 150, size.Width, size.Height);
			pg.Graphics.DrawString("Arial Bold Italic", new Font("Arial", 30, FontStyle.Italic | FontStyle.Bold),
				pen, brush, 20, 150, new TextState(TextRenderingMode.StrokeText));

			pg.Graphics.DrawString("Impact", new Font("Impact", 30),
				pen, brush, 20, 200, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Comic Sans MS", new Font("Comic Sans MS", 30),
				pen, brush, 20, 250, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Comic Sans MS Bold Italic",
				new Font("Comic Sans MS", 30, FontStyle.Italic | FontStyle.Bold),
				pen, brush, 20, 300, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("Symbol", new Font("Symbol", 30),
				pen, brush, 20, 350, new TextState(TextRenderingMode.FillText));

			pg.Graphics.DrawString("Wingdings", new Font("Wingdings", 30),
				pen, brush, 20, 400, new TextState(TextRenderingMode.FillText));

			Pen pen2 = new Pen(Color.Orange, 0.5);
			pg.Graphics.DrawString("PMingLiU \u65b0\u7d30\u660e\u9ad4",
				new Font("PMingLiU", 30),
				pen2, brush, 20, 450, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("MS PGothic \u30b4\u30b7\u30c3\u30b0 \u306f\u3058\u3081\u307e\u3057\u3066",
				new Font("MS PGothic", 30),
				pen2, brush, 20, 500, new TextState(TextRenderingMode.FillThenStrokeText));

			pg.Graphics.DrawString("MS PMincho \u660e\u671d \u306f\u3058\u3081\u307e\u3057\u3066",
				new Font("MS PMincho", 30),
				pen2, brush, 20, 550, new TextState(TextRenderingMode.FillThenStrokeText));
		}

		private void BezierCurveExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Bezier curve example");

			Color[] colors = {
				Color.Red, Color.Black, Color.Blue, Color.Red, Color.Black
			};

			float[,] xy = {
				{ 508, 486, 67, 504, 0, 782, 509, 508 },
				{ 283, 683, 231, 260, 493, 281, 434, 208 },
				{ 197, 261, 133, 14, 533, 598, 466, 704 },
				{ 50, 100, 50, 316, 376, 36, 311, 166 },
				{ 340, 486, 402, 409, 413, 483, 559, 225 }
			};
			
			Random rnd = new Random();
			for (int i = 0; i < 5; ++i) {
				Pen pen = new Pen(colors[i], 20);
				//pen.LineCap = LineCap.Round;

				// Draw the Bezier spline
				pg.Graphics.DrawBezier(pen,
					xy[i, 0], xy[i, 1], xy[i, 2], xy[i, 3],
					xy[i, 4], xy[i, 5], xy[i, 6], xy[i, 7]);
			}
		}

		private void AlternateFillModeExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Alternate fill mode example");
			
			double size = 150;
			double centerX = (double)pg.Graphics.Rect.Width / 2;
			double centerY = (double)pg.Graphics.Rect.Height / 3;

			GraphicsPath path = new GraphicsPath(pg.Graphics, FillMode.Alternate);

			for (int i = 0; i < 4; i++) {
				double radius = size / 2;
				path.AddArc(0, 360, centerX - radius, centerY, radius, radius, 0, true);
				path.AddArc(0, 360, centerX + radius, centerY, radius, radius, 0, true);
				path.AddArc(0, 360, centerX, centerY + radius, radius, radius, 0, true);
				path.AddArc(0, 360, centerX, centerY - radius, radius, radius, 0, true);
				size /= 2;
			}

			pg.Graphics.FillPath(new SolidBrush(Color.Black), path);
		}

		private void HexagonExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Coordinate transformation example (part I)");

			const double side = 125, halfSide = side / 2;
			double offset = side * Math.Sqrt(3) / 2;
			const double centerX = 0, centerY = 0;

			System.Drawing.PointF[] points = {
				new System.Drawing.PointF((float)(centerX - halfSide), (float)(centerY - offset)),
				new System.Drawing.PointF((float)(centerX + halfSide), (float)(centerY - offset)),
				new System.Drawing.PointF((float)(centerX + side), (float)centerY),
				new System.Drawing.PointF((float)(centerX + halfSide), (float)(centerY + offset)),
				new System.Drawing.PointF((float)(centerX - halfSide), (float)(centerY + offset)),
				new System.Drawing.PointF((float)(centerX - side), (float)centerY)
			};

			byte[] types = {
				(byte)PathPointType.Start,
				(byte)PathPointType.Line,
				(byte)PathPointType.Line,
				(byte)PathPointType.Line,
				(byte)PathPointType.Line,
				(byte)(PathPointType.Line | PathPointType.CloseSubPath)
			};

			GraphicsPath path = new GraphicsPath(pg.Graphics, points, types);

			double scale = 2.5 * Math.Sqrt(3) / 2;
			const double angle = Math.PI * 15 / 180;

			for (int i = 0; i < 8; i++) {	
				// Rotate and scale hexahedron
				double cos = scale * Math.Cos(angle * i);
				double sin = scale * Math.Sin(angle * i);
				pg.Graphics.Transform = new Matrix((float)cos, (float)sin,
					(float)-sin, (float)cos, 300F, 400F);
				pg.Graphics.FillPath(new SolidBrush(Color.FromArgb(30 * i, 30 * i, 30 * i)), path);
				scale *= Math.Sqrt(3) / 2;
			}
		}

		private void RotateExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Coordinate transformation example (part II)");

			byte[] types = {
				(byte)PathPointType.Start,
				(byte)PathPointType.Line,
				(byte)PathPointType.Line,
				(byte)PathPointType.Line,
				(byte)(PathPointType.Line | PathPointType.CloseSubPath)
			};

			GraphicsPath path = new GraphicsPath(pg.Graphics, StarPoints, types);

			double entry = Math.Cos(Math.PI / 4);
			pg.Graphics.DrawPath(new Pen(Color.Black, 30), path);
			pg.Graphics.Transform = new Matrix(entry, entry, -entry, entry, 0, 0);
			pg.Graphics.DrawPath(new Pen(Color.Red, 30), path);
		}


		private void TransparencyExamples(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Transparency example");
			pg.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)),
				new System.Drawing.RectangleF(130, 200, 350, 350));
			pg.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, 0, 255, 0)),
				205, 200, 200, 200);
			pg.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, 255, 0, 0)),
				130, 350, 200, 200);
			pg.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, 0, 0, 255)),
				280, 350, 200, 200);
		}

		private void StrokeExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Stroke path example");

			Pen pen = new Pen(Color.Black, 30);
			pen.DashStyle = DashStyle.Solid;

			pg.Graphics.DrawPolygon(pen, StarPoints);
		}

		private void FillExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Fill path example");

			pg.Graphics.FillPolygon(new SolidBrush(Color.Red), StarPoints, FillMode.Winding);

			pg.Graphics.FillRectangle(new SolidBrush(Color.Black), 
				new System.Drawing.RectangleF(300, pg.Graphics.Rect.Height - 250, 300, 200));
		}

		private void LineJoinExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Line join example");

			Pen pen = new Pen(Color.Black, 50);

			System.Drawing.PointF[] pointsBevel = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 500),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 700),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 500)
			};

			pen.LineJoin = LineJoin.Bevel;
			pg.Graphics.DrawLines(pen, pointsBevel);
			pg.Graphics.DrawString("Bevel join - ", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 150, pg.Graphics.Rect.Height - 600, new TextState(TextRenderingMode.FillText));


			System.Drawing.PointF[] pointsMiter = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 300),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 500),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 300)
			};

			pen.LineJoin = LineJoin.Miter;
			pg.Graphics.DrawLines(pen, pointsMiter);
			pg.Graphics.DrawString("Miter join - ", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 150, pg.Graphics.Rect.Height - 400, new TextState(TextRenderingMode.FillText));

			System.Drawing.PointF[] pointsRound = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 100),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 300),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 100)
			};

			pen.LineJoin = LineJoin.Round;
			pg.Graphics.DrawLines(pen, pointsRound);
			pg.Graphics.DrawString("Round join - ", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 150, pg.Graphics.Rect.Height - 200, new TextState(TextRenderingMode.FillText));

			pen = new Pen(Color.White, 10);
			pen.LineJoin = LineJoin.Bevel;

			System.Drawing.PointF[] pointsBevelInline = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 500),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 700),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 700),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 500)
			};
			pg.Graphics.DrawLines(pen, pointsBevelInline);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 390, pg.Graphics.Rect.Height - 710, 20, 20);

			System.Drawing.PointF[] pointsMiterInline = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 300),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 500),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 500),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 300)
			};
			pg.Graphics.DrawLines(pen, pointsMiterInline);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 390, pg.Graphics.Rect.Height - 510, 20, 20);

			System.Drawing.PointF[] pointsRoundInline = {
				new System.Drawing.PointF(300, pg.Graphics.Rect.Height - 100),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 300),
				new System.Drawing.PointF(400, pg.Graphics.Rect.Height - 300),
				new System.Drawing.PointF(500, pg.Graphics.Rect.Height - 100)
			};
			pg.Graphics.DrawLines(pen, pointsRoundInline);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 390, pg.Graphics.Rect.Height - 310, 20, 20);
		}

		private void LineCapExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Line cap example");

			Pen pen = new Pen(Color.Black, 100);

			pen.LineCap = LineCap.Flat;
			pg.Graphics.DrawLine(pen, 100, 200, 500, 200);
			pg.Graphics.DrawString("Flat line cap", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 220, 270, new TextState(TextRenderingMode.FillText));

			pen.LineCap = LineCap.Round;
			pg.Graphics.DrawLine(pen, 100, 400, 500, 400);
			pg.Graphics.DrawString("Round line cap", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 220, 470, new TextState(TextRenderingMode.FillText));

			pen.LineCap = LineCap.Square;
			pg.Graphics.DrawLine(pen, 100, 600, 500, 600);
			pg.Graphics.DrawString("Square line cap", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 220, 670, new TextState(TextRenderingMode.FillText));

			pen = new Pen(Color.White, 30);

			pg.Graphics.DrawLine(pen, 100, 200, 500, 200);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 75, 175, 50, 50);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 475, 175, 50, 50);

			pg.Graphics.DrawLine(pen, 100, 400, 500, 400);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 75, 375, 50, 50);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 475, 375, 50, 50);

			pg.Graphics.DrawLine(pen, 100, 600, 500, 600);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 75, 575, 50, 50);
			pg.Graphics.FillEllipse(new SolidBrush(Color.White), 475, 575, 50, 50);
		}

		private void DashExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Dash pattern example");

			Pen pen = new Pen(Color.Black, 5);

			pen.DashStyle = DashStyle.Dash;
			pg.Graphics.DrawLine(pen, 100, 150, 500, 150);
			pg.Graphics.DrawString("Dash style", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 170, new TextState(TextRenderingMode.FillText));

			pen.DashStyle = DashStyle.DashDot;
			pg.Graphics.DrawLine(pen, 100, 250, 500, 250);
			pg.Graphics.DrawString("DashDot style", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 270, new TextState(TextRenderingMode.FillText));

			pen.DashStyle = DashStyle.DashDotDot;
			pg.Graphics.DrawLine(pen, 100, 350, 500, 350);
			pg.Graphics.DrawString("DashDotDot style", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 370, new TextState(TextRenderingMode.FillText));

			pen.DashStyle = DashStyle.Dot;
			pg.Graphics.DrawLine(pen, 100, 450, 500, 450);
			pg.Graphics.DrawString("Dot style", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 470, new TextState(TextRenderingMode.FillText));

			pen.DashStyle = DashStyle.Solid;
			pg.Graphics.DrawLine(pen, 100, 550, 500, 550);
			pg.Graphics.DrawString("Solid style ", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 570, new TextState(TextRenderingMode.FillText));

			pen.DashStyle = DashStyle.Custom;
			pen.DashPattern = new double[] { 10, 5, 8 };
			pg.Graphics.DrawLine(pen, 100, 650, 500, 650);
			pg.Graphics.DrawString("Custom", new BuiltInFont("Helvetica", 21), new Pen(Color.Black),
				new SolidBrush(Color.Black), 100, 670, new TextState(TextRenderingMode.FillText));
		}

		private void ClipExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Clipping path example");

			pg.Graphics.Clip = new System.Drawing.Rectangle(100, 200, 400, 400);
			pg.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(System.Drawing.KnownColor.LightGray)), pg.Graphics.Clip);
			pg.Graphics.FillPolygon(new SolidBrush(Color.Black), StarPoints, FillMode.Winding);

			pg.Graphics.Transform.Rotate(-25);
			pg.Graphics.Clip = new System.Drawing.Rectangle(150, 250, 300, 400);
			pg.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(System.Drawing.KnownColor.LightGray)), pg.Graphics.Clip);
			pg.Graphics.FillPolygon(new SolidBrush(Color.Red), StarPoints, FillMode.Winding);
		}

		private void ImageExample(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Image transforms example");

			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic2.png"))) {
				pg.Graphics.DrawImage(image, 200, 100, 200, double.NaN);

				double sqrtHalf = Math.Sqrt(2) / 2;
				pg.Graphics.Transform.Rotate(45);
				pg.Graphics.DrawImage(image, (100 + 230) * sqrtHalf, (-100 + 230) * sqrtHalf, double.NaN, 100);
				pg.Graphics.DrawImage(image, (100 + 430) * sqrtHalf, (-100 + 430) * sqrtHalf, double.NaN, 100);
				pg.Graphics.DrawImage(image, (520 + 230) * sqrtHalf, (-520 + 230) * sqrtHalf, double.NaN, 100);
				pg.Graphics.DrawImage(image, (520 + 430) * sqrtHalf, (-520 + 430) * sqrtHalf, double.NaN, 100);

				pg.Graphics.Transform.Rotate(-45);
				pg.Graphics.DrawImage(image, (pg.Graphics.Rect.Width / 2) - (200 / 2), 600, 200, double.NaN);

				pg.Graphics.Transform.Rotate(-45);
				pg.Graphics.DrawImage(image, (30 - 630) * sqrtHalf, (30 + 630) * sqrtHalf, double.NaN, 100);
				double x = pg.Graphics.Rect.Width - 170;
				pg.Graphics.DrawImage(image, (x - 630) * sqrtHalf, (x + 630) * sqrtHalf, double.NaN, 100);
			}
		}

		private void ImageExample2(PDFDocument doc) {
			Page pg = doc.AddPage();
			AddDescription(pg, "Image types example");

			BuiltInFont font = new BuiltInFont("Helvetica", 10);
			double textHeight = pg.Graphics.MeasureString("pic1.jpg", font).Height;
			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic1.jpg"))) {
				pg.Graphics.DrawString("pic1.jpg", font, new Pen(Color.Black),
					new SolidBrush(Color.Black), 50, 200 + textHeight);
				pg.Graphics.DrawImage(image, 50, 100, double.NaN, 100);
			}

			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic2.png"))) {
				pg.Graphics.DrawString("pic2.png", font, new Pen(Color.Black),
					new SolidBrush(Color.Black), 300, 200 + textHeight);
				pg.Graphics.DrawImage(image, 300, 100, double.NaN, 100);
			}

			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic3.jpg"))) {
				pg.Graphics.DrawString("pic3.jpg", font, new Pen(Color.Black),
					new SolidBrush(Color.Black), 50, 400 + textHeight);
				pg.Graphics.DrawImage(image, 50, 300, double.NaN, 100);
			}

			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic1.jpg"))) {
				pg.Graphics.DrawString("pic1.jpg", font, new Pen(Color.Black),
					new SolidBrush(Color.Black), 300, 400 + textHeight);
				pg.Graphics.DrawImage(image, 300, 300, double.NaN, 100);
			}

			using (Image image = Image.FromStream(GetType().Assembly.GetManifestResourceStream("Examples.pic2.png"))) {
				pg.Graphics.DrawString("pic2.png", font, new Pen(Color.Black),
					new SolidBrush(Color.Black), 50, 600 + textHeight);
				pg.Graphics.DrawImage(image, 50, 500, double.NaN, 100);
			}
		}

		public void AddDescription(Page pg, string description) {
			pg.Graphics.DrawString(description,
				new BuiltInFont("Times Roman", 21, FontStyle.Italic),
				new Pen(Color.Black),
				new SolidBrush(Color.Black), 10, 30, new TextState(TextRenderingMode.FillText));
		}
	}
}
