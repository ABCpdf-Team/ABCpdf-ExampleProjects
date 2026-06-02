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
using System.EnterpriseServices;
using WebSupergoo.ABCpdf14;


// *** IMPORTANT ***
// The library will automatically be registered when it is first used.
// However if changes are made to the package settings then these will
// not be picked up.
//
// The easiest way to ensure your changes are picked up is to delete 
// the package from the Component Services Administrative Tool.
//
// Alternatiely you can re-register it manually from the command line:
//   regsvcs.exe PdfEnterpriseServices.dll

// The assembly must have a strong name.
// Create the key file with: sn -k PdfEnterpriseServices.snk
// This attribute should be specified on the Signing tab of
// the project properties for Visual Studio 2005.
// For the purpose of demonstration, it is placed here.
[assembly: System.Reflection.AssemblyKeyFile("PdfEnterpriseServices.snk")]

// The Application Name is the COM+ Application name displayed
// in the Component Services management tool MMC.
// This attribute should be placed in AssemblyInfo.cs
[assembly: ApplicationName("PDF Enterprise Services")]

// ApplicationActivation specifies the process in which the serviced components are activated.
// For activating in the creator's process (in-process), use ActivationOption.Library
// For activating in a system-provided process (out-of-process / out-of-machine),
// use ActivationOption.Server
// This attribute should be placed in AssemblyInfo.cs
[assembly: ApplicationActivation(ActivationOption.Server)]

// Enable Access Control
// If omitted, default true
// It is the same as the check box "Enforce access checks for this application"
// under Authorization of the Security tab of the Properties dialog box
// of the Component Services management tool MMC.
// This attribute should be placed in AssemblyInfo.cs
[assembly: ApplicationAccessControl(true)]

// Create a role so that everyone, including the internet guest user IUSR_<MachineName>,
// can use the serviced components
// This attribute should be placed in AssemblyInfo.cs
[assembly: SecurityRole("Internet Guest", true)]

namespace PdfEnterpriseServices {

    // Classes derived from ServicedComponent must be COM-visible.
    // The JustInTimeActivation attribute may be added to stateless classes.
    [System.Runtime.InteropServices.ComVisible(true)]
    public class PdfUtilities: ServicedComponent {
        public byte[] UrlToPdf(string url) {
            using (Doc theDoc = new Doc()) {
                theDoc.Rect.Inset(72, 144);


                theDoc.Page = theDoc.AddPage();
                int theID = theDoc.AddImageUrl(url);


                while (true) {
                    theDoc.FrameRect(); // add a black border
                    if (!theDoc.Chainable(theID))
                        break;
                    theDoc.Page = theDoc.AddPage();
                    theID = theDoc.AddImageToChain(theID);
                }


                for (int i = 1; i <= theDoc.PageCount; i++) {
                    theDoc.PageNumber = i;
                    theDoc.Flatten();
                }

                return theDoc.GetData();
            }
        }

        public byte[] DocToPdf(string path) {
            using (Doc theDoc = new Doc()) {
                theDoc.Read(path);
                return theDoc.GetData();
            }
        }
    }
}
