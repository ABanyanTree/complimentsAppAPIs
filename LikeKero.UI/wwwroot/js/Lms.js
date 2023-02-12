

function AllowedAttemptsExceded() {
    alert("You have exceeded the number of allowed attempts for this activity. Please contact your Performance Specialist.");
}

function OpenActivity(type, LaunchURL, ActivityId, UserId, AssignmentId, LaunchPreference, WindowSizeHeight, WindowSizeWidth, returnURL, IsAdminPreview, UniqueGUID) {

    sessionStorage.removeItem("final_exam_text_class");
    var url = "";

    switch (type) {
        case "Scorm":

            url = LaunchURL + "?courseId=" + ActivityId + "&userId=" + UserId + "&AssignmentId=" + AssignmentId + "&LaunchPreference=" + LaunchPreference + 
                "&returnURL=" + returnURL + "&IsAdminPreview=" + IsAdminPreview + "&UniqueGUID=" + UniqueGUID;
            
            OpenCourse(url, LaunchPreference, WindowSizeHeight, WindowSizeWidth);
            
            break;

        case "Assessment":
            url = turl + "?TestId=" + activityid + "&UserId=" + userid;
            OpenTest(url, 'Assessment', activityid, userid);
            break;
        case "Survey":
            url = surl + "?SurveyId=" + activityid + "&UserId=" + userid;
            OpenTest(url, 'Survey', activityid, userid);
            break;
        case "Poll":
            url = purl + "?PollId=" + activityid + "&UserId=" + userid;
            OpenTest(url, 'Poll', activityid, userid);
            break;
    }
}

function PreviewOpenActivity(type, activityid, userid) {
    var url = "";

    switch (type) {
        case "SCORM":

            url = scurl + "?courseId=" + activityid + "&userId=" + userid + "&isSameWindow=0&returnURL=" + screturnurl + "&IsPreview=1";
            window.location.href = url;
            break;
        case "Assessment":
            url = turl + "?TestId=" + activityid + "&UserId=" + userid + "&IsPreview=1";;
            OpenTestPreview(url, 'Assessment', activityid, userid);
            break;
        case "Survey":
            url = surl + "?SurveyId=" + activityid + "&UserId=" + userid;
            OpenTestPreview(url, 'Survey', activityid, userid);
            break;
        case "Poll":

            url = purl + "?PollId=" + activityid + "&UserId=" + userid;
            OpenTestPreview(url, 'Poll', activityid, userid);
            break;
    }
}

function PreviewActivity(type, activityid, userid) {
    var url = "";
    switch (type) {
        case "SCORM":



            url = scurl + "?courseId=" + activityid + "&userId=" + userid + "&isSameWindow=0&returnURL=" + screturnurl + "&IsPreview=1";
            // AttemptCount(activityid, userid) a
            //gPopupWindow = window.open(url, "course");

            ////window.location.href = url;

            //if (gPopupWindow)
            //    timerEvent = window.setInterval("CheckPopupWinClosed('scorm')", 100);
            OpenCourse(url, 'SCORM', activityid, userid);
            break;
        case "Assessment":
            url = turl + "?TestId=" + activityid + "&UserId=" + userid + "&IsPreview=1";;
            OpenTestPreview(url, 'Assessment', activityid, userid);
            break;
        case "Survey":
            url = surl + "?SurveyId=" + activityid + "&UserId=" + userid;
            OpenTestPreview(url, 'Survey', activityid, userid);
            break;
        case "Poll":

            url = purl + "?PollId=" + activityid + "&UserId=" + userid;
            OpenTestPreview(url, 'Poll', activityid, userid);
            break;
    }
}

function AttemptCount(activityid, userid) {

    $.ajax(
        {
            url: aurl + "?CourseId=" + activityid + "&LearnerId=" + userid,
            data: "",
            type: "POST",
            contentType: "text/plain",
            beforeSend: function (xhrObj) {
                //xhrObj.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                //xhrObj.setRequestHeader("Accept", "application/json");
            },
            success: function (data) {
                //alert("Assessment submitted");
                //fCloseAndRefresh("1");
            },
            error: function (x, y, z) {
                //alert("The connection to the internet has been lost. The assessment platform must close. \nYour assessment data will be safely stored and your progress will be bookmarked when you return to this assessment.");
                //fCloseAndRefresh("1");

            }
        });
}

function OpenTestPreview(url, type, activityid, userid) {
    var WinUrl = url;
    var w = $(window).width();
    var h = $(window).height();

    var left = ($(window).width() / 2) - (w / 2);
    var top = ($(window).height() / 2) - (h / 2);


    var WinName = "AssementViewer";
    //var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=1024,height=675,left=0,top=0,titlebar=yes';
    var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';

    gPopupWindow = window.open(WinUrl, WinName, WindowProperties);


    if (gPopupWindow)
        timerEvent = window.setInterval("CheckPopupWinClosed('" + type + "')", 100);





}


function OpenTest(url, type, activityid, userid) {
    var WinUrl = url;
    var w = $(window).width();
    var h = $(window).height();

    var left = ($(window).width() / 2) - (w / 2);
    var top = ($(window).height() / 2) - (h / 2);

    var WinName = "AssementViewer";
    //var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=1024,height=675,left=0,top=0,titlebar=yes';
    var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=yes';

    gPopupWindow = window.open(WinUrl, WinName, WindowProperties);


    if (gPopupWindow)
        timerEvent = window.setInterval("CheckPopupWinClosed('" + type + "')", 100);


    //check if the test has active questions


    var data = { testId: activityid, userid: userid }//
    $.ajax(
        {
            url: checkQuestCountURL,
            data: data,
            dataType: "Json",
            type: "POST",
            error: function () {

            },
            success: function (data) {
                if (data.s > 0) {
                    gPopupWindow = window.open(WinUrl, WinName, WindowProperties);

                    //if (gPopupWindow)
                    //    timerEvent = window.setInterval("CheckPopupWinClosed()", 100);
                }
                else {
                    alert("All the questions from the current test are inactive or not available.\nPlease contact your performance specialist.");
                    ////AssessmentRes.ZeroQuestions
                    //$.confirmOK({
                    //    "title": quesCountHeadMsg,
                    //    "message": quesCountMsg,
                    //    "buttons": {
                    //        'OK': {}
                    //    }
                    //});
                }
            },
            error: function (x, y) {
                if (x.status == 403) {
                    //var newURL = "../Session/SessionTimeout";
                    //window.location.href = newURL;
                }

            }
        });





}

function OpenCourse(url, LaunchPreference, WindowSizeHeight, WindowSizeWidth) {
    var WinUrl = url;
    var w = WindowSizeWidth;
    var h = WindowSizeHeight;
    

    if (LaunchPreference == 'Same Window') {
      
        window.location.href = url
    }
    else {
        

        var left = (WindowSizeWidth / 2) - (w / 2);
        var top = (WindowSizeHeight / 2) - (h / 2);


        var WinName = "LMS Player";
        var WindowProperties = 'toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,status=1,resizable=yes,width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',titlebar=0';

        
        gPopupWindow = window.open(WinUrl, WinName, WindowProperties);

        if (gPopupWindow)
            timerEvent = window.setInterval("CheckPopupWinClosed()", 100);


    }

   





}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function fCloseAndRefresh(sCatId) {
    //Commented by riday on 01/08/2018 to not to show msg on final exam after all
    if (top.opener && !top.opener.closed) {

        if (!gCurrentQuestion) { return }
        sFlag = "0";

        exitSaveElapsedTime();

        top.opener.document.location.href = top.opener.document.location.href;
    }
    top.close();
}

function CheckPopupWinClosed(type) {
    var flag;
    try {
        if (gPopupWindow && typeof (gPopupWindow.closed) != 'unknown')
            flag = gPopupWindow.closed;
        else
            flag = true;
    }
    catch (err) {
        flag = true;
    }
    //            alert(flag);
    if (flag) {
        clearInterval(timerEvent);
        fInternal = true;
        var uloc = document.location.href;
        uloc = updateQueryStringParameter(uloc, 'type', type)
        document.location.href = uloc;// document.location.href;
    }
}