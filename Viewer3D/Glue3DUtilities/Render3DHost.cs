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
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Glue3DUtilities
{
	/// <summary>Implementation of HwndHost to host the Win32 based OpenGL window</summary>
    internal class Render3DHost : HwndHost
    {
		#region Private Data

		private int m_width;
        private int m_height;
        private IntPtr m_renderProxyHwnd;
		private static int s_viewCounter = 0;

		#endregion

		#region Properties

		/// <summary>IntPtr to the RenderProxy instance</summary>
		public IntPtr RenderProxy { get; private set; }
		/// <summary>The name of the RenderProxy instance</summary>
		public string ProxyName { get; private set; }
		/// <summary>Flags describing details of the RenderProxy, see also ERenderProxyFlags</summary>
		public uint ProxyFlags { get; private set; }
		/// <summary>The antialiasing color samples currently used</summary>
		public ushort AntialiasingColorSamples { get; private set; }
		/// <summary>The antialiasing coverage samples currently used (CSAA)</summary>
		public ushort AntialiasingCoverageSamples { get; private set; }
		/// <summary>The list of available antialiasing modes. Note: Uses packed ints containing the color (lo word) and coverage (hi word) samples</summary>
		public uint[] AvailableAAModes { get; private set; }
		/// <summary>The currently used anisotropic filtering mode</summary>
		public uint Anisotropy { get; private set; }
		/// <summary>The list of available anisotropic filtering modes</summary>
		public uint[] AnisotropyModes { get; private set; }

		#endregion

		#region Methods
		/// <summary>Construct an instance, specifying window dimensions</summary>
		public Render3DHost(double width, double height)
        {
			m_width = (Int32)width;
            m_height = (Int32)height;

			RenderProxy = IntPtr.Zero;
            m_renderProxyHwnd = IntPtr.Zero;
        }

		/// <summary>Set up the core window from an HWND; part of HwndHost interface</summary>
		protected override HandleRef BuildWindowCore(HandleRef parent)
        {
            ++s_viewCounter;

            WebSupergoo.RenderProxyParams proxyParams = new WebSupergoo.RenderProxyParams();
            proxyParams.parent = parent.Handle.ToInt64();
            proxyParams.width = (UInt32)m_width;
            proxyParams.height = (UInt32)m_height;
            proxyParams.setUInt32List = delegate (int field, uint[] list, uint count) {
				switch(field) {
				case WebSupergoo.RenderProxyParams.availableAAModesField:
					AvailableAAModes = list;
					break;
				case WebSupergoo.RenderProxyParams.anisotropyModesField:
					AnisotropyModes = list;
					break;
				}
            };

			WebSupergoo.RenderProxyInfo proxyInfo = new WebSupergoo.RenderProxyInfo();
            RenderProxy = Dll_Glue3D.CreateRenderProxy(proxyParams, ref proxyInfo);
			Name = Dll_Glue3D.StringFromDll(proxyInfo.name, proxyInfo.nameLen);
			ProxyFlags = proxyInfo.flags;
			AntialiasingColorSamples = proxyInfo.antialiasingColorSamples;
			AntialiasingCoverageSamples = proxyInfo.antialiasingCoverageSamples;
			Anisotropy = proxyInfo.anisotropy;

			if (RenderProxy == IntPtr.Zero)
                return new HandleRef(null, IntPtr.Zero);

            m_renderProxyHwnd = Dll_Glue3D.GetRenderProxyHwnd(RenderProxy);
            if (m_renderProxyHwnd == IntPtr.Zero)
                return new HandleRef(null, IntPtr.Zero);

            return new HandleRef(this, m_renderProxyHwnd);
        }

		/// <summary>Destroy the core (HWND base) window; part of HwndHost interface</summary>
		protected override void DestroyWindowCore(HandleRef hwnd)
        {
            if (RenderProxy != IntPtr.Zero)
            {
                Dll_Glue3D.DestroyRenderProxy(RenderProxy);
				RenderProxy = m_renderProxyHwnd = IntPtr.Zero;
            }

            Dll_User32.DestroyWindow(m_renderProxyHwnd);            
        }

		/// <summary>Window procedure (not handling anything in this case)</summary>
		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }

		#endregion
	}

}

