<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_Left.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UserInfo_Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户信息</title>
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
            GetClickValue();
        })
        //点击获取部门ID
        function GetClickValue() {
            $(".strTree li div").click(function () {
                var id = "";
                //子目录 
                $(this).parent().find("span").each(function () {
                    if ($(this).html() != "") {
                        id += "'" + $(this).html() + "',";
                    }
                });
                id = id.substr(0, id.length - 1);
                var path = 'UserInfo_List.aspx?Organization_ID=' + id;
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
            组织机构
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
