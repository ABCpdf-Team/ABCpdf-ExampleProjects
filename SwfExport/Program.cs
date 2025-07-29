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
using System.IO;
using System.Text;
using System.Xml;

using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;

namespace SwfExport {
	class Program {
		static void Main(string[] args) {
			string theSourcePdf = Server.MapPath("MyFolder\\import.pdf");
			string theSourceSwf = Server.MapPath("MyFolder\\swfexport.swf");
			string theDestSwf   = Server.MapPath("MyFolder\\output.swf"); // n.b. 'output.html' needs updating if filename changed.

			using (Doc theDoc = new Doc()) {
				theDoc.Read(theSourcePdf);
				SortedDictionary<int, ImageInfo> images = FindAllImages(theDoc);
				ResizeImages(theDoc, images, 48, 48, 75); // make images 48 DPI using quality of 75
				theDoc.SaveOptions.Template = theSourceSwf;
				theDoc.Save(theDestSwf);
			}
		}

		private static SortedDictionary<int, ImageInfo> FindAllImages(Doc inDoc) {
			SortedDictionary<int, ImageInfo> images = new SortedDictionary<int, ImageInfo>();
			for (int i = 1; i <= inDoc.PageCount; i++) {
				inDoc.PageNumber = i;
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(inDoc.GetText("svg+"));
				XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
				nsmgr.AddNamespace("svg", "http://www.w3.org/2000/svg");
				XmlElement root = xmlDoc.DocumentElement;
				XmlNodeList nodeList = xmlDoc.SelectNodes("//svg:image", nsmgr);
				foreach (XmlNode n in nodeList) {
					ImageInfo ii = new ImageInfo(n);
					// Discard the visually smaller duplicates.
					ImageInfo iiDict;
					if (!images.TryGetValue(ii.PixMapId, out iiDict))
						images[ii.PixMapId] = ii;
					else {
						if (iiDict.WidthInPoints < ii.WidthInPoints)
							iiDict.WidthInPoints = ii.WidthInPoints;
						if (iiDict.HeightInPoints < ii.HeightInPoints)
							iiDict.HeightInPoints = ii.HeightInPoints;
					}
				}
			}
			return images;
		}

		private static void ResizeImages(Doc inDoc, SortedDictionary<int, ImageInfo> inImages,
			double inDpiHorizontal, double inDpiVertical, int inCompressionQuality)
		{
			foreach (ImageInfo ii in inImages.Values) {
				PixMap pm = inDoc.ObjectSoup[ii.PixMapId] as PixMap;
				int newWidth = Math.Max(1, (int)Math.Round(ii.WidthInPoints * inDpiHorizontal / 72));
				int newHeight = Math.Max(1, (int)Math.Round(ii.HeightInPoints * inDpiVertical / 72));
				if (newWidth < pm.Width || newHeight < pm.Height)
					pm.Resize(Math.Min(newWidth, pm.Width), Math.Min(newHeight, pm.Height));
				if (pm.BitsPerComponent == 1) {
					if (!pm.Compressed)
						pm.CompressCcitt();
					continue;
				}
				ColorSpaceType colorSpace = pm.ColorSpaceType;
				if (colorSpace == ColorSpaceType.Indexed) {
					if (!pm.Compressed)
						pm.Compress(); // Flate.
					continue;
				}
				if ((colorSpace != ColorSpaceType.DeviceGray) && (colorSpace != ColorSpaceType.DeviceRGB))
					pm.Recolor(new ColorSpace(inDoc.ObjectSoup)); // DeviceRGB.
				pm.CompressJpeg(inCompressionQuality);
			}
		}
	}

	sealed class ImageInfo {
		public double WidthInPoints;
		public double HeightInPoints;
		public int PixMapId;

		public ImageInfo(XmlNode inNode) {
			// Image nodes contain lots of useful infomation eg
			// <image xlink:href="image.png" width="810" height="400" x="210" y="414" transform="translate(210, 414) 
			// translate(-210, -414)" pdf_Name="Iabc43" pdf_ObjectID="44" pdf_CTM="810 0 0 400 210 410" pdf_Op="/Iabc43 Do" 
			// pdf_StreamID="41" pdf_StreamOffset="31" pdf_StreamLength="10" />
			this.PixMapId = int.Parse(inNode.Attributes["pdf_ObjectID"].Value);

			XmlAttribute at = inNode.Attributes["width"];
			this.WidthInPoints = (at == null) ? 0 : double.Parse(at.Value);

			at = inNode.Attributes["height"];
			this.HeightInPoints = (at == null) ? 0 : double.Parse(at.Value);
		}
	}

	static class Server {
		public static string MapPath(string fileName) {
			DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
			string theBase = di.Parent.Parent.FullName;
			return Path.Combine(theBase, fileName);
		}
	}
}
