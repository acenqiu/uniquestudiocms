<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="attachment.ascx.cs"
    Inherits="UniqueStudio.ComContent.Admin.attachment" %>

<script type="text/javascript">
function getfiledName()
{
   var a=document.getElementById("<%=filedNamelist.ClientID %>");
   return a;
}
function getSessionUri()
{
    var uri='<%=Session["posturi"] %>';
    return uri;
}
</script>

<span style="border: 1px solid #666666; padding: 4px; background: #ffff99; cursor: pointer;
    margin-left: 10px; position: absolute; bottom: 0">点击上传</span>
<div id="fileUpArea">
</div>
<div id="filetxt" style="margin-bottom: 10px">
</div>
<input id="filedNamelist" name="filedNamelist" type="hidden" runat="server" value="" />
