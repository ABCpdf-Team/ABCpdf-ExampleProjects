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
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Linq;

using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;
using WebSupergoo.ABCpdf13.Operations;
using WebSupergoo.ABCpdf13.Elements;
using WebSupergoo.ABCpdf13.Operations.Accessibility;
using WebSupergoo.TableDetection;


namespace AccessiblePDF {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		private Button button1;
		private Button button2;
		private Button button3;
		private Button button4;
		private Button button5;
		private Button button6;
		private Button button7;
		private TextBox textBox1;
		private TextBox textBox3;
		private TextBox textBox2;
		private Button button8;
		private Button button9;
		private Button button10;
		private Button button11;
		private Button button12;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.button1 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.button11 = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(19, 358);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(256, 33);
			this.button1.TabIndex = 0;
			this.button1.Text = "Change Content";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(301, 120);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(256, 33);
			this.button3.TabIndex = 2;
			this.button3.Text = "Detect Tables";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(19, 120);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(256, 33);
			this.button4.TabIndex = 3;
			this.button4.Text = "Detect Headers";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(301, 162);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(256, 34);
			this.button5.TabIndex = 4;
			this.button5.Text = "Detect Lists";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(19, 162);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(256, 34);
			this.button6.TabIndex = 5;
			this.button6.Text = "Detect Sections";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(584, 120);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(256, 33);
			this.button7.TabIndex = 6;
			this.button7.Text = "Detect Artifacts";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(17, 244);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(823, 88);
			this.textBox1.TabIndex = 7;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(19, 30);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(821, 68);
			this.textBox3.TabIndex = 9;
			this.textBox3.Text = resources.GetString("textBox3.Text");
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(19, 575);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(256, 33);
			this.button2.TabIndex = 10;
			this.button2.Text = "Extract Structure";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(19, 481);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(823, 67);
			this.textBox2.TabIndex = 11;
			this.textBox2.Text = resources.GetString("textBox2.Text");
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(586, 575);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(256, 33);
			this.button8.TabIndex = 12;
			this.button8.Text = "Markup Structure";
			this.button8.Click += new System.EventHandler(this.button8_Click);
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(584, 358);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(256, 33);
			this.button9.TabIndex = 13;
			this.button9.Text = "Add Tagged Signature";
			this.button9.Click += new System.EventHandler(this.button9_Click);
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(584, 162);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(256, 34);
			this.button10.TabIndex = 14;
			this.button10.Text = "Add Custom Tags";
			this.button10.Click += new System.EventHandler(this.button10_Click);
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(301, 358);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(256, 33);
			this.button11.TabIndex = 15;
			this.button11.Text = "Remove Content";
			this.button11.Click += new System.EventHandler(this.button11_Click);
			// 
			// button12
			// 
			this.button12.Location = new System.Drawing.Point(19, 404);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(256, 33);
			this.button12.TabIndex = 16;
			this.button12.Text = "Add Tagged Content";
			this.button12.Click += new System.EventHandler(this.button12_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
			this.ClientSize = new System.Drawing.Size(881, 626);
			this.Controls.Add(this.button12);
			this.Controls.Add(this.button11);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "AccessiblePDF";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			ChangeContent(Path.Combine(theRez, "_Flowers.pdf"), Path.Combine(theRez, "_Squirrel.png"), Path.Combine(theRez, "6-replace_content.pdf"));
		}

		private static void ChangeContent(string src, string img, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				Structure tags = new Structure(doc);
				List<StructureElementElement> images = tags.FindElementsByType("CustomImageTag");
				if (images.Count > 0) {
					using (XImage image = XImage.FromFile(img, null)) {
						PixMap pm = PixMap.FromXImage(doc.ObjectSoup, image);
						tags.ReplaceImages(images, pm);
					}
				}
				List<StructureElementElement> texts = tags.FindElementsByType("CustomTextTag");
				tags.ReplaceText(texts, "Replaced Text!");
				doc.Save(dst);
			}
		}

		private void button11_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithTables(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "9-subject_matter.pdf"));
			PurgeUnreferencedTags(Path.Combine(theRez, "9-subject_matter.pdf"), Path.Combine(theRez, "9-purge_tags_before.pdf"), Path.Combine(theRez, "9-purge_tags_after.pdf"));
			File.Delete(Path.Combine(theRez, "9-subject_matter.pdf"));
		}

		private static void PurgeUnreferencedTags(string src, string before, string after) {
			var r = new Random(5); // arbitrary seed set to ensure same output each time
			using (Doc doc = new Doc()) {
				doc.Read(src);
				foreach (var page in doc.ObjectSoup.Catalog.Pages.GetPageArrayAll()) {
					string txt = ASCIIEncoding.ASCII.GetString(page.GetContentData());
					string[] lines = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
					var sb = new StringBuilder();
					bool skipping = false;
					foreach (string line in lines) {
						if (line.Contains("/Span << /MCID") && (r.Next(5) == 0))
							skipping = true;
						else if (skipping && line.Contains("EMC"))
							skipping = false;
						else if (!skipping)
							sb.AppendLine(line);
					}
					var l = page.GetLayers()[0];
					l.SetData(Encoding.ASCII.GetBytes(sb.ToString()), true);
				}
				doc.Save(before);
				Structure tags = new Structure(doc);
				tags.Purge(new string[] { "P" });
				tags.UpdateActualText(true, true);
				doc.Save(after);

				//MarkupStructure(before, before.Replace(".pdf", "_Markup.pdf"));
				//MarkupStructure(after, after.Replace(".pdf", "_Markup.pdf"));
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			foreach (string file in Directory.GetFiles(theRez, "*.pdf")) {
				if (Path.GetFileName(file).StartsWith("_"))
					continue;
				// the content we extract isn't really HTML but it is similar enough that a browser will often display it
				ExtractStructure(file, Path.Combine(theRez, Path.GetFileNameWithoutExtension(file) + ".htm"));
			}
		}

		private static void ExtractStructure(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				Structure tags = new Structure(doc);
				StringBuilder sb = tags.ExtractStructure();
				File.WriteAllText(dst, sb.ToString());
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithTables(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "3-detect_tables.pdf"));
		}

		private static void AccessibilityWithTables(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				doc.Save(dst);
				Structure tags = new Structure(doc);
				// reorder elements so that they go from top to bottom
				StructureElementElement document = (StructureElementElement)tags.Root.EntryK[0];
				KidArranger.SortTopToBottom(tags, document, null); // can pass KidArranger.GetPageLayout(doc.MediaBox, 72.0, 72.0, 2) for columns
				// convert paragraphs to tables where appropriate
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				Tables tables = new Tables();
				tables.FontSensitive = false;
				tables.Add(tags, elements);
				foreach (Table table in tables.Items) {
					if (table.Items.Count <= 1)
						continue; // too small
					int index = 0;
					StructureElementElement parent = null;
					StructureElementElement tb = new StructureElementElement(document);
					tb.EntryType = "StructElem";
					tb.EntryS = "Table";
					foreach (Row row in table.Items) {
						StructureElementElement tr = new StructureElementElement(document);
						tr.EntryType = "StructElem";
						tr.EntryS = "TR";
						tr.SetParent(tb);
						foreach (StructureElementElement item in row.Items) {
							if ((parent == null) && (item != null)) {
								Debug.Assert(item.EntryP is StructureTreeRootElement == false);
								parent = item.EntryP as StructureElementElement;
								index = parent.EntryK.IndexOf(item);
							}
							StructureElementElement td = new StructureElementElement(document);
							td.EntryType = "StructElem";
							td.EntryS = "TD";
							td.SetParent(tr);
							if (item != null)
								item.SetParent(td);
						}
					}
					tb.SetParent(parent, index);
				}
				doc.Save(dst);
			}
		}

		private void button4_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithHeaders(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "1-detect_headers.pdf"));
		}

		private static void AccessibilityWithHeaders(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				doc.Save(dst);
				Structure tags = new Structure(doc);
				// reorder elements so that they go from top to bottom
				StructureElementElement document = (StructureElementElement)tags.Root.EntryK[0];
				KidArranger.SortTopToBottom(tags, document, null);
				// determine what styles are used for headers and change tag type appropriately
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				Dictionary<string, List<StructureElementElement>> styles = new Dictionary<string, List<StructureElementElement>>();
				foreach (StructureElementElement element in elements) {
					string weight = null;
					if (!element.GetStyle().TryGetValue("font-weight", out weight))
						continue; // we are only interested in bold fonts
					int value = 0;
					if ((!int.TryParse(weight, out value)) || (value < 700))
						continue;
					string size = null;
					if (!element.GetStyle().TryGetValue("font-size", out size))
						continue;
					List<StructureElementElement> matches = null;
					if (!styles.TryGetValue(size, out matches)) {
						matches = new List<StructureElementElement>();
						styles[size] = matches;
					}
					matches.Add(element);
				}
				List<Style> headers = new List<Style>();
				foreach (KeyValuePair<string, List<StructureElementElement>> pair in styles)
					headers.Add(new Style(pair.Key, pair.Value));
				headers.Sort((i1, i2) => i1.Elements.Count.CompareTo(i2.Elements.Count));
				for (int i = 0; i < headers.Count; i++) {
					if (i > 4)
						break; // limit the number of header levels
					string tag = "H" + (i + 1).ToString();
					foreach (StructureElementElement element in headers[i].Elements) {
						element.EntryS = tag;
					}
				}
				doc.Save(dst);
			}
		}

		private void button5_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithLists(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "4-detect_lists.pdf"));
		}

		private static void AccessibilityWithLists(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				doc.Save(dst);
				Structure tags = new Structure(doc);
				// reorder elements so that they go from top to bottom
				StructureElementElement document = (StructureElementElement)tags.Root.EntryK[0];
				KidArranger.SortTopToBottom(tags, document, null);
				// detect and insert list and list item structure
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				StructureElementElement list = null;
				foreach (StructureElementElement element in elements) {
					bool isListItem = element.EntryActualText.TrimStart().StartsWith("\u2022");
					if ((isListItem) && (list == null)) {
						list = new StructureElementElement(document);
						list.EntryType = "StructElem";
						list.EntryS = "L";
						Debug.Assert(element.EntryP is StructureTreeRootElement == false);
						StructureElementElement parent = element.EntryP as StructureElementElement;
						Debug.Assert(parent != null);
						int index = parent.EntryK.IndexOf(element);
						list.SetParent(parent, index);
					}
					if (isListItem) {
						element.SetParent(list, int.MaxValue);
						element.EntryS = "LI";
					}
					if ((!isListItem) && (list != null))
						list = null;
				}
				doc.Save(dst);
			}
		}

		private void button10_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithCustomTags(Path.Combine(theRez, "8-detect_custom_tags.pdf"));
		}

		private static void AccessibilityWithCustomTags(string dst) {
			using (Doc doc = new Doc()) {
				// add some content and some custom tags
				if (true) {
					doc.Rect.Inset(50, 50);
					doc.Page = doc.AddPage();
					Page page = (Page)doc.ObjectSoup[doc.Page];
					page.AddLayer(new StreamObject(doc.ObjectSoup, Encoding.ASCII.GetBytes("BX\r\n")));
					doc.AddTextStyled("<p tag-start=\"/H1 TAG\"><stylerun fontsize='72'><b>De Bello Gallico</b></stylerun></p>");
					doc.AddTextStyled("<p tag-start=\"/H2 TAG\"><stylerun fontsize='36'>Julius Caesar</stylerun></p>");
					doc.AddTextStyled("<p><stylerun fontsize='18'>&nbsp;</stylerun></p>");
					doc.AddTextStyled("<p tag-start=\"/P TAG\"><stylerun fontsize='18'><b>Gallia</b> est omnis divisa in partes tres, quarum unam incolunt <b>Belgae</b>, aliam <b>Aquitani</b>, tertiam qui ipsorum lingua <b>Celtae</b>, nostra <b>Galli</b> appellantur.</stylerun></p>");
					page.AddLayer(new StreamObject(doc.ObjectSoup, Encoding.ASCII.GetBytes("EX\r\n")));
				}
				// make accessible
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				// work out how our custom tags match up with the standard PDF tags
				Dictionary<int, Dictionary<int, string>> allCustomTags = new Dictionary<int, Dictionary<int, string>>();
				for (int i = 0; i < doc.PageCount; i++) {
					// get page contents
					doc.PageNumber = i + 1;
					Dictionary<int, string> customTags = new Dictionary<int, string>();
					allCustomTags[doc.Page] = customTags;
					List<int> deletion = new List<int>();
					Page page = (Page)doc.ObjectSoup[doc.Page];
					ArrayAtom array = ArrayAtom.FromContentStream(page.GetContentData());
					var items = OpAtom.Find(array, new string[] { "BDC", "TAG" });
					string style = null;
					// scan for tags
					foreach (var pair in items) {
						if (pair.Item1 == "BDC") {
							NumAtom mcid = Atom.GetItem(array[pair.Item2 - 1], "MCID") as NumAtom;
							if (mcid != null)
								customTags[mcid.Num] = style;
						}
						else {
							style = Atom.GetName(array[pair.Item2 - 1]);
							deletion.Add(pair.Item2 - 1);
							deletion.Add(pair.Item2);
						}
					}
					// remove the custom tags we added
					for (int j = deletion.Count - 1; j >= 0; j--)
						array.RemoveAt(deletion[j]);
					byte[] arrayData = array.GetData();
					StreamObject so = new StreamObject(doc.ObjectSoup);
					so.SetData(arrayData, 1, arrayData.Length - 2);
					so.CompressFlate();
					doc.SetInfo(page.ID, "/Contents:Del", "");
					page.AddLayer(so);
				}
				// determine what custom styles are used and change tag type appropriately
				Structure tags = new Structure(doc);
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				foreach (StructureElementElement element in elements) {
					var page = element.EntryPg;
					if (page == null)
						continue; // this should never happen
					Dictionary<Page, HashSet<int>> mcidMap = new Dictionary<Page, HashSet<int>>();
					element.FindMcids(mcidMap);
					if (mcidMap.Count != 1)
						continue; // paragraph spread across more than one page
					HashSet<int> mcids = null;
					mcidMap.TryGetValue(page.Object as Page, out mcids);
					if (mcids == null)
						continue;
					// match mcids with custom tags
					Dictionary<int, string> customTags = allCustomTags[page.Object.ID];
					Dictionary<string, int> styles = new Dictionary<string, int>();
					foreach (int mcid in mcids) {
						string style = null;
						customTags.TryGetValue(mcid, out style);
						if (!string.IsNullOrWhiteSpace(style)) {
							int count = 0;
							styles.TryGetValue(style, out count);
							styles[style] = count + 1;
						}
					}
					// if verdict is unanimous assign new style
					if (styles.Count == 1)
						element.EntryS = (new List<string>(styles.Keys))[0];
				}
				doc.Save(dst);
			}
		}

		private void button6_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithSections(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "2-detect_sections.pdf"));
		}

		private static void AccessibilityWithSections(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				doc.Save(dst);
				Structure tags = new Structure(doc);
				// reorder elements so that they go from top to bottom
				StructureElementElement document = (StructureElementElement)tags.Root.EntryK[0];
				KidArranger.SortTopToBottom(tags, document, null);
				// define structure hierarchy template
				List<Heading> template = new List<Heading>();
				template.Add(new Heading("Title ", "Part", 1));
				template.Add(new Heading("Subheader ", "Sect", 2));
				// detect and insert section item structure
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				Stack<StructureElementElement> hierarchy = new Stack<StructureElementElement>();
				foreach (StructureElementElement element in elements) {
					string text = element.EntryActualText.TrimStart();
					foreach (Heading item in template) {
						if (text.StartsWith(item.Match)) {
							while (hierarchy.Count >= item.Level)
								hierarchy.Pop();
							if (hierarchy.Count == item.Level - 1) {
								StructureElementElement heading = new StructureElementElement(document);
								heading.EntryType = "StructElem";
								heading.EntryS = item.Type;
								StructureElementElement parent = (StructureElementElement)element.EntryP;
								heading.SetParent(hierarchy.Count > 0 ? hierarchy.Peek() : parent, Int32.MaxValue);
								hierarchy.Push(heading);
							}
							break;
						}
					}
					if (hierarchy.Count > 0)
						element.SetParent(hierarchy.Peek(), Int32.MaxValue);
				}
				doc.Save(dst);
			}
		}

		private void button7_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AccessibilityWithArtifacts(Path.Combine(theRez, "_Romans.pdf"), Path.Combine(theRez, "5-detect_artifacts.pdf"), false);
		}

		private static void AccessibilityWithArtifacts(string src, string dst, bool frame) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				// define header and footer areas
				Dictionary<int, XRect> headers = new Dictionary<int, XRect>();
				Dictionary<int, XRect> footers = new Dictionary<int, XRect>();
				for (int i = 0; i < doc.PageCount; i++) {
					doc.PageNumber = i + 1;
					XRect header = doc.CropBox.Clone();
					header.Pin = XRect.Corner.TopLeft;
					header.Height = 2.0 * 72;
					headers[doc.Page] = header;
					XRect footer = doc.CropBox.Clone();
					footer.Pin = XRect.Corner.BottomLeft;
					footer.Height = 2.0 * 72;
					footers[doc.Page] = footer;
					if (frame) { // mark red so we can see where they are
						doc.Color.SetRgb(255, 0, 0, 40);
						doc.Rect.String = header.ToString();
						doc.FillRect();
						doc.Rect.String = footer.ToString();
						doc.FillRect();
					}
				}
				// make accessible
				AccessibilityOperation op = new AccessibilityOperation(doc);
				op.PageContents.AddPages();
				op.MakeAccessible();
				doc.Save(dst);
				Structure tags = new Structure(doc);
				// reorder elements so that they go from top to bottom
				StructureElementElement document = (StructureElementElement)tags.Root.EntryK[0];
				KidArranger.SortTopToBottom(tags, document, null);
				Dictionary<Page, HashSet<int>> artifacts = new Dictionary<Page, HashSet<int>>();
				// find header elements and remove from structure
				List<StructureElementElement> elements = tags.FindElementsByType("P");
				foreach (StructureElementElement element in elements) {
					var page = element.EntryPg;
					if ((page != null) && (headers[page.Object.ID].Contains(element.GetBBox())) || (footers[page.Object.ID].Contains(element.GetBBox()))) {
						Debug.Assert(element.Object is StreamObject == false, "This should never happen using ABCpdf. We should only ever see Pages.");
						element.FindMcids(artifacts);
						element.SetParent(null, int.MaxValue);
					}
				}
				// turn header elements into artifacts
				foreach (KeyValuePair<Page, HashSet<int>> pair in artifacts) {
					Page page = pair.Key;
					page.Flatten(true, false);
					HashSet<int> mcids = pair.Value;
					foreach (StreamObject layer in page.GetLayers()) {
						if (!layer.Decompress())
							throw new Exception("Unable to decompress stream.");
						bool changed = false;
						ArrayAtom array = ArrayAtom.FromContentStream(layer.GetData());
						// eg "/Span << /MCID 11 >> BDC" becomes "/Artifact << /Type /Pagination /Subtype /Header>> BDC"
						var items = OpAtom.Find(array, new string[] { "BDC" });
						foreach (var pair2 in items) {
							Atom[] args = OpAtom.GetParameters(array, pair2.Item2);
							DictAtom pars = (DictAtom)args[1];
							NumAtom mcid = (NumAtom)pars["MCID"];
							if ((mcid != null) && (mcids.Contains(mcid.Num))) {
								NameAtom name = (NameAtom)args[0];
								name.Text = "Artifact";
								pars.Clear();
								pars["Type"] = new NameAtom("Pagination");
								pars["Subtype"] = new NameAtom("Header");
								changed = true;
							}
						}
						if (changed) {
							byte[] arrayData = array.GetData();
							layer.SetData(arrayData, 1, arrayData.Length - 2);
						}
						layer.CompressFlate();
					}
				}
				doc.Save(dst);
			}
		}

		private void button8_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			foreach (string file in Directory.GetFiles(theRez, "*.pdf")) {
				if (Path.GetFileName(file).StartsWith("_"))
					continue;
				MarkupStructure(file, Path.Combine(theRez, "_" + Path.GetFileNameWithoutExtension(file) + "_Markup.pdf"));
			}
		}

		private static void MarkupStructure(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);
				Structure tags = new Structure(doc);
				tags.MarkupStructure();
				doc.Save(dst);
			}
		}

		private void button9_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AddTaggedSignature(Path.Combine(theRez, "_Flowers.pdf"), Path.Combine(theRez, "7-tagged-signature.pdf"));
		}

		private static void AddTaggedSignature(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);

				// find place in structure
				Structure tags = new Structure(doc);
				List<StructureElementElement> elements = tags.FindElementsByType("Span");
				Debug.Assert(elements.Count == 1);
				StructureElementElement signatureTag = elements[0];
				doc.Page = signatureTag.GetPage(tags).ID;

				// create signature and add to page
				// we use a restricted set of options because it seems that eSignLive may not
				// always understand all possible options 9th April 2018
				CatalogElement catalog = new CatalogElement(doc.ObjectSoup.Catalog);
				PageObjectElement page = new PageObjectElement(doc.ObjectSoup[doc.Page]);
				int id = doc.AddObject("<</FT /Sig /Subtype /Widget /Type /Annot >>");
				AnnotationElement sigAnnot = new AnnotationElement(doc.ObjectSoup[id]);
				sigAnnot.EntryP = page;
				XRect sigRect = XRect.FromLbwh(150, 600, 300, 100);
				sigAnnot.EntryRect = new RectangleElement(ArrayAtom.FromXRect(sigRect), sigAnnot.Host);
				SignatureFieldElement sigField = new SignatureFieldElement(doc.ObjectSoup[id]);
				sigField.EntryT = "Signature1"; // name of signature field
				sigField.EntryTU = "My Signature"; // display name of signature field
				if (page.EntryAnnots == null)
					page.EntryAnnots = new ArrayElement<AnnotationElement>(page);
				page.EntryAnnots.Add(sigAnnot);
				if (catalog.EntryAcroForm == null)
					catalog.EntryAcroForm = new InteractiveFormElement(catalog);
				if (catalog.EntryAcroForm.EntryFields == null)
					catalog.EntryAcroForm.EntryFields = new ArrayElement<FieldElement>(catalog);
				catalog.EntryAcroForm.EntrySigFlags = 1;
				catalog.EntryAcroForm.EntryFields.Add(sigField);

				// link into accessibility structure
				StructureElementElement form = new StructureElementElement(page);
				form.EntryType = "StructElem";
				form.EntryT = "";
				form.EntryS = "Form";
				form.EntryP = signatureTag;
				form.EntryK = new ArrayElement<Element>(form);
				signatureTag.EntryK.Add(form);
				ObjectReferenceElement objr = new ObjectReferenceElement(page);
				objr.EntryType = "OBJR";
				objr.EntryObj = sigAnnot;
				objr.EntryPg = page;
				form.EntryK.Add(objr);
				tags.ParentTree.AddStructParent(sigAnnot.Object, form.Object);
				doc.Save(dst);
			}
		}

		private void button12_Click(object sender, EventArgs e) {
			string theBase = Directory.GetCurrentDirectory();
			string theRez = Directory.GetParent(theBase).Parent.FullName + "\\";

			AddTaggedContent(Path.Combine(theRez, "_Flowers.pdf"), Path.Combine(theRez, "A-add-tagged-content.pdf"));
		}

		private static void AddTaggedContent(string src, string dst) {
			using (Doc doc = new Doc()) {
				doc.Read(src);

				Page page = null;
				StructureElementElement root = null;
				bool addToExistingPage = true, addToNewPage = true, addStampToExistingPage = true;

				// if there is no structure then add one
				var cat = doc.ObjectSoup.Catalog;
				if (Atom.GetItem(cat.Atom, "StructTreeRoot") == null) {
					int tree = doc.AddObject("<< /Nums [] >>");
					int roles = doc.AddObject("<< /Article /Art /CustomImageTag /Figure /CustomTextTag /Sect /NormalParagraphStyle /P >>");
					int document = doc.AddObject("<< /Lang (en-US) /S /Document >>");
					int parents = doc.AddObject("<< /ParentTreeNextKey 0 /Type /StructTreeRoot >>");
					Atom.SetItem(cat.Atom, "MarkInfo", Atom.FromString("<< /Marked true >>"));
					Atom.SetItem(cat.Atom, "ViewerPreferences", Atom.FromString("<< /Direction /L2R >>"));
					Atom.SetItem(cat.Atom, "StructTreeRoot", new RefAtom(cat.Soup[parents]));
					Atom.SetItem(cat.Soup[parents].Atom, "K", new RefAtom(cat.Soup[document]));
					Atom.SetItem(cat.Soup[parents].Atom, "ParentTree", new RefAtom(cat.Soup[tree]));
					Atom.SetItem(cat.Soup[parents].Atom, "RoleMap", new RefAtom(cat.Soup[roles]));
					Atom.SetItem(cat.Soup[document].Atom, "P", new RefAtom(cat.Soup[parents]));
					addToExistingPage = false;
				}

				if (addToExistingPage || addToNewPage) {
					// find insertion point at end of structure
					var tags = new Structure(doc);
					var elements = tags.FindElementsByType("Document");
					Debug.Assert(elements.Count == 1);
					root = elements[0];
					if (root.EntryK != null && root.EntryK.Count > 0) {
						var last = (StructureElementElement)root.EntryK[root.EntryK.Count - 1];
						page = last.GetPage(tags); // NB lastTag.EntryP points to parent element
					}
				}

				if (addToExistingPage) {
					int firstID = GetNextMcid(page), nextID = firstID;

					// create some content and add to page
					doc.Page = page.ID;
					doc.Rect.SetRect(0, 0, doc.MediaBox.Width, 150);
					doc.TextStyle.HPos = 0.5;
					doc.TextStyle.Size = 24;
					StartTag(page, "Span", ref nextID);
					doc.AddText("Some new additional content for the page.\r\n");
					EndTag(page);
					StartTag(page, "Span", ref nextID);
					doc.AddText("Continued here as we have more content.");
					EndTag(page);

					// create accessibility tag for content and link into structure
					var para = MakeElement("P", root);
					for (int i = firstID; i < nextID; i++)
						AddKid(para, i);
					AddKid(root, para);

					// update parent tree
					// we pass all items that have kids that are tag IDs
					// we pass them in the same order as they were created
					var pg = new PageObjectElement(page);
					AddPageItemsToTree(doc, pg, para);
				}

				if (addToNewPage) {
					// add some content to a new page
					doc.Page = doc.AddPage();
					doc.Rect.String = doc.MediaBox.String;
					doc.Rect.Inset(72, 72);
					doc.TextStyle.HPos = 0.5;

					int nextID = 0; // there are no MCIDs on an empty page
					page = (Page)doc.ObjectSoup[doc.Page];

					int s1 = StartTag(page, "Span", ref nextID);
					doc.AddText("Div one paragraph one.\r\n");
					EndTag(page);
					int s2 = StartTag(page, "Span", ref nextID);
					doc.AddText("Div one paragraph two.\r\n");
					EndTag(page);
					int s3 = StartTag(page, "Span", ref nextID);
					doc.AddText("Div two paragraph one.\r\n");
					EndTag(page);
					int s4 = StartTag(page, "Span", ref nextID);
					doc.AddText("Div two paragraph two.\r\n");
					EndTag(page);

					// create accessibility tags for content and link into structure
					var sect = AddKid(root, MakeElement("Sect", root));
					var div = AddKid(sect, MakeElement("Div", root));
					var p1 = AddKid(div, MakeElement("P", root));
					AddKid(p1, s1);
					AddKid(p1, s2);
					var p2 = AddKid(div, MakeElement("P", root));
					AddKid(p2, s3);
					AddKid(p2, s4);
					AddKid(root, sect);

					// update parent tree
					// we pass all items that have kids that are tag IDs
					// we pass them in the same order as they were created
					var pg = new PageObjectElement(page);
					pg = AddPageItemsToTree(doc, pg, p1, p2);
				}

				if (addStampToExistingPage) {
					page = (Page)doc.ObjectSoup[doc.Page];

					doc.TextStyle.HPos = 0;
					var stamp = new StampAnnotation(doc, XRect.FromLbwh(200, 400, 100, 40), "SECRET", XColor.FromRgb(255, 0, 0));

					// create accessibility tag for annotation and link into structure
					var figure = MakeElement("Figure", root);
					figure.EntryAlt = "Classification: Secret";
					AddKid(root, figure);
					AddKid(figure, stamp, page);

					var tags = new Structure(doc);
					tags.ParentTree.AddStructParent(stamp, figure.Object);
				}

				doc.Save(dst);
			}
		}

		private static int GetNextMcid(Page page) {
			var sp = new Structure.StructParent(page, page);
			sp.LoadMCIDs();
			var mcids = sp.MCIDs.Keys;
			return mcids.Count > 0 ? mcids.Max() + 1 : 0;
		}

		static int StartTag(Page page, string type, ref int mcid) {
			page.AddLayer($" /{type} << /MCID {mcid} >> BDC ");
			return mcid++;
		}

		static void EndTag(Page page) {
			page.AddLayer($" EMC ");
		}

		static StructureElementElement MakeElement(string type, Element relation) {
			var item = new StructureElementElement(relation);
			item.EntryType = "StructElem"; // optional entry
			item.EntryS = type;
			return item;
		}

		static void AddKid(StructureElementElement parent, int mcid) {
			if (parent.EntryK == null)
				parent.EntryK = new ArrayElement<Element>(parent);
			parent.EntryK.Add(new Element(new NumAtom(mcid), parent.Host));
		}

		static void AddKid(StructureElementElement parent, IndirectObject obj, Page page) {
			if (parent.EntryK == null)
				parent.EntryK = new ArrayElement<Element>(parent);
			var objr = new ObjectReferenceElement(parent);
			objr.EntryType = "OBJR";
			objr.EntryObj = new Element(obj);
			objr.EntryPg = new PageObjectElement(page);
			parent.EntryK.Add(objr);
		}

		static StructureElementElement AddKid(StructureElementElement parent, StructureElementElement kid) {
			kid.SetParent(parent);
			return kid;
		}

		private static PageObjectElement AddPageItemsToTree(Doc doc, PageObjectElement page, params StructureElementElement[] elements) {
			var cat = new CatalogElement(doc.ObjectSoup.Catalog);
			var parentTree = cat.EntryStructTreeRoot.EntryParentTree;
			var tree = new NumberTree<Element>(parentTree);
			ArrayAtom array = null;
			if (page.EntryStructParents == null) {
				int id = cat.EntryStructTreeRoot.EntryParentTreeNextKey.Value;
				cat.EntryStructTreeRoot.EntryParentTreeNextKey = id + 1;
				page.EntryStructParents = id;
				array = new ArrayAtom();
				tree[id] = new Element(array, cat.Host);
			}
			else {
				int id = page.EntryStructParents.Value;
				var e = tree[id];
				array = e.ArrayAtom;
				// add some Pg entries because PAC is not very good at this
				foreach (var item in array) {
					var se = new StructureElementElement(item, cat.Host);
					if (se.EntryK != null)
						se.EntryPg = page;
				}
			}
			foreach (var element in elements) {
				foreach (var k in element.EntryK) {
					if (k.NumAtom != null)
						array.Add(new RefAtom(element.Object));
				}
				element.EntryPg = page;
			}
			return page;
		}
	}

	class Heading {
		public string Match;
		public string Type;
		public int Level;

		public Heading(string match, string type, int level) { Match = match; Type = type; Level = level; }
	}

	class Style {
		public string Size;
		public List<StructureElementElement> Elements;

		public Style(string size, List<StructureElementElement> elements) { Size = size; Elements = elements; }
	};
}



