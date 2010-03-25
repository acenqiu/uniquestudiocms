<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="file.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.file" %>

<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <style type="text/css">
        .file
        {
            padding: 2px;
            padding-left: 20px;
            background: url(../images/file.png) no-repeat;
        }
        .folder a
        {
            text-decoration: underline;
            font-weight: bold;
            color: #0066FF;
        }
        .folder
        {
            padding: 2px;
            padding-left: 20px;
            background: url(../images/folder.png) no-repeat;
        }
    </style>
    <div class="error">
        <p>该功能尚未完成。</p>
    </div>
    <div class="toolbar">
        <asp:TextBox ID="txtPath" runat="server" Width="500px" Text="\" />
        <asp:Button ID="btnGo" runat="server" Text="前往" OnClick="btnGo_Click" />
        <asp:Button ID="btnUp" runat="server" Text="向上" OnClick="btnUp_Click" />
    </div>
    <div class="fileview">
        <asp:Label ID="lblDirectories" runat="server" />
        <asp:Label ID="lblFiles" runat="server" />
    </div>
</asp:Content>
