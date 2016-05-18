<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_ATSResultEdit.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_ATSResultEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>考勤审查数据修改</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="frm">
            <tr>
                <th>Name</th>
                <td><asp:Label ID="EmpID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Status</th>
                <td><asp:Label ID="Flag" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Date</th>
                <td><asp:Label ID="ATS_Date" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Date Status</th>
                <td><asp:Label ID="ATS_DateStatus" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Holiday Name</th>
                <td><asp:Label ID="ATS_Holiday" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Vacation</th>
                <td><asp:Label ID="ATS_Leave" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>Biz travel</th>
                <td><asp:Label ID="ATS_Travel" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>上班打卡</th>
                <td><asp:TextBox id="PunchINTime" runat="server" type="time" /></td>
            </tr>
            <tr>
                <th>下班打卡</th>
                <td><asp:TextBox id="PunchOutTime" runat="server" type="time" /></td>
            </tr>
            <tr>
                <th>ATS Result</th>
                <td>
                    <select id="ATS_Result" runat="server">
                        <option value="0">Punch Error</option>
                        <option value="1" selected="selected">Normal</option>
                        <option value="2">LL Short</option>
                        <option value="3">LL Long</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>
