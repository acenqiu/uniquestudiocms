<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="installModule.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.installModule" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>

<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
<US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">安装模块</div>
        <div class="panel_body">
            <p>相对路径：
                <asp:TextBox ID="txtPath" runat="server" Width="400px" /></p>
            <p>
                <asp:Button ID="btnInstall" runat="server" Text="安装" 
                    onclick="btnInstall_Click" />
                </p>
        </div>
    </div> 
    <div class="panel" id="pnlInfo" runat="server" visible="false">
        <div class="panel_title">模块信息</div>
        <div class="panel_body">
            <p>以下是您即将安装的模块信息，如果没有错误请点击“确认”继续安装，
            否则点击“取消”终止安装该模块，并检查插件目录下的模块信息。</p>
            <p>名称:
                <asp:Literal ID="ltlModuleName" runat="server"/></p>
            <p>后台显示名称:
                <asp:Literal ID="ltlDisplayName" runat="server"/></p>  
             <p>作者:
                <asp:Literal ID="ltlModuleAuthor" runat="server"/></p>
              <p>说明:
                <asp:Literal ID="ltlDescription" runat="server"/></p>
             <p>
                 <asp:Button ID="btnOk" runat="server" Text="确认" onclick="btnOk_Click" />
                 <asp:Button ID="btnCancel" runat="server" Text="取消" onclick="btnCancel_Click" />
             </p>
        </div>
    </div>
</asp:Content>

