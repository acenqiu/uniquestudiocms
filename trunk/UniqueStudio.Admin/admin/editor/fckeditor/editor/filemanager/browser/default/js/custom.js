function copyUrlHint(url)
{
 var s="";
 s+="<div class='hintBox'>";
s+="<div class='hintBox-title'>Hint</div>";
s+="<div class='hintBox-content'>"
s+="<div >Please copy the following URL and paste it into the textbox of image path.</div>"
s+="<div>"
s+="<span><input type='text' id='url-textbox'  value='";
s+=url;
s+="' onfocus='this.select()'/></span>"
s+="<span><a href='#' class='linkButton' onclick='copyToClipBoard()'>Copy</a></span>"
s+="</div>"
s+="</div>";
s+="</div>";
var div=document.createElement("div");
document.body.appendChild(div);
div.innerHTML = s;
document.getElementById("url-textbox").focus();

}
 function copyToClipBoard(){
   var clipBoardContent=''; 
   clipBoardContent+="复制到剪切板，记得结贴！！";
   window.clipboardData.setData("Text",document.getElementById('url-textbox').value);
     window.top.close();
  }