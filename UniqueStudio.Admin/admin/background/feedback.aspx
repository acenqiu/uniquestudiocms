<%@ Page MasterPageFile="background.Master" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeBehind="feedback.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.feedback" %>
<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="tip">
        <p>您可以发送反馈信息到如下邮箱，我们将及时给您回复，谢谢您的合作！</p>
        <p>support@uniquestudio.org</p>
        <p>或 support@hustunique.com</p>
    </div>
</asp:Content>
