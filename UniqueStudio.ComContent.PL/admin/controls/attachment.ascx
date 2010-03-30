<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="attachment.ascx.cs"
    Inherits="UniqueStudio.ComContent.PL.admin.controls.attachment" %>

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

<div id="fileUpArea">
</div>
<div id="filetxt">
</div>
<input id="filedNamelist" name="filedNamelist" type="hidden" runat="server" value="" />
