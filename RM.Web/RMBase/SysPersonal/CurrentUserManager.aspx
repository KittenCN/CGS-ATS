<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrentUserManager.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.CurrentUserManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>个人信息设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            divresize(62);
            Setform();
            $("#table4").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
            $('#table4 input[type="checkbox"]').attr('disabled', 'disabled');
            $('#table2').hide();
            $('#table3').hide();
            $('#table4').hide();
        })
        //点击切换面板
        function panel(obj) {
            if (obj == 1) {
                $('#table1').show();
                $('#table2').hide();
                $('#table3').hide();
                $('#table4').hide();
            } else if (obj == 2) {
                $('#table1').hide();
                $("#table2").show();
                $('#table3').hide();
                $('#table4').hide();
            } else if (obj == 3) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').show();
                $('#table4').hide();
            } else if (obj == 4) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').hide();
                $('#table4').show();
            }
        }
        //附加信息表单赋值
        function Setform() {
            var strArray = new Array();
            var strArray1 = new Array();
            var item_value = $("#AppendProperty_value").val(); //后台返回值
            strArray = item_value.split(';');
            for (var i = 0; i < strArray.length; i++) {
                var item_value1 = strArray[i];
                strArray1 = item_value1.split('|');
                $("#" + strArray1[0]).text(strArray1[1]);
            }
        }
        /**修改密码**/
        function editpwd() {
            var url = "/RMBase/SysUser/UpdateUserPwd.aspx";
            top.openDialog(url, 'UpdateUserPwd', '修改登录密码', 400, 225, 50, 50);
        }
    </script>
    <style type="text/css">
        .frm th
        {
            border-right: 1px solid #ccc;
            padding-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--    获取附加信息值--%>
    <input id="AppendProperty_value" type="hidden" runat="server" />
    <div class="btnbartitle">
        <div>
            <%=_UserName.ToString() %>
            - 相关信息
        </div>
    </div>
    <div class="btnbarcontetn">
        <div>
            <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="menutab" style="vertical-align: bottom;">
                        <div id="tab0" class="Tabsel" onclick="GetTabClick(this);panel(1)">
                            基本信息</div>
                        <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);panel(2);">
                            附加信息</div>
                        <div id="tab2" class="Tabremovesel" onclick="GetTabClick(this);panel(3);">
                            有用角色</div>
                        <div id="tab3" class="Tabremovesel" onclick="GetTabClick(this);panel(4);">
                            有用权限</div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="div-body">
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th>
                    职工工号
                </th>
                <td colspan="">
                    <asp:Label ID="User_Code" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    职工姓名
                </th>
                <td colspan="">
                    <asp:Label ID="User_Name" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    登录账户
                </th>
                <td colspan="">
                    <asp:Label ID="User_Account" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    登录密码
                </th>
                <td colspan="">
                    *********** <a href="javascript:void(0);" title="修改登录密码" style="text-decoration: underline;
                        color: Blue;" onclick="editpwd()">密码修改</a>
                </td>
            </tr>
            <tr>
                <th>
                    职工性别
                </th>
                <td colspan="">
                    <asp:Label ID="User_Sex" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    电子邮件
                </th>
                <td colspan="">
                    <asp:Label ID="Email" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    创建时间
                </th>
                <td colspan="">
                    <asp:Label ID="CreateDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    职称
                </th>
                <td colspan="">
                    <asp:Label ID="Title" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    状态
                </th>
                <td colspan="">
                    <asp:Label ID="DeleteMark" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    系统样式
                </th>
                <td colspan="">
                    <asp:Label ID="Theme" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    备注描述
                </th>
                <td colspan="">
                    <asp:Label ID="User_Remark" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table id="table2" border="0" cellpadding="0" cellspacing="0" class="frm">
            <%=AppendHtml.ToString()%>
        </table>
        <table id="table3" class="grid">
            <thead>
                <tr>
                    <td style="width: 50px; text-align: center;">
                        序号
                    </td>
                    <td style="width: 150px;">
                        角色名称
                    </td>
                    <td>
                        角色描述
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 50px; text-align: center;">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="width: 150px;">
                                <%#Eval("Roles_Name")%>
                            </td>
                            <td>
                                <%#Eval("Roles_Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='3' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
                               }
                           } %>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="example" id="table4">
            <thead>
                <tr>
                    <td style="width: 203px; padding-left: 20px;">
                        URL菜单权限
                    </td>
                    <td style="width: 30px; text-align: center;">
                        图标
                    </td>
                    <td style="width: 20px; text-align: center;">
                        <label id="checkAllOff" title="全选">
                            &nbsp;</label>
                    </td>
                    <td>
                        操作按钮
                    </td>
                </tr>
            </thead>
            <tbody>
                <%=StrTree_Menu.ToString()%>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
