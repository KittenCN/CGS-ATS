<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_LeaveLeft.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_LeaveLeft" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>休假信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            divresize(29);
            treeAttrCss();
            GetClickValue();
        })
        //点击获取部门ID
        function GetClickValue() {
            $(".strTree li div").click(function () {
                var id = "";
                //子目录 
                $(this).find("span").each(function () {
                    if ($(this).html() != "") {
                        id += "" + $(this).html() + ",";
                    }
                });
                id = id.substr(0, id.length - 1);
                var path = 'frmATS_LeaveList.aspx?key=' + id;
                window.parent.frames["target_right"].location = path;
                Loading(true);
            })
        }
    </script>
</head>
<body>
     <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            休假信息
        </div>
    </div>
    <div class="div-body">
        <ul class="strTree">
                <%=strHtml.ToString()%>

        </ul>
    </div>
    </form>
</body>
</html>
