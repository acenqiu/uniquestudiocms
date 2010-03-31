<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="controlconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.controlconfig" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="modify"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            控件信息</div>
        <div class="panel_body">
            <div>
                控件名称：<asp:TextBox ID="txtControlName" runat="server" />
                <asp:RequiredFieldValidator ID="require1" runat="server" ControlToValidate="txtControlName"
                    ValidationGroup="modify" Display="None" ErrorMessage="请输入控件名称" />
                <asp:HiddenField ID="hfOldControlName" runat="server" />
                是否启用：<asp:CheckBox ID="chkIsEnabled" Checked="true" runat="server" />
            </div>
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            控件配置</div>
        <div class="panel_body">
            <US:Config ID="config" runat="server" />
            <asp:Button ID="btnSave" runat="server" ValidationGroup="modify" Text="保存" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
