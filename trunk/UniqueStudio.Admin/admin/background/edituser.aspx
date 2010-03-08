<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="edituser.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.edituser" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>在该页面您可以为所选用户重新分配角色。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            编辑用户</div>
        <div class="panel_body">
            <table width="100%">
                <tr>
                    <td>
                        Email：<asp:Literal ID="ltlEmail" runat="server"/>
                    </td>
                    <td>
                        用户名：<asp:Literal ID="ltlUserName" runat="server"/>
                    </td>
                    <td>
                        创建时间：<asp:Literal ID="ltlCreateTime" runat="server"/>
                    </td>
                    <td>上次登录时间：<asp:Literal ID="ltlLastActivityDate" runat="server"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbIsApproved" Text="是否已激活" Enabled="false" runat="server"  /></td>
                    <td>
                        <asp:CheckBox ID="chbIsLockedOut"  Text="是否锁定" Enabled="false" runat="server" /></td>
                    <td>
                        <asp:CheckBox ID="chbIsOnline" Text="是否在线" Enabled="false" runat="server" /></td>
                     <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        所属角色：
                        <asp:CheckBoxList ID="cblRoles" runat="server" RepeatDirection="Horizontal" RepeatColumns="5">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        具有的权限：<br />
                        <asp:Repeater ID="rptPermissions" runat="server">
                            <ItemTemplate>
                                <span><%# Eval("PermissionName") %></span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" onclick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
