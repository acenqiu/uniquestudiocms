<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true" CodeBehind="pluginlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pluginlist" %>
<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>

<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
<US:Message ID="message" runat="server" />
    <%--<div class="panel">
        <div class="panel_title">筛选</div>
        <div class="panel_body">
            
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">插件列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>名称</td>
                            <td>显示名</td>
                            <td>作者</td>
                            <td>说明</td>
                            <td>启用</td>
                            <td>程序集</td>
                            <td>插件分类</td>
                            <td>顺序</td>
                            <td width="250px">安装文件路径</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("PlugInId") %>' /></td>
                        <td><%# Eval("PlugInName") %></td>
                        <td><%# Eval("DisplayName") %></td>
                        <td><%# Eval("PlugInAuthor") %></td>
                        <td><%# Eval("Description") %></td>
                        <td><asp:CheckBox ID="chbIsEnabled" Enabled="false" Checked='<%# Eval("IsEnabled") %>' runat="server" /></td>
                        <td><%# Eval("Assembly") %></td>
                        <td><%# Eval("PlugInCategory") %></td>
                        <td><%# Eval("PlugInOrdering") %></td>
                        <td><%# Eval("InstallFilePath") %></td>
                    </tr>
                    
                </ItemTemplate>
                <FooterTemplate>
                  </table>
                </FooterTemplate>
            </asp:Repeater>
             
            <div>
            </div>
        </div>
    </div>
</asp:Content>
