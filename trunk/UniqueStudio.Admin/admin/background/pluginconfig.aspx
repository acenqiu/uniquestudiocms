<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="pluginconfig.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pluginconfig" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/Config.ascx" TagPrefix="US" TagName="Config" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            插件信息</div>
        <div class="panel_body">
            <table width="100%">
                <tr>
                    <td>
                        插件名称：<asp:Literal ID="ltlPlugInName" runat="server" />
                    </td>
                    <td>
                        插件名称：<asp:Literal ID="ltlDisplayName" runat="server" />
                    </td>
                    <td>
                        作者：<asp:Literal ID="ltlAuthor" runat="server" />
                    </td>
                    <td>
                        分类：<asp:Literal ID="ltlCategory" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        说明：<asp:Literal ID="ltlDescription" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            实例列表</div>
        <div class="panel_body">
            <div>
                新增实例到：（网站）
                <asp:DropDownList ID="ddlSites" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnAdd" runat="server" Text="增加" OnClick="btnAdd_Click" />
            </div>
            <div>
                <asp:Repeater ID="rptList" runat="server">
                    <HeaderTemplate>
                        <table class="panel_table">
                            <tr class="panel_table_head">
                                <td width="10px">
                                    <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                                </td>
                                <td width="200px">
                                    网站名称
                                </td>
                                <td>
                                    是否启用
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("InstanceId") %>' />
                            </td>
                            <td>
                                <a href='pluginconfig.aspx?pluginId=<% = PlugInId %>&instanceId=<%# Eval("InstanceId") %>' title="点击配置该插件">
                                    <%# Eval("SiteName")%></a>
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
                    <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？');" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            插件配置：<asp:Literal ID="ltlCurrentInstance" runat="server" /></div>
        <div class="panel_body">
            <div>
                <US:Config ID="config" runat="server" />
                <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="返回" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
