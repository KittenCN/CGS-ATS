<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_PerTravelEdit.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_PerTravelEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>修改公出申请</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <style type="text/css">
        #Place {
            width: 250px;
        }
        #Reason {
            width: 249px;
        }
        #Remark {
            width: 250px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="frm">
            <tr>
                <th>申请人:</th>
                <td><asp:label id="EmpID" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>填写日期:</th>
                <td><asp:label id="CreateDate" runat="server" /></td>
            </tr>
            <tr>
                <th>开始日期:</th>
                <td><asp:TextBox id="BeginDate" type="date" runat="server" /></td>
            </tr>
            <tr>
                <th>开始日状态:</th>
                <td>
                    <select id="BeginFlag" runat="server">
                        <option value="0">半天</option>
                        <option value="1" selected="selected">全天</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>结束日期:</th>
                <td><asp:TextBox id="EndDate" type="date" runat="server" /></td>
            </tr>
            <tr>
                <th>结束日状态:</th>
                <td>
                    <select id="EndFlag" runat="server">
                        <option value="0">半天</option>
                        <option value="1" selected="selected">全天</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>休假天数</th>
                <td>
                    <asp:Label ID="TravelDays" Text="0.0" runat="server" Width="50px"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label Text="可生成调休天数:" runat="server"></asp:Label>
                    &nbsp;
                    <asp:Label ID="txDays" Text="0.0" runat="server" Width="50px"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnTravelDays" runat="server" Text="计算" OnClick="btnTravelDays_Click" />
                </td>
            </tr>
            <tr>
                <th>公出地点:</th>
                <td><textarea id="Place" runat="server" maxlength="500" rows="3"></textarea></td>
            </tr>
            <tr>
                <th>公出事由:</th>
                <td><textarea id="Reason" runat="server" maxlength="500" rows="3"></textarea></td>
            </tr>
            <tr>
                <th>备注:</th>
                <td><textarea id="Remark" runat="server" maxlength="500" rows="3"></textarea></td>
            </tr>
            <tr>
                <th>附件:</th>
                <td>
                     <input type="file" id="FilesAdd" name="FilesAdd" runat="server" />
                     <asp:Button ID="btn_submit" Text="Upload" runat="server" onclick="btn_submit_Click" />
                </td>
            </tr>
           <tr id="DownFiles" runat="server">
                <th>下载附件:</th>
                <td>
                     <asp:Button ID="btn_down" Text="Download" runat="server" onclick="btn_down_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
    </form>
</body>
</html>
