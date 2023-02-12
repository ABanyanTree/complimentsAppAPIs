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

function submitForm(pageNumber, formName) {
    var frm = $("#" + formName);

    var pageSize = $("#PageSize").val();
    $("#" + PageIndexhiddenFieldId).val(pageNumber);
    $("#" + PageSizehiddenFieldId).val(pageSize);

    frm.submit();
}


function btnGoClick(formId) {


    var pageSize = $("#PageSize1").val();

    if (pageSize < 0) {
        pageSize = "20";
    }

    var pageIndex = 1;
    $("#" + PageIndexhiddenFieldId).val(pageIndex);
    $("#" + PageSizehiddenFieldId).val(pageSize);

    var frm = $("#" + formId);
    frm.submit();
}
$(function () {
    $('th a, tfoot a').click(function () {

        var sort = getQueryVals("sort", $(this).attr('href'));
        var dir = getQueryVals("sortdir", $(this).attr('href'));
        var pageIndex = $("#grdPageLst").val();
        var pageSize = $("#PageSize").val();




        var sortName = $("#" + sorthiddenFieldId).val();
        var sortDir = $("#" + sortdirhiddenFieldId).val();
        if (sortName == sort) {
            if (sortDir == dir) {
                if (sortDir == "ASC") {
                    dir = "DESC";
                }
                else {
                    dir = "ASC";
                }
            }
        }



        $("#" + PageIndexhiddenFieldId).val(pageIndex);
        $("#" + PageSizehiddenFieldId).val(pageSize);
        $("#" + sorthiddenFieldId).val(sort);
        $("#" + sortdirhiddenFieldId).val(dir);

        //saveSortVals($('form'), sort, dir);
        $("#" + submitformname).submit();
        //  $("#" + PageFormName).submit();
        // alert($(this).attr('href'));
        //$(this).attr('href').submitForm();
        //$('form').attr('action', $(this).attr('href')).submit();
        return false;
    });



    function isNumberKey(evt) {

        var charCode = (evt.which) ? evt.which : event.keyCode

        if (charCode > 31 && (charCode < 48 || charCode > 57))

            return false;

        return true;

    }


    //Code Change Gaurav Serch Btn Click Change PageIndex Value
    $("input[data-search-btn='1']").click(function () {

        var SerchValuePageIndex = $('#' + PageIndexhiddenFieldId).val();
        if (SerchValuePageIndex > 1) {
            $('#' + PageIndexhiddenFieldId).val("1");
        }
    });






});
function getQueryVals(name, url) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(url);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


function passwordStrength(password) {
    var desc = new Array();
    desc[0] = "Very Weak";
    desc[1] = "Weak";
    desc[2] = "Better";
    desc[3] = "Medium";
    desc[4] = "Strong";
    desc[5] = "Strongest";

    var score = 0;

    //if password bigger than 6 give 1 point
    if (password.length > 6) score++;

    //if password has both lower and uppercase characters give 1 point	
    if ((password.match(/[a-z]/)) && (password.match(/[A-Z]/))) score++;

    //if password has at least one number give 1 point
    if (password.match(/\d+/)) score++;

    //if password has at least one special caracther give 1 point
    if (password.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;

    //if password bigger than 12 give another 1 point
    if (password.length > 7) score++;

    document.getElementById("passwordDescription").innerHTML = desc[score];
    document.getElementById("passwordStrength").className = "strength" + score;
}


function GetJsonDropDowns(url, data, OnSuccessCall, ID) {

    alert(url);
    $.ajax(
        {
            url: url,
            data: data,
            dataType: "Json",
            type: "POST",
            error: function () {
                alert('Error while retreiving data');
            },
            success: function (data) {
                OnSuccessCall(data, ID);
            }
        });
}

function getInnerText(el) {
    return el.textContent || el.innerText;
}
$(document).ready(function () {
    if (typeof tableDiv != 'undefined' && tableDiv != null && tableDiv != '') {
        var idofdiv = "#" + tableDiv + " table th";
        $(idofdiv).each(function () {
            var v = getInnerText(this);
            if (v.trim() == "{CheckBoxHeading}") {
                this.innerHTML = "<input type='checkbox' id='chkHeader' class='selectAll'  />";
            }
        });
    }
    $("#chkHeader").click(function () {

        if (document.getElementById('chkHeader').checked == true) {
            if (!($(".box").is(':disabled'))) {
                $(".box").prop('checked', true);
            }
        }
        else
            $(".box").prop('checked', false);
    });
});

/* grid select all  */


function PrintElem(elem) {
    Popup($(elem).html());
}

function Popup(data) {
    var mywindow = window.open('', '', 'height=400,width=600');
    mywindow.document.write('<html><head><title></title>');
    mywindow.document.write('<link rel="stylesheet" href="../Content/Style.css" type="text/css" />');
    mywindow.document.write('</head><body >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');
    mywindow.location.reload();
    mywindow.focus();
    //mywindow.print();

    //code change amit 13-8-14
    //close the window after print dialog is closed
    //mywindow.onfocus = function () { mywindow.close(); }

    var document_focus = false; // var we use to monitor document focused status.
    $(document).ready(function () { mywindow.window.print(); document_focus = true; });
    setInterval(function () { if (document_focus === true) { mywindow.window.close(); } }, 300);


    return true;
}

$(document).ready(function () {

    var tr = $('.bottomgrid_table').find('tr');
    tr.bind('mouseover', function (event) {
        $(this).css("background-color", "#e6f5fb");
    });
    tr.bind('mouseout', function (event) {
        $(this).css("background-color", "#d9e8ef");
    });


    //Code change Gaurav 8-8-14
    // jQuery plugin to prevent double submission of forms
    jQuery.fn.preventDoubleSubmission = function () {
        $(this).on('submit', function (e) {

            var $form = $(this);

            if ($form.data('submitted') === true) {
                // Previously submitted - don't submit again
                e.preventDefault();
            } else {
                // Mark it so that the next submit can be ignored
                if ($form.valid()) {
                    $form.data('submitted', true);
                }
            }
        });

        // Keep chainability
        return this;
    };


});

function disableAllControls() {
    $("input, select, textarea").attr("disabled", "disabled");
}


function showWarningMsg(valContent, valTitle, valIcon, valColor) {
    valTitle = typeof (valTitle) == 'undefined' ? 'Warning' : valTitle;
    valIcon = typeof (valIcon) == 'undefined' ? 'fa fa-warning' : valIcon;
    valColor = typeof (valColor) == 'undefined' ? 'orange' : valColor;

    $.alert({
        title: valTitle,
        icon: valIcon,
        type: valColor,
        content: valContent,
        animation: 'none'
    });
}

function showErrorMsg(valContent, valTitle, valIcon, valColor) {
    valTitle = typeof (valTitle) == 'undefined' ? 'Error' : valTitle;
    valIcon = typeof (valIcon) == 'undefined' ? 'fa fa-Error' : valIcon;
    valColor = typeof (valColor) == 'undefined' ? 'red' : valColor;

    $.alert({
        title: valTitle,
        icon: valIcon,
        type: valColor,
        content: valContent,
        animation: 'none'
    });
}

function isValidFile(fileName) {
    var _validFileExtensions = ["jpg", "jpeg", "bmp", "gif", "png", "pdf"];
    for (var j = 0; j < _validFileExtensions.length; j++) {
        var sCurExtension = _validFileExtensions[j];
        if (fileName.split('.').pop().toLowerCase() == sCurExtension) {
            return true;
        }
    }

    return false;
}

function isFileSizeExceeds(fileObj) {
    var maxFileSize = 10240000; // 10 MB
    if (isIE() && isIE() < 10) {
        return true;
    }
    else {
        fileSize = fileObj.files[0].size;
        if (fileSize > maxFileSize) {
            return false;
        }
        else {
            return true;
        }
    }


}
// Check if the browser is Internet Explorer
function isIE() {
    var myNav = navigator.userAgent.toLowerCase();
    return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
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

function commaSeparatedNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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

function GetOnlyDate(dt) {
    if (dt != null && dt != "") {
        var month_names = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var date = new Date(dt);
        var day = date.getDate();
        var month_index = date.getMonth();
        var year = date.getFullYear();
        //return "" + day + "-" + month_names[month_index] + "-" + year;
        return "" + month_names[month_index] + "-" + day + "-" + year;
    }
    else {
        return "";
    }
}

function GetDateWithTime(dt) {

    if (dt != null && dt != "") {

        //var month_names = ["Jan", "Feb", "Mar",
        //    "Apr", "May", "Jun",
        //    "Jul", "Aug", "Sep",
        //    "Oct", "Nov", "Dec"];

        //var date = new Date(dt);

        //var day = date.getDate();
        //var month_index = date.getMonth();
        //var year = date.getFullYear();

        //var sHour = date.getHours();
        //var sMinute = padValue(date.getMinutes());
        //var sAMPM = "AM";

        //var iHourCheck = parseInt(sHour);

        //if (iHourCheck > 12) {
        //    sAMPM = "PM";
        //    sHour = iHourCheck - 12;
        //}
        //else if (iHourCheck === 0) {
        //    sHour = "12";
        //}

        //sHour = padValue(sHour);

        //return "" + day + "-" + month_names[month_index] + "-" + year + " " + sHour + ":" + sMinute + " " + sAMPM;
        return "";
    }
    else {
        return "";
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
                location.reload();
            }
        }, 1000);
    }
}

function InnerLoder() {
    return '<div class="innerLoader"><div class="loadingimage"><div class="mainLoader"></div></div></div>';
}


function TruncateString(value, maxChars) {   

    return value == null ? "" : (value.length <= maxChars ? value : value.substring(0, maxChars) + "...");
}

$(document).ajaxSend(function (event, jqxhr, settings) {
}).ajaxComplete(function (event, xhr, options) {
}).ajaxError(function (event, jqxhr, settings, thrownError) {

    debugger;
    if (jqxhr != null && jqxhr != undefined && jqxhr.status != null && jqxhr.status != undefined && jqxhr.status == 500) {
        showWarningMsg('We were unable to process your request. Please try again later.', 'Oops! Sorry...');
    }
    else if (jqxhr != null && jqxhr != undefined && jqxhr.status != null && jqxhr.status != undefined && jqxhr.status != 0) {
        showWarningMsg('There seems to be some issue processing your request. Please try again.', 'Server Error');
    }
    
});