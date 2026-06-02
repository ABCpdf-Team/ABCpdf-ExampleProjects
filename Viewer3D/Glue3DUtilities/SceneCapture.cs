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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace Glue3DUtilities
{
    /// <summary>
    /// Helper class to allow capturing 3D scenes to memory or files
    /// </summary>
    public static class SceneCapture
    {
        #region Delegates
        public delegate Action<bool> DlgCaptureFinished(WebSupergoo.CaptureResult resultInfo, bool success);
        #endregion

        #region Members
        private static List<SingleCaptureImpl> s_captureList = null; 
        #endregion

        #region StaticConstructor
        static SceneCapture()
        {
            s_captureList = new List<SingleCaptureImpl>(); 
        }
        #endregion

        #region PublicInterface

        /// <summary>
        /// Loads a 3D file, captures it, then unloads it
        /// </summary>
        public static void DoCapture(ref WebSupergoo.CaptureParams parms, DlgCaptureFinished captureFinishedCB)
        {
            SingleCaptureImpl capture = new SingleCaptureImpl();
            s_captureList.Add(capture);
            capture.DoCapture(ref parms, captureFinishedCB);
        }
        
        #endregion

        #region Impl

        private class SingleCaptureImpl
        {
            #region Members
            private WebSupergoo.DelegateCaptureComplete m_activeSingleCaptureDelegate = null; // member to keep it alive
            #endregion

            public void DoCapture(ref WebSupergoo.CaptureParams parms, DlgCaptureFinished captureFinishedCB)
            {
                // single capture, pass lambda for callback
                m_activeSingleCaptureDelegate = new WebSupergoo.DelegateCaptureComplete((capResult, captureSuccess) =>
                {
                    WebSupergoo.CaptureResult castedResult = new WebSupergoo.CaptureResult();
                    if( capResult != IntPtr.Zero )
                        castedResult = (WebSupergoo.CaptureResult)Marshal.PtrToStructure(capResult, typeof(WebSupergoo.CaptureResult));

                    Action<bool> cb = captureFinishedCB == null ? null : captureFinishedCB(castedResult, captureSuccess);
                    // dispatch to main thread
                    Application.Current.Dispatcher.Invoke(new Action<bool>((result) =>
                    {
                        if (cb != null)
                            cb(result);

                        m_activeSingleCaptureDelegate = null;
                        s_captureList.Remove(this);

                    }), new object[] { captureSuccess });
                });

                Dll_Glue3D.CaptureScene(parms, m_activeSingleCaptureDelegate);
            }
        }

        #endregion

    }
}
