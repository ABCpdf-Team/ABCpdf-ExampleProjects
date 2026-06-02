// ===========================================================================
//	©2013-2017 WebSupergoo. All rights reserved.
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
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Security;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;

namespace ABCpdfPrinting
{
	public sealed class NativePrint
	{
		public static void Print(Doc doc, string docName, PrinterSettings printerSettings) {
			List<int> pages = GetPages(doc, printerSettings);
			if (pages.Count <= 0)
				return;

			// save state
			double oldDotsPerInch = doc.Rendering.DotsPerInch;
			XRendering.ColorSpaceType oldColorSpace = doc.Rendering.ColorSpace;
			int oldBitsPerChannel = doc.Rendering.BitsPerChannel;
			bool oldAutoRotate = doc.Rendering.AutoRotate;
			string oldRect = doc.Rect.String;
			int oldPage = doc.Page;

			// print job variables
			IntPtr hdc = IntPtr.Zero;
			IntPtr hPrinter = IntPtr.Zero;
			Graphics graphics = null;
			int printJobID = 0;

			try {
				IntPtr hDevMode = printerSettings.GetHdevmode();
				hdc = SafePInvoke.CreateDC("WINSPOOL", printerSettings.PrinterName, SafePInvoke.GlobalLock(hDevMode)); // NB driver "winspool" is likely ignored
				hDevMode = SafePInvoke.GlobalFree(hDevMode);
				Debug.Assert(hDevMode == IntPtr.Zero);
				if (hdc == IntPtr.Zero)
					throw new Exception("Unable to create printer device context.", new InvalidPrinterException(printerSettings));

				bool ok = SafePInvoke.OpenPrinter(printerSettings.PrinterName, ref hPrinter, IntPtr.Zero);
				Debug.Assert((ok) || (hPrinter != IntPtr.Zero), "OpenPrinter succeeded but print handle is null.");
				if ((!ok) || (hPrinter == IntPtr.Zero))
					throw new Exception("Unable to open printer.", new InvalidPrinterException(printerSettings));

				int logPixelsX = SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.LogPixelsX);
				int logPixelsY = SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.LogPixelsY);
				double pageWidth = (double)SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.PhysicalWidth) / logPixelsX * 100;
				double pageHeight = (double)SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.PhysicalHeight) / logPixelsY * 100;
				double dstWidth = (double)SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.HorzRes) / logPixelsX * 100;
				double dstHeight = (double)SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.VertRes) / logPixelsY * 100;

				int res = 72;
				if (doc.Encryption.CanPrintHi) {
					res = logPixelsX;
					int colors = SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.NumColors);
					if (colors == 1) {
						int planes = SafePInvoke.GetDeviceCaps(hdc, SafePInvoke.DeviceCap.Planes);
						if (planes > 1)
							colors = 1 << Math.Min(6, planes);
					}
					if (colors > 2) // color is generally CMYK so to translate from dpi to ppi we divide by four
						res /= 4;
					if (res <= 0) // Invalid printer resolution - use the default value
						res = 72;
				}
				doc.Rendering.DotsPerInch = res;
				doc.Rendering.ColorSpace = XRendering.ColorSpaceType.Rgb;
				doc.Rendering.BitsPerChannel = 8;
				doc.Rendering.AutoRotate = false;

				SafePInvoke.DocInfo di = new SafePInvoke.DocInfo();
				di.Size = Marshal.SizeOf(typeof(SafePInvoke.DocInfo));
				di.DocName = docName;
				printJobID = SafePInvoke.StartDoc(hdc, ref di);
				if (printJobID <= 0)
					throw new Exception("Unable to start document printing.", new InvalidPrinterException(printerSettings));
				graphics = Graphics.FromHdc(hdc, hPrinter);

				foreach (int page in pages) {
					doc.PageNumber = page;
					if (doc.Page == 0)
						break; // should never happen
					doc.Rect.SetRect(doc.CropBox);

					double srcWidth = (doc.Rect.Width / 72) * 100;
					double srcHeight = (doc.Rect.Height / 72) * 100;
					double pageW = pageWidth, pageH = pageHeight;
					double dstW = dstWidth, dstH = dstHeight;
					const bool autoRotate = true;
					int rotate = ((Page)doc.ObjectSoup[doc.Page]).Rotation % 360;
					if (autoRotate && srcWidth != srcHeight && pageW != pageH && (srcWidth > srcHeight) != (pageW > pageH)) {
						double temp = pageW;
						pageW = pageH;
						pageH = temp;
						temp = dstW;
						dstW = dstH;
						dstH = temp;
						if (rotate <= -180)
							rotate += 360;
						else if (rotate > 180)
							rotate -= 360;
						rotate = rotate > 0 ? 90 : -90; // default to -90
						// Use -90 because we want the staple to be at a top corner
						// of the page.  Assuming a rotation of "rotate" (0 or 180 degrees)
						// produces upright contents, a rotation of -90 or 90 degrees
						// (respectively) produces outputs whose top is at the
						// left edge of the portrait page.  The staple at the top-left
						// corner of the portrait page will be at the top-right corner of the
						// contents.
					}
					else {
						if (rotate != 180 && rotate != -180)
							rotate = 0;
					}

					// if source bigger than destination then scale
					if ((srcWidth > pageW) || (srcHeight > pageH)) {
						double sx = pageW / srcWidth;
						double sy = pageH / srcHeight;
						double s = Math.Min(sx, sy);
						srcWidth *= s;
						srcHeight *= s;
					}

					// now center
					double x = (dstW - srcWidth) / 2;
					double y = (dstH - srcHeight) / 2;

					Matrix oldTransform = null;
					Matrix matrix = null;
					Image image;
					if (!doc.Encryption.CanPrintHi)
						image = doc.Rendering.GetBitmap();
					else {
						byte[] data = doc.Rendering.GetData(".emf");
						MemoryStream stream = new MemoryStream(data);
						image = new Metafile(stream);
					}
					try {
						if (SafePInvoke.StartPage(hdc) <= 0)
							throw new Exception("Unable to start document page printing."); ;

						switch (rotate) {
							case 90:
								matrix = new Matrix(0, 1, -1, 0, (float)(2 * y + srcHeight), 0);
								break;
							case -90:
								matrix = new Matrix(0, -1, 1, 0, 0, (float)(2 * x + srcWidth));
								break;
							case 180:
							case -180:
								matrix = new Matrix(-1, 0, 0, -1,
									(float)(2 * x + srcWidth), (float)(2 * y + srcHeight));
								break;
						}
						if (matrix != null) {
							oldTransform = graphics.Transform;
							graphics.MultiplyTransform(matrix);
						}

						RectangleF rect = new RectangleF((float)x, (float)y, (float)srcWidth, (float)srcHeight);
#if !DISPLAY_PRINT_AREA
						graphics.SetClip(rect);
#endif
						graphics.DrawImage(image, rect);
#if DISPLAY_PRINT_AREA
						using(Pen pen = new Pen(Color.Red)) {
							g.DrawRectangle(pen,
								rect.X, rect.Y, rect.Width, rect.Height);
						}
#endif
						if (oldTransform != null)
							graphics.Transform = oldTransform;
						graphics.Flush();

						if (SafePInvoke.EndPage(hdc) <= 0)
							throw new Exception("Unable to end document page printing."); ;
					}
					finally {
						if (matrix != null)
							matrix.Dispose();
						if (oldTransform != null)
							oldTransform.Dispose();
						image.Dispose();
					}
				}
			}
			finally {
				// disppose of print job variables
				if (graphics != null)
					graphics.Dispose();
				if (printJobID > 0) {
					int ok = SafePInvoke.EndDoc(hdc);
					Debug.Assert(ok > 0);
				}
				if (hdc != IntPtr.Zero) {
					bool ok = SafePInvoke.DeleteDC(hdc);
					Debug.Assert(ok);
				}
				if (hPrinter != IntPtr.Zero) {
					bool ok = SafePInvoke.ClosePrinter(hPrinter);
					Debug.Assert(ok);
				}

				// restore state
				doc.Rendering.DotsPerInch = oldDotsPerInch;
				doc.Rendering.ColorSpace = oldColorSpace;
				doc.Rendering.BitsPerChannel = oldBitsPerChannel;
				doc.Rendering.AutoRotate = oldAutoRotate;
				doc.Rect.String = oldRect;
				doc.Page = oldPage;
			}
		}

		private static List<int> GetPages(Doc doc, PrinterSettings printerSettings) {
			List<int> pages = new List<int>();
			switch (printerSettings.PrintRange) {
				case PrintRange.AllPages:
					for (int i = 1; i <= doc.PageCount; i++)
						pages.Add(i);
					break;
				case PrintRange.CurrentPage:
					pages.Add(doc.PageNumber);
					break;
				case PrintRange.Selection:
				case PrintRange.SomePages:
					for (int i = Math.Max(1, printerSettings.FromPage); i <= Math.Min(doc.PageCount, printerSettings.ToPage); i++)
						pages.Add(i);
					break;
			}

			return pages;
		}
	}

	[SecuritySafeCritical]
	internal static class SafePInvoke
	{
		public static IntPtr GlobalLock(IntPtr p) { return N.GlobalLock(p); }
		public static IntPtr GlobalFree(IntPtr p) { return N.GlobalFree(p); }

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DocInfo
		{
			public int Size;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string DocName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string OutputFile;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string DataType;
			public uint Type;
		}
		public enum DeviceCap
		{
			HorzRes = 8,
			VertRes = 10,
			Planes = 14,
			NumColors = 24,
			LogPixelsX = 88,
			LogPixelsY = 90,
			PhysicalWidth = 110,
			PhysicalHeight = 111,
			PhysicalOffsetX = 112,
			PhysicalOffsetY = 113,
			ScalingFactorX = 114,
			ScalingFactorY = 115,
		}

		public static IntPtr CreateDC(string driver, string device, IntPtr initData) { return N.CreateDC(driver, device, null, initData); }
		public static bool DeleteDC(IntPtr hdc) { return N.DeleteDC(hdc); }

		public static int GetDeviceCaps(IntPtr hdc, DeviceCap index) { return N.GetDeviceCaps(hdc, (int)index); }

		public static int StartDoc(IntPtr hdc, [In] ref DocInfo di) { return N.StartDoc(hdc, ref di); }
		public static int EndDoc(IntPtr hdc) { return N.EndDoc(hdc); }
		public static int StartPage(IntPtr hdc) { return N.StartPage(hdc); }
		public static int EndPage(IntPtr hdc) { return N.EndPage(hdc); }

		public static bool OpenPrinter(string printerName, ref IntPtr hPrinter, IntPtr pDefault) { return N.OpenPrinter(printerName, out hPrinter, pDefault); }
		public static bool ClosePrinter(IntPtr hPrinter) { return N.ClosePrinter(hPrinter); }

		[SuppressUnmanagedCodeSecurity]
		private static class N
		{
			[DllImport("kernel32.dll")]
			public static extern IntPtr GlobalLock(IntPtr p);

			[DllImport("kernel32.dll")]
			public static extern IntPtr GlobalFree(IntPtr p);

			[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]   // PrinterSettings.GetHdevmode returns a Unicode DEVMODE
			public static extern IntPtr CreateDC(string driver, string device, string output, IntPtr initData);

			[DllImport("gdi32.dll")]
			public static extern bool DeleteDC(IntPtr hdc);

			[DllImport("gdi32.dll")]
			public static extern int GetDeviceCaps(IntPtr hdc, int index);

			[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
			public static extern int StartDoc(IntPtr hdc, [In] ref DocInfo di);

			[DllImport("gdi32.dll")]
			public static extern int EndDoc(IntPtr hdc);

			[DllImport("gdi32.dll")]
			public static extern int StartPage(IntPtr hdc);

			[DllImport("gdi32.dll")]
			public static extern int EndPage(IntPtr hdc);

			[DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool OpenPrinter(string printerName, out IntPtr hPrinter, IntPtr pDefault);

			[DllImport("winspool.drv", SetLastError = true)]
			public static extern bool ClosePrinter(IntPtr hPrinter);
		}
	}
}
