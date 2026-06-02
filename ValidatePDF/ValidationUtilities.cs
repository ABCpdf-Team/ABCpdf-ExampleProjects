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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Text;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;
using WebSupergoo.ABCpdf14.Atoms;
using WebSupergoo.ABCpdf14.Operations;
using WebSupergoo.ABCpdf14.Elements;


namespace ValidatePDF {
	public class ValidationUtilities {
		/// <summary>
		/// Validate a folder of PDF files.
		/// </summary>
		/// <param name="folder">The folder to process.</param>
		/// <param name="checkObjects">Whether to check the document objects.</param>
		/// <param name="reportOrphans">Whether to report orphaned Elements which were not fully validated.</param>
		/// <param name="reportCoverage">Whether to report the types of PDF classes which were covered, and those missed.</param>
		/// <param name="checkContentStreams">Whether to check the content streams.</param>
		/// <param name="checkCompression">Whether to check stream compression. The checkSyntax option does this as well so typically you do not need both.</param>
		/// <param name="checkSyntax">Whether to check syntax using the PDF/A validator.</param>
		public static void Validate(string folder, bool checkObjects, bool reportOrphans, bool reportCoverage, bool checkContentStreams, bool checkCompression, bool checkSyntax) {
			HashSet<string> coverage = new HashSet<string>();
			foreach (string file in Directory.GetFiles(folder, "*.pdf"))
				Validate(file, file.Substring(0, file.Length - 4) + ".txt", checkObjects, checkContentStreams, checkCompression, checkSyntax, coverage, reportOrphans);
			if (reportCoverage) {
				HashSet<string> missed = ValidationLog.AllTypes;
				foreach (string cls in coverage)
					missed.Remove(cls);
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Covered:");
				foreach (string cls in coverage)
					sb.AppendLine(cls);
				sb.AppendLine("");
				sb.AppendLine("Missed:");
				foreach (string cls in missed)
					sb.AppendLine(cls);
				File.WriteAllText(Path.Combine(folder, "coverage.txt"), sb.ToString());
			}
		}

		/// <summary>
		/// Validate a PDF file, reporting to a log file.
		/// </summary>
		/// <param name="pdfFile">The PDF to be validated</param>
		/// <param name="logFile">The log file to which results should be written</param>
		/// <param name="coverage">A HashSet to be populated with the types of PDF classes which are seen during validation</param>
		/// <param name="reportOrphans">Whether to report orphaned Elements which were not fully validated</param>
		public static void Validate(string pdfFile, string logFile, bool checkObjects, bool checkContentStreams, bool checkCompression, bool checkSyntax, HashSet<string> coverage, bool reportOrphans) {
			List<string> logs = new List<string>();
			using (Doc theDoc = new Doc()) {
				try {
					theDoc.Read(pdfFile);
				}
				catch (Exception ex) {
					logs.Add("Unable to read file. " + ex.Message);
					checkObjects = false;
					checkContentStreams = false;
				}
				if (checkObjects)
					logs.Add(ValidateDocument(theDoc, coverage, reportOrphans));
				if (checkContentStreams)
					logs.Add(ValidateContents(theDoc));
				if (checkCompression)
					logs.Add(ValidateCompression(theDoc));
			}
			if (checkSyntax)
				logs.Add(ValidateSyntax(pdfFile));
			File.WriteAllText(logFile, string.Join("\r\n\r\n", logs));
		}

		/// <summary>
		/// Validate the objects in a PDF file, returning the results.
		/// </summary>
		/// <param name="doc">The Doc to be validated</param>
		/// <param name="coverage">A HashSet to be populated with the types of PDF classes which are seen during validation</param>
		/// <param name="reportOrphans">Whether to report orphaned Elements which were not fully validated</param>
		/// <returns>The results of validation</returns>
		public static string ValidateDocument(Doc doc, HashSet<string> coverage, bool reportOrphans) {
			ValidationLog validation = new ValidationLog();
			validation.Coverage = coverage;
			validation.Stack = new Stack<ValidationStackNode>();
			CatalogElement cat = new CatalogElement(doc.ObjectSoup.Catalog);
			cat.Validate(validation);
			StreamObject trail = doc.ObjectSoup.Trailer;
			bool isCrossRef = Atom.GetItem(trail.Atom, "W") != null;
			CrossReferenceStreamElement crossRef = isCrossRef ? new CrossReferenceStreamElement(trail) : null;
			FileTrailerElement fileTrailer = !isCrossRef ? new FileTrailerElement(trail) : null;
			Element trailer = isCrossRef ? (Element)crossRef : (Element)fileTrailer;
			trailer.Validate(validation);
			if (doc.Encryption.Type != 0) {
				validation.Start(trailer);
				EncryptionElement encryption = crossRef != null ? crossRef.EntryEncrypt : fileTrailer.EntryEncrypt;
				validation.ReportEntryVersion("Encryption.V", encryption != null ? encryption.GetEntryVersion("V") : GetEncryptionVEntryVersion(doc.Encryption.Type));
			}

			StringBuilder sb = validation.Log;
			HashSet<int> done = validation.Done;
			List<IndirectObject> missed = new List<IndirectObject>();
			foreach (IndirectObject io in doc.ObjectSoup) {
				if ((io != null) && (io.ID != 0) && (!done.Contains(io.ID))) {
					try {
						Element e = ElementFactories.AutodetectFactory(new RefAtom(io), io, null);
						if ((e is ObjectStreamElement) || (e is CrossReferenceStreamElement) || (e is LinearizationParameterElement)) {
							e.Validate(validation);
							done = validation.Done;
						}
					}
					catch (Exception ex) {
						validation.ReportException(io, ex);
					}
				}
			}
			if (reportOrphans) {
				foreach (IndirectObject io in doc.ObjectSoup) {
					if ((io != null) && (io.ID != 0) && (!done.Contains(io.ID)))
						missed.Add(io);
				}
				if (missed.Count > 0) {
					HashSet<int> refOnlyObjects = new HashSet<int>();
					Dictionary<int, HashSet<int>> references = new Dictionary<int, HashSet<int>>();
					foreach (IndirectObject io in doc.ObjectSoup) {
						if (io != null) {
							bool hasContent = false;
							FindAllReferences(io.ID, io.Atom, references, ref hasContent);
							if (!hasContent)
								refOnlyObjects.Add(io.ID);
						}
					}
					foreach (IndirectObject io in missed) {
						HashSet<int> set = null;
						references.TryGetValue(io.ID, out set);
						if (set == null)
							continue; // object not referenced by anything - ignore
						if (refOnlyObjects.Contains(io.ID))
							continue; // only reference atoms - simply a stop en route to other items
						sb.AppendLine("Missed ID: " + io.ID.ToString() + ": " + IndirectObjectDescription(io));
						foreach (var id in set)
							sb.AppendLine("  Referenced by ID: " + id.ToString() + ": " + IndirectObjectDescription(doc.ObjectSoup[id]));
					}
				}
			}
			string header = "PDF Version: " + GetVersion(validation.Version) + "\r\n";
			if (validation.Version.HasFlag(PDFVersion.AdobeUndocumented))
				header += "PDF Uses Undocumented Extensions\r\n";
			if (validation.Version.HasFlag(PDFVersion.PDF20Deprecated)) {
				header += "PDF Uses PDF 2.0 Deprecated Features\r\n";
				foreach (string feature in validation.Deprecated)
					header += " - " + feature + "\r\n";
			}
			header += "\r\n";
			return header + sb.ToString();
		}

		/// <summary>
		/// Validate the content streams in a PDF file, returning the results.
		/// </summary>
		/// <param name="doc">The Doc to be validated</param>
		/// <returns>The results of validation</returns>
		public static string ValidateContents(Doc doc)
		{
			StringBuilder sb = new StringBuilder();
			List<IndirectObject> items = new List<IndirectObject>();
			// gather items
			HashSet<int> skip = new HashSet<int>();
			try {
				foreach (Page page in doc.ObjectSoup.Catalog.Pages.GetPageArrayAll()) {
					items.Add(page);
					foreach (Atom atom in page.GetResourcesByType("XObject", true, true, true, true, skip)) {
						IndirectObject rez = page.ResolveObj(atom);
						if (rez != null)
							skip.Add(rez.ID);
						if (rez is FormXObject)
							items.Add(rez);
					}
				}
			}
			catch (Exception ex) {
				return "Unable to validate document contents because document is badly formed.\r\n" + ex.Message;
			}
			// scan for errors
			int pageCount = 0;
			Page lastPage = null;
			foreach (IndirectObject item in items) {
				// get content stream
				byte[] data = null;
				Page page = item as Page;
				FormXObject form = item as FormXObject;
				if (page != null) {
					lastPage = page;
					pageCount++;
					try {
						doc.Page = page.ID;
						doc.Rendering.DotsPerInch = 18; // small value for speed
						doc.Rendering.GetData("dummy.jpg");
						string log = VetLog(doc.Rendering.Log);
						if (!string.IsNullOrWhiteSpace(log))
							sb.Append(log);
					}
					catch (Exception ex) {
						sb.AppendLine($"Unable to validate rendering of Page ID {page.ID} because of error {ex.Message}");
						continue;
					}
					try {
						data = page.GetContentData();
					}
					catch {
						sb.AppendLine($"Unable to validate Page ID {page.ID} as content stream would not decompress");
						continue;
					}
				}
				else {
					if (!form.Decompress()) {
						sb.AppendLine($"Unable to validate Form XObject {form.ID} as it would not decompress");
						continue;
					}
					data = form.GetData();
				}
				Debug.Assert(data != null);
				// find errors in content stream
				ArrayAtom array = ArrayAtom.FromContentStream(data);
				Dictionary<int, string> errors = FindContentStreamErrors(lastPage, item, array);
				if (errors.Count == 0)
					continue;
				// report errors in content stream
				ArrayAtom selection = new ArrayAtom();
				foreach (var pair in errors) {
					int start = Math.Max(0, pair.Key - 10);
					int end = Math.Min(pair.Key + 10, array.Count - 1);
					if (selection.Count != 0)
						selection.Add(new OpAtom("...\r\n\r\n"));
					for (int i = start; i <= end; i++) {
						selection.Add(array[i].Clone());
						if (i == pair.Key)
							selection.Add(new OpAtom(pair.Value));
					}
				}
				sb.AppendLine("Content Stream Errors Detected:");
				if (page != null)
					sb.AppendLine($"Page ID {page.ID} page number {pageCount}");
				else
					sb.AppendLine($"Form XObject ID {form.ID} first encountered on page {pageCount}");
				sb.AppendLine(ASCIIEncoding.ASCII.GetString(selection.GetData()));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Validate the stream compression in a PDF file, returning the results.
		/// </summary>
		/// <param name="doc">The Doc to be validated</param>
		/// <returns>The results of validation</returns>
		public static string ValidateCompression(Doc doc) {
			StringBuilder sb = new StringBuilder();
			List<StreamObject> items = new List<StreamObject>();
			// gather items
			try {
				foreach (IndirectObject io in doc.ObjectSoup) {
					if (io is StreamObject)
						items.Add((StreamObject)io);
				}
			}
			catch (Exception ex) {
				return "Unable to validate stream compression because document is badly formed.\r\n" + ex.Message;
			}
			// scan for errors
			foreach (StreamObject so in items) {
				try {
					so.CopyDecompressedData();
					so.ClearCachedDecompressed();
				}
				catch (Exception ex) {
					sb.AppendLine($"Error in StreamObject ID {so.ID}: {ex.Message}");
				}
			}
			return sb.ToString();
		}

		private static Dictionary<int, string> FindContentStreamErrors(Page lastPage, IndirectObject item, ArrayAtom array)
		{
			// Validation of content streams is a bit open ended.
			// This illustrates some basic sanity checks and can easily be extended.
			IDictionary<string, Atom> fonts = null;
			IDictionary<string, Atom> xobjects = null;
			IDictionary<string, Atom> colorspaces = null;
			OperatorState operatorState = new OperatorState();
			int graphicsStateDepth = 1;
			Dictionary<int, string> errors = new Dictionary<int, string>();
			foreach (var pair in OpAtom.Find(array)) {
				if (!operatorState.IsOK(pair.Item1))
					AddError(errors, pair.Item2, "Operator \'" + pair.Item1 + "\' not valid inside " + operatorState.ObjectState.ToString() + " graphics object.");
				Atom[] args = OpAtom.GetParameters(array, pair.Item2);
				if (pair.Item1 == "q") {
					graphicsStateDepth++;
					if (graphicsStateDepth > 26)
						AddError(errors, pair.Item2, "Graphics state nesting too deep.");
				}
				else if (pair.Item1 == "Q") {
					if (graphicsStateDepth <= 1)
						AddError(errors, pair.Item2, "No graphics states left to pop.");
					else
						graphicsStateDepth--;
				}
				if (args == null)
					AddError(errors, pair.Item2, "Bad Parameters.");
				else if (pair.Item1 == "Tf") {
					if (fonts == null)
						fonts = lastPage.GetResourceMap(item.ID, "Font");
					string name = ((NameAtom)args[0]).Text;
					if (!fonts.ContainsKey(name))
						AddError(errors, pair.Item2, "Named Font does not exist.");
				}
				else if (pair.Item1 == "TJ") {
					foreach (Atom arg in ((ArrayAtom)args[0])) {
						if ((arg is NumAtom == false) && (arg is StringAtom == false)) {
							AddError(errors, pair.Item2, "Bad String Arguments.");
							break;
						}
					}
				}
				else if (pair.Item1 == "Do") {
					if (xobjects == null)
						xobjects = lastPage.GetResourceMap(item.ID, "XObject");
					string name = ((NameAtom)args[0]).Text;
					if (!xobjects.ContainsKey(name))
						AddError(errors, pair.Item2, "Named XObject does not exist.");
				}
				else if ((pair.Item1 == "CS") || (pair.Item1 == "cs")) {
					string name = ((NameAtom)args[0]).Text;
					switch (name) {
						case "DeviceGray":
							break;
						case "DeviceRGB":
							break;
						case "DeviceCMYK":
							break;
						case "Pattern":
							break;
						default:
							if (colorspaces == null)
								colorspaces = lastPage.GetResourceMap(item.ID, "ColorSpace");
							if (!colorspaces.ContainsKey(name))
								AddError(errors, pair.Item2, "Named XObject does not exist.");
							break;
					}
				}
			}
			return errors;
		}

		/// <summary>
		/// Validate the low level syntax of a PDF file, returning the results.
		/// </summary>
		/// <param name="file">The PDF to be validated</param>
		/// <returns>The results of validation</returns>
		public static string ValidateSyntax(string file) {
			StringBuilder sb = new StringBuilder();
			using (PdfValidationOperation op = new PdfValidationOperation()) {
				op.Conformance = PdfConformance.Pdf;
				using (Doc doc = op.Read(file, null)) {
				}
				if (op.Errors != null && op.Errors.Count > 0) {
					sb.AppendLine("PDF Syntax Errors:");
					for (int i = 0; i < op.Errors.Count; ++i)
						sb.AppendLine(op.Errors[i]);
				}
				if (op.Warnings != null && op.Warnings.Count > 0) {
					sb.AppendLine("PDF Syntax Warnings:");
					for (int i = 0; i < op.Warnings.Count; ++i)
						sb.AppendLine(op.Warnings[i]);
				}
			}
			return sb.ToString();
		}

		private static void AddError(Dictionary<int, string> errors, int item, string error) {
			string str = null;
			errors.TryGetValue(item, out str);
			if (str == null)
				errors[item] = "%% " + error + "\r\n";
			else {
				if (str.Contains(error))
					return;
				errors[item] = str.Trim() + " " + error + "\r\n";
			}
		}

		private static string IndirectObjectDescription(IndirectObject io) {
			const int length = 300;
			string desc = io.ToString().Replace("\r\n", "\n");
			if (desc.Length > length) desc = desc.Substring(0, length - 3) + "...";
			return desc;
		}

		private static void FindAllReferences(int id, Atom atom, Dictionary<int, HashSet<int>> references, ref bool hasContent) {
			if (atom is RefAtom) {
				int refid = ((RefAtom)atom).ID;
				HashSet<int> set = null;
				references.TryGetValue(refid, out set);
				if (set == null) {
					set = new HashSet<int>();
					references[refid] = set;
				}
				set.Add(id);
			}
			else if (atom is ArrayAtom) {
				foreach (var entry in (ArrayAtom)atom)
					FindAllReferences(id, entry, references, ref hasContent);
			}
			else if (atom is DictAtom) {
				foreach (var entry in (DictAtom)atom)
					FindAllReferences(id, entry.Value, references, ref hasContent);
			}
			else {
				hasContent = true;
			}
		}

		private static string GetVersion(PDFVersion version) {
			if (version.HasFlag(PDFVersion.PDF20))
				return "200";
			if (version.HasFlag(PDFVersion.PDF17Extension5))
				return "175";
			if (version.HasFlag(PDFVersion.PDF17Extension3))
				return "173";
			if (version.HasFlag(PDFVersion.PDF17))
				return "170";
			if (version.HasFlag(PDFVersion.PDF16))
				return "160";
			if (version.HasFlag(PDFVersion.PDF15))
				return "150";
			if (version.HasFlag(PDFVersion.PDF14))
				return "140";
			if (version.HasFlag(PDFVersion.PDF13))
				return "130";
			if (version.HasFlag(PDFVersion.PDF12))
				return "120";
			if (version.HasFlag(PDFVersion.PDF11))
				return "110";
			return "100";
		}

		private static PDFVersion GetEncryptionVEntryVersion(int? v) {
			switch (v.HasValue ? v.Value : 0) {
				case 0:
					return PDFVersion.PDF10 | PDFVersion.PDF20Deprecated | PDFVersion.PDF17Obsolescent;
				case 1:
					return PDFVersion.PDF10 | PDFVersion.PDF20Deprecated;
				case 2:
					return PDFVersion.PDF14 | PDFVersion.PDF20Deprecated;
				case 3:
					return PDFVersion.PDF14 | PDFVersion.PDF20Deprecated | PDFVersion.PDF17Obsolescent;
				case 4:
					return PDFVersion.PDF15 | PDFVersion.PDF20Deprecated;
				case 5:
					return PDFVersion.PDF17Extension3 | PDFVersion.PDF20;
			}
			return PDFVersion.None;
		}

		private static string VetLog(string log) {
			StringBuilder sb = new StringBuilder();
			foreach (string line in log.Split(new char[] { '\r' })) {
				string trimmed = line.TrimEnd();
				if (line.Contains("Substituting"))
					continue; // not interested in font substitution
				if (line.Contains("No Content Read"))
					continue; // if it's blank that's fine by us
				sb.AppendLine(trimmed);
			}
			return sb.ToString();
		}
	}

	/// <summary>
	/// A simple class to demonstrate how a PDF file may be validated.
	/// The Report overrides in this class are called during validation to allow errors or warnings to be flagged.
	/// It is simply a matter of loggging appropriate information each time one of these is called.
	/// </summary>
	public class ValidationLog : ValidationBase {
		public PDFVersion Version { get; private set; }
		public StringBuilder Log { get; private set; }
		public HashSet<string> Deprecated { get; private set; }
		public bool ReportStack { get; set; }

		public ValidationLog() : base() {
			Version = PDFVersion.PDF10;
			Log = new StringBuilder();
			Deprecated = new HashSet<string>();
		}

		public override void ReportEntryVersion(string entry, PDFVersion version) {
			Version = Version | version;
			if (version.HasFlag(PDFVersion.PDF20Deprecated))
				Deprecated.Add($"Deprecated Feature: \"{Target(entry)}\".");
		}

		public override void ReportUnknownEntryName(string entry) {
			Element element = Stack.Peek().Element;
			string name = element.GetType().Name;
			if ((name == "FileTrailerElement") || (name == "CrossReferenceStreamElement")) {
				if (entry == "ABCpdf") return; // ABCpdf adds this
				if (entry == "Source") return; // and this
			}
			else if (name == "ResourceElement") {
				if (entry == "Encoding") return; // not in spec but used in documents
			}
			else if (name == "StandardLayoutAttributesElement") {
				if (entry == "ColumnSpan") return; // not in spec but used in documents
			}
			else if (name == "RichMediaAnnotationElement") {
				if (entry == "AA") return; // not in spec but used in documents and these features work when displayed in Acrobat
				if (entry == "BS") return; // spec says it has same entries as an Annotation but seems to share more in common with Widget
			}
			Log.AppendLine($"Unknown Entry: \"{Target(entry)}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportIncorrectEntryValue(string value) {
			Log.AppendLine($"Wrong Value: \"{Target()}\" = \"{value}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportMissingRequiredEntry() {
			Log.AppendLine($"Missing Required Entry: \"{Target()}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportNotIndirectObject() {
			Log.AppendLine($"Entry Not Indirect: \"{Target()}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportIncorrectItemType(string expectedType, string actualType, Atom atom) {
			base.ReportIncorrectItemType(expectedType, actualType, atom);
			Element element = Stack.Peek().Element;
			string entry = Stack.Peek().Entry;
			string key = Stack.Peek().Index >= 0 ? Stack.Peek().Index.ToString() : null;
			if ((actualType == null) && (atom != null)) {
				Element e = ElementFactories.AutodetectFactory(atom, element.Host, null);
				if (e != null)
					actualType = e.GetType().Name;
			}
			if (key == null)
				key = "";
			Log.Append($"Wrong Type: \"{Target(key)}\". ");
			if (expectedType != null)
				Log.Append("Expected \"" + expectedType + "\". ");
			if (actualType != null)
				Log.Append("Found \"" + actualType + "\". ");
			if (ReportStack)
				Log.AppendLine($"Stack: {GetStackTrace()}");
		}

		public override void ReportIncorrectNumberOfEntries(int expected, int found) {
			Log.AppendLine($"Wrong Number of Entries: \"{Target()}\". Expected {expected} found {found}.");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportRecursiveStructure() {
			Log.AppendLine($"Document contains recursive Atom structure: \"{Target()}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public override void ReportException(Element element, Exception ex) {
			Log.AppendLine($"Exception processing {element.GetType().Name} caused validation of this object to be aborted. Message \"{GetMessage(ex)}\".");
			if (ReportStack)
				Log.AppendLine($" Stack: {GetStackTrace()}");
		}

		public void ReportException(IndirectObject io, Exception ex) {
			Log.AppendLine($"Exception processing IndirectObject {io.ID} caused validation of this object to be aborted. Message \"{GetMessage(ex)}\".");
		}

		private static string GetMessage(Exception e) {
			StringBuilder msg = new StringBuilder();
			for (int i = 0; i < 20; i++) {
				if (e == null) break;
				if (e.Message.Length > 0) {
					if (i != 0)
						msg.Append(" ");
					msg.Append(e.Message.Replace("\r\n", ""));
				}
				e = e.InnerException;
			}
			return msg.ToString();
		}

		private string Target() {
			return Target(null);
		}

		private string Target(string entry) {
			var item = Stack.Peek();
			string type = item.Element.GetType().Name;
			if (entry == null)
				entry = item.Entry;
			int id = 0;
			ValidationStackNode[] stack = Stack.ToArray();
			for (int i = 0;i < stack.Length; i++) {
				var io = stack[i].Element.Object;
				if (io != null) {
					id = io.ID;
					break;
				}
			}
			return $"{type}({id}).{entry}";
		}
	}

	/// <summary>
	/// A simple class to hold content stream state and determine if the operators that are provided
	/// are compatible with the current state. In most situations incorrect operators can just be ignored
	/// but sometimes it is ambigious as to the best action to take.
	/// This information is derived from the graphics objects state chart in the PDF Specification.
	/// </summary>
	class OperatorState {
		static HashSet<string> sGeneralGraphicsState = new HashSet<string>(ContentStreamScanner.OperatorCategories.GeneralGraphicsState);
		static HashSet<string> sSpecialGraphicsState = new HashSet<string>(ContentStreamScanner.OperatorCategories.SpecialGraphicsState);
		static HashSet<string> sPathConstruction = new HashSet<string>(ContentStreamScanner.OperatorCategories.PathConstruction);
		static HashSet<string> sPathPainting = new HashSet<string>(ContentStreamScanner.OperatorCategories.PathPainting);
		static HashSet<string> sClippingPaths = new HashSet<string>(ContentStreamScanner.OperatorCategories.ClippingPaths);
		static HashSet<string> sTextState = new HashSet<string>(ContentStreamScanner.OperatorCategories.TextState);
		static HashSet<string> sTextPositioning = new HashSet<string>(ContentStreamScanner.OperatorCategories.TextPositioning);
		static HashSet<string> sTextShowing = new HashSet<string>(ContentStreamScanner.OperatorCategories.TextShowing);
		static HashSet<string> sColor = new HashSet<string>(ContentStreamScanner.OperatorCategories.Color);
		static HashSet<string> sMarkedContent = new HashSet<string>(ContentStreamScanner.OperatorCategories.MarkedContent);

		public enum State { Page, Path, Text, ClippingPath, Shading, InlineImage, External }
		private static Dictionary<State, HashSet<string>> sValidOps = null;

		public State ObjectState { get; set; }
		public bool IgnoreErrorsInCompatibilitySections { get; set; }
		private int CompatibilitySectionDepth { get; set; }

		public OperatorState() {
			if (sValidOps == null) {
				Dictionary<State, HashSet<string>> valid = new Dictionary<State, HashSet<string>>();
				valid[State.Page] = new HashSet<string>();
				valid[State.Page].UnionWith(sGeneralGraphicsState);
				valid[State.Page].UnionWith(sSpecialGraphicsState);
				valid[State.Page].UnionWith(sColor);
				valid[State.Page].UnionWith(sTextState);
				valid[State.Page].UnionWith(sMarkedContent);
				valid[State.Path] = new HashSet<string>();
				valid[State.Path].UnionWith(sPathConstruction);
				valid[State.Text] = new HashSet<string>();
				valid[State.Text].UnionWith(sGeneralGraphicsState);
				valid[State.Text].UnionWith(sColor);
				valid[State.Text].UnionWith(sTextState);
				valid[State.Text].UnionWith(sTextShowing);
				valid[State.Text].UnionWith(sTextPositioning);
				valid[State.Text].UnionWith(sMarkedContent);
				valid[State.ClippingPath] = new HashSet<string>();
				valid[State.Shading] = new HashSet<string>();
				valid[State.InlineImage] = new HashSet<string>();
				valid[State.InlineImage].Add("ID");
				valid[State.External] = new HashSet<string>();
				sValidOps = valid;
			}
			IgnoreErrorsInCompatibilitySections = false;
			ObjectState = State.Page;
			CompatibilitySectionDepth = 0;
		}

		public bool IsOK(string op) {
			if (op == "BX") {
				CompatibilitySectionDepth++;
				return true;
			}
			if (op == "EX") {
				CompatibilitySectionDepth--;
				return true;
			}
			if ((CompatibilitySectionDepth > 0) && (IgnoreErrorsInCompatibilitySections))
				return true;
			switch (ObjectState) {
				case State.Page:
					if ((op == "m") || (op == "re")) {
						ObjectState = State.Path;
						return true;
					}
					if (op == "BT") {
						ObjectState = State.Text;
						return true;
					}
					if (op == "sh") {
						return true;
					}
					if (op == "Do") {
						return true;
					}
					if (op == "BI") {
						ObjectState = State.InlineImage;
						return true;
					}
					break;
				case State.Path:
					if (sPathPainting.Contains(op)) {
						ObjectState = State.Page;
						return true;
					}
					if (sClippingPaths.Contains(op)) {
						ObjectState = State.ClippingPath;
						return true;
					}
					break;
				case State.Text:
					if (op == "ET") {
						ObjectState = State.Page;
						return true;
					}
					break;
				case State.ClippingPath:
					if (sPathPainting.Contains(op)) {
						ObjectState = State.Page;
						return true;
					}
					break;
				case State.Shading:
					Debug.Assert(false, "Should never hit this state because there is an immediate return from shading objects.");
					break;
				case State.InlineImage:
					if (op == "EI") {
						ObjectState = State.Page;
						return true;
					}
					break;
				case State.External:
					Debug.Assert(false, "Should never hit this state because there is an immediate return from external objects.");
					break;
			}
			return sValidOps[ObjectState].Contains(op);
		}
	}
}
