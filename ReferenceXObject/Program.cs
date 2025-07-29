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
using System.Text;
using System.IO;
using System.Globalization;
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;

// By default Acrobat is set up to allow you to use internal, but not external, Reference XObjects.
// To see external Reference XObjects you will need to adjust some Acrobat preferences.
//
// The precise settings may vary by Acrobat version and date, but as of January 2021
// these are the Acrobat preferences you need to adjust.
//
// 1) Preferences : Page Display : Reference XObject View Mode
//    - allow viewing of all Reference XObjects
// 2) Preferences : Security (Enhanced) : Privileged Locations
//    - add the folder/file path of the viewed PDF file (ie External.pdf)
// 
// If you do not do this Acrobat error messages are not very explanatory. If you do not do 
// any of this the images will likely be black. If you enable XObjects but do not add the
// location as privileged Acrobat may say things like "There is an error on this page.".
//
// For further details see:
// https://www.adobe.com/devnet-docs/acrobatetk/tools/AppSec/trust.html

namespace ReferenceXObject {
	class Program {
		static void Main(string[] args) {
			string baseDir = Directory.GetCurrentDirectory();
			string resDir = Path.Combine(Directory.GetParent(baseDir).Parent.Parent.FullName, "PDFTable");
			string inputFile = Path.Combine(resDir, "SpaceSample.pdf");	// The PDF file from PDFTable example
			string outputDir = Directory.GetParent(baseDir).Parent.FullName;
			string outputFile = Path.Combine(outputDir, "External.pdf");
			string outputFile2 = Path.Combine(outputDir, "Internal.pdf");
			string externalPath = "ImagePDF";
			using(Doc doc = new Doc()) {
				doc.Read(inputFile);
				ConvertImagesToRefXObjects(doc, outputDir, externalPath);
				doc.Save(outputFile);
			}
			using(Doc doc = new Doc()) {
				doc.Read(outputFile);
				ConvertRefXObjectsToImages(doc, outputDir);
				doc.Save(outputFile2);
			}
		}

		private static List<string> ConvertImagesToRefXObjects(Doc doc, string filePath, string externalPath) {
			List<string> pdfList = new List<string>();
			// Here images are distributed among PDF files according to their color spaces
			// Other schemes (e.g. one image per PDF file) can be used, instead
			Doc grayDoc = null, rgbDoc = null, cmykDoc = null, otherDoc = null;
			string grayRelPath = null, rgbRelPath = null, cmykRelPath = null, otherRelPath = null;
			ObjectCopy objCopy = new ObjectCopy(doc);
			// restrict the bounds so that the reciprocals (scaleX and scaleY) are accurate in 6 decimal places
			XRect maxRect = new XRect();
			maxRect.SetRect(0, 0, 100, 100);
			int objCount = doc.ObjectSoup.Count;
			try {
				for(int i = 0; i<objCount; ++i) {
					PixMap pixmap = doc.ObjectSoup[i] as PixMap;
					if(pixmap==null || pixmap.ImageMask)
						continue;
					Doc destDoc;
					string destRelPath;
					switch(GetSimpleColorSpace(doc, pixmap)) {
					case ColorSpaceType.DeviceGray:
						if(grayDoc==null) {
							grayDoc = new Doc();
							grayRelPath = Path.Combine(externalPath, "GrayImages.pdf");
						}
						destDoc = grayDoc;
						destRelPath = grayRelPath;
						break;
					case ColorSpaceType.DeviceRGB:
						if(rgbDoc==null) {
							rgbDoc = new Doc();
							rgbRelPath = Path.Combine(externalPath, "RGBImages.pdf");
						}
						destDoc = rgbDoc;
						destRelPath = rgbRelPath;
						break;
					case ColorSpaceType.DeviceCMYK:
						if(cmykDoc==null) {
							cmykDoc = new Doc();
							cmykRelPath = Path.Combine(externalPath, "CMYKImages.pdf");
						}
						destDoc = cmykDoc;
						destRelPath = cmykRelPath;
						break;
					default:
						if(otherDoc==null) {
							otherDoc = new Doc();
							otherRelPath = Path.Combine(externalPath, "OtherImages.pdf");
						}
						destDoc = otherDoc;
						destRelPath = otherRelPath;
						break;
					}
					PixMap destPixMap = objCopy.CopyPixMap(pixmap, destDoc, 0);
					destDoc.Rect.Width = destPixMap.Width;
					destDoc.Rect.Height = destPixMap.Height;
					destDoc.Rect.FitIn(maxRect, ContentAlign.Left | ContentAlign.Bottom,
						ContentScaleMode.ShowAll);
					destDoc.MediaBox.String = destDoc.Rect.String;
					destDoc.Page = destDoc.AddPage();
					destDoc.AddImageCopy(destPixMap.ID);

					StreamObject streamObj = (StreamObject)IndirectObject.FromString(
						"<</Type /XObject /Subtype /Form /Resources<</ProcSet [/PDF]>>>>stream\nendstream\n");
					doc.ObjectSoup[i] = streamObj;
					streamObj = (StreamObject)doc.ObjectSoup[i];
					streamObj.Version = 14;
					int id = streamObj.ID;
					int localCount = Math.Min(ObjectCopy.XObjectLocalEntryCount,
						objCopy.XObjectLocalEntryList.Count);
					for(int j = 0; j<localCount; ++j) {
						if(objCopy.XObjectLocalEntryList[j]!=null)
							doc.SetInfo(id, ObjectCopy.GetXObjectLocalEntryPath(j),
								objCopy.XObjectLocalEntryList[j]);
					}
					// Entries in referenced objects (Mask and SMask)
					// that are not copied are stored here for later restoration
					// However, these entries rarely exist
					for(int j = ObjectCopy.XObjectLocalEntryCount;
						j<objCopy.XObjectLocalEntryList.Count; ++j)
					{
						doc.SetInfo(id, "/MyMaskLocalEntries[]",
							objCopy.XObjectLocalEntryList[j]?? "null");
					}
					string bbox = destDoc.Rect.String;
					doc.SetInfo(id, "/BBox:Rect", bbox);
					double scaleX = 1 / destDoc.Rect.Width;
					double scaleY = 1 / destDoc.Rect.Height;
					double translateX = -scaleX * destDoc.Rect.Left;
					double translateY = -scaleY * destDoc.Rect.Bottom;
					XTransform xform = new XTransform(scaleX, 0, 0, scaleY, translateX, translateY);
					doc.SetInfo(id, "/Matrix", "[" + xform.String + "]");
					FileSpecification fs = new FileSpecification(doc.ObjectSoup);
					fs.Uri = destRelPath.Replace('\\', '/'); // convert into relative URI
					doc.SetInfo(id, "/Ref/F:Ref", fs.ID);	
					doc.SetInfo(id, "/Ref/Page:Num", destDoc.PageNumber-1);
					doc.SetInfo(id, "Stream", bbox + " re f");
				}
				string path, dir;
				if(grayDoc!=null) {
					path = Path.Combine(filePath, grayRelPath);
					dir = Path.GetDirectoryName(path);
					if(dir!=null && !Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					grayDoc.Save(path);
					pdfList.Add(path);
				}
				if(rgbDoc!=null) {
					path = Path.Combine(filePath, rgbRelPath);
					dir = Path.GetDirectoryName(path);
					if(dir!=null && !Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					rgbDoc.Save(path);
					pdfList.Add(path);
				}
				if(cmykDoc!=null) {
					path = Path.Combine(filePath, cmykRelPath);
					dir = Path.GetDirectoryName(path);
					if(dir!=null && !Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					cmykDoc.Save(path);
					pdfList.Add(path);
				}
				if(otherDoc!=null) {
					path = Path.Combine(filePath, otherRelPath);
					dir = Path.GetDirectoryName(path);
					if(dir!=null && !Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					otherDoc.Save(path);
					pdfList.Add(path);
				}
			} finally {
				if(grayDoc!=null)
					grayDoc.Dispose();
				if(rgbDoc!=null)
					rgbDoc.Dispose();
				if(cmykDoc!=null)
					cmykDoc.Dispose();
				if(otherDoc!=null)
					otherDoc.Dispose();
			}
			return pdfList;
		}

		private static ColorSpaceType GetSimpleColorSpace(Doc doc, PixMap pixmap) {
			ColorSpaceType colorspace = pixmap.ColorSpaceType;
			switch(colorspace) {
			case ColorSpaceType.DeviceGray:
			case ColorSpaceType.CalGray:
				return ColorSpaceType.DeviceGray;
			case ColorSpaceType.DeviceRGB:
			case ColorSpaceType.CalRGB:
				return ColorSpaceType.DeviceRGB;
			case ColorSpaceType.DeviceCMYK:
				return ColorSpaceType.DeviceCMYK;
			case ColorSpaceType.ICCBased:
				switch(doc.GetInfo(pixmap.ID, "/ColorSpace*[1]*/Alternate*:Name")) {
				case "DeviceGray":
				case "CalGray":
					return ColorSpaceType.DeviceGray;
				case "DeviceRGB":
				case "CalRGB":
					return ColorSpaceType.DeviceRGB;
				case "DeviceCMYK": return ColorSpaceType.DeviceCMYK;
				}
				goto default;
			case ColorSpaceType.Indexed:
				switch(doc.GetInfo(pixmap.ID, "/ColorSpace*[1]*:Name")) {
				case "DeviceGray":
				case "CalGray":
					return ColorSpaceType.DeviceGray;
				case "DeviceRGB":
				case "CalRGB":
					return ColorSpaceType.DeviceRGB;
				case "DeviceCMYK": return ColorSpaceType.DeviceCMYK;
				}
				goto default;
			case ColorSpaceType.DeviceN:
				switch(doc.GetInfo(pixmap.ID, "/ColorSpace*[2]*:Name")) {
				case "DeviceGray":
				case "CalGray":
					return ColorSpaceType.DeviceGray;
				case "DeviceRGB":
				case "CalRGB":
					return ColorSpaceType.DeviceRGB;
				case "DeviceCMYK": return ColorSpaceType.DeviceCMYK;
				}
				goto default;
			default: return colorspace;
			}
		}

		private static void ConvertRefXObjectsToImages(Doc doc, string filePath) {
			Dictionary<string, Doc> docDict = new Dictionary<string, Doc>();
			string[] localEntryList = new string[ObjectCopy.XObjectLocalEntryCount];
			int objCount = doc.ObjectSoup.Count;
			try {
				for(int i = 0; i<objCount; ++i) {
					string imageFile = doc.GetInfo(i, "/Ref*/F*/F*:Text");
					if(imageFile==string.Empty)
						continue;
					imageFile = imageFile.Replace('/', '\\'); // convert URI into path
					imageFile = Path.Combine(filePath, imageFile);
					Doc imageDoc;
					if(!docDict.TryGetValue(imageFile, out imageDoc)) {
						imageDoc = new Doc();
						docDict.Add(imageFile, imageDoc);
						imageDoc.Read(imageFile);
					}
					int pageIndex = doc.GetInfoInt(i, "/Ref*/Page*:Num");
					imageDoc.PageNumber = pageIndex+1;
					string key = imageDoc.GetInfo(imageDoc.Page, "/Resources*/XObject*:Keys");
					int k = key.IndexOf(',');	// There should be only one XObject, which is the image
					if(k>=0)
						key = key.Substring(0, k);
					int pixmapID = imageDoc.GetInfoInt(imageDoc.Page, "/Resources*/XObject*/"+key+":Ref");
					PixMap pixmap = (PixMap)imageDoc.ObjectSoup[pixmapID];
					for(int j = 0; j<ObjectCopy.XObjectLocalEntryCount; ++j) {
						localEntryList[j] = doc.GetInfo(i, ObjectCopy.GetXObjectLocalEntryPath(j));
					}
					ArrayAtom maskLocalEntries = ((DictAtom)pixmap.Atom)["MyMaskLocalEntries"] as ArrayAtom;
					ObjectCopy objCopy = new ObjectCopy(imageDoc);
					pixmap = objCopy.CopyPixMap(pixmap, doc, i);
					for(int j = 0; j<ObjectCopy.XObjectLocalEntryCount; ++j) {
						if(localEntryList[j]!=string.Empty)
							doc.SetInfo(i, ObjectCopy.GetXObjectLocalEntryPath(j), localEntryList[j]);
					}
					if(maskLocalEntries!=null) {
						int j = 0, count;
						int maskID = doc.GetInfoInt(i, "/Mask:Ref");
						if(maskID!=0) {
							count = Math.Min(ObjectCopy.XObjectLocalEntryCount, maskLocalEntries.Count);
							for(; j<count; ++j) {
								string v = maskLocalEntries[j].ToString();
								if(v!="null")
									doc.SetInfo(maskID, ObjectCopy.GetXObjectLocalEntryPath(j), v);
							}
						}
						if(j<maskLocalEntries.Count) {
							maskID = doc.GetInfoInt(i, "/SMask:Ref");
							if(maskID!=0) {
								count = Math.Min(ObjectCopy.XObjectLocalEntryCount, maskLocalEntries.Count-j);
								for(k = 0; k<count; ++k) {
									string v = maskLocalEntries[j+k].ToString();
									if(v!="null")
										doc.SetInfo(maskID, ObjectCopy.GetXObjectLocalEntryPath(k), v);
								}
							}
						}
					}
				}
			} finally {
				foreach(Doc imageDoc in docDict.Values) {
					imageDoc.Dispose();
				}
			}
		}
	}
}
