<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCategories.ascx.cs"
    Inherits="UniqueStudio.CGE.controls.SubCategories" %>
    <script >
    function hasClass(node,name,type)
    {
      var r=[];
      var re=new RegExp("(^|\\s)"+name+"(\\s|$)");
      var e = node.getElementsByTagName(type||"*");
      for (var j=0;j<e.length;j++)
      {

         if (re.test(e[j].className))
	     r.push(e[j]);
      }
      
      return r;
    }
      function reLink()
      {

        var list=hasClass(document.getElementById("subCategory"),"li-node","li");
      
        for (var i=0;i<list.length;i++)
        {
  
          for (var j=0;j<list[i].childNodes.length;j++)
          {
                  
            if (list[i].childNodes[j].tagName=="div"||list[i].childNodes[j].tagName=="DIV")
            {
              var div=list[i].childNodes[j];
              list[i].removeChild(div);
              list[i].appendChild(div);
                break;
            }
          
          }
         //list[i].getElementsByTagName("a")[0].href=list[i].getElementsByTagName("ul")[0].getElementsByTagName("a")[0].href;
        }
      }
    </script>
<div class="column-head" >
    <span>
        <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal></span></div>
<div class="column-content" id="subCategory">
    <asp:Literal ID="ltlList" runat="server"></asp:Literal>
    <script>reLink();</script>
</div>
