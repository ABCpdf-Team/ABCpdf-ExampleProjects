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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net;

namespace WPFTable
{
    /** Store the contents of a line from the text files. Bindss 
     to text tables rows, via the Columns array property. */
    public class TextDataInfo 
    {
        public TextDataInfo(string[] text) : base()
        {
            mText = text;
        }

        private string[] mText;
        public string[] Columns
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
            }
        }
       
    }

    /** Parse a tab separated text file and create TextDataInfo items, one 
     per line. Used as DataContext in the text tables, 
     @see SmallTable.xaml and LargeTable.xaml.*/
    public class TextDataProvider : Collection<TextDataInfo>
    {
        /** Called from xaml (designer and runtime). */
        public TextDataProvider() : base()
        {   
        }

        /** Called from code to determine how many items we have to
            add to the table.*/
        public TextDataProvider(string fileName) : base()
        {
            mFileName = new Uri(fileName);
            ScanFile();
        }

        private void ScanFile()
        {
            WebRequest req = WebRequest.Create(mFileName);
            WebResponse resp = req.GetResponse();

            Stream file = resp.GetResponseStream();           
            StreamReader fileReader = new StreamReader(file);

            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                Add(new TextDataInfo(line.Split('\t')));
            }

            fileReader.Close();
            file.Close();         
        }

        /** The filename is set by the binding in the xaml files. This code executes from 
         the xaml designer as well as normal runtime. The designer seems to work only with
         a relative file, whilst runtime we seem to need an absolute uri. */
        private Uri mFileName;
        public Uri FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                if (DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow))
                {
                    mFileName = value;
                }
                else
                {
                    mFileName = new Uri(Directory.GetCurrentDirectory() + "/TextTables/" + value);
                }
                ScanFile();
            }
        }
    }
}
