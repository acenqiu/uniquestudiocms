<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCategories.ascx.cs"
    Inherits="UniqueStudio.CGE.controls.SubCategories" %>
    <script >
      function reLink()
      {
        var list=document.getElementsByTagName("li");
      }
    </script>
<div class="column-head">
    <span>
        <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal></span></div>
<div class="column-content">
    <asp:Literal ID="ltlList" runat="server"></asp:Literal>
</div>
