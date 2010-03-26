<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="postlist.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.postlist" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="UniqueStudio.Common.Config" %>
<%@ Import Namespace="UniqueStudio.Core.Site" %>
<%@ Register Src="controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ID="content" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />

    <script language="javascript">
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

    <div class="write-article">
        <div class="column-head">
            文章管理</div>
        <div class="list-manager">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <div class="list-header">
                        <span>
                            <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                        </span><span class="article-title">文章题目</span> <span class="article-author">作者</span>
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
                            <%# Eval("Title") %></a></span> <span class="article-author">
                                <%# Eval("Author") %></span> <span class="article-category">
                                    <%# ((UniqueStudio.ComContent.Model.PostInfo)Container.DataItem).Categories%></span>
                        <span class="article-views">
                            <%# Eval("Count") %></span> <span class="article-date">
                                <%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></span>
                        <div class="article-do-menu">
                            <span class="article-edit"><a href='editpost.aspx?siteId=<%=SiteId %>&uri=<%# Eval("Uri") %>'>编辑</a></span>
                            <span class="article-views"><a href='<%= SiteManager.BaseAddress(SiteId) %>/view.aspx?uri=<%# Eval("Uri")%>'
                                target="_blank" title='<%# Eval("Title") %>'>查看</a></span> <span class="article-delete">
                                    <a href='deletepost.aspx?uriCollection=<%# Eval("Uri") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'>
                                        删除</a></span>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <div class="list-footer">
                        <span>
                            <input name="checkbox" type="checkbox" /></span> <span class="article-title">文章题目</span>
                        <span class="article-author">作者</span> <span class="article-category">分类</span>
                        <span class="">阅读次数</span> <span class="article-date">发表时间</span>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                <%--批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Selected="True" Value="delete" Text="删除" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClientClick="return selectcheck('chkSelected');" OnClick="btnExcute_Click" />--%>
                <US:Pagination ID="pagination" runat="server" Url="postlist.aspx?page={0}" />
            </div>
            <div>
            </div>
        </div>
    </div>
</asp:Content>
