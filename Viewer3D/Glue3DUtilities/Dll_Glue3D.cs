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
using System.Runtime.InteropServices;

namespace Glue3DUtilities {
	using Static = WebSupergoo.Static;

	/// <summary>
	/// The interface to the Glue3D dlls (which contain the 3D rendering functionality)
	/// </summary>
	[System.Security.SuppressUnmanagedCodeSecurity]
	internal static class Dll_Glue3D {
		#region StaticMembers

		/// <summary>This flag decides whether to call into 32 or 64 bit versions of the Glue3D dll</summary>
		private static bool Is64Bit;
		#endregion

		#region StaticConstructor

		static Dll_Glue3D() {
			// check if 32 or 64 bit by getting the size of the IntPtr
			Is64Bit = IntPtr.Size != 4;
			// verify minimum needed version
			Version minVersion = new Version(14, 0, 0); // ABCpdf14
			Version glue3DVersion;
			Dll_Glue3D.GetLibraryVersion((int)WebSupergoo.ELibrary.Lib_Glue3D, out glue3DVersion);
			if (!Utilities.IsVersionOk(minVersion, glue3DVersion, false, false)) // it might be a good idea to compare for exact versions to avoid interface problems
			{
				string message = "Minimum version of Glue3D expected: " + minVersion.ToString() + ", version found: " + glue3DVersion.ToString();
				throw new Exception(message);
			}
		}

		#endregion

		#region Public Interface

		public static string StringFromDll(IntPtr list, int len) {
#if NETCOREAPP
			return Marshal.PtrToStringUTF8(p, len);
#else
			unsafe {
				return list==IntPtr.Zero ? null :
					System.Text.Encoding.UTF8.GetString((byte*)list.ToPointer(), len);	// this overload requires .NET Framework 4.6
			}
#endif
		}
		public static string[] StringArrayFromDll(IntPtr list, IntPtr lenList, uint count) {
			if(list==IntPtr.Zero)
				return null;
			string[] arr = new string[count];
			int int32Size = Marshal.SizeOf<int>();
			for(int i = 0; i < count; ++i) {
				IntPtr p = Marshal.ReadIntPtr(list, IntPtr.Size * i);
				int len = Marshal.ReadInt32(lenList, int32Size * i);
				arr[i] = StringFromDll(p, len);
			}
			return arr;
		}


		#region VersionInfo

		//
		// Version info for the various subcomponents of the Glue3D library
		//

		/// <summary>Get the version info of the various library components inside the Glue3D dlls (see ELibrary enum)</summary>
		/// <seealso cref="ELibrary"/>
		public static void GetLibraryVersion(int libraryID, out Version version) {
			version = new Version();

			if (Is64Bit)
				Impl.GetLibraryVersion_64(libraryID, ref version.major, ref version.minor, ref version.build);
			else
				Impl.GetLibraryVersion_32(libraryID, ref version.major, ref version.minor, ref version.build);
		}

#endregion

#region Utilities
		//
		// Utilities
		//

		/// <summary>Set company name (can be queried by other libraries, and used for example to create an APPDATA path)</summary>
		public static void SetCompanyName(string companyName) {
			if (Is64Bit)
				Impl.SetCompanyName_64(false, companyName);
			else
				Impl.SetCompanyName_32(false, companyName);
		}

		/// <summary>Get company name</summary>
		public static string GetCompanyName() {
			if (Is64Bit)
				return Impl.GetCompanyName_64();
			else
				return Impl.GetCompanyName_32();
		}

		/// <summary>Set application name (can be queried by other libraries, and used for example to create an APPDATA path)</summary>
		public static void SetApplicationName(string companyName) {
			if (Is64Bit)
				Impl.SetApplicationName_64(false, companyName);
			else
				Impl.SetApplicationName_32(false, companyName);
		}

		/// <summary>Get application name</summary>
		public static string GetApplicationName() {
			if (Is64Bit)
				return Impl.GetApplicationName_64();
			else
				return Impl.GetApplicationName_32();
		}

#endregion

#region Logging
		//
		// Logging (to a specified file)
		//

		/// <summary>Start the library internal logger</summary>
		public static bool StartLogging(string logFile, bool append, int logLevel) {
			if (Is64Bit)
				return Impl.StartLogging_64(false, logFile, append, logLevel);
			else
				return Impl.StartLogging_32(false, logFile, append, logLevel);
		}

		/// <summary>Stop the library internal logger</summary>
		public static void StopLogging() {
			if (Is64Bit)
				Impl.StopLogging_64();
			else
				Impl.StopLogging_32();
		}

		/// <summary>Check if logging is enabled</summary>
		public static bool IsLogging() {
			if (Is64Bit)
				return Impl.IsLogging_64();
			else
				return Impl.IsLogging_32();
		}

		/// <summary>Change the log level</summary>
		public static void SetLogLevel(int logLevel) {
			if (Is64Bit)
				Impl.SetLogLevel_64(logLevel);
			else
				Impl.SetLogLevel_32(logLevel);
		}

		/// <summary>Get the current log level</summary>
		public static int GetLogLevel() {
			if (Is64Bit)
				return Impl.GetLogLevel_64();
			else
				return Impl.GetLogLevel_32();
		}

		/// <summary>Write a log message</summary>
		public static void LogMessage(int logLevel, string logMsg) {
			if (Is64Bit)
				Impl.Log_64(false, logLevel, logMsg);
			else
				Impl.Log_32(false, logLevel, logMsg);
		}

		/// <summary>Add a logging hook, for a delegate to also receive the log messages</summary>
		public static WebSupergoo.DelegateLogging SetLoggingHook(IntPtr userData, WebSupergoo.DelegateLogging hook) {
			if (Is64Bit)
				return Impl.SetLoggingHook_64(userData, hook);
			else
				return Impl.SetLoggingHook_32(userData, hook);
		}

		/// <summary>Get the current logging hook</summary>
		public static WebSupergoo.DelegateLogging GetLoggingHook(out IntPtr userData) {
			if (Is64Bit)
				return Impl.GetLoggingHook_64(out userData);
			else
				return Impl.GetLoggingHook_32(out userData);
		}

#endregion

#region Renderer

		//
		// Renderer - if rendering is supposed to use special parameters (for exanple OpenGL version, AA settings, vsync), 
		//			  then renderer should be initialized before use, otherwise it will fall back to using defaults
		//

		/// <summary>Set up the renderer with the provided parameters</summary>
		public static bool SetupRenderer(WebSupergoo.RendererParams rendererParams) {
			if (Is64Bit)
				return Impl.SetupRenderer_64(rendererParams);
			else
				return Impl.SetupRenderer_32(rendererParams);
		}

		/// <summary>Shut down the renderer</summary>
		internal static void ShutdownRenderer() {
			if (Is64Bit)
				Impl.ShutdownRenderer_64();
			else
				Impl.ShutdownRenderer_32();
		}

#endregion

#region Capture

		//
		// Capture functionality allows capturing 3D scenes to images (either to a file or to memory)
		//

		/// <summary>Load a scene for capturing (just loads, but does not capture, or render the scene)</summary>
		public static void LoadCaptureScene(WebSupergoo.LoadSceneParams loadParams, WebSupergoo.DelegateLoadComplete cb) {
			if (Is64Bit)
				Impl.LoadCaptureScene_64(false, loadParams, cb);
			else
				Impl.LoadCaptureScene_32(false, loadParams, cb);
		}

		/// <summary>Unload a currently open scene</summary>
		public static void UnloadCaptureScene(uint sceneHandle, WebSupergoo.DelegateUnloadComplete cb) {
			if (Is64Bit)
				Impl.UnloadCaptureScene_64(sceneHandle, cb);
			else
				Impl.UnloadCaptureScene_32(sceneHandle, cb);
		}

		/// <summary>Unload all currently open capture scenes</summary>
		public static void UnloadAllCaptureScenes(WebSupergoo.DelegateUnloadAllComplete cb) {
			if (Is64Bit)
				Impl.UnloadAllCaptureScenes_64(cb);
			else
				Impl.UnloadAllCaptureScenes_32(cb);
		}

		/// <summary>Do a capture, if the scene was already loaded just pass in the SceneHandle, otherwise pass in a filename to load the scene for capturing (and unload it afterwards if Capture_UnloadAfterCapture was set)</summary>
		public static void CaptureScene(WebSupergoo.CaptureParams parms, WebSupergoo.DelegateCaptureComplete cb) {
			if (Is64Bit)
				Impl.CaptureScene_64(false, parms, cb);
			else
				Impl.CaptureScene_32(false, parms, cb);
		}

#endregion

#region RenderProxy

		//
		// A Render proxy object is an object responsible for rendering scenes, and which has a window (on a different thread) associated with it
		//

		/// <summary>Create a render proxy instance using the provided parameters, returns the IntPtr for the instance and fills the RenderProxyInfo</summary>
		public static IntPtr CreateRenderProxy(WebSupergoo.RenderProxyParams proxyParams, ref WebSupergoo.RenderProxyInfo outInfo) {
			//MessageBox.Show("CreateRenderProxy");
			IntPtr proxy = IntPtr.Zero;

			if (Is64Bit)
				proxy = Impl.CreateRenderProxy_64(proxyParams, ref outInfo);
			else
				proxy = Impl.CreateRenderProxy_32(proxyParams, ref outInfo);

			return proxy;
		}

		/// <summary>Destroy a render proxy instance</summary>
		public static void DestroyRenderProxy(IntPtr renderProxyInst) {
			//MessageBox.Show("DestroyRenderProxy");

			if (renderProxyInst != IntPtr.Zero) {
				if (Is64Bit)
					Impl.DestroyRenderProxy_64(renderProxyInst);
				else
					Impl.DestroyRenderProxy_32(renderProxyInst);
			}
		}

		/// <summary>Get a HWND for this render proxy</summary>
		public static IntPtr GetRenderProxyHwnd(IntPtr renderProxyInst) {
			if (Is64Bit)
				return Impl.GetRenderProxyHwnd_64(renderProxyInst);
			else
				return Impl.GetRenderProxyHwnd_32(renderProxyInst);
		}

#endregion

#region Scene

		//
		// Scene related functions
		//

		/// <summary>Load a scene into the provided render proxy for display</summary>
		public static void LoadScene(IntPtr renderProxyInst, WebSupergoo.LoadSceneParams loadParams, WebSupergoo.ViewParams viewParams, WebSupergoo.DelegateLoadComplete cb) {
			if (Is64Bit)
				Impl.LoadScene_64(false, renderProxyInst, loadParams, viewParams, cb);
			else
				Impl.LoadScene_32(false, renderProxyInst, loadParams, viewParams, cb);
		}

		/// <summary>Close a loaded scene</summary>
		public static void CloseScene(IntPtr renderProxyInst, WebSupergoo.DelegateUnloadComplete cb) {
			if (Is64Bit)
				Impl.CloseScene_64(renderProxyInst, cb);
			else
				Impl.CloseScene_32(renderProxyInst, cb);
		}

		/// <summary>Check if any scene is loaded</summary>
		public static bool IsSceneLoaded(IntPtr renderProxyInst) {
			if (Is64Bit)
				return Impl.IsSceneLoaded_64(renderProxyInst);
			else
				return Impl.IsSceneLoaded_32(renderProxyInst);
		}

		/// <summary>Stop the current import (interrupts at the current point, without completing the loading process)</summary>
		public static void InterruptCurrentImport(IntPtr renderProxyInst) {
			if (Is64Bit)
				Impl.InterruptCurrentImport_64();
			else
				Impl.InterruptCurrentImport_32();
		}

		/// <summary>Allow exporting a scene to file</summary>
		public static void ExportScene(IntPtr renderProxyInst, WebSupergoo.ExportSceneParams exportParams, WebSupergoo.DelegateExportComplete cb) {
			if (Is64Bit)
				Impl.ExportScene_64(false, renderProxyInst, exportParams, cb);
			else
				Impl.ExportScene_32(false, renderProxyInst, exportParams, cb);
		}

#endregion

#region Camera

		// 
		// Camera related functionality (only active when a scene has been loaded)
		//

		/// <summary>Activate the specified camera</summary>
		public static void ActivateCamera(IntPtr renderProxyInst, WebSupergoo.ECameraActivationType activationType, string cameraName, WebSupergoo.DelegateOnActiveCameraChanged cb) {
			if (Is64Bit)
				Impl.ActivateCamera_64(false, renderProxyInst, (uint)activationType, cameraName, cb);
			else
				Impl.ActivateCamera_32(false, renderProxyInst, (uint)activationType, cameraName, cb);
		}

		/// <summary>Get a list of available cameras</summary>
		public static void GetAvailableCameras(IntPtr renderProxyInst, WebSupergoo.DelegateStrList cb) {
			if (Is64Bit)
				Impl.GetAvailableCameras_64(renderProxyInst, cb);
			else
				Impl.GetAvailableCameras_32(renderProxyInst, cb);
		}

		/// <summary>Write a screenshot to file</summary>
		public static void Screenshot(IntPtr renderProxyInst, string filename) {
			if (Is64Bit)
				Impl.Screenshot_64(false, renderProxyInst, filename);
			else
				Impl.Screenshot_32(false, renderProxyInst, filename);
		}

		/// <summary>Load view info from a text file containing PDF-conformat view infos (annotation of type 3DView)</summary>
		public static void LoadViewInfoFromFile(IntPtr renderProxyInst, string filename, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb) {
			if (Is64Bit)
				Impl.LoadViewInfoFromFile_64(false, renderProxyInst, filename, replaceExisting, cb);
			else
				Impl.LoadViewInfoFromFile_32(false, renderProxyInst, filename, replaceExisting, cb);
		}

		/// <summary>Load view info from a string containing PDF-conformat view infos (annotation of type 3DView)</summary>
		public static void LoadViewInfoFromString(IntPtr renderProxyInst, string viewInfo, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb) {
			if (Is64Bit)
				Impl.LoadViewInfoFromString_64(false, renderProxyInst, viewInfo, replaceExisting, cb);
			else
				Impl.LoadViewInfoFromString_32(false, renderProxyInst, viewInfo, replaceExisting, cb);
		}

#endregion

#region Properties

		//
		// Properties - allows to directly access various render-related properties
		//

		/// <summary>Get a property of boolean type</summary>
		public static bool GetProperty_Bool(IntPtr renderProxyInst, WebSupergoo.EProperty propType) {
			if (Is64Bit)
				return Impl.GetProperty_Bool_64(renderProxyInst, (uint)propType);
			else
				return Impl.GetProperty_Bool_32(renderProxyInst, (uint)propType);
		}

		/// <summary>Set a property of boolean type</summary>
		public static void SetProperty_Bool(IntPtr renderProxyInst, WebSupergoo.EProperty propType, bool newValue, bool async = true) {
			if (Is64Bit)
				Impl.SetProperty_Bool_64(renderProxyInst, (uint)propType, newValue, async);
			else
				Impl.SetProperty_Bool_32(renderProxyInst, (uint)propType, newValue, async);
		}

		/// <summary>Get a property of int32 type</summary>
		public static int GetProperty_Int32(IntPtr renderProxyInst, WebSupergoo.EProperty propType) {
			if (Is64Bit)
				return Impl.GetProperty_Int32_64(renderProxyInst, (uint)propType);
			else
				return Impl.GetProperty_Int32_32(renderProxyInst, (uint)propType);
		}

		/// <summary>Set a property of int32 type</summary>
		public static void SetProperty_Int32(IntPtr renderProxyInst, WebSupergoo.EProperty propType, int newValue, bool async = true) {
			if (Is64Bit)
				Impl.SetProperty_Int32_64(renderProxyInst, (uint)propType, newValue, async);
			else
				Impl.SetProperty_Int32_32(renderProxyInst, (uint)propType, newValue, async);
		}

		/// <summary>Get a property of int64 type</summary>
		public static Int64 GetProperty_Int64(IntPtr renderProxyInst, WebSupergoo.EProperty propType) {
			if (Is64Bit)
				return Impl.GetProperty_Int64_64(renderProxyInst, (uint)propType);
			else
				return Impl.GetProperty_Int64_32(renderProxyInst, (uint)propType);
		}

		/// <summary>Set a property of int64 type</summary>
		public static void SetProperty_Int64(IntPtr renderProxyInst, WebSupergoo.EProperty propType, long newValue, bool async = true) {
			if (Is64Bit)
				Impl.SetProperty_Int64_64(renderProxyInst, (uint)propType, newValue, async);
			else
				Impl.SetProperty_Int64_32(renderProxyInst, (uint)propType, newValue, async);
		}

		/// <summary>Get a property of double type</summary>
		public static double GetProperty_Double(IntPtr renderProxyInst, WebSupergoo.EProperty propType) {
			if (Is64Bit)
				return Impl.GetProperty_Double_64(renderProxyInst, (uint)propType);
			else
				return Impl.GetProperty_Double_32(renderProxyInst, (uint)propType);
		}

		/// <summary>Set a property of boolean type</summary>
		public static void SetProperty_Double(IntPtr renderProxyInst, WebSupergoo.EProperty propType, double newValue, bool async = true) {
			if (Is64Bit)
				Impl.SetProperty_Double_64(renderProxyInst, (uint)propType, newValue, async);
			else
				Impl.SetProperty_Double_32(renderProxyInst, (uint)propType, newValue, async);
		}

		/// <summary>Get a property of string type</summary>
		public static string GetProperty_String(IntPtr renderProxyInst, WebSupergoo.EProperty propType) {
			if (Is64Bit)
				return Impl.GetProperty_String_64(renderProxyInst, (uint)propType);
			else
				return Impl.GetProperty_String_32(renderProxyInst, (uint)propType);
		}

		/// <summary>Set a property of string type</summary>
		public static void SetProperty_String(IntPtr renderProxyInst, WebSupergoo.EProperty propType, string newValue, bool async = true) {
			if (Is64Bit)
				Impl.SetProperty_String_64(false, renderProxyInst, (uint)propType, newValue, async);
			else
				Impl.SetProperty_String_32(false, renderProxyInst, (uint)propType, newValue, async);
		}

#endregion

#endregion

		// -----------------

#region Impl

		/// <summary>
		/// Implementation for the actual dll interface, to call either into the 32 or 64 bit version of the dlls
		/// </summary>
		private static class Impl {
			private const CallingConvention DllApi = Static.DllApi;
			private const string Dll32Name = "3DGlue14-32.dll"; // ABCpdf14
			private const string Dll64Name = "3DGlue14-64.dll"; // ABCpdf14

#region VersionInfo

			//
			// version info
			//

			[DllImport(Dll32Name, EntryPoint = "GetLibraryVersion", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void GetLibraryVersion_32(int libID, ref int major, ref int minor, ref int build);

			[DllImport(Dll64Name, EntryPoint = "GetLibraryVersion", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void GetLibraryVersion_64(int libID, ref int major, ref int minor, ref int build);

#endregion

#region Utilities

			//
			// Utilities
			//

			[DllImport(Dll32Name, EntryPoint = "SetCompanyName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetCompanyName_32(bool nativeStr, string companyName);

			[DllImport(Dll64Name, EntryPoint = "SetCompanyName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetCompanyName_64(bool nativeStr, string companyName);

			[DllImport(Dll32Name, EntryPoint = "GetCompanyName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetCompanyName_32();

			[DllImport(Dll64Name, EntryPoint = "GetCompanyName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetCompanyName_64();

			[DllImport(Dll32Name, EntryPoint = "SetApplicationName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetApplicationName_32(bool nativeStr, string applicationName);

			[DllImport(Dll64Name, EntryPoint = "SetApplicationName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetApplicationName_64(bool nativeStr, string applicationName);

			[DllImport(Dll32Name, EntryPoint = "GetApplicationName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetApplicationName_32();

			[DllImport(Dll64Name, EntryPoint = "GetApplicationName", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetApplicationName_64();

#endregion

#region Logging

			//
			// Logging
			//

			[DllImport(Dll32Name, EntryPoint = "StartLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool StartLogging_32(bool nativeStr, string logFile, bool append, int logLevel);

			[DllImport(Dll64Name, EntryPoint = "StartLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool StartLogging_64(bool nativeStr, string logFile, bool append, int logLevel);

			[DllImport(Dll32Name, EntryPoint = "StopLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void StopLogging_32();

			[DllImport(Dll64Name, EntryPoint = "StopLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void StopLogging_64();

			[DllImport(Dll32Name, EntryPoint = "IsLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool IsLogging_32();

			[DllImport(Dll64Name, EntryPoint = "IsLoggingToDefault", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool IsLogging_64();

			[DllImport(Dll32Name, EntryPoint = "SetLogLevel", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetLogLevel_32(int logLevel);

			[DllImport(Dll64Name, EntryPoint = "SetLogLevel", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetLogLevel_64(int logLevel);

			[DllImport(Dll32Name, EntryPoint = "GetLogLevel", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern int GetLogLevel_32();

			[DllImport(Dll64Name, EntryPoint = "GetLogLevel", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern int GetLogLevel_64();

			[DllImport(Dll32Name, EntryPoint = "Log", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void Log_32(bool nativeStr, int logLevel, string logMsg);

			[DllImport(Dll64Name, EntryPoint = "Log", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void Log_64(bool nativeStr, int logLevel, string logMsg);

			[DllImport(Dll32Name, EntryPoint = "SetLoggingHook", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern WebSupergoo.DelegateLogging SetLoggingHook_32(IntPtr userData, WebSupergoo.DelegateLogging hook);

			[DllImport(Dll64Name, EntryPoint = "SetLoggingHook", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern WebSupergoo.DelegateLogging SetLoggingHook_64(IntPtr userData, WebSupergoo.DelegateLogging hook);

			[DllImport(Dll32Name, EntryPoint = "GetLoggingHook", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern WebSupergoo.DelegateLogging GetLoggingHook_32(out IntPtr userData);

			[DllImport(Dll64Name, EntryPoint = "GetLoggingHook", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern WebSupergoo.DelegateLogging GetLoggingHook_64(out IntPtr userData);


#endregion

#region Renderer

			// Renderer

			[DllImport(Dll32Name, EntryPoint = "SetupRenderer", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool SetupRenderer_32([MarshalAs(UnmanagedType.Struct)] WebSupergoo.RendererParams rendererParams);

			[DllImport(Dll64Name, EntryPoint = "SetupRenderer", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool SetupRenderer_64([MarshalAs(UnmanagedType.Struct)] WebSupergoo.RendererParams rendererParams);

			[DllImport(Dll32Name, EntryPoint = "ShutdownRenderer", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ShutdownRenderer_32();

			[DllImport(Dll64Name, EntryPoint = "ShutdownRenderer", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ShutdownRenderer_64();

#endregion

#region Capture

			[DllImport(Dll32Name, EntryPoint = "LoadCaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadCaptureScene_32(bool nativeStr, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.LoadSceneParams loadParams, WebSupergoo.DelegateLoadComplete cb);

			[DllImport(Dll64Name, EntryPoint = "LoadCaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadCaptureScene_64(bool nativeStr, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.LoadSceneParams loadParams, WebSupergoo.DelegateLoadComplete cb);

			[DllImport(Dll32Name, EntryPoint = "UnloadCaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void UnloadCaptureScene_32(uint sceneHandle, WebSupergoo.DelegateUnloadComplete cb);

			[DllImport(Dll64Name, EntryPoint = "UnloadCaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void UnloadCaptureScene_64(uint sceneHandle, WebSupergoo.DelegateUnloadComplete cb);

			[DllImport(Dll32Name, EntryPoint = "UnloadAllCaptureScenes", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void UnloadAllCaptureScenes_32(WebSupergoo.DelegateUnloadAllComplete cb);

			[DllImport(Dll64Name, EntryPoint = "UnloadAllCaptureScenes", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void UnloadAllCaptureScenes_64(WebSupergoo.DelegateUnloadAllComplete cb);

			[DllImport(Dll32Name, EntryPoint = "CaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void CaptureScene_32(bool nativeStr, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.CaptureParams parms, WebSupergoo.DelegateCaptureComplete cb);

			[DllImport(Dll64Name, EntryPoint = "CaptureScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void CaptureScene_64(bool nativeStr, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.CaptureParams parms, WebSupergoo.DelegateCaptureComplete cb);

#endregion

#region RenderProxy

			//
			// render proxy object, which has a window (on a different thread) 
			//

			[DllImport(Dll32Name, EntryPoint = "CreateRenderProxy", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern IntPtr CreateRenderProxy_32([MarshalAs(UnmanagedType.Struct)] WebSupergoo.RenderProxyParams proxyParams, ref WebSupergoo.RenderProxyInfo outInfo);

			[DllImport(Dll64Name, EntryPoint = "CreateRenderProxy", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern IntPtr CreateRenderProxy_64([MarshalAs(UnmanagedType.Struct)] WebSupergoo.RenderProxyParams proxyParams, ref WebSupergoo.RenderProxyInfo outInfo);

			[DllImport(Dll32Name, EntryPoint = "DestroyRenderProxy", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void DestroyRenderProxy_32(IntPtr renderProxyInst);

			[DllImport(Dll64Name, EntryPoint = "DestroyRenderProxy", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void DestroyRenderProxy_64(IntPtr renderProxyInst);

			[DllImport(Dll32Name, EntryPoint = "GetRenderProxyHwnd", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern IntPtr GetRenderProxyHwnd_32(IntPtr renderProxyInst);

			[DllImport(Dll64Name, EntryPoint = "GetRenderProxyHwnd", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern IntPtr GetRenderProxyHwnd_64(IntPtr renderProxyInst);

#endregion

#region Scene
			//
			// Scene
			//

			[DllImport(Dll32Name, EntryPoint = "LoadScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadScene_32(bool nativeStr, IntPtr renderProxyInst,
				[MarshalAs(UnmanagedType.Struct)] WebSupergoo.LoadSceneParams loadParams,
				[MarshalAs(UnmanagedType.Struct)] WebSupergoo.ViewParams viewParams, WebSupergoo.DelegateLoadComplete cb);

			[DllImport(Dll64Name, EntryPoint = "LoadScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadScene_64(bool nativeStr, IntPtr renderProxyInst,
				[MarshalAs(UnmanagedType.Struct)] WebSupergoo.LoadSceneParams loadParams,
				[MarshalAs(UnmanagedType.Struct)] WebSupergoo.ViewParams viewParams, WebSupergoo.DelegateLoadComplete cb);

			[DllImport(Dll32Name, EntryPoint = "CloseScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void CloseScene_32(IntPtr renderProxyInst, WebSupergoo.DelegateUnloadComplete cb);

			[DllImport(Dll64Name, EntryPoint = "CloseScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void CloseScene_64(IntPtr renderProxyInst, WebSupergoo.DelegateUnloadComplete cb);

			[DllImport(Dll32Name, EntryPoint = "IsSceneLoaded", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool IsSceneLoaded_32(IntPtr renderProxyInst);

			[DllImport(Dll64Name, EntryPoint = "IsSceneLoaded", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool IsSceneLoaded_64(IntPtr renderProxyInst);

			[DllImport(Dll32Name, EntryPoint = "InterruptCurrentImport", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void InterruptCurrentImport_32();

			[DllImport(Dll64Name, EntryPoint = "InterruptCurrentImport", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void InterruptCurrentImport_64();

			[DllImport(Dll32Name, EntryPoint = "ExportScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ExportScene_32(bool nativeStr, IntPtr renderProxyInst, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.ExportSceneParams parms, WebSupergoo.DelegateExportComplete cb);

			[DllImport(Dll64Name, EntryPoint = "ExportScene", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ExportScene_64(bool nativeStr, IntPtr renderProxyInst, [MarshalAs(UnmanagedType.Struct)] WebSupergoo.ExportSceneParams parms, WebSupergoo.DelegateExportComplete cb);

#endregion

#region Camera
			// 
			// Camera
			//

			[DllImport(Dll32Name, EntryPoint = "ActivateCamera", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ActivateCamera_32(bool nativeStr, IntPtr renderProxyInst, uint actType, string cameraName, WebSupergoo.DelegateOnActiveCameraChanged cb);

			[DllImport(Dll64Name, EntryPoint = "ActivateCamera", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void ActivateCamera_64(bool nativeStr, IntPtr renderProxyInst, uint actType, string cameraName, WebSupergoo.DelegateOnActiveCameraChanged cb);

			[DllImport(Dll32Name, EntryPoint = "GetAvailableCameras", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void GetAvailableCameras_32(IntPtr renderProxyInst, WebSupergoo.DelegateStrList cb);

			[DllImport(Dll64Name, EntryPoint = "GetAvailableCameras", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void GetAvailableCameras_64(IntPtr renderProxyInst, WebSupergoo.DelegateStrList cb);

			[DllImport(Dll32Name, EntryPoint = "Screenshot", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void Screenshot_32(bool nativeStr, IntPtr renderProxyInst, string filename);

			[DllImport(Dll64Name, EntryPoint = "Screenshot", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void Screenshot_64(bool nativeStr, IntPtr renderProxyInst, string filename);

			[DllImport(Dll32Name, EntryPoint = "LoadViewInfoFromFile", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadViewInfoFromFile_32(bool nativeStr, IntPtr renderProxyInst, string filename, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb);

			[DllImport(Dll64Name, EntryPoint = "LoadViewInfoFromFile", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadViewInfoFromFile_64(bool nativeStr, IntPtr renderProxyInst, string filename, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb);

			[DllImport(Dll32Name, EntryPoint = "LoadViewInfoFromString", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadViewInfoFromString_32(bool nativeStr, IntPtr renderProxyInst, string viewInfo, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb);

			[DllImport(Dll64Name, EntryPoint = "LoadViewInfoFromString", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void LoadViewInfoFromString_64(bool nativeStr, IntPtr renderProxyInst, string viewInfo, bool replaceExisting, WebSupergoo.DelegateOnActiveCameraChanged cb);

#endregion

#region Properties

			//
			// Properties
			//

			[DllImport(Dll32Name, EntryPoint = "SetProperty_Bool", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Bool_32(IntPtr renderProxyInst, uint propID, bool newValue, bool async);

			[DllImport(Dll64Name, EntryPoint = "SetProperty_Bool", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Bool_64(IntPtr renderProxyInst, uint propID, bool newValue, bool async);

			[DllImport(Dll32Name, EntryPoint = "GetProperty_Bool", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool GetProperty_Bool_32(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll64Name, EntryPoint = "GetProperty_Bool", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern bool GetProperty_Bool_64(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll32Name, EntryPoint = "SetProperty_Int32", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Int32_32(IntPtr renderProxyInst, uint propID, int newValue, bool async);

			[DllImport(Dll64Name, EntryPoint = "SetProperty_Int32", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Int32_64(IntPtr renderProxyInst, uint propID, int newValue, bool async);

			[DllImport(Dll32Name, EntryPoint = "GetProperty_Int32", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern int GetProperty_Int32_32(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll64Name, EntryPoint = "GetProperty_Int32", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern int GetProperty_Int32_64(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll32Name, EntryPoint = "SetProperty_Int64", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Int64_32(IntPtr renderProxyInst, uint propID, long newValue, bool async);

			[DllImport(Dll64Name, EntryPoint = "SetProperty_Int64", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Int64_64(IntPtr renderProxyInst, uint propID, long newValue, bool async);

			[DllImport(Dll32Name, EntryPoint = "GetProperty_Int64", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern long GetProperty_Int64_32(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll64Name, EntryPoint = "GetProperty_Int64", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern long GetProperty_Int64_64(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll32Name, EntryPoint = "SetProperty_Double", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Double_32(IntPtr renderProxyInst, uint propID, double newValue, bool async);

			[DllImport(Dll64Name, EntryPoint = "SetProperty_Double", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_Double_64(IntPtr renderProxyInst, uint propID, double newValue, bool async);

			[DllImport(Dll32Name, EntryPoint = "GetProperty_Double", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern double GetProperty_Double_32(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll64Name, EntryPoint = "GetProperty_Double", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern double GetProperty_Double_64(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll32Name, EntryPoint = "SetProperty_String", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_String_32(bool nativeStr, IntPtr renderProxyInst, uint propID, string newValue, bool async);

			[DllImport(Dll64Name, EntryPoint = "SetProperty_String", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			public static extern void SetProperty_String_64(bool nativeStr, IntPtr renderProxyInst, uint propID, string newValue, bool async);

			[DllImport(Dll32Name, EntryPoint = "GetProperty_String", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetProperty_String_32(IntPtr renderProxyInst, uint propID);

			[DllImport(Dll64Name, EntryPoint = "GetProperty_String", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = DllApi)]
			[return: MarshalAs(Static.MarshalStr)]
			public static extern string GetProperty_String_64(IntPtr renderProxyInst, uint propID);

#endregion
		}

#endregion
	}
}
