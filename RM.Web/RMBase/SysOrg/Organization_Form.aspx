<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Organization_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysOrg.Organization_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>组织机构部门表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                部门编号:
            </th>
            <td>
                <input id="Organization_Code" maxlength="7" runat="server" type="text" class="txt"
                    datacol="yes" err="机构编号" checkexpession="NotNull" style="width: 200px" />
            </td>
            <th>
                机构名称:
            </th>
            <td>
                <input id="Organization_Name" runat="server" type="text" class="txt" datacol="yes"
                    err="机构名称" checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                主负责人:
            </th>
            <td>
                <input id="Organization_Manager" runat="server" type="text" class="txt" datacol="yes"
                    err="主负责人" checkexpession="NotNull" style="width: 200px" />
            </td>
            <th>
                副负责人:
            </th>
            <td>
                <input id="Organization_AssistantManager" runat="server" type="text" class="txt"
                    style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                外线电话:
            </th>
            <td>
                <input id="Organization_InnerPhone" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
            <th>
                内线电话:
            </th>
            <td>
                <input id="Organization_OuterPhone" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                传真号码:
            </th>
            <td>
                <input id="Organization_Fax" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
            <th>
                邮政区号:
            </th>
            <td>
                <input id="Organization_Zipcode" maxlength="6" runat="server" type="text" class="txt"
                    style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                节点位置:
            </th>
            <td>
                <select id="ParentId" class="select" runat="server" style="width: 206px">
                </select>
            </td>
            <th>
                显示顺序:
            </th>
            <td>
                <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                    checkexpession="Num" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                所在地址:
            </th>
            <td colspan="3">
                <input id="Organization_Address" runat="server" type="text" class="txt" style="width: 550px" />
            </td>
        </tr>
        <tr>
            <th>
                说明:
            </th>
            <td colspan="3">
                <textarea id="Organization_Remark" class="txtRemark" runat="server" style="width: 552px;
                    height: 100px;"></textarea>
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
