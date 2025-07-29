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
	#region Pen
	/// <summary>
	/// Pens can be used to draw lines and curves. This class cannot be inherited. 
	/// </summary>
	public sealed class Pen {
		#region Declare variables
		private Color _color = new Color();
		private double _width = 0;
		private Drawing2D.LineJoin _lineJoin = Drawing2D.LineJoin.Miter;
		private Drawing2D.LineCap _lineCap = Drawing2D.LineCap.Flat;
		private Drawing2D.DashStyle _dashStyle = Drawing2D.DashStyle.Solid;
		private double[] _dashPattern  = new double[] {};
		private double _dashOffset = 0;
		private double _miterLimit = 10.0f;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the color of this Pen.  
		/// </summary>
		public Color Color {
			get { return _color; }
			set { _color = value; }
		} 
		
		/// <summary>
		/// Gets or sets the width of this Pen.  
		/// </summary>
		public double Width {
			get { return _width; }
			set { _width = value; }
		}

		/// <summary>
		/// Gets or sets the join style for the ends of two consecutive lines drawn with this Pen.
		/// </summary>
		public Drawing2D.LineJoin LineJoin {
			get { return _lineJoin; }
			set { _lineJoin = value; }
		}
		
		/// <summary>
		/// Gets or sets the cap style used of lines drawn with this Pen. 
		/// </summary>
		public Drawing2D.LineCap LineCap {
			get { return _lineCap; }
			set { _lineCap = value; }
		}

		/// <summary>
		/// Gets or sets the style used for dashed lines drawn with this Pen. 
		/// </summary>
		public Drawing2D.DashStyle DashStyle {
			get { return _dashStyle; }
			set { _dashStyle = value; }
		}

		/// <summary>
		/// Gets or sets an array of custom dashes and spaces. 
		/// </summary>
		public double[] DashPattern {
			get { return _dashPattern; } 
			set { _dashPattern = value; }
		}

		/// <summary>
		/// Gets or sets the distance from the start of a line to the beginning of a dash pattern. 
		/// </summary>
		public double DashOffset {
			get { return _dashOffset; }
			set { _dashOffset = value; }
		}

		/// <summary>
		/// Gets or sets the limit of the thickness of the join on a mitered corner. 
		/// The default value is 10.0f.
		/// </summary>
		public double MiterLimit {
			get { return _miterLimit; }
			set { _miterLimit = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Pen class with the specified Color and Width properties.
		/// </summary>
		/// <param name="color">The color of this Pen object.</param>
		/// <param name="width">The width of this Pen object.</param>
		public Pen(Color color, double width) {
			_width = width;
			_color = color;
		}

		/// <summary>
		/// Initializes a new instance of the Pen class with the specified color. 
		/// </summary>
		/// <param name="color">The color of this Pen object.</param>
		public Pen(Color color) {
			_color = color;
		}
		#endregion

		public override bool Equals(object obj) {
			Pen p = obj as Pen;
			if (p != null) {
				return p.Color.Equals(Color) && (DashOffset == p.DashOffset) && 
					(DashPattern == p.DashPattern) && (DashStyle == p.DashStyle) &&
					(LineCap == p.LineCap) && (LineJoin == p.LineJoin) && (MiterLimit == p.MiterLimit) &&
					(Width == p.Width);
			}
			else {
				return false;
			}
		}

		public override int GetHashCode() {
			return base.GetHashCode ();
		}


	}
	#endregion
}
