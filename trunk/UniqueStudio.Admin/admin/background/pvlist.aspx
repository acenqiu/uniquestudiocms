<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="pvlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pvlist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="~/admin/controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <%--<div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">
            PV统计</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td>
                                访问页面
                            </td>
                            <td>
                                客户端IP地址
                            </td>
                            <td>
                                客户端主机名
                            </td>
                            <td width="300px">
                                用户代理
                            </td>
                            <td>
                                UrlReferrer
                            </td>
                            <td width="110px">
                                访问时间
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("RawUrl") %>
                        </td>
                        <td>
                            <%# Eval("UserHostAddress") %>
                        </td>
                        <td>
                            <%# Eval("UserHostName") %>
                        </td>
                        <td>
                            <%# Eval("UserAgent") %>
                        </td>
                        <td>
                            <%# Eval("UrlReferrer") %>
                        </td>
                        <td>
                            <%# Eval("Time") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                <US:Pagination ID="pagination" Url="pvlist.aspx?page={0}" NumberOfShow="14" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
