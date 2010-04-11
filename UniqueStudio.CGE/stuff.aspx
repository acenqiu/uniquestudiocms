<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="stuff.aspx.cs" Inherits="UniqueStudio.CGE.stuff" %>

<%@ Import Namespace="UniqueStudio.Common.Config" %>
<%@ Register Src="controls/SubCategories.ascx" TagPrefix="US" TagName="SubCategories" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<title ><%=StuffName%></title>
  <link rel="stylesheet" type="text/css" href="css/article-style.css" />
<script>
function SetCwinHeight(obj)
{

  var cwin=obj;
  if (document.getElementById)
  {
    if (cwin && !window.opera)
    {
      if (cwin.contentDocument && cwin.contentDocument.body.offsetHeight)
        cwin.height = cwin.contentDocument.body.offsetHeight+30;
      else if(cwin.Document && cwin.Document.body.scrollHeight)
        cwin.height = cwin.Document.body.scrollHeight+30;
    }
  }

}
</script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="cphMain" runat="server">
 <div class="slider">
        <div class="column mini" id="category" style="min-height: 150px">
            <US:SubCategories ID="subCategories" runat="server"  />
        </div>
    </div>
    <div class="main-content">
        <div class="site-depth"> 
            <a href="default.aspx">首页</a><asp:Literal ID="sitePath" runat="server" Text="35" />
        </div>
        <iframe  id="stuffFrame"  height="800px"src="http://localhost:4761/stuff/<%=StuffName%>.html"  scrolling="auto"width="100%" onload="SetCwinHeight(this)" frameborder="0"></iframe>
       </div>
</asp:Content>