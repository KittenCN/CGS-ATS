<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_HolidaySetting.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_HolidaySetting" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>节日设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script type="text/javascript">
        //添加
        function add() {
            var url = "/RMBase/SysATS/frmATS_HolidayCreate.aspx";
            top.openDialog(url, 'HolidayCreate', '新建节日', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/RMBase/SysATS/frmATS_HolidayEdit.aspx?key=" + key;
                top.openDialog(url, 'HolidayEdit', '修改节日', 700, 350, 50, 50);
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
            height: 20px;
        }
        .auto-style2 {
            width: 60px;
            height: 20px;
        }
        .auto-style3 {
            width: 80px;
            height: 20px;
        }
        .auto-style4 {
            width: 100px;
            height: 20px;
        }
        .auto-style5 {
            width: 50px;
            height: 20px;
        }
        .auto-style6 {
            width: 200px;
            height: 20px;
        }
        .auto-style7 {
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="btnbarcontetn">
        <div style="text-align: left;">
            <asp:Button ID="btn_CreateHoliday" Text="新建节日" runat="server" OnClick="btn_CreateHoliday_Click" OnClientClick="add()" />
            <asp:Button ID="btn_EditHoliday" Text="修改节日" runat="server" OnClick="btn_EditHoliday_Click" OnClientClick="edit()" />
            <asp:Button ID="btn_DelHoliday" Text="删除节日" runat="server" OnClick="btn_DelHoliday_Click" />
        </div>
         <div style="text-align: right">
            <%--<uc2:LoadButton ID="LoadButton1" runat="server" />--%>
        </div>
    </div>
    <div>
        <table id="table1" class="grid" singleselect="true">
            <thead>
                <tr>
                    <td style="width: 20px; text-align: left;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style="width: 100px; text-align: center;">
                        节日名称
                    </td>
                    <td style="width: 80px; text-align: center;">
                        Begin Date
                    </td>
                    <td style="width: 80px; text-align: center;">
                        开始时间
                    </td>
                    <td style="width: 80px; text-align: center;">
                        End Date
                    </td>
                    <td style="width: 80px; text-align: center;">
                        结束时间
                    </td>
                    <td>
                        Remark
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound" >
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("id")%>" name="checkbox" />
                            </td>
                            <td style="width: 100px; text-align: center;">
                                <%#Eval("Holiday_Name")%>
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <%#Eval("BeginDate", "{0:d}")%></a>
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <asp:Label ID="lab_BeginFlag" runat="server" Text='<%#Eval("BeginFlag")%>'></asp:Label>
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <%#Eval("EndDate", "{0:d}")%></a>
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <asp:Label ID="lab_EndFlag" runat="server" Text='<%#Eval("EndFlag")%>'></asp:Label>
                            </td>
                            <td>
                                <%#Eval("Remark")%>
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
    </form>
</body>
</html>
