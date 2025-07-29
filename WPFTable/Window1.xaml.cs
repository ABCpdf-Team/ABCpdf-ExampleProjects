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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO.Packaging;
using System.Printing;

using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

using System.Windows.Markup;    

using WebSupergoo.ABCpdf13;

namespace WPFTable
{
  
    public partial class ExamplesWindow : Window
    {
        public ExamplesWindow() 
        {
            try
            {
                InitializeComponent();

                mExample = new Example();
                Table table = mExample.NewTable();

                mDocViewer = table.CreateDocViewer();
                MainArea.Children.Add(mDocViewer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void SaveAsPdf_Click(object sender, RoutedEventArgs e)
        {
            string pdfFileName = Directory.GetCurrentDirectory() + "\\..\\..\\output.pdf";
            Table table = mExample.NewTable();

            table.SaveToPdf(pdfFileName);
            System.Diagnostics.Process.Start(pdfFileName);
        }
        
        private void SaveAsXps_Click(object sender, RoutedEventArgs e)
        {
            string xpsFileName = Directory.GetCurrentDirectory() + "\\..\\..\\output.xps";
            Table table = mExample.NewTable();

            table.SaveToXps(xpsFileName);
            System.Diagnostics.Process.Start(xpsFileName);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /** Menu event handler, set the current example and recreate the
         document viewer, before attaching it to the window main area */
        private void Example_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item.Header as string == "Space")
            {
                mExample.Current = Example.Type.ESpace;
            }
            else if (item.Header as string == "Invoice")
            {
                mExample.Current = Example.Type.EInvoice;
            }
            else if (item.Header as string == "Small Table")
            {
                mExample.Current = Example.Type.ESmallTable;
            }
            else if (item.Header as string == "Large Table")
            {
                mExample.Current = Example.Type.ELargeTable;
            }

            Table table = mExample.NewTable();
            MainArea.Children.Remove(mDocViewer);

            mDocViewer = table.CreateDocViewer();
            MainArea.Children.Add(mDocViewer);
            
        }

        /** The current example */
        private Example mExample;

        /** The current document viewer attached to the window */
        private FlowDocumentPageViewer mDocViewer;
    }
}
