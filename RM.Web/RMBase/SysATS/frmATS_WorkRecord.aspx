<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_WorkRecord.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_WorkRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>工作日程安排</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href='/Themes/fullcalendar/fullcalendar.css' rel='stylesheet' />
    <link href='/Themes/fullcalendar/fullcalendar.print.css' rel='stylesheet' media='print' />
    <script src='/Themes/fullcalendar/lib/moment.min.js'></script>
    <script src='/Themes/fullcalendar/lib/jquery.min.js'></script>
    <script src='/Themes/fullcalendar/fullcalendar.min.js'></script>
    <script src='/Themes/fullcalendar/fullcalendar.js'></script>
    <script>
	$(document).ready(function() {
	    $('#calendar').fullCalendar({
	        height : window.innerHeight-20,
	        windowResize: function(view) {
	            $('#calendar').fullCalendar('option', 'height', window.innerHeight-20);
	        },
	        //defaultDate: '2016-01-12',
	        weekMode: 'liquid',
	        header: {
	            left: 'prev,next today',
	            center: 'title',
	            right: 'month,agendaWeek,agendaDay'
	        },   
	        editable: true,
	        eventLimit: true, // allow "more" link when too many events
	        events: 'hanATS_WorkRecord.ashx?EmpID=' + $("#EmpID").val(),
		});		
	});
	function search() {
	}
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="text-align: left;">
            <input id="EmpID" name="EmpID" runat="server"  type="text" list="Emplist" style="width: 200px"/>
            <datalist id="Emplist" runat="server"></datalist>
            <asp:Button ID="btnSearch" Text="Search" runat="server"  OnClientClick="search()" />
        </div>
        <div id="calendar" runat="server">

        </div>
    </div>
    </form>
</body>
</html>
