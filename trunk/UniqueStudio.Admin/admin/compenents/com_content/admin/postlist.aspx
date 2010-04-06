<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="postlist.aspx.cs"
    Inherits="UniqueStudio.ComContent.Admin.postlist" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="UniqueStudio.ComContent.Model" %>
<%@ Import Namespace="UniqueStudio.Common.Config" %>
<%@ Import Namespace="UniqueStudio.Core.Site" %>
<%@ Register Src="controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
	if(window.jQuery){jQuery(function(){
		(function(){ var target = jQuery('input#ctl00_cphBody_txtEditTo'); target.datepicker({dateFormat:'yy-mm-dd',dayNamesMin:['日', '一', '二', '三', '四', '五', '六'],dayNamesShort:['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],monthNames:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],monthNamesShort:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],showButtonPanel: true,currentText: '本月',closeText: '关闭'}); })();
		(function(){ var target = jQuery('input#ctl00_cphBody_txtCreateFrom'); target.datepicker({dateFormat:'yy-mm-dd',dayNamesMin:['日', '一', '二', '三', '四', '五', '六'],dayNamesShort:['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],monthNames:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],monthNamesShort:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],showButtonPanel: true,currentText: '本月',closeText: '关闭'}); })();
		(function(){ var target = jQuery('input#ctl00_cphBody_txtCreateTo'); target.datepicker({dateFormat:'yy-mm-dd',dayNamesMin:['日', '一', '二', '三', '四', '五', '六'],dayNamesShort:['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],monthNames:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],monthNamesShort:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],showButtonPanel: true,currentText: '本月',closeText: '关闭'}); })();
		(function(){ var target = jQuery('input#ctl00_cphBody_txtEditFrom'); target.datepicker({dateFormat:'yy-mm-dd',dayNamesMin:['日', '一', '二', '三', '四', '五', '六'],dayNamesShort:['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],monthNames:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],monthNamesShort:['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],showButtonPanel: true,currentText: '本月',closeText: '关闭'}); })();
	})};

    function hoverItem(sender)
    {
      var menu=sender.getElementsByTagName("div")[0];
      menu.style.display="block";
    }
    function outItem(sender)
    {
      var menu=sender.getElementsByTagName("div")[0];
      menu.style.display="none";
    }
    </script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <div class="write-article">
        <div class="column-head">
            文章管理</div>
        <div class="tip">
           <p>通过筛选功能您可以更方便地找到您要的文章，对于不需要进行筛选的条件请留空。</p>
           <p>仅输入日期时，默认的时间是当日的0:00:00。</p>
        </div>
        <US:Message ID="message" runat="server" />
        <div class="list-manager">
            <div>
                <p>
                    创建时间从<asp:TextBox ID="txtCreateFrom" runat="server" />
                    到<asp:TextBox ID="txtCreateTo" runat="server" />
                    修改时间从<asp:TextBox ID="txtEditFrom" runat="server" />
                    到<asp:TextBox ID="txtEditTo" runat="server" /></p>
                <p>
                    分类：<asp:DropDownList ID="ddlCategories" runat="server">
                    </asp:DropDownList>
                    文章类型：
                    <asp:DropDownList ID="ddlPostType" runat="server">
                        <asp:ListItem Selected="True" Value="Both">所有</asp:ListItem>
                        <asp:ListItem Value="PublishedOnly">已发布</asp:ListItem>
                        <asp:ListItem Value="DraftOnly">未发布</asp:ListItem>
                    </asp:DropDownList>
                    文章标题关键词：<asp:TextBox ID="txtKeyWord" runat="server" />
                    <asp:Button ID="txtSearch" runat="server" Text="筛选" OnClick="txtSearch_Click" /></p>
            </div>
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>标题</td>
                            <td width="40px">操作</td>
                            <td width="70px">作者</td>
                            <td>分类</td>
                            <td width="70px">添加用户</td>
                            <td width="100px">添加时间</td>
                            <td width="80px">最后修改用户</td>
                            <td width="100px">最后修改时间</td>
                            <td width="50px">已发布</td>
                            <td width="50px">阅读次数</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("Uri") %>' />
                        </td>
                        <td>
                            <a href='<%= SiteManager.BaseAddress(SiteId) %>/view.aspx?uri=<%# Eval("Uri")%>'
                                target="_blank" title='在新窗口/标签页中查看'><%# Eval("Title") %></a>
                        </td>
                        <td align="center">
                            <a href='editpost.aspx?siteId=<%=SiteId %>&uri=<%# Eval("Uri") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'
                             title="编辑"><img src="images/edit.png" alt="编辑" /></a>
                             <a href='deletepost.aspx?uri=<%# Eval("Uri") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'
                             title="删除"><img src="images/delete.png" alt="删除" /></a></span>
                         </td>
                        <td align="center"><%# Eval("Author") %></td>
                        <td><%# ((PostInfo)Container.DataItem).Categories%></td>
                        <td align="center"><%# Eval("AddUserName") %></td>
                        <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd  HH:mm") %></td>
                        <td align="center"><%# Eval("LastEditUserName") %></td>
                        <td><%# Convert.ToDateTime(Eval("LastEditDate")).ToString("yyyy-MM-dd  HH:mm") %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsPublished"))?"是":"否" %></td>
                        <td align="center"><%# Eval("Count") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Selected="True" Value="delete" Text="删除" />
                    <asp:ListItem Value="publish">发布</asp:ListItem>
                    <asp:ListItem Value="stoppublish">停止发布</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click" 
                        OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
                <%--<br />
                <asp:DropDownList ID="ddlPageSize" AutoPostBack="true" runat="server">
                    <asp:ListItem Value="10">每页10条</asp:ListItem>
                    <asp:ListItem Value="15" Selected="True">每页15条</asp:ListItem>
                    <asp:ListItem Value="20">每页20条</asp:ListItem>
                    <asp:ListItem Value="25">每页25条</asp:ListItem>
                </asp:DropDownList>--%>
                <US:Pagination ID="pagination" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
