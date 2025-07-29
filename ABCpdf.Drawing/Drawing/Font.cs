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
using System.Globalization;

namespace WebSupergoo.ABCpdf13.Drawing {
	public class Font: ICloneable, IDisposable {
		private System.Drawing.Font _gdiplusFont;
		private FontFamily _family;

		#region Properties
		/// <summary>
		/// Gets style information for this font.
		/// </summary>
		/// <value>A font style enumeration that contains style information for this font.</value>
		public virtual FontStyle Style{
			get{ return FontInfoConvert.ToFontStyle(_gdiplusFont.Style); }
		}

		/// <summary>
		/// Gets a value that indicates whether this font is bold.
		/// </summary>
		/// <value><c>true</c> if this font is bold; otherwise, <c>false</c>.</value>
		public virtual bool Bold{ get{ return _gdiplusFont.Bold; } }

		/// <summary>
		/// Gets a value that indicates whether this font is italic.
		/// </summary>
		/// <value><c>true</c> if this font is italic; otherwise, <c>false</c>.</value>
		public virtual bool Italic{ get{ return _gdiplusFont.Italic; } }

		/// <summary>
		/// Gets a value that indicates whether this font is underlined.
		/// </summary>
		/// <value><c>true</c> if this font is underlined; otherwise, <c>false</c>.</value>
		public virtual bool Underline{ get{ return _gdiplusFont.Underline; } }

		/// <summary>
		/// Gets a value that indicates whether this font specifies a horizontal line through the font.
		/// </summary>
		/// <value><c>true</c> if this font has a horizontal line through it; otherwise, <c>false</c>.</value>
		public virtual bool Strikeout{ get{ return _gdiplusFont.Strikeout; } }

		/// <summary>
		/// Gets the face name of this font.
		/// </summary>
		/// <value>A string representation of the face name of this font.</value>
		public virtual string Name{ get{ return _gdiplusFont.Name; } }

		/// <summary>
		/// Gets the em-size, in points, of this font.
		/// </summary>
		/// <value>The em-size, in points, of this font.</value>
		public virtual float SizeInPoints{ get{ return _gdiplusFont.SizeInPoints; } }

		/// <summary>
		/// Gets the em-size, in points, of this font.
		/// </summary>
		/// <value>The em-size, in points, of this font.</value>
		public virtual float Size{ get{ return _gdiplusFont.SizeInPoints; } }

		/// <summary>
		/// Gets the height, in pixels, for this font.
		/// </summary>
		/// <param name="resolution">The resolution at which to calculate the height.</param>
		/// <value>The height in pixels, for this font.</value>
		public virtual float GetHeight(float resolution) { 
			return (_gdiplusFont != null) ? _gdiplusFont.GetHeight(resolution) : (SizeInPoints * resolution) / 72;
		}

		/// <summary>
		/// Gets the font family associated with this font.
		/// </summary>
		/// <value>The font family associated with this font.</value>
		public FontFamily FontFamily{ get{ return _family; } }
		#endregion

		public override string ToString() {
			return string.Format("[Font: Name={0}, Size={1}]", Name, Size);
		}

		#region Constructors
		/// <summary>
		/// Initializes a new font using a specified size.
		/// </summary>
		/// <param name="familyName">A string representation of the font family for the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		public Font(string familyName, float emSize):
			this(familyName, emSize, FontStyle.Regular){}

		/// <summary>
		/// Initializes a new font using a specified size and style.
		/// </summary>
		/// <param name="familyName">A string representation of the font family for the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		/// <param name="style">The font style of the new font.</param>
		public Font(string familyName, float emSize, FontStyle style) {
			_gdiplusFont = new System.Drawing.Font(familyName, emSize,
				FontInfoConvert.ToGdiplusFontStyle(style));
			_family = new FontFamily(_gdiplusFont.FontFamily);
		}

		/// <summary>
		/// Initializes a new font using a specified size.
		/// </summary>
		/// <param name="family">The font family of the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		public Font(FontFamily family, float emSize):
			this(family, emSize, FontStyle.Regular){}

		/// <summary>
		/// Initializes a new font using a specified size and style.
		/// </summary>
		/// <param name="family">The font family of the new font.</param>
		/// <param name="emSize">The em-size, in points, of the new font.</param>
		/// <param name="style">The font style of the new font.</param>
		public Font(FontFamily family, float emSize, FontStyle style) {
			if(family.GetType() != typeof(FontFamily))
				throw new ArgumentException(
					"family must not be a derived class of FontFamily.", "family");
			_family = family;
			_gdiplusFont = new System.Drawing.Font(family.GdiplusFontFamily,
				emSize, FontInfoConvert.ToGdiplusFontStyle(style));
		}

		/// <summary>
		/// Initializes a new font using the specified existing font and font style enumeration.
		/// </summary>
		/// <param name="prototype">The existing font from which to create the new font.</param>
		/// <param name="newStyle">The font style to apply to the new font. Multiple values of the font style enumeration can be combined with the <c>OR</c> operator.</param>
		public Font(Font prototype, FontStyle newStyle):
			this(prototype.FontFamily, prototype.SizeInPoints, newStyle){}

		/// <summary>
		/// Initializes a new font using the specified existing <c>System.Drawing.Font</c>.
		/// </summary>
		/// <param name="prototype">The existing System.Drawing.Font from which to create the new font</param>
		public Font(System.Drawing.Font prototype) {
			_gdiplusFont = (System.Drawing.Font)prototype.Clone();
			_family = new FontFamily(_gdiplusFont.FontFamily);
		}

		internal Font(FontFamily family) {
			_gdiplusFont = null;
			_family = family;
		}
		#endregion

		#region Embedded Font Name
		private XFont GetFontWithBetterWeight(XFont xfont0, XFont xfont1){
			const int boldWeight = 700;
			if(Bold){
				if(xfont0.Weight >= boldWeight && xfont1.Weight >= boldWeight)
					return xfont0.Weight <= xfont1.Weight? xfont0: xfont1;
				if(xfont0.Weight >= boldWeight)
					return xfont0;
				if(xfont1.Weight >= boldWeight)
					return xfont1;
			}

			const int normalWeight = 400;
			return Math.Abs(xfont0.Weight - normalWeight)
				<= Math.Abs(xfont1.Weight - normalWeight)?
			xfont0: xfont1;
		}
		internal virtual string GetEmbeddedFontName(out FontStyle leftoutStyle){
			leftoutStyle = Style;
			const int boldWeight = 700;

			XFont xfont = null;
			XFont[] xfontList = XFont.FindFamily(Name);

			foreach(XFont xfontItor in xfontList){
				if(xfont == null)
					xfont = xfontItor;
				else if(xfontItor.Italic == xfont.Italic)
					xfont = GetFontWithBetterWeight(xfont, xfontItor);
				else if(Bold || (xfontItor.Weight >= boldWeight) == (xfont.Weight >= boldWeight)){
					if(Italic == xfontItor.Italic)
						xfont = xfontItor;
				}else if(Italic == xfontItor.Italic && xfontItor.Weight < boldWeight)
					xfont = xfontItor;	// exact match
				else if(Italic == xfont.Italic && xfont.Weight < boldWeight)
					continue;
				else if(!xfontItor.Italic && xfontItor.Weight < boldWeight)
					xfont = xfontItor;	// regular
				else if(!xfont.Italic && xfont.Weight < boldWeight)
					continue;
				else if(Italic == xfontItor.Italic)
					xfont = xfontItor;
			}

			if(xfont == null)
				return Name;

			if(xfont.Italic)
				leftoutStyle &= ~FontStyle.Italic;
			if(xfont.Weight >= boldWeight)
				leftoutStyle &= ~FontStyle.Bold;
			return xfont.PostScriptName;
		}
		#endregion

		object ICloneable.Clone(){ return Clone(); }
		public Font Clone(){
			Font o = (Font)MemberwiseClone();
			o._gdiplusFont = (System.Drawing.Font)_gdiplusFont.Clone();
			return o;
		}

		public virtual Font Clone(FontStyle style) {
			return new Font(this, style);
		}

		#region IDisposable Members

		private bool _disposed = false;

		~Font(){ Dispose(false); }
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if(_disposed)
				return;

			if(disposing) {
				if(_gdiplusFont != null)
					_gdiplusFont.Dispose();
			}
		}

		#endregion
	}

	public class FontFamily: IDisposable {
		#region Static Properties
		/// <summary>
		/// Gets a generic monospace font family.
		/// </summary>
		/// <value>A font family that represents a generic monospace font.</value>
		public static FontFamily GenericMonospace {
			get{ return new FontFamily(System.Drawing.FontFamily.GenericMonospace); }
		}

		/// <summary>
		/// Gets a generic sans serif font family.
		/// </summary>
		/// <value>A font family that represents a generic sans serif font.</value>
		public static FontFamily GenericSansSerif {
			get{ return new FontFamily(System.Drawing.FontFamily.GenericSansSerif); }
		}

		/// <summary>
		/// Gets a generic serif font family.
		/// </summary>
		/// <value>A font family that represents a generic serif font.</value>
		public static FontFamily GenericSerif {
			get{ return new FontFamily(System.Drawing.FontFamily.GenericSerif); }
		}
		#endregion

		private System.Drawing.FontFamily _gdiplusFamily;

		#region Properties and Simple Info Methods
		internal virtual System.Drawing.FontFamily GdiplusFontFamily {
			get{ return _gdiplusFamily; }
		}

		/// <summary>
		/// Gets the name of this font family.
		/// </summary>
		/// <value>A <c>String</c> that represents the name of this font family.</value>
		public virtual string Name{ get{ return _gdiplusFamily.Name; } }

		/// <summary>
		/// Returns the name, in the specified language, of this font family.
		/// </summary>
		/// <param name="language">The language in which the name is returned.</param>
		/// <returns>A <c>String</c> that represents the name, in the specified language, of this font family.</returns>
		public virtual string GetName(int language) {
			return _gdiplusFamily.GetName(language);
		}

		/// <summary>
		/// Returns the cell ascent, in design units, of the font family of the specified style.
		/// </summary>
		/// <param name="style">A font style that contains style information for the font.</param>
		/// <returns>The cell ascent for this font family that uses the specified font style.</returns>
		public virtual int GetCellAscent(FontStyle style) {
			return _gdiplusFamily.GetCellAscent(
				FontInfoConvert.ToGdiplusFontStyle(style));
		}

		/// <summary>
		/// Returns the cell descent, in design units, of the font family of the specified style.
		/// </summary>
		/// <param name="style">A font style that contains style information for the font.</param>
		/// <returns>The cell descent for this font family that uses the specified font style.</returns>
		public virtual int GetCellDescent(FontStyle style) {
			return _gdiplusFamily.GetCellDescent(
				FontInfoConvert.ToGdiplusFontStyle(style));
		}

		/// <summary>
		/// Gets the height, in design units, of the em square for the specified style.
		/// </summary>
		/// <param name="style">The font style for which to get the em height.</param>
		/// <returns>The height of the em square.</returns>
		public virtual int GetEmHeight(FontStyle style) {
			return _gdiplusFamily.GetEmHeight(
				FontInfoConvert.ToGdiplusFontStyle(style));
		}

		/// <summary>
		/// Returns the line spacing, in design units, of the font family of the specified style. The line spacing is the vertical distance between the base lines of two consecutive lines of text.
		/// </summary>
		/// <param name="style">The font style to apply.</param>
		/// <returns>The distance between two consecutive lines of text.</returns>
		public virtual int GetLineSpacing(FontStyle style) {
			return _gdiplusFamily.GetLineSpacing(
				FontInfoConvert.ToGdiplusFontStyle(style));
		}

		/// <summary>
		/// Indicates whether the specified font style enumeration is available.
		/// </summary>
		/// <param name="style">The font style to test.</param>
		/// <returns><c>true</c> if the specified font style is available; otherwise, <c>false</c>.</returns>
		public virtual bool IsStyleAvailable(FontStyle style) {
			return _gdiplusFamily.IsStyleAvailable(
				FontInfoConvert.ToGdiplusFontStyle(style));
		}
		#endregion

		public override string ToString() {
			return _gdiplusFamily.ToString();
		}

		#region Constructors
		/// <summary>
		/// Initializes a new font family from the specified generic font family.
		/// </summary>
		/// <param name="genericFamily">The generic font family from which to create the new font family.</param>
		public FontFamily(Text.GenericFontFamilies genericFamily) {
			_gdiplusFamily = new System.Drawing.FontFamily(
				FontInfoConvert.ToGdiplusGenericFontFamilies(genericFamily));
		}

		/// <summary>
		/// Initializes a new font family with the specified name.
		/// </summary>
		/// <param name="name">The name of the new font family.</param>
		public FontFamily(string name): this(new System.Drawing.FontFamily(name)){}

		/// <summary>
		/// Initializes a new font family using the specified existing <c>System.Drawing.FontFamily</c>.
		/// </summary>
		/// <param name="prototype">The existing System.Drawing.FontFamily from which to create the new font family.</param>
		public FontFamily(System.Drawing.FontFamily prototype) {
			_gdiplusFamily = prototype;
		}

		internal FontFamily(){}
		#endregion

		#region IDisposable Members

		private bool _disposed = false;

		~FontFamily(){ Dispose(false); }
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if(_disposed)
				return;

			if(disposing) {
				if(_gdiplusFamily != null)
					_gdiplusFamily.Dispose();
			}
		}

		#endregion
	}

}
