<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysLog_Content.aspx.cs"
    Inherits="RM.Web.RMBase.SysLog.SysLog_Content" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>异常日志详细</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize();
            $("#txtLog").css("height", $(window).height() - 35).css("width", $(window).width() - 7)
        })
        /**自应表格高度**/
        function divresize() {
            resizeU();
            $(window).resize(resizeU);
            function resizeU() {
                $("#divtxt").css("height", $(window).height() - 30);
            }
        }
        //提示
        function Alert_Ok() {
            showTipsMsg("清空成功！", 2000, 4);
            $("#txtLog").val("");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div style="float: left">
            <%=_FileName%>
            - 记事本
        </div>
        <div style="text-align: right">
            <asp:LinkButton ID="hlkempty" ToolTip="清空当前数据" Style="cursor: pointer; text-decoration: underline;
                color: Blue;" runat="server" OnClick="hlkempty_Click1" OnClientClick="return confirm('确认要清空当前数据吗？')">清空数据</asp:LinkButton>
        </div>
    </div>
    <div id="divtxt" class="div-body">
        <textarea id="txtLog" readonly="readonly" runat="server" style="border: 0px solid #A8A8A8;"></textarea>
    </div>
    </form>
</body>
</html>
