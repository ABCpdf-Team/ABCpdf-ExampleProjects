// ===========================================================================
//	©2013-2026 WebSupergoo. All rights reserved.
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
using System.Xml;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Markup;
using WebSupergoo.ABCpdf14;

namespace WPFTable
{
    /** 
     Load xaml table specification at runtime and dynamically add rows
     according to data provider attached to table. Support XmlDataProvider
     and TextDataProvider.
      
     The table that gets rows dynamically added must be called "ItemsTable"
     and must be located at "Page/FlowDocumentPageViewer/FlowDocument/Table"
     in the xaml file. The last row group will be duplicated, once per data item.
     */
    public class Table
    {
        /** Create a table using an XML data provider */
        public Table(string xamlFile, string xmlDataProvider, string xmlItem)
        {
            mXamlFile = xamlFile;
            mDataProvider = xmlDataProvider;
            mXmlItem = xmlItem;
            
            mType = Type.Xml;
        }

        /** Create a table using a text data provider */
        public Table(string xamlFile, string textDataProvider)
        {
            mXamlFile = xamlFile;
            mDataProvider = textDataProvider;

            mType = Type.Text;
        }

        private string mXamlFile;
        private string mDataProvider;
        private string mXmlItem;

        /** The name of the table in the xaml file, change this if you 
            give a different name to your table in the xaml file **/
        private string mTableName = "ItemsTable";
        public string TableName
        {
            get
            {
                return mTableName;
            }
            set
            {
                mTableName = value;
            }
        }

        /** The location of the table in the xaml file, change this if you
            need to change the struction of the xaml file from the default 
            location, which is:
            Page -> FlowDocumentPageViewer -> FlowDocument -> Table */
        private string mTableXamlLocation = "/x:Page/x:FlowDocumentPageViewer/x:FlowDocument/x:Table";
        public string TableXamlLocation
        {
            get
            {
                return mTableXamlLocation;
            }
            set
            {
                mTableXamlLocation = value;
            }

        }

        /** The data provider type (XML or text) */
        public enum Type
        {
            Text = 0,
            Xml = 1
        }

        private Type mType;

        /** Modify xaml file and then load the root visual: the page. Discard the page and 
            return the document viewer */
        public FlowDocumentPageViewer CreateDocViewer()
        {
            MemoryStream memStream;
            if (mType == Type.Xml)
            {
                memStream = ModifyXamlUsingXmlProvider();
            }
            else
            {
                memStream = ModifyXamlUsingTextProvider();
            }

            memStream.Seek(0, SeekOrigin.Begin);

            Page page = XamlReader.Load(memStream) as Page;
            FlowDocumentPageViewer docViewer = LogicalTreeHelper.FindLogicalNode(page, "DocViewer") as FlowDocumentPageViewer;
            page.Content = null;

            memStream.Close();
            return docViewer;
        }

        /** 
         Duplicate rows in XAML code using a text data provider, one row per data item.
         */
        private MemoryStream ModifyXamlUsingTextProvider()
        {
            TextDataProvider dataProvider = new TextDataProvider(mDataProvider);

            XmlDocument xamlDoc = new XmlDocument();
            FileStream xamlFile = new FileStream(mXamlFile, FileMode.Open);
            xamlDoc.Load(xamlFile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xamlDoc.NameTable);
            nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            XmlNode itemsTable = xamlDoc.DocumentElement.SelectSingleNode(mTableXamlLocation 
                                                            + "[@Name='" + mTableName + "']", nsmgr);

            for (int i = 1; i < dataProvider.Count; i++)
            {
                XmlNode rowGroup = itemsTable.LastChild;
                XmlNode newRowGroup = rowGroup.Clone();

                string bindingText = newRowGroup.Attributes["DataContext"].Value;
                bindingText = bindingText.Remove(bindingText.LastIndexOf('[')) + "[" + i + "] }";
                newRowGroup.Attributes["DataContext"].Value = bindingText;

                newRowGroup.Attributes["Background"].Value = (i % 2) == 0 ? "White" : "LightGray";

                itemsTable.InsertAfter(newRowGroup, rowGroup);
            }

            MemoryStream memStream = new MemoryStream();
            xamlDoc.Save(memStream);
            xamlFile.Close();
            return memStream;
        }

        /**
         Duplicate rows in XAML code using XML data provider, one row per data item.
         */
        private MemoryStream ModifyXamlUsingXmlProvider()
        {
            XmlDocument xmlDataDoc = new XmlDocument();
            FileStream xmlDataFile = new FileStream(mDataProvider, FileMode.Open);
            xmlDataDoc.Load(xmlDataFile);

            XmlDocument xamlDoc = new XmlDocument();
            FileStream xamlFile = new FileStream(mXamlFile, FileMode.Open);
            xamlDoc.Load(xamlFile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xamlDoc.NameTable);
            nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            XmlNode itemsTable = xamlDoc.DocumentElement.SelectSingleNode("/x:Page/x:FlowDocumentPageViewer/x:FlowDocument/x:Table[@Name='ItemsTable']", nsmgr);

            for (int i = 2; i <= xmlDataDoc.GetElementsByTagName(mXmlItem).Count; i++)
            {
                XmlNode rowGroup = itemsTable.LastChild;
                XmlNode newRowGroup = rowGroup.Clone();

                string bindingText = newRowGroup.Attributes["DataContext"].Value;
                bindingText = bindingText.Remove(bindingText.LastIndexOf('[')) + "[" + i + "] }";
                newRowGroup.Attributes["DataContext"].Value = bindingText;

                itemsTable.InsertAfter(newRowGroup, rowGroup);
            }
            xmlDataFile.Close();

            MemoryStream memStream = new MemoryStream();
            xamlDoc.Save(memStream);
            xamlFile.Close();
            return memStream;
        }
        
        /** Save file as PDF */
        public void SaveToPdf(string pdfFileName)
        {
            MemoryStream memStream = new MemoryStream();
            SaveToXps(memStream);

            XReadOptions options = new XReadOptions();
            options.ReadModule = ReadModuleType.Xps;
            Doc pdfDoc = new Doc();
            pdfDoc.Read(memStream, options);

            pdfDoc.Save(pdfFileName);
            pdfDoc.Clear();

            memStream.Close();
        }

        /** Save file as XPS */
        public void SaveToXps(string xpsFileName)
        {
            FileStream fileStream = new FileStream(xpsFileName,FileMode.Create);
            SaveToXps(fileStream);

            fileStream.Close();           
        }

        /** Save file stream as XPS */
        private void SaveToXps(Stream fileStream)
        {
            Package package = Package.Open(fileStream, FileMode.Create, FileAccess.ReadWrite);

            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            IDocumentPaginatorSource document = CreateDocViewer().Document;
            writer.Write(document.DocumentPaginator);

            doc.Close();
            package.Close();
        }
    }
}
