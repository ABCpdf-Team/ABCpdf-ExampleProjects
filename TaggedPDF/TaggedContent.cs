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
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using System.IO;
using System.Text;

namespace TaggedPDF {
	/// <summary>
	/// This class is used for construction of tagged pdf content and insertion into the document
	/// </summary>
	public class TaggedContent {
		private sealed class ContentsData {
			/// <summary>
			/// PDF version
			/// </summary>
			public int Version;
			/// <summary>
			/// PDF content string
			/// </summary>
			public string Contents;

			/// <summary>
			/// Constructor
			/// </summary>
			public ContentsData(int version, string contents) {
				Version = version;
				Contents = contents;
			}
		}

		#region Members
		/// <summary>
		/// Current tagged item
		/// </summary>
		private StructureElement mTaggedItem;
		/// <summary>
		/// Parent doc
		/// </summary>
		private Doc mDoc;
		/// <summary>
		/// PDF version of the content
		/// </summary>
		private int mVersion;
		/// <summary>
		/// PDF content string
		/// </summary>
		private StringBuilder mContents = new StringBuilder();
		/// <summary>
		/// Temporary storage for parts of the content representing different pages
		/// </summary>
		private List<ContentsData> mPageStore = new List<ContentsData>();

		#endregion

		#region Properties
		/// <summary>
		/// The logical structure of the tagged document is described by
		/// a hierarchy of objects called the structure hierarchy or
		/// structure tree.
		/// The structure tree root is at the root of this hierarchy.
		/// </summary>
		private StructureTreeRoot mStructureTreeRoot;
		/// <summary>
		/// The logical structure of the tagged document is described by
		/// a hierarchy of objects called the structure hierarchy or
		/// structure tree.
		/// The structure tree root is at the root of this hierarchy.
		/// </summary>
		public StructureTreeRoot StructureTreeRoot {
			get {
				if(mStructureTreeRoot == null) {
					mStructureTreeRoot = new StructureTreeRoot(mDoc);
				}
				return mStructureTreeRoot;
			}
		}
		/// <summary>
		/// Role map object which provides mapping to standard structure types
		/// </summary>
		private RoleMap mRoleMap;
		/// <summary>
		/// Role map object which provides mapping to standard structure types
		/// </summary>
		public RoleMap RoleMap {
			get {
				if(mRoleMap == null)
					mRoleMap = new RoleMap(mDoc, StructureTreeRoot);
				return mRoleMap;
			}
		}
		/// <summary>
		/// Class map object which provides the association between class names and
		/// attribute objects
		/// </summary>
		public ClassMap mClassMap;
		/// <summary>
		/// Class map object which provides the association between class names and
		/// attribute objects
		/// </summary>
		public ClassMap ClassMap {
			get {
				if(mClassMap == null)
					mClassMap = new ClassMap(mDoc, StructureTreeRoot);
				return mClassMap;
			}
		}
		/// <summary>
		/// Last/current tagged item
		/// </summary>
		public StructureElement TaggedItem {
			get { return mTaggedItem; }
			set { mTaggedItem = value; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document argument
		/// </summary>
		/// <param name="theDoc">document argument</param>
		public TaggedContent(Doc theDoc) {
			mDoc = theDoc;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Begin tagged item object
		/// </summary>
		/// <param name="name">tagged item name</param>
		/// <returns>tagged item object</returns>
		public StructureElement BeginTaggedItem(string name) {
			StructureElement el = new StructureElement(mDoc, name);
			// is this a tagged item root or it is a child of previously added tagged item
			if (mTaggedItem != null) {
				// set reference on parent item
				el.Parent = mTaggedItem;
				// update parent with reference on child
				mTaggedItem.AddChild(el);
			}
			else {
				// set reference on parent item
				el.Parent = StructureTreeRoot;
				StructureTreeRoot.AddChild(el.Id);
			}

			// update current tagged item
			mTaggedItem = el;

			return el;
		}
		/// <summary>
		/// End tagged items
		/// </summary>
		public void EndTaggedItems() {
			while(mTaggedItem != null) {
				EndTaggedItem();
			}
		}
		/// <summary>
		/// End tagged item object
		/// </summary>
		public void EndTaggedItem() {
			mTaggedItem = mTaggedItem.Parent as StructureElement;
		}
		/// <summary>
		/// Append a string of raw pdf content
		/// </summary>
		/// <param name="theContent">The content</param>
		public void AddString(string theContent) {
			BeginMarkedContentSequence();
			mContents.Append(theContent);
			EndMarkedContentSequence();
		}
		/// <summary>
		/// Start new page
		/// </summary>
		public void StartNewPage() {
			mDoc.Page = mDoc.AddPage();
			mPageStore.Add(new ContentsData(mVersion, mContents.ToString()));
			mVersion = 0;
			mContents.Length = 0;
		}
		/// <summary>
		/// Write content to the doc
		/// </summary>
		public void AddToDoc() {
			IndirectObject page;
			for (int i = 0; i < mPageStore.Count; i++) {
				mDoc.PageNumber = i+1;
				ContentsData contents = mPageStore[i];
				if (contents.Version > 0) {
					page = mDoc.ObjectSoup[mDoc.Page];
					if (page.Version < contents.Version)
						page.Version = contents.Version;
				}
				mDoc.SetInfo(mDoc.FrameRect(), "Stream", contents.Contents);
			}
			if (mContents.Length > 0) {
				if (mDoc.PageNumber != mDoc.PageCount)
					mDoc.PageNumber++;
				if (mVersion > 0) {
					page = mDoc.ObjectSoup[mDoc.Page];
					if (page.Version < mVersion)
						page.Version = mVersion;
				}
				mDoc.SetInfo(mDoc.FrameRect(), "Stream", mContents.ToString());
			}
		}

		/// <summary>
		/// Add a block of tagged text
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="text">The text to be added</param>
		public void AddTaggedText(string tagName, string text) {
			BeginTaggedItem(tagName);
			AddTaggedText(text);
			EndTaggedItem();
		}
		/// <summary>
		/// Add a block of tagged HTML-styled text
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="text">The HTML-styled text to be added</param>
		public void AddTaggedHtml(string tagName, string text) {
			BeginTaggedItem(tagName);
			AddTaggedHtml(text);
			EndTaggedItem();
		}
		/// <summary>
		/// Add a block of tagged text. Uses current tagged item.
		/// </summary>
		/// <param name="text">The text to be added</param>
		public void AddTaggedText(string text) {
			int id = mDoc.AddText(text);
			if (id == 0) {
				NextChain();
				id = mDoc.AddText(text);
			}
			AddTaggedChainedHtml(id);
		}
		/// <summary>
		/// Add a block of tagged HTML-styled text. Uses current tagged item.
		/// </summary>
		/// <param name="text">The HTML-styled text to be added</param>
		public void AddTaggedHtml(string text) {
			int id = mDoc.AddTextStyled(text);
			if (id == 0) {
				NextChain();
				id = mDoc.AddTextStyled(text);
			}
			AddTaggedChainedHtml(id);
		}

		/// <summary>
		/// Continue after Doc.AddText or Doc.AddTextStyled.
		/// </summary>
		/// <param name="id">The id of the object to be chained</param>
		private void AddTaggedChainedHtml(int id) {
			if (id > 0)
				SaveContent(id, false);

			while (mDoc.Chainable(id)) {
				NextChain();
				int oldId = id;
				id = mDoc.AddTextStyled("", id);
				ClearContent(oldId);
				SaveContent(id, false);
			}
			ClearContent(id);
		}
		/// <summary>
		/// Create a new page for the same tagged item.
		/// </summary>
		private void NextChain() {
			StartNewPage();
		}


		/// <summary>
		/// Add tagged image
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="fileName">A file path to the image</param>
		public void AddTaggedImage(string tagName, string fileName) {
			BeginTaggedItem(tagName);
			AddTaggedImage(fileName);
			EndTaggedItem();
		}
		/// <summary>
		/// Add tagged image. Current tagged item is used.
		/// </summary>
		/// <param name="fileName">A file path to the image</param>
		public void AddTaggedImage(string fileName) {
			if (!File.Exists(fileName))
				return;
			using (XImage image = XImage.FromFile(fileName, null)) {
				mDoc.Rect.Width = image.Width;
				mDoc.Rect.Height = image.Height;
				int id = mDoc.AddImage(image);
				SaveContent(id, true);
			}
		}
		#endregion

		#region Utility methods
		/// <summary>
		/// Add a separator to the contents if required
		/// </summary>
		private void AddContentSeparator() {
			if(mContents.Length <= 0)
				return;
			char ch = mContents[mContents.Length-1];
			if(ch != ' ' && ch != '\t' && (ch != '\n'
				|| mContents.Length < 2
				|| mContents[mContents.Length-2] != '\r'))
				mContents.Append("\r\n");
		}
		/// <summary>
		/// Append a separator to the contents if required, followed by the content
		/// </summary>
		/// <param name="theContent">The content</param>
		private void AppendContentWithSeparator(string theContent) {
			if(theContent.Length <= 0)
				return;
			if(theContent[0] != ' ' && theContent[0] != '\t'
				&& !theContent.StartsWith("\r\n"))
				AddContentSeparator();
			mContents.Append(theContent);
		}
		/// <summary>
		/// Begin marked content sequence
		/// </summary>
		public void BeginMarkedContentSequence() {
			if (mTaggedItem.MarkedContentSequenceIndex < 0 || mTaggedItem.PageRef != mDoc.Page) {
				if (mTaggedItem.PageRef == 0) {
					for (StructureElement el = mTaggedItem.Parent as StructureElement;
						el != null; el = el.Parent as StructureElement)
					{
						if (el.PageRef != 0)
							break;
						el.PageRef = mDoc.Page;
					}
				}
				// set reference for page which contains this tagged item
				mTaggedItem.PageRef = mDoc.Page;
				// add structure element reference
				// and update structure elements index of marked content sequence
				mTaggedItem.MarkedContentSequenceIndex = StructureTreeRoot.AddTreeRef(mTaggedItem.Id);
			}
			AddContentSeparator();
			mContents.Append("/");
			mContents.Append(mTaggedItem.Type);
			mContents.Append(" << /MCID ");
			mContents.Append(mTaggedItem.MarkedContentSequenceIndex);
			mContents.Append(" >> BDC");
		}
		/// <summary>
		/// Begin background content sequence
		/// </summary>
		/// <param name="bbox">The bounding box</param>
		/// <param name="ids">The names of the attached sides</param>
		public void BeginBackgroundContentSequence(string bbox, string attached) {
			// do not add /MCID for background
			// Background type requires PDF 1.7
			if (mVersion < 17)
				mVersion = 17;
			AddContentSeparator();
			mContents.Append("/Artifact << /Type /Background /BBox [ ");
			mContents.Append(bbox);
			if (!string.IsNullOrEmpty(attached)) {
				mContents.Append(" ] /Attached [ ");
				mContents.Append(attached);
			}
			mContents.Append(" ] >> BDC");
		}
		/// <summary>
		/// End marked content sequence
		/// </summary>
		public void EndMarkedContentSequence() {
			AddContentSeparator();
			mContents.Append("EMC");
		}
		/// <summary>
		/// Save the object contents
		/// </summary>
		/// <param name="id">The object ID</param>
		/// <param name="clear">Whether it is necessary to clear object contents after saving</param>
		public void SaveContent(int id, bool clear) {
			string streamText = mDoc.GetInfo(id, "Stream");
			if (clear)
				ClearContent(id);

			BeginMarkedContentSequence();
			AppendContentWithSeparator(streamText);
			EndMarkedContentSequence();
		}
		/// <summary>
		/// Save the object contents
		/// </summary>
		/// <param name="ids">The list of object IDs</param>
		/// <param name="bbox">The bounding box</param>
		/// <param name="ids">The names of the attached sides</param>
		/// <param name="clear">Whether it is necessary to clear object contents after saving</param>
		public void SaveBackground(IEnumerable<int> ids, string bbox, string attached, bool clear) {
			string streamText = null;
			foreach (int id in ids) {
				if (streamText == null)
					BeginBackgroundContentSequence(bbox, attached);
				streamText = mDoc.GetInfo(id, "Stream");
				if (clear)
					ClearContent(id);
				AppendContentWithSeparator(streamText);
			}
			if (streamText != null)
				EndMarkedContentSequence();
		}
		/// <summary>
		/// Clear the object contents
		/// </summary>
		/// <param name="id">The object ID</param>
		public void ClearContent(int id) {
			mDoc.Delete(id);
		}

		#endregion
	}
}
