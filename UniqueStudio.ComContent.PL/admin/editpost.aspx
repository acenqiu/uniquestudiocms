<%@ Page MasterPageFile="Site.Master" Language="C#" validateRequest="false" AutoEventWireup="true" CodeBehind="editpost.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.editpost" %>

<%@ Register Src="controls/PostEditor.ascx" TagPrefix="AO" TagName="PostEditor" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="main-content">
        <div class="column-head">
            编辑文章</div>
          <a href='postlist.aspx?<%= PostListQuery %>'>返回文章列表</a>
        <AO:PostEditor runat="server" ID="editor" Mode="Edit" Height="500px" />
    </div>
</asp:Content>
