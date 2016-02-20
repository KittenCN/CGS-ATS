<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recyclebin_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.Recyclebin_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回收站数据</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
            divresize(90);
            FixedTableHeader("#table1", $(window).height() - 124);
        })
        //点击查看详细
        function Onclickhref(key) {
            var url = 'Recyclebin_Info.aspx?key=' + key;
            Urlhref(url);
        }
        //还原
        function restore() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var parm = 'action=restore_Data&pkVal=' + key;
                showConfirmMsg("您确定要还原当前【" + key.split(",").length + "】条数据吗？", function (r) {
                    if (r) {
                        getAjax('Recyclebin.ashx', parm, function (rs) {
                            if (parseInt(rs) > 0) {
                                showTipsMsg("还原成功！", 2000, 4);
                                windowload();
                            } else if (parseInt(rs) == 0) {
                                showTipsMsg("还原失败，0 行受影响！", 3000, 3);
                            }
                            else {
                                showTipsMsg("<span style='color:red'>还原失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
        //清空
        function Empty() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var parm = 'action=restore_Empty&pkVal=' + key;
                showConfirmMsg("此操作不可恢复，您确定要清空当前【" + key.split(",").length + "】条数据吗？", function (r) {
                    if (r) {
                        getAjax('Recyclebin.ashx', parm, function (rs) {
                            if (parseInt(rs) > 0) {
                                showTipsMsg("清空成功！", 2000, 4);
                                windowload();
                            } else if (parseInt(rs) == 0) {
                                showTipsMsg("清空失败，0 行受影响！", 3000, 3);
                            }
                            else {
                                showTipsMsg("<span style='color:red'>清空失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            回收站
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="float: left;">
            删除时间:
            <input type="text" id="BeginBuilTime" class="txt" runat="server" style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' })" />
            至
            <input type="text" id="endBuilTime" class="txt" runat="server" style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' });" />
            <asp:LinkButton ID="lbtSearch" runat="server" class="button green" OnClick="lbtSearch_Click"><span class="icon-botton"
            style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
        </span>查 询</asp:LinkButton>
        </div>
        <div style="text-align: right">
            <uc2:LoadButton ID="LoadButton1" runat="server" />
        </div>
    </div>
    <div class="div-body">
        <table id="table1" class="grid">
            <thead>
                <tr>
                    <td style="width: 20px; text-align: left;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style="width: 250px;">
                        对象字段值
                    </td>
                    <td style="width: 150px; text-align: center;">
                        原理位置
                    </td>
                    <td style="width: 150px; text-align: center;">
                        删除用户
                    </td>
                    <td style="width: 150px; text-align: center;">
                        删除时间
                    </td>
                    <td>
                        备注
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Recyclebin_ID")%>" name="checkbox" />
                            </td>
                            <td style="width: 250px;">
                                <a href="javascript:void()" onclick="Onclickhref('<%#Eval("Recyclebin_ID")%>')">
                                    <%#Eval("Recyclebin_EventField")%></a>
                            </td>
                            <td style="width: 150px; text-align: center;">
                                <%#Eval("Recyclebin_Name")%>页面
                            </td>
                            <td style="width: 150px; text-align: center;">
                                <%#Eval("CreateUserName")%>
                            </td>
                            <td style="width: 150px; text-align: center;">
                                <%#Eval("CreateDate")%>
                            </td>
                            <td>
                                <%#Eval("Recyclebin_Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='6' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
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
