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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace ExamplesProcessing
{
	class Response {
		public static string Write(string value) {
			return value;
		}
	}

	static class Certificates {
		public static X509Certificate2 GetFromStore() {
			try {
				return new X509Certificate2("MyCertificate.p12", "mypassword", X509KeyStorageFlags.Exportable);
			}
			catch {
			}
			return null;
		}
	}

	static class Utilities {
		public static string FindDirectory(string folder) {
			string directory = Directory.GetCurrentDirectory();
			while (true) {
				string d = Path.Combine(directory, folder);
				if (Directory.Exists(d))
					return directory;
				DirectoryInfo info = new DirectoryInfo(directory);
				if (info.Parent == null)
					throw new DirectoryNotFoundException("No such directory as " + folder);
				directory = info.Parent.FullName;
			}
		}
	}
}
