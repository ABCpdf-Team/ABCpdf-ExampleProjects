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

namespace TaggedPDF {
	#region Enumerations
	/// <summary>
	/// Type which identifies class map attribute
	/// </summary>
	public enum AttributeObjectType {
		/// <summary>
		/// Array attribute object
		/// </summary>
		Array,
		/// <summary>
		/// Boolean attribute object
		/// </summary>
		Boolean,
		/// <summary>
		/// Dictionary attribute object
		/// </summary>
		Dictionary,
		/// <summary>
		/// Fixed point attribute object
		/// </summary>
		FixedPoint,
		/// <summary>
		/// Fixed point attribute object
		/// </summary>
		Integer,
		/// <summary>
		/// Name attribute object
		/// </summary>
		Name,
		/// <summary>
		/// String attribute object
		/// </summary>
		String,
	}
	#endregion

	#region TaggedObject
	/// <summary>
	/// Base class for all tagged-related classes
	/// </summary>
	public class TaggedObject {
		#region Members
		/// <summary>
		/// Document instance
		/// </summary>
		protected Doc mDoc;
		#endregion

		#region Properties
		/// <summary>
		/// Unique id
		/// </summary>
		protected int mId;
		/// <summary>
		/// Unique id
		/// </summary>
		public int Id {
			get { return mId; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document instance
		/// </summary>
		/// <param name="doc">document instance</param>
		public TaggedObject(Doc doc) {
			mDoc = doc;
		}
		#endregion
	}
	#endregion

	#region StructureElement
	/// <summary>
	/// Structure element
	/// </summary>
	public class StructureElement: TaggedObject {
		#region Members
		/// <summary>
		/// Custom name
		/// </summary>
		protected string mName = string.Empty;
		#endregion

		#region Properties

		/// <summary>
		/// Predefined/custom type
		/// </summary>
		private string mType;
		/// <summary>
		/// Predefined/custom type
		/// </summary>
		public string Type {
			get	{ return mType;	}
		}
		/// <summary>
		/// The title of the structure element, a text string representing it
		/// in human-readable form.
		/// </summary>
		public string Title {
			set	{ mDoc.SetInfo(mId, "/T:Text", value); }
		}
		/// <summary>
		/// Alternate text description of the structure element
		/// </summary>
		public string AltDescription {
			set	{ mDoc.SetInfo(mId, "/Alt:Text", value); }
		}
		/// <summary>
		/// A page object representing (the last page) a
		/// page on which some or all of the content items
		/// designated by the K entry are rendered.
		/// </summary>
		private int mPageRef;
		/// <summary>
		/// A page object representing (the last page) a
		/// page on which some or all of the content items
		/// designated by the K entry are rendered.
		/// </summary>
		public int PageRef {
			get { return mPageRef; }
			set	{
				if (mPageRef == 0)
					mDoc.SetInfo(mId, "/Pg:Ref", value.ToString());
				mPageRef = value;
			}
		}
		/// <summary>
		/// The structure element that is the
		/// immediate parent of this one in the structure hierarchy.
		/// </summary>
		private TaggedObject mParent;
		/// <summary>
		/// The structure element that is the
		/// immediate parent of this one in the structure hierarchy.
		/// </summary>
		public TaggedObject Parent {
			get { return mParent; }
			set {
				mParent = value;
				mDoc.SetInfo(mId, "/P:Ref", value.Id.ToString());
			}
		}
		/// <summary>
		/// Unique index for marked content sequence.
		/// </summary>
		private int mMarkedContentSequenceIndex = -1;
		/// <summary>
		/// Unique index for marked content sequence.
		/// </summary>
		internal int MarkedContentSequenceIndex {
			get { return mMarkedContentSequenceIndex; }
			set {
				mMarkedContentSequenceIndex = value;
				int page = mDoc.GetInfoInt(mId, "/Pg:Ref");
				if (page == mPageRef)
					mDoc.SetInfo(mId, "/K*[]:Num", value.ToString());
				else {
					mDoc.SetInfo(mId, "/K*[]", "<< /Type /MCR /Pg "
						+ mPageRef.ToString() + " 0 R /MCID " + value.ToString()+" >>");
				}
			}
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with pdf document instance, type and name
		/// </summary>
		/// <param name="doc">pdf document instance</param>
		/// <param name="type">structure element type</param>
		public StructureElement(Doc doc, string type)
			: base(doc) {
			mType = type;
			mId = mDoc.AddObject(
				"<< /Type /StructElem /S /" + type + " >>" );
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Add structure element as child
		/// </summary>
		/// <param name="el">structure element to be added as child</param>
		public void AddChild(StructureElement el) {
			// update global storage
			mDoc.SetInfo(mId, "/K*[]:Ref", el.Id.ToString());
		}
		/// <summary>
		/// Add an attribute object
		/// </summary>
		public void AddAttribute(int id) {
			mDoc.SetInfo(mId, "/A*[]:Ref", id.ToString());
		}
		#endregion
	}
	#endregion

	#region RoleMap
	/// <summary>
	/// Role map object provides mapping to standard structure types
	/// </summary>
	public class RoleMap: TaggedObject {
		#region Members
		/// <summary>
		/// Structure tree root instance
		/// </summary>
		private StructureTreeRoot mRoot;
		#endregion

		#region Properties
		/// <summary>
		/// Values/aliases dynamic array
		/// </summary>
		private List<string> mAliases = new List<string>();
		/// <summary>
		/// Values/aliases dynamic array
		/// </summary>
		public List<string> Aliases {
			get { return mAliases; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document and structure tree root instances
		/// </summary>
		/// <param name="doc">document instance</param>
		/// <param name="root">structure tree root instance</param>
		public RoleMap(Doc doc, StructureTreeRoot root)
			: base(doc) {
			mRoot = root;

			// adding structure role map
			mId = mDoc.AddObject("<<>>");

			// update strucute tree root with reference on role map
			mDoc.SetInfo(mRoot.Id, "/RoleMap:Ref", mId.ToString());
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Add alias to predefined structure type
		/// </summary>
		/// <param name="type">predefined structure type</param>
		/// <param name="alias">alias for predefined structure type</param>
		public void AddType(string type, string alias) {
			// update role map with new mapping
			mDoc.SetInfo(mId, "/" + alias + ":Name", type);
			mAliases.Add(alias);
		}
		#endregion
	}
	#endregion

	#region AttributeClass
	/// <summary>
	/// Attribute class is object which holds its name and the set
	/// of attributes( they are identified by its type, key and value )
	/// If many structure elements share the same set of attribute values, they can
	/// be defined as an attribute class sharing the identical attribute object.
	/// </summary>
	public class AttributeClass: TaggedObject {
		#region Members
		/// <summary>
		/// The set of child attributes
		/// </summary>
		private List<AttributeObject> mAttributes = new List<AttributeObject>();
		/// <summary>
		/// Class map instance
		/// </summary>
		private ClassMap mClassMap;
		#endregion

		#region Properties
		/// <summary>
		/// Predefined owner type
		/// </summary>
		private string mOwner;
		/// <summary>
		/// Predefined owner type
		/// </summary>
		public string Owner {
			get { return mOwner; }
			set {
				mOwner = value;
				mDoc.SetInfo( mClassMap.Id, "/" + mName + "/O:Name", value.ToString());
			}
		}
		/// <summary>
		/// Unique attribute class name
		/// </summary>
		private string mName = string.Empty;
		/// <summary>
		/// Unique attribute class name
		/// </summary>
		public string Name {
			get { return mName; }
			set { mName = value; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document instance, class map and unqie name
		/// </summary>
		/// <param name="doc">document instance</param>
		/// <param name="classMap">class map instance</param>
		/// <param name="owner">owner, one of predefined</param>
		/// <param name="name">unique name</param>
		public AttributeClass(Doc doc, ClassMap classMap, string owner, string name)
			: base(doc) {
			mName = name;
			mClassMap = classMap;

			// add attribute class reference to class map dictionary
			mDoc.SetInfo(mClassMap.Id, "/" + mName + ":Ref", "1");

			Owner = owner;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Create and add attribute to the attributes collection
		/// </summary>
		/// <param name="type">attribute type</param>
		/// <param name="key">attribute key</param>
		/// <param name="value">attribute value</param>
		/// <returns>unique attribute id</returns>
		public int AddAttribute(AttributeObjectType type, string key, string value) {
			AttributeObject attrClass = new AttributeObject(
				mDoc, mClassMap, this, type, key, value);

			return AddAttribute(attrClass);
		}
		/// <summary>
		/// Add attribute class sub element to the attributes collection
		/// </summary>
		/// <param name="attrClass">attribute sub element to be added</param>
		/// <returns>unique attribute id</returns>
		public int AddAttribute(AttributeObject attr) {
			mAttributes.Add(attr);

			return attr.Id;
		}
		#endregion
	}
	#endregion

	#region AttributeObject
	/// <summary>
	/// Attribute class' child, identified by its type, key and value.
	/// </summary>
	public class AttributeObject: TaggedObject {
		#region Members
		/// <summary>
		/// Parent class map
		/// </summary>
		private ClassMap mClassMap;
		/// <summary>
		/// Parent attribute class
		/// </summary>
		private AttributeClass mAttributeClass;
		/// <summary>
		/// Attribute class key used for accessing other sub-elements.
		/// Is used for faster access.
		/// </summary>
		private string mAttributeClassKey = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Predefined attribute type
		/// </summary>
		private AttributeObjectType mType = AttributeObjectType.Integer;
		/// <summary>
		/// Predefined attribute type
		/// </summary>
		public AttributeObjectType Type {
			get{ return mType; }
		}
		/// <summary>
		/// Attribute key
		/// </summary>
		private string mKey;
		/// <summary>
		/// Attribute key
		/// </summary>
		public string Key {
			get{ return mKey; }
		}
		/// <summary>
		/// Attribute value. Allowed range depends on attribute type
		/// </summary>
		private string mValue;
		/// <summary>
		/// Attribute value. Allowed range depends on attribute type
		/// </summary>
		public string Value {
			get{ return mValue; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document instance, AttributeObjectType type, key and value
		/// </summary>
		/// <param name="doc">document instance</param>
		/// <param name="classMap">class map parent</param>
		/// <param name="parent">attribute class parent</param>
		/// <param name="type">AttributeObjectType type, one of predefined</param>
		/// <param name="key">unique key</param>
		/// <param name="value">value for specified key</param>
		public AttributeObject(Doc doc, ClassMap classMap, AttributeClass parent,
			AttributeObjectType type, string key, string value): base(doc)
		{
			mClassMap = classMap;
			mAttributeClass = parent;

			// update class map name key
			mAttributeClassKey = "/" + mAttributeClass.Name + "/";

			SetKeyAndValue(type, key, value);
		}
		#endregion

		#region Utility methods
		/// <summary>
		/// Set key and value
		/// </summary>
		/// <param name="type">type to be setted</param>
		/// <param name="key">key to be setted</param>
		/// <param name="value">value to be setted</param>
		private void SetKeyAndValue(AttributeObjectType type, string key, string value) {
			mType = type;

			mKey = key;
			mValue = value;

			// set key
			string keySuffix = key;

			// append value
			switch(type) {
				case AttributeObjectType.Array:
					keySuffix += "";
					break;
				case AttributeObjectType.Boolean:
					keySuffix += ":Bool";
					break;
				case AttributeObjectType.Dictionary:
					keySuffix += "";
					break;
				case AttributeObjectType.FixedPoint:
					keySuffix += ":Num";
					value = ConvertDouble.ToString(value);
					break;
				case AttributeObjectType.Integer:
					keySuffix += ":Num";
					break;
				case AttributeObjectType.Name:
					keySuffix += ":Name";
					break;
				case AttributeObjectType.String:
					keySuffix += ":Text";
					break;
			}

			mDoc.SetInfo(mClassMap.Id, mAttributeClassKey + keySuffix, value);
		}
		#endregion
	}
	#endregion

	#region ClassMap
	/// <summary>
	/// Class map object which provides the association between class names and
	/// attribute objects
	/// </summary>
	public class ClassMap: TaggedObject {
		#region Constants
		/// <summary>
		/// Key for generation unique ids for attribute class entities
		/// </summary>
		private const string DEF_ATTRIBUTE_MAP_GENERATOR_KEY = "AttributeClass";
		#endregion

		#region Members
		/// <summary>
		/// Structure tree root instance
		/// </summary>
		private StructureTreeRoot mRoot;
		/// <summary>
		/// Collection of attribute classes
		/// </summary>
		private List<AttributeClass> mAttributeClasses = new List<AttributeClass>();
		/// <summary>
		/// Hashtable which hold attribute class name as key and
		/// the appropriate atribute class object as value.
		/// </summary>
		private Dictionary<string, AttributeClass> mAttributeClassNames = new Dictionary<string, AttributeClass>();
		#endregion

		#region Properties
		/// <summary>
		/// Create on demand and retrieve attribute class with specified name
		/// </summary>
		public AttributeClass this[string attributeClassKey] {
			get {
				return AddAttributeClass(attributeClassKey);
			}
		}
		/// <summary>
		/// Collection of the attributes. Each attribute is identified
		/// by its type, key and value.
		/// </summary>
		private List<AttributeObject> mAttributes = new List<AttributeObject>();
		/// <summary>
		/// Collection of the attributes. Each attribute is identified
		/// by its type, key and value.
		/// </summary>
		public List<AttributeObject> Attributes {
			get { return mAttributes; }
		}
		#endregion

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document, structure tree root instances
		/// and class map name
		/// </summary>
		/// <param name="doc">document instance</param>
		/// <param name="root">structure tree root instance</param>
		public ClassMap(Doc doc, StructureTreeRoot root)
			: base(doc) {
			mRoot = root;
			// adding structure class map
			mId = mDoc.AddObject("<<>>");
			// update structure tree root with reference on class map
			mDoc.SetInfo(mRoot.Id, "/ClassMap:Ref", mId.ToString());
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Add attribute class
		/// </summary>
		/// <param name="attributeClassKey">attribute class name</param>
		/// <returns>attribute class object</returns>
		public AttributeClass AddAttributeClass(string attributeClassKey) {
			AttributeClass attrClass;
			// add attribute class with specified name if it doesn't exist yet
			if(!mAttributeClassNames.TryGetValue(attributeClassKey, out attrClass)) {
				attrClass = new AttributeClass(mDoc, this,
					"Layout", attributeClassKey);

				mAttributeClasses.Add(attrClass);
				mAttributeClassNames.Add(attributeClassKey, attrClass);
			}

			return attrClass;
		}
		#endregion
	}
	#endregion

	#region StructureTreeRoot
	/// <summary>
	/// The logical structure of the tagged document is described by
	/// a hierarchy of objects called the structure hierarchy or
	/// structure tree.
	/// The structure tree root is at the root of this hierarchy.
	/// </summary>
	public class StructureTreeRoot: TaggedObject {
		private int mParentTreeNextKey;
		private int mParentTreeId;
		private int mParentTreeValueId;
		private int mPage;

		#region Initialize/Finalize methods
		/// <summary>
		/// Initialize object with document and
		/// tagged item references object instances
		/// </summary>
		/// <param name="doc">document instance</param>
		/// <param name="taggedItemRefs">tagged item references object</param>
		public StructureTreeRoot(Doc doc)
			: base(doc) {
			// adding structure tree root
			mId = mDoc.AddObject("<< /Type /StructTreeRoot >>");

			// Lang and MarkInfo requires PDF 1.4
			if (mDoc.GetInfoInt(mDoc.Root, "Version") < 4)
				mDoc.SetInfo(mDoc.Root, "Version", "4");

			// update catalog with reference on structure tree root
			mDoc.SetInfo(mDoc.Root, "/StructTreeRoot:Ref", mId.ToString());

			// update catalog with the mark information dictionary containing
			// information about the document's usage of Tagged PDF conventions
			mDoc.SetInfo(mDoc.Root, "/MarkInfo", "<</Marked true /LetterspaceFlags 0>>");

			// update catalog with a language identifier specifying the natural
			// language for all text in the document except where overridden by
			// language specifications for structure elements or marked content
			mDoc.SetInfo(mDoc.Root, "/Lang:Text", "en");

			// adding parent tree
			mParentTreeId = mDoc.AddObject("<<>>");

			// add parent tree reference
			mDoc.SetInfo(mId, "/ParentTree:Ref", mParentTreeId.ToString());
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Set child reference
		/// </summary>
		/// <param name="childId">child tagged item ID</param>
		public void AddChild(int childId) {
			// add child reference
			mDoc.SetInfo(mId, "/K*[]:Ref", childId.ToString());
		}

		/// <summary>
		/// Add structure element reference
		/// </summary>
		/// <param name="id">structure element reference</param>
		public int AddTreeRef(int id) {
			if (mPage != mDoc.Page) {
				mPage = mDoc.Page;
				string key = mParentTreeNextKey.ToString();
				// StructParent and StructParents requires PDF 1.3
				if (mDoc.GetInfoInt(mPage, "Version") < 3)
					mDoc.SetInfo(mPage, "Version", "3");
				// update page with integer key
				// of the page's entry in the structural parent tree
				mDoc.SetInfo(mPage, "/StructParents:Num", key);
				// adding parent tree value for the page
				mDoc.SetInfo(mParentTreeId, "/Nums*[]:Num", key);
				mParentTreeValueId = mDoc.AddObject("[]");
				mDoc.SetInfo(mParentTreeId, "/Nums*[]:Ref",
					mParentTreeValueId.ToString());
				++mParentTreeNextKey;
				mDoc.SetInfo(mId, "/ParentTreeNextKey:Num", mParentTreeNextKey.ToString());
			}
			int index = mDoc.GetInfoInt(mParentTreeValueId, "*:Count");
			mDoc.SetInfo(mParentTreeValueId, "*[]:Ref", id.ToString());
			return index;
		}
		#endregion
	}
	#endregion
}
