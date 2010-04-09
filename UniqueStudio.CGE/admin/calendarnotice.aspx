<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarnotice.aspx.cs"
    Inherits="UniqueStudio.ComCalendar.Admin.calendernotice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="Stylesheet" href="css/dataControl.css" />
    <link rel="Stylesheet" href="css/toolTip.css" />

    <script language="javascript" type="text/javascript" src="js/toolTip.js"> </script>
    <script language="javascript" type="text/javascript" src="js/dataControl.js"></script>
    <script language="javascript" type="text/javascript" src="js/base.js"></script>
    <script language="javascript" type="text/javascript" src="js/jquery-plus-jquery-ui.js"></script>
    
    <script type="text/javascript">

        function addRow()
        {
            if (!lock)
            {
	            var postO=new Object();
  	            postO["action"]="add";
  	            postO["siteId"]='<%=SiteId %>';
  	            postO["date"]='<%=Date.ToString("yyyy-MM-dd") %>';
                $.post("datacontrol.ashx",
                        postO,
                        function(data){
                            toolTip("添加成功!",1000);
                            window.location.reload();
                        });
            }
            else
            {
                    if(confirm("当前已经有一条记录正在被修改，确认不保存修改？"))
	                {
	                    cancelRow(preRowCache);
		                addRow();
	                }
             }
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            <asp:Literal ID="calendarDate" runat="server"></asp:Literal></h2>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <a href="#" onclick="addRow()" class="addItem">+添加</a>
    </div>
    </form>
</body>
</html>
