<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" 
Inherits="UniqueStudio.ComContent.Admin.login" %>

<%@ Register Src="controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>引力中心实验室 - 登录</title>
    <link rel="stylesheet" type="text/css" href="css/admin-style.css" />
    <link rel="Stylesheet" type="text/css" href="css/index.css" />
</head>
<body>
    <form id="form1" runat="server">
    用户登录
    <US:Message ID="message" runat="server" />
    邮箱：
    <asp:TextBox ID="txtEmail" CssClass="input-text" runat="server" />
    密码：
    <asp:TextBox ID="txtPassword" CssClass="input-text" runat="server" TextMode="Password" />
    <asp:CheckBox ID="chbAutoLogin" ForeColor="White" Checked="true" runat="server" Text="下次自动登录" /></span></div>
    <asp:Button ID="btnLogin" runat="server" CssClass="input-button" ValidationGroup="login"
        Text="登录" OnClick="btnLogin_Click" />
    </form>
</body>
</html>
