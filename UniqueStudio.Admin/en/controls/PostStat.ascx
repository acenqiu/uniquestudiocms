<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostStat.ascx.cs" Inherits="UniqueStudio.ComContent.PL.controls.PostStat" %>
<ul>
    <asp:Repeater ID="rptStatByYear" runat="server">
        <ItemTemplate>
            <li><a href='search.aspx?start=<%# Eval("Year") %>-1&end=<%# Convert.ToInt32(Eval("Year"))+1 %>-1'>
                <%# Eval("Year") %>年（<%# Eval("Count") %>）</a></li>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptStatByMonth" runat="server">
        <ItemTemplate>
            <li><a href='search.aspx?start=<%# Eval("Year") %>-<%# Eval("Month") %>&end=<%# Eval("Year") %>-<%# Convert.ToInt32(Eval("Month"))+1 %>'>
                <%# Eval("Year") %>年<%# Eval("Month") %>月（<%# Eval("Count") %>）</a></li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
