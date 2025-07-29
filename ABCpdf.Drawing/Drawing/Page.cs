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

namespace WebSupergoo.ABCpdf13.Drawing {
	#region Page
	/// <summary>A visible page within the document.</summary>
	public sealed class Page {
		#region Declare variables
		private PDFDocument _document = null;
		private Drawing.Graphics _graphics = null;
		private int _id = -1;
		#endregion
	
		#region Properties
		/// <summary>The PDF document.</summary>
		internal PDFDocument Document { get { return _document; } }

		/// <summary>The ID of the PDF page.</summary>
		public int ID { get { return _id; } }

		/// <summary>The Graphics object for the page.</summary>
		public Drawing.Graphics Graphics {
			get {
				_document.Doc.Page = _id;
				return _graphics; 
			}
		}

		/// <summary>The Width of the page.</summary>
		public int Width {
			get {
				_document.Doc.Page = _id;
				return (int)_document.Doc.MediaBox.Width; 
			}
		}

		/// <summary>The Height of the page.</summary>
		public int Height {
			get {
				_document.Doc.Page = _id;
				return (int)_document.Doc.MediaBox.Height; 
			}
		}

		#endregion
		
		#region Constructors
		/// <summary>Page constructor.</summary>
		/// <param name="page">The PDFDocument instance.</param>
		internal Page(PDFDocument doc) {
			if (doc == null || doc.Doc == null)
				throw new ApplicationException("Invalid document ptr");

			_document = doc;

			_graphics = new Drawing.Graphics(this);
			_id = _document.Doc.AddPage();

			_document.Doc.Page = _id;
			_document.Doc.Rect.SetRect(_document.Doc.MediaBox);
			_document.Doc.Color.SetRgb(0, 0, 0);
		}
		
		/// <summary>Page constructor.</summary>
		/// <param name="index">The zero-based index at which Page should be inserted.</param>
		/// <param name="doc">The PDFDocument instance.</param>
		internal Page(int index, PDFDocument doc) {
			if (doc == null || doc.Doc == null)
				throw new ApplicationException("Invalid document ptr");

			_document = doc;

			_graphics = new Drawing.Graphics(this);
			_id = _document.Doc.AddPage(index);

			_document.Doc.Page = _id;
			_document.Doc.Rect.SetRect(_document.Doc.MediaBox);
			_document.Doc.Color.SetRgb(0, 0, 0);
		}
		#endregion
	}
	#endregion
}
