<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="deletepost.aspx.cs"
    Inherits="UniqueStudio.ComContent.Admin.deletepost" %>

<%@ Register Src="controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="main-content">
        <div class="column-head">
            删除文章</div>
        <div>
            <US:Message ID="message" runat="server" />
            <p>
                你确认删除所选文章吗？</p>
            <p class="submits">
                <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
                <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click" />
            </p>
        </div>
    </div>
</asp:Content>
