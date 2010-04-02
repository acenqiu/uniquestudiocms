<%@ Page MasterPageFile="Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="UniqueStudio.CGE.Default" %>

<%@ Register Assembly="UniqueStudio.Controls" Namespace="UniqueStudio.Controls" TagPrefix="US" %>

<%@ Import Namespace="UniqueStudio.Common.Config" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">AC_FL_RunContent = 0;</script>
    <script src="flashad/js/AC_RunActiveContent.js" language="javascript"></script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
    <div class="index-slider">
        <div class="column mini" style="min-height: 210px">
            <US:Module ID="plIndex1" ModuleName="mod_postlist" runat="server" />
        </div>
         <div class="column mini" style="line-height: normal">
           <iframe frameborder="0" src="calendarframe.aspx" height="220px" width="100%" scrolling="no">
           </iframe>
        </div>
        <div class="column mini" style="display: none">
            <div class="column-head">
                站内搜索</div>
            <div class="column-content">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" class="input-button" runat="server" Text="搜索" OnClick="btnSearch_Click" />
            </div>
        </div>
        
        <div class="column mini">
            <US:Module ID="friendlyLink" ModuleName="mod_friendlylink" runat="server" />
        </div>
    </div>
    <div class="index-main-content">
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
            <US:Module ID="plIndex2" ModuleName="mod_postlist" runat="server" />
        </div>
        <div class="column">
            <US:Module ID="plIndex3" ModuleName="mod_postlist" runat="server" />
        </div>
        <div class="column">
            <US:Module ID="plIndex4" ModuleName="mod_postlist" runat="server" />
        </div>
    </div>
</asp:Content>
