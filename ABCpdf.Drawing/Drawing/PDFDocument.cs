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
using WebSupergoo.ABCpdf13.Drawing;

namespace WebSupergoo.ABCpdf13.Drawing {
	#region PDFDocument
	/// <summary>Top level object that represents a PDF document.</summary>
	public class PDFDocument : IDisposable {
		#region Declare variables
		private Doc _doc = null;
		private List<Page> _pages = new List<Page>();
		private ColorSpace _colorSpace = ColorSpace.RGB;
		private float _resolution = 96;
		private PDFContent.DocumentCache _docCache = new PDFContent.DocumentCache();
		#endregion

		#region Properties
		/// <summary>The document</summary>
		public Doc Doc { get { return _doc; } }

		/// <summary>A collection of pages</summary>
		public PageCollection Pages {
			get { return new PageCollection(_pages); }
		}
		
		/// <summary>The color space for the document</summary>
		public Drawing.ColorSpace ColorSpace {
			get { return _colorSpace; }
			set { _colorSpace = value; }
		}

		/// <summary>The effective resolution of the document in dots per inch.
		/// When you write System.Drawing code, most measures are specified in pixel based units.
		/// However, measures such as font sizes are often specified in physical units rather than pixels.
		/// The size on the System.Drawing output surface is determined by the resolution of the output medium.
		/// As such, output to PDF requires a similar conversion and an effective
		/// resolution to mimic the behavior of System.Drawing.</summary>
		public float Resolution {
			get { return _resolution; }
			set { _resolution = value; }
		}
		#endregion

		#region Constructors and destructors
		/// <summary>Initializes a instance of the PDFDocument class.</summary>
		public PDFDocument() {
			_doc = new Doc();
		}

		/// <summary>Releases all resources used by this PDFDocument object.</summary>
		public void Dispose() {
			if (_doc != null) {
				_doc.Clear();
				_doc.Dispose();
			}
		}
		#endregion

		internal PDFContent NewContent() {
			return new PDFContent(_doc, _docCache);
		}
		internal PDFBoundedContent NewBoundedContent() {
			return new PDFBoundedContent(_doc, _docCache);
		}
		internal PDFBoundedContent NewBoundedContent(int xobjID) {
			return new PDFBoundedContent(_doc, _docCache, xobjID);
		}

		#region Page methods
		/// <summary>Adds a page to the current document.</summary>
		/// <returns>The page that this method creates.</returns>
		public Page AddPage() {
			Page page = new Page(this);
			_pages.Add(page);
			return page;
		}

		/// <summary>Adds a page of specified size to the current document.</summary>
		/// <param name="width">The width of pages in this document measured in points.</param>
		/// <param name="height">The height of pages in this document measured in points.</param>
		/// <returns>The page that this method creates.</returns>
		public Page AddPage(int width, int height) {
			_doc.MediaBox.Width = width;
			_doc.MediaBox.Height = height;
			_doc.Rect.SetRect(_doc.MediaBox);
			return AddPage();
		}

		/// <summary>Inserts a Page into the document at the specified index.</summary>
		/// <param name="index">The zero-based index at which the page should be inserted.</param>
		/// <returns>The page that this method creates.</returns>
		public Page InsertPage(int index) {
			_pages.Insert(index, new Page(index, this));
			return Pages[index];
		}
		#endregion

		#region Save methods
		/// <summary>Saves the document as PDF.</summary>
		/// <param name="stream">The destination stream.</param>
		/// <param name="flatten">Flatten mode.</param>
		public void Save(System.IO.Stream stream, bool flatten) {
			Realize(flatten);
			_docCache.Clear();
			_doc.Save(stream);
		}

		/// <summary>Saves the document as PDF.</summary>
		/// <param name="stream">The destination stream.</param>
		public void Save(System.IO.Stream stream) {
			Save(stream, false);
		}

		/// <summary>Saves the document as PDF.</summary>
		/// <param name="path">The destination file path.</param>
		/// <param name="flatten">Flatten mode.</param>
		public void Save(string path, bool flatten) {
			Realize(flatten);
			_docCache.Clear();
			_doc.Save(path);
		}

		/// <summary>Saves the document as PDF.</summary>
		/// <param name="path">The destination file path.</param>
		public void Save(string path) {
			Save(path, false);
		}

		internal void Realize(bool flatten) {
			if (_doc == null)
				throw new ApplicationException("Invalid document ptr");

			foreach (Page page in _pages) {
				page.Graphics.Realize();
				if (flatten) {
					_docCache.Clear();
					page.Graphics.Content.Doc.Flatten();
				}
			}
		}
		#endregion
	}
	#endregion
}
