<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarframe.aspx.cs" Inherits="UniqueStudio.ComContent.PL.calendarframe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="controls/Calendar.ascx" TagPrefix="US" TagName="Calendar" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style>
    #form1 a
    {
    	text-decoration:none;
    	display:block;
	 height:100%;
	 width:100%;
    }
     #form1 a:hover
     {
     	font-weight:bold;
     	text-decoration:underline;
     }
     #form1 .currentMonth
     {
     	color:Black;
     }
     #form1 .otherMonth
     {
     	color:Gray;
     }
    </style>

</head>
<body >
    <form id="form1" runat="server">
    <div>
     <US:Calendar ID="showcalendar2" Target="_blank" runat="server"  />
    </div>
    </form>
</body>
</html>
