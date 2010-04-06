<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs"
    Inherits="UniqueStudio.CGE.Search" %>

<%@ Register Assembly="UniqueStudio.Controls" Namespace="UniqueStudio.Controls" TagPrefix="US" %>
<%@ Register Src="controls/SubCategories.ascx" TagPrefix="US" TagName="SubCategories" %>
<%@ Import Namespace="UniqueStudio.Core.Site" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="css/search-style.css" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <div id="divSlider" runat="server" visible="false" class="slider">
        <div class="column mini" style="min-height: 150px" id="category">
            <US:SubCategories ID="categories" runat="server" />
        </div>
        <div class="column mini" style="min-height: 150px">
            <US:Module ID="psList" ModuleName="mod_poststat" runat="server" />
        </div>
    </div>
    <div class="main-content">
        <div class="search-result">
            <div class="result-summary">
                共找到&nbsp;<strong><asp:Literal ID="ltlCount" runat="server" Text="0" /></strong>&nbsp;篇文章</div>
            <hr />
            <asp:Literal ID="ltlMessage" runat="server" />
            <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                    <div class="result-item">
                        <div class="result-title">
                            <a href='view.aspx?uri=<%# Eval("Uri") %>' target="_blank">
                                <%# Eval("Title") %></a>
                        </div>
                        <div class="result-abstract">
                            <%# Eval("Summary") %>
                        </div>
                        <div class="result-extra-info">
                            <span class="result-link"><a href="view.aspx?uri=<%# Eval("Uri") %>">
                                <%=SiteManager.BaseAddress(SiteId) %>
                                %>/view.aspx?uri=<%# Eval("Uri") %></a></span> <span class="result-author">作者：<%# Eval("Author") %></span>
                            <span class="result-publish-date">发表时间：<%# Eval("LastEditDate") %></span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
