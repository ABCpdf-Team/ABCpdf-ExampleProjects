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
	public interface Brush {
		WebSupergoo.ABCpdf13.Drawing.Color Color {
			get;
			set;
		}
	}
	
	#region SolidBrush
	/// <summary>
	/// Used to fill the interiors of graphical shapes such as rectangles, ellipses, pies, polygons, and paths.
	/// </summary>
	public sealed class SolidBrush : Brush {
		#region Declare variables
		private Color _color;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the color of this Brush object.  
		/// </summary>
		public Color Color {
			get { return _color; }
			set { _color = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Brush class with 
		/// the specified Color property.
		/// </summary>
		/// <param name="color">The color of this System.Drawing.Pen object.</param>
		public SolidBrush(Color color) {
			_color = color;
		}
		#endregion

		public override bool Equals(object obj) {
			Brush b = obj as Brush;
			if (b != null)
				return b.Color.Equals(Color);
			else
				return false;
		}

		public override int GetHashCode() {
			return base.GetHashCode ();
		}


	}
	#endregion
}
