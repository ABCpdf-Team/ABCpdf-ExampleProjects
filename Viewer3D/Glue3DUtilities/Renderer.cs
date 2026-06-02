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
using System.ComponentModel;
using System.Windows;

namespace Glue3DUtilities
{
	/// <summary>Convenience class to interface with the renderer</summary>
    [Description("Helper class to interface with the Renderer")]
    public static class Renderer
    {
		/// <summary>Helper struct to hold version info about OpenGL and GLSL (OpenGL Shading Language)</summary>
		public struct GLVersionInfo
		{
			/// <summary>The version info of the OpenGL context being used</summary>
			public Version version_GL;
			/// <summary>The version info of the shaders (GLSL) in the OpenGL context being used</summary>
			public Version version_GLSL;

			/// <summary>Helper function to set the version from packed values</summary>
			public static GLVersionInfo FromPackedInt32s(Int32 glVersionPacked, Int32 glslVersionPacked)
			{
				GLVersionInfo outInfo = new GLVersionInfo();
				outInfo.version_GL.major = (glVersionPacked >> 16);
				outInfo.version_GL.minor = (glVersionPacked & 0xFFFF);
				outInfo.version_GLSL.major = (glslVersionPacked >> 16);
				outInfo.version_GLSL.minor = (glslVersionPacked & 0xFFFF);

				return outInfo;
			}
		};

		/// <summary>Static constructor, which adds an ExitEventHandler</summary>
		static Renderer()
        {
            Application.Current.Exit += new ExitEventHandler(OnApplicationExit);
        }

		/// <summary>The exit handler</summary>
		private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Shutdown();
        }

		/// <summary>Shut down the renderer, free up resources</summary>
		internal static void Shutdown()
        {
            Dll_Glue3D.ShutdownRenderer();
        }

		/// <summary>Initialize the renderer with our specified settings (Note: If SetupRenderer is not called, it will just use default values)</summary>
		public static bool Init(WebSupergoo.RendererParams rendererParams)
        {
            return Dll_Glue3D.SetupRenderer(rendererParams);
        }

		/// <summary>Helper to retrieve OpenGL and GLSL version info from the renderer, as used in the provided RenderProxy</summary>
		public static GLVersionInfo GetVersionInfo(IntPtr renderProxy)
		{
			Int32 glVersionPacked = Dll_Glue3D.GetProperty_Int32(renderProxy, WebSupergoo.EProperty.Prop_OpenGL_Version);
			Int32 glslVersionPacked = Dll_Glue3D.GetProperty_Int32(renderProxy, WebSupergoo.EProperty.Prop_GLSL_Version);
			GLVersionInfo v = GLVersionInfo.FromPackedInt32s(glVersionPacked, glslVersionPacked);
			return v;
		}
	}
}
