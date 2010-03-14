function changestate(sender)
{
    if (sender.className=="menu-activeted")
    {
        sender.className="";
    }
    else
    {
        sender.className="menu-activeted";
    }
}
var preLi=null;
var preRow=null;
function addFunction()
{
  addLiAction();
  addTrFunction();
}
function addTrAction()
{
 var tableRow=document.getElementsByTagName("tr");
 var i=0;
 
 for (i=0;i<tableRow.length;i++)
 {
     if (document.addEventListener)
     {
      tableRow[i].addEventListener("mouseover",hoverRow,true);
      }
      else
      {
         if (document.attachEvent)
         {
         tableRow[i].attachEvent("mouseover",hoverRow);
         }
      }
 }
}

function hoverRow(e)
{

  var row;
  if (e.currentTarget)
    {
     row=e.currentTarget;
    }
    
    else
    { 
      row=e.srcElement;
    }
     
 if (row.className=="")
 {
   row.className="row-hover";
 }

 ;
  if ((preRow!=null)&&(preRow.className!="row-select"))
  {
  preRow.className="";
  }
  preRow=row;

}

function addLiAction()
{
    var i=0;
    var j=0;
    var links=document.getElementsByTagName("a");
    for (i=0;i<links.length;i++)
    {  
      var li=links[i].parentNode;
     //li.onclick=clickMenu;
     // li.attachEvent("onclick",clickMenu);
     if (document.addEventListener)
     {
      li.addEventListener("click",clickMenu,true);
      }
      else
      {
         if (document.attachEvent)
         {
         li.attachEvent("onclick",clickMenu);
         }
      }
     // 

    }
   
}
function clickMenu(e)
{

  var li;
  if (e.currentTarget)
    {
     li=e.currentTarget;
    }
    
    else
    { 
      li=e.srcElement;
    }
 
  li.className="menu-activeted";
  var link=li.getElementsByTagName("a");
  parent.right.location=link[0].href;
  if (preLi!=null)
  {
  preLi.className="";
  }
  preLi=li;
  
    if (e.stopPropagation)
   {
      e.stopPropagation();
   }
   else
   {
     e.cancelBubble=true;
   }
}

function selectall(sender,controls)
{
    var c = document.getElementsByName(controls);
    for(var i=0;i<c.length;i++)   
    { 
          c[i].checked=sender.checked;
          selectRow(c[i]);
    }   
}

function selectRow(checkBox)
{
var row=checkBox.parentNode.parentNode;
if (checkBox.checked)
{
   row.className="row-select";
}
else
{
  row.className=""; 
}
}
function selectcheck(controls)
{
    var c = document.getElementsByName(controls);
    var count=0;
    for(var i=0;i<c.length;i++)   
    { 
          if (c[i].checked)
          {
            count++;
          }
    }   
    if (count==0)
    {
        alert("请选择至少一项。");
        return false;
    }
    else
    {
        return true;
    }
}
