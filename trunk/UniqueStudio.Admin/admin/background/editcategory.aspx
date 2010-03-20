<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="editcategory.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editcategory" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="update"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            编辑分类</div>
        <div class="panel_body">
            <p>
                分类名称：<asp:TextBox ID="txtCategoryName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCategoryName"
                    Display="None" ValidationGroup="update" ErrorMessage="请输入分类名" />
                别名：<asp:TextBox ID="txtNiceName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNiceName"
                    Display="None" ValidationGroup="update" ErrorMessage="请输入分类别名" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNiceName"
                    Display="None" ValidationGroup="update" ValidationExpression="^[a-z,_,1-9]+$"
                    ErrorMessage="别名格式错误，请使用字母、数字或下划线" />
                说明：<asp:TextBox ID="txtDescription" runat="server" /></p>
            隶属于：
            <asp:DropDownList ID="ddlCategories" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnOk" runat="server" Text="确定" ValidationGroup="update" 
                onclick="btnOk_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" onclick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
