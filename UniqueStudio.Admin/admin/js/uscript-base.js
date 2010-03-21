  
    function getEventSender(e)
    {
         if (e.currentTarget)
        {
         return e.currentTarget;
        }
        
        else
        { 
 
          return e.srcElement;
        }
    }
    
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
    
    
    function addEvent(target,event,func)
    {
    if (document.addEventListener)
     {
      target.addEventListener(event,func,true);
      }
      else
      {
         if (document.attachEvent)
         {
        
         target.attachEvent("on"+event,func);
         }
      }
    }
    
    
function getControlValue(control,cache)
{
 if (control.type=="select-one")
 {
   if(cache)
   {
   return control.selectedIndex ;
   }
   else
   {
    return control.options[control.selectedIndex].innerHTML;
   }
 }
 else
 {
  return control.value;
 }
}
function isStringEqualsIgnoreCase(str1,str2)
{
   return str1.toLowerCase()==str2.toLowerCase();
}
function getAncestor(sender,tag)
{
	
   var parent=sender;
   if (tag.constructor==Number)
   {
	   for (var i=0;i<tag;i++)
	   {
	     parent=parent.parentNode;
	   }
	 
   }
   else
   {
        while ((!isStringEqualsIgnoreCase(parent.tagName,tag))&&(!isStringEqualsIgnoreCase(parent.tagName,"body")))
	   {
		 parent=parent.parentNode;
	   }
   }
   return parent;
} 
function setControlValue(control,value)
{
   if (control.type=="select-one")
 {
   control.selectedIndex=value;
 }
 else
 {
  control.value=value;
 }
}