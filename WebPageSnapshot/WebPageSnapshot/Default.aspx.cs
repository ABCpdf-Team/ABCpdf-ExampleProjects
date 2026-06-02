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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text.RegularExpressions;

using WebSupergoo.ABCpdf14;


public partial class _Default : System.Web.UI.Page {
    public string RandomMessage;
    public string RandomColor;
    public int ImageNumber;

	protected void Page_Load(object sender, EventArgs e) {
		Random rnd = new Random();
        RandomMessage = rnd.Next(0, 999999999).ToString();
        RandomColor = String.Format("#{0:X6}", rnd.Next(0x1000000));
        Session["ImageNumber"] = Session["ImageNumber"] == null ? rnd.Next(1, 6) : ((int)Session["ImageNumber"] % 6) + 1;
	}

	protected void OnButtonClick(object sender, CommandEventArgs e) {
        // put base tag into HTML to link in resources like images or style sheets
		string html = Session["SavedHTML"].ToString();
		if(Regex.Match(html, "<base\\s+href=\"(.*)\"(.*)>", RegexOptions.IgnoreCase).Success)
			throw new Exception("This HTML already has a <base> tag");
		string baseUrlTag = "<base href = \"" + Request.UrlReferrer.AbsoluteUri + "\">";
		html = html.Replace("<head>", "<head>" + baseUrlTag);
        // make the PDF
        XSettings.InstallLicense(@"change me"); // NB use your own license or copy a trial one from PDFSettings (under the ABCpdf menu item)
        byte[] pdfData = null;
        using (Doc doc = new Doc()) {
			doc.HtmlOptions.AddLinks = true;
			doc.Rect.Inset(25, 25);
			int id = doc.AddImageHtml(html);
			while (doc.Chainable(id)) {
				doc.Page = doc.AddPage();
				id = doc.AddImageToChain(id);
			}
            for (int i = 1; i <= doc.PageCount; i++)  {
                doc.PageNumber = i;
                doc.Flatten();
            }
            pdfData = doc.GetData();
        }
        // stream to browser
		Response.Clear();
		Response.ContentType = "application/pdf";
		Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF");
		Response.AddHeader("content-length", pdfData.Length.ToString());
		Response.BinaryWrite(pdfData);
		Response.End();
	}

    protected override void Render(HtmlTextWriter writer)  {
        // Capture the html of the page in a session variable on page load
        TextWriter tw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(tw);
        base.Render(htw);
        string html = tw.ToString();
        Session["SavedHTML"] = html;
        writer.Write(html);
    }
}