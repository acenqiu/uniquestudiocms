<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostList.ascx.cs" Inherits="UniqueStudio.ComContent.PL.controls.PostList" %>
<%@ Import Namespace="UniqueStudio.Common.Config" %>
<div class="column-head">
    <span>
        <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal><a href="list.aspx?catId=<%=CategoryId %>">more...</a></span></div>
<div class="column-content">
    <ul>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <li><a href='view.aspx?catId=<%=CategoryId %>&uri=<%# Eval("Uri") %>' title='<%# Eval("Title") %>'
                    <%# Convert.ToDateTime(Eval("LastEditDate")) >= DateTime.Now.AddDays(-WebSiteConfig.NewImageThreshold) ? "class='new'" : ""  %>>
                    <%# Eval("Title").ToString().Length > 15 ? Eval("Title").ToString().Substring(0, 15)+"..." : Eval("Title")%></a>
                    <span class="postdate">
                        <%# Convert.ToDateTime(Eval("LastEditDate")).ToString(WebSiteConfig.TimeFormatOfIndexPostList) %></span></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
