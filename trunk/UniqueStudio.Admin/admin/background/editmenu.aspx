<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="editmenu.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editmenu" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            编辑菜单</div>
        <div class="panel_body">
            <p>
                菜单名称：<asp:TextBox ID="txtMenuName" runat="server"></asp:TextBox>
                说明：<asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></p>
        </div>
    </div>
    
    <div class="panel">
        <div class="panel_title">
            新增菜单项</div>
        <div class="panel_body">
            <p>
                菜单项名称：<asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                链接：<asp:TextBox ID="txtLink" runat="server" />
                目标：<asp:TextBox ID="txtTarget" runat="server" />
                次序：<asp:TextBox ID="txtOrdering" runat="server" />
                父菜单项：<asp:DropDownList ID="ddlItems" runat="server">
                </asp:DropDownList>
                </p>
            <asp:Button ID="btnAdd" runat="server" Text="添加" onclick="btnAdd_Click" />
        </div>
    </div>
    
    <div class="panel">
        <div class="panel_title">
            编辑菜单项</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>
                                菜单项
                            </td>
                            <td>
                                链接
                            </td>
                            <td>目标</td>
                            <td>层次</td>
                            <td>次序</td>
                            <td>父菜单项</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("Id") %>' />
                        </td>
                        <td>
                                <%# Eval("ItemName") %></a>
                        </td>
                        <td>
                        <a href='<%# Eval("Link") %>' target="_blank">
                            <%# Eval("Link") %></a>
                        </td>
                        <td>
                            <%# Eval("Target") %>
                        </td>
                        <td>
                            <%# Eval("Depth") %>
                        </td>
                        <td>
                            <%# Eval("Ordering") %>
                        </td>
                        <td>
                            <%# Eval("ParentItemName") %>
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
