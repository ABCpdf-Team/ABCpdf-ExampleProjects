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

namespace WebSupergoo.ABCpdf13.Drawing.Text
{
	#region FontMetrics
	internal abstract class FontMetrics
	{
		public string FontMetricsVersion = string.Empty;
		public int MetricsSets = 0;
		public string FontName = string.Empty;
		public string FullName = string.Empty;
		public string FamilyName = string.Empty;
		public string Weight = string.Empty;
		public double FontBBox_llx = double.NaN;
		public double FontBBox_lly = double.NaN;
		public double FontBBox_urx = double.NaN;
		public double FontBBox_ury = double.NaN;
		public string Version = string.Empty;
		public string Notice = string.Empty;
		public string EncodingScheme = string.Empty;
		public string CharacterSet = string.Empty;
		public bool IsBaseFont = true;
		public double CapHeight = double.NaN;
		public double XHeight = double.NaN;
		public double Ascender = double.NaN;
		public double Descender = double.NaN;
		public double StdHW = double.NaN;
		public double StdVW = double.NaN;
		public double UnderlinePosition = double.NaN;
		public double UnderlineThickness = double.NaN;
		public double ItalicAngle = double.NaN;
		public bool IsFixedPitch = false;
		public int CharMetricsCount = 0;
		public CharMetrics[] CharMetrics = null;

		internal virtual System.Drawing.SizeF MeasureString(string s, double size, TextState state)
		{
			return new System.Drawing.SizeF(float.NaN, float.NaN);
		}

		internal virtual double GetSizeFactor(double size) 
		{
			return double.NaN;
		}

	}
	#endregion

	#region CharMetrics 
	internal abstract class CharMetrics 
	{
		#region Ligature
		public class Ligature 
		{
			public string Successor = string.Empty;
			public string LigatureString = string.Empty;

			public Ligature(string successor, string ligatureString)
			{
				Successor = successor;
				LigatureString = ligatureString;
			}
		}
		#endregion

		public int CharacterCode = -1;
		public double WidthX = 250;
		public string Name = string.Empty;
		public double BBox_llx;
		public double BBox_lly;
		public double BBox_urx;
		public double BBox_ury;
		public List<Ligature> Ligatures = null;
	}
	#endregion
}
