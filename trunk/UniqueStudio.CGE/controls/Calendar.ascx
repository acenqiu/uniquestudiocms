<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="UniqueStudio.CGE.controls.CalendarEditor" %>
<asp:Calendar ID="CalNotice" runat="server" BackColor="White" BorderColor="#999999"
    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
    ForeColor="Black" Height="189px" Width="241px" OnSelectionChanged="CalendarNotice_SelectionChanged"
    OnDayRender="CalNotice_DayRender">
    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
    <SelectorStyle BackColor="#CCCCCC" />
    <WeekendDayStyle BackColor="#FFFFCC" />
    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
    <OtherMonthDayStyle ForeColor="#808080" />
    <NextPrevStyle VerticalAlign="Bottom" ForeColor="White" />
    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
    <TitleStyle BackColor="#FF9900" BorderColor="Black" Font-Bold="True" ForeColor="White" />
</asp:Calendar>