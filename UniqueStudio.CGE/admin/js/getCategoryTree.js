
var idRegex=/#(\d)*?#/;
var pidRegex=/\*(\d)*?\*/;
var catList=[];
var idHash=[];
var root=new Object();
function getCategory()
{
root=new Category(0,0,"");
var catSpan=$("#categorySpan");
 catSpan.find("td").each(function (item)
                {
                var p=document.createElement("span");
                //p.innerHTML=item.html;
                var content=$(this).html();
                p.innerHTML=content;
                var label=$(this).find("label").html();
                /*
                var t=$(this);
                t.find("input").each(function (a)
                {
                    $(this).attr("disabled","disabled");
                   
                });
                */
                if (label)
                {
                  var id=trimId(label.match(idRegex)[0]);
                  var pid=trimId(label.match(pidRegex)[0]);
                  content=content.replace(idRegex,"");
                  content=content.replace(pidRegex,"");
                  var obj=new Category(id,pid,content);
                  catList[item]=obj;
                  idHash[id]=item;

                 }
               
                });
   catSpan.addClass("categorySpan-common");
   catSpan.mouseover(function(){ catSpan.addClass("categorySpan-hover"); catSpan.removeClass("categorySpan-common");});
   catSpan.mouseout(function(){ catSpan.removeClass("categorySpan-hover"); catSpan.addClass("categorySpan-common");});
   catList.push(root);
   idHash[0]=catList.length-1;
  for(var i=0;i<catList.length-1;i++)
  {
    catList[idHash[catList[i].pid]].childrenList.push(catList[i]);
  }
 catSpan.html(rangeCategory(root));
  catSpan.css("visibility","visible");
}
function Category(id,pid,html)
{
   this.id=id;
   this.pid=pid;
   this.html=html;
   this.childrenList=[];
}
function rangeCategory(root)
{
  var s=root.html
  var l=root.childrenList.length;
  if (l>0)
  {

      var j= s.indexOf("input");
      if (j<0)
      {
      j=s.indexOf("INPUT");
      }
      var a;
    //  alert(j);
      if (j>=0)
      {
          j+=6;
         // alert(s);
          a=s.substring(0,j);
          a+="disabled=\"disabled\"";
          a+=s.substring(j+1,s.length);
          s=a;
      }
    
      s+="<ul>";
      for (var i=0;i<l;i++)
      {
         s+="<li>"+rangeCategory(root.childrenList[i])+"</li>";
      }
      s+="</ul>";
   }
   
 return s;
}
function trimId(id)
{
    return id.substr(1,id.length-2);
}
function getCatListByParentId(id)
{
   
}