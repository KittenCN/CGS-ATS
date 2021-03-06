﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UserInfo_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Info Form</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            Setform();
            treeAttrCss();
            $('#table2').hide();
            $('#table3').hide();
            $('#table4').hide();
            $('#table5').hide();
            $('#table6').hide();
            ChekOrgClick();
        })
        //点击切换面板
        var IsFixedTableLoad = 1;
        function panel(obj) {
            if (obj == 1) {
                $('#table1').show();
                $('#table2').hide();
                $('#table3').hide();
                $('#table4').hide();
                $('#table5').hide();
                $('#table6').hide();
            } else if (obj == 2) {
                $('#table1').hide();
                $("#table2").show();
                $('#table3').hide();
                $('#table4').hide();
                $('#table5').hide();
                $('#table6').hide();
            } else if (obj == 3) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').show();
                $('#table4').hide();
                $('#table5').hide();
                $('#table6').hide();
            } else if (obj == 4) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').hide();
                $('#table4').show();
                $('#table5').hide();
                $('#table6').hide();
            } else if (obj == 5) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').hide();
                $('#table4').hide();
                $('#table5').show();
                $('#table6').hide();
            }
            else if (obj == 6) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').hide();
                $('#table4').hide();
                $('#table5').hide();
                $('#table6').show();
                $("#dnd-example").treeTable({
                    initialState: "expanded" //collapsed 收缩 expanded展开的
                });
                if (IsFixedTableLoad == 1) {
                    FixedTableHeader("#dnd-example", $(window).height() - 105);
                    IsFixedTableLoad = 0;
                }
            }
        }
        //附加信息表单赋值
        function Setform() {
            var pk_id = GetQuery('key');
            if (IsNullOrEmpty(pk_id)) {
                var strArray = new Array();
                var strArray1 = new Array();
                var item_value = $("#AppendProperty_value").val(); //后台返回值
                strArray = item_value.split(';');
                for (var i = 0; i < strArray.length; i++) {
                    var item_value1 = strArray[i];
                    strArray1 = item_value1.split('|');
                    $("#" + strArray1[0]).val(strArray1[1]);
                }
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            if (!IsNullOrEmpty(ChekOrgVale)) {
                showWarningMsg("Select The Dept.！");
                return false;
            }
            var item_value = '';
            $("#AppendProperty_value").empty;
            $("#table2 tr").each(function (r) {
                $(this).find('td').each(function (i) {
                    var pk_id = $(this).find('input,select').attr('id');
                    if ($(this).find('input,select').val() != "" && $(this).find('input,select').val() != "==请选择==" && $(this).find('input,select').val() != undefined) {
                        item_value += pk_id + "|" + $(this).find('input,select').val() + ";";
                    }
                });
            });
            $("#AppendProperty_value").val(item_value);
            $("#checkbox_value").val(CheckboxValue())
            if (!confirm('Save？')) {
                return false;
            }
        }
        //验证所属部门必填
        var ChekOrgVale = "";
        function ChekOrgClick() {
            var pk_id = GetQuery('key');
            if (IsNullOrEmpty(pk_id)) {
                ChekOrgVale = 1;
            }
            $("#table3 [type = checkbox]").click(function () {
                ChekOrgVale = "";
                if ($(this).val() != "") {
                    if ($(this).attr("checked") == "checked") {
                        ChekOrgVale = 1;
                    };
                }
            })
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 所有打勾复选框值--%>
    <input id="checkbox_value" type="hidden" runat="server" />
    <%--获取附加信息值--%>
    <input id="AppendProperty_value" type="hidden" runat="server" />
    <div class="frmtop">
        <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td id="menutab" style="vertical-align: bottom;">
                    <div id="tab0" class="Tabsel" onclick="GetTabClick(this);panel(1)">
                        Base Info</div>
                    <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);panel(2);">
                        Other Info</div>
                    <div id="tab2" class="Tabremovesel" onclick="GetTabClick(this);panel(3);">
                        Dept.</div>
<%--                    <div id="tab3" class="Tabremovesel" onclick="GetTabClick(this);panel(4);">
                        所属角色</div>
                    <div id="tab4" class="Tabremovesel" onclick="GetTabClick(this);panel(5);">
                        所属工作组</div>--%>
                    <div id="tab5" class="Tabremovesel" onclick="GetTabClick(this);panel(6);">
                        Permissions</div>
                </td>
            </tr>
        </table>
    </div>
    <div class="div-frm" style="height: 275px;">
        <%--基本信息--%>
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th>
                    User Code *:
                </th>
                <td>
                    <input id="User_Code" runat="server" type="text" class="txt" datacol="yes" err="职工工号"
                        checkexpession="NotNull" style="width: 200px" />
                </td>
                <th>
                    User Name:
                </th>
                <td>
                    <input id="User_Name" runat="server" type="text" class="txt" datacol="yes" err="职工姓名"
                        checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th>
                    Birthday *:
                </th>
                <td>
                    <asp:TextBox id="birthday" runat="server" type="date" err="员工生日" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th>
                    入职前社会工龄:
                </th>
                <td>
                    <input id="Sage_b" runat="server" type="text" class="txt" datacol="yes" err="入职前社会工龄"
                        checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th>
                    Join Date *:
                </th>
                <td>
                    <asp:TextBox id="join_date" runat="server" type="date" err="入职日期" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th>
                    Dimission Date:
                </th>
                <td>
                    <asp:TextBox id="out_date" runat="server" type="date" err="入职日期" checkexpession="NotNull" style="width: 200px"  OnTextChanged="out_date_TextChanged" AutoPostBack="true"/>
                </td>
            </tr>
          <tr>
            <th>
                Status *:
            </th>
            <td>
                <select id="work_flag" name="work_flag" runat="server" style="width:200px">
                  <option value ="0">Dimission</option>
                  <option value ="1" selected="selected">In Service</option>
               </select>
            </td>
            <th>
                Boss ID:
            </th>
            <td>
                <input id="Boss_id" name="country_name" runat="server"  type="text" list="Boss_list" style="width: 200px"/>
                <datalist id="Boss_list" runat="server">
                </datalist>
            </td>
        </tr>
            <tr>
                <th>
                    Login Account:
                </th>
                <td>
                    <input id="User_Account" runat="server" type="text" class="txt" datacol="yes" err="登录账户"
                        checkexpession="NotNull" style="width: 200px" />
                </td>
                <th>
                    Login Password:
                </th>
                <td>
                    <input id="User_Pwd" runat="server" type="text" class="txt" datacol="yes" err="登录密码"
                        checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th>
                    Gender:
                </th>
                <td>
                    <select id="User_Sex" class="select" runat="server" style="width: 206px">
                        <option value="1">1 - Male</option>
                        <option value="0">0 - Female</option>
                    </select>
                </td>
                <th>
                    EMail *:
                </th>
                <td>
                    <input id="Email" runat="server" type="text" class="txt" datacol="yes" err="电子邮箱"
                        checkexpession="EmailOrNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th>
                    Create User:
                </th>
                <td>
                    <input id="CreateUserName" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
                <th>
                    Create Date:
                </th>
                <td>
                    <input id="CreateDate" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th class="auto-style1">
                    Edit User:
                </th>
                <td class="auto-style1">
                    <input id="ModifyUserName" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
                <th class="auto-style1">
                    Edit Date:
                </th>
                <td class="auto-style1">
                    <input id="ModifyDate" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
            </tr>
            <tr>
            <th>
                User Level *:
            </th>
             <td>
                 <select id="Boss_Flag" runat="server">
                     <option value="0" selected="selected">Ordinary Employee</option>
                     <option value="1">Manager and Above</option>
                 </select>
             </td>
             <th>
                 ATS_Code *:
             </th>
             <td>
                 <input id="ATS_Code" runat="server" type="text" class="txt" style="width: 200px" />
             </td>
            </tr>
            <tr>
                <th>
                    Is HR?
                </th>
                <td>
                    <select id="HR" runat="server">
                     <option value="0" selected="selected">NO</option>
                     <option value="1">Yes</option>
                 </select>
                </td>
                <th>
                    Remark:
                </th>
                <td colspan="3">
                    <textarea id="User_Remark" class="txtRemark" runat="server" style="width: 200px;
                        height: 83px;"></textarea>
                </td>
            </tr>
        </table>
        <%--附加信息--%>
        <table id="table2" border="0" cellpadding="0" cellspacing="0" class="frm">
            <%=str_OutputHtml.ToString()%>
        </table>
        <%--所属部门--%>
        <div id="table3">
            <div class="btnbartitle">
                <div>
                    Dept.
                </div>
            </div>
            <div class="div-body" style="height: 245px;">
                <ul class="strTree">
                    <%=strOrgHtml.ToString()%>
                </ul>
            </div>
        </div>
        <%--所属角色--%>
        <div id="table4">
            <div class="btnbartitle">
                <div>
                    Role
                </div>
            </div>
            <div class="div-body" style="height: 245px;">
                <ul class="strTree">
                    <%=strRoleHtml.ToString()%>
                </ul>
            </div>
        </div>
        <%--所属工作组--%>
        <div id="table5">
            <div class="btnbartitle">
                <div>
                    Work Group
                </div>
            </div>
            <div class="div-body" style="height: 245px;">
                <ul class="strTree">
                    <%=strUserGroupHtml.ToString()%>
                </ul>
            </div>
        </div>
        <%--用户权限--%>
        <div id="table6">
            <div class="div-body" style="height: 273px;">
                <table class="example" id="dnd-example">
                    <thead>
                        <tr>
                            <td style="width: 200px; padding-left: 20px;">
                                URL Right
                            </td>
                            <td style="width: 30px; text-align: center;">
                                Icon
                            </td>
                            <td style="width: 20px; text-align: center;">
                                <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                    &nbsp;</label>
                            </td>
                            <td>
                                Button Rights
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <%=strUserRightHtml.ToString()%>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckValid();"
            OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>
