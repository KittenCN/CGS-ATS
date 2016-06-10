<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_ATSResult.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_ATSResult" %>
<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ATS Check</title>
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
            top.openDialog(url, 'PerLeaveCreate', 'New Leave', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/RMBase/SysATS/frmATS_ATSResultEdit.aspx?key=" + key;
                top.openDialog(url, 'ATSResultEdit', 'Edit Leave Result', 700, 400, 50, 50);
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
                top.openDialog(url, 'PerLeaveAppPro', 'Examine Leave', 700, 500, 50, 50);
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
            width: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="btnbarcontetn">
        <div style="text-align: left;">           
<%--            <asp:Button ID="btn_CreateTravel" Text="Create BizTravel" runat="server"  OnClientClick="add()" />
            <asp:Button ID="btn_EditTravel" Text="Edit BizTravel" runat="server"  OnClientClick="edit()" />
            <asp:Button ID="btn_DelTravel" Text="Cancel BizTravel" runat="server" />--%>
            <input id="EmpID" name="EmpID" runat="server"  type="text" list="Emplist" style="width: 200px"/>
            <datalist id="Emplist" runat="server"></datalist>
            <asp:label Text="Begin Date:" runat="server" ></asp:label>
            <asp:TextBox id="tb_BeginDate" type="date" runat="server" />
            <asp:label Text="End Date:" runat="server" ></asp:label>            
            <asp:TextBox id="tb_EndDate" type="date" runat="server" />
            <asp:Button ID="btn_ATSCheck" Text="Begin Exmaine" runat="server"  OnClick="btn_ATSCheck_Click"/>
            <asp:Button ID="btn_Search" Text="Search" runat="server" OnClick="btn_Search_Click" />
            <asp:Button ID="btn_EditTravel" Text="Edit ATS Result" runat="server"  OnClientClick="edit()" />
            <asp:Button ID="btn_SetNor" Text="Set Normal Status" runat="server" OnClick="btn_SetNor_Click" Width="135px" />
        </div>
         <div style="text-align: right">
            <%--<uc2:LoadButton ID="LoadButton1" runat="server" />--%>
        </div>
    </div>
    <div class="div-body" >
        <table id="table1" class="grid">
            <thead>
                <tr>
                    <td style="text-align: left;" class="auto-style1">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style=" text-align: center;">
                        EmpName
                    </td>
                    <td style=" text-align: center;">
                        ExamStatus
                    </td>
                    <td style=" text-align: center;">
                        ATS Date
                    </td>
                    <td style=" text-align: center;">
                        ATS Status
                    </td>
                    <td style=" text-align: center;">
                        Holiday
                    </td>
                    <td style=" text-align: center;">
                        Hol Status
                    </td>
                    <td style=" text-align: center;">
                        Leave
                    </td>
                    <td style=" text-align: center;">
                        Biz Travel
                    </td>
                    <td style=" text-align: center;">
                        Work Time
                    </td>
                    <td style=" text-align: center;">
                        Lunch Time
                    </td>
                    <td style=" text-align: center;">
                        Off Time
                    </td>
                    <td style=" text-align: center;">
                        ATS Result
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound" >
                    <ItemTemplate>
                        <tr id="tbtr">
                            <td id="tbtrtd" style=" text-align: left;" class="auto-style1">
                                <input id="tbtrtdin" type="checkbox" value="<%#Eval("id")%>" name="tbtrtdin" class="auto-style1" />
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="EmpID" runat="server" Text='<%#Eval("EmpID")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="Flag" runat="server" Text='<%#Eval("Flag")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <%#Eval("ATS_Date", "{0:d}")%>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="ATS_DateStatus" runat="server" Text='<%#Eval("ATS_DateStatus")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="ATS_Holiday" runat="server" Text='<%#Eval("ATS_Holiday")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="ATS_HolidayStatus" runat="server" Text='<%#Eval("ATS_HolidayStatus")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="ATS_Leave" runat="server" Text='<%#Eval("ATS_Leave")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="ATS_Travel" runat="server" Text='<%#Eval("ATS_Travel")%>'></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <%#Eval("PunchINTime","{0:T}")%>
                            </td>
                            <td style=" text-align: center;">
                                <%#Eval("LunchTime")%>
                            </td>
                            <td style=" text-align: center;">
                                <%#Eval("PunchOutTime","{0:T}")%>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="ATS_Result" runat="server" Text='<%#Eval("ATS_Result")%>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='8' style='color:red;text-align:center'>None Data！</td></tr>");
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
