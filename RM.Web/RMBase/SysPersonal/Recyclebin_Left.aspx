<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recyclebin_Left.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.Recyclebin_Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回收站原位置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            divresize(29);
            treeAttrCss();
        })
        //点击事件
        function GetRecyclebin_Name(Recyclebin_Name) {
            var path = 'Recyclebin_List.aspx?Recyclebin_Name=' + escape(Recyclebin_Name);
            window.parent.frames["target_right"].location = path;
            Loading(true);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            回收站原位置
        </div>
    </div>
    <div class="div-body">
        <ul class="strTree">
            <li>
                <div>
                    所属业务
                </div>
                <%=strHtml.ToString()%>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
