<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recyclebin_Index.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.Recyclebin_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回收站</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("html").css("overflow", "hidden");
            $("body").css("overflow", "hidden");
            iframeresize();
            Loading(true);
            $("#target_right").attr("src", "Recyclebin_List.aspx");
            $("#target_left").attr("src", "Recyclebin_Left.aspx");
        })
        /**自应高度**/
        function iframeresize() {
            resizeU();
            $(window).resize(resizeU);
            function resizeU() {
                var iframeMain = $(window).height();
                $("#iframeMainContent").height(iframeMain);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="iframeMainContent">
        <div class="iframeleft">
            <iframe id="target_left" name="target_left" scrolling="auto" frameborder="0" scrolling="yes"
                width="100%" height="100%"></iframe>
        </div>
        <div class="iframeContent">
            <iframe id="target_right" name="target_right" scrolling="auto" frameborder="0" scrolling="yes"
                width="100%" height="100%"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
