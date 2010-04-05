﻿<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.list" %>

<%@ Import Namespace="UniqueStudio.Core.Site" %>
<%@ Register Src="~/admin/controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<%@ Register Src="controls/PostStat.ascx" TagPrefix="US" TagName="PostStat" %>
<%@ Register Src="controls/SubCategories.ascx" TagPrefix="US" TagName="SubCategories" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="css/list-style.css" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
    <div class="slider">
        <div class="column mini" style="min-height:150px">
            <US:SubCategories ID="categories" runat="server" />
        </div>
    </div>
    <div class="main-content">
        <div class="site-depth">
            <a href="default.aspx">首页</a><asp:Literal ID="ltlCategoryLink" runat="server" />
        </div>
        <div class="list">
            <ul>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <li><a href='view.aspx?catId=<%=CategoryId %>&uri=<%# Eval("Uri") %>' title='<%# Eval("Title") %>'
                            <%# Convert.ToDateTime(Eval("LastEditDate")) >= DateTime.Now.AddDays(-SiteManager.Config(SiteId).NewImageThreshold) ? "class='new'" : ""  %>>
                            <%# Eval("Title")%></a> <span class="postdate">
                                <%# Convert.ToDateTime(Eval("LastEditDate")).ToString(SiteManager.Config(SiteId).TimeFormatOfSectionPostList) %></span></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <US:Pagination ID="pagination" runat="server" Url="list.aspx?page={0}" NumberOfShow="15" />
        </div>
    </div>
</asp:Content>