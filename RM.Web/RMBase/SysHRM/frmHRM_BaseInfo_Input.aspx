<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmHRM_BaseInfo_Input.aspx.cs" Inherits="RM.Web.RMBase.SysHRM.frmHRM_BaseInfo_Input" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>组织机构部门表单</title>
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
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                员工工号:
            </th>
            <td>
                <input id="id" maxlength="7" runat="server" type="text" class="txt"
                    datacol="yes" checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                姓名:
            </th>
            <td>
                <input id="name" runat="server" type="text" class="txt" datacol="yes"
                     checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                生日:
            </th>
            <td>
                <asp:TextBox id="birthday" runat="server" type="date" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                入职前社会工龄:
            </th>
            <td>
                <input id="Sage_B" maxlength="7" runat="server" type="text" class="txt"
                    datacol="yes" checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                入职日期:
            </th>
            <td>
                <asp:TextBox id="join_date" runat="server" type="date" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                离职日期:
            </th>
            <td>
                <asp:TextBox id="out_date" runat="server" type="date" style="width: 200px" />
            </td>
        </tr>
         <tr>
            <th>
                工作状态:
            </th>
            <td>
                <select id="work_flag" name="work_flag">
                  <option value ="0">离职</option>
                  <option value ="1" selected="selected">在职</option>
               </select>
            </td>
        </tr>
        <tr style="width: 300px">
            <th>
                直属上级:
            </th>
            <td>
                <input id="Boss_id" runat="server" type="text" datacol="yes"
                     checkexpession="NotNull" style="width: 200px" />
                <input id="select_boss" runat="server" type="button" />
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
