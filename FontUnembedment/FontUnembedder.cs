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
// NB In ABCpdf Version 10 and later, much of the functionality here is
// provided by the ReduceSizeOperation class.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Atoms;

namespace WebSupergoo.FontUnembedment {
	/// <summary>IFontOptions contains the options for font processing.</summary>
	public interface IFontOptions {
		/// <summary>Font name.</summary>
		string Name { get; }

		/// <summary>Font language. This specifies the internal PDF encoding.</summary>
		LanguageType Language { get; }

		/// <summary>Whether the font is embedded.</summary>
		bool Embedded { get; }

		/// <summary>Whether to (embed and) subset the font.</summary>
		bool IsSubset { get; }

		/// <summary>Makes the font referenced and not embedded.
		/// It throws an exception if the font uses the Unicode language
		/// and is thus always embedded.</returns>
		void MakeReferenced();

		/// <summary>Makes the font embedded.</summary>
		/// <param name="subset">Whether to subset the font.</param>
		void MakeEmbedded(bool subset);
	}

	/// <summary>
	/// FontUnembedder unembeds fonts. Always manually check the output for problems.
	/// It does not process annotation appearances.
	/// </summary>
	public sealed class FontUnembedder {
		/// <summary>Information about source font.</summary>
		public sealed class FontObjectInfo {
			private int _id;
			private string _name;
			private bool _encodingIsIdentical;
			private int _characterCount;

			public FontObjectInfo(int id, string name, bool encodingIsIdentical) {
				_id = id;
				_name = name;
				_encodingIsIdentical = encodingIsIdentical;
			}

			/// <summary>Object ID.</summary>
			public int Id { get { return _id; } }

			/// <summary>Font name (including the prefix for font subset).</summary>
			public string Name { get { return _name; } }

			/// <summary>Whether the encoding is identical to that of the replacement font.</summary>
			public bool EncodingIsIdentical { get { return _encodingIsIdentical; } }

			/// <summary>Number of characters processed.</summary>
			public int CharacterCount {
				get { return _characterCount; }
				set { _characterCount = value; }
			}
		}

		/// <summary>Information about destination font.</summary>
		public sealed class ProcessingInfo {
			private int _fontId;
			private string _fontName;
			private LanguageType _language;
			private int _characterCount;
			private List<FontObjectInfo> _sourceFonts;

			public ProcessingInfo(int fontId, string fontName, LanguageType langauge) {
				_fontId = fontId;
				_fontName = fontName;
				_language = langauge;
				_sourceFonts = new List<FontObjectInfo>();
			}

			/// <summary>Object ID.</summary>
			public int FontId { get { return _fontId; } }

			/// <summary>Font name.</summary>
			public string FontName { get { return _fontName; } }

			/// <summary>Language.</summary>
			public LanguageType Language { get { return _language; } }

			/// <summary>Number of characters processed, the sum of CharacterCount in SourceFonts.</summary>
			public int CharacterCount {
				get { return _characterCount; }
				set { _characterCount = value; }
			}

			/// <summary>The list of source fonts replaced by this font.</summary>
			public List<FontObjectInfo> SourceFonts { get { return _sourceFonts; } }
		}

		private sealed class TargetFont: IFontOptions {
			public string Name;
			public LanguageType Language;
			public bool Embed;
			public bool Subset;

			public TargetFont(string name, LanguageType language) {
				Name = name;
				Language = language;
				if(GetEmbed(language)) {
					Embed = true;
					Subset = true;
				}
			}

			public static bool GetEmbed(LanguageType language) {
				return language==LanguageType.Unicode;
			}

			string IFontOptions.Name { get { return Name; } }
			LanguageType IFontOptions.Language { get { return Language; } }
			bool IFontOptions.Embedded { get { return Embed; } }
			bool IFontOptions.IsSubset { get { return Subset; } }

			void IFontOptions.MakeReferenced() {
				if(GetEmbed(Language))
					throw new NotSupportedException("Font in the Unicode language cannot be referenced without being embedded.");
				Embed = false;
				Subset = false;
			}
			void IFontOptions.MakeEmbedded(bool subset) {
				Embed = true;
				Subset = subset;
			}
		}

		private sealed class FontIdentity: IEquatable<FontIdentity> {
			public string Name;
			public bool Vertical;

			public FontIdentity(string name, bool vertical) {
				Name = name;
				Vertical = vertical;
			}

			public bool Equals(FontIdentity obj) {
				return Name==obj.Name && Vertical==obj.Vertical;
			}
			public override bool Equals(object obj) {
				FontIdentity v = obj as FontIdentity;
				return v!=null && Equals(v);
			}
			public override int GetHashCode() {
				int v = Name.GetHashCode();
				if(Vertical)
					v = ~v;
				return v;
			}
		}

		private sealed class ReplacementFont {
			public FontIdentity Identity;
			public LanguageType Language;
			public int FontId;
			public bool Subset;
			public int HelperFontId;
			public string NonSpaceCode;
			public string NonSpaceChar;
			public int NonSpaceCharPreference;
			public List<FontObjectInfo> SourceFonts;

			public ReplacementFont(FontIdentity identity, LanguageType language, int fontId, bool subset) {
				Identity = identity;
				Language = language;
				FontId = fontId;
				Subset = subset;
			}

			public void SetNonSpaceChar(string s) {
				const int maxPreference = 9;
				if(NonSpaceCharPreference>=maxPreference || s=="")
					return;

				UnicodeCategory preferenceCategory = default(UnicodeCategory);
				int preference = 0;
				int preferenceIndex = 0;
				for(int i = 0; i<s.Length; ++i) {
					UnicodeCategory category = char.GetUnicodeCategory(s, i);
					int pref = GetUnicodePreference(category);
					if(pref>preference) {
						if(category!=UnicodeCategory.Surrogate
							|| char.IsHighSurrogate(s, i)
							&& i<s.Length-1 && char.IsLowSurrogate(s, i+1))
						{
							preferenceCategory = category;
							preference = pref;
							preferenceIndex = i;
							if(pref>=maxPreference)
								break;
						}
					}
				}
				if(preference<=NonSpaceCharPreference)
					return;
				NonSpaceCharPreference = preference;
				NonSpaceChar = s.Substring(preferenceIndex,
					preferenceCategory!=UnicodeCategory.Surrogate? 1: 2);
			}
			private static int GetUnicodePreference(UnicodeCategory category) {
				switch(category) {
				case UnicodeCategory.SpaceSeparator:
				case UnicodeCategory.LineSeparator:
				case UnicodeCategory.ParagraphSeparator:
				case UnicodeCategory.Control:
				case UnicodeCategory.Format:
					return 0;
				case UnicodeCategory.PrivateUse:
					return 1;
				case UnicodeCategory.Surrogate:
					return 2;
				case UnicodeCategory.SpacingCombiningMark:
					return 3;
				case UnicodeCategory.NonSpacingMark:
				case UnicodeCategory.EnclosingMark:
					return 4;
				case UnicodeCategory.ModifierSymbol:
					return 5;
				case UnicodeCategory.ModifierLetter:
					return 6;
				case UnicodeCategory.OtherPunctuation:
				case UnicodeCategory.OtherSymbol:
					return 7;
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.LetterNumber:
				case UnicodeCategory.OtherNumber:
					return 8;
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.DecimalDigitNumber:
				case UnicodeCategory.ConnectorPunctuation:
				case UnicodeCategory.DashPunctuation:
				case UnicodeCategory.OpenPunctuation:
				case UnicodeCategory.ClosePunctuation:
				case UnicodeCategory.InitialQuotePunctuation:
				case UnicodeCategory.FinalQuotePunctuation:
				case UnicodeCategory.MathSymbol:
				case UnicodeCategory.CurrencySymbol:
					return 9;
				}
				return 1;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		private struct FontResource {
			public FontObjectInfo SourceFont;
			public ReplacementFont Font;

			public FontResource(FontObjectInfo sourceFont, ReplacementFont font) {
				SourceFont = sourceFont;
				Font = font;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		private struct Replacement {
			public FontObjectInfo SourceFont;
			public ReplacementFont Font;
			public int Offset;
			public int Length;
			public List<string> Text;
		}

		[StructLayout(LayoutKind.Auto)]
		private struct Reference {
			public int PageId;
			public int Id;
			public string Path;

			public Reference(int pageId, int id, string path) {
				PageId = pageId;
				Id = id;
				Path = path;
			}
		}

		private sealed class FontReference {
			public ReplacementFont Font;
			public List<Reference> References;

			public FontReference(ReplacementFont font) {
				Font = font;
				References = new List<Reference>();
			}
		}

		[StructLayout(LayoutKind.Auto)]
		private struct SourceFontReference {
			public FontObjectInfo SourceFont;
			public FontReference FontRef;

			public SourceFontReference(FontObjectInfo sourceFont,
				FontReference fontRef)
			{
				SourceFont = sourceFont;
				FontRef = fontRef;
			}
		}


		private readonly char[] _stringStart;
		private LanguageType? _defaultLanguage;
		private Dictionary<string, TargetFont> _fonts;
		private Dictionary<string, TargetFont> _fontVariants;
		private bool _forceDefaultLanguage;

		public FontUnembedder() {
			_stringStart = new char[] { '(', '<' };
			_fonts = new Dictionary<string, TargetFont>(StringComparer.OrdinalIgnoreCase);
			_fontVariants = new Dictionary<string, TargetFont>(StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>The default language. If the property is not null, all fonts are processed.</summary>
		public LanguageType? DefaultLanguage {
			get { return _defaultLanguage; }
			set { _defaultLanguage = value; }
		}

		/// <summary>Whether to throw an exception when it fails to process fonts using DefaultLanguage.</summary>
		public bool ForceDefaultLanguage {
			get { return _forceDefaultLanguage; }
			set { _forceDefaultLanguage = value; }
		}

		/// <summary>Adds font for processing. An exception is thrown in the Process method
		/// if it fails to process an added font.</summary>
		/// <param name="family">Font family name.</param>
		/// <param name="language">Language. If it is Unicode, the destination font
		/// is embedded and subsetted by default.</param>
		/// <returns>Font options.</returns>
		public IFontOptions AddFont(string family, LanguageType language) {
			return AddFont(family, family, language);
		}

		/// <summary>Adds font for processing. An exception is thrown in the Process method
		/// if it fails to process an added font.</summary>
		/// <param name="family">Source font family name.</param>
		/// <param name="targetFamily">Destination font family name.</param>
		/// <param name="language">Language. If it is Unicode, the destination font
		/// is embedded and subsetted by default.</param>
		/// <returns>Font options.</returns>
		public IFontOptions AddFont(string family, string targetFamily, LanguageType language) {
			TargetFont target = new TargetFont(targetFamily, language);
			_fonts.Add(family, target);
			if(family.IndexOf(' ')<0)
				AddFontNames(_fontVariants, family, target, false);
			else {
				AddFontNames(_fontVariants, family.Replace(" ", ""), target, true);
				AddFontNames(_fontVariants, family.Replace(' ', '+'), target, true);
				AddFontNames(_fontVariants, family.Replace(' ', '_'), target, true);
				AddFontNames(_fontVariants, family.Replace(' ', '-'), target, true);
			}
			return target;
		}

		private static void AddFontNames(Dictionary<string, TargetFont> fontVariants,
			string name, TargetFont target, bool addBaseName)
		{
			if(addBaseName && !fontVariants.ContainsKey(name))
				fontVariants.Add(name, target);
			string s = name+"PS";
			if(!fontVariants.ContainsKey(s))
				fontVariants.Add(s, target);
			s = name+"MT";
			if(!fontVariants.ContainsKey(s))
				fontVariants.Add(s, target);
			s = name+"PSMT";
			if(!fontVariants.ContainsKey(s))
				fontVariants.Add(s, target);
		}

		private bool GetTargetFont(string name, out TargetFont outTarget,
			out string outTargetName)
		{
			if(_fonts.TryGetValue(name, out outTarget)
				|| _fontVariants.TryGetValue(name, out outTarget))
			{
				outTargetName = outTarget.Name;
				return true;
			}

			char[] anyOf = new char[] { '-', ',' };
			string name0 = name;
			int i;
			while((i = name0.LastIndexOfAny(anyOf))>=0) {
				name0 = name0.Substring(0, i);
				if(_fonts.TryGetValue(name0, out outTarget)
					|| _fontVariants.TryGetValue(name0, out outTarget))
				{
					int removeCount = 0;
					if(name.EndsWith("PSMT", StringComparison.Ordinal))
						removeCount = 4;
					else if(name.EndsWith("MT", StringComparison.Ordinal)
						|| name.EndsWith("PS", StringComparison.Ordinal))
						removeCount = 2;
					outTargetName = outTarget.Name+name.Substring(
						i, name.Length-i-removeCount);
					return true;
				}
			}
			outTargetName = null;
			return false;
		}

		public void Process(Doc doc, ICollection<ProcessingInfo> infos) {
			if(_fonts.Count<=0 && !_defaultLanguage.HasValue)
				return;

			SortedDictionary<int, bool> objectMap = new SortedDictionary<int, bool>();

			SortedDictionary<int, SourceFontReference> fontMap
				= new SortedDictionary<int, SourceFontReference>();	// int:source font id
			Dictionary<FontIdentity, FontReference> uniqueFontMap
				= new Dictionary<FontIdentity, FontReference>();
			SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Replacement>>> replaceMap
				= new SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Replacement>>>();	// int:stream id, int:page id, int:stream offset
			int resourceStreamId = 0, resourcePageId = 0;
			Dictionary<string, FontResource> fontResources
				= new Dictionary<string, FontResource>();
			int oldPageId = doc.Page;
			try {
				for(int i = 0, count = doc.PageCount; i<count; ++i) {
					doc.PageNumber = i+1;
					AddSourceFonts(doc, infos!=null, doc.Page,
						fontMap, uniqueFontMap, objectMap);

					doc.Flatten();
					string content = doc.GetText("SVG+");
					XmlDocument xml;
					using(StringReader reader = new StringReader(content)) {
						xml = new XmlDocument();
						xml.Load(reader);
					}
					XmlNode svg = xml["svg"];
					XmlNode node = svg==null? null: svg.FirstChild;
					while(node!=svg) {
						if(node.Name=="text")
							ProcessText(doc, infos!=null, node, fontMap, uniqueFontMap,
								replaceMap, ref resourceStreamId, ref resourcePageId,
								fontResources);

						if(node.FirstChild!=null)
							node = node.FirstChild;
						else if(node.NextSibling!=null)
							node = node.NextSibling;
						else {
							do {
								node = node.ParentNode;
							} while(node!=svg && node.NextSibling==null);
							if(node!=svg)
								node = node.NextSibling;
						}
					}
					resourceStreamId = 0;
					resourcePageId = 0;
					fontResources.Clear();
				}
			} finally {
				doc.Page = oldPageId;
			}

			SortedDictionary<int, SortedDictionary<int, int>> streamRemap
				= new SortedDictionary<int, SortedDictionary<int, int>>();	// int:page id, int stream id
			// replace text operands in content streams
			int pageId = 0;
			int oldFontId = 0;
			double oldFontSize = 0;
			XColor oldColor = null;
			double oldRectLeft = 0, oldRectBottm = 0, oldRectRight = 0, oldRectTop = 0;
			XTextStyle.KerningType oldKerning = XTextStyle.KerningType.None;

			Doc helperDoc = null;
			StringBuilder builder = new StringBuilder();
			try {
				foreach(KeyValuePair<int, SortedDictionary<int, SortedDictionary<int, Replacement>>> streamPair in replaceMap) {
					doc.GetInfo(streamPair.Key, "Decompress");
					string content = doc.GetInfo(streamPair.Key, "Stream");
					bool hasUnchanged = false;
					int pageCount = streamPair.Value.Count;
					foreach(KeyValuePair<int, SortedDictionary<int, Replacement>> pagePair in streamPair.Value) {
						--pageCount;
						SortedDictionary<int, int> remap = null;
						int iContent = 0;
						foreach(Replacement replace in pagePair.Value.Values) {
							if(replace.SourceFont.EncodingIsIdentical && !replace.Font.Subset)
								continue;
							int iEnd = replace.Offset;
							int opEnd = replace.Offset+replace.Length;
							for(int j = 0; j<replace.Text.Count; ++j) {
								int i = content.IndexOfAny(_stringStart, iEnd, opEnd-iEnd);
								if(i<0)
									throw new ApplicationException("Unable to find text operand.");
								iEnd = GetStringEnd(content, i, opEnd);
								if(iEnd<0)
									throw new ApplicationException("Unable to find text operand end.");
								string s = replace.Text[j];
								if(s!="") {
									if(replace.SourceFont!=null)
										replace.SourceFont.CharacterCount += s.Length;
									int extraCount;
									int id;
									if(replace.Font.Subset) {
										if(pageId==0) {
											oldFontId = doc.Font;
											oldFontSize = doc.TextStyle.Size;
											if(oldColor==null)
												oldColor = new XColor();
											oldColor.SetColor(doc.Color);
											oldRectLeft = doc.Rect.Left;
											oldRectBottm = doc.Rect.Bottom;
											oldRectRight = doc.Rect.Right;
											oldRectTop = doc.Rect.Top;
											oldKerning = doc.TextStyle.Kerning;

											// Use a detached page to avoid changes to the page tree
											doc.Page = pageId = doc.AddObject("<</Type /Page /MediaBox ["
												+ doc.MediaBox.String + "] >>");
											doc.FontSize = 8;
											doc.Color.Alpha = 255;
											doc.TextStyle.Kerning = XTextStyle.KerningType.None;
										}
										doc.Font = replace.Font.FontId;
										doc.Rect.SetSides(0, 0, 10000, 10000);	// make sure it is big enough and reset Pos
										id = AddText(doc, replace.Font, s, out extraCount);
										if(replace.SourceFont.EncodingIsIdentical)
											continue;
										s = doc.GetInfo(id, "Stream");
										doc.Flatten();	// transfer used characters to the font
										int newId = doc.GetInfoInt(doc.Page, "/Contents*[-1]:Ref");
										if(newId!=0)
											doc.ObjectSoup.RemoveAt(newId);	// avoid "Object ID unrealistically large"
										doc.SetInfo(doc.Page, "/Contents*[-1]:Del", "");
										// remove id last. order is important
										if(id!=0)
											doc.ObjectSoup.RemoveAt(id);	// avoid "Object ID unrealistically large"
									} else {
										if(helperDoc==null) {
											helperDoc = new Doc();
											helperDoc.TextStyle.Kerning = XTextStyle.KerningType.None;
										}
										if(replace.Font.HelperFontId==0) {
											bool embed = TargetFont.GetEmbed(replace.Font.Language);
											replace.Font.HelperFontId = AddDocFont(helperDoc,
												replace.Font.Identity, replace.Font.Language,
												embed, embed);
											if(replace.Font.HelperFontId==0)
												throw new ApplicationException(string.Format(
													"Unable to add font \"{0}\" in language {1}.",
													replace.Font.Identity, replace.Font.Language));
										}
										helperDoc.Font = replace.Font.HelperFontId;
										helperDoc.Rect.SetSides(0, 0, 10000, 10000);	// make sure it is big enough and reset Pos
										id = AddText(helperDoc, replace.Font, s, out extraCount);
										s = helperDoc.GetInfo(id, "Stream");
										helperDoc.SetInfo(helperDoc.Page, "/Contents*[-1]:Del", "");
										if(id!=0)
											helperDoc.ObjectSoup.RemoveAt(id);	// avoid "Object ID unrealistically large"
									}
									if(s.Length<=0)	// this can happen - e.g. a zwsp char
										s = "()";
									int k = s.IndexOfAny(_stringStart);
									if(k<0)
										throw new ApplicationException("Unable to find text operand.");
									int kEnd = GetStringEnd(s, k, s.Length);
									if(kEnd<0)
										throw new ApplicationException("Unable to find text operand end.");
									builder.Append(content, iContent, i-iContent);
									if(extraCount<=0)
										builder.Append(s, k, kEnd-k);
									else {
										builder.Append(s, k, kEnd-k-extraCount-1);
										builder.Append(s, kEnd-1, 1);
									}
									iContent = iEnd;
								}
							}
						}
						if(iContent<=0)
							hasUnchanged = true;
						else {
							builder.Append(content, iContent, content.Length-iContent);
							if(pageCount<=0 && !hasUnchanged)
								doc.SetInfo(streamPair.Key, "Stream", builder.ToString());
							else {
								Debug.Assert(pagePair.Key>0);
								DictAtom dict = (DictAtom)doc.ObjectSoup[streamPair.Key].Atom;
								int id = doc.AddObject(dict.ToString()+"stream\nendstream\n");
								doc.SetInfo(id, "Stream", builder.ToString());
								doc.GetInfo(id, "Compress");
								if(remap==null && !streamRemap.TryGetValue(pagePair.Key, out remap)) {
									remap = new SortedDictionary<int, int>();
									streamRemap.Add(pagePair.Key, remap);
								}
								remap.Add(streamPair.Key, id);
							}
							builder.Length = 0;
						}
					}
					doc.GetInfo(streamPair.Key, "Compress");
				}
			} finally {
				if(helperDoc!=null)
					helperDoc.Dispose();
				if(pageId!=0) {
					doc.Page = oldPageId;
					doc.Delete(pageId);
					doc.Font = oldFontId;
					doc.TextStyle.Size = oldFontSize;
					if(oldColor!=null)
						doc.Color.SetColor(oldColor);
					doc.Rect.SetSides(oldRectLeft, oldRectBottm, oldRectRight, oldRectTop);
					doc.TextStyle.Kerning = oldKerning;
				}
			}
			Dictionary<int, ProcessingInfo> infoMap = infos==null? null:
				new Dictionary<int, ProcessingInfo>();
			foreach(SourceFontReference sourceFontRef in fontMap.Values) {
				List<Reference> list = sourceFontRef.FontRef.References;
				for(int i = 0; i<list.Count; ++i) {	// update font references
					int id = list[i].Id;
					if(list[i].PageId>0) {
						SortedDictionary<int, int> remap;
						if(streamRemap.TryGetValue(list[i].PageId, out remap)) {
							int remappedId;
							if(remap.TryGetValue(id, out remappedId))
								id = remappedId;
						}
					}
					doc.SetInfo(id, list[i].Path,
						sourceFontRef.FontRef.Font.FontId);
				}
				if(infoMap!=null) {	// collect statistics
					ReplacementFont font = sourceFontRef.FontRef.Font;
					ProcessingInfo info;
					if(!infoMap.TryGetValue(font.FontId, out info)) {
						info = new ProcessingInfo(font.FontId, font.Identity.Name, font.Language);
						infoMap.Add(font.FontId, info);
					}
					info.SourceFonts.Add(sourceFontRef.SourceFont);
					info.CharacterCount += sourceFontRef.SourceFont.CharacterCount;
				}
			}
			// remap stream references
			foreach(KeyValuePair<int, SortedDictionary<int, int>> pagePair in streamRemap) {
				objectMap.Clear();
				RemapXObjects(doc, pagePair.Key, pagePair.Value, objectMap);

				int count = doc.GetInfoInt(pagePair.Key, "/Contents*:Count");
				if(count>0) {
					bool remapped = false;
					for(int i = 0; i<count; ++i) {
						string path = "/Contents*["+i.ToString()+"]:Ref";
						int id = doc.GetInfoInt(pagePair.Key, path);
						if(pagePair.Value.TryGetValue(id, out id)) {
							if(!remapped) {
								remapped = true;
								if(doc.GetInfoInt(pagePair.Key, "/Contents:Ref")!=0) {
									string arr = doc.GetInfo(pagePair.Key, "/Contents*");
									doc.SetInfo(pagePair.Key, "/Contents", arr);
								}
							}
							doc.SetInfo(pagePair.Key, path, id);
						}
					}
				} else {
					int id = doc.GetInfoInt(pagePair.Key, "/Contents:Ref");
					if(pagePair.Value.TryGetValue(id, out id))
						doc.SetInfo(pagePair.Key, "/Contents:Ref", id);
				}
			}
			if(infoMap!=null) {	// return statistics
				foreach(ProcessingInfo info in infoMap.Values)
					infos.Add(info);
			}
		}
		private static void RemapXObjects(Doc doc, int resourceObj,
			SortedDictionary<int, int> remap, SortedDictionary<int, bool> objectMap)
		{
			string keys = doc.GetInfo(resourceObj, "/Resources*/XObject*:Keys");
			if(keys=="")
				return;

			bool remapped = false;
			int i = 0;
			while(true) {
				int j = keys.IndexOf(',', i);
				string key = j<0? keys.Substring(i): keys.Substring(i, j-i);
				string path = "/Resources*/XObject*/"+key+":Ref";
				int id = doc.GetInfoInt(resourceObj, path);
				if(remap.TryGetValue(id, out id)) {
					if(!remapped) {
						remapped = true;
						string dict;
						if(doc.GetInfoInt(resourceObj, "/Resources:Ref")!=0) {
							dict = doc.GetInfo(resourceObj, "/Resources*");
							doc.SetInfo(resourceObj, "/Resources", dict);
						}
						if(doc.GetInfoInt(resourceObj, "/Resources/XObject:Ref")!=0) {
							dict = doc.GetInfo(resourceObj, "/Resources/XObject*");
							doc.SetInfo(resourceObj, "/Resources/XObject", dict);
						}
					}
					doc.SetInfo(resourceObj, path, id);
				}
				if(!objectMap.ContainsKey(id)) {
					objectMap.Add(id, false);
					RemapXObjects(doc, id, remap, objectMap);
				}
				if(j<0)
					break;
				i = j+1;
			}
		}

		private void AddSourceFonts(Doc doc, bool report, int resourceObj,
			SortedDictionary<int, SourceFontReference> fontMap,
			Dictionary<FontIdentity, FontReference> uniqueFontMap,
			SortedDictionary<int, bool> objectMap)
		{	// fonts not used in Tj/TJ need this method to get processed
			int i;
			string keys = doc.GetInfo(resourceObj, "/Resources*/Font*:Keys");
			if(keys!="") {
				i = 0;
				while(true) {
					int j = keys.IndexOf(',', i);
					string resourceName = j<0? keys.Substring(i): keys.Substring(i, j-i);
					GetSourceFont(doc, report, 0, resourceObj, resourceName,
						fontMap, uniqueFontMap);
					if(j<0)
						break;
					i = j+1;
				}
			}
			keys = doc.GetInfo(resourceObj, "/Resources*/XObject*:Keys");
			if(keys=="")
				return;
			i = 0;
			while(true) {
				int j = keys.IndexOf(',', i);
				string key = j<0? keys.Substring(i): keys.Substring(i, j-i);
				int id = doc.GetInfoInt(resourceObj, "/Resources*/XObject*/"+key+":Ref");
				if(!objectMap.ContainsKey(id)) {
					objectMap.Add(id, false);
					AddSourceFonts(doc, report, id, fontMap, uniqueFontMap, objectMap);
				}
				if(j<0)
					break;
				i = j+1;
			}
		}
		private SourceFontReference GetSourceFont(Doc doc, bool report,
			int pageId, int resourceObj, string resourceName,
			SortedDictionary<int, SourceFontReference> fontMap,
			Dictionary<FontIdentity, FontReference> uniqueFontMap)
		{
			string path = "/Resources*/Font*/"+resourceName;
			string fontIdPath = path+":Ref";
			SourceFontReference sourceFontRef = new SourceFontReference();
			FontIdentity identity = null;
			string fontName = null;
			string encoding = null;
			string fontV = "";
			int sourceFontId = doc.GetInfoInt(resourceObj, fontIdPath);
			if(sourceFontId!=0) {
				if(!fontMap.TryGetValue(sourceFontId, out sourceFontRef))
					identity = NewFontIdentity(doc, sourceFontId, "",
						out fontName, out encoding);
			} else if((fontV = doc.GetInfo(resourceObj, path))!="")
				identity = NewFontIdentity(doc, resourceObj, path+"*",
					out fontName, out encoding);
			if(sourceFontRef.FontRef==null) {
				if(identity==null)
					return sourceFontRef;

				ReplacementFont font;
				if(!uniqueFontMap.TryGetValue(identity, out sourceFontRef.FontRef)) {
					font = null;
					TargetFont target;
					string targetName;
					if(GetTargetFont(identity.Name, out target, out targetName))
						font = AddReplaceFont(doc, new FontIdentity(targetName,
							identity.Vertical), target.Language,
							target.Embed, target.Subset);
					else if(_defaultLanguage.HasValue) {
						LanguageType language = _defaultLanguage.Value;
						bool embed = TargetFont.GetEmbed(language);
						font = AddReplaceFont(doc, identity, language, embed, embed);
					}

					if(font==null) {
						const string format = "Unable to add font \"{0}\" in language {1}.";
						if(targetName!=null)
							throw new ApplicationException(string.Format(format,
								targetName, target.Language));
						else if(_forceDefaultLanguage && _defaultLanguage.HasValue)
							throw new ApplicationException(string.Format(format,
								identity.Name, _defaultLanguage.Value));
						return sourceFontRef;
					}
					string fontName0, encoding0;
					FontIdentity identity0 = NewFontIdentity(doc, font.FontId, "",
						out fontName0, out encoding0);
					if(identity0!=null) {
						if(identity0.Name==identity.Name)
							identity0 = null;
						else {
							FontReference fontRef;
							if(uniqueFontMap.TryGetValue(identity0, out fontRef)) {
								if(font.FontId!=fontRef.Font.FontId)
									doc.Delete(font.FontId);
								sourceFontRef.FontRef = fontRef;
							}
						}
					}
					if(sourceFontRef.FontRef==null) {
						if(report)
							font.SourceFonts = new List<FontObjectInfo>();
						sourceFontRef.FontRef = new FontReference(font);
						if(identity0!=null)
							uniqueFontMap.Add(identity0, sourceFontRef.FontRef);
					}
					uniqueFontMap.Add(identity, sourceFontRef.FontRef);
				}
				font = sourceFontRef.FontRef.Font;
				if(fontV!="") {	// create indirect object
					sourceFontId = doc.AddObject(fontV);
					doc.SetInfo(resourceObj, fontIdPath, sourceFontId);
					if(font.SourceFonts!=null) {
						if(encoding==null)
							encoding = doc.GetInfo(sourceFontId, "/Encoding*:Name");
						sourceFontRef.SourceFont = new FontObjectInfo(0, fontName,
							encoding!="" && !encoding.StartsWith("Identity-",
							StringComparison.Ordinal)
								&& encoding==doc.GetInfo(font.FontId, "/Encoding*:Name"));
						font.SourceFonts.Add(sourceFontRef.SourceFont);
					}
				} else if(font.SourceFonts!=null) {
					if(encoding==null)
						encoding = doc.GetInfo(sourceFontId, "/Encoding*:Name");
					sourceFontRef.SourceFont = new FontObjectInfo(sourceFontId, fontName,
						encoding!="" && !encoding.StartsWith("Identity-",
						StringComparison.Ordinal)
							&& encoding==doc.GetInfo(font.FontId, "/Encoding*:Name"));
					font.SourceFonts.Add(sourceFontRef.SourceFont);
				}
				fontMap.Add(sourceFontId, sourceFontRef);
			}
			sourceFontRef.FontRef.References.Add(new Reference(pageId, resourceObj, fontIdPath));
			return sourceFontRef;
		}

		private void ProcessText(Doc doc, bool report, XmlNode node,
			SortedDictionary<int, SourceFontReference> fontMap,
			Dictionary<FontIdentity, FontReference> uniqueFontMap,
			SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Replacement>>> replaceMap,
			ref int resourceStreamId, ref int resourcePageId,
			Dictionary<string, FontResource> fontResources)
		{
			XmlAttribute attr = node.Attributes["pdf_Tf"];
			if(attr==null)
				return;

			string resourceName = attr.Value;
			int streamId;
			if(!ParseInt32Attribute(node.Attributes["pdf_StreamID"], out streamId))
				return;
			if(streamId==0)
				return;

			int offset;
			if(!ParseInt32Attribute(node.Attributes["pdf_StreamOffset"], out offset))
				return;

			int length;
			if(!ParseInt32Attribute(node.Attributes["pdf_StreamLength"], out length))
				return;

			SortedDictionary<int, Replacement> streamReplaceMap = null;
			SortedDictionary<int, SortedDictionary<int, Replacement>> fontResourceDictReplaceMap;
			if(replaceMap.TryGetValue(streamId, out fontResourceDictReplaceMap)) {
				if(fontResourceDictReplaceMap.TryGetValue(0, out streamReplaceMap)) {
					if(streamReplaceMap.ContainsKey(offset))
						return;
				}
			}
			int pageId = 0;	// stream has its resource dictionary
			if(streamReplaceMap==null) {
				string resourceDict = doc.GetInfo(streamId, "/Resources*/Font");
				if(resourceDict=="")
					resourceDict = doc.GetInfo(streamId, "/Resources");
				if(resourceDict=="") {
					pageId = doc.Page;	// stream uses the page's resource dictionary
					resourceDict = doc.GetInfo(pageId, "/Resources*/Font");
					if(resourceDict=="")
						resourceDict = doc.GetInfo(pageId, "/Resources");
					if(resourceDict=="")
						pageId = -1;	// failed
				}
				if(pageId!=0 && fontResourceDictReplaceMap!=null) {
					if(fontResourceDictReplaceMap.TryGetValue(pageId, out streamReplaceMap))
						if(streamReplaceMap.ContainsKey(offset))
							return;
				}
			}

			FontResource resource;
			if(streamId!=resourceStreamId || pageId!=resourcePageId) {
				resourceStreamId = streamId;
				resourcePageId = pageId;
				fontResources.Clear();
				resource = new FontResource();
			} else if(fontResources.TryGetValue(resourceName, out resource)) {
				if(resource.Font==null)
					return;
			}
			if(resource.Font==null) {
				SourceFontReference sourceFontRef = GetSourceFont(doc, report, pageId,
					pageId<=0? streamId: pageId, resourceName, fontMap, uniqueFontMap);
				if(sourceFontRef.FontRef==null) {
					fontResources.Add(resourceName, resource);
					return;
				}
				resource.Font = sourceFontRef.FontRef.Font;
				resource.SourceFont = sourceFontRef.SourceFont;
				fontResources.Add(resourceName, resource);
			}

			Replacement replace = new Replacement();
			replace.SourceFont = resource.SourceFont;
			replace.Font = resource.Font;
			replace.Offset = offset;
			replace.Length = length;
			string s;
			if(node.ChildNodes.Count<=0 || node.ChildNodes[0].Name!="tspan") {
				s = node.InnerText;
				replace.Text = new List<string>(1);
				replace.Text.Add(s);
				replace.Font.SetNonSpaceChar(s);
			} else {
				replace.Text = new List<string>(node.ChildNodes.Count);
				for(int i = 0; i<node.ChildNodes.Count; ++i) {
					XmlNode child = node.ChildNodes[i];
					if(child.Name=="tspan") {
						s = child.InnerText;
						replace.Text.Add(s);
						replace.Font.SetNonSpaceChar(s);
					}
				}
			}
			if(streamReplaceMap==null) {
				if(fontResourceDictReplaceMap==null) {
					fontResourceDictReplaceMap = new SortedDictionary<int, SortedDictionary<int, Replacement>>();
					replaceMap.Add(streamId, fontResourceDictReplaceMap);
				}
				streamReplaceMap = new SortedDictionary<int, Replacement>();
				fontResourceDictReplaceMap.Add(pageId, streamReplaceMap);
			}
			streamReplaceMap.Add(offset, replace);
		}
		private static ReplacementFont AddReplaceFont(Doc doc, FontIdentity identity,
			LanguageType language, bool embed, bool subset)
		{
			int fontId = AddDocFont(doc, identity, language, embed, subset);
			return fontId==0? null: new ReplacementFont(identity,
				language, fontId, subset);
		}
		private static int AddDocFont(Doc doc, FontIdentity identity,
			LanguageType language, bool embed, bool subset)
		{
			Debug.Assert(embed || !subset && !TargetFont.GetEmbed(language));
			return !embed? doc.AddFont(identity.Name, language, identity.Vertical):
				doc.EmbedFont(identity.Name, language, identity.Vertical, subset);
		}
		private static FontIdentity NewFontIdentity(Doc doc, int id, string path,
			out string outFontName, out string outEncoding)
		{
			string name = doc.GetInfo(id, path+"/DescendantFonts*[0]*/BaseFont*:Name");
			if(name=="")
				name = doc.GetInfo(id, path+"/BaseFont*:Name");
			if(name=="") {
				outFontName = null;
				outEncoding = null;
				return null;
			}
			outFontName = name;
			if(HasEmbeddedSubsetPrefix(name))
				name = name.Substring(7);

			string encoding = doc.GetInfo(id, path+"/Encoding*:Name");
			outEncoding = encoding;
			bool vertical = encoding.Length>0? encoding[encoding.Length-1]=='V':
				doc.GetInfo(id, path+"/Subtype*:Name")=="Type0"
				&& doc.GetInfoInt(id, path+"/Encoding*/WMode*:Num")==1;
			return new FontIdentity(name, vertical);
		}
		private int AddText(Doc doc, ReplacementFont font, string text, out int outExtraCount) {
			if(text[text.Length-1]!=' ') {
				outExtraCount = 0;
				return doc.AddText(text);
			}

			if(font.NonSpaceCode==null) {
				if(font.NonSpaceChar==null)
					font.NonSpaceChar = "0";
				int id = doc.AddText(font.NonSpaceChar);
				string s = doc.GetInfo(id, "Stream");
				doc.Delete(id);
				int i = s.IndexOfAny(_stringStart);
				if(i<0)
					throw new ApplicationException("Unable to find text operand.");
				int iEnd = GetStringEnd(s, i, s.Length);
				if(iEnd<0)
					throw new ApplicationException("Unable to find text operand end.");
				Debug.Assert(iEnd-i>2);
				font.NonSpaceCode = s.Substring(i+1, iEnd-i-2);
			}
			outExtraCount = font.NonSpaceCode.Length;
			return doc.AddText(text+font.NonSpaceChar);
		}
		private static bool ParseInt32Attribute(XmlAttribute attr, out int outV) {
			if(attr==null) {
				outV = 0;
				return false;
			}
			return int.TryParse(attr.Value, NumberStyles.Integer,
				NumberFormatInfo.InvariantInfo, out outV);
		}
		private static bool HasEmbeddedSubsetPrefix(string name) {
			if(name.Length<7 || name[6]!='+')
				return false;
			for(int i = 0; i<6; ++i) {
				if(name[i]<'A' || name[i]>'Z')
					return false;
			}
			return true;
		}
		private static int GetStringEnd(string s, int i, int iEnd) {
			if(s[i]=='<') {
				++i;
				i = s.IndexOf('>', i, iEnd-i);
				return i<0? i: i+1;
			}
			if(s[i]=='(') {
				int level = 0;
				bool escaped = false;
				while(++i<iEnd) {
					if(escaped)
						escaped = false;
					else {
						switch(s[i]) {
						case '\\': escaped = true; break;
						case '(': ++level; break;
						case ')':
							if(level<=0)
								return i+1;
							--level;
							break;
						}
					}
				}
			}
			return -1;
		}
	}
}
