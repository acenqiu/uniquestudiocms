<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="deletepost.aspx.cs" Inherits="UniqueStudio.ComContent.Admin.deletepost" %>

<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="main-content">
        <div class="column-head">删除文章</div>
        <div>
            <p>
                你确认删除所选文章吗？</p>
            <p class="submits">
                <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
                <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click" />
            </p>
        </div>
        <asp:Panel ID="pnlError" Visible="false" runat="server">
            <p>
                删除过程中出现了异常，以下文章删除失败：</p>
            <p>
                <asp:Literal ID="lblErrorList" runat="server" />
            </p>
            <asp:Button ID="btnOK" runat="server" Text="确认" OnClick="btnOK_Click" />
        </asp:Panel>
    </div>
</asp:Content>
