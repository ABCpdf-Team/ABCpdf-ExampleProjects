// ===========================================================================
//	©2013-2025 WebSupergoo. All rights reserved.
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
using System.Windows;
using System.Windows.Data;

namespace Viewer3D
{
	/// <summary>Helper class to convert UInt32 to Bool</summary>
	public class UInt32ToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UInt32 newVal = (UInt32)parameter;
            UInt32 curVal = (UInt32)value;

            return newVal.Equals(curVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (UInt32)parameter;
        }
    }

	/// <summary>Helper class to convert a lighting scheme to a bool </summary>
	public class LightingSchemeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            WebSupergoo.ELightingScheme newVal = (WebSupergoo.ELightingScheme)parameter;
            WebSupergoo.ELightingScheme curVal = (WebSupergoo.ELightingScheme)value;

            return newVal.Equals(curVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (WebSupergoo.ELightingScheme)parameter;
        }
    }

	/// <summary>Helper to convert a bool to a visibility flag</summary>
	public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                if ((Visibility)value == Visibility.Visible)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }



}
