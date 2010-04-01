<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarnotice.aspx.cs" Inherits="UniqueStudio.Admin.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript" src="js/base.js">
</script>
<script language="javascript" type="text/javascript" src="js/toolTip.js">
</script>
<script language="javascript" type="text/javascript" src="js/jquery.js"></script>
    <style type="text/css">

body
{
 
}
table
{

}
td
{
	width:130px;
}
 table,tr,td
 {
 border:1px solid;
 outline:0px;
 border-spacing:0px;
 margin:0px;
 

 }
 .label-input, .label-select
 {
  background:#FFFFFF;
  display:none;
  
 }
 .label-input-focus
 {
 
   border:1px  ;
   outline:1px;
 }
 span a
 {
     padding-left:1.5em;
	
 }
 .edit-row
 {
 }
 .save-or-cancel
 {
   display:none;
 }
 .edit-button
 {
 background:url(images/edit.png) no-repeat;
 }
 .save-button
 {
 background:url(images/save.png) no-repeat;
 }
 .cancel-button
 {
  background:url(images/cancel.png) no-repeat;
 }
 select
 {
 display:none;
 }
 .tipBox
 {
    position:fixed;
	left:40%;
	top:40%;
	width:200px;
	  width:200px;
	background:#FFFF99;
	border:#FFCC66 1px solid;
	font-size:12px;
 }
 #toolTip .tipBox
 {
  position:inherit;
  display:block;

 }
 .tipBox .tipHead
 {
  border-bottom:#333333 1px solid;
  text-align:left;
  	padding:10px;
 }
 .tipBox  .tipContent
 {
  text-align:center;
  	padding:2px;
 }
</style>
<script type="text/javascript">
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
function addRow()
{
	  var postO=new Object();
  	  postO["action"]="add";
  	  postO["date"]='<%=Session["date"] %>';
   $.post("datacontrol.ashx",
       postO,
       function(data){
          toolTip("修改成功!",1000);
          window.location.reload();
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
          toolTip("修改成功!",1000);
       });
  }

}
function trial()
{
 cancelToolTip();
toolTip("修改成功!",1000);
}
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <a href="#" onclick="addRow()">add</a>
    </div>
    </form>
</body>
</html>
