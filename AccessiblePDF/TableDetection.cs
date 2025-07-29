// ===========================================================================
//	©2013-2022 WebSupergoo. All rights reserved.
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;
using WebSupergoo.ABCpdf13.Operations;
using WebSupergoo.ABCpdf13.Elements;

using Structure = WebSupergoo.ABCpdf13.Operations.Accessibility.Structure;
using ParentTree = WebSupergoo.ABCpdf13.Operations.Accessibility.ParentTree;
using KidArranger = WebSupergoo.ABCpdf13.Operations.Accessibility.KidArranger;


namespace WebSupergoo.TableDetection {
	public class Tables {
		private List<Table> _tables = null;

		public bool FontSensitive { get; set; } = false;

		public IList<Table> Items { get { return _tables; } }

		public void Add(Structure tags, List<StructureElementElement> elements) {
			_tables = new List<Table>();
			_tables.Add(new Table(this));
			Dictionary<int, Rows> pageRows = new Dictionary<int, Rows>();
			foreach (StructureElementElement element in elements) {
				Rows rows = null;
				if (!pageRows.TryGetValue(element.GetPage(tags).ID, out rows)) {
					rows = new Rows(this, tags);
					pageRows[element.GetPage(tags).ID] = rows;
				}
				rows.Add(element);
			}
			foreach (KeyValuePair<int, Rows> pair in pageRows) {
				int page = tags.GetPageNumber(pair.Key);
				Rows rows = pair.Value;
				rows.SortTopToBottom();
				for (int i = 0; i < rows.Items.Count; i++) {
					Row row = rows.Items[i];
					Add(row);
				}
			}
		}

		private void Add(Row row) {
			if (row.Items.Count > 1) {
				int count = Math.Min(_tables.Count, 3); // try last few tables
				for (int i = 0; i < count; i++) {
					if (_tables[_tables.Count - 1 - i].TryAdd(row))
						return;
				}
			}
			Table last = new Table(this);
			_tables.Add(last);
			last.TryAdd(row);
		}
	}

	public class Table {
		private Tables _tables;
		private List<Row> _rows = null;
		private List<XRect> _columns = null;

		private Table() {}

		public Table(Tables tables) {
			_tables = tables;
			_rows = new List<Row>();
			_columns = new List<XRect>();
		}

		public void Clear() {
			_rows.Clear();
			_columns.Clear();
		}

		public IList<Row> Items { get { return _rows; } }
		public Page Page { get { return Items.Count > 0 ? Items[0].Page : null; } }

		public bool TryAdd(Row row) {
			if (_rows.Count == 0) {
				_rows.Add(row);
				// assume that first row defines the column widths
				// if it doesn't then we would need to adjust the bounding
				// boxes as we saw items or indeed perhaps even add new columns
				foreach (StructureElementElement element in row.Items)
					_columns.Add(element.GetBBox());
				return true;
			}
			List<int> blanks = null;
			int i = 0;
			foreach (StructureElementElement element in row.Items) {
				XRect bbox = element.GetBBox();
				if (i >= _columns.Count) {
					// items left but no columns left to put them into
					return false;
				}
				for (int j = i; j < _columns.Count; j++) {
					XRect col_bbox = _columns[j];
					if (bbox.Left > col_bbox.Right) {
						// perhaps this column is blank, see if we fit under the next one
						if (blanks == null)
							blanks = new List<int>();
						blanks.Add(i);
						i++;
						continue;
					}
					if (bbox.Right < col_bbox.Left)
						return false;
					// this matches
					i++;
					break;
				}
			}
			if (blanks != null) {
				foreach (int index in blanks) {
					row.Items.Insert(index, null);
				}
			}
			_rows.Add(row);
			return true;
		}

		private void UpdateColumns() {
			if (_columns == null) {
				_columns = new List<XRect>();
			}
		}
	}

	public class Rows {
		private Tables _tables;
		private Structure _tags;
		private List<Row> _rows = null;

		private Rows() {}

		public Rows(Tables tables, Structure tags) {
			_tables = tables;
			_tags = tags;
			_rows = new List<Row>();
			_rows.Add(new Row(_tables, _tags));
		}

		public IList<Row> Items { get { return _rows; } }
		public Page Page { get { return Items.Count > 0 ? Items[0].Page : null; } }

		public void Add(StructureElementElement element) {
			Row last = _rows[_rows.Count - 1];
			if (!last.TryAdd(element)) {
				last = new Row(_tables, _tags);
				_rows.Add(last);
				last.TryAdd(element);
			}
		}

		public void SortTopToBottom() { // and left to right if top == bottom
			_rows.Sort((i1, i2) =>
				i1.BBox.Top != i2.BBox.Top ? i2.BBox.Top.CompareTo(i1.BBox.Top) : i1.BBox.Left.CompareTo(i2.BBox.Left)
			);
		}
	}

	[DebuggerDisplay("\\{ BBox = {BBox} Page = {Page.ID} Text = {Text} \\}")]
	public class Row {
		private Tables _tables;
		private Structure _tags;
		private List<StructureElementElement> _items = null;
		private XRect _bbox = null;
		private Page _page = null;
		private Dictionary<string, string> _style = null;

		private Row() { }

		public Row(Tables tables, Structure tags) {
			_tables = tables;
			_tags = tags;
			_items = new List<StructureElementElement>();
		}

		public IList<StructureElementElement> Items { get { return _items; } }
		public Page Page { get { return Items.Count > 0 ? Items[0].GetPage(_tags) : null; } }
		public XRect BBox { get { return _bbox; } }
		public string Text { get { StringBuilder sb = new StringBuilder(); foreach (var item in _items) sb.Append(item.EntryActualText); return sb.ToString(); } }

		public bool TryAdd(StructureElementElement element) {
			if (_items.Count == 0) {
				_items.Add(element);
				_bbox = element.GetBBox();
				_page = element.GetPage(_tags);
				_style = element.GetStyle();
				return true;
			}
			// elements need to be on the same page
			if (_page.ID != element.GetPage(_tags).ID)
				return false;
			// elements in rows need to line up horizontally
			XRect bbox = element.GetBBox();
			if ((bbox.Top < _bbox.Bottom) || (bbox.Bottom > _bbox.Top))
				return false;
			if ((_bbox.Top < bbox.Bottom) || (_bbox.Bottom > bbox.Top))
				return false;
			if ((_tables.FontSensitive) && (!StylesCompatible(_style, element.GetStyle())))
				return false;
			_items.Add(element);
			_bbox.Union(bbox);
			return true;
		}

		public static bool StylesCompatible(Dictionary<string, string> style1, Dictionary<string, string> style2) {
			if ((style1 == null) || (style2 == null))
				return true;
			string family1 = null, family2 = null;
			style1.TryGetValue("font-family", out family1);
			style2.TryGetValue("font-family", out family2);
			if ((family1 == null) || (family2 == null))
				return true;
			return family1 == family2;
		}
	}
}
