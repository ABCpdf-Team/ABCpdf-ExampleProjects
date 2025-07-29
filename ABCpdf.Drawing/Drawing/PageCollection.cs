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
using System.Collections.ObjectModel;

namespace WebSupergoo.ABCpdf13.Drawing {
	#region PageCollection
	/// <summary>
	/// Represents a collection of Page objects for a PDF document.
	/// </summary>
	public sealed class PageCollection : ReadOnlyCollection<Page> {

		public PageCollection(IList<Page> sourceList) : base(sourceList) { }
	}
	#endregion
}
