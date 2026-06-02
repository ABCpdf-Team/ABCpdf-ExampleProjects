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
using System.Windows;
using System.Reflection;
using Glue3DUtilities;

namespace Viewer3D
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
		private Renderer.GLVersionInfo VersionInfo { get; set; }

		/// <summary>Constructor, taking the version info of the Renderer</summary>
		public About(Renderer.GLVersionInfo versionInfo)
        {
            InitializeComponent();
			VersionInfo = versionInfo;
        }

		/// <summary>Handler for clicking the Ok button</summary>
		private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

		/// <summary>Handler for Window_Loaded event, to fill in the version info</summary>
		private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			Glue3DUtilities.Version version;

			Label_Viewer3DVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			Glue3DUtilities.LibVersionInfo.Glue3D(out version);
			Label_3DGlueVersion.Content = version.ToString();

			Glue3DUtilities.LibVersionInfo.RenderLib(out version);
			Label_RenderLibVersion.Content = version.ToString(); 

			Glue3DUtilities.LibVersionInfo.U3DLib(out version);
			Label_U3DLibVersion.Content = version.ToString();

			Glue3DUtilities.LibVersionInfo.PRCLib(out version);
			Label_PRCLibVersion.Content = version.ToString();

			Glue3DUtilities.LibVersionInfo.OBJLib(out version);
			Label_OBJLibVersion.Content = version.ToString();

			Label_GLVersion.Content = VersionInfo.version_GL.ToString();
			Label_GLSLVersion.Content = VersionInfo.version_GLSL.ToString();
		}
    }
}
