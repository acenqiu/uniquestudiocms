<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="pvstat.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.pvstat" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

    <script src="../js/jquery.gchart.js" language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
$(function () {
    var div = document.getElementById("RecentMonthPvStat");
    if (div!=null)
    {
        div.style.width = div.parentNode.offsetWidth +"px";
    }

	var array1=<%= JsArrayOfCount %>;
	var array2=<%= JsArrayOfDay %>;
	
	$('#RecentMonthPvStat').gchart({
		type: 'line',//图表类型
		title: '<%= Date.ToString("yyyy年MM月") %>网站访问量折线图', //图表标题
		series: [
			$.gchart.series(array1,'red')//图表数据
		],
		axes: [//图表坐标轴
			$.gchart.axis('left', <%= MinCount %>, <%= MaxCount %>,'blue'),
			$.gchart.axis('bottom',array2,'008000')
		],
		legend: 'top'});
});
    </script>

</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>选择月份可以查看各个月的访问统计结果。</p>
        <p>注：可能由于网络原因导致折线图无法显示，您可以检查网络并重试。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
            选择月份：
            <asp:DropDownList ID="ddlYears" runat="server" >
            </asp:DropDownList>年
           <asp:DropDownList ID="ddlMonths" runat="server" >
            </asp:DropDownList>月
            <asp:Button ID="btnView" runat="server" Text="查看" onclick="btnView_Click"  />
        </div>
    </div>
    <div class="panel">
        <div class="panel_title">
            访问统计图</div>
        <div class="panel_body">
            <div id="RecentMonthPvStat" style="height: 300px;">
            </div>
        </div>
    </div>
    
    <div class="panel">
        <div class="panel_title">每日访问量</div>
        <div class="panel_body">
            <table class="panel_table">
                <tr class="panel_table_head">
                    <asp:Literal ID="ltlHead" runat="server" /></tr>
                <tr><asp:Literal ID="ltlData" runat="server" /></tr>
             </table>
        </div>
    </div>
</asp:Content>
