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

namespace WebSupergoo.ABCpdf13.Drawing
{
	#region FontStyle
	[FlagsAttribute]
	/// <summary>
	/// Specifies style information applied to text.
	/// </summary>
	public enum FontStyle
	{ 
		/// <summary>
		/// Normal text.
		/// </summary>
		Regular = 0,
		/// <summary>
		/// Bold text.
		/// </summary>
		Bold = 1,
		/// <summary>
		/// Italic text.
		/// </summary>
		Italic = 2,
		/// <summary>
		/// Underlined text.
		/// </summary>
		Underline = 4,
		/// <summary>
		/// Text with a line through the middle.
		/// </summary>
		Strikeout = 8
	};
	#endregion

	namespace Text{
		#region GenericFontFamilies
		/// <summary>
		/// Specifies a generic FontFamily object.
		/// </summary>
		public enum GenericFontFamilies {
			/// <summary>
			/// A generic Serif FontFamily object.
			/// </summary>
			Serif,
			/// <summary>
			/// A generic Sans Serif FontFamily object.
			/// </summary>
			SansSerif,
			/// <summary>
			/// A generic Monospace FontFamily object.
			/// </summary>
			Monospace
		}
		#endregion
	}

	#region TextRenderingMode
	/// <summary>
	/// Determines whether showing text causes glyph outlines to be stroked, filled, used as a clipping boundary, 
	/// or some combination of the three.
	/// </summary>
	public enum TextRenderingMode {
		/// <summary>
		/// Fill text.
		/// </summary>
		FillText,
		/// <summary>
		/// Stroke text.
		/// </summary>
		StrokeText,
		/// <summary>
		/// Fill, then stroke text.
		/// </summary>
		FillThenStrokeText,
		/// <summary>
		/// Neither fill nor stroke text (invisible).
		/// </summary>
		Invisible,
		/// <summary>
		/// Fill text and add to path for clipping (see above).
		/// </summary>
		FillTextAndAddForClipping,
		/// <summary>
		/// Stroke text and add to path for clipping.
		/// </summary>
		StrokeTextAndAddForClipping,
		/// <summary>
		/// Fill, then stroke text and add to path for clipping.
		/// </summary>
		FillThenStrokeTextAndAddForClipping,
		/// <summary>
		/// Add text to path for clipping.
		/// </summary>
		AddForClipping
	}
	#endregion

	#region TextState
	/// <summary>
	/// Comprises graphics state parameters that only affect text.
	/// </summary>
	public sealed class TextState {
		#region Declare variables
		private double _characterSpacing = 0;
		private double _wordSpacing = 0;
		private double _horizontalScaling = 100;
		private double _leading = 0;
		private TextRenderingMode _renderingMode;
		#endregion

		#region Properties
		/// <summary>
		/// Specified in unscaled text space units.
		/// </summary>
		public double CharacterSpacing {
			get { return _characterSpacing; }
			set { _characterSpacing = value; }
		}

		/// <summary>
		/// Specified in unscaled word space units.
		/// </summary>
		public double WordSpacing {
			get { return _wordSpacing; }
			set { _wordSpacing = value; }
		}

		/// <summary>
		/// Adjusts the width of glyphs by stretching or compressing them in the horizontal direction.
		/// </summary>
		public double HorizontalScaling {
			get { return _horizontalScaling; }
			set { _horizontalScaling = value; }
		}

		/// <summary>
		/// Measured in unscaled text space units.
		/// </summary>
		public double Leading {
			get { return _leading; }
			set { _leading = value; }
		}

		/// <summary>
		/// Determines whether showing text causes glyph outlines to be stroked, filled, used as a clipping boundary, 
		/// or some combination of the three.
		/// </summary>
		public TextRenderingMode  RenderingMode {
			get { return _renderingMode; }
			set { _renderingMode = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new TextState object using a specified character spacing, 
		/// word spacing, horizontal scaling, leading, rendering mode.
		/// </summary>
		/// <param name="characterSpacing">Specified in unscaled word space units.</param>
		/// <param name="wordSpacing">Specified in unscaled word space units.</param>
		/// <param name="horizontalScaling">Adjusts the width of glyphs by stretching or compressing them in the horizontal direction.</param>
		/// <param name="leading">Measured in unscaled text space units.</param>
		/// <param name="renderingMode">Determines whether showing text causes glyph outlines to be stroked, filled, used as a clipping boundary, or some combination of the three.</param>
		/*/// <param name="rise">Specifies the distance, in unscaled text space units.</param>*/
		public TextState(double characterSpacing, double wordSpacing, double horizontalScaling, double leading, TextRenderingMode renderingMode/*, double rise*/) {
			_characterSpacing = characterSpacing;
			_wordSpacing = wordSpacing;
			_horizontalScaling = horizontalScaling;
			_leading =  leading;
			_renderingMode = renderingMode;
			//_rise =  rise;
		}

		/// <summary>
		/// Initializes a new TextState object using a specified rendering mode.
		/// </summary>
		/// <param name="renderingMode">Determines whether showing text causes glyph outlines to be stroked, filled, used as a clipping boundary, or some combination of the three.</param>
		public TextState(TextRenderingMode renderingMode) {
			_renderingMode = renderingMode;
		}

		public TextState() : this(TextRenderingMode.FillText) {
		}
		#endregion
	};
	#endregion

	internal sealed class FontInfoConvert
	{
		internal static bool TestFlag(FontStyle value, FontStyle testValue)
		{
			return (value & testValue) == testValue;
		}

		internal static bool TestFlag(System.Drawing.FontStyle value,
			System.Drawing.FontStyle testValue)
		{
			return (value & testValue) == testValue;
		}

		public static System.Drawing.FontStyle ToGdiplusFontStyle(FontStyle style)
		{
			return (TestFlag(style, FontStyle.Bold)?
					System.Drawing.FontStyle.Bold: System.Drawing.FontStyle.Regular)
				| (TestFlag(style, FontStyle.Italic)?
					System.Drawing.FontStyle.Italic: System.Drawing.FontStyle.Regular)
				| (TestFlag(style, FontStyle.Strikeout)?
					System.Drawing.FontStyle.Strikeout: System.Drawing.FontStyle.Regular)
				| (TestFlag(style, FontStyle.Underline)?
					System.Drawing.FontStyle.Underline: System.Drawing.FontStyle.Regular);
		}

		public static FontStyle ToFontStyle(System.Drawing.FontStyle style)
		{
			return (TestFlag(style, System.Drawing.FontStyle.Bold)?
					FontStyle.Bold: FontStyle.Regular)
				| (TestFlag(style, System.Drawing.FontStyle.Italic)?
					FontStyle.Italic: FontStyle.Regular)
				| (TestFlag(style, System.Drawing.FontStyle.Strikeout)?
					FontStyle.Strikeout: FontStyle.Regular)
				| (TestFlag(style, System.Drawing.FontStyle.Underline)?
					FontStyle.Underline: FontStyle.Regular);
		}

		public static System.Drawing.Text.GenericFontFamilies ToGdiplusGenericFontFamilies(
			Text.GenericFontFamilies genericFamily)
		{
			switch(genericFamily)
			{
				case Text.GenericFontFamilies.Monospace:
					return System.Drawing.Text.GenericFontFamilies.Monospace;
				case Text.GenericFontFamilies.SansSerif:
					return System.Drawing.Text.GenericFontFamilies.SansSerif;
				case Text.GenericFontFamilies.Serif:
				default:
					return System.Drawing.Text.GenericFontFamilies.Serif;
			}
		}

		public static Text.GenericFontFamilies ToGenericFontFamilies(
			System.Drawing.Text.GenericFontFamilies genericFamily)
		{
			switch(genericFamily)
			{
				case System.Drawing.Text.GenericFontFamilies.Monospace:
					return Text.GenericFontFamilies.Monospace;
				case System.Drawing.Text.GenericFontFamilies.SansSerif:
					return Text.GenericFontFamilies.SansSerif;
				case System.Drawing.Text.GenericFontFamilies.Serif:
				default:
					return Text.GenericFontFamilies.Serif;
			}
		}
	}
}
