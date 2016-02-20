<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Button_Form.aspx.cs" Inherits="RM.Web.RMBase.SysMenu.Button_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>按钮设置表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onkeyButton_Name(text) {
            $("#Button_Title").val(text);
        }
        //全取系统图标
        function SelectOpenImg() {
            var url = "../RMBase/SysMenu/Icons_List.aspx?Size=16";
            top.openDialog(url, 'Icons_List', '系统图标 - 全取', 615, 400, 100, 100);
        }
        //全取图标回调赋值
        function Get_Menu_Img(img) {
            $("#Img_Button_Img").attr("src", '../../Themes/Images/16/' + img);
            $("#Button_Img").val(img);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                按钮名称：
            </th>
            <td>
                <input id="Button_Name" runat="server" type="text" class="txt" datacol="yes" err="按钮名称"
                    checkexpession="NotNull" style="width: 85%" onkeyup="onkeyButton_Name(this.value)" />
            </td>
        </tr>
        <tr>
            <th>
                按钮标记：
            </th>
            <td>
                <input id="Button_Title" runat="server" type="text" class="txt" datacol="yes" err="菜单标记"
                    checkexpession="NotNull" style="width: 85%" />
            </td>
        </tr>
        <tr>
            <th>
                显示位置：
            </th>
            <td>
                <select id="Button_Type" class="select" runat="server" style="width: 87%">
                    <option value="工具栏">工具栏</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>
                菜单图标：
            </th>
            <td>
                <input id="Button_Img" type="hidden" runat="server" />
                <img id="Img_Button_Img" src="/Themes/Images/illustration.png" runat="server" alt=""
                    style="vertical-align: middle; padding-right: 10px;" />
                <a href="javascript:void(0)" class="button green" onclick="SelectOpenImg()">图标全取</a>
            </td>
        </tr>
        <tr>
            <th>
                显示顺序：
            </th>
            <td>
                <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                    checkexpession="Num" style="width: 85%" />
            </td>
        </tr>
        <tr>
            <th>
                按钮事件：
            </th>
            <td>
                <textarea id="Button_Code" class="txtRemark" runat="server" style="width: 85.5%;
                    height: 50px;"></textarea>
            </td>
        </tr>
        <tr>
            <th>
                说明：
            </th>
            <td>
                <textarea id="Button_Remak" class="txtRemark" runat="server" style="width: 85.5%;
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
