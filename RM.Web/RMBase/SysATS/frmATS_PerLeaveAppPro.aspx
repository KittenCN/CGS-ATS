﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_PerLeaveAppPro.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_PerLeaveAppPro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>审批休假申请</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <style type="text/css">
        #Remark {
            width: 241px;
        }
        .auto-style1 {
            width: 885px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="frm">
            <tr>
                <th>申请人:</th>
                <td><asp:label id="EmpID" runat="server" ></asp:label></td>
            </tr>
            <tr>
                <th>填写日期:</th>
                <td><asp:label id="CreateDate" runat="server" /></td>
            </tr>
            <tr>
                <th>假期类别:</th>
                <td>
                    <asp:DropDownList ID="LeaveID" runat="server" Enabled="false">

                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>开始日期:</th>
                <td><asp:TextBox id="BeginDate" type="date" runat="server"  Enabled="false"/></td>
            </tr>
            <tr>
                <th>开始日状态:</th>
                <td>
                    <select id="BeginFlag" runat="server" aria-readonly="True" >
                        <option value="0">下午开始时间</option>
                        <option value="1" selected="selected">上午开始时间</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>结束日期:</th>
                <td><asp:TextBox id="EndDate" type="date" runat="server"  Enabled="false"/></td>
            </tr>
            <tr>
                <th>结束日状态:</th>
                <td>
                    <select id="EndFlag" runat="server" aria-readonly="True">
                        <option value="0">上午结束时间</option>
                        <option value="1" selected="selected">下午结束时间</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>备注:</th>
                <td><textarea id="Remark" runat="server" maxlength="500" rows="3" readonly="readonly"></textarea></td>
            </tr>
            <tr>
                <th>附件:</th>
                <td>
                     <input type="file" id="FilesAdd" name="FilesAdd" runat="server"  aria-readonly="true" />
                     <asp:Button ID="btn_submit" Text="Upload" runat="server" onclick="btn_submit_Click"  Enabled="false"/>                    
                </td>
            </tr>
            <tr id="DownFiles" runat="server">
                <th>下载附件:</th>
                <td>
                     <asp:Button ID="btn_down" Text="Download" runat="server" onclick="btn_down_Click" />
                </td>
            </tr>
            <tr>
                <th>审批人备注:</th>
                <td><textarea id="ApprovalRemark" runat="server" maxlength="500" rows="10" class="auto-style1"></textarea></td>
            </tr>
        </table>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Pass_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />通  过</span></asp:LinkButton>
        <asp:LinkButton ID="Reject" runat="server" class="l-btn" OnClick="Reject_Click"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />否  决</span></asp:LinkButton>
    </div>
    </form>
</body>
</html>
