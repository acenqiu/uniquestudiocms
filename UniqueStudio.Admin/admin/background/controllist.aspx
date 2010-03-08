<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="controllist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.controllist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="error">当前该功能处于不可用状态。</div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            创建控件</div>
        <div class="panel_body">
            <p>
                控件ID：<asp:TextBox ID="txtControlId" runat="server" />
                模块名：<asp:DropDownList ID="ddlModules" runat="server">
                </asp:DropDownList>
                是否启用：<asp:CheckBox ID="chkIsEnabled" Checked="true" runat="server" />
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
                                控件ID
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
                            <a href='editcontrol.aspx?id=<%# Eval("ControlId") %>'>
                                <%# Eval("ControlId") %></a>
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
            </div>
        </div>
    </div>
</asp:Content>
