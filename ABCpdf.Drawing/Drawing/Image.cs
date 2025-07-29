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
using System.IO;
using System.Text;

namespace WebSupergoo.ABCpdf13.Drawing {
	#region Image
	public sealed class Image : IDisposable {
		internal readonly XImage xImage = new XImage();
		internal readonly Stream xStream;

		/// <summary>Gets the width, in pixels, of this Image.</summary>
		/// <value>The width, in pixels, of this Image.</value>
		public int Width { get { return xImage.Width; } }

		/// <summary>Gets the height, in pixels, of this Image.</summary>
		/// <value>The height, in pixels, of this Image.</value>
		public int Height { get { return xImage.Height; } }

		private Image() { }
		private Image(Stream stream) {
			xStream = stream;
		}

		/// <summary>Creates a Image from the specified file.</summary>
		/// <param name="filename">A path to the image file.</param>
		/// <returns>The Image this method creates.</returns>
		public static Image FromFile(string filename) {
			if (filename == null || filename == string.Empty)
				return null;
			if (!System.IO.File.Exists(filename))
				return null;
			Image image = new Image();
			image.xImage.SetFile(filename);
			return image;
		}
		
		/// <summary>Creates a Image from the specified data stream.</summary>
		/// <param name="stream">A System.IO.Stream that contains the image data.</param>
		/// <returns>The Image this method create.</returns>
		public static Image FromStream(System.IO.Stream stream) {
			return FromStream(stream, false);
		}
		/// <summary>Creates a Image from the specified data stream.</summary>
		/// <param name="stream">A System.IO.Stream that contains the image data.</param>
		/// <param name="leaveOpen">Whether to leave the stream open when Image is disposed of.</param>
		/// <returns>The Image this method create.</returns>
		public static Image FromStream(System.IO.Stream stream, bool leaveOpen) {
			Image image = new Image(leaveOpen? null: stream);
			image.xImage.SetStream(stream);
			return image;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override bool Equals(object obj) {
			Image image = obj as Image;
			if (obj == null)
				return false;
			return image.GetHashCode() == GetHashCode(); 
		}

		public override string ToString() {
			return base.ToString();
		}

		#region IDisposable Members

		public void Dispose() {
			if (xImage != null) {
				if (xStream != null)
					xImage.Dispose();
				else {	// leave stream open
					XReadOptions options;
					Stream stream;
					xImage.Dispose(out options, out stream);
					if (options != null)
						options.Dispose();
				}
			}
			if (xStream != null) {
				xStream.Close();
			}
		}

		#endregion
	}
	#endregion
}
