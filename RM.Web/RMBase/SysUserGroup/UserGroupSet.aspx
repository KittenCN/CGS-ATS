<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGroupSet.aspx.cs" Inherits="RM.Web.RMBase.SysUserGroup.UserGroupSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户组成员设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            UserGroupInfo();
            GetUserList();
            treeAttrCss();
        })
        //用户列表
        function GetUserList() {
            var UserGroup_ID = '<%=_UserGroup_ID %>';
            var Searchwhere = $("#Searchwhere").val();
            var txt_Search = $("#txt_Search").val();
            var parm = 'action=UserList&Searchwhere=' + escape(Searchwhere) + '&txt_Search=' + escape(txt_Search) + '&UserGroup_ID=' + UserGroup_ID;
            $("#htmlrp_Item").empty();
            getAjax('UserGroup.ashx', parm, function (rs) {
                var json = eval("(" + rs + ")");
                var rowsum = json.UserGroupList.length;
                if (rowsum > 0) {
                    for (var i = 0; i < rowsum; i++) {
                        var GroupList = json.UserGroupList[i];
                        $("#htmlrp_Item").append('<tr>');
                        $("#htmlrp_Item").append('<td style="width: 20px; text-align: left;"><input type="checkbox" value=' + GroupList.USER_ID + ' name="checkbox" /></td>');
                        $("#htmlrp_Item").append('<td style="width: 60px; text-align: center;">' + GroupList.USER_CODE + '</td>');
                        $("#htmlrp_Item").append('<td style="width: 80px; text-align: center;">' + GroupList.USER_NAME + '</td>');
                        $("#htmlrp_Item").append('<td style="width: 100px; border-right: 0px; text-align: center;">' + GroupList.USER_ACCOUNT + '</td>');
                        $("#htmlrp_Item").append('</tr>');
                    }
                } else {
                    $("#htmlrp_Item").append("<tr><td colspan='4' style='color:red;text-align:center;border-right: 0px;'>没有找到您要的相关数据！</td></tr>");
                }
            });
        }
        //用户组树
        function UserGroupInfo() {
            var UserGroup_ID = '<%=_UserGroup_ID %>';
            var parm = 'action=UserGroupInfo&UserGroup_ID=' + UserGroup_ID;
            $("#ulstrTree").empty();
            getAjax('UserGroup.ashx', parm, function (rs) {
                $("#ulstrTree").append(rs);
            });
            treeCss();
        }
        //添加成员到用户组
        function addMember() {
            var UserGroup_ID = '<%=_UserGroup_ID %>';
            var key = CheckboxValue();
            if (key != undefined || key != "") {
                var parm = 'action=UserGroupaddMember&UserGroup_ID=' + UserGroup_ID + '&User_ID=' + key;
                getAjax('UserGroup.ashx', parm, function (rs) {
                    if (parseInt(rs) > 0) {
                        showTipsMsg("添加用户到组成功！", 2000, 4);
                        UserGroupInfo();
                        GetUserList();
                    }
                    else {
                        showTipsMsg("操作失败，请稍后重试！", 4000, 5);
                    }
                });
            } else {
                showWarningMsg("请选中用户列表员工");
            }
        }
        //删除用户组里面成员
        function DeleteMember(id) {
            var delparm = 'action=Delete&tableName=Base_UserInfoUserGroup&pkName=UserInfoUserGroup_ID&pkVal=' + id;
            DeleteData('/Ajax/Common_Ajax.ashx', delparm)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="UserInfolist" style="height: 470px;">
        <div id="UserInfolistleft" style="float: left; width: 45%; border: 1px solid #ccc;">
            <div class="box-title" style="height: 27px;">
                用户列表
            </div>
            <div style="padding-top: 5px;">
                <select id="Searchwhere" class="select" runat="server" style="float: left;margin-top: -2px;">
                    <option value="User_Code">工号</option>
                    <option value="User_Account">账户</option>
                    <option value="User_Name">姓名</option>
                </select>
                <input type="text" id="txt_Search" class="txtSearch SearchImg" onkeyup="GetUserList()"
                    runat="server" style="width: 200px;" />
            </div>
            <div style="height: 30px; line-height: 30px;">
                用户列表(仅显示符合条件的前50条数据)
            </div>
            <div class="div-overflow" id="allUserInfo" style="padding-bottom: 5px; height: 380px;">
                <table id="table1" class="grid">
                    <thead>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                    &nbsp;</label>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                职工工号
                            </td>
                            <td style="width: 80px; text-align: center;">
                                用户姓名
                            </td>
                            <td style="width: 100px; border-right: 0px; text-align: center;">
                                登录账户
                            </td>
                        </tr>
                    </thead>
                    <tbody id="htmlrp_Item">
                    </tbody>
                </table>
            </div>
        </div>
        <div style="float: left; width: 8%; text-align: center; padding-top: 200px;">
            <a onclick="addMember()" class="button green" title="添加用户到组"><span class="icon-botton"
                style="background: url('/Themes/images/arrow_right.png') no-repeat scroll 5px 4px;">
            </span></a>
        </div>
        <div id="UserInfolistright" style="float: left; width: 45%; border: 1px solid #ccc;">
            <div class="box-title" style="height: 27px;">
                用户组成员;<span style="color: Red;">双击移除成员</span>
            </div>
            <div class="div-overflow" id="selectedUserInfo" style="padding-bottom: 5px; height: 440px;">
                <ul class="strTree">
                    <li>
                        <div class="node">
                            <%=_UserGroup_Name%></div>
                        <ul id="ulstrTree">
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
