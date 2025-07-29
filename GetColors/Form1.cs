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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Diagnostics;

using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;
using WebSupergoo.ABCpdf13.Operations;
using WebSupergoo.ABCpdf13.Elements;
using System.Net.Http.Headers;

namespace GetColors {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			backgroundWorker1.WorkerReportsProgress = true;
		}

		internal string Root;
		internal List<string> Files;

		private void Form1_Load(object sender, EventArgs e) {
			Root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\";
			Files = new List<string>();
		}

		private void Form1_Activated(object sender, EventArgs e) {
			string item = comboBox1.SelectedItem != null ? (string)comboBox1.SelectedItem : null;
			comboBox1.Items.Clear();
			foreach (string path in Directory.GetFiles(Root, "*.pdf", SearchOption.AllDirectories))
				comboBox1.Items.Add(path);
			foreach (string path in Directory.GetFiles(Root, "*.eps", SearchOption.AllDirectories))
				comboBox1.Items.Add(path);
			if (comboBox1.Items.Count > 0)
				comboBox1.SelectedIndex = item == null ? 0 : comboBox1.FindStringExact(item);
		}

		private void button1_Click(object sender, EventArgs e) {
			string file = (string)comboBox1.SelectedItem;
			// get colors
			ColorScanner scanner = new ColorScanner();
			HashSet<AbstractColor> colors = new HashSet<AbstractColor>();
			TimeSpan time = scanner.Scan(file, colors);
			// sort colors
			List<AbstractColor> list = new List<AbstractColor>(colors);
			list.Sort((i1, i2) => {
				string i1s = (i1.Space == null ? "na" : i1.Space.ColorSpace.ToString()) + i1.Color.ToString();
				string i2s = (i2.Space == null ? "na" : i2.Space.ColorSpace.ToString()) + i2.Color.ToString();
				return i1s.CompareTo(i2s);
			});
			// report colors
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Document \"" + Path.GetFileName(file) + "\" contains " + colors.Count.ToString() + " colors.");
			foreach (AbstractColor color in list)
				sb.AppendLine(color.ToString());
			sb.AppendLine("");
			sb.AppendLine($"Processed in {(int)time.TotalMilliseconds} ms.");
			textBox1.Text = sb.ToString();
		}

		private void button2_Click(object sender, EventArgs e) {
			MakeSimpleSvg((string)comboBox1.SelectedItem, 0);
		}

		private void button3_Click(object sender, EventArgs e) {
			List<string> files = new List<string>();
			for (int i = 0; i < comboBox1.Items.Count; i++)
				files.Add((string)comboBox1.Items[i]);
			Files = files;
			backgroundWorker1.RunWorkerAsync();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
			int total = Files.Count;
			while (Files.Count > 0) {
				string file = Files[Files.Count - 1];
				Files.RemoveAt(Files.Count - 1);
				int done = total - Files.Count;
				string txt = MakeSimpleSvg(file, done);
				backgroundWorker1.ReportProgress((done * 100) / total, txt);
			}
			backgroundWorker1.ReportProgress(0);
		}
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			textBox1.Text += (string)e.UserState;
			progressBar1.Value = e.ProgressPercentage;
		}

		private string MakeSimpleSvg(string srcPath, int count) {
			string txt = "";
			string dstDir = Path.Combine(Root, "_svg");
			Directory.CreateDirectory(dstDir);
			string name = Path.GetFileNameWithoutExtension(srcPath);
			try {
				txt += $"Document \"{name}\"\r\n";
				TimeSpan time = new TimeSpan();
				SvgExporter scanner = new SvgExporter();
				scanner.Warnings = new List<string>();
				using (Doc doc = new Doc()) {
					Stopwatch timer = new Stopwatch();
					timer.Start();
					doc.Read(srcPath);
					timer.Stop();
					txt += $"Read in {(int)timer.ElapsedMilliseconds} ms.\r\n";
					int pageCount = Math.Min((int)maxPages.Value, doc.PageCount);
					for (int j = 0; j < pageCount; j++) {
						doc.PageNumber = j + 1;
						time += scanner.Scan(doc);
						string svgPath = Path.Combine(dstDir, $"{name}_p{j + 1}.svg");
						File.WriteAllText(svgPath, scanner.Svg.ToString());
						scanner.Svg.Clear();
					}
				}
				txt += $"Scanned in {(int)time.TotalMilliseconds} ms.\r\n";
				if (scanner.Warnings.Count > 0)
					txt += $"Warnings:\r\n" + string.Join("\r\n", scanner.Warnings);
				txt += "\r\n";
			}
			catch (Exception ex) {
				string errPath = Path.Combine(Path.Combine(Root, "_svg"), $"{name}_error.txt");
				File.WriteAllText(errPath, ex.ToString(), Encoding.UTF8);
			}
			return txt;
		}
	}

	public class ColorScanner : ContentStreamScanner {
		public int Precision { get; set; } = 255;

		public ColorScanner() {
			List<string> ops = new List<string>(new string[] { "q", "Q" });
			ops.AddRange(ContentStreamScanner.OperatorCategories.Color);
			Operators = new HashSet<string>(ops);
		}

		public HashSet<AbstractColor> Colors { get; } = new HashSet<AbstractColor>();

		public TimeSpan Scan(string file, HashSet<AbstractColor> colors) {
			Stopwatch timer = new Stopwatch();
			using (Doc doc = new Doc()) {
				doc.Read(file);
				for (int i = 0; i < doc.PageCount; i++) {
					doc.PageNumber = i + 1;
					Page page = (Page)doc.ObjectSoup[doc.Page];
					ContentStreamOperation op = new ContentStreamOperation(doc);
					op.AddPages(i + 1);
					foreach (var pair in op.ArrayAtoms) {
						Colors.Clear();
						timer.Start();
						Process(pair.Key, pair.Value);
						timer.Stop();
						ResolveColorSpaces(page, pair.Key.ID, Colors, colors);
					}
				}
			}
			return timer.Elapsed;
		}

		public override void ProcessItem(IndirectObject owner, ArrayAtom array, string op, int pos) {
			base.ProcessItem(owner, array, op, pos);
			switch (op) {
				case "g":
				case "rg":
				case "k":
				case "sc":
				case "scn":
					Colors.Add(new AbstractColor(State.FillColorSpace, State.FillColor, Precision));
					break;
				case "G":
				case "RG":
				case "K":
				case "SC":
				case "SCN":
					Colors.Add(new AbstractColor(State.StrokeColorSpace, State.StrokeColor, Precision));
					break;
			}
		}

		public void ResolveColorSpaces(Page page, int id, HashSet<AbstractColor> src, HashSet<AbstractColor> dst) {
			IDictionary<string, Atom> spaces = page.GetResourceMap(id, "ColorSpace");
			foreach (string deviceSpace in new string[] { "DeviceGray", "DeviceRGB", "DeviceCMYK" })
				if (!spaces.ContainsKey(deviceSpace))
					spaces[deviceSpace] = new NameAtom(deviceSpace);
			foreach (AbstractColor color in src) {
				if (color.Name != "Pattern") {
					Atom space = spaces[color.Name];
					color.Space = (ColorSpaceElement)ElementFactories.AutodetectFactory(space, page, null);
					color.Name = page.Resolve(space).ToString();
				}
				dst.Add(color);
			}
		}
	}

	public class AbstractColor {
		public AbstractColor() { Precision = 255; }
		public AbstractColor(string name, double[] color, int precision) {
			Name = name; Color = color; Space = null; Precision = precision;
		}

		public double[] Color { get; set; }
		public string Name { get; set; }
		public ColorSpaceElement Space { get; set; }
		public int Precision { get; set; }

		public override int GetHashCode() {
			int hash = 17;
			int count = Color != null ? Color.Length : 0;
			for (int i = 0; i < count; i++)
				hash = hash * 23 + ToInt(Color[i]).GetHashCode();
			if (Name != null)
				hash = hash * 23 + Name.GetHashCode();
			return hash;
		}

		public override bool Equals(object obj) {
			AbstractColor other = obj as AbstractColor;
			if (other == null)
				return false;
			int n1 = Color != null ? Color.Length : 0;
			int n2 = other.Color != null ? other.Color.Length : 0;
			if (n1 != n2)
				return false;
			for (int i = 0; i < n1; i++) {
				if (ToInt(Color[i]) != ToInt(other.Color[i]))
					return false;
			}
			if (Name != other.Name)
				return false;
			return true;
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("Components: ");
			int n1 = Color != null ? Color.Length : 0;
			string format = Precision == 0xFF ? "x2" : (Precision == 0xFFFF ? "x4" : "");
			for (int i = 0; i < n1; i++) {
				if (i != 0)
					sb.Append(", ");
				sb.Append(ToInt(Color[i]).ToString(format));
			}
			sb.Append(" Space: ");
			sb.Append(Name);
			return sb.ToString();
		}

		private int ToInt(double value) {
			return (int)Math.Round(value * Precision);
		}
	}

	public class SvgExporter : ContentStreamScanner {
		public SvgExporter() {
			List<string> ops = new List<string>();
			ops.AddRange(OperatorCategories.SpecialGraphicsState);
			ops.AddRange(OperatorCategories.TextObjects);
			ops.AddRange(OperatorCategories.TextPositioning);
			ops.AddRange(OperatorCategories.TextShowing);
			ops.AddRange(OperatorCategories.TextState);
			ops.AddRange(OperatorCategories.XObjects);
			ops.Add("gs");
			Operators = new HashSet<string>(ops);
		}
		public SvgExporter(SvgExporter parent) : base(parent) {
			Operators = parent.Operators;
		}

		public IndirectObject Owner { get; set; } = null;
		public StringBuilder Svg { get; set; } = new StringBuilder();

		public TimeSpan Scan(Doc doc) {
			Stopwatch timer = new Stopwatch();
			Page page = (Page)doc.ObjectSoup[doc.Page];
			ArrayAtom contents = ArrayAtom.FromContentStream(page.GetContentData());
			bool topLevel = Svg.Length == 0;
			timer.Start();
			if (topLevel) {
				double w = page.MediaBox.Width, h = page.MediaBox.Height;
				Svg.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
				Svg.AppendLine($"<svg width=\"{w}\" height=\"{h}\" x=\"0\" y=\"0\" version=\"1.1\" baseProfile=\"full\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" viewBox=\"0 -{h} {w} {h}\">");
				Svg.AppendLine($"\t<rect x=\"0\" y=\"{-h}\" width=\"{w}\" height=\"{h}\" fill=\"seashell\" />");
			}
			Owner = page;
			Process(page, contents);
			if (topLevel)
				Svg.AppendLine($"</svg>");
			timer.Stop();
			return timer.Elapsed;
		}

		public override void ProcessItem(IndirectObject owner, ArrayAtom contents, string op, int pos) {
			base.ProcessItem(owner, contents, op, pos);
			if (op == "Do") {
				string name = ((NameAtom)contents[pos - 1]).Text;
				FormXObject xobj = Resources.GetResource(owner, ResourceType.XObject, name).Object as FormXObject;
				if (xobj == null)
					return;
				byte[] data = new byte[xobj.CopyDecompressedData()];
				xobj.CopyDecompressedData(data);
				ArrayAtom array = ArrayAtom.FromContentStream(data);
				SvgExporter svg = new SvgExporter(this);
				svg.State.CTM.SetTransform(State.CTM);
				if (xobj.Matrix != null)
					svg.State.CTM.PostMultiply(xobj.Matrix);
				svg.Svg = Svg;
				svg.Owner = xobj;
				svg.Process(xobj, array);
			}
		}

		public override void ShowText(List<int> codes, List<int> widths, List<double> advances, double advanceTotal, StringBuilder text, List<string> strings, bool vertical) {
			GraphicsState state = State;
			FontObject font = (FontObject)Resources.GetResource(Owner, ResourceType.Font, state.TextFont).Object;
			double[] saved = Text.TextMatrix.Elements;
			string comment = VetText(string.Join("", text.ToString())).Replace("--", "\u2013\u2013"); // n-dash characters
			Svg.AppendLine($"\t<g><!-- {WebUtility.HtmlEncode(comment)} -->");
			for (int i = 0; i < codes.Count; i++) {
				string part = strings[i];
				if (part == null)
					part = text[i].ToString();
				double[] trm = Text.GetTextRenderingMatrix(state).Elements;
				string matrix = $"transform=\"matrix({trm[0]} {trm[1]} {trm[2]} {trm[3]} {trm[4]} {-trm[5]})\"";
				string family = $"font-family=\"{WebUtility.HtmlEncode(VetText(font.BaseFont))}\"";
				string html = WebUtility.HtmlEncode(VetText(part));
				Svg.AppendLine($"\t\t<text font-size=\"1\" {matrix} {family}>{html}</text>");
				Text.Advance(advances[i], vertical);
			}
			Svg.AppendLine("\t</g>");
			Text.TextMatrix.Elements = saved;
		}

		public static string VetText(string text) {
			if (text == null)
				return " ";
			bool ok = true;
			for (int i = 0; i < text.Length; i++) {
				if (!XmlConvert.IsXmlChar(text[i])) {
					ok = false;
					break;
				}
			}
			if (ok) return text;
			StringBuilder sb = new StringBuilder(text.Length);
			for (int i = 0; i < text.Length; i++) {
				char c = text[i];
				sb.Append(XmlConvert.IsXmlChar(c) ? c : ' ');
			}
			return sb.ToString();
		}
	}
}


