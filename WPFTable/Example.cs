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
using System.IO;

namespace WPFTable
{
    class Example
    {
        public Example()
        {
            mData = new Dictionary<Type, Data>();

            mData[Type.ESpace] = new Data("XMLTables/spaceSample.xml", "Sample", "XMLTables/SpaceTable.xaml");
            mData[Type.EInvoice] = new Data("XMLTables/travelfun.xml", "Item", "XMLTables/InvoiceTable.xaml");
            mData[Type.ELargeTable] = new Data(Directory.GetCurrentDirectory() + "/TextTables/text7.txt", "", "TextTables/LargeTable.xaml");
            mData[Type.ESmallTable] = new Data(Directory.GetCurrentDirectory() + "/TextTables/text6.txt", "", "TextTables/SmallTable.xaml");

            mType = Type.ESpace;
        }
        
        public Table NewTable()
        {
            if (mData[mType].XmlDataItem != "")
            {
                return new Table(mData[mType].XamlFile,mData[mType].DataFile, mData[mType].XmlDataItem);
            }
            else
            {
                return new Table(mData[mType].XamlFile,mData[mType].DataFile);
            }
        }

        /** We support 4 different examples*/
        public enum Type
        {
            ESpace = 0,      // XML table
            EInvoice = 1,    // XMl table
            ESmallTable = 2, // Text table
            ELargeTable = 3  // Text table
        }

        /** Stores data necessary to implement an example, e.g the xaml design file name */
        private class Data
        {
            public Data(string dataFile, string xmlDataItem, string xamlFile)
            {
                mDataFile = dataFile;
                mXmlDataItem = xmlDataItem;
                mXamlFile = xamlFile;
            }

            /** The file containing input data, either text or xml */
            private string mDataFile;
            public string DataFile
            {
                get
                {
                    return mDataFile;
                }
            }

            /** If table data provider is xml, then this is the name of the xml items in
             * the collection that populates the table, e.g. "Sample" for the space
             * table */
            private string mXmlDataItem;
            public string XmlDataItem
            {
                get
                {
                    return mXmlDataItem;
                }
            }

            /** The xaml design file */
            private string mXamlFile;
            public string XamlFile
            {
                get
                {
                    return mXamlFile;
                }
            }
        } //Data

        private Dictionary<Type, Data> mData;
        private Type mType;

        public Type Current
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

    }
}
