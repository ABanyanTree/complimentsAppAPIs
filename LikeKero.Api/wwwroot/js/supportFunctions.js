function ChangeURL(sReLoadURL) {
    
    window.location.replace(sReLoadURL);
}

var timer = -1;
function setTimer() {
     
    var sPageName = "scormcoursedefaultpage.aspx";
    var sConUrl = parent.DefaultContentFrame.location.href;
    sConUrl = sConUrl.substring(0, (sConUrl.toLowerCase().indexOf(sPageName)));
    parent.ContentSrvFrame.mainFrame.location.href = sConUrl + "dummy.html";
    timer = window.setInterval("sendRequest()", 2000);
}

function stop_timer() {
    if (timer !== -1) {
        window.clearInterval(timer);
        timer = -1;
    }
}

function sendRequest() {
    parent.DefaultContentFrame.location.reload(true);
    parent.DefaultContentFrame.location.href = "ScormCourseDefaultPage.aspx";
    parent.document.getElementById("frmsetLaunch").rows = "50,*,0,0";
    stop_timer();
}
var blnLaunchSco = false;
function SetLaunchSco(blnSetLaunch) {
    blnLaunchSco = blnSetLaunch;

}
function IsSingleLaunchSco() {
    return blnLaunchSco;
}
function hideFrame() {
    //var frameset = parent.document.getElementById("frmsetLaunch");
    //if (window.parent.name == "course") {
    //    frameset.rows = "0,*,0,0";
    //}
    //else {
    //    frameset.rows = "50,*,0,0";
    //}
}
function fnExitFrame() {
    try {
        if (parent.DefaultContentFrame.API != null)
            parent.DefaultContentFrame.LMSFinish("")
    } catch (e) { }
}



function fSetURL(lURL) {
    
    $("#coursecontainer").css('background-color', '#FFFFFF');
    $("#coursecontainer").attr('src', lURL)
    
}

var sendDataToServerAPI;
var totalscos;
var CourseTrackingId;
var AssignmentId;
var IsAdminPreview;
var CourseId;
var SessionId;

function sendDataToServer(taRecords) {
   
    var formData = {
        'TARecords': taRecords,
        'totalscos': totalscos,
        'CourseTrackingId': CourseTrackingId,
        'AssignmentId': AssignmentId,
        'IsAdminPreview': IsAdminPreview,
        'gLearnerId': gLearnerId,
        'gContentModuleName': gContentModuleName,
        'CourseId': CourseId,
        'SessionId': SessionId
    };

    $.ajax({
        url: sendDataToServerAPI,
        type: "POST",
        data: formData,
        cache: false,
        async: true,
        dataType: "json",
        success: function (data, textStatus, jqXHR) {
            if (typeof data.error === 'undefined') {
                //alert("Done");
            }
            else {
                alert('Error - API Communication error: ' + data.error)
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('No active internet connection! Your progress will be lost. Please connect to internet and launch again.');
            window.close();
        }
    });
}


function fnSetSessionPoller(gTimeInterval_Home) {
     
    if (gTimeInterval_Home > (60000 * 35)) //i.e 60 sec
    {
        window.setInterval("sessionPollerPing()", (gTimeInterval_Home - (60000 * 35)))
    }
    else {
        window.setInterval("sessionPollerPing()", (60000 * 35))
    }
}

function sessionPollerPing(SessionPingURL) {
     
    var formData = {
        'Sample': 'session pollar'
    };

    $.ajax({
        url: SessionPingURL,
        type: "GET",
        data: formData,
        cache: false,
        dataType: "json",
        success: function (data, textStatus, jqXHR) {
            if (typeof data.error === 'undefined') {
                //alert("Done");
            }
            else {
                //alert('ERRORS - Else: ' + data.error)
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //alert('ERRORS - Error Callback: ' + textStatus);
        }
    });
}

function saveOnExit() {

    LMSIntFinish("");
}
