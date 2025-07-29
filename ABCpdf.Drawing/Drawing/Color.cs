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
	#region Color
	/// <summary>
	/// A color in RGB, CMYK or Grayscale.
	/// </summary>
	public class Color {
		#region Declare variables
		internal double a = 1;
		internal double r = 0;
		internal double g = 0;
		internal double b = 0;
		internal double c = 0;
		internal double m = 0;
		internal double y = 0;
		internal double k = 0;
		internal double gray = 0;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the alpha component.
		/// </summary>
		public double A {
			get { return a; }
			set { a = value; }
		} 
		
		/// <summary>
		/// Gets or sets the red component.
		/// </summary>
		public double R {
			get { return r; }
			set { r = value; }
		} 

		/// <summary>
		/// Gets or sets the green component.
		/// </summary>
		public double G {
			get { return g; }
			set { g = value; }
		} 

		/// <summary>
		/// Gets or sets the blue component.
		/// </summary>
		public double B {
			get { return b; }
			set { b = value; }
		} 

		/// <summary>
		/// Gets or sets the cyan component.
		/// </summary>
		public double C {
			get { return c; }
			set { c = value; }
		} 

		/// <summary>
		/// Gets or sets the magenta component.
		/// </summary>
		public double M {
			get { return m; }
			set { m = value; }
		} 

		/// <summary>
		/// Gets or sets the yellow component.
		/// </summary>
		public double Y {
			get { return y; }
			set { y = value; }
		} 

		/// <summary>
		/// Gets or sets the black component.
		/// </summary>
		public double K {
			get { return k; }
			set { k = value; }
		} 

		/// <summary>
		/// Gets or sets the gray level.
		/// </summary>
		public double GrayScale {
			get { return gray; }
			set { gray = value; }
		} 
		#endregion

		public override bool Equals(object obj) {
			Color c = obj as Color;
			if (c != null)
				return (A == c.A) && (R == c.R) && (G == c.G) && (B == c.B);
			else
				return false;
		}

		public override int GetHashCode() {
			return base.GetHashCode ();
		}

		#region Static methods
		/// <summary>
		/// Creates a Color from the specified 8-bit color values 
		/// (red, green, and blue). The alpha value is implicitly 255 (fully opaque). 
		/// </summary>
		/// <param name="red">The red component value. Valid values are 0 through 255.</param>
		/// <param name="green">The green component value. Valid values are 0 through 255.</param>
		/// <param name="blue">The blue component value. Valid values are 0 through 255.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromArgb(int red, int green, int blue) {
			return FromArgb(255, red, green, blue);
		}
		
		/// <summary>
		/// Creates a Color from a 32-bit ARGB value.
		/// </summary>
		/// <param name="argb">A value specifying the 32-bit ARGB value.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromArgb(int argb) {
			System.Drawing.Color clr = System.Drawing.Color.FromArgb(argb);
			return FromArgb(clr.A, clr.R, clr.G, clr.B);
		}

		/// <summary>
		/// Creates a Color from the specified pre-defined color.
		/// </summary>
		/// <param name="color">An element of the System.Drawing.KnownColor enumeration.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromKnownColor(System.Drawing.KnownColor color) {
			System.Drawing.Color sysColor = System.Drawing.Color.FromKnownColor(color);
			return FromArgb(sysColor.A, sysColor.R, sysColor.G, sysColor.B);
		}

		/// <summary>
		/// Creates a Color from the specified System.Drawing.Color structure, 
		/// but with the new specified alpha value. 
		/// </summary>
		/// <param name="alpha">The alpha value for the new System.Drawing.Color structure. Valid values are 0 through 255.</param>
		/// <param name="baseColor">The System.Drawing.Color structure from which to create the new System.Drawing.Color structure.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromArgb(int alpha, System.Drawing.Color baseColor) {
			return FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);
		}
		
		/// <summary>
		/// Creates a Color from the specified 8-bit RGB color values 
		/// (alpha, red, green, and blue). 
		/// </summary>
		/// <param name="alpha">The alpha component value. Valid values are 0 through 255.</param>
		/// <param name="red">The red component value. Valid values are 0 through 255.</param>
		/// <param name="green">The green component value. Valid values are 0 through 255.</param>
		/// <param name="blue">The blue component value. Valid values are 0 through 255.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromArgb(int alpha, int red, int green, int blue) {
			Color clr = new Color();
			clr.a = alpha / 255F;
			clr.r = red / 255F;
			clr.g = green / 255F;
			clr.b = blue / 255F;
			return clr;
		}

		/// <summary>
		/// Creates a Color from the specified ARGB color values 
		/// (alpha, red, green, and blue). 
		/// </summary>
		/// <param name="cyan">The alpha component. Valid values are 0 through 1.</param>
		/// <param name="magenta">The red component. Valid values are 0 through 1.</param>
		/// <param name="yellow">The green component. Valid values are 0 through 1.</param>
		/// <param name="black">The blue component. Valid values are 0 through 1.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromArgb(double alpha, double red, double green, double blue) {
			Color clr = new Color();
			clr.a = alpha;
			clr.r = red;
			clr.g = green;
			clr.b = blue;
			return clr;
		}

		/// <summary>
		/// Creates a Color from the specified CMYK color values 
		/// (cyan, magenta, yellow, and black). 
		/// </summary>
		/// <param name="cyan">The cyan component. Valid values are 0 through 1.</param>
		/// <param name="magenta">The magenta component. Valid values are 0 through 1.</param>
		/// <param name="yellow">The yellow component. Valid values are 0 through 1.</param>
		/// <param name="black">The black component. Valid values are 0 through 1.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromCmyk(double cyan, double magenta, double yellow, double black) {
			Color clr = new Color();
			clr.c = cyan;
			clr.m = magenta;
			clr.y = yellow;
			clr.k = black;
			return clr;
		}

		/// <summary>
		/// Creates a Color from the specified gray level.
		/// </summary>
		/// <param name="gray">The gray component. Valid values are 0 through 1.</param>
		/// <returns>The Color that this method creates.</returns>
		public static Color FromCmyk(double gray) {
			Color clr = new Color();
			clr.gray = gray;
			return clr;
		}

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color AliceBlue { get { return FromKnownColor(System.Drawing.KnownColor.AliceBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color AntiqueWhite { get { return FromKnownColor(System.Drawing.KnownColor.AntiqueWhite); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color AppWorkspace { get { return FromKnownColor(System.Drawing.KnownColor.AppWorkspace); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Aqua { get { return FromKnownColor(System.Drawing.KnownColor.Aqua); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Aquamarine { get { return FromKnownColor(System.Drawing.KnownColor.Aquamarine); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Azure { get { return FromKnownColor(System.Drawing.KnownColor.Azure); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Beige { get { return FromKnownColor(System.Drawing.KnownColor.Beige); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Bisque { get { return FromKnownColor(System.Drawing.KnownColor.Bisque); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Black { get { return FromKnownColor(System.Drawing.KnownColor.Black); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color BlanchedAlmond { get { return FromKnownColor(System.Drawing.KnownColor.BlanchedAlmond); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Blue { get { return FromKnownColor(System.Drawing.KnownColor.Blue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color BlueViolet { get { return FromKnownColor(System.Drawing.KnownColor.BlueViolet); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Brown { get { return FromKnownColor(System.Drawing.KnownColor.Brown); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color BurlyWood { get { return FromKnownColor(System.Drawing.KnownColor.BurlyWood); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color CadetBlue { get { return FromKnownColor(System.Drawing.KnownColor.CadetBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Chartreuse { get { return FromKnownColor(System.Drawing.KnownColor.Chartreuse); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Chocolate { get { return FromKnownColor(System.Drawing.KnownColor.Chocolate); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Control { get { return FromKnownColor(System.Drawing.KnownColor.Control); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ControlDark { get { return FromKnownColor(System.Drawing.KnownColor.ControlDark); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ControlDarkDark { get { return FromKnownColor(System.Drawing.KnownColor.ControlDarkDark); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ControlLight { get { return FromKnownColor(System.Drawing.KnownColor.ControlLight); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ControlLightLight { get { return FromKnownColor(System.Drawing.KnownColor.ControlLightLight); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ControlText { get { return FromKnownColor(System.Drawing.KnownColor.ControlText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Coral { get { return FromKnownColor(System.Drawing.KnownColor.Coral); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color CornflowerBlue  { get { return FromKnownColor(System.Drawing.KnownColor.CornflowerBlue ); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Cornsilk { get { return FromKnownColor(System.Drawing.KnownColor.Cornsilk); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Crimson { get { return FromKnownColor(System.Drawing.KnownColor.Crimson); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Cyan { get { return FromKnownColor(System.Drawing.KnownColor.Cyan); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkBlue { get { return FromKnownColor(System.Drawing.KnownColor.DarkBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkCyan { get { return FromKnownColor(System.Drawing.KnownColor.DarkCyan); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkGoldenrod { get { return FromKnownColor(System.Drawing.KnownColor.DarkGoldenrod); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkGray { get { return FromKnownColor(System.Drawing.KnownColor.DarkGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkGreen { get { return FromKnownColor(System.Drawing.KnownColor.DarkGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkKhaki { get { return FromKnownColor(System.Drawing.KnownColor.DarkKhaki); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkMagenta { get { return FromKnownColor(System.Drawing.KnownColor.DarkMagenta); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkOliveGreen { get { return FromKnownColor(System.Drawing.KnownColor.DarkOliveGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkOrange { get { return FromKnownColor(System.Drawing.KnownColor.DarkOrange); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkOrchid { get { return FromKnownColor(System.Drawing.KnownColor.DarkOrchid); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkRed { get { return FromKnownColor(System.Drawing.KnownColor.DarkRed); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkSalmon { get { return FromKnownColor(System.Drawing.KnownColor.DarkSalmon); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkSeaGreen { get { return FromKnownColor(System.Drawing.KnownColor.DarkSeaGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkSlateBlue { get { return FromKnownColor(System.Drawing.KnownColor.DarkSlateBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkSlateGray { get { return FromKnownColor(System.Drawing.KnownColor.DarkSlateGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkTurquoise { get { return FromKnownColor(System.Drawing.KnownColor.DarkTurquoise); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DarkViolet { get { return FromKnownColor(System.Drawing.KnownColor.DarkViolet); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DeepPink { get { return FromKnownColor(System.Drawing.KnownColor.DeepPink); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DeepSkyBlue { get { return FromKnownColor(System.Drawing.KnownColor.DeepSkyBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Desktop { get { return FromKnownColor(System.Drawing.KnownColor.Desktop); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DimGray { get { return FromKnownColor(System.Drawing.KnownColor.DimGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color DodgerBlue { get { return FromKnownColor(System.Drawing.KnownColor.DodgerBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Firebrick { get { return FromKnownColor(System.Drawing.KnownColor.Firebrick); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color FloralWhite { get { return FromKnownColor(System.Drawing.KnownColor.FloralWhite); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ForestGreen { get { return FromKnownColor(System.Drawing.KnownColor.ForestGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Fuchsia { get { return FromKnownColor(System.Drawing.KnownColor.Fuchsia); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Gainsboro { get { return FromKnownColor(System.Drawing.KnownColor.Gainsboro); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color GhostWhite { get { return FromKnownColor(System.Drawing.KnownColor.GhostWhite); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Gold { get { return FromKnownColor(System.Drawing.KnownColor.Gold); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Goldenrod { get { return FromKnownColor(System.Drawing.KnownColor.Goldenrod); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Gray { get { return FromKnownColor(System.Drawing.KnownColor.Gray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color GrayText { get { return FromKnownColor(System.Drawing.KnownColor.GrayText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Green { get { return FromKnownColor(System.Drawing.KnownColor.Green); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color GreenYellow { get { return FromKnownColor(System.Drawing.KnownColor.GreenYellow); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Highlight { get { return FromKnownColor(System.Drawing.KnownColor.Highlight); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color HighlightText { get { return FromKnownColor(System.Drawing.KnownColor.HighlightText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Honeydew { get { return FromKnownColor(System.Drawing.KnownColor.Honeydew); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color HotPink { get { return FromKnownColor(System.Drawing.KnownColor.HotPink); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color HotTrack { get { return FromKnownColor(System.Drawing.KnownColor.HotTrack); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color InactiveBorder { get { return FromKnownColor(System.Drawing.KnownColor.InactiveBorder); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color InactiveCaption { get { return FromKnownColor(System.Drawing.KnownColor.InactiveCaption); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color InactiveCaptionText { get { return FromKnownColor(System.Drawing.KnownColor.InactiveCaptionText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color IndianRed { get { return FromKnownColor(System.Drawing.KnownColor.IndianRed); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Indigo { get { return FromKnownColor(System.Drawing.KnownColor.Indigo); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Info { get { return FromKnownColor(System.Drawing.KnownColor.Info); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color InfoText { get { return FromKnownColor(System.Drawing.KnownColor.InfoText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Ivory { get { return FromKnownColor(System.Drawing.KnownColor.Ivory); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Khaki { get { return FromKnownColor(System.Drawing.KnownColor.Khaki); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Lavender { get { return FromKnownColor(System.Drawing.KnownColor.Lavender); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LavenderBlush { get { return FromKnownColor(System.Drawing.KnownColor.LavenderBlush); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LawnGreen { get { return FromKnownColor(System.Drawing.KnownColor.LawnGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LemonChiffon { get { return FromKnownColor(System.Drawing.KnownColor.LemonChiffon); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightBlue { get { return FromKnownColor(System.Drawing.KnownColor.LightBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightCoral { get { return FromKnownColor(System.Drawing.KnownColor.LightCoral); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightCyan { get { return FromKnownColor(System.Drawing.KnownColor.LightCyan); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightGoldenrodYellow { get { return FromKnownColor(System.Drawing.KnownColor.LightGoldenrodYellow); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightGray { get { return FromKnownColor(System.Drawing.KnownColor.LightGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightGreen { get { return FromKnownColor(System.Drawing.KnownColor.LightGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightPink { get { return FromKnownColor(System.Drawing.KnownColor.LightPink); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightSalmon { get { return FromKnownColor(System.Drawing.KnownColor.LightSalmon); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightSeaGreen { get { return FromKnownColor(System.Drawing.KnownColor.LightSeaGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightSkyBlue { get { return FromKnownColor(System.Drawing.KnownColor.LightSkyBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightSlateGray { get { return FromKnownColor(System.Drawing.KnownColor.LightSlateGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightSteelBlue { get { return FromKnownColor(System.Drawing.KnownColor.LightSteelBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LightYellow { get { return FromKnownColor(System.Drawing.KnownColor.LightYellow); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Lime { get { return FromKnownColor(System.Drawing.KnownColor.Lime); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color LimeGreen { get { return FromKnownColor(System.Drawing.KnownColor.LimeGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Linen { get { return FromKnownColor(System.Drawing.KnownColor.Linen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Magenta { get { return FromKnownColor(System.Drawing.KnownColor.Magenta); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Maroon { get { return FromKnownColor(System.Drawing.KnownColor.Maroon); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumAquamarine { get { return FromKnownColor(System.Drawing.KnownColor.MediumAquamarine); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumBlue { get { return FromKnownColor(System.Drawing.KnownColor.MediumBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumOrchid { get { return FromKnownColor(System.Drawing.KnownColor.MediumOrchid); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumPurple { get { return FromKnownColor(System.Drawing.KnownColor.MediumPurple); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumSeaGreen { get { return FromKnownColor(System.Drawing.KnownColor.MediumSeaGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumSlateBlue { get { return FromKnownColor(System.Drawing.KnownColor.MediumSlateBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumSpringGreen { get { return FromKnownColor(System.Drawing.KnownColor.MediumSpringGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumTurquoise { get { return FromKnownColor(System.Drawing.KnownColor.MediumTurquoise); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MediumVioletRed { get { return FromKnownColor(System.Drawing.KnownColor.MediumVioletRed); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Menu { get { return FromKnownColor(System.Drawing.KnownColor.Menu); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MenuText { get { return FromKnownColor(System.Drawing.KnownColor.MenuText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MidnightBlue { get { return FromKnownColor(System.Drawing.KnownColor.MidnightBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MintCream { get { return FromKnownColor(System.Drawing.KnownColor.MintCream); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color MistyRose { get { return FromKnownColor(System.Drawing.KnownColor.MistyRose); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Moccasin { get { return FromKnownColor(System.Drawing.KnownColor.Moccasin); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color NavajoWhite { get { return FromKnownColor(System.Drawing.KnownColor.NavajoWhite); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Navy { get { return FromKnownColor(System.Drawing.KnownColor.Navy); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color OldLace { get { return FromKnownColor(System.Drawing.KnownColor.OldLace); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Olive { get { return FromKnownColor(System.Drawing.KnownColor.Olive); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color OliveDrab { get { return FromKnownColor(System.Drawing.KnownColor.OliveDrab); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Orange { get { return FromKnownColor(System.Drawing.KnownColor.Orange); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color OrangeRed { get { return FromKnownColor(System.Drawing.KnownColor.OrangeRed); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Orchid { get { return FromKnownColor(System.Drawing.KnownColor.Orchid); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PaleGoldenrod { get { return FromKnownColor(System.Drawing.KnownColor.PaleGoldenrod); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PaleGreen { get { return FromKnownColor(System.Drawing.KnownColor.PaleGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PaleTurquoise { get { return FromKnownColor(System.Drawing.KnownColor.PaleTurquoise); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PaleVioletRed { get { return FromKnownColor(System.Drawing.KnownColor.PaleVioletRed); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PapayaWhip { get { return FromKnownColor(System.Drawing.KnownColor.PapayaWhip); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PeachPuff { get { return FromKnownColor(System.Drawing.KnownColor.PeachPuff); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Peru { get { return FromKnownColor(System.Drawing.KnownColor.Peru); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Pink { get { return FromKnownColor(System.Drawing.KnownColor.Pink); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Plum { get { return FromKnownColor(System.Drawing.KnownColor.Plum); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color PowderBlue { get { return FromKnownColor(System.Drawing.KnownColor.PowderBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Purple { get { return FromKnownColor(System.Drawing.KnownColor.Purple); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Red { get { return FromKnownColor(System.Drawing.KnownColor.Red); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color RosyBrown { get { return FromKnownColor(System.Drawing.KnownColor.RosyBrown); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color RoyalBlue { get { return FromKnownColor(System.Drawing.KnownColor.RoyalBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SaddleBrown { get { return FromKnownColor(System.Drawing.KnownColor.SaddleBrown); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Salmon { get { return FromKnownColor(System.Drawing.KnownColor.Salmon); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SandyBrown { get { return FromKnownColor(System.Drawing.KnownColor.SandyBrown); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color ScrollBar { get { return FromKnownColor(System.Drawing.KnownColor.ScrollBar); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SeaGreen { get { return FromKnownColor(System.Drawing.KnownColor.SeaGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SeaShell { get { return FromKnownColor(System.Drawing.KnownColor.SeaShell); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Sienna { get { return FromKnownColor(System.Drawing.KnownColor.Sienna); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Silver { get { return FromKnownColor(System.Drawing.KnownColor.Silver); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SkyBlue { get { return FromKnownColor(System.Drawing.KnownColor.SkyBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SlateBlue { get { return FromKnownColor(System.Drawing.KnownColor.SlateBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SlateGray { get { return FromKnownColor(System.Drawing.KnownColor.SlateGray); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Snow { get { return FromKnownColor(System.Drawing.KnownColor.Snow); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SpringGreen { get { return FromKnownColor(System.Drawing.KnownColor.SpringGreen); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color SteelBlue { get { return FromKnownColor(System.Drawing.KnownColor.SteelBlue); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Tan { get { return FromKnownColor(System.Drawing.KnownColor.Tan); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Teal { get { return FromKnownColor(System.Drawing.KnownColor.Teal); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Thistle { get { return FromKnownColor(System.Drawing.KnownColor.Thistle); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Tomato { get { return FromKnownColor(System.Drawing.KnownColor.Tomato); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Transparent { get { return FromKnownColor(System.Drawing.KnownColor.Transparent); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Turquoise { get { return FromKnownColor(System.Drawing.KnownColor.Turquoise); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Violet { get { return FromKnownColor(System.Drawing.KnownColor.Violet); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Wheat { get { return FromKnownColor(System.Drawing.KnownColor.Wheat); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color White { get { return FromKnownColor(System.Drawing.KnownColor.White); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color WhiteSmoke { get { return FromKnownColor(System.Drawing.KnownColor.WhiteSmoke); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Window { get { return FromKnownColor(System.Drawing.KnownColor.Window); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color WindowFrame { get { return FromKnownColor(System.Drawing.KnownColor.WindowFrame); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color WindowText { get { return FromKnownColor(System.Drawing.KnownColor.WindowText); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color Yellow { get { return FromKnownColor(System.Drawing.KnownColor.Yellow); } }

		/// <summary>
		/// Gets a system defined color
		/// </summary>
		public static Color YellowGreen { get { return FromKnownColor(System.Drawing.KnownColor.YellowGreen); } }

		#endregion
	}
	#endregion
}
