This project demonstrates our preferred method of printing documents using ABCpdf under .NET.

There are a variety of ways that printing can be implemented under .NET. For example:

- Via the Windows Printing APIs
- Via the .NET Printing APIs
- Via XPS

In addition some printers may support direct printing of certain formats. For example,

- PostScript
- Printer Control Language (PCL)
- PDF
- XPS

Our experience is that the Printing APIs provide the most reliable route. However we also find that there can be a significant overhead using the .NET APIs as opposed to the native ones. This seems to be printer dependent but can change the speed by an order of magnitude.

As such we feel that the fastest and most reliable printing method is via the Windows Printing APIs. However if you prefer to use the .NET Printing APIs you can find example code in the ABCpdfView example project.