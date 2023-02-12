

var APIAdapter = new clAPIAdapter();
var extLmsWin = getPlayerWindow();

if (window.addEventListener) {
    window.addEventListener("message", onMessage, false);
} else if (window.attachEvent) {
    window.attachEvent("onmessage", onMessage, false);
}

function getPlayerWindow() {
    var lmsWin = window;
    if ("undefined" != typeof lmsWin.opener && null !== lmsWin.opener)
        return lmsWin.opener;

    return null;
}

function DispatchMessage(func, message) {
    extLmsWin = getPlayerWindow();

    if (extLmsWin) {
        extLmsWin.postMessage({
            'func': func,
            'message': message
        }, "*");
    }
}

function onMessage(event) {
    var data = event.data;
    console.log("CDP: " + event)
    console.log("CDP: " + data)

    alert('Acknowledgement: ${event.data} from ${event.origin}');
}

function clAPIAdapter() {
    this.LMSInitialize = LMSInitialize;
    this.LMSGetValue = LMSGetValue;
    this.LMSSetValue = LMSSetValue;
    this.LMSCommit = LMSCommit;
    this.LMSFinish = LMSFinish;
    this.LMSGetLastError = LMSGetLastError;
    this.LMSGetErrorString = LMSGetErrorString;
    this.LMSGetDiagnostic = LMSGetDiagnostic;
    this.name = "CDPAPI";
}

function toDebug(str) {
    var tmpstr = "LMS: " + str;
    console.log(tmpstr);
    //alert(tmpstr);
}

function LMSInitialize(str) {
    toDebug("in LMSInitialize");

    return LMSIntInitialize(str)
}

function LMSGetValue(lParam) {
    toDebug("in LMSGetValue : " + lParam);
    return LMSIntGetValue(lParam)
}

function LMSSetValue(lParam, lSetValue) {
    toDebug("in LMSSetValue: " + lParam + " Data: " + lSetValue);
    DispatchMessage('set' + lParam, lSetValue)
    return LMSIntSetValue(lParam, lSetValue)
}

function LMSFinish(lParam) {
    toDebug("in LMSFinish");

    var retLMSFinish;
    try {
        DispatchMessage('finish', lParam);
        retLMSFinish = LMSIntFinish(lParam);
    }
    catch (e) { parent.alert(e); }

    if (!parent.IsSingleLaunchSco()) {
        //parent.ExitServerFrame.fnExitContentPlayer(true);
    }
    else {
        //top.close ();
    }

	/* // Let the exit button do the trick
    if (IsSameWindow == 'False') {
        window.close();
    }
    else {
        window.location.href = returnURL;
    }
	*/ 

    return retLMSFinish;
    //return LMSIntFinish(lParam)

}

function LMSCommit(lParam) {
    toDebug("in LMSCommit");

    DispatchMessage('commit', lParam)
    return LMSIntCommit(lParam)
}

function LMSGetLastError() {
    toDebug("in LMSGetLastError");

    var retVal = parseInt(LMSIntGetLastError(), 10);
    if (isNaN(retVal))
        return 0;
    else
        return retVal;
}

function LMSGetErrorString(lErrNo) {
    toDebug("in LMSGetErrorString");

    return LMSIntGetErrorString(lErrNo)
}

function LMSGetDiagnostic(lErrNo) {
    toDebug("in LMSGetDiagnostic");

    return LMSIntGetDiagnostic(lErrNo)
}

function LMSGetPage(strXML) {
    toDebug("in LMSGetPage");

    return LMSIntGetPage(strXML)
}