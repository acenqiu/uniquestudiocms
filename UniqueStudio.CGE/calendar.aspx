<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendar.aspx.cs" MasterPageFile="Site.Master"
    Inherits="UniqueStudio.CGE.calendar" %>

<%@ Register Src="controls/Calendar.ascx" TagPrefix="US" TagName="Calendar" %>
<asp:Content ID="calendarHead" runat="server" ContentPlaceHolderID="head">
<style>
.column td a
{
	 display:block;
	 height:100%;
	 width:100%;
}
</style>
<link rel="Stylesheet" href="css/calendar.css" />
</asp:Content>
<asp:Content ID="calendar" runat="server" ContentPlaceHolderID="cphMain">
    <div class="index-slider">
        <div class="column mini" style="line-height: normal">
            <US:Calendar ID="showcalendar2" runat="server" />
        </div>
    </div>
    <div class="index-main-content">
    <div  class="calendarListWrap">
    <h2 class="calendarList-title"><asp:Literal ID="calendarDate" runat="server"></asp:Literal>日程安排</h2>
        <asp:Literal ID="calendarNotice" runat="server"></asp:Literal>
        </div></div>
</asp:Content>
