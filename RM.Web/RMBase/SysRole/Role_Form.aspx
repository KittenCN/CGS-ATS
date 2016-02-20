<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Form.aspx.cs" Inherits="RM.Web.RMBase.SysRole.Role_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统角色设置表单</title>
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
            treeAttrCss();
            $("#UserInfolist").height(270);
            $("#UserInfolistright").height(273);
            $("#UserInfolist").hide();
            //SubmitCheckForRC();
        })
        //双击添加员工
        function addUserInfo(userName, userID, Organization_Name) {
            var IsExist = true;
            $("#table1 tbody tr").each(function (i) {
                if ($(this).find('td:eq(0)').html() == userName) {
                    IsExist = false;
                    return false;
                }
            })
            if (IsExist == true) {
                $("#table1 tbody").append("<tr ondblclick='$(this).remove()'><td>" + userName + "</td><td>" + Organization_Name + "</td><td  style='display:none'>" + userID + "</td></tr>");
                publicobjcss();
            } else {
                showWarningMsg("【" + userName + "】员工已经存在");
            }
        }
        //面板切换
        function TabPanel(id) {
            if (id == 1) {
                $(".frm").show();
                $("#UserInfolist").hide();
            } else if (id == 2) {
                $(".frm").hide();
                $("#UserInfolist").show();
                //固定表头
                FixedTableHeader("#table1", $(window).height() - 127);
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            var item_value = "";
            $("#table1 tbody tr").each(function (i) {
                item_value += $(this).find('td:eq(2)').html() + ",";
            })
            $("#User_ID_Hidden").val(item_value);
            if (!confirm('确认要保存此操作吗？')) {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="User_ID_Hidden" type="hidden" runat="server" />
    <div class="frmtop">
        <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td id="menutab" style="vertical-align: bottom;">
                    <div id="tab0" class="Tabsel" onclick="GetTabClick(this);TabPanel(1)">
                        基本信息</div>
                    <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);TabPanel(2)">
                        角色成员</div>
                </td>
            </tr>
        </table>
    </div>
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                角色名称：
            </th>
            <td>
                <input id="Roles_Name" runat="server" type="text" class="txt" datacol="yes" err="角色名称"
                    checkexpession="NotNull" style="width: 85%" />
            </td>
        </tr>
        <tr>
            <th>
                节点位置：
            </th>
            <td>
                <select id="ParentId" class="select" runat="server" style="width: 86.5%">
                </select>
            </td>
        </tr>
        <tr>
            <th>
                显示顺序：
            </th>
            <td>
                <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                    checkexpession="Num" style="width: 85%" />
            </td>
        </tr>
        <tr>
            <th>
                角色描述：
            </th>
            <td>
                <textarea id="Roles_Remark" class="txtRemark" runat="server" style="width: 85.5%;
                    height: 170px;"></textarea>
            </td>
        </tr>
    </table>
    <div id="UserInfolist">
        <div id="UserInfolistleft" style="float: left; width: 48.6%; border: 1px solid #ccc;
            margin-right: 1px;">
            <div class="box-title" style="height: 27px;">
                所有成员;<span style="color: Blue;">双击添加</span>
            </div>
            <div class="div-overflow" id="allUserInfo" style="overflow-x: hidden; overflow-y: scroll;
                padding-bottom: 5px; height: 240px;">
                <ul class="strTree">
                    <%=str_allUserInfo %>
                </ul>
            </div>
        </div>
        <div id="UserInfolistright" style="float: left; width: 50%; border: 1px solid #ccc;
            border-top: 0px solid #ccc;">
            <div class="div-overflow" id="selectedUserInfo" style="padding-bottom: 5px; height: 250px;">
                <table id="table1" class="grid">
                    <thead>
                        <tr>
                            <td>
                                已选成员;<span style="color: Red;">双击移除</span>
                            </td>
                            <td>
                                所属部门
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <%=str_seleteUserInfo%>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckValid();"
            OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a id="Close" class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span
            class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
    </form>
</body>
</html>
