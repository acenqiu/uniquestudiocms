<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true" CodeBehind="editrole.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editrole" %>
<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>在此您可以查看角色的详细信息，并且可以为其重新分配权限，以及管理其中用户。</p>
        <p>注：超级管理员具有所有权限，在权限列表中并不会选中其权限。</p>
    </div>   
     <US:Message ID="message" runat="server" />
     <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="update"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">编辑角色</div>
        <div class="panel_body">
            <p>角色名称：<asp:TextBox ID="txtRoleName" runat="server"/>
                <asp:RequiredFieldValidator ID="require1" runat="server" ControlToValidate="txtRoleName"
                            ValidationGroup="update" Display="None" ErrorMessage="请输入角色名称" />
                 说明：<asp:TextBox ID="txtDescription" Width="300px" runat="server"/>
                 所属网站：<asp:DropDownList ID="ddlSites" runat="server">
                </asp:DropDownList>
                 </p>
            <p>
                权限列表：
                <asp:Repeater ID="rptPermissions" runat="server">
                    <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected_p')" id="chkSelectAll" />
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
                            <input type="checkbox" name='chkSelected_p' <asp:Literal ID="ltlSelected" runat="server"/> onchange='selectRow(this)' value='<%# Eval("PermissionId") %>' />
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
                <asp:Button ID="btnSave" runat="server" ValidationGroup="update" Text="保存" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="返回" onclick="btnCancel_Click" />
            </p>
        </div>
    </div>
    
    <div class="panel">
        <div class="panel_title">用户列表</div>
        <div class="panel_body">
         <asp:Repeater ID="rptUsers" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected_u')" id="chkSelectAll" />
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
                            <input type="checkbox" name='chkSelected_u' onchange='selectRow(this)' value='<%# Eval("UserId") %>' />
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
        </div>
    </div>
</asp:Content>
