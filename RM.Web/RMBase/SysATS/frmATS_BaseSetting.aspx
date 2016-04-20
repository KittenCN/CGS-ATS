<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_BaseSetting.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_BaseSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>考勤基础设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="Main" class="frm">
            <tr>
                <th>标准上班时间:</th>
                <td><asp:TextBox id="BeginTime" runat="server" type="time" style="width: 200px" /></td>
            </tr>
            <tr>
                <th>Morning</th>
                <td><asp:TextBox ID="AMEndTime" runat="server" type="time" style="width: 200px"></asp:TextBox></td>
            </tr>
            <tr>
                <th>Afternoon</th>
                <td><asp:TextBox ID="PMBeginTime" runat="server" type="time" style="width: 200px"></asp:TextBox></td>
            </tr>
            <tr>
                <th>标准下班时间:</th>
                <td><asp:TextBox id="EndTime" runat="server" type="time" style="width: 200px" /></td>
            </tr>
            <tr>
                <th>午休开始时间:</th>
                <td><asp:TextBox id="LunchBeginTime" runat="server" type="time" style="width: 200px" /></td>
            </tr>
            <tr>
                <th>午休结束时间:</th>
                <td><asp:TextBox id="LunchEndTime" runat="server" type="time" style="width: 200px" /></td>
            </tr>
            <tr>
                <th>弹性工作时间:</th>
                <td><input id="ExWorkTime" runat="server" type="text" class="txt" style="width: 200px" />分钟</td>
            </tr>
            <tr>
                <th>上午工作时间:</th>
                <td><input id="AMWorkTime" runat="server" type="text" class="txt" style="width: 200px" />小时</td>
            </tr>
            <tr>
                <th>下午工作时间:</th>
                <td><input id="PMWorkTime" runat="server" type="text" class="txt" style="width: 200px" />小时</td>
            </tr>
            <tr>
                <th>标准工作时间:</th>
                <td><input id="NorWorkTime" runat="server" type="text" class="txt" style="width: 200px" />小时</td>
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
