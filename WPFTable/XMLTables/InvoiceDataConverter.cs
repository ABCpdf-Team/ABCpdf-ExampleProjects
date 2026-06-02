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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace WPFTable
{
    /** Binding for InvoiceTable, determines the numbers that show up in the right column (quant * price)
     of the items table and in the totals table. @see InvoiceTable.xaml for how this converted is used in
     multibinding. */
    public class InvoiceDataConverter : IMultiValueConverter
    {
        private Int32 mQuantity;
        private Double mUnitPrice;
        private Double mSubTotal;
        private readonly Int32 mShipping = 500;
        private readonly Double mTax = 0.05;

        public InvoiceDataConverter()
        {
            mQuantity = 0;
            mUnitPrice = 0;
            mSubTotal = 0;
        }

        /** Return values for the Totals table if a parameter is specified, e.g. "SubTotal". If no
         parameter specified then return the right column of the itmes table (quant * price). In this
         latter case also increment sub-total. */
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name;

            /** Totals table */
            if (parameter as string == "SubTotal")
            {
                name = mSubTotal.ToString("n");
            }
            else if (parameter as string == "Shipping")
            {
                name = mShipping.ToString("n");
            }
            else if (parameter as string == "Tax")
            {
                name = mTax.ToString("n");
            }
            else if (parameter as string == "Total")
            {
                Double total = mSubTotal + mShipping + (mTax * mSubTotal);
                name = total.ToString("n");
            }
            else
            {/** ItemsTable */
                mQuantity = System.Convert.ToInt32(values[0] as string);
                mUnitPrice = System.Convert.ToDouble(values[1] as string);

                Double value = mQuantity * mUnitPrice;
                mSubTotal += value;

                name = value.ToString("n");
            }
            return name;
        }


        /** Not called */
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            object[] values = new object[2];
            values[0] = mQuantity;
            values[1] = mUnitPrice;

            return values;
        }
    }
}
