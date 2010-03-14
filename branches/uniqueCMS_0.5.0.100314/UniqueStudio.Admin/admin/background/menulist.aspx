<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="menulist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.menulist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建菜单</div>
        <div class="panel_body">
            <p>
                菜单名称：<asp:TextBox ID="txtMenuName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireMenuName" runat="server" ControlToValidate="txtMenuName"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入菜单名称" />
                说明：<asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></p>
            <asp:Button ID="btnCreate" runat="server" ValidationGroup="create" Text="创建" OnClick="btnCreate_Click" />
        </div>
    </div>

    <div class="panel">
        <div class="panel_title">
            菜单列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>
                                菜单名称
                            </td>
                            <td>
                                说明
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("MenuId") %>' />
                        </td>
                        <td>
                            <a href='editmenu.aspx?menuId=<%# Eval("MenuId") %>' title="单击编辑该菜单">
                                <%# Eval("MenuName") %></a>
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
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClientClick="return selectcheck('chkSelected');" OnClick="btnExcute_Click" />
            </div>
        </div>
    </div>
</asp:Content>
