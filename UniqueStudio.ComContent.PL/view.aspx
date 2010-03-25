<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.view" %>
<%@ Register Src="controls/PostStat.ascx" TagPrefix="US" TagName="PostStat" %>
<%@ Register Src="controls/SubCategories.ascx" TagPrefix="US" TagName="SubCategories" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="css/article-style.css" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
    <div class="slider">
       <div class="column mini" style="min-height:150px">
            <US:SubCategories ID="subCategories" runat="server" />
        </div>
    </div>
    <div class="main-content">
        <div class="site-depth">
            <a href="default.aspx">首页</a><asp:Literal ID="ltlCategoryLink" runat="server" />
        </div>
        <div class="article">
            <h4 class="article-title">
                <asp:Literal ID="ltlTitle" runat="server" /></h4>
            <div class="detail" id="divDetail" runat="server">
                <span class="article-author">作者：<asp:Literal ID="ltlAuthor" runat="server" /></span>
                <span class="views">阅读次数：<asp:Literal ID="ltlCount" runat="server" /></span> <span
                    class="article-date">发表时间：<asp:Literal ID="ltlCreateDate" runat="server" /></span>
            </div>
            <div class="article-content">
                <asp:Literal ID="ltlContent" runat="server" />
            </div>
            <div id="divAttachment" class="attachment" visible="false" runat="server">
                <span>附件下载：</span><span class='file <asp:Literal ID="ltlAttachmentExt" runat="server"/>'>
                <a href='<asp:Literal ID="ltlAttachmentLink" runat="server"/>'>
                    <asp:Literal ID="ltlAttachmentTitle" runat="server"/></a></span>
             </div>
        </div>
    </div>
</asp:Content>
