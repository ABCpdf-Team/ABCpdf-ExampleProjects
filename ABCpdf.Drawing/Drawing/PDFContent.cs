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

#if DEBUG
#define CONTENT_VALIDATION
#endif

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.InteropServices;
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Atoms;
using WebSupergoo.ABCpdf13.Objects;
using DocPage = WebSupergoo.ABCpdf13.Objects.Page;
#if CONTENT_VALIDATION
using System.Windows.Forms;
#endif


namespace WebSupergoo.ABCpdf13.Drawing {
	#region PDFContent
	/// <summary>
	/// PDFContent class is a simple constructor of the pdf content.
	/// It provides easy access to the most of the commands described in the PDF Reference
	/// </summary>
	internal class PDFContent {
		public sealed class DocumentCache {
			[StructLayout(LayoutKind.Auto)]
			private struct ReferencedImage {
				public WeakReference Image;
				public int ImageID;

				public ReferencedImage(Image image, int imageID) {
					Image = new WeakReference(image);
					ImageID = imageID;
				}
			}

			private Dictionary<string, Text.FontMetrics> _fontMetrics
				= new Dictionary<string, Text.FontMetrics>(14);
			private LinkedList<ReferencedImage> _images = new LinkedList<ReferencedImage>();

			public void Clear() {
				_images.Clear();
			}

			public Text.FontMetrics RegisterFontMetric(BuiltInFont font) {
				Text.FontMetrics metrics;
				if (!_fontMetrics.TryGetValue(font.BuiltInName, out metrics)) {
					metrics = font.FontMetrics;
					_fontMetrics.Add(font.BuiltInName, metrics);
				}

				return metrics;
			}

			public int GetReferencedImageID(Image image) {
				LinkedListNode<ReferencedImage> node = _images.First;
				while (node != null) {
					object obj = node.Value.Image.Target;
					if (obj == null) {
						LinkedListNode<ReferencedImage> nextNode = node.Next;
						_images.Remove(node);
						node = nextNode;
					} else {
						if (obj == image)
							return node.Value.ImageID;
						node = node.Next;
					}
				}
				return 0;
			}
			public void AddReferencedImage(Image image, int imageID) {
				_images.AddFirst(new ReferencedImage(image, imageID));
			}
		}

		#region Constructors
		public PDFContent() { }
		public PDFContent(Doc doc): this(doc, new DocumentCache()) { }
		public PDFContent(Doc doc, DocumentCache docCache) {
			mDoc = doc;
			mDocCache = docCache;
		}
		#endregion

		internal Doc Doc { get { return mDoc; } }
		internal System.Text.StringBuilder Contents { get { return mContents; } }

		/// <summary>The ID of the object containing the resources dictionary.</summary>
		protected virtual int ResourcesObjectID {
			get {
				int id = mDoc.Page;
				if(id == 0)
					mDoc.Page = id = mDoc.AddPage();
				return id;
			}
		}

		#region PDF content methods
		public int GetReferencedImageID(Image image) {
			return mDocCache.GetReferencedImageID(image);
		}

		public void AddReferencedImage(Image image, int imageID) {
			mDocCache.AddReferencedImage(image, imageID);
		}

		/// <summary>Begin new subpath.</summary>
		/// <param name="x">x coordinate</param>
		/// <param name="y">y coordinate</param>
		public virtual void Move(double x, double y) {
			AppendParameter(x);
			AppendParameter(y);
			mContents.Append(" m");
#if CONTENT_VALIDATION
			Validate("m");
#endif
		}
		/// <summary>Append straight line segment to path.</summary>
		/// <param name="x">x coordinate </param>
		/// <param name="y">y coordinate </param>
		public virtual void Line(double x, double y) {
			AppendParameter(x);
			AppendParameter(y);
			mContents.Append(" l");
#if CONTENT_VALIDATION
			Validate("l");
#endif
		}
		/// <summary>Append rectangle to path.</summary>
		/// <param name="x">Bottom left x coordinate</param>
		/// <param name="y">Bottom left y coordinate</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		public virtual void Rect(double x, double y, double w, double h) {
			AppendParameter(x);
			AppendParameter(y);
			AppendParameter(w);
			AppendParameter(h);
			mContents.Append(" re");
#if CONTENT_VALIDATION
			Validate("re");
#endif
		}
		/// <summary>Append curved segment to path (three control points).</summary>
		/// <param name="x1">x coordinate (1st control point)</param>
		/// <param name="y1">y coordinate (1st control point)</param>
		/// <param name="x2">x coordinate (2nd control point)</param>
		/// <param name="y2">y coordinate (2nd control point)</param>
		/// <param name="x3">x coordinate (3rd control point)</param>
		/// <param name="y3">y coordinate (3rd control point)</param>
		public virtual void Bezier(double x1, double y1, double x2, double y2, double x3, double y3) {
			AppendParameter(x1);
			AppendParameter(y1);
			AppendParameter(x2);
			AppendParameter(y2);
			AppendParameter(x3);
			AppendParameter(y3);
			mContents.Append(" c");
#if CONTENT_VALIDATION
			Validate("c");
#endif
		}
		/// <summary>Close subpath.</summary>
		public virtual void Close() {
			mContents.Append(" h");
		}
		/// <summary>Stroke path.</summary>
		public virtual void Stroke() {
			mContents.Append(" S");
#if CONTENT_VALIDATION
			Validate("S");
#endif
		}
		/// <summary>Fill path using nonzero winding number rule.</summary>
		public virtual void Fill() {
			mContents.Append(" f");
#if CONTENT_VALIDATION
			Validate("f");
#endif
		}
		/// <summary>Fill path using even-odd rule.</summary>
		public virtual void FillEvenOddRule() {
			mContents.Append(" f*");
#if CONTENT_VALIDATION
			Validate("f*");
#endif
		}
		/// <summary>Set clipping path using nonzero winding number rule.</summary>
		public virtual void Clip() {
			mContents.Append(" W n");
#if CONTENT_VALIDATION
			Validate("W");
#endif
		}
		/// <summary>Set clipping path using even-odd rule.</summary>
		public virtual void ClipEvenOddRule() {
			mContents.Append(" W* n");
#if CONTENT_VALIDATION
			Validate("W*");
#endif
		}
		/// <summary>Save graphics state.</summary>
		public virtual void SaveState() {
			mContents.Append(" q");
#if CONTENT_VALIDATION
			Validate("q");
#endif
		}
		/// <summary>Restore graphics state.</summary>
		public virtual void RestoreState() {
			mContents.Append(" Q");
#if CONTENT_VALIDATION
			Validate("Q");
#endif
		}
		/// <summary>Set line width.</summary>
		/// <param name="v">Line width</param>
		public virtual void SetLineWidth(double v) {
			AppendParameter(v);
			mContents.Append(" w");
#if CONTENT_VALIDATION
			Validate("w");
#endif
		}
		/// <summary>The line cap for the ends of any lines to be stroked.</summary>
		public enum LineCap {
			Butt,
			Round,
			ProjectingSquare
		}
		/// <summary>Set line cap style.</summary>
		/// <param name="v">Line cap style</param>
		public virtual void SetLineCap(int v) {
			AppendParameter(v);
			mContents.Append(" J");
#if CONTENT_VALIDATION
			Validate("J");
#endif
		}
		/// <summary>The line join for the shape of joints between connected segments of a path.</summary>
		public enum LineJoin {
			Miter,
			Round,
			Bevel
		}
		/// <summary>Set line join style.</summary>
		/// <param name="v">Line join style</param>
		public virtual void SetLineJoin(int v) {
			AppendParameter(v);
			mContents.Append(" j");
#if CONTENT_VALIDATION
			Validate("j");
#endif
		}
		/// <summary>Set miter limit.</summary>
		/// <param name="v">Miter limit</param>
		public virtual void SetMiterLimit(double v) {
			AppendParameter(v);
			mContents.Append(" M");
#if CONTENT_VALIDATION
			Validate("M");
#endif
		}
		/// <summary>Set line dash pattern.</summary>
		/// <param name="dashPattern">Dash pattern</param>
		public virtual void LineDash(string dashPattern) {
			mContents.Append(" ").Append(dashPattern).Append(" d");
#if CONTENT_VALIDATION
			Validate("d");
#endif
		}
		/// <summary>Set gray level for stroking operations.</summary>
		/// <param name="w">Level of gray color</param>
		public virtual void SetGrayStrokeColor(double w) {
			AppendParameter(w);
			mContents.Append(" G");
#if CONTENT_VALIDATION
			Validate("G");
#endif
		}
		/// <summary>Set gray level for nonstroking operations.</summary>
		/// <param name="w">Level of gray color</param>
		public virtual void SetGrayNonStrokeColor(double w) {
			AppendParameter(w);
			mContents.Append(" g");
#if CONTENT_VALIDATION
			Validate("g");
#endif
		}
		/// <summary>Set RGB color for stroking operations.</summary>
		/// <param name="r">Level of red color</param>
		/// <param name="g">Level of green color </param>
		/// <param name="b">Level of blue color</param>
		public virtual void SetRGBStrokeColor(double r, double g, double b) {
			AppendParameter(r);
			AppendParameter(g);
			AppendParameter(b);
			mContents.Append(" RG");
#if CONTENT_VALIDATION
			Validate("RG");
#endif
		}
		/// <summary>Set RGB color for nonstroking operations.</summary>
		/// <param name="r">Level of red color</param>
		/// <param name="g">Level of green color </param>
		/// <param name="b">Level of blue color</param>
		public virtual void SetRGBNonStrokeColor(double r, double g, double b) {
			AppendParameter(r);
			AppendParameter(g);
			AppendParameter(b);
			mContents.Append(" rg");
#if CONTENT_VALIDATION
			Validate("rg");
#endif
		}
		/// <summary>Set CMYK color for stroking operations.</summary>
		/// <param name="c">Level of cyan color</param>
		/// <param name="m">Level of magenta color</param>
		/// <param name="y">Level of yellow color</param>
		/// <param name="k">Level of black color</param>
		public virtual void SetCMYKStrokeColor(double c, double m, double y, double k) {
			AppendParameter(c);
			AppendParameter(m);
			AppendParameter(y);
			AppendParameter(k);
			mContents.Append(" K");
#if CONTENT_VALIDATION
			Validate("K");
#endif
		}
		/// <summary>Set CMYK color for nonstroking operations.</summary>
		/// <param name="c">Level of cyan color</param>
		/// <param name="m">Level of magenta color</param>
		/// <param name="y">Level of yellow color</param>
		/// <param name="k">Level of black color</param>
		public virtual void SetCMYKNonStrokeColor(double c, double m, double y, double k) {
			AppendParameter(c);
			AppendParameter(m);
			AppendParameter(y);
			AppendParameter(k);
			mContents.Append(" k");
#if CONTENT_VALIDATION
			Validate("k");
#endif
		}
		/// <summary>Concatenate matrix to current transformation matrix.</summary>
		/// <param name="a">Transformation matrix parameter</param>
		/// <param name="b">Transformation matrix parameter</param>
		/// <param name="c">Transformation matrix parameter</param>
		/// <param name="d">Transformation matrix parameter</param>
		/// <param name="h">Transformation matrix parameter</param>
		/// <param name="v">Transformation matrix parameter</param>
		public virtual void Transform(double a, double b, double c, double d, double h, double v) {
			AppendParameter(a);
			AppendParameter(b);
			AppendParameter(c);
			AppendParameter(d);
			AppendParameter(h);
			AppendParameter(v);
			mContents.Append(" cm");
#if CONTENT_VALIDATION
			Validate("cm");
#endif
		}
		/// <summary>Append arc segment to path.</summary>
		/// <param name="start">Start angle of the arc in degrees</param>
		/// <param name="end">End angle of the arc in degrees</param>
		/// <param name="cx">Horizontal center of the arc</param>
		/// <param name="cy">Vertical center of the arc</param>
		/// <param name="rx">Horizontal radius of the arc</param>
		/// <param name="ry">Vertical radius of the arc</param>
		/// <param name="angle">Rotate angle</param>
		/// <param name="inMove">If true, move to the first point of the arc</param>
		public virtual void Arc(double start, double end, double cx, double cy, double rx, double ry, double angle, bool inMove) {
			angle = angle / 180 * Math.PI;
			start = start / 180 * Math.PI;
			end = end / 180 * Math.PI;
			// for efficiency reasons it might be a good idea to suppress the calculation of the bounds rect here
			const int theN = 8;
			const int theNum = (theN * 3) - 2; // start and end only have one 	control point

			double delta = (end - start) / (theN - 1);
			double kv = 4 * (1 - Math.Cos(delta / 2)) / (3 * Math.Sin(delta / 2));
			double[] x = new double[theNum];
			double[] y = new double[theNum];
			double ca = 0, sa = 0, tx = 0, ty = 0;
			int i = 0, n = 0;

			// make points
			for (i = 0; i < theN; i++) {
				// establish point
				n = i * 3;
				ca = Math.Cos((i * delta) + start);
				sa = Math.Sin((i * delta) + start);
				x[n] = rx * ca;
				y[n] = ry * sa;
				sa = kv * rx * sa;
				ca = kv * ry * ca;
				// establish control points
				if ((n + 1) < theNum) x[n + 1] = x[n] - sa;
				if ((n + 1) < theNum) y[n + 1] = y[n] + ca;
				if (n > 0) x[n - 1] = x[n] + sa;
				if (n > 0) y[n - 1] = y[n] - ca;
			}
			// translate and rotate
			ca = Math.Cos(angle);
			sa = Math.Sin(angle);
			for (i = 0; i < theNum; i++) {
				tx = (x[i] * ca) - (y[i] * sa) + cx;
				ty = (x[i] * sa) + (y[i] * ca) + cy;
				x[i] = tx; y[i] = ty;
			}
			// draw ellipse
			if (inMove) Move(x[0], y[0]);
			for (i = 0; i < (theN - 1); i++) {
				n = i * 3;
				Bezier(x[n + 1], y[n + 1], x[n + 2], y[n + 2], x[n + 3], y[n + 3]);
			}
		}
		/// <summary>Append pdf content.</summary>
		/// <param name="theContent">The content</param>
		public virtual void AddContent(PDFContent theContent) {
			mContents.Append(theContent.mContents.ToString());
		}
		/// <summary>Add graphic state dictionaries to the page resources.</summary>
		/// <param name="enumerable">Graphic states</param>
		private void WriteGStates(IEnumerable<ExtGState> enumerable) {	// idempotent
			foreach (ExtGState gstate in enumerable) {
				string path = "/Resources/ExtGState/" + gstate.Name + ":Ref";
				if (mDoc.GetInfoInt(ResourcesObjectID, path) == 0) {
					int theID = mDoc.AddObject(gstate.PdfObject);
					mDoc.SetInfo(ResourcesObjectID, path, theID);
				}
			}
		}

		/// <summary>Write content to the doc.</summary>
		public virtual bool AddToDoc() {	// idempotent
			if (mDoc.Page == 0)
				mDoc.Page = mDoc.AddPage();

			if (mContents.Length <= 0)
				return false;

			WriteAllGStates();
			mDoc.SetInfo(mDoc.FrameRect(), "stream", mContents.ToString());
			mContents.Clear();
			ShowErrors();
			return true;
		}

		protected void WriteAllGStates() {
			WriteGStates(mNonStrokeExtGStates.Values);
			WriteGStates(mStrokeExtGStates.Values);
			WriteGStates(mBlendModeExtGStates.Values);
			WriteGStates(mArbitraryExtGStates);
		}
		protected void ShowErrors() {
#if CONTENT_VALIDATION
			if(mErrors.Count > 0) {
				string errors = "The content contains following errors:";
				int errorsToShow = (mErrors.Count > 20)? 20: mErrors.Count;
				for(int i = 0; i< errorsToShow; i++)
					errors += "\n" + mErrors[i];
				if(errorsToShow < mErrors.Count)
					errors += "\n...";

				MessageBox.Show(errors, "PDFContent",
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
#endif
		}

		#region ExtGState
		/// <summary>Extended graphic state.</summary>
		private class ExtGState {
			public string Name = "";
			public string PdfObject = "";
		}
		#endregion

		/// <summary>Set nonstroking alpha constant.</summary>
		/// <param name="ca">Nonstroking alpha constant</param>
		public virtual void SetNonStrokeAlpha(double ca) {
			string val = ca.ToString(mFloatFormat, NumberFormatInfo.InvariantInfo);
			ca = double.Parse(val, NumberFormatInfo.InvariantInfo);

			ExtGState gstate;
			if ( !mNonStrokeExtGStates.TryGetValue(ca, out gstate) ) {
				gstate = new ExtGState();
				gstate.Name = GetNextGSName();
				gstate.PdfObject = "<< /Type/ExtGState\n /ca "+ val + "\n >>";
				mNonStrokeExtGStates.Add(ca, gstate);
			}
			mContents.Append(" /").Append(gstate.Name).Append(" gs");
#if CONTENT_VALIDATION
			Validate("gs");
#endif
		}
		/// <summary>Set stroking alpha constant.</summary>
		/// <param name="CA">Stroking alpha constant</param>
		public virtual void SetStrokeAlpha(double CA) {
			string val = CA.ToString(mFloatFormat, NumberFormatInfo.InvariantInfo);
			CA = double.Parse(val, NumberFormatInfo.InvariantInfo);

			ExtGState gstate;
			if ( !mStrokeExtGStates.TryGetValue(CA, out gstate) ) {
				gstate = new ExtGState();
				gstate.Name = GetNextGSName();
				gstate.PdfObject = "<< /Type/ExtGState\n /CA "+ val + "\n >>";
				mStrokeExtGStates.Add(CA, gstate);
			}
			mContents.Append(" /").Append(gstate.Name).Append(" gs");
#if CONTENT_VALIDATION
			Validate("gs");
#endif
		}
		/// <summary>Set graphic state.</summary>
		/// <param name="pdfFormattedDict">Graphic State dictionary in pdf native format</param>
		public virtual void SetGraphicState(string pdfFormattedDict) {
			ExtGState gstate = new ExtGState();
			gstate.Name = GetNextGSName();
			gstate.PdfObject = pdfFormattedDict;
			mArbitraryExtGStates.Add(gstate);
			mContents.Append(" /").Append(gstate.Name).Append(" gs");
#if CONTENT_VALIDATION
			Validate("gs");
#endif
		}
		/// <summary>Set blend mode.</summary>
		/// <param name="blendMode">Blend mode</param>
		public virtual void SetBlendMode(string blendMode) {
			ExtGState gstate;
			if ( !mBlendModeExtGStates.TryGetValue(blendMode, out gstate) ) {
				string name = GetNextGSName();

				gstate = new ExtGState();
				gstate.Name = name;
				gstate.PdfObject = "<< /Type/ExtGState\n /BM /"+ blendMode + "\n >>";
				mBlendModeExtGStates.Add(blendMode, gstate);
			}
			mContents.Append(" /").Append(gstate.Name).Append(" gs");
#if CONTENT_VALIDATION
			Validate("gs");
#endif
		}
		/// <summary>Get next available name for Do pdf command.</summary>
		/// <returns>Name of the Do command</returns>
		private string GetNextXObjectName() {
			for (int i = mXObjNumber; ; i++) {
				string objName = "Iabc" + i.ToString(NumberFormatInfo.InvariantInfo);
				string test = mDoc.GetInfo(ResourcesObjectID, "/Resources/XObject/" + objName);
				if (test == "") {
					mXObjNumber = i + 1;
					return objName;
				}
			}
		}
		/// <summary>Get next available name for gs pdf command.</summary>
		/// <returns>Name of the gs command</returns>
		private string GetNextGSName() {
			for (int i = mGSNumber; ; i++) {
				string gsName = "GS" + i.ToString(NumberFormatInfo.InvariantInfo);
				string test = mDoc.GetInfo(ResourcesObjectID, "/Resources/ExtGState/" + gsName);
				if (test == "") {
					mGSNumber = i + 1;
					return gsName;
				}
			}
		}

		/// <summary>Find or add a resource if it is not already there.</summary>
		/// <param name="type">The type of the resource eg Font or XObject</param>
		/// <param name="id">The ID of the resource</param>
		/// <returns>The name of the resource</returns>
		private string FindOrAddResource(string type, int id) {
			DocPage page = (DocPage)mDoc.ObjectSoup[ResourcesObjectID];
			IDictionary<string, Atom> map = page.GetResourceMap(page.ID, type);
			if (map != null) {
				foreach(KeyValuePair<string, Atom> pair in map) {
					RefAtom theRef = pair.Value as RefAtom;
					if(theRef != null && theRef.ID == id) 
						return pair.Key;
				}
			}
			return page.AddResource(mDoc.ObjectSoup[id], "XObject", "Iabc" + id.ToString(NumberFormatInfo.InvariantInfo));
		}

		/// <summary>Set rendering intent.</summary>
		/// <param name="intent">Rendering intent</param>
		public virtual void SetRenderingIntent(string intent) {
			mContents.Append(" /").Append(intent).Append(" ri");
#if CONTENT_VALIDATION
			Validate("ri");
#endif
		}
		/// <summary>Add image to the pdf content.</summary>
		/// <param name="imageID">ID of the image returned by Doc.AddImage method</param>
		public virtual void DoImage(int imageID) {
			imageID = mDoc.GetInfoInt(imageID , "XObject");
			if (imageID == 0)
				throw new ArgumentException("Invalid image ID.");
			string doImageCommand = FindOrAddResource("XObject", imageID);
			mContents.Append(" /").Append(doImageCommand).Append(" Do");
#if CONTENT_VALIDATION
			Validate("Do");
#endif
		}
		/// <summary>Add Form XObject to the pdf content.</summary>
		/// <param name="xobjID">ID of the Form XObject returned by PDFBoundedContent.WriteToFormXObject method</param>
		public virtual void DoFormXObject(PDFFormXObjectID xobjID) {
			string doCommand = FindOrAddResource("XObject", xobjID.ID);
			mContents.Append(" /").Append(doCommand).Append(" Do");
#if CONTENT_VALIDATION
			Validate("Do");
#endif
		}
		/// <summary>Begin text object.</summary>
		public virtual void BeginText() {
			mContents.Append(" BT");
#if CONTENT_VALIDATION
			Validate("BT");
#endif
		}
		/// <summary>End text object.</summary>
		public virtual void EndText() {
			mContents.Append(" ET");
#if CONTENT_VALIDATION
			Validate("ET");
#endif
		}
		/// <summary>Text rendering mode.</summary>
		/// <param name="mode">Text rendering mode</param>
		public virtual void SetTextRenderingMode(TextRenderingMode mode) {
			AppendParameter((double)mode);
			mContents.Append(" Tr");
#if CONTENT_VALIDATION
			Validate("Tr");
#endif
		}
		/// <summary>Set character spacing.</summary>
		/// <param name="tc">Character spacing</param>
		public virtual void SetCharacterSpacing(double tc) {
			AppendParameter(tc);
			mContents.Append(" Tc");
#if CONTENT_VALIDATION
			Validate("Tc");
#endif
		}
		/// <summary>Set Word Spacing.</summary>
		/// <param name="tw">Word spacing</param>
		public virtual void SetWordSpacing(double tw) {
			AppendParameter(tw);
			mContents.Append(" Tw");
#if CONTENT_VALIDATION
			Validate("Tw");
#endif
		}
		/// <summary>Set horizontal text scaling.</summary>
		/// <param name="th">Horizontal text scaling</param>
		public virtual void SetHorizontalScaling(double th) {
			AppendParameter(th);
			mContents.Append(" Tz");
#if CONTENT_VALIDATION
			Validate("Tz");
#endif
		}
		/// <summary>Set text leading.</summary>
		/// <param name="tl">Text leading</param>
		public virtual void SetTextLeading(double tl) {
			AppendParameter(tl);
			mContents.Append(" TL");
#if CONTENT_VALIDATION
			Validate("TL");
#endif
		}
		/// <summary>Set text rise.</summary>
		/// <param name="ts">Text rise</param>
		public virtual void SetTextRise(double ts) {
			AppendParameter(ts);
			mContents.Append(" Ts");
#if CONTENT_VALIDATION
			Validate("Ts");
#endif
		}
		/// <summary>Show text.</summary>
		/// <param name="text">Text string</param>
		public virtual void ShowTextString(string text) {
			mContents.AppendFormat(" ({0}) Tj", mPdfString.Replace(text,  @"\$&"));
#if CONTENT_VALIDATION
			Validate("Tj");
#endif
		}
		/// <summary>Set text font and size.</summary>
		/// <param name="fontObjID">Font object ID</param>
		/// <param name="fontSize">Font size</param>
		public virtual void SetFont(int fontObjID, double fontSize) {
			string fontCommand = FindOrAddResource("Font", fontObjID);
			mContents.AppendFormat(" /{0} {1} Tf", fontCommand, fontSize);
#if CONTENT_VALIDATION
			Validate("Tf");
#endif
		}
		/// <summary>Set text matrix and text line matrix.</summary>
		/// <param name="a">Text matrix parameter</param>
		/// <param name="b">Text matrix parameter</param>
		/// <param name="c">Text matrix parameter</param>
		/// <param name="d">Text matrix parameter</param>
		/// <param name="e">Text matrix parameter</param>
		/// <param name="f">Text matrix parameter</param>
		public virtual void SetTextMatrix(double a, double b, double c, double d, double e, double f) {
			AppendParameter(a);
			AppendParameter(b);
			AppendParameter(c);
			AppendParameter(d);
			AppendParameter(e);
			AppendParameter(f);
			mContents.Append(" Tm");
#if CONTENT_VALIDATION
			Validate("Tm");
#endif
		}
		/// <summary>Move to start of next text line.</summary>
		public virtual void NextLine() {
			mContents.Append(" T*");
#if CONTENT_VALIDATION
			Validate("T*");
#endif
		}
		/// <summary>Move text position.</summary>
		/// <param name="tx">x offset from the start of the current line</param>
		/// <param name="ty">y offset from the start of the current line</param>
		public virtual void TextMove(double tx, double ty) {
			AppendParameter(tx);
			AppendParameter(ty);
			mContents.Append(" Td");
#if CONTENT_VALIDATION
			Validate("Td");
#endif
		}
		
		private void AppendParameter(double v) {
			double iv = (double)decimal.Truncate((decimal)v);
			mContents.AppendFormat(NumberFormatInfo.InvariantInfo, " {0:" + mFloatFormat + "}", v);
		}
		#endregion
		
		#region Declare variables
		/// <summary>The PDF floating-point format.</summary>
		internal const string mFloatFormat = "0.#####";
		/// <summary>The PDF string escape.</summary>
		private static readonly Regex mPdfString = new Regex(@"[\\\(\)]",
			RegexOptions.Compiled | RegexOptions.Singleline
			| RegexOptions.CultureInvariant);
		/// <summary>Last used number for XObject Do command (e.g. Iabc7 for mXObjNumber = 7).</summary>
		private int mXObjNumber = 0;
		/// <summary>Last used number for graphic state command (e.g. GS7 for mGSNumber = 7).</summary>
		private int mGSNumber = 0;
		/// <summary>Parent doc.</summary>
		private Doc mDoc;
		/// <summary>Document cache.</summary>
		private DocumentCache mDocCache;
		/// <summary>Pdf content string.</summary>
		private System.Text.StringBuilder mContents = new System.Text.StringBuilder();
		/// <summary>Binary tree of used extended graphic states for stroke color transparency.</summary>
		private SortedDictionary<double, ExtGState> mStrokeExtGStates = new SortedDictionary<double, ExtGState>();
		/// <summary>Binary tree of used extended graphic states for nonstroke color transparency.</summary>
		private SortedDictionary<double, ExtGState> mNonStrokeExtGStates = new SortedDictionary<double, ExtGState>();
		/// <summary>Hashtable of used extended graphic states for blending modes.</summary>
		private Dictionary<string, ExtGState> mBlendModeExtGStates = new Dictionary<string, ExtGState>();
		/// <summary>List of used ExtGStates.</summary>
		private List<ExtGState> mArbitraryExtGStates = new List<ExtGState>();

#if CONTENT_VALIDATION
		/// <summary>Pdf command validation.</summary>
		/// <param name="command">Pdf command</param>
		private void Validate(string command) {
			int generalOperation =  Array.BinarySearch(mGeneralOps, command);
			int textOperation =  Array.BinarySearch(mTextOps, command);

			if (generalOperation > 0 && mBeginTextOpen)
				mErrors.Add("Illegal operation '" + command + "' inside text object");
			else if (textOperation > 0 && !mBeginTextOpen)
				mErrors.Add("Illegal operation '" + command + "' outside text object");
			else {
				switch (command) {
					case "q":
						mOpenQCount++;
						break;
					case "Q":
						mOpenQCount--;
						if (mOpenQCount < 0)
							mErrors.Add("Invalid restore");
						break;
					case "BT":
						mBeginTextOpen = true;
						break;
					case "ET":
						mBeginTextOpen = false;
						break;
				}
			}
		}

		/// <summary>Array of pdf commands.</summary>
		private static readonly string[] mGeneralOps = {
			"b", "B", "b*", "B*", "c", "d", "Do", "f", "F", "f*",
			"g", "G", "h", "i", "j", "J", "k", "l", "m", "M", "n", "re",
			"rg", "RG", "ri", "s", "S", "v", "w", "W", "W*", "y" };
		/// <summary>Array of pdf commands used for text output.</summary>
		private static readonly string[] mTextOps = {
			"T*", "Tc", "Td", "TD", "Tf", "Tj", "TJ", "TL",
			"Tm", "Tr", "Ts", "Tw", "Tz" };

		/// <summary>Number of open "q" operators.</summary>
		private int mOpenQCount = 0;
		/// <summary>True if Begin text operator wasn't closed by End text operator.</summary>
		private bool mBeginTextOpen = false;
		/// <summary>List of errors in pdf content.</summary>
		private List<string> mErrors = new List<string>();
#endif
		#endregion
	}
	#endregion
}
