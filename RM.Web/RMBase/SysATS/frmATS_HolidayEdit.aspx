<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_HolidayEdit.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_HolidayEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>新建节日</title>
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
    <table class="frm">
        <tr>
            <th>节日名称:</th>
            <td> <input id="Holiday_name" type="text" class="txt" runat="server" style="width: 200px" /> </td>
        </tr>
        <tr>
            <th>Begin Date:</th>
            <td> <asp:TextBox id="BeginDate" runat="server" type="date" style="width: 200px"  /> </td>
        </tr>
        <tr>
            <th>开始日类型:</th>
            <td><select id="BeginFlag" name="BeginFlag" runat="server" style="width: 200px" >
                <option value="0">Afternoon</option>
                <option value="1" selected="selected">Morning</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>End Date:</th>
            <td> <asp:TextBox id="EndDate" runat="server" type="date"  style="width: 200px" /> </td>
        </tr>
        <tr>
            <th>结束日类型:</th>
            <td><select id="EndFlag" name="EndFlag" runat="server" style="width: 200px" >
                <option value="0">Morning</option>
                <option value="1" selected="selected">Aftermoon</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>Remark:</th>
            <td><input id="Remark" type="text" class="txt" runat="server" style="width: 200px"  /></td>
        </tr>
     </table>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>