<%@ Page MasterPageFile="Site.Master" Language="C#" ValidateRequest="false" AutoEventWireup="true"
    CodeBehind="addpost.aspx.cs" Inherits="UniqueStudio.ComContent.Admin.addpost" %>

<%@ Register Src="controls/PostEditor.ascx" TagPrefix="AO" TagName="PostEditor" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="css/fileup.css" type="text/css" rel="Stylesheet"/>
    <link href="css/toolTip.css" type="text/css" rel="Stylesheet"/>

    <script src="js/fileup.js" type="text/javascript" language="javascript"></script>
    <script src="js/toolTip.js" type="text/javascript" language="javascript"></script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="main-content">
        <div class="column-head">
            发表文章</div>
        <AO:PostEditor runat="server" ID="editor" Mode="Add" Height="500px" />
    </div>
</asp:Content>
