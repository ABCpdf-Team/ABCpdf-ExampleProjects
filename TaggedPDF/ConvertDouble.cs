// ===========================================================================
//	©2013-2024 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product\. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.Globalization;

namespace TaggedPDF {
	/// <summary>
	/// Class used for converting floating point numbers
	/// </summary>
	public class ConvertDouble {
		/// <summary>
		/// Number format for string conversion
		/// </summary>
		private static NumberFormatInfo _formatProvider = GetFormatProvider();

		/// <summary>
		/// Get the number format
		/// </summary>
		private static NumberFormatInfo GetFormatProvider() {
			NumberFormatInfo formatProvider = new NumberFormatInfo();
			formatProvider.NumberDecimalSeparator = ".";
			formatProvider.NumberDecimalDigits = 5;
			return formatProvider;
		}

		/// <summary>
		/// Convert double value to string representation
		/// </summary>
		/// <param name="x">the double value</param>
		/// <returns>pdf string representation of the double</returns>
		public static string ToString(string x) {
			double res;
			double.TryParse(x, NumberStyles.Float, _formatProvider, out res);
			return string.Format(_formatProvider, " {0:F} ", res);
		}

		/// <summary>
		/// Converts string to float.
		/// </summary>
		/// <param name="str">string to be converted</param>
		/// <returns>double representation of the string.</returns>
		public static double FromString(string str) {
			double res;
			double.TryParse(str, NumberStyles.Float, _formatProvider, out res);
			return res;
		}
	}
}
