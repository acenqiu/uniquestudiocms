<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="calendar.aspx.cs"
    Inherits="UniqueStudio.ComCalendar.Admin.calendar" %>

<%@ Register Src="controls/Calendar.ascx" TagPrefix="US" TagName="Calendar" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        #ifrcalnotice
        {
            height: 100%;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphBody">
    <US:Calendar ID="calendar1" runat="server" />
    <br />
    <iframe src="calendarnotice.aspx" id="ifrcalnotice"></iframe>
</asp:Content>
