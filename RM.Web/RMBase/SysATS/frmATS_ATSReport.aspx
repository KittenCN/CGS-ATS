<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_ATSReport.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_ATSReport" %>
<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>考勤报告</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
        <style type="text/css">
        .auto-style1 {
            width: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>

        </div>
        <div>

        </div>
    </div>
        <div class="div-body" >
        <table id="table1" class="grid">
            <thead>
                <tr>
<%--                    <td style=" text-align: left;" class="auto-style1">
                        <label id="checkAllOff">
                            &nbsp;</label>
                    </td>--%>
                    <td style=" text-align: center;">
                        员工姓名
                    </td>
                    <td style=" text-align: center;">
                        应工作天数
                    </td>
                    <td style=" text-align: center;">
                        节日天数
                    </td>
                    <td style=" text-align: center; width:120px">
                        迟到早退次数(<=4h)
                    </td>
                    <td style=" text-align: center; width:120px">
                        迟到早退次数(>4h)
                    </td>
                    <td style=" text-align: center;">
                        旷工次数
                    </td>
                    <td style=" text-align: center;">
                        事假天数
                    </td>
                    <td style=" text-align: center;">
                        婚假天数
                    </td>
                    <td style=" text-align: center;">
                        产假天数
                    </td>
                    <td style=" text-align: center;">
                        丧假天数
                    </td>
                    <td style=" text-align: center;">
                        调休假天数
                    </td>
                    <td style=" text-align: center;">
                        病假天数
                    </td>
                    <td style=" text-align: center;">
                        年假天数
                    </td>
                    <td style=" text-align: center;">
                        实工作天数
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound" >
                    <ItemTemplate>
                        <tr id="tbtr">
<%--                            <td id="tbtrtd" style=" text-align: left;" class="auto-style1">
                                <input id="tbtrtdin" type="checkbox" name="checkbox" class="auto-style1"/>
                            </td>--%>
                            <td style=" text-align: center;">
                                 <asp:Label ID="USER_ID" runat="server" Text='<%#Eval("USER_ID")%>' ></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="ygzts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                <asp:Label ID="jrts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center; width:120px">
                                 <asp:Label ID="czl" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center; width:120px">
                                 <asp:Label ID="czp" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="kgcs" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="shjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="hjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="cjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="sjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="txjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="bjts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="njts" runat="server"></asp:Label>
                            </td>
                            <td style=" text-align: center;">
                                 <asp:Label ID="sjgz" runat="server"></asp:Label>
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
