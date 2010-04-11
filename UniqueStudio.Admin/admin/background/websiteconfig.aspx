<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true" CodeBehind="websiteconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.websiteconfig" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>在此您可以进行各个子网站相关的配置。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            网站设置<asp:Literal ID="ltlControlId" runat="server" /></div>
        <div class="panel_body">
                <US:Config ID="config" runat="server" />
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
        </div>
    </div>
</asp:Content>