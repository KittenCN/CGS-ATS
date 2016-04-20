<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppendProperty_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysAppend.AppendProperty_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>附加属性表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="frmtop" style="text-align: center">
        <label style="font-size: 20px">
            动态附加属性维护</label>
    </div>
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                属性名称：
            </th>
            <td>
                <input id="Property_Name" runat="server" type="text" class="txt" datacol="yes" err="属性名称"
                    checkexpession="NotNull" style="width: 200px" />
            </td>
            <th>
                控件ID：
            </th>
            <td>
                <input id="Property_Control_ID" runat="server" type="text" class="txt" datacol="yes"
                    err="控件ID" checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                控件类型：
            </th>
            <td>
                <select id="Property_Control_Type" class="select" runat="server" style="width: 206px">
                    <option value="1">1 - 文本框</option>
                    <option value="2">2 - 下拉框</option>
                    <option value="3">3 - 日期框</option>
                    <option value="4">4 - 标  签</option>
                </select>
            </td>
            <th>
                控件宽度：
            </th>
            <td>
                <input id="Property_Control_Length" runat="server" type="text" class="txt" style="width: 200px"
                    datacol="yes" err="控件宽度" checkexpession="NotNull" value='200' />
            </td>
        </tr>
        <tr>
            <th>
                控件样式：
            </th>
            <td>
                <select id="Property_Control_Style" class="select" runat="server" style="width: 206px">
                    <option value="txt">txt</option>
                    <option value="select">select</option>
                </select>
            </td>
            <th>
                表单验证：
            </th>
            <td>
                <select id="Property_Control_Validator" class="select" runat="server" style="width: 206px">
                    <option value="">允许为空</option>
                    <option value="NotNull">NotNull - 不能为空</option>
                    <option value="Num">Num - 必须为数字</option>
                    <option value="NumOrNull">NumOrNull - 必须为数字</option>
                    <option value="Phone">Phone - 必须电话格式！</option>
                    <option value="PhoneOrNull">PhoneOrNull - 必须电话格式！</option>
                    <option value="Mobile">Mobile - 必须为手机格式！</option>
                    <option value="MobileOrNull">MobileOrNull - 必须为手机格式！</option>
                    <option value="MobileOrPhoneOrNull">必须为电话格式或手机格式！</option>
                    <option value="Email">Email - 必须为E-mail格式！</option>
                    <option value="NumOrNull">NumOrNull - 必须为E-mail格式！</option>
                    <option value="Date">Date - 必须为日期格式！</option>
                    <option value="DateOrNull">DateOrNull - 必须为日期格式！</option>
                    <option value="IDCard">IDCard - 必须为身份证格式！</option>
                    <option value="IDCardOrNull">IDCardOrNull - 必须为身份证格式！</option>
                    <option value="Double">Double - 必须为小数！</option>
                    <option value="DoubleOrNull">DoubleOrNull - 必须为小数！</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>
                最大长度：
            </th>
            <td>
                <input id="Property_Control_Maxlength" runat="server" type="text" class="txt" datacol="yes"
                    err="最大长度" checkexpession="NotNull" style="width: 200px" />
            </td>
            <th>
                合并列：
            </th>
            <td>
                <input id="Property_Colspan" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                显示顺序：
            </th>
            <td colspan="3">
                <input id="SortCode" runat="server" type="text" class="txt" style="width: 200px"
                    datacol="yes" err="显示顺序" checkexpession="Num" />
            </td>
        </tr>
        <tr>
            <th>
                控件事件：
            </th>
            <td colspan="3">
                <textarea id="Property_Event" class="txtRemark" runat="server" style="width: 552px;
                    height: 60px;"></textarea>
            </td>
        </tr>
        <tr>
            <th>
                数据源：
            </th>
            <td colspan="3">
                <textarea id="Property_Control_DataSource" class="txtRemark" runat="server" style="width: 552px;
                    height: 60px;"></textarea>
            </td>
        </tr>
    </table>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckDataValid('#form1');"
            OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>
