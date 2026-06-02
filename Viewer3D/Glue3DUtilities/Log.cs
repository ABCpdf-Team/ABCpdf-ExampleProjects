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


namespace Glue3DUtilities
{
	/// <summary>Convenience class to wrap the logging functionaity contained in the Glue3D dlls</summary>
	public static class Log
	{

		#region Constants

		/// <summary>The log level as used inside the Glue3D dlls</summary>
		public enum LogLevel : int
		{
			Message = 0,
			Error,
			Warning,
			Info,
			Verbose,
			ExtraVerbose
		}

		#endregion

		/// <summary>Specify a log file and log level, and start the logger; if append is specified it will append to the last log (if available). Note: If no log file is specified, will log to output console</summary>
		public static bool StartLogging(string logFile, bool append, LogLevel logLevel)
		{
			return Dll_Glue3D.StartLogging(logFile, append, (Int32)logLevel);
		}

		/// <summary>Stop the logger</summary>
		public static void StopLogging()
		{
			Dll_Glue3D.StopLogging();
		}

		/// <summary>Check if logging is enabled</summary>
		public static bool IsLogging()
		{
			return Dll_Glue3D.IsLogging();
		}

		/// <summary>Change the log level</summary>
		public static void SetLogLevel(LogLevel level)
		{
			Dll_Glue3D.SetLogLevel((Int32)level);
		}

		/// <summary>Get the current log level</summary>
		public static LogLevel GetLogLevel()
		{
			return (LogLevel)Dll_Glue3D.GetLogLevel();
		}

		/// <summary>Write a (high priority) message to the log</summary>
		public static void Message(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.Message, logMsg);
		}

		/// <summary>Write an error to the log</summary>
		public static void Error(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.Error, logMsg);
		}

		/// <summary>Write a warning to the log</summary>
		public static void Warn(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.Warning, logMsg);
		}

		/// <summary>Write a informational message to the log</summary>
		public static void Info(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.Info, logMsg);
		}

		/// <summary>Write a (lower priority) message to the log</summary>
		public static void Verbose(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.Verbose, logMsg);
		}

		/// <summary>Write a (lowest priority) message to the log</summary>
		public static void ExtraVerbose(string logMsg)
		{
			Dll_Glue3D.LogMessage((Int32)LogLevel.ExtraVerbose, logMsg);
		}

		/// <summary>Add a logging hook to receive any log messages (if the hook is zero, it will revert to logging to the default logger)</summary>
		public static WebSupergoo.DelegateLogging SetLoggingHook(IntPtr userData, WebSupergoo.DelegateLogging hook)
		{
			return Dll_Glue3D.SetLoggingHook(userData, hook);
		}

		/// <summary>Get the current logging hook</summary>
		public static WebSupergoo.DelegateLogging GetLoggingHook(out IntPtr userData)
		{
			return Dll_Glue3D.GetLoggingHook(out userData);
		}
	}
}
