﻿    var path=""//删除按钮路径
    var inputCount=1;//控件个数，实际上传数少一个， 
    var Allupfiled=0;//总共上传
    var Endupfiled=0;//已上传
    var ua = navigator.userAgent.toLowerCase(); //浏览器信息
    //自动保存
    var AutoSaveDiv=document.getElementById("autosavestate");
    var AutoSaveTime=6000;
    var iframeid=0;
    //var fEditor=FCKeditorAPI.GetInstance('fckContent');
    var content;
    //获取sessionuri
    //var user=getSessionUser();
    //ajax
    if (typeof(XMLHttpRequest) == "undefined") 
    {
	    XMLHttpRequest = function() {
		    try { return new ActiveXObject("Msxml2.XMLHTTP.6.0"); }
		    catch(e) {}
		    try { return new ActiveXObject("Msxml2.XMLHTTP.4.0"); }
		    catch(e) {}
		    try { return new ActiveXObject("Msxml2.XMLHTTP.3.0"); }
		    catch(e) {}
		    try { return new ActiveXObject("Msxml2.XMLHTTP"); }
		    catch(e) {}
		    try { return new ActiveXObject("Microsoft.XMLHTTP"); }
		    catch(e) {}
		    throw new Error("This browser does not support XMLHttpRequest.");
	    };
    }
    var xmlhttp=new XMLHttpRequest();
    var info = {    
            ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
            op: /opera/.test(ua),     //匹配Opera浏览器    
            sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
            ch: /chrome/.test(ua),     //匹配Chrome浏览器    
            ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
            };    
    window.onload=SetClick;//加载完成，添加一个控件
    function SetClick()
    {
        //setInterval("AutoSave()",AutoSaveTime);
        var container=document.getElementById("fileUpArea");
        var input1=document.createElement("input");
	    input1.type="file";
	    //input1.name="file"+inputCount;
	    input1.name="filesupload";
	    input1.id="file"+inputCount;
	    input1.className="fileinput";
	    input1.onchange=function(event)
	    {
	        if(this.value)
	        {
	           var k=this.value.lastIndexOf("\\");
	           var str=this.value.substring(k+1);
	           var divs=document.getElementById("filetxt").getElementsByTagName("div");
	           var check=false;

	           for(var i=0;i<divs.length;i++)
	           {
	             if(divs[i].innerHTML.indexOf(str)!=-1)   
	             {
	                  check=true;
	                  break;
	             }
	           }
	           if(!check)
	           {   
	               Allupfiled++;
	               SetIframeInput(this,inputCount,str);
	           }
	           else
	           {
	               alert("不允许添加同名附件！请重命名！");
	               return;
	           }
	       }
	    }
	    container.appendChild(input1);
	    inputCount++;
	    //input1.click();
	}
	function CheckFileNames(str)
	{
	    var divs=document.getElementById("filetxt").getElementsByTagName("div");
	    var check=false;
	          
	    for(var i=0;i<divs.length;i++)
	    {
	        if(/"+str"+/i.test(divs[i].innerHTML))
	        {
	                  check=true;
	                  break;
	        }
	   }
	   
	}
	function SetIframeInput(input,num,str)//选取值后的file控件，第几个，选取的文件名称
	{
	    var body = document.body; //页面body
	    var name=input.id;//fileName
	    var contxt=document.createElement("div");//显示附件名称用的div
	    var filetxtDiv=document.getElementById("filetxt");//显示用的div(contxt)的上级div;
	    var iframename = "iframe"+name;//框架iframe的名称
	    var	iframe;//框架
	    var form=document.createElement('form');//创建表单
	    var statediv=document.createElement("span");//状态div
	    var stopdiv=document.createElement("span");//停止按钮
	    var jxupdiv;//上传按钮
	    var imgs=document.createElement('img');//删除按钮
	    var upedfilename;//上传后文件名称
	    //var filedNames=document.getElementById("filedName");//显示上传后所有附件名称，后台取值用
	    var filedNames=getfiledName();
	    var IsClicked=true;//判断img是否点击
        if(Endupfiled==0)
        {
            filedNames.value="";
        }
	    
	    contxt.id=input.id+"text";//显示用的div(contxt)的ID
	    contxt.innerHTML=str+"&nbsp;&nbsp;";//contxt的innerHTML（显示内容）
        contxt.className="";
        filetxtDiv.appendChild(contxt);//添加一个显示附件内容的div
        
        imgs.src=path+"img/f2.gif";
        imgs.onclick=Dispose;//删除事件
        contxt.appendChild(imgs);//添加删除按钮
        
        statediv.id="state"+num;
        statediv.className="spanstate";
        statediv.innerHTML="准备上传";
        contxt.appendChild(statediv);//添加状态div

	    //创建iframe
    	iframe = document.createElement( info.ie ? "<iframe name=\"" + iframename + "\">" : "iframe");
		if(info.ie)
		{
		    document.createElement("<iframe name=\"" + iframename + "\">");
		}
		else
		{
		    document.createElement("iframe");
		}
		iframe.name = iframename;
		iframe.style.display = "none";
		//插入body
		body.insertBefore( iframe, body.childNodes[0]);	
		iframeid++;
		var uri=getSessionUri();
		
		//设置form
		form.name="form"+name;//表单名称
		form.target=iframename;
        form.method="post";
        form.encoding="multipart/form-data";
        form.action=path+"Fileup.ashx?uri="+uri+"&action=add";
        //form.action="UserControls/CallBack.aspx";
        body.insertBefore( form, body.childNodes[0]);
        
        //添加控件进form
        form.appendChild(input);
        
        SetClick();//重新添加一个upfile控件
	    statediv.innerHTML="正在上传";
       
       //添加停止按钮
        stopdiv.id="stopdiv"+num;
        stopdiv.innerHTML="停止";
        stopdiv.style.cursor="hand";
        contxt.appendChild(stopdiv);
        stopdiv.onclick=function()//停止事件
        {
            iframe.src =path+"StopUpLoad.ashx";//无任何处理代码
            window.frames[iframename].location.reload(); //重新刷新iframe终止上传，在 iframe onload事件中处理
            //iframe.location.reload();
        }   
        //定义iframe 的onload事件
        if (info.ie)//IE 需要注册onload事件
        {
             iframe.attachEvent("onload",CallBack);
        } 
        else
        {
           iframe.onload = CallBack;
        }
        
        //提交 --------------------------------------------------
        form.submit();
        //提交完毕-----------------------------------------------
        function CallBack()//iframe加载完成，返回结果处理
        {
             
             try
             {
                      var value =iframe.contentWindow.document.body.innerHTML;
                      upedfilename=value.substring(value.indexOf("@returnstart@")+13,value.indexOf("@returnend@"));
                      if(upedfilename.length>3)//正常上传，返回上传后文件名
                      {
                          //alert(upedfilename); 
                          finished();
                      }
                      else//停止上传，从StopUpLoad.ashx页面返回空值，也可能是返回错误001，000
                      {
                          //扩展名判断
                          if(upedfilename=="002")
                          {
                            alert("附件扩展名不正确，请重新上传！");
                            IsClicked=false;
                            imgs.onclick();
                          }
                          if(upedfilename=="005")
                          {
                            alert("附件已存在!");
                            IsClicked=false;
                            imgs.onclick();
                          }
                          if(upedfilename=="003")
                          {
                            alert("删除成功");
                          }
                          statediv.innerHTML="等待上传";
                          statediv.style.color="#008080";
                          continueUpLoad();//上传按钮
                      }
              }
              catch(msg)
              {
                   statediv.innerHTML="上传失败";
                   statediv.style.color="#808080";
                   continueUpLoad();//上传按钮
              }
         }
         function continueUpLoad()//上传按钮
         {
             stopdiv.style.visibility="hidden";
             jxupdiv=document.createElement("span");//上传按钮
             jxupdiv.id="jxupdiv"+num;
             jxupdiv.innerHTML="上传";
             jxupdiv.style.cursor="hand";
             contxt.appendChild(jxupdiv);//添加上传按钮
             jxupdiv.onclick=function()//重新上传
             {           
                 contxt.removeChild(jxupdiv);
                 statediv.innerHTML="正在上传";
                 statediv.style.color="#0099FF";     
                 stopdiv.style.visibility="visible";
                 form.submit();//重新提交         
             }
         }
         function Dispose()//删除事件
         {
             //alert("ddd");
             if(IsClicked){
                DeleteEnclosure(str);
             }
             IsClicked=true;
             filetxtDiv.removeChild(contxt);
             body.removeChild(form);
             body.removeChild(iframe);
             iframeid--;
             Allupfiled--;//总上传数减一
             if(upedfilename)
             {
                 if(upedfilename.length>3)
                 {
                     Endupfiled--;
                     if(upedfilename==filedNames.value)
                     {
                         filedNames.value="";
                     }
                     else if(filedNames.value.indexOf(upedfilename)==0)
                     {
                         filedNames.value=filedNames.value.replace(upedfilename+",","");
                     }else{
                         filedNames.value=filedNames.value.replace(","+upedfilename,""); 
                     }
                 }
             }
         }
         function finished()//上传完毕
         {
            statediv.style.color="#ff0000";
	        statediv.innerHTML="上传成功";
	        contxt.removeChild(stopdiv);
            if(filedNames.value=="")
            {
                filedNames.value=upedfilename;
            }
            else
            {
                filedNames.value+=","+upedfilename;
            }
            Endupfiled++;//已上传数加一
         }
	}
	function checkFileState()
	{
	    if(Endupfiled!=Allupfiled)
	    {
	        alert("还有文件未上传成功！");
	        return false;
	    }
	    return true;
	}
	function HideEditDiv(i)
	{
	    var divid="editenclosure"+i;
	    document.getElementById(divid).style.display="none";
	}
	function DeleteEnclosure(filename)
	{
	    xmlhttp.open("POST",path+"Fileup.ashx?uri="+getSessionUri()+"&action=delete&filename="+filename,true);
        xmlhttp.onreadystatechange=function(){
            if(xmlhttp.readyState==4&&xmlhttp.status==200)
            {
                alert("删除成功,"+filename);
            }
        }
        xmlhttp.send(null);
	}
	function AutoSave()
	{
	    //alert("abc");
	    //content=fEditor.GetXHTML();
	    //var fEditor=getFCKName();+"&user="+user
	    content=window.frames[iframeid].window.frames[0].document.body.innerHTML;
	    xmlhttp.open("POST",path+"AutoSave.ashx?uri="+getSessionUri()+"&content="+content+"&action=save");
	    AutoSaveDiv.innerHTML="正在自动保存草稿";
	    xmlhttp.onreadystatechange=function(){
	        if(xmlhttp.readyState==4&&xmlhttp.status==200)
	        {
	            //alert("草稿保存成功");
	            var now=new Date();
	            AutoSaveDiv.innerHTML="草稿成功保存于"+now.getDate()+" "+now.getHours()+":"+now.getMinutes();
	        }
	    }
	    xmlhttp.send(null);
	    //alert(content);
	}
	function GetDraft()
	{
	    xmlhttp.open("GET",path+"AutoSave.ashx?uri="+getSessionUri()+"&action=load");
	    AutoSaveDiv.innerHTML="载入中";
	    xmlhttp.onreadystatechange=function(){
	        if(xmlhttp.readyState==4&&xmlhttp.status==200)
	        {
	           //window.frames[iframeid].window.frames[0].document.body.innerHTML=;
	        }
	    }
	    xmlhttp.send(null);
	}