<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_List.aspx.cs" Inherits="RM.Web.RMBase.SysRole.Role_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统角色设置</title>
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
        var Roles_ID = '';
        var Roles_Name = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 8) {
                        Roles_ID = $(this).text();
                    }
                    if (i == 0) {
                        Roles_Name = $(this).text();
                    }
                });
            });
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        }
        //新增
        function add() {
            var url = "/RMBase/SysRole/Role_Form.aspx?ParentId=" + Roles_ID;
            top.openDialog(url, 'Role_Form', '系统角色信息 - 添加', 600, 355, 50, 50);
        }
        //编辑
        function edit() {
            var key = Roles_ID;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysRole/Role_Form.aspx?key=" + key;
                top.openDialog(url, 'Role_Form', '系统角色信息 - 编辑', 600, 355, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = Roles_ID;
            if (IsDelData(key)) {
                var isExistparm = 'action=IsExist&tableName=Base_Roles&pkName=ParentId&pkVal=' + key;
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=角色管理&tableName=Base_Roles&pkName=Roles_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        }
        //分配权限
        function allotAuthority() {
            var key = Roles_ID;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysRole/AllotAuthority_Form.aspx?key=" + key + '&Roles_Name=' + escape(Roles_Name);
                Urlhref(url);
            }
        }
        //详细信息
        function detail() {
            var key = Roles_ID;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysRole/Role_Info.aspx?key=" + key + '&Roles_Name=' + escape(Roles_Name);
                Urlhref(url);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            系统角色信息
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
                        角色名称
                    </td>
                    <td style="width: 60px; text-align: center;">
                        角色状态
                    </td>
                    <td style="width: 60px; text-align: center;">
                        显示顺序
                    </td>
                    <td style="width: 120px; text-align: center;">
                        创建用户
                    </td>
                    <td style="width: 120px; text-align: center;">
                        创建时间
                    </td>
                    <td style="width: 120px; text-align: center;">
                        修改用户
                    </td>
                    <td style="width: 120px; text-align: center;">
                        修改时间
                    </td>
                    <td>
                        角色描述
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
