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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WPFTable
{
    /**
     This example shows how to import WPF xaml tables into a PDF document. WPF tables are created 
     inside a  WPF FlowDocument. The document is then imported into PDF via XPS. 
     If tables span over multiple pages they are paginated by the document paginator.
      
     @note Important notes
     The WPF Table component does not support the following features:
      
        @liRow or Column borders, only table and cell borders are supported. 
        You can however specify a background for rows and columns.
        
        @li Automatic header repetition when the table is split across pages. This is not supported 
        out of the box but apparently people have been achieving this by adding the header manually 
        in code during the XPS serialization process. This is most likely achieved by extending the 
        DocumentPaginator class, although we have not tried it.
     
     */
    public partial class App : Application
    {
    }
}
