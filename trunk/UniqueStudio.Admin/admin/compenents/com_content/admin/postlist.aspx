<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="postlist.aspx.cs"
    Inherits="UniqueStudio.ComContent.Admin.postlist" %>

<%@ Import Namespace="System.Web" %>
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
        <div class="list-manager">
            <US:Message ID="message" runat="server" />
            <div>
                分类：<asp:DropDownList ID="ddlCategories" runat="server">
                </asp:DropDownList>
                创建时间从<asp:TextBox ID="txtCreateFrom" runat="server" />
                到<asp:TextBox ID="txtCreateTo" runat="server" />
                修改时间从<asp:TextBox ID="txtEditFrom" runat="server" />
                到<asp:TextBox ID="txtEditTo" runat="server" />
                标题关键词：<asp:TextBox ID="txtKeyWord" runat="server" />
                <asp:Button ID="txtSearch" runat="server" Text="搜索" OnClick="txtSearch_Click" />
            </div>
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <div class="list-header">
                        <span>
                            <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                        </span><span class="article-title">标题</span> <span class="article-author">作者</span>
                        <span class="article-category">分类</span> <span class="article-views">阅读次数</span>
                        <span class="article-date">发表时间</span>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="list-item" onmouseover="hoverItem(this)" onmouseout="outItem(this)">
                        <span>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("Uri") %>' /></span>
                        <span class="article-title"><a href='<%= SiteManager.BaseAddress(SiteId) %>/view.aspx?uri=<%# Eval("Uri")%>'
                            target="_blank" title='查看  <%# Eval("Title") %>'>
                            <%# Eval("Title") %></a></span>
                        <span class="article-author">
                                <%# Eval("Author") %></span> 
                       <span class="article-category">
                                    <%# ((UniqueStudio.ComContent.Model.PostInfo)Container.DataItem).Categories%></span>
                        <span class="article-views">
                            <%# Eval("Count") %></span>
                        <span class="article-date">
                                <%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></span>
                        <div class="article-do-menu">
                            <span class="article-edit"><a href='editpost.aspx?siteId=<%=SiteId %>&uri=<%# Eval("Uri") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'>
                                编辑</a></span> <span class="article-views"><a href='<%= SiteManager.BaseAddress(SiteId) %>/view.aspx?uri=<%# Eval("Uri")%>'
                                    target="_blank" title='<%# Eval("Title") %>'>查看</a></span> <span class="article-delete">
                                        <a href='deletepost.aspx?uri=<%# Eval("Uri") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'>
                                            删除</a></span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Selected="True" Value="delete" Text="删除" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click"
                 OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
                <US:Pagination ID="pagination" runat="server" />
            </div>
            <div>
            </div>
        </div>
    </div>
</asp:Content>
