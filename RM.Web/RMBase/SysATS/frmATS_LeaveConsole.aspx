<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_LeaveConsole.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_LeaveConsole" %>
<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>休假控制台</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
            divresize(90);
            FixedTableHeader("#table1", $(window).height() - 118);
        })
        //添加
        function add() {
            var url = "/RMBase/SysATS/frmATS_PerLeaveCreate.aspx";
            top.openDialog(url, 'PerLeaveCreate', '新建休假申请', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/RMBase/SysATS/frmATS_LeaveConsoleEdit.aspx?key=" + key;
                top.openDialog(url, 'LeaveConsoleEdit', '修改休假结果', 700, 400, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var delparm = 'action=Virtualdelete&module=用户管理&tableName=Base_UserInfo&pkName=User_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        }
        //审批
        function Approval() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = "/RMBase/SysATS/frmATS_PerTravelAppPro.aspx?key=" + key;
                top.openDialog(url, 'PerLeaveAppPro', '审批休假申请', 700, 500, 50, 50);
            }
        }
        //授 权
        function accredit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var parm = 'action=accredit&user_ID=' + key;
                showConfirmMsg('注：您确认要【授 权】当前选中用户吗？', function (r) {
                    if (r) {
                        getAjax('UserInfo.ashx', parm, function (rs) {
                            if (parseInt(rs) > 0) {
                                showTipsMsg("恭喜授权成功！", 2000, 4);
                                windowload();
                            }
                            else {
                                showTipsMsg("<span style='color:red'>授权失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
        //锁 定
        function lock() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var parm = 'action=lock&user_ID=' + key;
                showConfirmMsg('注：您确认要【锁 定】当前选中用户吗？', function (r) {
                    if (r) {
                        getAjax('UserInfo.ashx', parm, function (rs) {
                            if (parseInt(rs) > 0) {
                                showTipsMsg("锁定成功！", 2000, 4);
                                windowload();
                            }
                            else {
                                showTipsMsg("<span style='color:red'>锁定失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbarcontetn">
        <div style="text-align: left;">           
<%--            <asp:Button ID="btn_CreateTravel" Text="新建公出申请" runat="server"  OnClientClick="add()" />
            <asp:Button ID="btn_EditTravel" Text="修改公出申请" runat="server"  OnClientClick="edit()" />
            <asp:Button ID="btn_DelTravel" Text="取消公出申请" runat="server" />
            <asp:label Text="审查开始日期:" runat="server" ></asp:label>
            <asp:TextBox id="tb_BeginDate" type="date" runat="server" />
            <asp:label Text="审查结束日期:" runat="server" ></asp:label>            
            <asp:TextBox id="tb_EndDate" type="date" runat="server" />
            <asp:Button ID="btn_ATSCheck" Text="考勤审查" runat="server"  OnClick="btn_ATSCheck_Click"/>
            <asp:Button ID="btn_Search" Text="查询" runat="server" OnClick="btn_Search_Click" />--%>
            <asp:Button ID="btn_EditLeaveConsole" Text="修改休假结果" runat="server"  OnClientClick="edit()" />
        </div>
         <div style="text-align: right">
            <%--<uc2:LoadButton ID="LoadButton1" runat="server" />--%>
        </div>    
    </div>
    <div class="div-body" >
        <table id="table1" class="grid" singleselect="true">
            <thead>
                <tr>
                    <td style=" text-align: left;" class="auto-style1">
                        <label id="checkAllOff">
                            &nbsp;</label>
                    </td>
                    <td style=" text-align: center;">
                        员工姓名
                    </td>
                    <td style=" text-align: center;">
                        结转年假
                    </td>
                    <td style=" text-align: center;">
                        结转到期日
                    </td>
                    <td style=" text-align: center;">
                        当年可生成年假
                    </td>
                    <td style=" text-align: center;">
                        当年已生成年假
                    </td>
                    <td style=" text-align: center;">
                        当前已使用年假
                    </td>
                    <td style=" text-align: center;">
                        当前总可用年假
                    </td>
                    <td style=" text-align: center;">
                        剩余调休天数
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound" >
                    <ItemTemplate>
                        <tr id="tbtr">
                            <td id="tbtrtd" style=" text-align: left;" class="auto-style1">
                                <input id="tbtrtdin" type="checkbox" value="<%#Eval("EmpID")%>" name="checkbox" class="auto-style1"/>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="EmpID" runat="server" Text='<%#Eval("EmpID")%>' ></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="JZAL" runat="server" Text='<%#Eval("JZAL")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <%#Eval("JZDate", "{0:d}")%></a>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="CKAL" runat="server" Text='<%#Eval("CKAL")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="CYAL" runat="server" Text='<%#Eval("CYAL")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="UsedAL" runat="server" Text='<%#Eval("UsedAL")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="UseAL" runat="server" Text='<%#Eval("UseAL")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="SYTX" runat="server" Text='<%#Eval("SYTX")%>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='8' style='color:red;text-align:center'>没有数据！</td></tr>");
                               }
                           } %>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
        <uc1:PageControl ID="PageControl1" runat="server" />
    </form>
</body>
</html>
