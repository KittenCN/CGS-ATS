<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_LeaveConsoleEdit.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_LeaveConsoleEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>修改休假结果</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script></head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="frm">
            <tr>
                <th>Name</th>
                <td><asp:Label ID="EmpID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>结转年假</th>
                <td><asp:TextBox ID="JZAL" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>结转到期日</th>
                <td><asp:TextBox id="JZDate" runat="server" type="date" /></td>
            </tr>
            <tr>
                <th>当年可生成年假</th>
                <td><asp:Label ID="CKAL" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>当年已生成年假</th>
                <td><asp:Label ID="CYAL" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>当年调整年假</th>
                <td><asp:TextBox ID="ALEdit" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>剩余调休天数</th>
                <td><asp:TextBox ID="SYTX" runat="server"></asp:TextBox></td>
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
