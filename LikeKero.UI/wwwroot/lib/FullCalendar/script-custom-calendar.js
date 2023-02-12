$(document).ready(function () {
    var cmurl = "";
    if (window.location.href.indexOf("tekserver") > -1 || window.location.href.indexOf("192.168.0.7") > -1) {
        cmurl = "/MMI";
    }
    $('#calendar').fullCalendar({
        header:
        {
            left: '',
            center: 'prev,title,next',
            right: ''
            //left: 'prev,next today',
            //center: 'title',
            //right: ''
        },
        //buttonText: {
        //    today: 'today',
        //    month: 'month',
        //    week: 'week',
        //    day: 'day'
        //},

        events: function (start, end, timezone, callback) {
            $('#DivCMloader').show();
            var viewMonth = $('#calendar').fullCalendar('getView').title;

            $.ajax({
                //url: window.location.origin + "/CMDashboard/GetCalendarData?viewMonth=" + viewMonth,
                url: cmurl + "/CMDashboard/GetCalendarData?viewMonth=" + viewMonth,
                //'http://'+$(location).attr('host')+'/MMI/CMDashboard/GetCalendarData?viewMonth=' + viewMonth,
                type: "GET",
                dataType: "JSON",
                success: function (result) {
                    if (result.length > 0) {
                        var estimate = result[0].sTotalEstimateHrs != null ? result[0].sTotalEstimateHrs : 0;
                        $("#TotalEstimateHours").text(estimate);
                        var layout = result[0].sTotalLayoutHrs != null ? result[0].sTotalLayoutHrs : 0;
                        $("#TotalLayoutHours").text(layout);
                        var actual = result[0].sTotalActualHrs != null ? result[0].sTotalActualHrs : 0;
                        $("#TotalActualHours").text(actual);
                        var timesheet = result[0].sTotalTimesheetHrs != null ? result[0].sTotalTimesheetHrs : 0;
                        $("#TotalTimesheetHours").text(timesheet);

                        var events = [];
                        //var startDate = moment(new Date());
                        //  var endDate = moment().add(15, 'days');       



                        $.each(result, function (i, data) {
                            //TodaysDate = data.TodayDate;
                            var startDate = moment(Todays).format('ddd,DD MMM YYYY');
                            var endDate = moment(startDate).add(16, 'days');
                            var currDate = moment(data.CalDate).format('ddd,DD MMM YYYY');
                            var dateDiff = moment(startDate).diff(currDate, 'days');
                            if (moment(data.CalDate) >= moment(startDate) && moment(data.CalDate) <= moment(endDate) && dateDiff != 0) {
                                events.push(
                                {
                                    title: data.TotalJobs > 0 ? data.TotalJobs : 0,
                                    description: data.TotalLabours >= 0 && data.TotalJobs > 0 ? data.TotalLabours : '',
                                    Shows: data.Shows,
                                    start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                                    start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                                    backgroundColor: "#fef5e6"
                                    // jobType: data.JobType
                                    //borderColor: "#fc0101",
                                });
                            }
                            else if (dateDiff != 0) {
                                events.push(
                           {
                               title: data.TotalJobs > 0 ? data.TotalJobs : 0,
                               description: data.TotalLabours >= 0 && data.TotalJobs > 0 ? data.TotalLabours : '',
                               Shows: data.Shows,
                               start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                               start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                               backgroundColor: "#ffffff"
                               //borderColor: "#fc0101",
                           });
                            }
                            else if (dateDiff == 0) {
                                events.push(
                          {
                              title: data.TotalJobs > 0 ? data.TotalJobs : 0,
                              description: data.TotalLabours >= 0 && data.TotalJobs > 0 ? data.TotalLabours : '',
                              Shows: data.Shows,
                              start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                              start: moment(data.CalDate).format('ddd,DD MMM YYYY'),
                              backgroundColor: "yellow"
                              //borderColor: "#fc0101",
                          });
                            }
                        });
                        callback(events);
                    }
                    else {
                        $("#TotalEstimateHours").text(0);
                        $("#TotalLayoutHours").text(0);
                        $("#TotalActualHours").text(0);
                        $("#TotalTimesheetHours").text(0);
                    }
                    $('#DivCMloader').hide();
                }
            });
        },

        eventMouseover: function (event, jsEvent, view) {
            if (event.description >= 0 && event.title > 0) {
                $(this).css('cursor', 'pointer');
                $(this).css('background-color', '#a5e0ef');
                $(this).css('border-width', '5');//f0f9ff- mockup color
                var dateString = event.start.format('YYYY-MM-DD')
                $(view.el[0]).find('.fc-day[data-date=' + dateString + ']').css('background-color', '#a5e0ef');
            }
        },
        eventMouseout: function (calEvent, jsEvent, view) {
            $(this).css('background-color', calEvent.backgroundColor);
            var dateString = calEvent.start.format('YYYY-MM-DD')
            $(view.el[0]).find('.fc-day[data-date=' + dateString + ']').css('background-color', calEvent.backgroundColor);
        },

        //dayClick: function (date, jsEvent, view) {
        //$(this).css('background-color', 'red');
        //window.location = '/CMDashboard/Dashboard?selectedDate=' + date.format();
        //},
        eventClick: function (info) {
            if (info.description >= 0 && info.title > 0) {
                //$.post("/CMDashboard/Dashboard", { selectedDate: info.start.format('YYYY-MM-DD') }, function (data) { window.location.href = data.url; });
                //window.location.href = 'http://' + $(location).attr('host') + '/MMI/CMDashboard/Dashboard?selectedDate=' + info.start.format('YYYY-MM-DD');

                window.location.href = cmurl + "/CMDashboard/Dashboard?selectedDate=" + info.start.format('YYYY-MM-DD');

            }
        },
        eventRender: function (event, element, view) {

            //alert(TodaysDate);
            var today = moment(Todays);
            var todayString = today.format("YYYY-MM-DD");
            $(view.el[0]).find('.fc-day[data-date=' + todayString + ']').css('background-color', 'yellow');

            var startDate = moment(Todays).add(1, 'days');
            var endDate = moment().add(15, 'days');
            for (var date = moment(startDate) ; date.diff(endDate) < 0; date.add(1, 'days')) {
                var dateString = date.format("YYYY-MM-DD");
                $(view.el[0]).find('.fc-day[data-date=' + dateString + ']').css('background-color', '#fef5e6');
            }

            //element.qtip(
            //{
            //    content: event.description
            //});
        },

        editable: false
    });
});

