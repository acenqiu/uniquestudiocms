<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true" CodeBehind="compenentconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.compenentconfig" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            组件配置</div>
        <div class="panel_body">
            <US:Config ID="config" runat="server" />
            <asp:Button ID="btnSave" runat="server" ValidationGroup="modify" Text="保存" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
