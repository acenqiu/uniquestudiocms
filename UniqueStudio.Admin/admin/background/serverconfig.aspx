<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true" CodeBehind="serverconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.serverconfig" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            服务器设置<asp:Literal ID="ltlControlId" runat="server" /></div>
        <div class="panel_body">
                <US:Config ID="config" runat="server" />
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
        </div>
    </div>
</asp:Content>