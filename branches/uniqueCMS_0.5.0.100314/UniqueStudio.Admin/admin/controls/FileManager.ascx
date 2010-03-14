<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileManager.ascx.cs" Inherits="UniqueStudio.Admin.admin.controls.FileManager" %>
<div class="toolbar">
    <asp:TextBox ID="txtPath" runat="server" Text="\"/>
    <asp:Button ID="btnGo" runat="server" Text="前往" onclick="btnGo_Click" />
    <asp:Button ID="btnUp" runat="server" Text="向上" onclick="btnUp_Click" />
</div>
<div class="fileview">
文件夹：<br />
    <asp:Label ID="lblDirectories" runat="server"/>
文件：<br />
    <asp:Label ID="lblFiles" runat="server" />
</div>