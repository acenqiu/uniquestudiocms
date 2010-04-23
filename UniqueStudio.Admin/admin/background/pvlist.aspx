<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="pvlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pvlist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
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
                            <td width="300px">
                                用户代理
                            </td>
                            <td>
                                UrlReferrer
                            </td>
                            <td width="130px">
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
                            <a href='http://open.baidu.com/ipsearch/s?wd=<%# Eval("UserHostAddress") %>&tn=baiduip' target="_blank" 
                            title="查看该IP来源"><%# Eval("UserHostAddress")%></a>
                        </td>
                        <td>
                            <%# Eval("UserAgent")%>
                        </td>
                        <td>
                            <%# Eval("UrlReferrer")%>
                        </td>
                        <td>
                            <%# Eval("Time")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                <US:Pagination ID="pagination" Url="pvlist.aspx?page={0}" NumberOfShow="15" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
