﻿<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.list" %>

<%@ Import Namespace="UniqueStudio.Common.Config" %>
<%@ Register Src="admin/compenents/com_content/admin/controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<%@ Register Src="~/controls/PostStat.ascx" TagPrefix="US" TagName="PostStat" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="list-style.css" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
    <div class="slider">
    
        <div class="column mini" style="display:none">
            <div class="column-head">
                文章统计</div>
            <div class="column-content">
                <US:PostStat ID="postStat" runat="server" />
            </div>
        </div>
        
        <div class="column mini" style="display:none">
            <div class="column-head">
                友情链接</div>
            <div class="column-content">
                <ul>
                    <li><a href="#">ACIGA</a></li>
                    <li><a href="#">IGR</a></li>
                    <li><a href="#">ICGA9</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="main-content">
        <div class="site-depth">
            <a href="default.aspx">首页</a>-><a href="#"><asp:Literal ID="ltlCategoryName" runat="server" /></a>
        </div>
        <div class="list">
            <ul>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <li><a href='view.aspx?catId=<%=CategoryId %>&uri=<%# Eval("Uri") %>' title='<%# Eval("Title") %>'
                            <%# Convert.ToDateTime(Eval("LastEditDate")) >= DateTime.Now.AddDays(-WebSiteConfig.NewImageThreshold) ? "class='new'" : ""  %>>
                            <%# Eval("Title")%></a> <span class="postdate">
                                <%# Convert.ToDateTime(Eval("LastEditDate")).ToString(WebSiteConfig.TimeFormatOfSectionPostList) %></span></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <US:Pagination ID="pagination" runat="server" Url="list.aspx?page={0}" NumberOfShow="15" />
        </div>
    </div>
</asp:Content>
