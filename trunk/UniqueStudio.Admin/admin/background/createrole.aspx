<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true" CodeBehind="createrole.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.createrole" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            在此您可以完成角色创建的工作，并且为其分配权限。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建角色</div>
        <div class="panel_body">
            <p>
                角色名称：<asp:TextBox ID="txtRoleName" runat="server" />
                <asp:RequiredFieldValidator ID="require1" runat="server" ControlToValidate="txtRoleName"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入角色名称" />
                说明：<asp:TextBox ID="txtDescription" Width="400px" runat="server" />
                所属网站：
                <asp:DropDownList ID="ddlSites" runat="server">
                </asp:DropDownList>
                </p>
            <p>
                包含以下权限：
            </p>
            <p>
                <asp:Repeater ID="rptPermissions" runat="server">
                    <HeaderTemplate>
                        <table class="panel_table">
                            <tr class="panel_table_head">
                                <td width="10px">
                                    <input type="checkbox" onchange="selectall(this,'chkSelected_p')" id="chkSelectAll" />
                                </td>
                                <td>
                                    权限名称
                                </td>
                                <td>
                                    说明
                                </td>
                                <td>
                                    权限提供者
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" name='chkSelected_p' onchange='selectRow(this)' value='<%# Eval("PermissionId") %>' />
                            </td>
                            <td>
                                <%# Eval("PermissionName")%>
                            </td>
                            <td>
                                <%# Eval("Description")%>
                            </td>
                            <td>
                                <%# Eval("Provider")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </p>
            <asp:Button ID="btnCreate" runat="server" ValidationGroup="create" Text="创建" OnClick="btnCreate_Click" />
        </div>
    </div>
</asp:Content>
