<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="pagination.ascx.cs" Inherits="UniqueStudio.ComContent.PL.pagination" %>
<style type="text/css">
.pagination a, .pagination  strong
{
	border:solid 1px #cccccc;
	text-decoration:none;
	margin:0px;
	width:12px;
	line-height:26px;
	height:26px;
	padding:3px 7px;
	font:12px/1.6em Verdana,Helvetica,Arial,sans-serif;
	color:#3366CC;
}
.pagination strong
{
	margin-right:6px;
	background:#3366CC;
	color:White;

}
.pageIndex
{
	display:none;
}
</style>
<asp:Literal ID="htmlContent" runat="server"></asp:Literal>
<span class="pageIndex"><asp:TextBox ID="pageIndexTextBox" runat="server" Width="37px"></asp:TextBox>
<asp:Button ID="gotoPageBtn" runat="server" onclick="gotoPageBtn_Click" 
    Text="前往" /></span>
