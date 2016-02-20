<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu_List.aspx.cs" Inherits="RM.Web.RMBase.SysMenu.Menu_List1" %>

<%@ Register Src="/UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单导航设置</title>
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
        var Menu_Id = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 6) {
                        Menu_Id = $(this).text();
                    }
                });
            });
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        }
        //新增
        function add() {
            var url = "/RMBase/SysMenu/Menu_Form.aspx?ParentId=" + Menu_Id;
            top.openDialog(url, 'Menu_Form', '导航菜单信息 - 添加', 450, 305, 50, 50);
        }
        //编辑
        function edit() {
            var key = Menu_Id;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysMenu/Menu_Form.aspx?key=" + key;
                top.openDialog(url, 'Menu_Form', '导航菜单信息 - 编辑', 450, 305, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = Menu_Id;
            if (IsDelData(key)) {
                var isExistparm = 'action=IsExist&tableName=Base_SysMenu&pkName=ParentId&pkVal=' + key;
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=导航菜单&tableName=Base_SysMenu&pkName=Menu_Id&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        }
        //分配按钮
        function allotButton() {
            var key = Menu_Id;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysMenu/AllotButton_Form.aspx?key=" + key;
                top.openDialog(url, 'AllotButton_Form', '导航菜单信息 - 分配按钮', 580, 370, 50, 50);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            导航菜单信息 &nbsp;&nbsp;<span style="color: Red;">注：该功能谨慎使用！</span>
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
                    <td style="width: 230px; padding-left: 20px;">
                        菜单名称
                    </td>
                    <td style="width: 30px; text-align: center;">
                        图标
                    </td>
                    <td style="width: 60px; text-align: center;">
                        类型
                    </td>
                    <td style="width: 60px; text-align: center;">
                        连接目标
                    </td>
                    <td style="width: 60px;">
                        显示顺序
                    </td>
                    <td>
                        连接地址
                    </td>
                </tr>
            </thead>
            <tbody>
                <%=TableTree_Menu.ToString()%>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
