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
using System.IO;
using System.Text;

Console.WriteLine("*** Examples Start ***");

int idx = 1;
var fns = ExampleTests.Tests.GetAll();
var msg = new StringBuilder();
foreach (var pair in fns) {
	try {
		Console.WriteLine($"{idx++} of {fns.Count} - {pair.Key}");
		pair.Value();
	}
	catch (Exception ex) {
		Console.WriteLine(" -- Error");
		msg.AppendLine(pair.Key);
	}
}

Console.WriteLine("*** Examples End ***");

if (msg.Length >= 0) {
	Console.WriteLine();
	Console.WriteLine("Errors in:");
	Console.WriteLine(msg);
}

Console.WriteLine();
Console.WriteLine("Press any key to continue...");
Console.ReadKey();