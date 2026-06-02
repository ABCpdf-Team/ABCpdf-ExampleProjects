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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Glue3DUtilities;

namespace Viewer3D
{


	/// <summary>Interaction logic for MainWindow.xaml</summary>
	public partial class MainWindow : Window
	{
		#region Members
		private Application m_app;
		private Window m_window;
		private List<MenuItem> m_viewMenu = new List<MenuItem>();
		private WebSupergoo.DelegateLogging m_loggingHook = null;
		#endregion

		/// <summary>MainWindow constructor</summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>Handler for window initialized event, to set up various event handlers and logging etc</summary>
		private void Window_Initialized(object sender, EventArgs e)
		{
			// general
			m_app = System.Windows.Application.Current;
			m_window = m_app.MainWindow;

			OpenGLRenderControl.RenderProxyReadyEvent += new Render3DControl.RenderProxyReadyEventHandler(OpenGLRenderControl_RenderProxyReadyEvent);

			OpenGLRenderControl.FileOpeningEvent += new Render3DControl.FileOpeningEventHandler(OpenGLRenderControl_FileOpeningEvent);
			OpenGLRenderControl.FileOpenedEvent += new Render3DControl.FileOpenedEventHandler(OpenGLRenderControl_FileOpenedEvent);
			OpenGLRenderControl.FileClosingEvent += new Render3DControl.FileClosingEventHandler(OpenGLRenderControl_FileClosingEvent);
			OpenGLRenderControl.FileClosedEvent += new Render3DControl.FileClosedEventHandler(OpenGLRenderControl_FileClosedEvent);

			OpenGLRenderControl.FileProgressEvent += new Render3DControl.FileProgressEventHandler(OpenGLRenderControl_FileProgressEvent);

			OpenGLRenderControl.FileExportEvent += new Render3DControl.FileExportEventHandler(OpenGLRenderControl_FileExportEvent);
			OpenGLRenderControl.FileExportedEvent += new Render3DControl.FileExportedEventHandler(OpenGLRenderControl_FileExportedEvent);

			OpenGLRenderControl.LightingSchemeChangedEvent += new Render3DControl.LightingSchemeChangedEventHandler(OpenGLRenderControl_LightingSchemeChangedEvent);

			OpenGLRenderControl.Select3DStreamEvent += new Render3DControl.ContentSelect3DStreamEventHandler(OpenGLRenderControl_OnSelect3DStreamCallback);
			OpenGLRenderControl.SelectCameraEvent += new Render3DControl.CameraSelectEventHandler(OpenGLRenderControl_OnCameraSelectCallback);

			OpenGLRenderControl.LoadingImage = Utilities.GetImageSourceFromResource("Viewer3D", "Images/placeholder.png");

			// logging test (demonstrate logging hook)
			m_loggingHook = new WebSupergoo.DelegateLogging(LogMessageHook);
			Log.SetLoggingHook(IntPtr.Zero, m_loggingHook);

			Log.Info("MainWindow initialized..");
		}

		/// <summary>Handler for RenderProxyReady event, to retrieve various renderer settings and adjust the UI accordingly</summary>
		void OpenGLRenderControl_RenderProxyReadyEvent(object sender, Render3DControl.RenderProxyReadyEventArgs fe)
		{
			Log.Info("OpenGLRenderControl_RenderProxyReadyEvent for '" + fe.Name + "'");

			// write OpenGL version
			Log.Message("Using OpenGL " + OpenGLRenderControl.VersionInfo.version_GL.major + "." + OpenGLRenderControl.VersionInfo.version_GL.minor +
						", and GLSL " + OpenGLRenderControl.VersionInfo.version_GLSL.major + "." + OpenGLRenderControl.VersionInfo.version_GLSL.minor);

			if (OpenGLRenderControl.AvailableAntiAliasingModes != null)
			{
				foreach (UInt32 currMode in OpenGLRenderControl.AvailableAntiAliasingModes)
				{
					UInt32 colSmpl = (currMode & 0x0000FFFF);
					UInt32 covSmpl = (currMode & 0xFFFF0000) >> 16;

					string modeName = colSmpl.ToString() + "xMSAA";
					if (covSmpl > 1)
						modeName += " + " + covSmpl.ToString() + "xCSAA";

					//Log.Info("OpenGLRenderControl_RenderProxyReadyEvent with AA mode '" + modeName + "'");

					MenuItem aaItem = new MenuItem();
					aaItem.Header = modeName;
					aaItem.IsCheckable = true;
					Binding aaBinding = new Binding("AntiAliasing");
					aaBinding.Source = OpenGLRenderControl;
					aaBinding.Converter = new UInt32ToBooleanConverter();
					aaBinding.ConverterParameter = (UInt32)currMode;
					aaItem.SetBinding(MenuItem.IsCheckedProperty, aaBinding);
					AntialiasingMenu.Items.Add(aaItem);
				}
			}

			if (OpenGLRenderControl.AvialableAnisotropyModes != null)
			{
				foreach (UInt32 currMode in OpenGLRenderControl.AvialableAnisotropyModes)
				{
					string anisoName = currMode.ToString() + "x";

					MenuItem anisoItem = new MenuItem();
					anisoItem.Header = anisoName;
					anisoItem.IsCheckable = true;
					Binding aaBinding = new Binding("AnisotropicFiltering");
					aaBinding.Source = OpenGLRenderControl;
					aaBinding.Converter = new UInt32ToBooleanConverter();
					aaBinding.ConverterParameter = (UInt32)currMode;
					anisoItem.SetBinding(MenuItem.IsCheckedProperty, aaBinding);
					AnisotropyMenu.Items.Add(anisoItem);
				}
			}
		}

		/// <summary>Example log message hook for custom logging (print to the window's status line)</summary>
		void LogMessageHook(IntPtr userData, Int32 level, string msg)
		{
			App.Current.Dispatcher.Invoke((Action)delegate
			{
				if (Status_Info != null)
					Status_Info.Text = msg;
			});
		}

		/// <summary>Handler for the FileProgress event, to display the loading progress</summary>
		void OpenGLRenderControl_FileProgressEvent(object sender, Render3DControl.FileProgressEventArgs fe)
		{
			if (fe.Percent < 100.0f)
				Status_Info.Text = string.Format("Loading...{0:F1}%", fe.Percent);
			else
				Status_Info.Text = "Ready";
		}

		/// <summary>Handler for the FileOpening event, sent when it begins loading a file</summary>
		void OpenGLRenderControl_FileOpeningEvent(object sender, Render3DControl.FileOpeningEventArgs fe)
		{
			MainMenu.IsEnabled = false;
		}

		/// <summary>Handler for FileOpened event, when it finished loading a file</summary>
		void OpenGLRenderControl_FileOpenedEvent(object sender, Render3DControl.FileOpenedEventArgs fe)
		{
			MainMenu.IsEnabled = true;

			if (fe.Success)
			{
				CameraChangedCallback(fe.OpenInfo.activeCameraInfo, true);
				LightingSchemeChangedCallback(Utilities.LightingSchemeToString(fe.OpenInfo.lightingScheme), true);

				CameraListReceivedCallback(fe.OpenInfo.availableCameras);
			}
		}

		/// <summary>Handler for FileClosing event, when it starts unloading a file</summary>
		void OpenGLRenderControl_FileClosingEvent(object sender, Render3DControl.FileClosingEventArgs fe)
		{
			MainMenu.IsEnabled = false;

			foreach (MenuItem currItem in m_viewMenu)
				ViewMenu.Items.Remove(currItem);

			m_viewMenu.Clear();
		}

		/// <summary>Handler for FileClosed event, when it finished unloading a file</summary>
		void OpenGLRenderControl_FileClosedEvent(object sender, Render3DControl.FileClosedEventArgs fe)
		{
			MainMenu.IsEnabled = true;
		}

		/// <summary>Handler for FileExport event, when it starts exporting a scene</summary>
		void OpenGLRenderControl_FileExportEvent(object sender, Render3DControl.FileExportEventArgs fe)
		{
			MainMenu.IsEnabled = false;
		}

		/// <summary>Handle for FileExported event, when it finished exporting a scene</summary>
		void OpenGLRenderControl_FileExportedEvent(object sender, Render3DControl.FileExportedEventArgs fe)
		{
			MainMenu.IsEnabled = true;

			if( fe.Success )
			{
				Log.Info("File successfully exported to PRC.");
			}
		}

		/// <summary>Handler for view menu items that have been clicked</summary>
		void ViewItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem item = e.Source as MenuItem;
			string itemText = item.Header as string;
			OpenGLRenderControl.ActivateCamera(itemText, CameraChangedCallback);
		}

		/// <summary>Handler for receiving the list of available cameras</summary>
		private void OnCameraNamesReceived(string[] camNames)
		{
			if (camNames != null)
			{
				for (UInt32 i = 0; i < camNames.Length; ++i)
					Log.Info("Camera " + i + ": " + camNames[i]);
			}
		}

		/// <summary>Handler for WindowClosing event</summary>
		private void Window_Closing(object sender, EventArgs e)
		{
			Log.SetLoggingHook(IntPtr.Zero, null);
		}

		/// <summary>Handler for WindowClosed event</summary>
		private void Window_Closed(object sender, EventArgs e)
		{
			Log.Info("Window_Closed");
		}

		/// <summary>Helper function to display a file dialog for opening a scene, and triggering the actual scene loading</summary>
		private void OpenSceneWithFileDialog()
		{
			string filename = Utilities.Get3DFileDialogResult();
			if (filename.Length > 0)
				OpenGLRenderControl.SceneFilename = filename;
		}

		/// <summary>Helper function to display a file dialog for opening view files (text files containing PDF 3DView annotation info), and triggering the loading of the view file</summary>
		private void LoadViewForScene()
		{
			string filename = Utilities.GetViewFileDialogResult();
			if (filename.Length > 0)
				OpenGLRenderControl.LoadViewFileForScene(filename, CameraChangedCallback);
		}

		private void LoadViewForSceneFromPDF()
		{
			string filename = Utilities.GetPDFFileDialogResult();
			if (filename.Length > 0)
				OpenGLRenderControl.LoadViewFromPDF(filename, CameraChangedCallback);
		}

		/// <summary>Helper function to close the currently open scene</summary>
		private void CloseCurrentScene()
		{
			if (OpenGLRenderControl != null)
				OpenGLRenderControl.SceneFilename = string.Empty;
		}

		/// <summary>Helper for exporting a scene to PRC</summary>
		private void ExportSceneWithFileDialog()
		{
			string sourceFile = string.Empty;
			if (OpenGLRenderControl.SceneFilename.Length == 0)
			{
				sourceFile = Utilities.Get3DFileDialogResult();
				if (sourceFile == null || sourceFile.Length == 0)
					return;
			}

			string filename = Utilities.GetSaveToPRCFileDialogResult();
			if (filename.Length > 0)
			{
				OpenGLRenderControl.ExportSceneToPRC(filename, sourceFile);
			}
		}

		/// <summary>Helper for extracting 3D Data from a PDF file</summary>
		private void Extract3DDataFromPDF()
		{
			string sourceFile = Utilities.GetPDFFileDialogResult();
			if (sourceFile != null)
			{
				if (PdfUtilities.ExtractAll3DDataToFiles(sourceFile))
				{
					Log.Info("Successfully extracted 3D data from '" + sourceFile + "'");
				}
				else
				{
					Log.Error("Failed to extract 3D data from '" + sourceFile + "'");
				}
			}
		}

		/// <summary>Handler for CameraChanged events</summary>
		private void CameraChangedCallback(Render3DControl.CameraInfo camInfo, bool success)
		{
			if (success)
			{
				Status_CameraName.Text = camInfo.cameraName;
				if (camInfo.cameraMode == WebSupergoo.ECameraMode.CameraMode_Target)
					Status_CameraType.Text = "Target";
				else if (camInfo.cameraMode == WebSupergoo.ECameraMode.CameraMode_Free)
					Status_CameraType.Text = "Free";
				else
					Status_CameraType.Text = "Static";
				Status_CameraOrtho.Text = camInfo.isOrtho ? "Ortho" : "";

				AddNewCameraViewItem(camInfo.cameraName);
			}
		}

		/// <summary>Handler for CameraListReceived events</summary>
		private void CameraListReceivedCallback(string[] camNames)
		{
			foreach (string currCam in camNames)
			{
				AddNewCameraViewItem(currCam);
			}
		}

		private void AddNewCameraViewItem(string cameraName)
		{
			// check for duplicates
			foreach (MenuItem currItem in m_viewMenu)
			{
				string itemText = currItem.Header as string;
				if (itemText == cameraName)
					return;
			}

			// add new item
			MenuItem viewItem = new MenuItem();
			viewItem.Header = cameraName;
			viewItem.Click += new RoutedEventHandler(ViewItem_Click);
			ViewMenu.Items.Add(viewItem);
			m_viewMenu.Add(viewItem);
		}

		/// <summary>Handler for Select3DStream events</summary>
		private int OpenGLRenderControl_OnSelect3DStreamCallback(object sender, Render3DControl.ContentSelect3DStreamEventArgs ce)
		{
			if (ce.Content != null )
			{
				if (ce.Content.Count > 1) // more than one item available, let user choose
				{
					List<string> choices = new List<string>();
					for (int i = 0; i < ce.Content.Count; ++i)
						choices.Add(ce.Content[i].Path);

					UserSelection selectWindow = new UserSelection("Select 3D Stream", choices);
					selectWindow.ShowDialog();

					return selectWindow.UserSelectionIndex;
				}
				else // only one item available anyway
					return 0;
			}

			return -1;
		}

		/// <summary>Handler for SelectCamera events</summary>
		private int OpenGLRenderControl_OnCameraSelectCallback(object sender, Render3DControl.CameraSelectEventArgs ce)
		{
			if (ce.Views != null)
			{
				if (ce.Views.Count > 1) // more than one item available, let user choose
				{
					List<string> choices = new List<string>();
					for (int i = 0; i < ce.Views.Count; ++i)
						choices.Add(ce.Views[i].Path);

					UserSelection selectWindow = new UserSelection("Select 3D View", choices);
					selectWindow.ShowDialog();

					return selectWindow.UserSelectionIndex;
				}
				else // only one item available anyway
					return 0;
			}

			return -1;
		}

		/// <summary>Handler for LightingSchemeChanged events</summary>
		void OpenGLRenderControl_LightingSchemeChangedEvent(object sender, Render3DControl.LightingSchemeChangedEventArgs fe)
		{
			Status_LightScheme.Text = Utilities.LightingSchemeToString(fe.LightingScheme);
		}

		/// <summary>Callback for changed lighting schemes</summary>
		private void LightingSchemeChangedCallback(string lightingSchemeName, bool success)
		{
			if (success)
			{
				Status_LightScheme.Text = lightingSchemeName;
			}
		}

		/// <summary>Helper function to open the About window</summary>
		private void OpenAboutWindow()
		{
			Renderer.GLVersionInfo v = OpenGLRenderControl.VersionInfo;
			About aboutWindow = new About(v);
			aboutWindow.ShowDialog();
		}

		/// <summary>Helper function to handle menu item clicks</summary>
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem item = e.Source as MenuItem;

			switch (item.Name)
			{
				case "File_Open":
					OpenSceneWithFileDialog();
					break;
				case "File_Close":
					CloseCurrentScene();
					break;
				case "File_Export":
					ExportSceneWithFileDialog();
					break;
				case "File_Extract":
					Extract3DDataFromPDF();
					break;
				case "File_Quit":
					Application.Current.Shutdown();
					break;
				case "View_NextCamera":
					OpenGLRenderControl.ActivateNextCamera(CameraChangedCallback);
					break;
				case "View_FirstCamera":
					OpenGLRenderControl.ActivateFirstCamera(CameraChangedCallback);
					break;
				case "View_DefaultCamera":
					OpenGLRenderControl.ActivateDefaultCamera(CameraChangedCallback);
					break;
				case "View_Screenshot":
					OpenGLRenderControl.Screenshot(string.Empty);
					break;
				case "View_LoadFromFile":
					LoadViewForScene();
					break;
				case "View_LoadFromPDF":
					LoadViewForSceneFromPDF();
					break;
				case "Help_About":
					OpenAboutWindow();
					break;
				default:
					Log.Error("MenuItem_Click: Invalid item name: " + item.Name);
					break;
			}
		}

	}
}
