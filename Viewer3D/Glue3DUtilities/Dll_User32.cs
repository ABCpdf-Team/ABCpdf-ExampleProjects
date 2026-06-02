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
using System.Runtime.InteropServices;
using System.Drawing; 



namespace Glue3DUtilities
{
	/// <summary>
	/// The interface to the User32 dll (which contain which contains some window management related functionality)
	/// </summary>
	[System.Security.SuppressUnmanagedCodeSecurity]
    public static class Dll_User32
    {

        // CreateWindowEx
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx( int dwExStyle,
                                                        string lpszClassName,
                                                        string lpszWindowName,
                                                        int style,
                                                        int x, int y,
                                                        int width, int height,
                                                        IntPtr hwndParent,
                                                        IntPtr hMenu,
                                                        IntPtr hInst,
                                                        [MarshalAs(UnmanagedType.AsAny)] object pvParam );

        // DestroyWindow
        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        // SendMessage
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        public static extern Int32 SendMessage( IntPtr hwnd,
                                                    Int32 msg,
                                                    IntPtr wParam,
                                                    IntPtr lParam);
        // GetClientRect
        [DllImport("user32.dll", EntryPoint = "GetClientRect", CharSet = CharSet.Unicode)]
        public static extern bool GetClientRect( IntPtr hWnd,
                                                    [In, Out, MarshalAs(UnmanagedType.Struct)] ref Rectangle lpRect); 
            
        // GetWindowRect
        [DllImport("user32.dll", EntryPoint = "GetWindowRect", CharSet = CharSet.Unicode)]
        public static extern bool GetWindowRect( IntPtr hWnd,
                                                    [In, Out, MarshalAs(UnmanagedType.Struct)] ref Rectangle lpRect); 
        
        // MoveWindow
        [DllImport("user32.dll", EntryPoint = "MoveWindow", CharSet = CharSet.Unicode)]
        public static extern bool MoveWindow(IntPtr hWnd, Int32 x, Int32 y, Int32 w, Int32 h, bool repaint);

        // SetWindowPos
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags); 

        // Win32 constants
        public const int WS_CLIPCHILDREN        = 0x02000000;
        public const int WS_VISIBLE             = 0x10000000;
        public const int WS_CHILD               = 0x40000000;
        public const int WM_SIZE                = 0x00000005;
        public const int SWP_NOZORDER           = 0x00000004;
        public const int SWP_NOACTIVATE         = 0x00000010;
        public const int SWP_ASYNCWINDOWPOS     = 0x00004000; 
    }
}
