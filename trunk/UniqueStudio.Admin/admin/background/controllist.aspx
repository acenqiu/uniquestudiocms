<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="controllist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.controllist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">

    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建控件</div>
        <div class="panel_body">
            <p>
                控件名称：<asp:TextBox ID="txtControlName" runat="server" />
                <asp:RequiredFieldValidator ID="require1" runat="server" ControlToValidate="txtControlName"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入控件名称" />
                模块名：<asp:DropDownList ID="ddlModules" runat="server">
                </asp:DropDownList>
                是否启用：<asp:CheckBox ID="chkIsEnabled" Checked="true" runat="server" />
            </p>
            <asp:Button ID="btnCreate" runat="server" ValidationGroup="create" Text="创建" OnClick="btnCreate_Click" />
        </div>
    </div>
    
    <div class="panel">
        <div class="panel_title">
            控件列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>
                                控件名称
                            </td>
                            <td>
                                模块名
                            </td>
                            <td>
                                是否启用
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("ControlId") %>' />
                        </td>
                        <td>
                            <a href='controlconfig.aspx?siteId=<%=SiteId %>&controlId=<%# Eval("ControlId") %>'
                                title="单击编辑该控件">
                                <%# Eval("ControlName") %></a>
                        </td>
                        <td>
                            <%# Eval("ModuleName")%>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsEnabled" Enabled="false" Checked='<%# Eval("IsEnabled") %>'
                                runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
            批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                        <asp:ListItem Value="start" Text="启用" />
                        <asp:ListItem Value="stop" Text="停用" />
                        <asp:ListItem Value="delete" Text="删除" />
                    </asp:DropDownList>
                    <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" 
                    OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
            </div>
        </div>
    </div>
</asp:Content>
