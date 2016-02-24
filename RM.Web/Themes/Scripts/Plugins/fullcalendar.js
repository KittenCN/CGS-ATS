

jQuery(document).ready(function () {
    /* initialize the external events */
    jQuery('#external-events div.external-event').each(function () {

        // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
        // it doesn't need to have a start or end
        var eventObject = {
            title: jQuery.trim(jQuery(this).text()) // use the element's text as the event title
        };

        // store the Event Object in the DOM element so we can get to it later
        jQuery(this).data('eventObject', eventObject);

        // make the event draggable using jQuery UI
        jQuery(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });


   
    /* 初始化calendar */
    jQuery('#calendar').fullCalendar({
        header: {
            left: 'month,agendaWeek,agendaDay',
            center: 'title',
            right: 'today, prev, next'
        },
        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
        dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
        today: ["今天"],
        firstDay: 1,
        buttonText: {
            prev: '&laquo;',
            next: '&raquo;',
            prevYear: '&nbsp;&lt;&lt;&nbsp;',
            nextYear: '&nbsp;&gt;&gt;&nbsp;',
            today: '今天',
            month: '月',
            week: '周',
            day: '日'
        },
        viewDisplay: function (view) {
            //动态把数据查出，按照月份动态查询
            var viewStart = $.fullCalendar.formatDate(view.start, "yyyy-MM-dd HH:mm:ss");
            var viewEnd = $.fullCalendar.formatDate(view.end, "yyyy-MM-dd HH:mm:ss");
            $("#calendar").fullCalendar('removeEvents');
            $.post("../SysATS/hanATS_WorkRecord.ashx?start=" + viewStart + "&end=" + viewEnd + "&empid=" + viewEmpID, { start: viewStart, end: viewEnd,empid:viewEmpID }, function (data) {
                var resultCollection = jQuery.parseJSON(data);
                $.each(resultCollection, function (index, term) {
                    $("#calendar").fullCalendar('renderEvent', term, true);
                });

            }); //把从后台取出的数据进行封装以后在页面上以fullCalendar的方式进行显示
        },

       
        dayClick: function (date, allDay, jsEvent, view) {
        },
        loading: function (bool) {
            if (bool) $('#loading').show();
            else $('#loading').hide();
        },
        

        //#region 数据绑定上去后添加相应信息在页面上(一开始加载数据时运行)
        eventAfterRender: function (event, element, view) {

            var fstart = $.fullCalendar.formatDate(event.start, "HH:mm");
            var fend = $.fullCalendar.formatDate(event.end, "HH:mm");
            var confbg = '<span class="fc-event-bg"></span>';
            if (view.name == "month") {//按月份                
                var evtcontent = '<div class="fc-event-vert"><a>';
                evtcontent = evtcontent + confbg;
                //evtcontent = evtcontent + '<span class="fc-event-titlebg">' + fstart + " - " + fend  + event.fullname + '</span>';   
                evtcontent = evtcontent + '<span class="fc-event-titlebg">' + event.fullname + '</span>';
                element.html(evtcontent);
            } else if (view.name == "agendaWeek") {//按周

                var evtcontent = '<a>';
                evtcontent = evtcontent + confbg;
                evtcontent = evtcontent + '<span class="fc-event-time">' + fstart + "-" + fend + '</span>';
                evtcontent = evtcontent + '<span>' + event.fullname + '</span>';
                element.html(evtcontent);
            } else if (view.name == "agendaDay") {//按日

                var evtcontent = '<a>';
                evtcontent = evtcontent + confbg;
                evtcontent = evtcontent + '<span class="fc-event-time">' + fstart + " - " + fend + '</span>';
                element.html(evtcontent);
            }
        },
        //#endregion

        //#region 鼠标放上去显示信息
        eventMouseover: function (calEvent, jsEvent, view) {           
            //var fstart = $.fullCalendar.formatDate(calEvent.start, "yyyy/MM/dd HH:mm");
            //var fend = $.fullCalendar.formatDate(calEvent.end, "yyyy/MM/dd HH:mm");
            //$(this).attr('title', fstart + " - " + fend + " " + calEvent.fullname);
            $(this).attr('title', calEvent.fullname);
            $(this).css('font-weight', 'normal');
            //            $(this).tooltip({
            //                effect: 'toggle',
            //                cancelDefault: true
            //            });
        },
        eventClick: function (event) {             
          
        },
        events: [],
        //#endregion

        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (date, allDay) { // this function is called when something is dropped

            // retrieve the dropped element's stored Event Object
            var originalEventObject = jQuery(this).data('eventObject');

            // we need to copy it, so that multiple events don't have a reference to the same object
            var copiedEventObject = jQuery.extend({}, originalEventObject);

            // assign it the date that was reported
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;

            // render the event on the calendar
            // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
            jQuery('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            // is the "remove after drop" checkbox checked?

            jQuery(this).remove();

        }
    });



    ///// SWITCHING LIST FROM 3 COLUMNS TO 2 COLUMN LIST /////
    function reposTitle() {
        if (jQuery(window).width() < 450) {
            if (!jQuery('.fc-header-title').is(':visible')) {
                if (jQuery('h3.calTitle').length == 0) {
                    var m = jQuery('.fc-header-title h2').text();
                    jQuery('<h3 class="calTitle">' + m + '</h3>').insertBefore('#calendar table.fc-header');
                }
            }
        } else {
            jQuery('h3.calTitle').remove();
        }
    }
    reposTitle();

    ///// ON RESIZE WINDOW /////
    jQuery(window).resize(function () {
        reposTitle();
    });



});
