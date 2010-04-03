<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="compenentlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.compenentlist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            “组件列表”中列出了当前系统中已经安装的所有组件。点击组件进行组件配置。</p>
        <p>
            卸载组件是一个比较耗时的操作，建议您一次仅卸载一个组件。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            组件列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>
                                名称
                            </td>
                            <td>
                                显示名
                            </td>
                            <td>
                                作者
                            </td>
                            <td>
                                说明
                            </td>
                            <td>
                                网站名称
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("CompenentId") %>' />
                        </td>
                        <td>
                            <a href='compenentconfig.aspx?comId=<%# Eval("CompenentId") %>' title="点击配置该组件" >
                            <%# Eval("CompenentName")%></a>
                        </td>
                        <td>
                            <%# Eval("DisplayName") %>
                        </td>
                        <td>
                            <%# Eval("CompenentAuthor")%>
                        </td>
                        <td>
                            <%# Eval("Description") %>
                        </td>
                        <td>
                            <%# Eval("SiteName") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Value="uninstall" Text="卸载" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
            </div>
        </div>
    </div>
</asp:Content>
