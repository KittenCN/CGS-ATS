<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_ATSReport.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_ATSReport" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考勤报告</title>
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
            top.openDialog(url, 'PerLeaveCreate', 'Create Leave', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/RMBase/SysATS/frmATS_ATSResultEdit.aspx?key=" + key;
                top.openDialog(url, 'ATSResultEdit', '修改考勤结果', 700, 400, 50, 50);
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
                top.openDialog(url, 'PerLeaveAppPro', 'Approve Leave', 700, 500, 50, 50);
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
        function browseFolder(path) {
            try {
                var Message = "\u8bf7\u9009\u62e9\u6587\u4ef6\u5939"; //选择框提示信息
                var Shell = new ActiveXObject("Shell.Application");
                var Folder = Shell.BrowseForFolder(0, Message, 64, 17); //起始目录为：我的电脑
                //var Folder = Shell.BrowseForFolder(0,Message,0); //起始目录为：桌面
                if (Folder != null) {
                    Folder = Folder.items(); // 返回 FolderItems 对象
                    Folder = Folder.item(); // 返回 Folderitem 对象
                    Folder = Folder.Path; // 返回路径
                    if (Folder.charAt(Folder.length - 1) != "") {
                        Folder = Folder + "";
                    }
                    document.getElementById(path).value = Folder;
                    return Folder;
                }
            }
            catch (e) {
                alert(e.message);
            }
        }
    </script>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:Label Text="Begin Date:" runat="server"></asp:Label>
                <asp:TextBox ID="tb_BeginDate" type="date" runat="server" />
                <asp:Label Text="End Date:" runat="server"></asp:Label>
                <asp:TextBox ID="tb_EndDate" type="date" runat="server" />
                <asp:Button ID="btn_LVsearch" Text="Search" runat="server" OnClick="btn_LVsearch_Click" />
                <asp:Button ID="btn_Export" Text="Export" runat="server" OnClick="btn_Export_Click" Height="20px" />
            </div>
        </div>
        <div class="div-body">
            <table id="table1" class="grid">
                <thead>
                    <tr>
                        <%--                    <td style=" text-align: left;" class="auto-style1">
                        <label id="checkAllOff">
                            &nbsp;</label>
                    </td>--%>
                        <td style="text-align: center;">Name
                        </td>
                        <td style="text-align: center;">Days of Work
                        </td>
                        <td style="text-align: center;">Working Days
                        </td>
                        <td style="text-align: center; width: 120px">LL times(<=4h)
                        </td>
                        <td style="text-align: center; width: 120px">LL times(>4h)
                        </td>
                        <td style="text-align: center;">Absent times
                        </td>
                        <td style="text-align: center;">Personal leave
                        </td>
                        <td style="text-align: center;">Marital Leave
                        </td>
                        <td style="text-align: center;">Maternity Leave
                        </td>
                        <td style="text-align: center;">Funeral Leave
                        </td>
                        <td style="text-align: center;">Leave in lieu
                        </td>
                        <td style="text-align: center;">Sick Leave
                        </td>
                        <td style="text-align: center;">Annual Leave
                        </td>
                        <td style="text-align: center;">Actual Working Days
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                        <ItemTemplate>
                            <tr id="tbtr">
                                <%--                            <td id="tbtrtd" style=" text-align: left;" class="auto-style1">
                                <input id="tbtrtdin" type="checkbox" name="checkbox" class="auto-style1"/>
                            </td>--%>
                                <td style="text-align: center;">
                                    <asp:Label ID="USER_ID" runat="server" Text='<%#Eval("USER_ID")%>'></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="ygzts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="jrts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center; width: 120px">
                                    <asp:Label ID="czl" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center; width: 120px">
                                    <asp:Label ID="czp" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="kgcs" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="shjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="hjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="cjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="sjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="txjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="bjts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="njts" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="sjgz" runat="server"></asp:Label>
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
