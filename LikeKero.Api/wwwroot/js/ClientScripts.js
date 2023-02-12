function OpenInNewWindow(WindowURL, WindowName)
{
	var WindowProperties=''
	switch(WindowName)
	{
		//case 'PLA':
		//	WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,width=1024,height=675,left=0,top=0';			
		//	break;
		//case 'PreTest':	
		//	WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,width=1024,height=675,left=0,top=0';
		//	break;
		case 'FullScreen':	
			WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,width=1024,height=675,left=0,top=0';
			break;
		case 'OpenFile':
			WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes, resizable=yes,width=1024,height=693,left=0,top=0';
			break;
		case 'IMOWWindow':
			WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes, resizable=yes,width=1024,height=750,left=0,top=0';
			break;
		case 'IMOVWindow':
			WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes, resizable=yes,width=1024,height=768,left=0,top=0';
			break;
		case 'LearnerMessage':
			WindowProperties = 'toolbar=no,statusbar=no,menubar=no,scrollbars=yes, resizable=no,width=600,height=450,left=0,top=0';
			break;	
									
	}
	//WindowURL = UnMaskChar(WindowURL);	
	WindowURL = UrlFullDecode(UrlFullEncode(WindowURL));
	//alert(WindowURL);
	var newWin = window.open(WindowURL, WindowName, WindowProperties);
} 

function UrlFullEncode(str) 
{
    str = escape(str);
    str = str.replace('%','%*');
    return str;
}

function UrlFullDecode(str)
{
    str = str.replace('%*','%');
    str = unescape(str);
    return str;
}
             
function UnMaskChar(URL)
{
	return URL.replace("$quote$","'");
}

function onEnterKeyPress()
{
	if ((event.which && event.which == 13) || (event.keyCode &&	event.keyCode == 13))
	{			
		event.keyCode=0;				
		return false;
	}
}

function onF5KeyPress()
{
	if ((event.which && event.which == 116) || (event.keyCode && event.keyCode == 116))
	{			
		event.keyCode=0;				
		return false;
	}

	if ((event.which && event.which == 13) || (event.keyCode &&	event.keyCode == 13))
	{			
		event.keyCode=0;				
		return false;
	}
}

function OnBeforeResultPrint()
{
	//document.all.PrintTable.bordercolor = "blue";
	document.all.PrintTable.border = "1";
	document.all.PrintHide1.style.visibility = "hidden";
	document.all.PrintHide2.style.visibility = "hidden";
	document.all.PrintHide3.style.visibility = "hidden";
	document.all.PrintHide4.style.visibility = "hidden";
	document.all.PrintHide5.style.visibility = "hidden";
}

function OnAfterResultPrint()
{
	document.all.PrintTable.border = "0";
	document.all.PrintHide1.style.visibility = "visible";
	document.all.PrintHide2.style.visibility = "visible";
	document.all.PrintHide3.style.visibility = "visible";
	document.all.PrintHide4.style.visibility = "visible";
	document.all.PrintHide5.style.visibility = "visible";
}


//Nikhil Rescheduling Start
function AskForResumeStudySession(kbaseid,cpoid,courseid,curRowIndex,bookmarkQuesIndex,dispQuesIndex)
{
    	
   	if(confirm('Do you want to resume from your saved study session?')==true)
	{
	   var str = "PLANormalQues.aspx?kbaseid="+kbaseid+"&cpoid="+cpoid+"&courseid="+courseid +"&curRowIndex="+curRowIndex+"&index="+bookmarkQuesIndex+"&displayindex="+dispQuesIndex;
	   OpenInNewWindow(str,"FullScreen");		
	}
	else
	{
		var str = "PLANormalQues.aspx?kbaseid="+kbaseid+"&cpoid="+cpoid+"&courseid="+courseid +"&curRowIndex="+curRowIndex+"&displayindex="+dispQuesIndex+"&restart=true";
		OpenInNewWindow(str,"FullScreen");
	}										
}

function AskForResumeStudySessionSummaryType(kbaseid,cpoid,courseid,curRowIndex,bookmarkQuesIndex,dispQuesIndex)
{
	if(confirm('Do you want to resume from your saved study session?')==true)
	{
	   var str = "PLAQuizModeSummaryQues.aspx?kbaseid="+kbaseid+"&cpoid="+cpoid+"&courseid="+courseid +"&curRowIndex="+curRowIndex+"&index="+bookmarkQuesIndex+"&displayindex="+dispQuesIndex;
	   OpenInNewWindow(str,"FullScreen");		
	}
	else
	{
		var str = "PLAQuizModeSummaryQues.aspx?kbaseid="+kbaseid+"&cpoid="+cpoid+"&courseid="+courseid +"&curRowIndex="+curRowIndex+"&displayindex="+dispQuesIndex+"&restart=true";
		OpenInNewWindow(str,"FullScreen");
	}
}

function reloadparentwindow()
{
	window.opener.location.reload(); 
	window.close();
}

////Nikhil Rescheduling End
