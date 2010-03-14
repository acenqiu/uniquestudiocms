<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="UniqueStudio.ComContent.PL.Default" %>

<%@ Import Namespace="UniqueStudio.Common.Config" %>
<%@ Register Src="~/controls/PostList.ascx" TagPrefix="US" TagName="PostList" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">AC_FL_RunContent = 0;</script>

    <script src="flashad/js/AC_RunActiveContent.js" language="javascript"></script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
    <div class="slider">
        <div class="column mini">
            <US:PostList ID="pltongzhi" CategoryId="6" runat="server" />
        </div>
        <div class="column mini">
            <div class="column-head">
                站内搜索</div>
            <div class="column-content">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" class="input-button" runat="server" Text="搜索" 
                    onclick="btnSearch_Click" />
            </div>
        </div>
        <div class="column mini">
            <div class="column-head">
                友情链接</div>
            <div class="column-content">
                <ul>
                    <li><a href="#">ACIGA</a></li>
                    <li><a href="#">IGR</a></li>
                    <li><a href="#">ICGA9</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="main-content">
        <div class="column picnews">
            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"
                width="312" height="210">
                <param name="movie" value="flashad/viewer.swf">
                <param name="quality" value="high">
                <param value="transparent">
                <param name="wmode" value="transparent">
                <embed src="flashad/viewer.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer"
                    type="application/x-shockwave-flash" width="312" height="210"></embed>
            </object>
        </div>
        <div class="column">
            <US:PostList ID="plxinwen" CategoryId="2" runat="server" />
        </div>
        <div class="column">
            <US:PostList ID="plkeyan" CategoryId="3" runat="server" />
        </div>
        <div class="column">
            <US:PostList ID="plzhaosheng" CategoryId="5" runat="server" />
        </div>
    </div>
</asp:Content>
