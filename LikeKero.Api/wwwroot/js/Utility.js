function showSuccessMsg(valContent, valTitle, valIcon, valColor) {
    valTitle = typeof (valTitle) == 'undefined' ? 'Success' : valTitle;
    valIcon = typeof (valIcon) == 'undefined' ? 'fa fa-check' : valIcon;
    valColor = typeof (valColor) == 'undefined' ? 'green' : valColor;

    $.alert({
        title: valTitle,
        icon: valIcon,
        type: valColor,
        content: valContent
    });
}

function successMsgWithCallBack(msg, functionName) {
    $.confirm({
        title: 'Success',
        icon: 'fa fa-check',
        type: 'green',
        content: msg,
        buttons: {
            Ok: functionName
        }
    });
}

function showWarningMsg(valContent, valTitle, valIcon, valColor) {
    valTitle = typeof (valTitle) == 'undefined' ? 'Warning' : valTitle;
    valIcon = typeof (valIcon) == 'undefined' ? 'fa fa-warning' : valIcon;
    valColor = typeof (valColor) == 'undefined' ? 'orange' : valColor;

    $.alert({
        title: valTitle,
        icon: valIcon,
        type: valColor,
        content: valContent
    });
}

//Get time format in string
function GetCourseDuration(CourseDurationHH, CourseDurationMM) {
    if (CourseDurationHH == 0) {
        return (CourseDurationMM < 9 ? "0" : "") + CourseDurationMM + " mins";
    }
    else if (CourseDurationMM == 0) {
        return (CourseDurationHH < 9 ? "0" : "") + CourseDurationHH + " hr";
    }
    else {
        return (CourseDurationHH < 9 ? "0" : "") + CourseDurationHH + " hr " + (CourseDurationMM < 9 ? "0" : "") + CourseDurationMM + " mins";
    }
}



function LaunchNewWindow(url, WinName, Isreload) {

    var w = 1000;//$(window).width();
    var h = 800;//$(window).height();


    var left = ($(window).width() / 2) - (w / 2);
    var top = ($(window).height() / 2) - (h / 2);
    var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';
    var win = window.open(url, WinName, WindowProperties);
    if (Isreload == true) {
        var timer = setInterval(function () {
            if (win.closed) {
                clearInterval(timer);
                //location.reload();
                //alert('new');
                //alert(window.parent.frames["FrameLaunchSco"]);

                if (window.parent.frames["FrameLaunchSco"] != undefined
                    && window.parent.frames["FrameLaunchSco"] != null
                    && window.parent.frames["FrameLaunchSco"] != 'undefined') {
                    window.parent.frames["FrameLaunchSco"].contentWindow.location.reload();
                }
                else {
                    window.parent.parent.frames["FrameLaunchSco"].contentWindow.location.reload();
                }


                //var el = document.getElementById('FrameLaunchSco');
                //el.src = el.src;
            }
        }, 1000);
    }
}

var publicurl = '';
function LaunchNewWindowStart(url, WinName, Isreload) {
    try {
        var el = window.parent.frames["FrameLaunchSco"];
        publicurl = encodeURI(el.src);



        var w = 1000;//$(window).width();
        var h = 800;//$(window).height();

        var left = ($(window).width() / 2) - (w / 2);

        var top = ($(window).height() / 2) - (h / 2);

        var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';

        var win = window.open(url, WinName, WindowProperties);

        postDataToNewWindowWithReload()

        if (win === null) {


            err = {};
            err.Message = "A Popup blocker setting is preventing the window from opening. Please disable pop-up blockers for this site.";
            throw err;
        }


        if (Isreload == true) {

            var timer = setInterval(function () {
                if (win.closed) {
                    clearInterval(timer);





                    var reloadurl = publicurl;



                    if (reloadurl.indexOf('IsReload') != -1) {

                    }
                    else {
                        reloadurl = reloadurl + encodeURI("&IsReload=true");
                    }


                    el.src = reloadurl;

                }
            }, 1000);
        }
    }
    catch (e) {

        $("#divPopupBlocker").show();

        $("#divPossiblePopupBlockerMessage").hide();
    }
}

//Devi: Sco window is closed. Initiate collapsing of bridge
function CloseBridge(Close_BridgeURL) {
    var el = window.parent.frames["FrameLaunchSco"];
    el.src = Close_BridgeURL;
}

function postToIframe(data, url, target) {
    //alert("in postToIframe");
    //try {
    //    $('body').append('<form action="' + url + '" method="post" target="' + target + '" id="postToIframe"></form>');
    //    $.each(data, function (n, v) {
    //        $('#postToIframe').append('<input type="hidden" name="' + n + '" value="' + v + '" />');
    //    });
    //    $('#postToIframe').submit().remove();
    //}
    //catch (e) {
    //    alert("Unable to post the data to server.");
    //}

    //debugger;

    try {
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("action", url);
        // form.setAttribute("target", target);
        form.setAttribute("id", "postToIframe");
        form.setAttribute("name", "postToIframe");

        //for (var key in data) {
        //    var value = data[key];
        //    console.log('key: ' + key + ' value:' + value);
        //}

        for (var key in data) {
            var value = data[key];

            var inField = document.createElement("input");
            inField.setAttribute("type", "hidden");
            inField.setAttribute("name", key);
            inField.setAttribute("value", value);

            form.appendChild(inField);
        }

        var contentWin = document.getElementById(target).contentWindow;
        contentWin.document.body.appendChild(form);

        // document.getElementsByTagName("body")[0].appendChild(form);

        form.submit();
        form.parentNode.removeChild(form);

        //var element = document.getElementById(elementId);
        //element.parentNode.removeChild(element);

    }
    catch (e) {
        alert("Unable to post the data to server.");
    }
}

var win = null;
function postDataCourseFirstTime(data, url, target) {

    //var potocols = data.pipeurl.substring(0, 6);
    //if (potocols == "https:") {
    //    url = url.replace('http:', 'https:');
    //}
    //else {
    //    url = url.replace('https:', 'http:');
    //}

    try {
        var el = window.parent.frames["FrameLaunchSco"];
        publicurl = encodeURI(el.src);

        var Isreload = true;
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("action", url);
        form.setAttribute("target", target);
        form.setAttribute("id", "postToIframe");
        form.setAttribute("name", "postToIframe");

        for (var key in data) {
            var value = data[key];

            var inField = document.createElement("input");
            inField.setAttribute("type", "hidden");
            inField.setAttribute("name", key);
            inField.setAttribute("value", value);

            form.appendChild(inField);
        }

        document.getElementsByTagName("body")[0].appendChild(form);

        var w = 1000;//$(window).width();
        var h = 800;//$(window).height();

        var left = ($(window).width() / 2) - (w / 2);

        var top = ($(window).height() / 2) - (h / 2);

        var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';

        win = window.open("", target, WindowProperties);
        form.submit();
        form.parentNode.removeChild(form);

        if (win === null) {


            err = {};
            err.Message = "A Popup blocker setting is preventing the window from opening. Please disable pop-up blockers for this site.";
            throw err;
        }


        if (Isreload == true) {

            var timer = setInterval(function () {
                if (win.closed) {
                    clearInterval(timer);
                    //window.parent.frames["FrameLaunchSco"].contentWindow.fnFirstCourseLaunch();
					//window.parent.frames["FrameLaunchSco"].fnFirstCourseLaunch();
					fnFirstCourseLaunch();
                    
                }
            }, 1000);
        }
    }
    catch (e) {
        $("#divPopupBlocker").show();

        $("#divPossiblePopupBlockerMessage").hide();
    }
}


function postDataAssessment(data, url, target, AssignmentId) {
   
    try { 
        var Isreload = true;
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("action", url);
        form.setAttribute("target", target);
        form.setAttribute("id", "postToIframe");
        form.setAttribute("name", "postToIframe");

        for (var key in data) {
            var value = data[key];

            var inField = document.createElement("input");
            inField.setAttribute("type", "hidden");
            inField.setAttribute("name", key);
            inField.setAttribute("value", value);

            form.appendChild(inField);
        }

        document.getElementsByTagName("body")[0].appendChild(form);

        var w = 1000;//$(window).width();
        var h = 800;//$(window).height();

        var left = ($(window).width() / 2) - (w / 2);

        var top = ($(window).height() / 2) - (h / 2);

        var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';

        win = window.open("", target, WindowProperties);
        form.submit();
        form.parentNode.removeChild(form);

        
        if (win === null) {


            err = {};
            err.Message = "A Popup blocker setting is preventing the window from opening. Please disable pop-up blockers for this site.";
            throw err;
        } 
        if (Isreload == true) {
            
            var timer = setInterval(function () {
                
                if (win.closed) {
                    clearInterval(timer);

                    window.parent.fnLoadFrameLaunchSco(false, AssignmentId);
				    

                }
            }, 1000);
        }



    }
    catch (e) {
        $("#divPopupBlocker").show();

        $("#divPossiblePopupBlockerMessage").hide();
    }
}

