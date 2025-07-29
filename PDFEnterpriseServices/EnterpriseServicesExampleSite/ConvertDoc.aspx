<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="ConvertDoc.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="PdfEnterpriseServices" %>
<%-- Add a reference to the PdfEnterpriseServices project --%>

<%
    PdfUtilities thePdfUtil = new PdfUtilities();
    byte[] theData = thePdfUtil.DocToPdf(Server.MapPath("Letter.doc"));
    Response.Clear();
    Response.ContentType = "application/pdf";
    Response.AddHeader("content-length", theData.Length.ToString());
    //Response.AddHeader("content-disposition", "attachment; filename=MyPDF.PDF");
    Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF");
    Response.BinaryWrite(theData);
    HttpContext.Current.ApplicationInstance.CompleteRequest();
%>
