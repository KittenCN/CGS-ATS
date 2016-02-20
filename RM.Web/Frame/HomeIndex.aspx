<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeIndex.aspx.cs" Inherits="RM.Web.Frame.HomeIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统首页</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#BeautifulGreetings").text(BeautifulGreetings());
        })
        /**修改密码**/
        function editpwd() {
            var url = "/RMBase/SysUser/UpdateUserPwd.aspx";
            top.openDialog(url, 'UpdateUserPwd', '修改登录密码', 400, 225, 50, 50);
        }
        //新增快捷功能
        function add_HomeShortcut() {
            var url = "/RMBase/SysPersonal/HomeShortcut_Form.aspx";
            top.openDialog(url, 'Menu_Form', '首页快捷功能信息 - 添加', 450, 300, 50, 50);
        }
        //快捷功能点击事件
        function ClickShortcut(url, title, Target) {
            top.NavMenu(url, title);
        }
    </script>
    <style type="text/css">
        .shortcuticons
        {
            float: left;
            border: solid 1px #fff;
            width: 62px;
            height: 55px;
            margin: 5px;
            padding: 5px;
            cursor: pointer;
            vertical-align: middle;
            text-align: center;
            word-break: keep-all;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .shortcuticons:hover
        {
            color: #FFF;
            border: solid 1px #3399dd;
            background: #2288cc;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#33bbee', endColorstr='#2288cc');
            background: linear-gradient(top, #33bbee, #2288cc);
            background: -moz-linear-gradient(top, #33bbee, #2288cc);
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#33bbee), to(#2288cc));
            text-shadow: -1px -1px 1px #1c6a9e;
            width: 62px;
            height: 55px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="box">
        <div class="box-title">
            <img src="/Themes/Images/sun_2.png" alt="" width="20" height="20" />
            <label id="BeautifulGreetings">
            </label>
            <%=_UserName.ToString()%>，Welcome to CGS Online ATS Technology Preview
        </div>
        <div class="box-content">
            <%--快捷功能--%>
            <%=sbHomeShortcouHtml.ToString() %>
            <br />
            <br />
            <br />
            <br />
            <a href="javascript:void(0)" onclick="add_HomeShortcut()" class="button green"><span
                class="icon-botton" style="background: url('/Themes/images/world_add.png') no-repeat scroll 0px 4px;">
            </span>添加新的快捷功能</a>
        </div>
    </div>
    <div class="blank10">
    </div>
    <div class="box">
        <div class="box-title">
            <img src="/Themes/Images/milestone.png" alt="" width="20" height="20" />当前登录情况
        </div>
        <div class="box-content">
            <%=Login_InfoHtml%>
            <br />
            <img src="/Themes/Images/exclamation_octagon_fram.png" style="vertical-align: middle;
                margin-bottom: 3px;" width="16" height="16" alt="tip" />
            提示：为了账号的安全，如果上面的登录情况不正常，建议您马上 <a href="javascript:void(0);" title="修改登录密码" style="text-decoration: underline;
                color: Blue;" onclick="editpwd()">密码修改</a>
        </div>
    </div>
    <div class="blank10">
    </div>
    </form>
</body>
</html>
