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
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace WebSupergoo.ABCpdf13.Drawing {

	public class BuiltInFont: Font {
		private FontStyle _style;
		private float _emSize;

		#region Properties
		/// <summary>
		/// Gets style information.
		/// </summary>
		/// <value>An enumeration that contains style information for this font.</value>
		public override FontStyle Style {
			get{ return _style; }
		}

		/// <summary>
		/// Gets a value that indicates whether this font is bold.
		/// </summary>
		/// <value>true if this font is bold; otherwise, false.</value>
		public override bool Bold{
			get{ return FontInfoConvert.TestFlag(_style, FontStyle.Bold); }
		}

		/// <summary>
		/// Gets a value that indicates whether this font is italic.
		/// </summary>
		/// <value>true if this font is italic; otherwise, false.</value>
		public override bool Italic{
			get{ return FontInfoConvert.TestFlag(_style, FontStyle.Italic); }
		}

		/// <summary>
		/// Gets a value that indicates whether this font is underlined.
		/// </summary>
		/// <value>true if this font is underlined; otherwise, false.</value>
		public override bool Underline{
			get{ return FontInfoConvert.TestFlag(_style, FontStyle.Underline); }
		}

		/// <summary>
		/// Gets a value that indicates whether this font specifies a horizontal line through the font.
		/// </summary>
		/// <value>true if this font has a horizontal line through it; otherwise, false.</value>
		public override bool Strikeout{
			get{ return FontInfoConvert.TestFlag(_style, FontStyle.Strikeout); }
		}

		/// <summary>
		/// Gets the face name of this font.
		/// </summary>
		/// <value>A string representation of the face name of this font.</value>
		public override string Name{ get{ return FontFamily.Name; } }

		/// <summary>
		/// Gets the em-size, in points, of this font.
		/// </summary>
		/// <value>The em-size, in points, of this font.</value>
		public override float SizeInPoints{ get{ return _emSize; } }

		/// <summary>
		/// Gets the em-size, in points, of this font.
		/// </summary>
		/// <value>The em-size, in points, of this font.</value>
		public override float Size{ get{ return _emSize; } }

		/// <summary>
		/// Gets the font family associated with this font.
		/// </summary>
		/// <value>The font family associated with this font.</value>
		public new BuiltInFontFamily FontFamily{
			get{ return (BuiltInFontFamily)base.FontFamily; }
		}

		internal string BuiltInName {
			get{ return FontFamily.GetBuiltInName(_style); }
		}
		internal Text.BuiltInFontMetrics FontMetrics {
			get{ return FontFamily.GetFontMetrics(_style); }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new font using a specified size.
		/// </summary>
		/// <param name="familyName">A string representation of the font family for the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		public BuiltInFont(string familyName, float emSize):
			this(familyName, emSize, FontStyle.Regular){}

		/// <summary>
		/// Initializes a new font using a specified size and style.
		/// </summary>
		/// <param name="familyName">A string representation of the font family for the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		/// <param name="style">The style of the new font.</param>
		public BuiltInFont(string familyName, float emSize, FontStyle style):
			this(BuiltInFontFamily.FromName(familyName), emSize, style){}

		/// <summary>
		/// Initializes a new font using a specified size.
		/// </summary>
		/// <param name="family">The font family of the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		public BuiltInFont(BuiltInFontFamily family, float emSize):
			this(family, emSize, FontStyle.Regular){}

		/// <summary>
		/// Initializes a new font using a specified size and style.
		/// </summary>
		/// <param name="family">The font family of the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		/// <param name="style">The style of the new font.</param>
		public BuiltInFont(BuiltInFontFamily family, float emSize, FontStyle style):
			base(family) {
			_style = style;
			_emSize = emSize;
		}

		/// <summary>
		/// Initializes a new font that uses the specified existing font and style enumeration.
		/// </summary>
		/// <param name="prototype">The existing font from which to create the new font</param>
		/// <param name="newStyle">The style to apply to the new font. Multiple values of the style enumeration can be combined with the OR operator.</param>
		public BuiltInFont(BuiltInFont font, FontStyle style):
			this(font.FontFamily, font.SizeInPoints, style){}
		#endregion

		internal override string GetEmbeddedFontName(out FontStyle leftoutStyle){
			leftoutStyle = _style;
			return FontFamily.GetEmbeddedFontName(ref leftoutStyle);
		}

		public new BuiltInFont Clone() {
			return (BuiltInFont)base.Clone();
		}

		public override Font Clone(FontStyle style) {
			return new BuiltInFont(this, style);
		}
	}


	public abstract class BuiltInFontFamily: FontFamily {
		#region Static Properties
		internal static BuiltInFontFamily TimesRomanFontFamily {
			get{ return TimesRomanBuiltInFontFamily.Instance; }
		}
		internal static BuiltInFontFamily HelveticaFontFamily {
			get{ return HelveticaBuiltInFontFamily.Instance; }
		}
		internal static BuiltInFontFamily CourierFontFamily {
			get{ return CourierBuiltInFontFamily.Instance; }
		}
		internal static BuiltInFontFamily SymbolFontFamily {
			get{ return SymbolBuiltInFontFamily.Instance; }
		}
		internal static BuiltInFontFamily ZapfDingbatsFontFamily {
			get{ return ZapfDingbatsBuiltInFontFamily.Instance; }
		}

		/// <summary>
		/// Gets a generic monospace font family.
		/// </summary>
		/// <value>A font family that represents a generic monospace font.</value>
		public static new BuiltInFontFamily GenericMonospace {
			get{ return CourierFontFamily; }
		}

		/// <summary>
		/// Gets a generic sans serif font family.
		/// </summary>
		/// <value>A font family that represents a generic sans serif font.</value>
		public static new BuiltInFontFamily GenericSansSerif {
			get{ return HelveticaFontFamily; }
		}

		/// <summary>
		/// Gets a generic serif font family.
		/// </summary>
		/// <value>A font family that represents a generic serif font.</value>
		public static new BuiltInFontFamily GenericSerif {
			get{ return TimesRomanFontFamily; }
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Gets the font family for the specified generic font family.
		/// </summary>
		/// <param name="genericFamily">The generic font family for which to get the font family.</param>
		/// <returns>The font family for the specified generic font family.</returns>
		public static BuiltInFontFamily FromGenericFontFamilies(
			Text.GenericFontFamilies genericFamily) {
			switch(genericFamily) {
				case Text.GenericFontFamilies.Monospace:
					return GenericMonospace;
				case Text.GenericFontFamilies.SansSerif:
					return GenericSansSerif;
				case Text.GenericFontFamilies.Serif:
				default:
					return GenericSerif;
			}
		}

		/// <summary>
		/// Gets the font family with the specified name.
		/// </summary>
		/// <param name="name">The name of the font family to get.</param>
		/// <returns>The font family with the specified name.</returns>
		public static BuiltInFontFamily FromName(string name) {
			BuiltInFontFamily family
				= name == TimesRomanFontFamily.Name? TimesRomanFontFamily:
				name == HelveticaFontFamily.Name? HelveticaFontFamily:
				name == CourierFontFamily.Name? CourierFontFamily:
				name == SymbolFontFamily.Name? SymbolFontFamily:
				name == ZapfDingbatsFontFamily.Name? ZapfDingbatsFontFamily:
				null;
			if(family == null)
				throw new ArgumentException("Unknown BuiltInFontFamily "+name+".", "name");
			return family;
		}
		#endregion

		private string _name;

		#region Properties and Simple Info Methods

		internal override System.Drawing.FontFamily GdiplusFontFamily {
			get {
				throw new NotSupportedException(
					"BuiltInFontFamily does not support property GdiplusFontFamily.");
			}
		}

		/// <summary>
		/// Gets the name of this font family.
		/// </summary>
		/// <value>A String that represents the name of this font family.</value>
		public override string Name{ get{ return _name; } }

		/// <summary>
		/// Returns the name, in the specified language, of this font family.
		/// </summary>
		/// <param name="language">The language in which the name is returned.</param>
		/// <returns>A String that represents the name, in the specified language, of this font family.</returns>
		public override string GetName(int language){ return _name; }
		internal abstract string GetBuiltInName(FontStyle style);
		internal Text.BuiltInFontMetrics GetFontMetrics(FontStyle style) {
			string name = string.Format("WebSupergoo.ABCpdf13.Drawing.Core14_AFMs.{0}.afm", GetBuiltInName(style));
			Stream stream = GetType().Assembly.GetManifestResourceStream(name);
			Debug.Assert(stream != null);
			return new Text.BuiltInFontMetrics(stream);
		}

		/// <summary>
		/// Returns the cell ascent, in design units, of the font family of the specified style.
		/// </summary>
		/// <param name="style">A style that contains style information for the font.</param>
		/// <returns>The cell ascent for this font family that uses the specified style.</returns>
		public override int GetCellAscent(FontStyle style) {
			double v = GetFontMetrics(style).Ascender;
			return double.IsNaN(v)? 0: (int)v + 150;
		}

		/// <summary>
		/// Returns the cell descent, in design units, of the font family of the specified style.
		/// </summary>
		/// <param name="style">A style that contains style information for the font.</param>
		/// <returns>The cell descent for this font family that uses the specified style.</returns>
		public override int GetCellDescent(FontStyle style) {
			double v = GetFontMetrics(style).Descender;
			return double.IsNaN(v)? 0: -(int)v;
		}

		/// <summary>
		/// Gets the height, in design units, of the em square for the specified style.
		/// </summary>
		/// <param name="style">The style for which to get the em height.</param>
		/// <returns>The height of the em square.</returns>
		public override int GetEmHeight(FontStyle style) {
			double v = GetFontMetrics(style).CapHeight;
			return double.IsNaN(v)? 0: 1000;
		}

		/// <summary>
		/// Returns the line spacing, in design units, of the font family of the specified style. The line spacing is the vertical distance between the base lines of two consecutive lines of text.
		/// </summary>
		/// <param name="style">The style to apply.</param>
		/// <returns>The distance between two consecutive lines of text.</returns>
		public override int GetLineSpacing(FontStyle style) {
			return  GetCellAscent(style) + GetCellDescent(style);
		}

		/// <summary>
		/// Indicates whether the specified style enumeration is available.
		/// </summary>
		/// <param name="style">The style to test.</param>
		/// <returns>true if the specified style is available; otherwise, false.</returns>
		public override bool IsStyleAvailable(FontStyle style) { return true; }
		#endregion

		public override string ToString() {
			return string.Format("[BuiltInFontFamily: Name={0}]", Name);
		}

		internal BuiltInFontFamily(string name) {
			_name = name;
		}

		internal abstract string GetEmbeddedFontName(ref FontStyle style);
	}

	#region Intermediate Built-in Font Family Classes
	internal abstract class RegularBuiltInFontFamily: BuiltInFontFamily {
		private string _regularBuiltInName;

		internal override string GetBuiltInName(FontStyle style) {
			return _regularBuiltInName;
		}


		public RegularBuiltInFontFamily(string name, string regularBuiltInName):
			base(name) {
			_regularBuiltInName = regularBuiltInName;
		}

		internal override string GetEmbeddedFontName(ref FontStyle style){
			return GetBuiltInName(style);
		}
	}

	internal abstract class BoldItalicBuiltInFontFamily: RegularBuiltInFontFamily {
		private string _boldBuiltInName;
		private string _italicBuiltInName;
		private string _boldItalicBuiltInName;

		internal override string GetBuiltInName(FontStyle style) {
			return 
				FontInfoConvert.TestFlag(style, FontStyle.Bold | FontStyle.Italic)?
			_boldItalicBuiltInName:
				FontInfoConvert.TestFlag(style, FontStyle.Bold)? _boldBuiltInName:
				FontInfoConvert.TestFlag(style, FontStyle.Italic)? _italicBuiltInName:
				base.GetBuiltInName(style);
		}


		public BoldItalicBuiltInFontFamily(string name, string regularBuiltInName,
			string boldBuiltInName, string italicBuiltInName, string boldItalicBuiltInName):
			base(name, regularBuiltInName) {
			_boldBuiltInName = boldBuiltInName;
			_italicBuiltInName = italicBuiltInName;
			_boldItalicBuiltInName = boldItalicBuiltInName;
		}

		internal override string GetEmbeddedFontName(ref FontStyle style){
			string name = GetBuiltInName(style);
			style &= ~(FontStyle.Bold | FontStyle.Italic);
			return name;
		}
	}
	#endregion

	#region Concrete Font Family Classes
	internal sealed class TimesRomanBuiltInFontFamily: BoldItalicBuiltInFontFamily {
		public static readonly TimesRomanBuiltInFontFamily Instance
			= new TimesRomanBuiltInFontFamily();

		private TimesRomanBuiltInFontFamily(): base("Times Roman", "Times-Roman",
			"Times-Bold", "Times-Italic", "Times-BoldItalic"){}
	}

	internal sealed class HelveticaBuiltInFontFamily: BoldItalicBuiltInFontFamily {
		public static readonly HelveticaBuiltInFontFamily Instance
			= new HelveticaBuiltInFontFamily();

		private HelveticaBuiltInFontFamily(): base("Helvetica", "Helvetica",
			"Helvetica-Bold", "Helvetica-Oblique", "Helvetica-BoldOblique"){}
	}

	internal sealed class CourierBuiltInFontFamily: BoldItalicBuiltInFontFamily {
		public static readonly CourierBuiltInFontFamily Instance
			= new CourierBuiltInFontFamily();

		private CourierBuiltInFontFamily(): base("Courier", "Courier",
			"Courier-Bold", "Courier-Oblique", "Courier-BoldOblique"){}
	}

	internal sealed class SymbolBuiltInFontFamily: RegularBuiltInFontFamily {
		public static readonly SymbolBuiltInFontFamily Instance
			= new SymbolBuiltInFontFamily();

		private SymbolBuiltInFontFamily(): base("Symbol", "Symbol"){}
	}

	internal sealed class ZapfDingbatsBuiltInFontFamily: RegularBuiltInFontFamily {
		public static readonly ZapfDingbatsBuiltInFontFamily Instance
			= new ZapfDingbatsBuiltInFontFamily();

		private ZapfDingbatsBuiltInFontFamily():
			base("Zapf Dingbats", "ZapfDingbats"){}
	}
	#endregion
}
