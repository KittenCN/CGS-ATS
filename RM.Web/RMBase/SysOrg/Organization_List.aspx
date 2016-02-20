<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Organization_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysOrg.Organization_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>组织机构部门</title>
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
        var Organization_ID = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 9) {
                        Organization_ID = $(this).text();
                    }
                });
            });
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        }
        //新增
        function add() {
            var url = "/RMBase/SysOrg/Organization_Form.aspx?ParentId=" + Organization_ID;
            top.openDialog(url, 'Organization_Form', '部门信息 - 添加', 700, 335, 50, 50);
        }
        //编辑
        function edit() {
            var key = Organization_ID;
            if (IsEditdata(key)) {
                var url = "/RMBase/SysOrg/Organization_Form.aspx?key=" + key;
                top.openDialog(url, 'Organization_Form', '部门信息 - 编辑', 700, 335, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = Organization_ID;
            if (IsDelData(key)) {
                var isExistparm = 'action=IsExist&tableName=Base_Organization&pkName=ParentId&pkVal=' + key;
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=部门管理&tableName=Base_Organization&pkName=Organization_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            部门信息列表
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
                    <td style="width: 120px; padding-left: 20px;">
                        组织机构
                    </td>
                    <td style="width: 50px; text-align: center;">
                        部门编号
                    </td>
                    <td style="width: 100px; text-align: center;">
                        主负责人
                    </td>
                    <td style="width: 100px; text-align: center;">
                        外线电话
                    </td>
                    <td style="width: 100px; text-align: center;">
                        内线电话
                    </td>
                    <td style="width: 100px; text-align: center;">
                        传真号码
                    </td>
                    <td style="width: 50px; text-align: center;">
                        邮政区号
                    </td>
                    <td style="width: 50px; text-align: center;">
                        显示顺序
                    </td>
                    <td>
                        所在地址
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
