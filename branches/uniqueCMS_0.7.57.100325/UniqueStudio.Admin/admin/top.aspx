<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="UniqueStudio.Admin.admin.top" %>
<%@ Import Namespace="UniqueStudio.Common.Config" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>引力中心实验室 - 后台管理</title>
   
    <link rel="stylesheet" type="text/css" href="css/index.css" />
    <script src="js/uscript-base.js" type="text/javascript"  language="javascript"></script>
    <script language="javascript" type="text/javascript" >
  
    function applyMenu(parent,actived)
    {
      var item=parent.getElementsByTagName("li");
      for (var i=0;i<item.length;i++)
      {
        addEvent(item[i],"click",onLiClick);
      }
      item[0].className =actived;
      
     function onLiClick(e)
     {
       var l=getEventSender(e);
        while (!isStringEqualsIgnoreCase(l.tagName,"li")&&!isStringEqualsIgnoreCase(l.tagName,"body"))
        {
           l=l.parentNode;
        }
        var n=hasClass(l.parentNode,actived,"li");
        for (var j=0;j<n.length;j++)
        {
            n[j].className="";
        }
                                                    
        l.className=actived;
    }
    }

    </script>
    <asp:Literal ID="ltlEnableTime" runat="server">
        <script language="javascript" type="text/javascript">
             window.setInterval(cal,1000);
            <!--
            function cal()
            {   
            calendar = new Date();  
            day = calendar.getDay();  
            month = calendar.getMonth(); 
            date = calendar.getDate(); 
            year = calendar.getYear()+1900; 
            var dayname = new Array ("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"); 
            var monthname = 
                    new Array ("1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月" );          
            var span = document.getElementById("span_time");
            if (span !=null)
            {
                span.innerHTML='今天是'+year+"年"+monthname[month]+date+"日,"+dayname[day]+",北京时间"
                                                +calendar.toLocaleTimeString()+"。";
            }
            }   
        //-->
    </script></asp:Literal>

</head>
<body  onload="applyMenu(hasClass(document,'nav','ul')[0],'li-actived')">
    <div class="header" >
        <div class="logo">
            <h1>后台管理</h1>
        </div>
        <div>
            <ul class="nav" >
                <asp:Repeater ID="rptSiteList" runat="server">
                    <ItemTemplate>
                        <li><a href='sidebar.aspx?siteId=<%# Eval("SiteID") %>' target='left' >
                                    <%# Eval("SiteName") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
                <li><a href='sidebar.aspx' target='left'>系统管理</a></li>

                <li><a href="background/feedback.aspx" target="right">反馈信息</a></li>
            </ul>
         </div>
        <span style="text-align: right; color: White; font-weight: bold; float:right;padding-right:20px;">
            <h7>
            <asp:Literal ID="ltlUserName" runat="server" />,

            <script language="javascript" type="text/javascript">
                <!--
                    var s1=new String;
                    var h=((new Date()).getHours());
                    if (h>=23 || h<5) 	s1='凌晨好！';
                        else if (h>=5 && h<9)	s1='早上好！';
                        else if (h>=9 && h<12)	s1='上午好！';
                        else if (h>=12 && h<18)	s1='下午好！';
                        else 	s1='晚上好！';
                    document.write(s1);
                //-->
            </script>

            <span id="span_time"></span><a href="logout.aspx" target="_parent">注销</a>
            </h7>
        </span>
    </div>
</body>
</html>
