<?PHP
if (!defined('VALIDADMIN')) die ('Access Denied.');

if ($act=='edit')
{
$editoreditmodeonly2=<<<eot
<span id="timemsg">{$lna[1179]}</span>&nbsp; &nbsp;<span id="timemsg2"></span>
<script type='text/javascript' src='editor/fckeditor/autosaver.js'></script>
[<a href="javascript: stopautosaver();">{$lna[1176]}</a>] | [<a href="javascript: restartautosaver();">{$lna[1175]}</a>] | [<a href="javascript: stopforever();">{$lna[1177]}</a>] | [<a href="javascript: switchtodraft();">{$lna[1173]}</a>] | [<a href="javascript: savedraft();">{$lna[1178]}</a>] | [<a href="javascript: cleardraft();">{$lna[1180]}</a>]
eot;
}

$editorjs=<<<eot
<script language="javascript" type="text/javascript" src="editor/fckeditor/fckeditor.js"></script>

<script type="text/javascript">
window.onload = function()
{
	var oFCKeditor = new FCKeditor( 'content' ) ;
	oFCKeditor.Height = "400";
	oFCKeditor.Width = "100%" ; 
	oFCKeditor.BasePath = '/blog/editor/fckeditor/' ;
	oFCKeditor.ReplaceTextarea() ;
}

function fckAddText(txt)
{
	var oEditor = FCKeditorAPI.GetInstance('content');
	oEditor.InsertHtml(txt);
}

function fckInsertPic(fid)
{
	var oEditor = FCKeditorAPI.GetInstance('content');
	var ip = oEditor.EditorDocument.createElement('p');
	ip.innerHTML = '<a href="/blog/attachment.php?fid=' + fid + '" target="_blank"><img style="border: 1px solid black;" src="/blog/attachment.php?fid=' + fid + '" alt="" /></a>';
	oEditor.InsertElement(ip);
}	

</script>
eot;

$editorbody=<<<eot
<textarea name="content" id="content" rows='20' cols='100' style='font-size: 10pt;'>{content}</textarea>
<div style="margin-top:8px;margin-bottom:8px">
[<a href="javascript:showhidediv('FrameUpload');" title="{$lna[741]}" class="thickbox" style="color:blue;font-weight:900">{$lna[741]}</a>]
-- [<a href="javascript:fckAddText('[separator]');" title="{$lna[701]}" class="thickbox" style="color:red;font-weight:900">{$lna[701]}</a>]
-- [<a href="javascript:fckAddText('[newpage]');" title="{$lna[702]}" class="thickbox" style="color:red;font-weight:900">{$lna[702]}</a>]
-- [<a href="javascript:fckAddText('[code][/code]');" title="{$lna[694]}" class="thickbox" style="color:olive;font-weight:900">{$lna[694]}</a>]
-- [<a href="javascript:fckAddText('[file][attach][/attach][/file]');" title="{$lna[698]}" class="thickbox" style="color:olive;font-weight:900">{$lna[698]}</a>]
-- [<a href="javascript:fckAddText('[sfile][attach][/attach][/sfile]');" title="{$lna[699]}" class="thickbox" style="color:olive;font-weight:900">{$lna[699]}</a>]
<br />&gt;&gt; $editoreditmodeonly2 
	</div>
<div id="FrameUpload" style="display:none;"><iframe width=100% frameborder=0 height=200 src='admin.php?act=upload&useeditor={$useeditor}'></iframe></div>
<li>{$lna[743]}</li>
<li>{$lna[744]}</li>
eot;

?>