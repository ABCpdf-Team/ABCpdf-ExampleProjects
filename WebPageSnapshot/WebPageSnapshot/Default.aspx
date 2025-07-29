<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Take a Snapshot of the Current Page</title>
	<style>
		.center{
			text-align:center;
		}
        .left{
			text-align:left;
		}
		html{
			font-family:'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
		}
		.myButton{
			margin:10px auto;
			display:block;
			background-color:#fff;
			border: 1px solid #000;
			padding:10px;
			font-size:1.5em;
			cursor:pointer;
		}
		.number{
			margin:10px auto;
			font-size:3em;
			color:<%:RandomColor%>;"
		}
		#pageFrame{
			border: groove 2px  <%:RandomColor%>;
			margin:20px auto;
			min-width:700px;
			padding:0 5px;
		}
		#pageFrame img{
		}
	</style>
</head>
<body>
    <div id="pageFrame" class="center">
    <h1 class=" center">Take a Snapshot of the Current Page</h1>
		<p class="left">Sometimes you may need to convert a protected page to PDF. These types of pages are not identifiable by URL.</p>
		<p class="left">A typical example of this is a page being viewed by an authenticated user. The user will see a particular page but ABCpdf cannot access this page via URL because the content on the page is both protected and also specific to that user.</p>
		<p class="left">In theory ABCpdf could log on as that user to see the same page, but this is complex to implement, and fragile in the event that security procedures or checks are changed.</p>
		<p class="left">In practice it is much simpler to keep the HTML which was passed to the user and then provide this direct to ABCpdf.</p>
		<p class="left">This sample project shows you how to achieve this. For simplicity we don't use an authenticated site; we use a page that changes each time it is viewed. However the principles are exactly the same.</p>
		<h2 class="number center"><%:RandomMessage%></h2>
		<form id="form1" runat="server">
		<asp:Button id="SubmitButton" Text="ABCpdf this Page!" CommandName="Submit" OnCommand="OnButtonClick" CssClass="myButton" runat="server"/>
		</form>
		<div>
			<img src="img/<%:Session["ImageNumber"]%>.jpg" class="center"/>
			<p>image courtesy of <a href="https://pixabay.com/">pixabay</a></p>
		</div>
    </div></body>
</html>
