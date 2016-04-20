<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Backup_Confirm.aspx.cs"
    Inherits="RM.Web.RMBase.SysDataCenter.Backup_Confirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备份恢复信息确认</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //保存
        function SaveForm() {
            var type = GetQuery("type");
            var UserPwd = "<%=_UserPwd %>";
            if (UserPwd != $.md5($("#txtUserPwd").val()).toUpperCase()) {
                showWarningMsg("您输入登录密码不对!");
                return false;
            }
            if (type == 0) {
                showConfirmMsg('注：您确认要备份当前数据库数据吗？', function (r) {
                    if (r) {
                        top.main.okbackups($("#Remak").val()); OpenClose();
                    }
                });
            } else if (type == 1) {
                showConfirmMsg('注：您确认要恢复还原当前数据库数据吗？', function (r) {
                    if (r) {
                        top.main.okrecover($("#Remak").val()); OpenClose();
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                登录密码：
            </th>
            <td>
                <input id="txtUserPwd" runat="server" type="password" class="txt" datacol="yes" err="登录密码"
                    checkexpession="NotNull" style="width: 85%" />
            </td>
        </tr>
        <tr>
            <th>
                说明：
            </th>
            <td>
                <textarea id="Remak" class="txtRemark" runat="server" style="width: 85.5%; height: 80px;"></textarea>
            </td>
        </tr>
    </table>
    <div class="frmbottom">
        <a class="l-btn" href="javascript:void(0)" onclick="SaveForm();"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></a> <a class="l-btn" href="javascript:void(0)"
                onclick="OpenClose();"><span class="l-btn-left">
                    <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>
