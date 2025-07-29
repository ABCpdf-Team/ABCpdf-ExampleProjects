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

using WebSupergoo.FontUnembedment;
using WebSupergoo.ABCpdf13;

namespace FontUnembedment {
	class Program {
		static void Main(string[] args) {
			//Always manually check the output for problems.
			string inputFile = MapPath("input.pdf");
			string outputFile = MapPath("output.pdf");

			FontUnembedder unembedder = new FontUnembedder();
			// The regular Times is called Times Roman, not Times,
			// so the fonts for different styles are added separately.
			unembedder.AddFont("Times Roman", LanguageType.Latin);
			unembedder.AddFont("Times Bold", LanguageType.Latin);
			unembedder.AddFont("Times Italic", LanguageType.Latin);
			unembedder.AddFont("Times Bold Italic", LanguageType.Latin);
			unembedder.AddFont("Helvetica", LanguageType.Latin);
			unembedder.AddFont("Courier", LanguageType.Latin);
			unembedder.AddFont("Symbol", LanguageType.Latin);
			unembedder.AddFont("Times New Roman", LanguageType.Latin);
			unembedder.AddFont("Arial", LanguageType.Latin);
			unembedder.AddFont("Courier New", LanguageType.Latin);
			unembedder.AddFont("Wingdings", LanguageType.Latin);
			unembedder.AddFont("Verdana", LanguageType.Latin);
			unembedder.AddFont("MingLiU", LanguageType.ChineseT);
			unembedder.AddFont("PMingLiU", LanguageType.ChineseT);
			// Some software changes the names of MingLiU and PMingLiU
			// on Traditional Chinese Windows to "___" and "____" because
			// the font names are 3 or 4 Chinese characters.
			// You'll need to change "___" and "____" depending on
			// the font names in the PDF files.
			//unembedder.AddFont("___", "MingLiU", LanguageType.ChineseT);
			//unembedder.AddFont("____", "PMingLiU", LanguageType.ChineseT);

			using(Doc doc = new Doc()) {
				doc.Read(inputFile);

				SortedDictionary<int, bool> fonts = new SortedDictionary<int, bool>();
				for(int i = 1, count = doc.ObjectSoup.Count; i<count; ++i) {
					if(doc.GetInfo(i, "Type")=="font") {
						if(!fonts.ContainsKey(i))
							fonts.Add(i, false);
						int descendant = doc.GetInfoInt(i, "/DescendantFonts*[0]:Ref");
						if(descendant!=0)
							fonts[descendant] = true;
					}
				}

				List<FontUnembedder.ProcessingInfo> infos = new List<FontUnembedder.ProcessingInfo>();
				unembedder.Process(doc, infos);

				for(int i = 0; i<infos.Count; ++i) {
					FontUnembedder.ProcessingInfo info = infos[i];
					Console.WriteLine("{0} {1} (ID:{2}, Language:{3}) {4} characters:",
						i+1, Quote(doc.GetInfo(info.FontId, "/BaseFont*:Name")),
						info.FontId, info.Language, info.CharacterCount);
					List<FontUnembedder.FontObjectInfo> list = info.SourceFonts;
					for(int j = 0; j<list.Count; ++j) {
						FontUnembedder.FontObjectInfo sourceFont = list[j];
						Console.WriteLine(sourceFont.Id==0?
						"    {0} {2}": "    {0} (ID:{1}) {2}",
							Quote(sourceFont.Name), sourceFont.Id,
							sourceFont.EncodingIsIdentical? "identical encoding":
							string.Format("{0} characters", sourceFont.CharacterCount));
						if(sourceFont.Id!=0)
							fonts.Remove(sourceFont.Id);
					}
				}

				int k = 0;
				foreach(KeyValuePair<int, bool> pair in fonts) {
					if(!pair.Value) {
						if(k<=0)
							Console.WriteLine();
						Console.WriteLine("{0} {1} (ID:{2}) not processed.",
							k+1, Quote(doc.GetInfo(pair.Key, "/BaseFont*:Name")), pair.Key);
						++k;
					}
				}
				doc.Save(outputFile);
			}
		}
		private static string Quote(string s) {
			char[] whiteSpaces = {
				'\0', ' ', '\t', '\n', '\r', '\v', '\f', '\x85', '\u00a0', '\u1680',
				'\u2000', '\u2001', '\u2002', '\u2003', '\u2004', '\u2005',
				'\u2006', '\u2007', '\u2008', '\u2009', '\u200a', '\u200b',
				'\u2028', '\u2029', '\u3000', '\ufeff'
			};
			return s.IndexOfAny(whiteSpaces)<0? s: '"'+s+'"';
		}
		private static string MapPath(string fileName) {
			string basePath = Directory.GetCurrentDirectory();
			return Path.Combine(Directory.GetParent(basePath).Parent.FullName, fileName);
		}
	}
}
