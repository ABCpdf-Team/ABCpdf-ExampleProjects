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
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using WebSupergoo.ABCpdf13.Atoms;

namespace ReferenceXObject {
	public sealed class ObjectCopy {
		public const int XObjectLocalEntryCount = 3;	// Metadata, OPI, OC

		private Doc _doc;
		private List<string> _xobjLocalEntryList;

		public ObjectCopy(Doc doc) {
			_doc = doc;
			_xobjLocalEntryList = new List<string>();
		}

		public List<string> XObjectLocalEntryList {
			get { return _xobjLocalEntryList; }
		}
		public static string GetXObjectLocalEntryPath(int index) {
			// These are the entries in PixMap that are also valid in Form XObject
			// They are not copied over
			switch(index) {
			case 0: return "/Metadata";
			case 1: return "/OPI";
			case 2: return "/OC";
			}
			throw new ArgumentOutOfRangeException("index");
		}

		private static IndirectObject NewStreamObject(Doc doc, DictAtom dict, int id) {
			// Some IndirectObject derived classes assert the existence of data
			dict["Length"] = new NumAtom(1);
			string dictStr = dict.ToString();
			IndirectObject obj = IndirectObject.FromString(dictStr + "stream\n0\nendstream\n");
			if(id==0)
				id = doc.ObjectSoup.Count;
			doc.ObjectSoup[id] = obj;
			return doc.ObjectSoup[id];
		}

		private static Atom CopyAtom(IndirectObject obj, Doc destDoc, bool allowStreams) {
			if(!allowStreams && obj is StreamObject)
				return null;
			return CopyAtom(obj, obj.Atom, destDoc, allowStreams);
		}
		private static Atom CopyAtom(IndirectObject obj, Atom atom, Doc destDoc, bool allowStreams) {
			if(atom is RefAtom) {
				IndirectObject obj2 = obj.ResolveObj(atom);
				StreamObject streamObj = obj2 as StreamObject;
				if(streamObj==null) {
					Atom atom2 = obj2==null? null: CopyAtom(obj2, destDoc, allowStreams);
					if(atom2==null)
						return null;
					obj2 = IndirectObject.FromString(atom2.ToString());
					int id = destDoc.ObjectSoup.Count;
					destDoc.ObjectSoup[id] = obj2;
					obj2 = destDoc.ObjectSoup[id];
					return new RefAtom(obj2);
				}
				if(!allowStreams)
					return null;
				DictAtom dict = CopyAtom(streamObj, destDoc, allowStreams) as DictAtom;
				if(dict==null)
					return null;
				byte[] data = streamObj.GetData();
				streamObj = (StreamObject)NewStreamObject(destDoc, dict, 0);
				streamObj.SetData(data);
				return new RefAtom(streamObj);
			}

			if(atom is DictAtom) {
				DictAtom dict = new DictAtom();
				foreach(KeyValuePair<string, Atom> pair in (DictAtom)atom) {
					Atom atom2 = CopyAtom(obj, pair.Value, destDoc, allowStreams);
					if(atom2==null)
						return null;
					dict[pair.Key] = atom2;
				}
				return dict;
			}

			if(atom is ArrayAtom) {
				ArrayAtom arr = new ArrayAtom();
				foreach(Atom v in (ArrayAtom)atom) {
					Atom atom2 = CopyAtom(obj, v, destDoc, allowStreams);
					if(atom2==null)
						return null;
					arr.Add(atom2);
				}
				return arr;
			}

			return atom.Clone();
		}

		public PixMap CopyPixMap(PixMap pixmap, Doc destDoc, int id) {
			_xobjLocalEntryList.Clear();
			return CopyPixMap(pixmap, destDoc, id, 0);
		}

		private PixMap CopyPixMap(PixMap pixmap, Doc destDoc, int id, int iLocal) {
			DictAtom dict = new DictAtom();
			foreach(KeyValuePair<string, Atom> pair in (DictAtom)pixmap.Atom) {
				switch(pair.Key) {
				case "Metadata":
					SetListItem(_xobjLocalEntryList, iLocal, pair.Value.ToString());
					break;
				case "OPI":
					SetListItem(_xobjLocalEntryList, iLocal+1, pair.Value.ToString());
					break;
				case "OC":
					SetListItem(_xobjLocalEntryList, iLocal+2, pair.Value.ToString());
					break;
				case "Mask":
				case "SMask":
					if(pair.Value is RefAtom) {
						PixMap mask = pixmap.ResolveObj(pair.Value) as PixMap;
						if(mask==null)
							goto default;
						PixMap destMask = CopyPixMap(mask,
							destDoc, 0, iLocal+XObjectLocalEntryCount);
						dict[pair.Key] = new RefAtom(destMask);
						break;
					}
					goto default;
				default: {
						// Allow stream objects only for ColorSpace
						Atom atom = CopyAtom(pixmap, pair.Value,
							destDoc, pair.Key=="ColorSpace");
						if(atom!=null)
							dict[pair.Key] = atom;
					}
					break;
				}
			}
			PixMap outPixMap = (PixMap)NewStreamObject(destDoc, dict, id);
			byte[] data = pixmap.GetData();
			outPixMap.SetData(data);
			return outPixMap;
		}

		private static void SetListItem(List<string> list, int index, string value) {
			if(list.Count>=index+1)
				list[index] = value;
			else {
				while(list.Count<index)
					list.Add(null);
				list.Add(value);
			}
		}
	}
}
