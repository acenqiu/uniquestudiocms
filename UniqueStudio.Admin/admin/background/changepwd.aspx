<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="changepwd.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.changepwd" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" runat="server" DisplayMode="List"
        ForeColor="#333333" />
    <div class="tip">
        <p>
            为了防止他人趁您不在时修改您的密码，请提供您的邮箱及旧密码。</p>
    </div>
    <div class="panel">
        <div class="panel_title">
            修改密码</div>
        <div class="panel_body">
            <p>
                请输入邮箱：<asp:TextBox ID="txtEmail" runat="server" /></p>
            <p>
                输入旧密码：<asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" /></p>
            <p>
                输入新密码：<asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" /></p>
            <p>
                确认新密码：<asp:TextBox ID="txtConfirmNewPassword" TextMode="Password" runat="server" /></p>
            <p>
                <asp:Button ID="btnOk" runat="server" Text="确认" OnClick="btnOk_Click" /></p>
            <asp:RequiredFieldValidator ID="requireEmail" runat="server" ControlToValidate="txtEmail"
                Display="None" ErrorMessage="请输入邮箱" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="您输入的邮箱格式不正确" Display="None" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOldPassword"
                Display="None" ErrorMessage="请输入旧密码" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPassword"
                Display="None" ErrorMessage="请输入新密码" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="您输入的密码格式不正确" Display="None" ValidationExpression="^[^ ]{1,40}$"
                ValidationGroup="create" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmNewPassword"
                Display="None" ErrorMessage="请再次输入新密码" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtConfirmNewPassword"
                ControlToCompare="txtNewPassword" Display="None" ErrorMessage="您两次输入的密码不一致。" />
        </div>
    </div>
</asp:Content>
