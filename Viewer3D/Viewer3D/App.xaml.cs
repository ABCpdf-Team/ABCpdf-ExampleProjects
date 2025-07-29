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
using System.Windows;
using System.Threading;

using Glue3DUtilities;


namespace Viewer3D
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		/// <summary>Constructor</summary>
		public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

		/// <summary>Handler for Application startup event, to set up logging and various other variables</summary>
		void App_Startup(object sender, StartupEventArgs e)
        {
            // set app and company name
            Utilities.SetApplicationName("Viewer3D");
            Utilities.SetCompanyName("WebSupergoo");

            // start logger
            if (Log.StartLogging("", false, Log.LogLevel.Info) == false)
            {
                MessageBox.Show("Failed to initialize logging!", "Error");
            }

            // setup renderer
            WebSupergoo.RendererParams rendererParams = new WebSupergoo.RendererParams();
            rendererParams.majorVersionGL = 0;
            rendererParams.minorVersionGL = 0;
            rendererParams.antialiasingColorSamples = 8;
            rendererParams.antialiasingCoverageSamples = 0;
            rendererParams.flags = (UInt32)WebSupergoo.ERendererFlags.RenderFlags_RenderToFBO;
            Renderer.Init(rendererParams);

			// print version info
			Glue3DUtilities.Version version;
			LibVersionInfo.Glue3D(out version);
			Log.Info("Glue3D Version " + version.ToString());
			LibVersionInfo.RenderLib(out version);
			Log.Info("RenderLib Version " + version.ToString());
			LibVersionInfo.U3DLib(out version);
			Log.Info("U3dLib Version " + version.ToString());
			LibVersionInfo.PRCLib(out version);
			Log.Info("PRCLib Version " + version.ToString());
			LibVersionInfo.OBJLib(out version);
			Log.Info("OBJLib Version " + version.ToString());
		}

		/// <summary>Handler for Application exit event</summary>
		void App_Exit(object sender, ExitEventArgs e)
        {
            Log.Info("Shutting down...");

            Thread.Sleep(20);   // let it finish printing messages 
        }

    }
}
