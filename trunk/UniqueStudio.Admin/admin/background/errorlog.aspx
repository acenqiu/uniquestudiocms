<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="errorlog.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.errorlog" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
    
	if(window.jQuery){jQuery(function(){
		(function(){ var target = jQuery('input#ctl00_cphBody_txtDate'); target.datepicker({dateFormat:'yy-mm-dd',dayNamesMin:['日', '一', '二', '三', '四', '五', '六'],dayNamesShort:['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],monthNames:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],monthNamesShort:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],showButtonPanel: true,currentText: '本月',closeText: '关闭'}); })();
	})};
	
    </script>

</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            系统日志文件位于：<i>{网站根目录}\admin\xml\log\{日期}.xml</i>。您可以定期将其发送给我们，便于我们了解系统的运行状况。</p>
    </div>
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="view"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
            选择日志文件：<asp:DropDownList ID="ddlLogs" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlLogs_SelectedIndexChanged">
            </asp:DropDownList>
            按日期查看：<asp:TextBox ID="txtDate" runat="server" />
            <asp:RequiredFieldValidator ID="requireEmail" runat="server" ControlToValidate="txtDate"
                ValidationGroup="view" Display="None" ErrorMessage="请输入日期" />
            <asp:Button ID="btnView" runat="server" Text="查看" ValidationGroup="view" OnClick="btnView_Click" />
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            日志列表:<asp:Literal ID="ltlFileName" runat="server" /></div>
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
