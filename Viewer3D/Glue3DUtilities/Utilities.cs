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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.ComponentModel;


namespace Glue3DUtilities
{
	[System.Security.SecuritySafeCritical]
	internal static class SafeUtils {
		public static GCHandle GCHandle_Alloc(object v, GCHandleType type) {
			return GCHandle.Alloc(v, type);
		}
		public static void GCHandle_Free(ref GCHandle handle) {
			handle.Free();
		}
		public static IntPtr GCHandle_AddrOfPinnedObject(ref GCHandle handle) {
			return handle.AddrOfPinnedObject();
		}
	}

	/// <summary>Helper class to represent version/summary>
	public struct Version
	{
		public int major;
		public int minor;
		public int build;

		public Version(int _major, int _minor, int _build)
		{
			major = _major;
			minor = _minor;
			build = _build;
		}

		public override string ToString()
		{
			return major.ToString() + "." + minor.ToString() + "." + build.ToString();
		}
	}

	/// <summary>Various static utilites</summary>
	public static class Utilities
	{
		/// <summary>Allow setting the company name (will be used when writing to APPDATA)</summary>
		public static void SetCompanyName(string companyName)
		{
			Dll_Glue3D.SetCompanyName(companyName);
		}

		/// <summary>Get the currently set company name</summary>
		public static string GetCompanyName()
		{
			return Dll_Glue3D.GetCompanyName();
		}

		/// <summary>Allow setting the application name (will be used when writing to APPDATA)</summary>
		public static void SetApplicationName(string companyName)
		{
			Dll_Glue3D.SetApplicationName(companyName);
		}

		/// <summary>Get the currently set application name</summary>
		public static string GetApplicationName()
		{
			return Dll_Glue3D.GetApplicationName();
		}

		/// <summary>Set the HiWord of an Int32</summary>
		public static Int32 HiWord(Int32 number)
		{
			if ((number & 0x80000000) == 0x80000000)
				return (number >> 16);
			else
				return (number >> 16) & 0xffff;
		}

		/// <summary>Set the LoWord of an Int32</summary>
		public static Int32 LoWord(Int32 number)
		{
			return (number & 0xffff);
		}

		/// <summary>Make a DWord from the provided HiWord and LoWord</summary>
		public static Int32 MakeDword(Int32 LoWord, Int32 HiWord)
		{
			return (HiWord << 16) | (LoWord & 0xffff);
		}

		/// <summary>Make a PARAM (IntPtr of a Dword)</summary>
		public static IntPtr MakeParam(Int32 LoWord, Int32 HiWord)
		{
			return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
		}

		/// <summary>Convenience function to get the name of the lighting scheme as a string</summary>
		public static string LightingSchemeToString(WebSupergoo.ELightingScheme scheme)
		{
			string outStr = "";
			switch (scheme)
			{
				case WebSupergoo.ELightingScheme.LightingScheme_None:
					outStr = "None"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Artwork:
					outStr = "Artwork"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_White:
					outStr = "White"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Day:
					outStr = "Day"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Night:
					outStr = "Night"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Hard:
					outStr = "Hard"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Primary:
					outStr = "Primary"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Blue:
					outStr = "Blue"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Red:
					outStr = "Red"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Cube:
					outStr = "Cube"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_CAD:
					outStr = "CAD"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Headlamp:
					outStr = "Headlamp"; break;
				case WebSupergoo.ELightingScheme.LightingScheme_Custom:
					outStr = "Custom"; break;
				default:
					outStr = "<Invalid>"; break;
			}
			return outStr;
		}

		/// <summary>Create an open file dialog to ask the user to choose a file to open</summary>
		public static string GetOpenFileDialogResult(string title, string defaultExt, string filter)
		{
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
			openFileDlg.Title = title;
			openFileDlg.DefaultExt = defaultExt;
			openFileDlg.Filter = filter;

			// Show open file dialog box
			Nullable<bool> result = openFileDlg.ShowDialog();

			// Process open file dialog box results
			if (result == true)
				return openFileDlg.FileName;

			return string.Empty;
		}

		/// <summary>Create an save file dialog to ask the user to choose a file to save to</summary>
		public static string GetSaveFileDialogResult(string title, string defaultExt, string filter)
		{
			Microsoft.Win32.SaveFileDialog saveFileDlg = new Microsoft.Win32.SaveFileDialog();
			saveFileDlg.Title = title;
			saveFileDlg.DefaultExt = defaultExt;
			saveFileDlg.Filter = filter;

			// Show save file dialog box
			Nullable<bool> result = saveFileDlg.ShowDialog();

			// Process open file dialog box results
			if (result == true)
				return saveFileDlg.FileName;

			return string.Empty;
		}

		/// <summary>Convenience function to open a file dialog that allows opening a 3D file type</summary>
		public static string Get3DFileDialogResult()
		{
			return GetOpenFileDialogResult("Open 3D File", ".pdf;.u3d;.prc;.obj", "3D Files|*.pdf;*.u3d;*.prc;*.obj");
		}

		/// <summary>Convenience function to open a file dialog that allows opening a view file</summary>
		public static string GetViewFileDialogResult()
		{
			return GetOpenFileDialogResult("Open ViewInfo File", ".txt", "Text Files (.txt)|*.txt");
		}

		/// <summary>Convenience function to open a file dialog that allows opening a PDF file</summary>
		public static string GetPDFFileDialogResult()
		{
			return GetOpenFileDialogResult("Open PDF File", ".pdf", "PDF Files (.pdf)|*.pdf");
		}

		/// <summary>Convenience function to open a file dialog that allows saving to PRC file type</summary>
		public static string GetSaveToPRCFileDialogResult()
		{
			return GetSaveFileDialogResult("Save to PRC file", ".prc", "PRC files (.prc)|*.prc");
		}

		/// <summary>Retrieve an Image from an assembly</summary>
		public static ImageSource GetImageSourceFromResource(string assemblyName, string resourceName)
		{
			try
			{
				Uri oUri = new Uri("pack://application:,,,/" + assemblyName + ";component/" + resourceName, UriKind.RelativeOrAbsolute);
				return BitmapFrame.Create(oUri);
			}
			catch (IOException)
			{
				return null;
			}
		}

		/// <summary>Retrieve an Image from a file</summary>
		public static ImageSource GetImageSourceFromFile(string filename)
		{
			if (!File.Exists(filename))
				return null;

			try
			{
				return BitmapFrame.Create(new Uri(filename));
			}
			catch (IOException)
			{
				return null;
			}
		}

		/// <summary>Convert a Color to an UInt32</summary>
		public static UInt32 Color2Int(System.Windows.Media.Color inColor)
		{
			UInt32 col = 0xFF000000;
			col |= (((UInt32)inColor.R << 16) | ((UInt32)inColor.G << 8) | ((UInt32)inColor.B));
			return col;
		}

		/// <summary>Convert an UInt32 to a Color</summary>
		public static System.Windows.Media.Color UInt2Color(UInt32 inColor)
		{
			System.Windows.Media.Color bgCol = new System.Windows.Media.Color();
			bgCol.R = (byte)((inColor & 0x00FF0000) >> 16);
			bgCol.G = (byte)((inColor & 0x0000FF00) >> 8);
			bgCol.B = (byte)(inColor & 0x000000FF);
			bgCol.A = 0xFF;

			return bgCol;
		}

		/// <summary>Set the value of a property and send a notification upon change</summary>
		public static bool SetPropertyAndNotify<T>(PropertyChangedEventHandler changeHandler, ref T field, T value, System.Linq.Expressions.Expression<Func<T>> expr)
		{
			if (expr == null)
			{
				throw new ArgumentNullException("ChangePropertyAndNotify - Missing expression");
			}
			var body = expr.Body as MemberExpression;
			if (body == null)
			{
				throw new ArgumentException("ChangePropertyAndNotify - Expression missing body");
			}
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}

			field = value;

			var vmExpr = body.Expression as ConstantExpression;
			if (vmExpr != null)
			{
				LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(vmExpr);
				Delegate vmFunc = lambda.Compile();
				object sender = vmFunc.DynamicInvoke();

				if (changeHandler != null)
				{
					changeHandler(sender, new PropertyChangedEventArgs(body.Member.Name));
				}
			}

			return true;
		}

		/// <summary>Helper for comparing versions</summary>
		public static bool IsVersionOk(Version minVersion, Version currVersion, bool exactSame = false, bool compareBuild = false)
		{
			// if exactSame, means all the build numbers must be exactly the same
			if (exactSame)
			{
				if (currVersion.major != minVersion.major || currVersion.minor != minVersion.minor)
					return false;

				if (compareBuild && currVersion.build != minVersion.build)
					return false;

				return true;
			}
			// otherwise a newer build will be accepted 
			else
			{
				// check fail on major version
				if (currVersion.major < minVersion.major)
					return false;
				else if (currVersion.major > minVersion.major)
					return true;

				// major version same, check minor
				if (currVersion.minor < minVersion.minor)
					return false;
				else if (currVersion.minor > minVersion.minor)
					return true;

				// minor version same, check build
				if (compareBuild && currVersion.build < minVersion.build)
					return false;

				return true;
			}
		}

		/// <summary>Output the pinned handles of ViewData strings</summary>
		public static void PinViews(IList<PdfUtilities.ViewInfo> list, out GCHandle[] outHandles) {
			if(list == null) {
				outHandles = null;
				return;
			}
			outHandles = new GCHandle[list.Count];
			for (int i = 0; i < list.Count; ++i) {
				string s = list[i].View;
				if (s != null)
					outHandles[i] = SafeUtils.GCHandle_Alloc(s, GCHandleType.Pinned);
			}
		}
	}
}
