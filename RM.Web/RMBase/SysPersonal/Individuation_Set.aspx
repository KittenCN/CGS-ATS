<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Individuation_Set.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.Individuation_Set" %>

<%@ Register Src="../../UserControl/SubmitCheck.ascx" TagName="SubmitCheck" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>个性化设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(29);
        })
        function MainSwitch() {
            window.top.location.href = "/Frame/MainSwitch.aspx";
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            请先确认您的浏览器启用了 cookie，否则无法使用个性化设置
        </div>
    </div>
    <div class="div-body">
        <div style="padding-left: 20px; padding-right: 20px; padding-top: 5px;">
            <div style="padding: 5px; margin-left: 10px;">
                <label style="font-size: 20px; font-weight: bold; color: gray;">
                    语言设置</label></div>
            <div class="line" style="height: 2px;">
            </div>
            <div style="padding-top: 20px; padding-bottom: 15px; padding-left: 100px;">
                语言首选项：<select id="Language_Type" class="select" runat="server" style="width: 200px">
                    <option value="中文简体 [zh-CN]">中文简体 [zh-CN]</option>
                    <option value="英语(美国) [en_US]">英语(美国) [en_US]</option>
                </select>
            </div>
            <div style="padding: 5px; margin-left: 10px;">
                <label style="font-size: 20px; font-weight: bold; color: gray;">
                    皮肤设置</label></div>
            <div class="line" style="height: 2px;">
            </div>
            <div style="padding-top: 20px; padding-bottom: 15px; padding-left: 100px;">
                UI皮肤设置：<select id="WebUI_Type" class="select" runat="server" style="width: 200px">
                    <option value="0">默认皮肤</option>
                    <option value="1">蓝色皮肤</option>
                    <option value="2">深蓝色皮肤</option>
                    <option value="3">咖啡色皮肤</option>
                </select>
            </div>
            <div style="padding: 5px; margin-left: 10px;">
                <label style="font-size: 20px; font-weight: bold; color: gray;">
                    导航设置</label></div>
            <div class="line" style="height: 2px;">
            </div>
            <div style="padding-top: 20px; padding-bottom: 15px; padding-left: 100px;">
                &nbsp;&nbsp; 表现方式：<select id="Menu_Type" class="select" runat="server" style="width: 200px">
                    <option value="0">默认导航</option>
                    <option value="1">手风琴菜单(2级)</option>
                    <option value="2">Top+Left菜单(2级)</option>
                    <option value="3">树形结构菜单(无限级)</option>
                </select>
            </div>
            <div style="padding: 5px; margin-left: 10px;">
                <label style="font-size: 20px; font-weight: bold; color: gray;">
                    表格设置</label></div>
            <div class="line" style="height: 2px;">
            </div>
            <div style="padding-top: 20px; padding-bottom: 15px; padding-left: 100px;">
                每页记录数：<select id="PageIndex" class="select" runat="server" style="width: 200px">
                    <option value="15">15</option>
                    <option value="30">30</option>
                    <option value="50">50</option>
                </select>
            </div>
            <div style="padding-top: 30px; padding-left: 180px;">
                <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保存设置</span></asp:LinkButton>
            </div>
        </div>
    </div>
    <uc1:SubmitCheck ID="SubmitCheck1" runat="server" />
    </form>
</body>
</html>
