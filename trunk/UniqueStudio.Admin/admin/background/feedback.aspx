<%@ Page MasterPageFile="background.Master" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeBehind="feedback.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.feedback" %>
<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="tip">
        <p>您可以发送反馈信息到如下邮箱，我们将及时给您回复，谢谢您的合作！</p>
        <p>support@uniquestudio.org</p>
        <p>或 support@hustunique.com</p>
    </div>
    <div class="panel" style="display:none">
        <div class="panel_title">
            发送反馈信息</div>
        <div class="panel_body">
            <p>
                您的邮箱（便于我们及时给您回复）：
                <asp:TextBox ID="txtEmail" runat="server" Width="287px" />
            <p>
                <FCKeditorV2:FCKeditor ID="fckContent" runat="server" ToolbarSet="Basic" />
            <asp:Button ID="btnSend" runat="server" Text="发送" onclick="btnSend_Click"/>
        </div>
    </div>
</asp:Content>
