<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="UniqueStudio.Admin.admin._default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>后台管理 - UniqueCMS</title>
</head>
<frameset id="mainframes" framespacing="0" border="false" rows="100,*,30" frameborder="0"
    scrolling="yes">
<frame name="top" scrolling="no" src="top.aspx" />
<frameset id="bottomframes" framespacing="0" border="false" cols="220,*" frameborder="0" scrolling="yes">
	<frame name="left" scrolling="auto" marginwidth="0" marginheight="0" src="sidebar.aspx" noresize />
	<frame name="right" scrolling="auto" src="background/default.aspx"/>
</frameset>
<frame name="bottom" scrolling="no" src="bottom.aspx" />
</frameset>
<noframes>
    <body>
        <p>
            This page uses frames, but your browser doesn't support them.</p>
    </body>
</noframes>
</html>
