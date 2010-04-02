var lock=false;
var cache=[];
var preRowCache;
var CONTROL_TYPE=[["label-input","input"],["label-select","select"]];
function focusInput(sender)
{
sender.style.display="block";
sender.parentNode.getElementsByTagName("label")[0].style.display="none";
}
function synchronizeData(dst,src)
{
  alert(src.type);
}
function blurInput(sender)
{
sender.style.display="none";
var l=sender.parentNode.getElementsByTagName("label")[0];
l.style.display="block";
l.innerHTML=getControlValue(sender);
}

function editRow(row)
{

 if (!lock)
 {
 	 lock=true;
	 var inputs=getControls(row);
	 for (var j=0;j<inputs.length;j++)
	 {
		 focusInput(inputs[j]);
		 //alert(inputs[j].value);
	    cache.push(getControlValue(inputs[j],true));
	 }
	  showSaveOrCancel(row);
	 preRowCache=row;
 }
 else
 {
    if(confirm("当前已经有一条记录正在被修改，确认不保存修改？"))
	{
	    cancelRow(preRowCache);
		  editRow(row);
	  
	}

 }
}
function showSaveOrCancel(row)
{
    var e=hasClass(row,"edit-row","span")[0];
	 e.style.display="none";
	 var sc=hasClass(row,"save-or-cancel","span")[0];
	 sc.style.display="block";
}

function getControls(row)
{
  var inputs=[];
  for (var i=0;i<CONTROL_TYPE.length;i++)
  {
     inputs=inputs.concat(hasClass(row,CONTROL_TYPE[i][0],CONTROL_TYPE[i][1]));
  }
  return inputs;
}
function returnToEdit(row)
{
    var e=hasClass(row,"edit-row","span")[0];
	 e.style.display="block";
	 var sc=hasClass(row,"save-or-cancel","span")[0];
	 sc.style.display="none";
	 cache=[];
	 lock=false;
}
function cancelRow(row)
{
 
 	  var inputs=getControls(row);
	 for (var j=0;j<inputs.length;j++)
	 {
		setControlValue(inputs[j],cache[j]);
		blurInput(inputs[j]);
	 }
	 returnToEdit(row);
}
function getRowFromLink(sender)
{
 return  getAncestor(sender,"tr");
}


function saveRow(row)
{

  ///ajax refresh data
  //alert(row.id);
  	  var inputs=getControls(row);	
  	  var s="?"
  	  var postO=new Object();
  	  postO["action"]="update";
  	  postO["ID"]=row.id;
    for (var j=0;j<inputs.length;j++)
	 {
		blurInput(inputs[j]);
		postO[inputs[j].name]=getControlValue(inputs[j]);
	 }

	   returnToEdit(row);
	   $.post("datacontrol.ashx",
       postO,
       function(data){
          toolTip("修改成功!",1000);
       });
     
}

function deleteRow(row)
{
  if (confirm("确认要删除?"))
  {
       row.parentNode.removeChild(row);
        var postO=new Object();
       postO["action"]="delete";
  	   postO["ID"]=row.id;
       $.post("datacontrol.ashx",
       postO,
       function(data){
          toolTip("删除成功!",1000);
       });
  }

}