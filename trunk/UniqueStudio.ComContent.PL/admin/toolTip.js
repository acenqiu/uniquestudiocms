// JavaScript Document
var toolTipInstance;
function toolTip(title,content,time)
{
setTimeout(function (){aa(title,content,time);},100);
function aa(title,content,time)
{
	var hasTitle=true;

	if (!content)
	{
		content=title;title=null;hasTitle=false;
	}
	else
	{
		if (content.constructor==Number)
				{
					time=content;
					content=title;
					title=null;
					hasTitle=false;
				}
	}
		

   var tip=document.createElement("span");
	  
	 if (hasTitle)
     {
	   var head=document.createElement("div");
	   head.className="tipHead";
	   head.innerHTML=title;
	   tip.appendChild(head);
	}
     
	 var mainContent=document.createElement("div");
	 mainContent.innerHTML=content;
	 mainContent.className="tipContent";
	 tip.appendChild(mainContent);
     tip.className="tipBox";
	 var p=document.getElementById("toolTip")||document.body;
     p.appendChild(tip);
	 if (!time||time>0)
	 {
	 setTimeout(function(){p.removeChild(tip);},time||1000);
	 }
	 else
	 {
		 toolTip.instance=tip;
	 }
}
}
function cancelToolTip()
{
	;
	 if(toolTip.instance)
	 {

		  var tip=toolTip.instance;
			var parent=tip.parentNode;
			parent.removeChild(tip);
			toolTip.instance=null;
	 }
}

