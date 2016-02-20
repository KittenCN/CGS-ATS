<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUserPwd.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UpdateUserPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改登录密码</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckValid() {
            var _PasPwd = '<%=_PasPwd %>';
            var PasPwd = $("#txtPastUserPwd").val();
            var pwd = $("#txtUserPwd").val();
            var NewPwd = $("#User_NewPwd").val();
            var code = $("#txtCode").val();
            if (PasPwd == "") {
                $("#txtPastUserPwd").focus();
                $("#errorMsg").html("旧密码不能为空！");
                return false;
            }
            if (_PasPwd != $.md5(PasPwd).toUpperCase()) {
                $("#txtPastUserPwd").focus();
                $("#errorMsg").html("您输入旧密码不对！");
                return false;
            }
            if (pwd == "") {
                $("#txtUserPwd").focus();
                $("#errorMsg").html("密码不能为空！");
                return false;
            }
            if (pwd == PasPwd) {
                $("#txtUserPwd").focus();
                $("#errorMsg").html("新密码不能与旧密码相同！");
                return false;
            }
            if (NewPwd == "") {
                $("#User_NewPwd").focus();
                $("#errorMsg").html("确认密码不能为空！");
                return false;
            }
            if (pwd != NewPwd) {
                $("#User_NewPwd").empty();
                $("#txtUserPwd").empty();
                $("#errorMsg").html("两次密码不一致，请重新输入！");
                return false;
            }
            if (code == "") {
                $("#txtCode").focus();
                $("#errorMsg").html("验证码不能为空！");
                return false;
            }
            return confirm('请牢记当前密码，您确认要修改登录密码？');
        }
        //清空
        function resetInput() {
            $("#txtPastUserPwd").focus(); //默认焦点
            $("#txtPastUserPwd").val("");
            $("#txtUserPwd").val("");
            $("#User_NewPwd").val("");
        }
    </script>
    <style type="text/css">
        #errorMsg
        {
            border: 1px solid #A8A8A8;
            width: auto;
            padding-left: 30px;
            padding-right: 20px;
            padding-top: 3px;
            padding-bottom: 2px;
            background: rgb(255, 253, 215) url('/Themes/images/exclamation_octagon_fram.png') no-repeat scroll 5px 2px;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                账&nbsp;&nbsp;户：
            </th>
            <td>
                <input type="text" disabled id="txtUserName" runat="server" class="txt" style="width: 200px;" />
            </td>
        </tr>
        <tr>
            <th>
                旧密码：
            </th>
            <td>
                <input type="password" id="txtPastUserPwd" runat="server" class="txt" style="width: 200px;" />
            </td>
        </tr>
        <tr>
            <th>
                新密码：
            </th>
            <td>
                <input type="password" id="txtUserPwd" runat="server" class="txt" style="width: 200px;" />
            </td>
        </tr>
        <tr>
            <th>
                确认密码：
            </th>
            <td>
                <input id="User_NewPwd" runat="server" type="password" class="txt" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                验证码：
            </th>
            <td>
                <input type="text" id="txtCode" runat="server" class="inp3 txt" style="width: 48px;" />
                <img src="/Ajax/Verify_code.ashx" width="70" height="22" alt="点击切换验证码" title="点击切换验证码"
                    style="margin-top: 0px; vertical-align: top; cursor: pointer;" onclick="ToggleCode(this, '/Ajax/Verify_code.ashx');return false;" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <span id="errorMsg" runat="server">修改密码</span>
            </td>
        </tr>
    </table>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckValid();"
            OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
    </form>
</body>
</html>
