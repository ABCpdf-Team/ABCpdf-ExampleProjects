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

namespace Glue3DUtilities
{
	/// <summary>Convenience class to access version info of various libs contained inside the Glue3d dlls</summary>
	public static class LibVersionInfo
	{
		/// <summary>Get the version of the Glue3d dlls</summary>
		public static void Glue3D(out Version version)
		{
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_Glue3D, out version);
		}

		/// <summary>Get the version of the render library inside the Glue3D dll</summary>
		public static void RenderLib(out Version version)
		{
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_Rendering, out version);
		}

		/// <summary>Get the version of the U3D library inside the Glue3D dll</summary>
		public static void U3DLib(out Version version)
		{
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_U3D, out version);
		}

		/// <summary>Get the version of the PRC library inside the Glue3D dll</summary>
		public static void PRCLib(out Version version)
		{
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_PRC, out version);
		}

		/// <summary>Get the version of the OBJ library inside the Glue3D dll</summary>
		public static void OBJLib(out Version version)
		{
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_OBJ, out version);
		}

	}
}
