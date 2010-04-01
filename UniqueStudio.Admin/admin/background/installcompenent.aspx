<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="installcompenent.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.installcompenent" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            所有组件存放于：<i>{网站根目录}/admin/compenents/</i>，组件的所有文件在以组件名称命名的文件夹里。 安装前请确保install.xml文件的完整性。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            安装组件</div>
        <div class="panel_body">
            <p>
                相对路径：<i>{网站根目录}/admin/compenents/</i>
                <asp:DropDownList ID="ddlFolders" Width="200px" runat="server">
                </asp:DropDownList>
                应用于：
                <asp:DropDownList ID="ddlSites" runat="server">
                </asp:DropDownList>
            </p>
            <p>
                <asp:Button ID="btnInstall" runat="server" Text="安装" OnClick="btnInstall_Click" />
            </p>
        </div>
    </div>
    <div class="panel" id="pnlInfo" runat="server" visible="false">
        <div class="panel_title">
            组件信息</div>
        <div class="panel_body">
            <p>
                以下是您即将安装的组件信息，如果没有错误请点击“确认”继续安装， 否则点击“取消”终止安装该组件，并检查组件目录下的组件信息。</p>
            <p>
                名称:
                <asp:Literal ID="ltlCompenentName" runat="server" /></p>
            <p>
                后台显示名称:
                <asp:Literal ID="ltlDisplayName" runat="server" /></p>
            <p>
                作者:
                <asp:Literal ID="ltlCompenentAuthor" runat="server" /></p>
            <p>
                说明:
                <asp:Literal ID="ltlDescription" runat="server" /></p>
            <p>
                程序集:
                <asp:Literal ID="ltlAssembly" runat="server" /></p>
            <p>
                类名:
                <asp:Literal ID="ltlClassPath" runat="server" /></p>
            <p>
                <asp:Button ID="btnOk" runat="server" Text="确认" OnClick="btnOk_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
            </p>
        </div>
    </div>
</asp:Content>
