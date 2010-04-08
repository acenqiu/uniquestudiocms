<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="userinfo.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.userinfo" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            个人信息</div>
        <div class="panel_body">
            <div class='form-item'>
                <span class='form-item-label'>本用户使用的文章作者：</span><span class='form-item-input'><span>
                    <asp:TextBox ID="txtPenName" runat="server" /></span></span>
                    
                  <div style="clear:both"></div>
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>
