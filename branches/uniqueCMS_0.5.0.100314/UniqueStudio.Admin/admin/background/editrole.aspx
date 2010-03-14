<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="editrole.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editrole" %>
<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="error">
        当前该功能处于不可用状态，完成后请及时移除该信息。
    </div>
    <div class="tip">
        <p>在此您可以查看角色的详细信息，并且可以为其重新分配权限，以及管理其中用户。</p>
    </div>
    <div class="panel">
        <div class="panel_title">编辑角色：<asp:Literal ID="ltlRoleName" runat="server"></asp:Literal></div>
        <div class="panel_body">
            <p>角色名称：<asp:TextBox ID="txtRoleName" runat="server"/>
                 说明：<asp:TextBox ID="txtDescription" Width="300px" runat="server"/></p>
            <p>
                权限列表：
                <asp:Repeater ID="rptPermissions" runat="server">
                    <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected_permission')" id="chkSelectAll" />
                            </td>
                            <td width="200px">
                                权限名称
                            </td>
                            <td>
                                说明
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected_permission' onchange='selectRow(this)' value='<%# Eval("PermissionName") %>' />
                        </td>
                        <td>
                            <%# Eval("PermissionName") %>
                        </td>
                        <td>
                            <%# Eval("Description") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
                </asp:Repeater>
            </p>
            <p>
            用户列表：
                <asp:Repeater ID="rptUsers" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected_user')" id="chkSelectAll" />
                            </td>
                            <td>
                                Email
                            </td>
                            <td>
                               用户名
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected_user' onchange='selectRow(this)' value='<%# Eval("UserId") %>' />
                        </td>
                        <td>
                            <%# Eval("Email") %>
                        </td>
                        <td>
                            <%# Eval("UserName") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
                </asp:Repeater>
            </p>
            <p>
                <asp:Button ID="btnSave" runat="server" Text="保存" />
                <asp:Button ID="btnCancel" runat="server" Text="返回" onclick="btnCancel_Click" />
            </p>
        </div>
    </div>
</asp:Content>
