﻿<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="websiteconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.websiteconfig" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="~/admin/controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
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