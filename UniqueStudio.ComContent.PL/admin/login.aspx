<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="UniqueStudio.ComContent.PL.admin.login" %>
<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>引力中心实验室 - 登录</title>
    <link rel="stylesheet" type="text/css" href="css/admin-style.css" />
    <link rel="Stylesheet" type="text/css" href="css/index.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="header">
            <h5>
                Center for Gravitational Experiments</h5>
            <div class="logo">
                <h1>
                    引力中心实验室</h1>
            </div>
            <h3>
                华中科技大学教育部重点实验室</h3>
        </div>
        <div class="log-in-wrapper">
            <div class="log-in-main">
                <h4>
                    用户登录</h4>
                <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="login"
                    runat="server" DisplayMode="List" ForeColor="#333333" />
                <US:Message ID="message" runat="server" />
                <div class="form-item">
                    <span class="form-item-label">邮箱：</span> <span class="form-item-input">
                        <asp:TextBox ID="txtEmail" CssClass="input-text" runat="server" />
                        <asp:RequiredFieldValidator ID="requireEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationGroup="login" Display="None" ErrorMessage="请输入邮箱" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="您输入的邮箱格式不正确" Display="None" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                            ValidationGroup="login" />
                    </span>
                </div>
                <div class="form-item">
                    <span class="form-item-label">密码：</span> <span class="form-item-input">
                        <asp:TextBox ID="txtPassword" CssClass="input-text" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="requirePassword" runat="server" ControlToValidate="txtPassword"
                            ValidationGroup="login" Display="None" ErrorMessage="请输入密码" /></span></div>
                <div class="form-item">
                    <span class="form-item-label"></span><span class="form-item-input">
                        <asp:CheckBox ID="chbAutoLogin" ForeColor="White" Checked="true" runat="server" Text="下次自动登录" /></span></div>
                <div class="form-item">
                    <asp:Button ID="btnLogin" runat="server" CssClass="input-button" ValidationGroup="login"
                        Text="登录" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
        <div class="footer">
            <p>
                Copyright (C) 华中科技大学教育部重点实验室</p>
            <p>
                地址 Add：中国·华中科技大学 邮编 P.C.：430074</p>
            <p>
                鄂ICP备05000313号 Best view 1024*768</p>
        </div>
    </div>
    </form>
</body>
</html>
