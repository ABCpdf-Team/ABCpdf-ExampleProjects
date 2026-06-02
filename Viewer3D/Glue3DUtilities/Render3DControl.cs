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
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Glue3DUtilities
{
	#region Info
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:Glue3DUtilities"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:Glue3DUtilities;assembly=Glue3DUtilities"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:Render3DControl/>
	///
	/// </summary>
	///
	#endregion


	/// <summary>A control that allows rendering 3D models</summary>
	public class Render3DControl : Control, INotifyPropertyChanged
	{
		#region MemberStructs
		/// <summary>Describes a camera by it's key properties</summary>
		public struct CameraInfo
		{
			/// <summary>Name of the camera</summary>
			public string cameraName;
			/// <summary>The mode of the camera (how it can move)</summary>
			public WebSupergoo.ECameraMode cameraMode;
			/// <summary>Whether the camera uses orthogonal or perspective projection</summary>
			public bool isOrtho;
			/// <summary>Whether the camera can be modified</summary>
			public bool isModifiable;
		}
		/// <summary>Info on a newly opened file</summary>
		public struct FileOpenInfo
		{
			/// <summary>The name of the file that the scene originates from</summary>
			public string originalFilename;
			/// <summary>The scene handle that the renderer uses internally</summary>
			public UInt32 sceneHandle;
			/// <summary>The list of available cameras (by name)</summary>
			public string[] availableCameras;
			/// <summary>Information about the currently active camera</summary>
			public CameraInfo activeCameraInfo;
			/// <summary>The lighting scheme that is used for the scene</summary>
			public WebSupergoo.ELightingScheme lightingScheme;
		}
		#endregion

		#region Delegates
		/// <summary>Delegate which gets called when the camera changed</summary>
		public delegate void DelegateCameraChanged(CameraInfo camInfo, bool success);
		/// <summary>Delegate the gets called when the list of available cameras is received</summary>
		public delegate void DelegateCameraListReceived(string[] camNames);
		#endregion

		#region MemberVariables

		#region PrivateMembers
		private Border m_borderElement = null;
		private Render3DHost m_renderHost = null;
		private System.Windows.Controls.Image m_placeholderImage = null;
		private ImageSource m_placeholderImageSource = null;
		private IntPtr m_renderProxy = IntPtr.Zero;
		private bool m_isInDesignMode = false;
		private string m_currentSceneFilename = null;
		private UInt32 m_currentSceneHandle = 0;
		private bool m_hasCSAASupport = false;
		private WebSupergoo.DelegateOnActiveCameraChanged m_currentCamDelegate = null;
		private WebSupergoo.DelegateStrList m_currentCamListDelegate = null;
		private WebSupergoo.DelegateLoadComplete m_loadCompleteDelegate = null;
		private WebSupergoo.DelegateUnloadComplete m_unloadCompleteDelegate = null;
		private WebSupergoo.DelegateLoadProgress m_loadProgressDelegate = null;
		private WebSupergoo.DelegateExportComplete m_exportCompleteDelegate = null;
		private bool m_loadViewFilesOnSceneOpen = true;
		private Renderer.GLVersionInfo m_versionInfo = new Renderer.GLVersionInfo();
		#endregion

		#region PublicMembers
		/// <summary>Eventhandler for property changed events</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Whether Coverage Sampled Antialiasing (CSAA) is availabe (Note: Only on Nvidia graphics cards)</summary>
		public bool HasCSAASupport 
		{
			get { return m_hasCSAASupport; }
			private set { Utilities.SetPropertyAndNotify(this.PropertyChanged, ref m_hasCSAASupport, value, () => HasCSAASupport); }
		}

		/// <summary>The list of available antialiasing modes (each AA mode described by an UInt32, with the lower word representing the color samples, and higher word the  coverage samples</summary>
		public UInt32[] AvailableAntiAliasingModes { get; private set; }

		/// <summary>The list of available Anisotropy modes (affects image quality of textures)</summary>
		public UInt32[] AvialableAnisotropyModes { get; private set; }

		/// <summary>Whether view files (text files containing PDF view annotations (3DV)) should be loaded upon scene opening (if available next to the scene file)</summary>
		public bool LoadViewFilesOnSceneOpen
		{
			get { return m_loadViewFilesOnSceneOpen; }
			private set { Utilities.SetPropertyAndNotify(this.PropertyChanged, ref m_loadViewFilesOnSceneOpen, value, () => LoadViewFilesOnSceneOpen); }
		}

		/// <summary>Access the version info of OpenGL and GLSL (will only return actual values after the renderer was initialized)</summary>
		public Renderer.GLVersionInfo VersionInfo
		{
			get { return m_versionInfo; }
			private set { m_versionInfo = value; }
		}

		#endregion

		#endregion

		#region CustomEvents

		#region RenderProxyReadyEvent
		/// <summary>Event arguments for RenderProxyReady event</summary>
		public class RenderProxyReadyEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public RenderProxyReadyEventArgs(string proxyName)
			{
				this.Name = proxyName;
			}

			/// <summary>Name of the RenderProxy instance</summary>
			public string Name { get; private set; }
		}

		/// <summary>Delegate for RenderProxyReady event</summary>
		public delegate void RenderProxyReadyEventHandler(object sender, RenderProxyReadyEventArgs fe);
		/// <summary>RenderProxyReady event, fired when the render proxy was initialized</summary>
		public event RenderProxyReadyEventHandler RenderProxyReadyEvent;
		#endregion

		#region FileOpeningEvent

		/// <summary>Event arguments for FileOpening event</summary>
		public class FileOpeningEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileOpeningEventArgs(string filename)
			{
				this.Filename = filename;
			}

			/// <summary>Name of the file to be opened</summary>
			public string Filename { get; private set; }
		}
		/// <summary>Delegate for FileOpening event</summary>
		public delegate void FileOpeningEventHandler(object sender, FileOpeningEventArgs fe);
		/// <summary>FileOpening event, fired when it starts loading a scene</summary>
		public event FileOpeningEventHandler FileOpeningEvent;
		#endregion

		#region FileOpenedEvent

		/// <summary>Event arguments for FileOpened event</summary>
		public class FileOpenedEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileOpenedEventArgs(FileOpenInfo openInfo, bool success)
			{
				this.OpenInfo = openInfo;
				this.Success = success;
			}

			/// <summary>Extra info about the opened file</summary>
			public FileOpenInfo OpenInfo { get; private set; }
			/// <summary>Whether the file was opened successfully</summary>
			public bool Success { get; private set; }
		}
		/// <summary>Delegate for FileOpened event</summary>
		public delegate void FileOpenedEventHandler(object sender, FileOpenedEventArgs fe);
		/// <summary>FileOpened event, fired when it finished loading a scene</summary>
		public event FileOpenedEventHandler FileOpenedEvent;
		#endregion

		#region FileClosingEvent
		/// <summary>Event arguments for FileClosing event</summary>
		public class FileClosingEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileClosingEventArgs(string filename)
			{
				this.Filename = filename;
			}

			/// <summary>Name of the file to be closed</summary>
			public string Filename { get; private set; }
		}
		/// <summary>Delegate for FileClosing event</summary>
		public delegate void FileClosingEventHandler(object sender, FileClosingEventArgs fe);
		/// <summary>FileOpened event, fired when it starts unloading a scene</summary>
		public event FileClosingEventHandler FileClosingEvent;
		#endregion

		#region FileClosedEvent
		/// <summary>Event arguments for FileClosed event</summary>
		public class FileClosedEventArgs : EventArgs
		{
		}
		/// <summary>Delegate for FileClosed event</summary>
		public delegate void FileClosedEventHandler(object sender, FileClosedEventArgs fe);
		/// <summary>FileClosed event, fired when it finished unloading a scene</summary>
		public event FileClosedEventHandler FileClosedEvent;
		#endregion

		#region FileProgressEvent
		/// <summary>Event arguments for FileProgress event</summary>
		public class FileProgressEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileProgressEventArgs(float percent)
			{
				this.Percent = percent;
			}

			/// <summary>The current percentage of the loading process</summary>
			public float Percent { get; private set; }
		}
		/// <summary>Delegate for FileProgress event</summary>
		public delegate void FileProgressEventHandler(object sender, FileProgressEventArgs fe);
		/// <summary>FileProgress event, fired many times during the loading process of a file</summary>
		public event FileProgressEventHandler FileProgressEvent;
		#endregion

		#region FileExportEvent

		/// <summary>Event arguments for FileExport event</summary>
		public class FileExportEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileExportEventArgs(string filename)
			{
				this.Filename = filename;
			}

			/// <summary>Name of the target file to be exported to</summary>
			public string Filename { get; private set; }
		}
		/// <summary>Delegate for FileExport event</summary>
		public delegate void FileExportEventHandler(object sender, FileExportEventArgs fe);
		/// <summary>FileExport event, fired when an export is started</summary>
		public event FileExportEventHandler FileExportEvent;

		#endregion

		#region FileExportedEvent

		/// <summary>Event arguments for FileExported event</summary>
		public class FileExportedEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public FileExportedEventArgs(WebSupergoo.ExportSceneResult exportResult, bool success)
			{
				SceneHandle = exportResult.sceneHandle;
				if(exportResult.exportData != null && exportResult.exportDataSize > 0) {
					ExportData = new byte[exportResult.exportDataSize];
					Marshal.Copy(exportResult.exportData, ExportData, 0, (int)exportResult.exportDataSize);
				}
				UserData = exportResult.userData;
				Success = success;
			}

			/// <summary>Handle of scene that was exported (just for reference)</summary>
			public uint SceneHandle { get; private set; }
			/// <summary>The export data (if exportFilename is not set, it will use exportData and exportDataSize to write the data to)</summary>
			public byte[] ExportData { get; private set; }
			/// <summary>The user data that was passed in</summary>
			public IntPtr UserData { get; private set; }
			/// <summary>Whether the scene was successfully exported</summary>
			public bool Success { get; private set; }
		}
		/// <summary>Delegate for FileExported event</summary>
		public delegate void FileExportedEventHandler(object sender, FileExportedEventArgs fe);
		/// <summary>FileExported event, fired when an export has finished</summary>
		public event FileExportedEventHandler FileExportedEvent;

		#endregion

		#region LightingSchemeChangedEvent
		/// <summary>Event arguments for LightingSchemeChanged event</summary>
		public class LightingSchemeChangedEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public LightingSchemeChangedEventArgs(WebSupergoo.ELightingScheme newScheme)
			{
				this.LightingScheme = newScheme;
			}

			/// <summary>The lighting scheme in use now</summary>
			public WebSupergoo.ELightingScheme LightingScheme { get; private set; }
		}
		/// <summary>Delegate for LightingSchemeChanged event</summary>
		public delegate void LightingSchemeChangedEventHandler(object sender, LightingSchemeChangedEventArgs fe);
		/// <summary>LightingSchemeChanged event, fired when a lighting scheme has changed</summary>
		public event LightingSchemeChangedEventHandler LightingSchemeChangedEvent;
		#endregion

		#region ContentSelectEvent

		/// <summary>Event arguments for ContentSelect3DStream event</summary>
		public class ContentSelect3DStreamEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public ContentSelect3DStreamEventArgs(List<PdfUtilities.StreamInfo> available3DStreams)
			{
				this.Content = available3DStreams;
			}

			/// <summary>The available 3D content (D3DStreams)</summary>
			public List<PdfUtilities.StreamInfo> Content { get; private set; }
		}
		/// <summary>Delegate for ContentSelect3DStream event</summary>
		public delegate int ContentSelect3DStreamEventHandler(object sender, ContentSelect3DStreamEventArgs ce);
		/// <summary>ContentSelectEvent event, fired when trying to open a scene with more than one 3DStream available</summary>
		public event ContentSelect3DStreamEventHandler Select3DStreamEvent;

		#endregion

		#region CameraSelectEvent

		/// <summary>Event arguments for CameraSelectEvent event</summary>
		public class CameraSelectEventArgs : EventArgs
		{
			/// <summary>Constructor</summary>
			public CameraSelectEventArgs(List<PdfUtilities.ViewInfo> available3DViews)
			{
				this.Views = available3DViews;
			}

			/// <summary>The available 3D view info</summary>
			public List<PdfUtilities.ViewInfo> Views { get; private set; }
		}
		/// <summary>Delegate for CameraSelect event</summary>
		public delegate int CameraSelectEventHandler(object sender, CameraSelectEventArgs ce);
		/// <summary>CameraSelect event, fired when it starts loading a scene which has more than one view info available</summary>
		public event CameraSelectEventHandler SelectCameraEvent;

		#endregion

		#endregion

		#region Properties

		#region DependencyProperties

		/// <summary>DependencyProperty for the lighting scheme</summary>
		public static readonly DependencyProperty LightingSchemeProperty =
                DependencyProperty.Register("LightingScheme",
                                             typeof(WebSupergoo.ELightingScheme),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(WebSupergoo.ELightingScheme.LightingScheme_CAD,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnLightingSchemePropertyChanged));

		/// <summary>DependencyProperty for the antialiasing mode</summary>
		public static readonly DependencyProperty AntiAliasingProperty =
                DependencyProperty.Register("AntiAliasing",
                                             typeof(UInt32),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata((UInt32)0,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnAntiAliasingPropertyChanged));

		/// <summary>DependencyProperty for the anisotropic filtering mode</summary>
		public static readonly DependencyProperty AnisotropyProperty =
                DependencyProperty.Register("AnisotropicFiltering",
                                             typeof(UInt32),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata((UInt32)0,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnAnisotropyPropertyChanged));

		/// <summary>DependencyProperty for the scene filename</summary>
		public static readonly DependencyProperty SceneFilenameProperty =
                DependencyProperty.Register("SceneFilename",
                                             typeof(string),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(string.Empty,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnSceneFilenamePropertyChanged));

		/// <summary>DependencyProperty for the loading image</summary>
		public static readonly DependencyProperty LoadingImageProperty =
                DependencyProperty.Register("LoadingImage",
                                             typeof(ImageSource),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(null,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnLoadingImagePropertyChanged));

		/// <summary>DependencyProperty for the background color</summary>
		public static readonly DependencyProperty BackgroundColorProperty =
                DependencyProperty.Register("BackgroundColor",
                                             typeof(System.Windows.Media.Color),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(System.Windows.Media.Color.FromRgb(0, 0, 0),
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnBackgroundColorPropertyChanged));

		/// <summary>DependencyProperty for whether to show the world axis</summary>
		public static readonly DependencyProperty ShowWorldAxisProperty =
                DependencyProperty.Register("ShowWorldAxis",
                                             typeof(bool),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(false,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnShowWorldAxisPropertyChanged));

		/// <summary>DependencyProperty for whether to show the world grid</summary>
		public static readonly DependencyProperty ShowWorldGridProperty =
                DependencyProperty.Register("ShowWorldGrid",
                                             typeof(bool),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(false,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnShowWorldGridPropertyChanged));

		/// <summary>DependencyProperty for whether to show the scene enclosing bounding box</summary>
		public static readonly DependencyProperty ShowSceneBBoxProperty =
                DependencyProperty.Register("ShowSceneBBox",
                                             typeof(bool),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(false,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnShowSceneBBoxPropertyChanged));

		/// <summary>DependencyProperty for whether to show individual model bounding boxes</summary>
		public static readonly DependencyProperty ShowModelBBoxesProperty =
                DependencyProperty.Register("ShowModelBBoxes",
                                             typeof(bool),
                                             typeof(Render3DControl),
                                             new FrameworkPropertyMetadata(false,
                                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                            OnShowModelBBoxesPropertyChanged));

		#endregion

		/// <summary>Sets the lighting scheme</summary>
		[Description("Sets the lighting scheme"),
		Category("3D Settings")]
		public WebSupergoo.ELightingScheme LightingScheme
		{
			get { return (WebSupergoo.ELightingScheme)GetValue(LightingSchemeProperty); }
			set { SetValue(LightingSchemeProperty, value); }
		}

		/// <summary>Sets the antialiasing mode for the frame buffoer object (FBO) that is being rendered to (packed UInt32)</summary>
		[Description("Sets the antialiasing mode for the FBO that is being rendered to (packed UInt32)"),
		Category("3D Settings")]
		public UInt32 AntiAliasing
		{
			get { return (UInt32)GetValue(AntiAliasingProperty); }
			set { SetValue(AntiAliasingProperty, value); }
		}

		/// <summary>Sets the anisotropic filtering mode</summary>
		[Description("Sets the anisotropic filtering for the scene"),
		Category("3D Settings")]
		public UInt32 AnisotropicFiltering
		{
			get { return (UInt32)GetValue(AnisotropyProperty); }
			set { SetValue(AnisotropyProperty, value); }
		}

		/// <summary>Sets the scene filename (to load a new scene, if empty will unload the current scene)</summary>
		[Description("Sets the scene to be loaded, or empty to unload a scene"),
		Category("3D Settings")]
		public string SceneFilename
		{
			get { return (string)GetValue(SceneFilenameProperty); }
			set { SetValue(SceneFilenameProperty, value); }
		}

		/// <summary>Sets the image to be displayed while un/loading scenes</summary>
		[Description("Sets the image to be displayed while un/loading scenes"),
		Category("3D Settings")]
		public ImageSource LoadingImage
		{
			get { return (ImageSource)GetValue(LoadingImageProperty); }
			set { SetValue(LoadingImageProperty, value); }
		}

		/// <summary>Sets the background color of a scene</summary>
		[Description("Sets the background color to be applied to a scene"), Category("3D Settings")]
		public System.Windows.Media.Color BackgroundColor
		{
			get { return (System.Windows.Media.Color)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		/// <summary>Sets the visibility of the world axis</summary>
		[Description("Sets Visibility of World Axis"), Category("3D Settings")]
		public bool ShowWorldAxis
		{
			get { return (bool)GetValue(ShowWorldAxisProperty); }
			set { SetValue(ShowWorldAxisProperty, value); }
		}

		/// <summary>Sets the visibility of the world grid</summary>
		[Description("Sets Visibility of World Grid"), Category("3D Settings")]
		public bool ShowWorldGrid
		{
			get { return (bool)GetValue(ShowWorldGridProperty); }
			set { SetValue(ShowWorldGridProperty, value); }
		}

		/// <summary>Sets the visibility of the scene enclosing bounding box</summary>
		[Description("Sets Visibility of Scene Bounding Box"), Category("3D Settings")]
		public bool ShowSceneBBox
		{
			get { return (bool)GetValue(ShowSceneBBoxProperty); }
			set { SetValue(ShowSceneBBoxProperty, value); }
		}

		/// <summary>Sets the visibility of bounding boxes for the individual models</summary>
		[Description("Sets Visibility of individual model Bounding Boxes"), Category("3D Settings")]
		public bool ShowModelBBoxes
		{
			get { return (bool)GetValue(ShowModelBBoxesProperty); }
			set { SetValue(ShowModelBBoxesProperty, value); }
		}

		#endregion

		#region StaticConstructor

		/// <summary>Static constructor to override the control metadata</summary>
		static Render3DControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Render3DControl), new FrameworkPropertyMetadata(typeof(Render3DControl)));
		}

		#endregion

		#region ControlRelated

		/// <summary>Override the OnApplyTemplage of the Control base class, to set up various values and handlers</summary>
		public override void OnApplyTemplate()
		{
			if (DesignerProperties.GetIsInDesignMode(this))
				m_isInDesignMode = true;

			m_borderElement = GetTemplateChild("RenderHostBorder") as Border;

			this.AllowDrop = true;
			this.Drop += new DragEventHandler(Render3DControl_Drop);
			this.Loaded += new RoutedEventHandler(OnControlLoaded);

			base.OnApplyTemplate();
		}

		/// <summary>Handle file drop events</summary>
		void Render3DControl_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

				// debug
				for (int i = 0; i < filenames.Length; ++i)
					Log.Info("Dropped file into render control: " + filenames[i]);

				// only open first one
				if (filenames.Length > 0)
				{
					string ext = System.IO.Path.GetExtension(filenames[0]).ToLower();
					SceneFilename = filenames[0];
				}
			}
		}

		/// <summary>Helper function to create a placeholder image object</summary>
		private void CreatePlaceholderImageObject()
		{
			if (m_placeholderImage == null)
			{
				m_placeholderImage = new System.Windows.Controls.Image();
				m_placeholderImage.Width = m_borderElement.ActualWidth;
				m_placeholderImage.Height = m_borderElement.ActualHeight;
			}
		}

		/// <summary>Handle the Loaded event of the control</summary>
		private void OnControlLoaded(object sender, RoutedEventArgs e)
		{
			// set up border element
			m_borderElement.Background = System.Windows.Media.Brushes.Black;
			m_borderElement.BorderBrush = System.Windows.Media.Brushes.Yellow;

			// prepare placeholder image
			CreatePlaceholderImageObject();

			// in design mode set a background image
			if (m_isInDesignMode)
			{
				m_borderElement.Child = m_placeholderImage;

				LoadScene(SceneFilename);
			}
			// otherwise load the real view
			else
			{
				m_renderHost = new Render3DHost(m_borderElement.ActualWidth, m_borderElement.ActualHeight);
				if (m_renderHost != null)
				{
					m_borderElement.Child = m_renderHost;
					m_renderProxy = m_renderHost.RenderProxy;

					// get properties                    
					HasCSAASupport = (m_renderHost.ProxyFlags & (UInt32)WebSupergoo.ERenderProxyFlags.RenderProxyFlags_SupportsCSAA) != 0;
					AntiAliasing = ((UInt32)m_renderHost.AntialiasingCoverageSamples << 16) | (UInt32)m_renderHost.AntialiasingColorSamples;
					if (m_renderHost.AvailableAAModes != null && m_renderHost.AvailableAAModes.Length > 0)
					{
						AvailableAntiAliasingModes = new UInt32[m_renderHost.AvailableAAModes.Length];
						Array.Copy(m_renderHost.AvailableAAModes, AvailableAntiAliasingModes, m_renderHost.AvailableAAModes.Length);
					}
					AnisotropicFiltering = m_renderHost.Anisotropy;
					if (m_renderHost.AnisotropyModes != null && m_renderHost.AnisotropyModes.Length > 0)
					{
						AvialableAnisotropyModes = new UInt32[m_renderHost.AnisotropyModes.Length];
						Array.Copy(m_renderHost.AnisotropyModes, AvialableAnisotropyModes, m_renderHost.AnisotropyModes.Length);
					}
					m_versionInfo = Renderer.GetVersionInfo(m_renderProxy);

					// apply properties
					SetBackgroundColor(BackgroundColor);

					// load scene
					LoadScene(SceneFilename);

					// fire event
					RenderProxyReadyEventArgs proxyArgs = new RenderProxyReadyEventArgs(m_renderHost.ProxyName);
					RenderProxyReadyEvent(this, proxyArgs);
				}
			}
		}

		#endregion

		#region PropertyChangedCallbacks

		/// <summary>Callback when the lighting scheme changed</summary>
		private static void OnLightingSchemePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			WebSupergoo.ELightingScheme lightScheme = (WebSupergoo.ELightingScheme)e.NewValue;

			r3DControl.SetLightingScheme(lightScheme);
		}

		/// <summary>Callback when the antialiasing mode changed</summary>
		private static void OnAntiAliasingPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			UInt32 packedAAMode = (UInt32)e.NewValue;

			r3DControl.SetAntialiasingMode(packedAAMode);
		}

		/// <summary>Callback when the anisotropic filtering mode changed</summary>
		private static void OnAnisotropyPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			UInt32 anisotropy = (UInt32)e.NewValue;

			r3DControl.SetAnisotropicFiltering(anisotropy);
		}

		/// <summary>Callback when the scene filename property changed, to trigger un/loading a scene</summary>
		private static void OnSceneFilenamePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			string newFilename = (string)e.NewValue;

			r3DControl.LoadScene(newFilename);
		}

		/// <summary>Callback when the loading image changed</summary>
		private static void OnLoadingImagePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.m_placeholderImageSource = (ImageSource)e.NewValue;
		}

		/// <summary>Callback when the background color changed</summary>
		private static void OnBackgroundColorPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.SetBackgroundColor((System.Windows.Media.Color)e.NewValue);
		}

		/// <summary>Callback when the show world axis property changed</summary>
		private static void OnShowWorldAxisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.SetShowWorldAxis((bool)e.NewValue);
		}

		/// <summary>Callback when the show world grid property changed</summary>
		private static void OnShowWorldGridPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.SetShowWorldGrid((bool)e.NewValue);
		}

		/// <summary>Callback when the property for showing the scene enclosing bounding box changed</summary>
		private static void OnShowSceneBBoxPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.SetShowSceneBoundingBox((bool)e.NewValue);
		}

		/// <summary>Callback when the property for showing individual model bounding boxes changed</summary>
		private static void OnShowModelBBoxesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			Render3DControl r3DControl = source as Render3DControl;

			r3DControl.SetShowModelBoundingBoxes((bool)e.NewValue);
		}


		#endregion

		#region SceneRelated

		/// <summary>Helper function to create scene flags from currently set properties</summary>
		private UInt32 GetSceneFlagsFromProperties()
		{
			UInt32 outFlags = 0;
			if (ShowWorldAxis)
				outFlags |= (UInt32)WebSupergoo.EViewFlags.ViewParams_ShowAxis;
			if (ShowWorldGrid)
				outFlags |= (UInt32)WebSupergoo.EViewFlags.ViewParams_ShowGrid;
			if (ShowSceneBBox)
				outFlags |= (UInt32)WebSupergoo.EViewFlags.ViewParams_ShowSceneBBox;
			if (ShowModelBBoxes)
				outFlags |= (UInt32)WebSupergoo.EViewFlags.ViewParams_ShowModelBBoxes;
			if (LoadViewFilesOnSceneOpen)
				outFlags |= (UInt32)WebSupergoo.EViewFlags.ViewParams_LoadViewFiles | (UInt32)WebSupergoo.EViewFlags.ViewParams_ViewFilesOverride;

			return outFlags;
		}

		/// <summary>Helper function to load a scene (depending on whether we are in design mode or not)</summary>
		private void LoadScene(string newSceneFilename)
		{
			if (m_isInDesignMode)
				LoadScenePlaceholder(newSceneFilename);
			else
				LoadSceneFromFile(newSceneFilename);
		}

		/// <summary>Implement loading a scene from file, and potentially unloading previously loaded scenes</summary>
		private void LoadSceneFromFile(string newSceneFilename)
		{
			if (m_renderProxy != IntPtr.Zero)
			{
				if (newSceneFilename == null || newSceneFilename.Length == 0)
				{
					FileClosingEventArgs closeArgs = new FileClosingEventArgs(m_currentSceneFilename);
					FileClosingEvent(this, closeArgs);

					m_unloadCompleteDelegate = new WebSupergoo.DelegateUnloadComplete((sceneHandle) =>
					{
						// reset name
						m_currentSceneFilename = null;
						m_currentSceneHandle = 0;

						FileClosedEventArgs closeArgs0 = new FileClosedEventArgs();

						Application.Current.Dispatcher.Invoke(new Action<FileClosedEventArgs>((fe) => {
							// fire event
							FileClosedEvent(this, fe);
						}), new object[] { closeArgs0 });
						m_unloadCompleteDelegate = null;
					});

					Dll_Glue3D.CloseScene(m_renderProxy, m_unloadCompleteDelegate);
				} else
				{
					if (File.Exists(newSceneFilename))
					{
						// check if we have to close first
						if (m_currentSceneFilename != null && m_currentSceneFilename.Length > 0)
						{
							// just fire off the closing/closed events, and reset name, as it will be handled internally in dll anyway
							FileClosingEventArgs closingArgs = new FileClosingEventArgs(m_currentSceneFilename);
							FileClosingEvent(this, closingArgs);
							FileClosedEventArgs closedArgs = new FileClosedEventArgs();
							FileClosedEvent(this, closedArgs);
							m_currentSceneFilename = string.Empty;
						}

						FileOpeningEventArgs openArgs = new FileOpeningEventArgs(newSceneFilename);
						FileOpeningEvent(this, openArgs);

						// put placeholder image meanwhile
						m_placeholderImage.Source = m_placeholderImageSource;
						m_borderElement.Child = m_placeholderImage;

						// our delegate for when loading is completed
						m_loadCompleteDelegate = new WebSupergoo.DelegateLoadComplete((loadResult, success) => {
							// get result structure, if available
							WebSupergoo.LoadSceneResult castedResult = new WebSupergoo.LoadSceneResult();
							if(loadResult != IntPtr.Zero)
								castedResult = (WebSupergoo.LoadSceneResult)Marshal.PtrToStructure(loadResult, typeof(WebSupergoo.LoadSceneResult));
							WebSupergoo.ActiveCameraInfo castedCamInfo = new WebSupergoo.ActiveCameraInfo();
							if(castedResult.activeCameraInfo != IntPtr.Zero)
								castedCamInfo = (WebSupergoo.ActiveCameraInfo)Marshal.PtrToStructure(castedResult.activeCameraInfo, typeof(WebSupergoo.ActiveCameraInfo));

							// store name and handle
							m_currentSceneFilename = Dll_Glue3D.StringFromDll(castedResult.sceneFilename, castedResult.sceneFilenameLen);
							m_currentSceneHandle = castedResult.sceneHandle;

							// create FileOpenInfo
							FileOpenInfo openInfo = new FileOpenInfo();
							openInfo.originalFilename = m_currentSceneFilename;
							openInfo.sceneHandle = castedResult.sceneHandle;
							openInfo.lightingScheme = (WebSupergoo.ELightingScheme)castedResult.lightingScheme;
							openInfo.availableCameras = Dll_Glue3D.StringArrayFromDll(castedResult.availableCameras,
								castedResult.availableCameraLens, castedResult.availableCameraCount);
							openInfo.activeCameraInfo = new CameraInfo();
							if(castedResult.activeCameraInfo != IntPtr.Zero) {
								openInfo.activeCameraInfo.cameraName = Dll_Glue3D.StringFromDll(castedCamInfo.cameraName, castedCamInfo.cameraNameLen);
								openInfo.activeCameraInfo.cameraMode = (WebSupergoo.ECameraMode)castedCamInfo.cameraMode;
								openInfo.activeCameraInfo.isModifiable = castedCamInfo.isModifiable != 0;
								openInfo.activeCameraInfo.isOrtho = castedCamInfo.isOrtho != 0;
							}

							FileOpenedEventArgs openArgs0 = new FileOpenedEventArgs(openInfo, success);
							Application.Current.Dispatcher.Invoke(new Action<FileOpenedEventArgs>((fe) => {
								// set render host as child again
								m_borderElement.Child = m_renderHost;

								// fire event
								FileOpenedEvent(this, fe);
							}), new object[] { openArgs0 });
							m_loadCompleteDelegate = null;
							m_loadProgressDelegate = null;
						});

						// delegate for progress updates
						m_loadProgressDelegate = new WebSupergoo.DelegateLoadProgress(OnLoadSceneProgress);

						string ext = System.IO.Path.GetExtension(newSceneFilename).ToLower();
						bool isPDF = (ext == ".pdf");
						PdfUtilities.ExtractionResult extRes;
						PdfUtilities.StreamInfo selectedStream = null;
						if (isPDF)
						{
							if (PdfUtilities.ExtractAll3DData(newSceneFilename, out extRes) && extRes.Streams.Count > 0 && Select3DStreamEvent != null)
							{
								ContentSelect3DStreamEventArgs selectArgs = new ContentSelect3DStreamEventArgs(extRes.Streams);
								int userSelection = Select3DStreamEvent(this, selectArgs);
								if (userSelection >= 0 && userSelection < extRes.Streams.Count)
								{
									selectedStream = extRes.Streams[userSelection];
								}
							}
						}

						GCHandle streamHandle = new GCHandle(), viewsHandle = new GCHandle();
						GCHandle[] handles = null;
						try
						{
							// set up loading parameters
							WebSupergoo.LoadSceneParams loadParams = new WebSupergoo.LoadSceneParams();
							if (isPDF && selectedStream != null && selectedStream.Stream != null)
							{
								loadParams.flags = (UInt32)WebSupergoo.ELoadParamFlags.LoadParam_Input_RawData;
								byte[] streamData = selectedStream.Stream;
								streamHandle = SafeUtils.GCHandle_Alloc(streamData, GCHandleType.Pinned);
								loadParams.sceneData = SafeUtils.GCHandle_AddrOfPinnedObject(ref streamHandle);
								loadParams.sceneDataSize = (uint)streamData.Length;

								// add all the view infos
								if (selectedStream.Views.Count > 0)
								{
									// select default view
									int camSelection = 0;
									if (selectedStream.Views.Count > 1 && SelectCameraEvent != null)
									{
										CameraSelectEventArgs selectArgs = new CameraSelectEventArgs(selectedStream.Views);
										camSelection = SelectCameraEvent(this, selectArgs);
										if (camSelection < 0 || camSelection >= selectedStream.Views.Count)
											camSelection = 0;
									}

									loadParams.viewData.defaultViewIndex = (UInt32)camSelection;
									loadParams.viewData.flags = (UInt32)WebSupergoo.ELoadViewFlags.LoadView_StringData;
									Utilities.PinViews(selectedStream.Views, out handles);
									IntPtr[] viewInfos = new IntPtr[handles.Length];
									viewsHandle = SafeUtils.GCHandle_Alloc(viewInfos, GCHandleType.Pinned);
									loadParams.viewData.viewInfos = SafeUtils.GCHandle_AddrOfPinnedObject(ref viewsHandle);
									loadParams.viewData.viewInfoCount = viewInfos.Length;
									for (int i = 0; i < handles.Length; ++i)
									{
										viewInfos[i] = SafeUtils.GCHandle_AddrOfPinnedObject(ref handles[i]);
									}
								}
							}
							else
							{
								loadParams.flags = (UInt32)WebSupergoo.ELoadParamFlags.LoadParam_Input_Filename;
								loadParams.sceneFilename = newSceneFilename;
							}
							loadParams.progressCallback = m_loadProgressDelegate;
							WebSupergoo.ViewParams viewParams = new WebSupergoo.ViewParams();
							viewParams.flags = GetSceneFlagsFromProperties();
							viewParams.backgroundColor = Utilities.Color2Int(BackgroundColor);
							viewParams.lightingScheme = (UInt32)LightingScheme;

							Dll_Glue3D.LoadScene(m_renderProxy, loadParams, viewParams, m_loadCompleteDelegate);
						}
						finally
						{
							if (streamHandle.IsAllocated)
								SafeUtils.GCHandle_Free(ref streamHandle);
							if (viewsHandle.IsAllocated)
								SafeUtils.GCHandle_Free(ref viewsHandle);
							if (handles != null)
							{
								for (int i = 0; i < handles.Length; ++i)
								{
									if (handles[i].IsAllocated)
										SafeUtils.GCHandle_Free(ref handles[i]);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>Handler for LoadProgress events</summary>
		private void OnLoadSceneProgress(WebSupergoo.ProgressInfo progress)
		{
			Application.Current.Dispatcher.Invoke((Action)delegate
			{
				// fire event
				FileProgressEventArgs evArg = new FileProgressEventArgs(progress.currentProgress);
				FileProgressEvent(this, evArg);
			});
		}

		/// <summary>Helper function to load a scene placeholder in design mode</summary>
		private void LoadScenePlaceholder(string newSceneFilename)
		{
			if (m_placeholderImage != null)
			{
				if (newSceneFilename == null || newSceneFilename.Length == 0 || !File.Exists(newSceneFilename))
				{
					m_placeholderImage.Source = null;
				} else
				{
					// add placeholder
					//m_placeholderImage.Source = Utilities.GetImageSourceFromResource("Glue3DUtilities", "placeholder.png");
					m_placeholderImage.Source = m_placeholderImageSource;

					// load real one if possible
					WebSupergoo.CaptureParams parms = new WebSupergoo.CaptureParams();
					//parms.loadParams = new WebSupergoo.LoadSceneParams();
					parms.loadParams.flags = (UInt32)WebSupergoo.ELoadParamFlags.LoadParam_Input_Filename;
					parms.loadParams.sceneHandle = 0;
					parms.loadParams.sceneFilename = newSceneFilename;
					parms.viewParams.flags = GetSceneFlagsFromProperties();
					parms.viewParams.cameraName = string.Empty;
					parms.viewParams.backgroundColor = Utilities.Color2Int(BackgroundColor);
					parms.flags = (UInt32)WebSupergoo.ECaptureFlags.Capture_To_Memory | (UInt32)WebSupergoo.ECaptureFlags.Capture_UnloadAfterCapture;
					parms.captureFilename = string.Empty;
					parms.width = Convert.ToUInt32(m_placeholderImage.Width);
					parms.height = Convert.ToUInt32(m_placeholderImage.Height);

					OnCaptureFinished onCaptureFinished = new OnCaptureFinished(this);
					SceneCapture.DoCapture(ref parms, new SceneCapture.DlgCaptureFinished(onCaptureFinished.CaptureFinished));
				}
			}
		}

		private sealed class OnCaptureFinished {
			private Render3DControl _control;
			private uint _flags;
			private string _captureFilename;
			private byte[] _pixelData;
			private int _width, _height;

			public OnCaptureFinished(Render3DControl control) {
				_control = control;
			}

			public Action<bool> CaptureFinished(WebSupergoo.CaptureResult captureResult, bool captureSuccess) {
				// not in main thread
				if(captureSuccess) {
					_flags = captureResult.flags;
					if((_flags & (UInt32)WebSupergoo.ECaptureResultFlags.CaptureResult_CaptureFile_Set) != 0) {
						_captureFilename = Dll_Glue3D.StringFromDll(captureResult.captureFilename, captureResult.captureFilenameLen);
					} else if((_flags & (UInt32)WebSupergoo.ECaptureResultFlags.CaptureResult_CaptureMemory_Set) != 0) {
						// create byte array from data
						_pixelData = new byte[captureResult.dataSize];
						Marshal.Copy(captureResult.data, _pixelData, 0, (int)captureResult.dataSize);
						_width = (int)captureResult.width;
						_height = (int)captureResult.height;
					}
				}
				return Invoke;
			}
			public void Invoke(bool captureSuccess) {
				// in main thread
				// cannot access pointers in WebSupergoo.CaptureResult
				if(captureSuccess) {
					if((_flags & (UInt32)WebSupergoo.ECaptureResultFlags.CaptureResult_CaptureFile_Set) != 0) {
						BitmapImage src = new BitmapImage();
						src.BeginInit();
						src.UriSource = new Uri(_captureFilename);
						src.CacheOption = BitmapCacheOption.OnLoad;
						src.EndInit();

						_control.m_placeholderImage.Source = src;
					} else if((_flags & (UInt32)WebSupergoo.ECaptureResultFlags.CaptureResult_CaptureMemory_Set) != 0) {
						// create image
						BitmapSource bmpSource = BitmapSource.Create(_width, _height,
							96.0, 96.0, PixelFormats.Bgra32, null, _pixelData, _width*4);

						// set it
						_control.m_placeholderImage.Source = bmpSource;
					}
				} else {
					_control.m_placeholderImage.Source = null;
				}
			}
		}

		/// <summary>Helper function to set the antialiasing mode on the renderer</summary>
		private void SetAntialiasingMode(UInt32 packedAAMode)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_FBOAntialiasingMode, (Int32)packedAAMode);
		}

		/// <summary>Helper to get the antialiasing mode from the renderer</summary>
		private UInt32 GetAntialiasingMode()
		{
			UInt32 packedAAMode = 0;

			if (m_renderProxy != IntPtr.Zero)
			{
				packedAAMode = (UInt32)Dll_Glue3D.GetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_FBOAntialiasingMode);
			}
			return packedAAMode;
		}

		/// <summary>Helper to set the anisotropic filtering on the renderer</summary>
		private void SetAnisotropicFiltering(UInt32 anisotropy)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_AnisotropicFiltering, (Int32)anisotropy);
		}

		/// <summary>Helper function to get the anisotropic filtering mode from the renderer</summary>
		private UInt32 GetAnisotropicFiltering()
		{
			UInt32 anistropy = 0;

			if (m_renderProxy != IntPtr.Zero)
				anistropy = (UInt32)Dll_Glue3D.GetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_AnisotropicFiltering);

			return anistropy;
		}

		/// <summary>Helper to set the lighting scheme on the renderer</summary>
		private void SetLightingScheme(WebSupergoo.ELightingScheme lightScheme)
		{
			if (m_renderProxy != IntPtr.Zero)
			{
				Dll_Glue3D.SetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_LightingScheme, (Int32)lightScheme);

				// fire event
				LightingSchemeChangedEventArgs evArg = new LightingSchemeChangedEventArgs(lightScheme);
				LightingSchemeChangedEvent(this, evArg);
			}
		}

		/// <summary>Helper to get the lighting scheme from the renderer</summary>
		private WebSupergoo.ELightingScheme GetLightingScheme()
		{
			if (m_renderProxy != IntPtr.Zero)
				return (WebSupergoo.ELightingScheme)Dll_Glue3D.GetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_LightingScheme);
			else
				return WebSupergoo.ELightingScheme.LightingScheme_None;
		}

		/// <summary>Helper to set the background color on the renderer</summary>
		private void SetBackgroundColor(System.Windows.Media.Color bgColor)
		{
			if (m_renderProxy != IntPtr.Zero)
			{
				Int32 col = (Int32)Utilities.Color2Int(bgColor);
				Dll_Glue3D.SetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_BackgroundColor, col);
			}
		}

		/// <summary>Helper to get the background color from the renderer</summary>
		private System.Windows.Media.Color GetBackgroundColor()
		{
			System.Windows.Media.Color bgCol = new System.Windows.Media.Color();

			if (m_renderProxy != IntPtr.Zero)
			{
				Int32 col = Dll_Glue3D.GetProperty_Int32(m_renderProxy, WebSupergoo.EProperty.Prop_Renderer_BackgroundColor);
				bgCol = Utilities.UInt2Color((UInt32)col);
			}
			return bgCol;
		}

		/// <summary>Helper to set the visibility of the world axis</summary>
		private void SetShowWorldAxis(bool newValue)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowAxis, newValue);
		}

		/// <summary>Helper to get the visibility of the world axis</summary>
		private bool GetShowWorldAxis()
		{
			if (m_renderProxy != IntPtr.Zero)
				return Dll_Glue3D.GetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowAxis);
			return false;
		}

		/// <summary>Helper to set the visibility of the world grid</summary>
		private void SetShowWorldGrid(bool newValue)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowGrid, newValue);
		}

		/// <summary>Helper to get the visibility of the world grid</summary>
		private bool GetShowWorldGrid()
		{
			if (m_renderProxy != IntPtr.Zero)
				return Dll_Glue3D.GetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowGrid);
			return false;
		}

		/// <summary>Helper to set the visibility of the scene enclosing bounding box</summary>
		private void SetShowSceneBoundingBox(bool newValue)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowBoundingBox, newValue);
		}

		/// <summary>Helper to get the visibility of the scene enclosing bounding box</summary>
		private bool GetShowSceneBoundingBox()
		{
			if (m_renderProxy != IntPtr.Zero)
				return Dll_Glue3D.GetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Scene_ShowBoundingBox);
			return false;
		}

		/// <summary>Helper to set the visibility of the model boundinx boxes</summary>
		private void SetShowModelBoundingBoxes(bool newValue)
		{
			if (m_renderProxy != IntPtr.Zero)
				Dll_Glue3D.SetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Model_ShowBoundingBoxes, newValue);
		}

		/// <summary>Helper to get the visibility of the model bounding boxes</summary>
		private bool GetShowModelBoundingBoxes()
		{
			if (m_renderProxy != IntPtr.Zero)
				return Dll_Glue3D.GetProperty_Bool(m_renderProxy, WebSupergoo.EProperty.Prop_Model_ShowBoundingBoxes);
			return false;
		}

		#endregion

		#region Camera

		/// <summary>Helper to get the list of available cameras, to be sent to the provided delegate</summary>
		public void GetAvailableCameras(DelegateCameraListReceived camListReceivedCB)
		{
			if (m_renderProxy != IntPtr.Zero && m_currentCamListDelegate == null)
			{
				m_currentCamListDelegate = new WebSupergoo.DelegateStrList((field, camNames, camLens, camNameCount) =>
				{
					string[] list = Dll_Glue3D.StringArrayFromDll(camNames, camLens, camNameCount);
					Application.Current.Dispatcher.Invoke(new DelegateCameraListReceived((resultNames) =>
					{
						if (camListReceivedCB != null)
							camListReceivedCB(resultNames);

						m_currentCamListDelegate = null;
					}), new object[] { list });
				});

				Dll_Glue3D.GetAvailableCameras(m_renderProxy, m_currentCamListDelegate);
			}
		}

		/// <summary>Helper to activate the specified camera</summary>
		public void ActivateCamera(WebSupergoo.ECameraActivationType activationType, string cameraName, DelegateCameraChanged camChangedCB)
		{
			if (m_renderProxy != IntPtr.Zero && m_currentCamDelegate == null)
			{
				m_currentCamDelegate = new WebSupergoo.DelegateOnActiveCameraChanged((inCamInfo, inSuccess) =>
				{
					CameraInfo outputInfo = new CameraInfo();
					if (inCamInfo != IntPtr.Zero)
					{
						WebSupergoo.ActiveCameraInfo castedResult = (WebSupergoo.ActiveCameraInfo)Marshal.PtrToStructure(inCamInfo, typeof(WebSupergoo.ActiveCameraInfo));
						outputInfo.cameraName = Dll_Glue3D.StringFromDll(castedResult.cameraName, castedResult.cameraNameLen);
						outputInfo.cameraMode = (WebSupergoo.ECameraMode)castedResult.cameraMode;
						outputInfo.isModifiable = castedResult.isModifiable == 0 ? false : true;
						outputInfo.isOrtho = castedResult.isOrtho == 0 ? false : true;
					}

					// dispatch to main thread
					Application.Current.Dispatcher.Invoke(new DelegateCameraChanged((resultStruct, result) => 
					{
						if (camChangedCB != null)
							camChangedCB(resultStruct, result);

						m_currentCamDelegate = null;

					}), new object[] { outputInfo, inSuccess });
				});

				Dll_Glue3D.ActivateCamera(m_renderProxy, activationType, cameraName, m_currentCamDelegate);
			}
		}

		/// <summary>Helper to activate the first camera in the list of available cameras</summary>
		public void ActivateFirstCamera(DelegateCameraChanged callback)
		{
			ActivateCamera(WebSupergoo.ECameraActivationType.ActivateCamera_First, string.Empty, callback);
		}

		/// <summary>Helper to activate the next camera in the list of available cameras</summary>
		public void ActivateNextCamera(DelegateCameraChanged callback)
		{
			ActivateCamera(WebSupergoo.ECameraActivationType.ActivateCamera_Next, string.Empty, callback);
		}

		/// <summary>Helper to activate the camera designated as default camera</summary>
		public void ActivateDefaultCamera(DelegateCameraChanged callback)
		{
			ActivateCamera(WebSupergoo.ECameraActivationType.ActivateCamera_Default, string.Empty, callback);
		}

		/// <summary>Helper to activate a camera by name</summary>
		public void ActivateCamera(string cameraName, DelegateCameraChanged callback)
		{
			ActivateCamera(WebSupergoo.ECameraActivationType.ActivateCamera_ByName, cameraName, callback);
		}

		/// <summary>Helper to write a screenshot of the scene -as it is currently rendered- to the specified file</summary>
		public void Screenshot(string filename)
		{
			Dll_Glue3D.Screenshot(m_renderProxy, filename);
		}

		/// <summary>Helper to load a view from the specified file (file has to be a text based file, containing PDF 3DView annotation info)</summary>
		public void LoadViewFileForScene(string filename, DelegateCameraChanged callback)
		{
			if (m_renderProxy != IntPtr.Zero && m_currentCamDelegate == null)
			{
				m_currentCamDelegate = new WebSupergoo.DelegateOnActiveCameraChanged((inCamInfo, inSuccess) =>
				{
					CameraInfo outputInfo = new CameraInfo();
					if (inCamInfo != IntPtr.Zero)
					{
						WebSupergoo.ActiveCameraInfo castedResult = (WebSupergoo.ActiveCameraInfo)Marshal.PtrToStructure(inCamInfo, typeof(WebSupergoo.ActiveCameraInfo));
						outputInfo.cameraName = Dll_Glue3D.StringFromDll(castedResult.cameraName, castedResult.cameraNameLen);
						outputInfo.cameraMode = (WebSupergoo.ECameraMode)castedResult.cameraMode;
						outputInfo.isModifiable = castedResult.isModifiable == 0 ? false : true;
						outputInfo.isOrtho = castedResult.isOrtho == 0 ? false : true;
					}

					// dispatch to main thread
					Application.Current.Dispatcher.Invoke(new DelegateCameraChanged((resultStruct, result) =>
					{
						if (callback != null)
							callback(resultStruct, result);

						m_currentCamDelegate = null;

					}), new object[] { outputInfo, inSuccess });
				});

				Dll_Glue3D.LoadViewInfoFromFile(m_renderProxy, filename, true, m_currentCamDelegate);
			}
		}

		/// <summary>Helper to load a view from the specified PDF file (if available)</summary>
		public void LoadViewFromPDF(string filename, DelegateCameraChanged callback)
		{
			if (m_renderProxy != IntPtr.Zero && m_currentCamDelegate == null)
			{
				string ext = System.IO.Path.GetExtension(filename).ToLower();
				if (ext == ".pdf")
				{
					PdfUtilities.ExtractionResult extRes;
					if (PdfUtilities.ExtractAll3DViews(filename, out extRes) && extRes.Streams.Count > 0 && extRes.ViewsProcessed > 0 && SelectCameraEvent != null)
					{
						// find the stream we want to use, if necessary, ask the user
						PdfUtilities.StreamInfo useStream = null;
						if (extRes.Streams.Count > 1 && Select3DStreamEvent != null)
						{
							ContentSelect3DStreamEventArgs selectStreamArgs = new ContentSelect3DStreamEventArgs(extRes.Streams);
							int selectedStream = Select3DStreamEvent(this, selectStreamArgs);
							if (selectedStream < 0 || selectedStream >= extRes.Streams.Count)
								return;
							useStream = extRes.Streams[selectedStream];
						}
						else
							useStream = extRes.Streams[0];

						// safety
						if (useStream == null)
							return;

						// find the views for that stream 
						CameraSelectEventArgs selectArgs = new CameraSelectEventArgs(useStream.Views);
						int userSelection = SelectCameraEvent(this, selectArgs);
						if (userSelection >= 0 && userSelection < useStream.Views.Count)
						{
							string viewInfo = useStream.Views[userSelection].View;

							m_currentCamDelegate = new WebSupergoo.DelegateOnActiveCameraChanged((inCamInfo, inSuccess) =>
							{
								CameraInfo outputInfo = new CameraInfo();
								if (inCamInfo != IntPtr.Zero)
								{
									WebSupergoo.ActiveCameraInfo castedResult = (WebSupergoo.ActiveCameraInfo)Marshal.PtrToStructure(inCamInfo, typeof(WebSupergoo.ActiveCameraInfo));
									outputInfo.cameraName = Dll_Glue3D.StringFromDll(castedResult.cameraName, castedResult.cameraNameLen);
									outputInfo.cameraMode = (WebSupergoo.ECameraMode)castedResult.cameraMode;
									outputInfo.isModifiable = castedResult.isModifiable == 0 ? false : true;
									outputInfo.isOrtho = castedResult.isOrtho == 0 ? false : true;
								}

								// dispatch to main thread
								Application.Current.Dispatcher.Invoke(new DelegateCameraChanged((resultStruct, result) =>
								{
									if (callback != null)
										callback(resultStruct, result);

									m_currentCamDelegate = null;

								}), new object[] { outputInfo, inSuccess });
							});

							Dll_Glue3D.LoadViewInfoFromString(m_renderProxy, viewInfo, true, m_currentCamDelegate);
						}
					}
				}
			}
		}

		#endregion

		#region Export

		/// <summary>Export a scene to PRC</summary>
		public void ExportSceneToPRC(string targetFilename, string sourceFilename)
		{
			if (m_renderProxy != IntPtr.Zero)
			{
				// send event that we start exporting
				FileExportEventArgs exportArgs = new FileExportEventArgs(targetFilename);
				FileExportEvent(this, exportArgs);

				// dispatch to main thread
				m_exportCompleteDelegate = new WebSupergoo.DelegateExportComplete((exportResult, success) =>
				{
					// get result structure, if available
					WebSupergoo.ExportSceneResult castedResult = new WebSupergoo.ExportSceneResult();
					if(exportResult != IntPtr.Zero)
						castedResult = (WebSupergoo.ExportSceneResult)Marshal.PtrToStructure(exportResult, typeof(WebSupergoo.ExportSceneResult));

					FileExportedEventArgs exportArgs0 = new FileExportedEventArgs(castedResult, success);
					Application.Current.Dispatcher.Invoke(new Action<FileExportedEventArgs>((fe) => {
						// testing write to file from received buffer data
						if(fe.ExportData!=null) {
							string targetFile = m_currentSceneFilename + ".prc";
							File.WriteAllBytes(targetFile, fe.ExportData);
						}

						// fire event
						FileExportedEvent(this, fe);
					}), new object[] { exportArgs0 });
					m_exportCompleteDelegate = null;
				});

				// trigger export
				bool testWriteToMemory = false;
				WebSupergoo.ExportSceneParams exportParams = new WebSupergoo.ExportSceneParams();
				if( sourceFilename.Length == 0 )
				{
					exportParams.loadParams.flags = (UInt32)WebSupergoo.ELoadParamFlags.LoadParam_Input_SceneHandle;
					exportParams.loadParams.sceneHandle = m_currentSceneHandle;
				}
				else
				{
					exportParams.loadParams.flags = (UInt32)WebSupergoo.ELoadParamFlags.LoadParam_Input_Filename;
					exportParams.loadParams.sceneFilename = sourceFilename;
				}
				exportParams.loadParams.userData = IntPtr.Zero;
				exportParams.exportFlags = (UInt32)(WebSupergoo.EExportFlags.ExportFormat_PRC | WebSupergoo.EExportFlags.ExportTexturesJPEG);
				if( !testWriteToMemory )
					exportParams.exportFilename = targetFilename;
				Dll_Glue3D.ExportScene(m_renderProxy, exportParams, m_exportCompleteDelegate);
			}
		}

		#endregion


	}
}
