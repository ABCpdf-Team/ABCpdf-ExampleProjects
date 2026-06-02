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
using System.Collections;
using System.Xml;
using System.Globalization;
using WebSupergoo.ABCpdf14;


namespace PDFTableExamples {

	public class SpaceSampleItem {
		public ArrayList header = new ArrayList();
		public ArrayList info = new ArrayList();
	}

	/// <summary>
	/// SpaceSampleData class is used to represent example information stored in xml file
	/// </summary>
	public class SpaceSampleData {
	
		public SpaceSampleData(){}

		public ArrayList Items = new ArrayList();

		/// <summary>
		/// Load data from xml file
		/// </summary>
		/// <param name="theResPath">Path to the images folder</param>
		/// <param name="inFile">Xml file path</param>
		public void LoadFromFile(string theResPath, string inFile) 
		{
			try {
				XmlTextReader xmlReader = new XmlTextReader(inFile);
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(xmlReader);

				XmlNode samples = xmlDoc["Samples"];
				for ( int i = 0; i < samples.ChildNodes.Count; i++) {
					XmlNode sample = samples.ChildNodes[i];
					SpaceSampleItem item = new SpaceSampleItem();

					if (sample.Attributes["image"] != null)	{
						XImage image = new XImage();
						image.SetFile(theResPath + sample.Attributes["image"].Value);
						item.header.Add(image);
					}
					if (sample.Attributes["image_id"] != null)
						item.header.Add( "<B> Photo ID:</B><BR><BR> " + sample.Attributes["image_id"].Value );
					if (sample.Attributes["program"] != null)
						item.header.Add( "<B> Program:</B><BR><BR> " + sample.Attributes["program"].Value );
					if (sample.Attributes["mission"] != null)
						item.header.Add( "<B> Mission:</B><BR><BR> " + sample.Attributes["mission"].Value );
					if (sample.Attributes["date"] != null)
						item.header.Add( "<B> Date:</B><BR><BR> " + sample.Attributes["date"].Value );
					if (sample.Attributes["film_type"] != null)
						item.header.Add( "<B> Film Type:</B><BR><BR> " + sample.Attributes["film_type"].Value );

					item.info.Add("<B> Title:</B><BR> " + sample["title"].InnerXml);
					item.info.Add("<B> Description:</B><BR> " + sample["description"].InnerXml);
					item.info.Add("<B> Subject Terms:</B><BR> " + sample["subject"].InnerXml);

					Items.Add(item);	
				}
			}
			catch {
				//Invalid xml file
			}
		}

	}
}
