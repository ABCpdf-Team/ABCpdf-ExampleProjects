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
    static class Server
    {
        public static List<string> Paths = new List<string>();
        public static string ReadPath, WritePath;

        static Server() {
            string dir = Utilities.FindDirectory("_ExamplesFromDocs");
            ReadPath = Path.Combine(dir, @"_ExamplesFromDocs\Images\Server");
            WritePath = Path.Combine(dir, @"_ExamplesFromDocs\Images\Outputs");
        }

        public static string MapPath(string path) {
            path = path.Replace('/', '\\');
            bool readOnly = path.Contains("..") || path.Contains(@"Rez");
            path = path.Replace(@"..\", "");
            path = path.Replace(@"Rez\", "");
            string src = Path.Combine(ReadPath, path);
            if (File.Exists(src))
                return src;
            if (readOnly)
                throw new FileNotFoundException();
            string dst = Path.Combine(WritePath, path);
            Paths.Add(dst);
            return dst;
        }

        public static void Clear() {
            Paths.Clear();
            if (Directory.Exists(WritePath))
                Directory.Delete(WritePath, true);
            Directory.CreateDirectory(WritePath);
        }
    }

	class Response {
		public static string Write(string value) {
			return value;
		}
	}

	static class Certificates {
        public static X509Certificate2 GetFromStore() {
			try {
				return new X509Certificate2(Server.MapPath("MyCertificate.p12"), "mypassword", X509KeyStorageFlags.Exportable);
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
