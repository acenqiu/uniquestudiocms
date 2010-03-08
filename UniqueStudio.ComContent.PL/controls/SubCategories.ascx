<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCategories.ascx.cs"
    Inherits="UniqueStudio.ComContent.PL.controls.SubCategories" %>
<div class="column-head">
    <span>
        <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal></span></div>
<div class="column-content">
    <ul>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <li><a href='list.aspx?catId=<%# Eval("CategoryID") %>'>
                    <%# Eval("CategoryName") %></a></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
