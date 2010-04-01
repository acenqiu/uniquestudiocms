<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="UniqueStudio.Admin.admin.sidebar" %>

<%@ Import Namespace="UniqueStudio.Core.Site" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>引力中心实验室</title>
    <link rel="stylesheet" type="text/css" href="css/admin.css" />
    <link rel="stylesheet" type="text/css" href="css/index.css" />

    <script type="text/javascript" src="js/main.js" language="javascript"></script>

</head>
<body onload="addLiAction()">
    <div class="slider">
        <div class="admin-navigation">
            <ul>
                <asp:Panel ID="pnlGoBackToWebSite" runat="server">
                    <li><a href='<%= SiteManager.BaseAddress(SiteId)  %>' target="_top">进入网站</a></li>
                </asp:Panel>
                <asp:Literal ID="ltlNavigation" runat="server" />
            </ul>
        </div>
    </div>
</body>
</html>
