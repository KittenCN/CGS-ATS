<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu_Form.aspx.cs" Inherits="RM.Web.RMBase.SysMenu.Menu_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单导航设置表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onkeyMenu_Name(text) {
            $("#Menu_Title").val(text);
        }
        //全取系统图标
        function SelectOpenImg() {
            var url = "/RMBase/SysMenu/Icons_List.aspx?Size=32";
            top.openDialog(url, 'Icons_List', '系统图标 - 全取', 615, 400, 100, 100);
        }
        //全取图标
        function Get_Menu_Img(img) {
            $("#Img_Menu_Img").attr("src", '/Themes/Images/32/' + img);
            $("#Menu_Img").val(img);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                菜单名称：
            </th>
            <td>
                <input id="Menu_Name" runat="server" type="text" class="txt" datacol="yes" err="菜单名称"
                    checkexpession="NotNull" style="width: 90%" onkeyup="onkeyMenu_Name(this.value)" />
            </td>
        </tr>
        <tr>
            <th>
                菜单标记：
            </th>
            <td>
                <input id="Menu_Title" runat="server" type="text" class="txt" datacol="yes" err="菜单标记"
                    checkexpession="NotNull" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <th>
                节点位置：
            </th>
            <td>
                <select id="ParentId" class="select" runat="server" style="width: 92%">
                </select>
            </td>
        </tr>
        <tr>
            <th>
                菜单图标：
            </th>
            <td>
                <input id="Menu_Img" type="hidden" runat="server" />
                <img id="Img_Menu_Img" src="/Themes/Images/illustration.png" runat="server" alt=""
                    style="vertical-align: middle; padding-right: 10px;" />
                <a href="javascript:void(0)" class="button green" onclick="SelectOpenImg()">图标全取</a>
            </td>
        </tr>
        <tr>
            <th>
                连接目标：
            </th>
            <td>
                <select id="Target" class="select" runat="server" style="width: 92%">
                    <option value="Iframe">Iframe</option>
                    <option value="Open">Open</option>
                    <option value="href">href</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>
                显示顺序：
            </th>
            <td>
                <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                    checkexpession="Num" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <th>
                连接地址：
            </th>
            <td>
                <textarea id="NavigateUrl" class="txtRemark" runat="server" style="width: 90.5%;
                    height: 50px;"></textarea>
            </td>
        </tr>
    </table>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckDataValid('#form1');"
            OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
    </form>
</body>
</html>
