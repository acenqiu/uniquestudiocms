<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="editcontrol.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editcontrol" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="~/admin/controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            编辑控件：<asp:Literal ID="ltlControlId" runat="server" /></div>
        <div class="panel_body">
            <div>
                <p>模块名：<asp:Literal ID="ltlModuleName" runat="server"/></p>
                <p>切换状态：<asp:Button ID="btnEnable" runat="server" Text="启用" /></p>
            </div>
            <div>
                <US:Config ID="config" runat="server" />
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="返回" />
            </div>
        </div>
    </div>
</asp:Content>
