<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="errorlog.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.errorlog" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="~/admin/controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>系统日志文件位于：<i>{网站根目录}\admin\xml\log\{日期}.xml</i>。您可以定期将其发送给我们，便于我们了解系统的运行状况。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            日志列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td>
                                异常类型
                            </td>
                            <td>
                                异常信息
                            </td>
                            <td width="110px">
                                抛出时间
                            </td>
                            <td>
                                备注
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("ExceptionType") %>
                        </td>
                        <td>
                            <%# Eval("ErrorMessage") %>
                        </td>
                        <td>
                            <%# Eval("Time") %>
                        </td>
                        <td>
                            <%# Eval("Remarks") %>
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
