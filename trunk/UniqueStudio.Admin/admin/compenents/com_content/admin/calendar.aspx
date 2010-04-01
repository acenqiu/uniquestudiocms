<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="calendar.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.admin.calendar" %>

<%@ Register Src="controls/Calendar.ascx" TagPrefix="US" TagName="Calendar" %>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="cphBody">
    <US:Calendar ID="calendar1" runat="server" />
    <br />
    <iframe src="calendarnotice.aspx" id="ifrcalnotice"></iframe>
</asp:Content>
