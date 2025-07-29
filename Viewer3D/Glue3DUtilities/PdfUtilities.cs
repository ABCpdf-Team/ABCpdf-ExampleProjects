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

using System.Collections.Generic;
using System.IO;
using System.Text;
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;

namespace Glue3DUtilities
{
	/// <summary>Static class containing utilities to extract specific data from a PDF file</summary>
	public static class PdfUtilities
	{
		/// <summary>Helper struct to store a view and associate it with a name</summary>
		public class ViewInfo
		{
			/// <summary>Construct an instance</summary>
			public ViewInfo(string viewPath, string viewData)
			{
				Path = viewPath;
				View = viewData;
			}

			/// <summary>Name of the stream (composed from page, annotation and type of the stream)</summary>
			public string Path { get; private set; }
			/// <summary>The string instance containing the view data</summary>
			public string View { get; private set; }
		}

		/// <summary>Helper struct to store a stream and associate it with a name</summary>
		public class StreamInfo
		{
			/// <summary>Construct an instance</summary>
			public StreamInfo(string streamPath, byte[] streamData)
			{
				Path = streamPath;
				Stream = streamData;
				Views = new List<ViewInfo>();
			}

			/// <summary>Name of the stream (composed from page, annotation and type of the stream)</summary>
			public string Path { get; private set; }
			/// <summary>The actual stream containing the 3D data</summary>
			public byte[] Stream { get; private set; }
			/// <summary>The list of 3D views associated with this stream</summary>
			public List<ViewInfo> Views { get; set; }
		}

		/// <summary>Settings to use when extracting 3D data from a PDF</summary>
		public class ExtractionSettings
		{
			/// <summary>Construct an instance</summary>
			public ExtractionSettings()
			{
				ProcessU3D = ProcessPRC = false;
				ProcessViews = false;
				MaxViewsPerStream = 0;
			}

			/// <summary>Whether U3D data should be extracted</summary>
			public bool ProcessU3D { get; set; }
			/// <summary>Whether PRC data should be extracted</summary>
			public bool ProcessPRC { get; set; }
			/// <summary>Whether 3D views should be extracted</summary>
			public bool ProcessViews { get; set; }
			/// <summary>If there are a lot of views available, limit the number of views to this number (0 means no limit)</summary>
			public int MaxViewsPerStream { get; set; }
		}

		/// <summary>Struct containing information about the results of the extraction process</summary>
		public class ExtractionResult 
		{
			/// <summary>Construct an instance</summary>
			public ExtractionResult()
			{
				Streams = new List<StreamInfo>();
				StreamsProcessed = PRCDataProcessed = U3DDataProcessed = 0;
				ViewsProcessed = 0;
			}

			/// <summary>The list of 3D streams</summary>
			public List<StreamInfo> Streams { get; set; }
			/// <summary>Counts the number of 3D streams that were encountered</summary>
			public int StreamsProcessed { get; set; }
			/// <summary>Counts the number of 3D streams that were PRC type</summary>
			public int PRCDataProcessed { get; set; }
			/// <summary>Counts the number 3D streams thate were U3D type</summary>
			public int U3DDataProcessed { get; set; }
			/// <summary>Counts the number of views that were processed</summary>
			public int ViewsProcessed { get; set; }
		}

		/// <summary>Convenience function to extract all 3D data (prc, u3d and views) from a pdf file and write them to individual files</summary>
		public static bool ExtractAll3DDataToFiles(string filepath)
		{
			ExtractionResult result = null;
			if (!ExtractAll3DData(filepath, out result))
				return false;

			for( int i = 0; i < result.Streams.Count; ++i )
			{
				StreamInfo currStream = result.Streams[i];

				File.WriteAllBytes(currStream.Path, currStream.Stream);

				for (int j = 0; j < currStream.Views.Count; ++j)
				{
					File.WriteAllText(currStream.Views[j].Path, currStream.Views[j].View.ToString());
				}
			}

			return true;
		}

		/// <summary>Convenience function to extract all 3D data (prc, u3d and views) from a pdf file</summary>
		public static bool ExtractAll3DData(string filepath, out ExtractionResult result)
		{
			ExtractionSettings extractSettings = new ExtractionSettings();
			extractSettings.MaxViewsPerStream = 0;
			extractSettings.ProcessPRC = true;
			extractSettings.ProcessU3D = true;
			extractSettings.ProcessViews = true;

			if (!ProcessPDF(filepath, extractSettings, out result))
				return false;

			return true;
		}

		/// <summary>Convenience function to extract all 3D views from a pdf file</summary>
		public static bool ExtractAll3DViews(string filepath, out ExtractionResult result)
		{
			ExtractionSettings extractSettings = new ExtractionSettings();
			extractSettings.MaxViewsPerStream = 0;
			extractSettings.ProcessPRC = false;
			extractSettings.ProcessU3D = false;
			extractSettings.ProcessViews = true;

			if (!ProcessPDF(filepath, extractSettings, out result))
				return false;

			return true;
		}

		/// <summary>Process a PDF file and extract 3D data and/or views from it</summary>
		public static bool ProcessPDF(string filepath, ExtractionSettings extractSettings, out ExtractionResult outResult)
		{
			// create result object, and check if any 3d data needs to be written
			outResult = new ExtractionResult();

			// open and process the document
			using (Doc theDoc = new Doc()) 
			{
				theDoc.Read(filepath);

				// go through all the pages
				for (int pageNum = 1; pageNum <= theDoc.PageCount; pageNum++)
				{
					theDoc.PageNumber = pageNum;
					Page p = theDoc.ObjectSoup[theDoc.Page] as Page;

					// get annotations of the page
					Annotation[] annots = p.GetAnnotations();
					foreach (Annotation annot in annots)
					{
						// we need a 3D annotation
						if (annot.SubType != "3D")
							continue;

						DeUnicode(theDoc, theDoc.ObjectSoup[annot.ID].Atom);
						DictAtom dict3D = theDoc.ObjectSoup[annot.ID].Atom as DictAtom;
						if (dict3D == null)
							continue;

						// viewbox info, take 3DB if available, otherwise fall back to annotation rectangle (as per PDF spec 1.7)
						string viewRectStr = "";
						if (dict3D.Contains("3DB"))
						{
							ArrayAtom dict3DB = dict3D["3DB"] as ArrayAtom;
							if (dict3DB == null)
							{
								RefAtom ref3DB = dict3D["3DB"] as RefAtom;
								if (ref3DB == null)
									continue;

								dict3DB = theDoc.ObjectSoup[ref3DB.ID].Atom as ArrayAtom;
								if (dict3DB == null)
									continue;
							}
							viewRectStr = "3DB " + dict3DB.ToString();
						}
						else
						{
							viewRectStr = "3DB [0 0 " + annot.Rect.Width + " " + annot.Rect.Height + "]";
						}

						// find the 3d stream data (3DD)
						StreamInfo curr = null;
						int id = theDoc.GetInfoInt(annot.ID, "/3DD:Ref");
						if (id != 0)
						{
							// get the stream object
							D3DStream streamObj = theDoc.ObjectSoup[id] as D3DStream;
							if (streamObj == null)
								continue;

							// update numbers
							outResult.StreamsProcessed++;

							// check the subtype
							DictAtom dict3DD = streamObj.Atom as DictAtom;
							bool doWrite = true;
							if (dict3DD != null)
							{
								string objSubtype = dict3DD["Subtype"].ToString();
								if ((objSubtype == "/PRC" && extractSettings.ProcessPRC == false) ||
									(objSubtype == "/U3D" && extractSettings.ProcessU3D == false))
								{
									doWrite = false;
								}
							}

							// check whether to decompress
							if( streamObj.Compressed )
							{
								// try to decompress
								if (!streamObj.Decompress())
									continue;
							}

							// stream data
							byte[] streamData = streamObj.GetData();
							if (streamData.Length < 10)
								continue;

							// add to result data
							bool isU3D = streamData[0] == 'U';
							string streamPath = filepath + "_p" + pageNum.ToString() + "_an" + annot.ID.ToString() + (isU3D ? ".u3d" : ".prc");
							curr = new StreamInfo(streamPath, doWrite ? streamData : null);
							outResult.Streams.Add(curr);

							// update numbers
							if (isU3D)
								outResult.U3DDataProcessed++;
							else
								outResult.PRCDataProcessed++;

							// extract 3D views from 3DD's view array (VA)
							if (extractSettings.ProcessViews)
							{ 
								DeUnicode(theDoc, theDoc.ObjectSoup[id].Atom);

								int va = theDoc.GetInfoInt(id, "/VA:Ref");
								if (va != 0)
									DeUnicode(theDoc, theDoc.ObjectSoup[va].Atom);

								int numViews = theDoc.GetInfoInt(id, "/VA*:Count");
								if (extractSettings.MaxViewsPerStream > 0 && numViews > extractSettings.MaxViewsPerStream)
									numViews = extractSettings.MaxViewsPerStream;
								StringBuilder viewData = new StringBuilder();
								for (int j = 0; j < numViews; j++)
								{
									string opath = "/VA[" + j.ToString() + "]*";
									string[] keys = theDoc.GetInfo(id, opath + ":Keys").Split(new char[] { ',' });
									foreach (string key in keys)
									{
										string val = theDoc.GetInfo(id, opath + "/" + key + "*");
										viewData.Append(key);
										viewData.Append("\t");
										viewData.AppendLine(val);
									}
									if (viewRectStr.Length > 0)
										viewData.AppendLine(viewRectStr);

									// write to result
									string viewPath = filepath + "_p" + pageNum.ToString() + "_an" + annot.ID.ToString() + "_view" + j.ToString() + ".txt";
									if( curr != null )
										curr.Views.Add(new ViewInfo(viewPath, viewData.ToString()));
									
									outResult.ViewsProcessed++;
									viewData.Clear();
								}
							}
						}
						// look for 3DV as well, if we extract views
						if (extractSettings.ProcessViews)
						{
							if (dict3D.Contains("3DV"))
							{
								DictAtom dict3DV = dict3D["3DV"] as DictAtom;
								if (dict3DV == null)
								{
									RefAtom ref3DV = dict3D["3DV"] as RefAtom;
									if (ref3DV == null)
										continue;

									dict3DV = theDoc.ObjectSoup[ref3DV.ID].Atom as DictAtom;
									if (dict3DV == null)
										continue;
								}

								string[] myKeys = dict3DV.GetKeys();
								StringBuilder viewData = new StringBuilder();

								foreach (string currKey in myKeys)
								{
									string currValue = "";

									RefAtom valRef = dict3DV[currKey] as RefAtom;
									if (valRef != null)
									{
										DictAtom refedDic = theDoc.ObjectSoup[valRef.ID].Atom as DictAtom;
										if (refedDic != null)
											currValue = refedDic.ToString();
										else
										{
											ArrayAtom arrAt = theDoc.ObjectSoup[valRef.ID].Atom as ArrayAtom;
											if (arrAt != null)
											{
												currValue = arrAt.ToString();
											}
										}
									}
									else
									{
										currValue = dict3DV[currKey].ToString();
									}

									viewData.Append(currKey);
									viewData.Append("\t");
									viewData.Append(currValue);
									viewData.Append("\n");
								}

								if (viewRectStr.Length > 0)
									viewData.AppendLine(viewRectStr);

								// create the view name/path and write to result
								string viewPath = filepath + "_p" + pageNum.ToString() + "_an" + annot.ID.ToString() + "_view_default.txt";
								outResult.ViewsProcessed++;
								if( curr != null )
									curr.Views.Add(new ViewInfo(viewPath, viewData.ToString()));
							}
						}
					}
				}
			}

			return true;
		}

		private static void DeUnicode(Doc doc, Atom atom)
		{
			if (atom == null)
				return;
			ArrayAtom aa = atom as ArrayAtom;
			DictAtom da = atom as DictAtom;
			int n = 0;
			if (aa != null)
				n = aa.Count;
			if (da != null)
				n = da.Keys.Count;
			for (int i = 0; i < n; i++)
				DeUnicode(doc, Atom.GetItem(atom, i));
			if (atom is StringAtom)
			{
				string str = ((StringAtom)atom).Text;
				int p = str.IndexOf('\0');
				if (p != -1)
					str = str.Substring(0, p);
				((StringAtom)atom).Text = str;
			}
		}

	}
}
