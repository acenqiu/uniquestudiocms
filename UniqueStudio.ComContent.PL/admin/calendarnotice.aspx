<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarnotice.aspx.cs"
    Inherits="UniqueStudio.Admin.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <link rel="Stylesheet" href="css/toolTip.css" />
    <link rel="Stylesheet" href="css/dataControl.css" />

    <script language="javascript" type="text/javascript" src="js/toolTip.js"> </script>
    <script language="javascript" type="text/javascript" src="js/base.js"> </script>
    <script language="javascript" type="text/javascript" src="js/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="js/dataControl.js"></script>

    <style type="text/css">
        body
        {
        }
        table
        {
        }
        td
        {
            width: 130px;
        }
        table, tr, td
        {
            border: 1px solid;
            outline: 0px;
            border-spacing: 0px;
            margin: 0px;
        }
    </style>

    <script type="text/javascript">

function addRow()
{
	  var postO=new Object();
  	  postO["action"]="add";
  	  postO["caldate"]='<%=Session["caldate"] %>';
   $.post("datacontrol.ashx",
       postO,
       function(data){
          toolTip("添加成功!",1000);
          window.location.reload();
       });
}
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <a href="#" onclick="addRow()">add</a>
    </div>
    </form>
</body>
</html>
