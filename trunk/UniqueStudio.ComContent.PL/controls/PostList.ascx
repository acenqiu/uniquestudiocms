<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostList.ascx.cs" Inherits="UniqueStudio.ComContent.PL.controls.PostList" %>
<%@ Import Namespace="UniqueStudio.Core.Site" %>
<div class="column-head">
    <span>
        <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal><a href="list.aspx?catId=<%=CategoryId %>">more...</a></span></div>
<div class="column-content">
    <ul>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <li><a href='view.aspx?catId=<%=CategoryId %>&uri=<%# Eval("Uri") %>' title='<%# Eval("Title") %>'
                    <%# Convert.ToDateTime(Eval("LastEditDate")) >= DateTime.Now.AddDays(-SiteManager.Config(1).NewImageThreshold) ? "class='new'" : ""  %>>
                    <%# Eval("Title").ToString().Length > MaxTitleLength ? Eval("Title").ToString().Substring(0, MaxTitleLength)+"..." : Eval("Title")%></a>
                    <span class="postdate">
                        <%# Convert.ToDateTime(Eval("LastEditDate")).ToString(SiteManager.Config(1).TimeFormatOfIndexPostList) %></span></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
