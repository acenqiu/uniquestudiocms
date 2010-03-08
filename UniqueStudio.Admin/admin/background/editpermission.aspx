<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="editpermission.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editpermission" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="panel">
        <div class="panel_title">
            编辑权限：<asp:Literal ID="ltlPermissionName" runat="server" />
        </div>
        <div class="panel_body">
            <p>
                权限名称：<asp:TextBox ID="txtPermissionName" runat="server"/>
                说明：<asp:TextBox ID="txtDescription" runat="server"/>
                权限提供者：<asp:TextBox ID="txtProvider" runat="server"/>
            </p>
            <p>
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="返回" onclick="btnCancel_Click" />
            </p>
        </div>
    </div>
</asp:Content>