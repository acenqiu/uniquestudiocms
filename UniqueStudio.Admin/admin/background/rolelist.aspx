<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="rolelist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.rolelist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            在“创建角色”部分您可以完成角色创建的工作，并且为其分配权限。</p>
        <p>
            “角色列表”中列出了当前系统中已经创建的所有角色，点击角色名可以对其进行设置； 勾选某几项角色，可对其进行批量操作。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            创建角色</div>
        <div class="panel_body">
            <p>
                角色名称：<asp:TextBox ID="txtRoleName" runat="server" />
                说明：<asp:TextBox ID="txtDescription" Width="400px" runat="server" /></p>
            <p>
                包含以下权限：
            </p>
            <p>
                <asp:CheckBoxList ID="cblPermissions" runat="server" RepeatDirection="Horizontal"
                    Width="100%" RepeatColumns="7">
                </asp:CheckBoxList>
            </p>
            <asp:Button ID="btnCreate" runat="server" Text="创建" OnClick="btnCreate_Click" />
        </div>
    </div>
    <%--<div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
            
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">
            角色列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td width="200px">
                                角色名称
                            </td>
                            <td>
                                说明
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("ID") %>' />
                        </td>
                        <td>
                            <a href='editrole.aspx?roleId=<%# Eval("ID") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'
                                title="单击编辑该角色">
                                <%# Eval("RoleName") %></a>
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
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Value="delete" Text="删除"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClientClick="return selectcheck('chkSelected');"
                    OnClick="btnExcute_Click" />
            </div>
        </div>
    </div>
</asp:Content>
