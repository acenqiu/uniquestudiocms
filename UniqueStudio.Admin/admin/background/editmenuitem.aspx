<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="editmenuitem.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editmenuitem" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        "目标"项表示点击链接后浏览器的行为：
        <ul>
            <li>空白: 在当前窗口后标签页中打开；</li>
            <li>_blank: 在新窗口后新标签中打开；</li>
            <li>_top: 在不包含框架的当前窗口或标签。</li>
        </ul>
    </div>
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="modify"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            编辑菜单项</div>
        <div class="panel_body">
            <p>
                菜单项名称：<asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemName"
                    ValidationGroup="modify" Display="None" ErrorMessage="请输入菜单项名称" />
                链接：<asp:TextBox ID="txtLink" runat="server" />
                目标：<asp:TextBox ID="txtTarget" runat="server" />
                次序：<asp:TextBox ID="txtOrdering" runat="server" />
                父菜单项：<asp:DropDownList ID="ddlItems" runat="server">
                </asp:DropDownList>
            </p>
            <asp:Button ID="btnOk" runat="server" Text="确定" ValidationGroup="update" OnClick="btnOk_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
