<%@ Page MasterPageFile="Site.Master" Language="C#" ValidateRequest="false" AutoEventWireup="true"
    CodeBehind="addpost.aspx.cs" Inherits="UniqueStudio.ComContent.PL.addpost" %>

<%@ Register Src="controls/PostEditor.ascx" TagPrefix="AO" TagName="PostEditor" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="fileup.css" />
    <link type="text/css" rel="Stylesheet" href="toolTip.css" />

    <script type="text/javascript" language="javascript" src="fileup.js"></script>

    <script type="text/javascript" language="javascript" src="toolTip.js"></script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="main-content">
        <div class="column-head">
            发表文章</div>
        <AO:PostEditor runat="server" ID="editor" Mode="Add" Height="500px" />
    </div>
</asp:Content>
