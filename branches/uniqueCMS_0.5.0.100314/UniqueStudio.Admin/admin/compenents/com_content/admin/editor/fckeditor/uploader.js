var currentpicname;
var realpath="attachment/";

// the 1st frame, for insert image by button.
function insertmyUpload (filename) {
	//filename=filename.replace(/\*/g, '.');
	//filename=realpath+filename;
	//if (document.getElementById('ifautoaddubb').checked) filename=autoattachubb (filename);
	//parent.fckAddText(filename);
	//parent.fckInsertPic(attachid);
	alert("This function is disabed, \nplease contact ppbbqq-8@hotmail.com !");
}

// the 2nd/3rd frame, for insert image by link.
function generateUpload (attachid, filename) {
	//filename=filename.replace(/\*/g, '.');
	//filename=realpath+filename;
	//if (document.getElementById('ifautoaddubb').checked) filename=autoattachubb (attachid, filename);
	//parent.fckAddText(filename);

	if(filename.match(/.*\*jpg$/)||filename.match(/.*\*jpeg$/)||filename.match(/.*\*png$/)||filename.match(/.*\*gif$/)||filename.match(/.*\*bmp$/))
	{
		attachid=attachid.replace(/\[attach\]/g,'');
		attachid=attachid.replace(/\[\/attach\]/g,'');
		parent.fckInsertPic(attachid);
	}
	else if(filename.match(/.*\*swf$/)||filename.match(/.*\*flv$/)||filename.match(/.*\*mp3$/)||filename.match(/.*\*wma$/)||filename.match(/.*\*rm$/))
		alert("Type Mismatch!\nFor swf/flv | mp3/asf/wmv/wma/etc | rm/rmvb/ra/ram supporting, please wait...");	
	else
		parent.fckAddText("[sfile]" + attachid + "[/sfile]");
}

function autoattachubb (attachid, filename) {
	var finalresult;
	finalresult="[file]"+attachid+"[/file]";
	var extindex = filename.lastIndexOf(".");
	if (extindex!=-1) {
		var realext=filename.substring(extindex+1).toLowerCase();
		if (realext=="gif" || realext=="jpg" || realext=="png" || realext=="bmp" || realext=="jpeg") {
			 finalresult="[img]"+attachid+"[/img]";
		}
		else if (realext=="swf")  {
			 finalresult="[swf=400,300]"+attachid+"[/swf]";
		}
		else if (realext=="wma" || realext=="mp3" || realext=="asf" || realext=="wmv")  {
			 finalresult="[wmp=400,300]"+attachid+"[/wmp]";
		}
		else if (realext=="rm" || realext=="rmvb" || realext=="ra" || realext=="ram")  {
			 finalresult="[real=400,300]"+attachid+"[/real]";
		}
		else if (realext=="htm" || realext=="html")  {
			 finalresult="[url]"+attachid+"[/url]";
		}
		else if (realext=="zip" || realext=="rar")  {
			 finalresult="[file]"+attachid+"[/file]";
		}
	}
	return finalresult;
}

// the 3rd frame, for preview image.
function picPreview (filename, attachid) {
	//currentpicname=filename;
	var divdvs="<div style='margin-top: 0px;  width: 400px !important; width: 320px; height: 165px; background-repeat: no-repeat; background-position: 130px 20px !important; background-position: 0px 20px; background-image: url("+filename+");'>"+jslang[18]+"<input type='text' id='picalign' value='m' size=1 maxlength=1>(l-"+jslang[19]+",m-"+jslang[20]+",r-"+jslang[21]+") <input type='button' value='"+jslang[22]+"' onclick=\"doinsertimage('"+attachid+"');\"></div>";
	document.getElementById('picp').innerHTML=divdvs;
}

// the 3rd frame, for insert image by button.
function doinsertimage(attachid) {
	var alignpro=document.getElementById('picalign').value;
	var plusalign;
	if (alignpro=='l') plusalign=' align=l';
	else if  (alignpro=='r') plusalign=' align=r';
	else plusalign='';
	var finalresult="[img"+plusalign+"]"+attachid+"[/img]";
	//parent.fckAddText(finalresult);
	parent.fckInsertPic(attachid);
}
