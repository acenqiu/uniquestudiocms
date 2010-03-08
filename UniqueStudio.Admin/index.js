// JavaScript Document
function addMenuEvent()
{
	 var p= document.getElementById("navigation");
	 var nodes=p.getElementsByTagName("ul");
   var i=0;
	for (i=0;i<nodes[0].childNodes.length;i++)
	{
		var li=nodes[0].childNodes[i];
		if (li.nodeType==1)
		{
		//alert(li);
			//li.childNodes[3].onclick="aa";
			
			li.attachEvent( "mouseove",showMenu);
			li.attachEvent( "mouseout",hideMenu);
			
			
		}
	}
}
function showMenu(e)
{
	var div=e.currentTarget.getElementsByTagName("div")[0]; 
	div.style.display="inline";
e.currentTarget.style.backgroundColor="#0066CC";

	
}
function hideMenu(e)
{
	var div=e.currentTarget.getElementsByTagName("div")[0] 
	div.style.display="none";
}
function show(li)
{
	var div=li.getElementsByTagName("div")[0]; 
	div.style.display="inline";
li.style.backgroundColor="#0066CC";
if ((li.className=="li-node")&&(li.parentNode.parentNode.id!="navigation"))
{
	 li.style.backgroundColor="#333333";
}

	
}
function hide(li)
{
	var div=li.getElementsByTagName("div")[0] 
	div.style.display="none";
	li.style.backgroundColor="";
}