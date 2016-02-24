<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_WorkRecordTest.aspx.cs" Inherits="RM.Web.RMBase.frmATS_WorkRecordTest" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <link href="/Themes/Scripts/Plugins/fullcalendar.css" rel="stylesheet" />
    <link href="/Themes/Scripts/Plugins/jquery.ui.css" rel="stylesheet" />
    <link href="/Themes/Scripts/Plugins/style.default.css" rel="stylesheet" />

    <script src="/Themes/Scripts/Plugins/jquery-1.7.min.js"></script>
    <script src="/Themes/Scripts/Plugins/jquery-ui-1.8.16.custom.min.js"></script>
    <script src="/Themes/Scripts/Plugins/fullcalendar.min.js"></script>
    <script src="/Themes/Scripts/Plugins/fullcalendar.js"></script>
    <script>
        $(document).ready(function() {
	        //$('#calendar').fullCalendar({
	        //    height : window.innerHeight-20,
	        //    windowResize: function(view) {
	        //        $('#calendar').fullCalendar('option', 'height', window.innerHeight-20);
	        //    },
	            //defaultDate: '2016-01-12',
	            //weekMode: 'liquid',
	            //header: {
	            //    left: 'prev,next today',
	            //    center: 'title',
	            //    right: 'month,agendaWeek,agendaDay'
	            //},   
	            //editable: false,
	            //eventLimit: true, // allow "more" link when too many events
	            //events: 'hanATS_WorkRecord.ashx?EmpID=' + $("#EmpID").val(),
		    //});		
        });
    </script>
</head>
<body>   
<div id='calendar'>
</div> 
</body>
</html>
