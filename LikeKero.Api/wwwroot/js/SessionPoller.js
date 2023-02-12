var objText, oInterval;
var gMyWinOnload = true;
var gSPInterval;
var gIsIE55OrAbove = fIsIE55OrAbove();

var gTimeCounter = 0;
var gDummyTimerId;
//var gTimeInterval; 

function fConnChkClock()
{
	gTimeCounter++;
	if (gTimeCounter == 10)
	{
		gTimeCounter = 0;
		fCheckServerConnection();
	}
}

function fCheckServerConnection()
{
	if (!top.frames.frmPoller.document.formMain)
	{
		alert("Aborting due to connection failure.");
		top.close();
		return;
    }
    
   
 	top.frames["frmPoller"].location.replace("SessionPoller.aspx");
}


function fMyWinOnload()
{

if(IsSessionNeverExpires)
  sessionpoller(gTimeInterval)
  
//	if(!gIsIE55OrAbove)
//	{
//		//document.all.tdModifyTable.style.visibility = 'hidden';
//	}
//	
//	//every 30 secs
//	if (gTimeInterval > 60000)
//	{
//		//gDummyTimerId = window.setInterval("fCheckServerConnection()", gTimeInterval);
//	}
//	else
//	{
//		//gDummyTimerId = window.setInterval("fCheckServerConnection()", 60000);
//	}
}

function fIsIE55OrAbove()
{
	var sBrowserInfo = navigator.appVersion;

	// Is it MSIE?
	var browserCheck1 = sBrowserInfo.indexOf("MSIE");
	if (browserCheck1 > -1)
	{
		browserCheck1 = true;
	}
	else
	{
		browserCheck1 = false;
	}

	// Is it version 5.5 or above?
	var browserCheck2;
	if (sBrowserInfo.indexOf("5.5") > -1 || sBrowserInfo.indexOf("6.0") > -1)
	{
		browserCheck2 = true;
	}
	else
	{
		browserCheck2 = false;
	}

	// Is it NOT Opera?
	browserCheck3 = sBrowserInfo.indexOf("Opera");

	if (browserCheck3 == -1)
	{
		browserCheck3 = true;
	}
	else
	{
		browserCheck3 = false;
	}

	if (browserCheck1 && browserCheck2 && browserCheck3)
	{
		return true;
	}
	else
	{
		return false;
	}
}

// code added by Rohit Gajarmal Date:28/12/2009 for SAI Project
function sessionpoller(gTimeInterval)
{
     
    
//alert ("Development Testing pls ignore session poller alerts....");
    if(gTimeInterval > 60000)//i.e 60 sec
    {
        window.setInterval("sendRequest('SessionPoller.aspx',AlertStatus)", gTimeInterval)
        // window.setInterval("sendRequest('http://grohitpc/admin/SessionPoller.aspx',AlertStatus)", gTimeInterval);
    }
    else
    {
        window.setInterval("sendRequest('SessionPoller.aspx',AlertStatus)", 60000)
        // window.setInterval("sendRequest('http://grohitpc/admin/SessionPoller.aspx',AlertStatus)", 60000);
    }
}
var count = 0;
function AlertStatus(req)
{
    if(count == 0)
    // alert("Session Poller message : "+req.responseText);
        count++;
}

/* XMLHTTP */

function sendRequest(url,callback,postData) {

     
	var req = createXMLHTTPObject();
	 
	if (!req) return;
	var method = (postData) ? "POST" : "GET";
	req.open(method,url,true);
	req.setRequestHeader('User-Agent','XMLHTTP/1.0');
	if (postData)
		req.setRequestHeader('Content-type','application/x-www-form-urlencoded');
	req.onreadystatechange = function () {
		if (req.readyState != 4) return;
		if (req.status != 200 && req.status != 304) {
		alert('HTTP error ' + req.status);
			return;
		}
		callback(req);
	}
	if (req.readyState == 4) return;
	req.send(postData);
}

function XMLHttpFactories() {
	return [
		function () {return new XMLHttpRequest()},
		function () {return new ActiveXObject("Msxml2.XMLHTTP")},
		function () {return new ActiveXObject("Msxml3.XMLHTTP")},
		function () {return new ActiveXObject("Microsoft.XMLHTTP")}
	];
}

function createXMLHTTPObject() {
	var xmlhttp = false;
	var factories = XMLHttpFactories();
	for (var i=0;i<factories.length;i++) {
		try {
			xmlhttp = factories[i]();
	        break;    		
		}
		catch (e) {
			continue;
		}
		
	}
	return xmlhttp;
}
