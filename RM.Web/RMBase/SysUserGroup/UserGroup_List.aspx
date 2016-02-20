<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGroup_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysUserGroup.UserGroup_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户组信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#dnd-example", $(window).height() - 91);
            GetClickTableValue();
        })
        /**
        获取table TD值
        主键ID
        column:列名
        **/
        var UserGroup_ID = '';
        var UserGroup_Name = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 8) {
                        UserGroup_ID = $(this).text();
                    }
                    if (i == 0) {
                        UserGroup_Name = $(this).text();
                    }
                });
            });
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        }
        //新增
        function add() {
            var url = "/RMBase/SysUserGroup/UserGroup_Form.aspx?ParentId=" + UserGroup_ID;
            top.openDialog(url, 'UserGroup_Form', '用户组信息 - 添加', 500, 260, 50, 50);
        }
        //编辑
        function edit() {
            var key = UserGroup_ID;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysUserGroup/UserGroup_Form.aspx?key=" + key;
                top.openDialog(url, 'UserGroup_Form', '用户组信息 - 编辑', 500, 260, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = UserGroup_ID;
            if (IsDelData(key)) {
                var isExistparm = 'action=IsExist&tableName=Base_UserGroup&pkName=ParentId&pkVal=' + key;
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=用户组管理&tableName=Base_UserGroup&pkName=UserGroup_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        }
        //分配成员
        function allotMember() {
            if (IsEditdata(UserGroup_ID)) {
                var url = "/RMBase/SysUserGroup/UserGroupSet.aspx?UserGroup_ID=" + UserGroup_ID + '&UserGroup_Name=' + escape(UserGroup_Name);
                top.openDialog(url, 'UserGroupSet', '用户组成员信息 - 设置', 750, 480, 50, 50);
            }
        }
        //分配权限
        function allotAuthority() {
            if (IsEditdata(UserGroup_ID)) {
                var url = "/RMBase/SysUserGroup/AllotAuthority_Form.aspx?UserGroup_ID=" + UserGroup_ID + '&UserGroup_Name=' + escape(UserGroup_Name);
                Urlhref(url);
            }
        }
        //详细信息
        function detail() {
            if (IsEditdata(UserGroup_ID)) {
                var url = "/RMBase/SysUserGroup/UserGroup_Info.aspx?UserGroup_ID=" + UserGroup_ID + '&UserGroup_Name=' + escape(UserGroup_Name);
                Urlhref(url);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            用户组信息
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="float: left;">
        </div>
        <div style="text-align: right">
            <uc1:LoadButton ID="LoadButton1" runat="server" />
        </div>
    </div>
    <div class="div-body">
        <table class="example" id="dnd-example">
            <thead>
                <tr>
                    <td style="width: 180px; padding-left: 20px;">
                        用户组名称
                    </td>
                    <td style="width: 60px; text-align: center;">
                        用户组编号
                    </td>
                    <td style="width: 60px; text-align: center;">
                        显示顺序
                    </td>
                    <td style="width: 120px;text-align: center;">
                        创建用户
                    </td>
                    <td style="width: 120px;text-align: center;">
                        创建时间
                    </td>
                    <td style="width: 120px;text-align: center;">
                        修改用户
                    </td>
                    <td style="width: 120px;text-align: center;">
                        修改时间
                    </td>
                    <td>
                        描述信息
                    </td>
                </tr>
            </thead>
            <tbody>
                <%=str_tableTree.ToString()%>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
