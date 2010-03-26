<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="pluginlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pluginlist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <%--<div class="panel">
        <div class="panel_title">筛选</div>
        <div class="panel_body">
            
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">
            插件列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td width="200px">
                                名称
                            </td>
                            <td width="200px">
                                显示名
                            </td>
                            <td width="200px">
                                作者
                            </td>
                            <td>
                                说明
                            </td>
                            <td>
                                插件分类
                            </td>
                            <td>
                                顺序
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("PlugInId") %>' />
                        </td>
                        <td>
                            <a href='pluginconfig.aspx?pluginId=<%# Eval("PlugInId") %>' title="点击配置该插件">
                            <%# Eval("PlugInName") %></a>
                        </td>
                        <td>
                            <%# Eval("DisplayName") %>
                        </td>
                        <td>
                            <%# Eval("PlugInAuthor") %>
                        </td>
                        <td>
                            <%# Eval("Description") %>
                        </td>
                        <td>
                            <%# Eval("PlugInCategory") %>
                        </td>
                        <td>
                            <%# Eval("PlugInOrdering") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem  Value="uninstall" Text="卸载" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" 
                OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
            </div>
        </div>
    </div>
</asp:Content>
