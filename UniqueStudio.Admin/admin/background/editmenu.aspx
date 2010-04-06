<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="editmenu.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.editmenu" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        "目标"项表示点击链接后浏览器的行为：
        <ul>
            <li>空白: 在当前窗口后标签页中打开；</li>
            <li>_blank: 在新窗口后新标签中打开；</li>
            <li>_top: 在不包含框架的当前窗口或标签。</li>
        </ul>
    </div>
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="modify"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <asp:ValidationSummary ID="validationSummary1" CssClass="error" ValidationGroup="add"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            编辑菜单</div>
        <div class="panel_body">
            <p>
                菜单名称：<asp:TextBox ID="txtMenuName" runat="server" />
                <asp:RequiredFieldValidator ID="requireMenuName" runat="server" ControlToValidate="txtMenuName"
                    ValidationGroup="modify" Display="None" ErrorMessage="请输入菜单名称" />
                <asp:HiddenField ID="hfOldMenuName" runat="server" />   
                说明：<asp:TextBox ID="txtDescription" runat="server" /></p>
            <asp:Button ID="btnSave" runat="server" ValidationGroup="modify" Text="保存" OnClick="btnSave_Click" />
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            新增菜单项</div>
        <div class="panel_body">
            <p>
                菜单项名称：<asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemName"
                    ValidationGroup="add" Display="None" ErrorMessage="请输入菜单项名称" />
                链接：<asp:TextBox ID="txtLink" runat="server" />
                目标：<asp:TextBox ID="txtTarget" runat="server" />
                次序：<asp:TextBox ID="txtOrdering" runat="server" />
                父菜单项：<asp:DropDownList ID="ddlItems" runat="server">
                </asp:DropDownList>
            </p>
            <asp:Button ID="btnAdd" runat="server" ValidationGroup="add" Text="添加" OnClick="btnAdd_Click" />
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
                                链接（如果是相对链接，点击后可能转向错误的页面）
                            </td>
                            <td>
                                目标
                            </td>
                            <td>
                                层次
                            </td>
                            <td>
                                次序
                            </td>
                            <td>
                                父菜单项
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" id='chk_<%# Eval("Id") %>' name='chkSelected' onchange="selectRow(this);selectChildren(this,'<%= chkSelectChildren.ClientID %>')"
                                value='<%# Eval("Id") %>' />
                            <input type="hidden" name='<%# Eval("ParentItemId") %>' value='chk_<%# Eval("Id") %>' />
                        </td>
                        <td>
                            <a href='editmenuitem.aspx?siteId=<%=SiteId %>&menuId=<%# Eval("MenuId") %>&itemId=<%# Eval("Id") %>'
                                    title="单击编辑该菜单项">
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
                            <%# Eval("ParentItemName").ToString().Length == 0 ? "无" : Eval("ParentItemName")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                <asp:CheckBox ID="chkSelectChildren" runat="server" Checked="true" />同时选中子菜单 批量操作：<asp:DropDownList
                    ID="ddlOperation" runat="server">
                    <asp:ListItem Value="delete" Text="删除"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" 
                OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
            </div>
        </div>
    </div>
</asp:Content>
