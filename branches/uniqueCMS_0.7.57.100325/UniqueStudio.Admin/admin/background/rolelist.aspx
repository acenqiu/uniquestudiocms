<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="rolelist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.rolelist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            “角色列表”中列出了当前系统中已经创建的所有角色，点击角色名可以对其进行设置； 勾选某几项角色，可对其进行批量操作。</p>
    </div>
    <US:Message ID="message" runat="server" />
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
                            <td width="200px">
                                所属网站
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("RoleID") %>' />
                        </td>
                        <td>
                            <a href='editrole.aspx?roleId=<%# Eval("RoleID") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'
                                title="单击编辑该角色">
                                <%# Eval("RoleName") %></a>
                        </td>
                        <td>
                            <%# Eval("Description") %>
                        </td>
                        <td>
                            <%# Eval("SiteName").ToString().Length!=0?Eval("SiteName"):"所有网站" %>
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
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click"
                OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？');" />
            </div>
        </div>
    </div>
</asp:Content>
