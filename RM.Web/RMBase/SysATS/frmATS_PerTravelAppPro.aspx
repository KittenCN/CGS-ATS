<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_PerTravelAppPro.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_PerTravelAppPro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>审批公出申请</title>
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
                <th>Emp:</th>
                <td><asp:label id="EmpID" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Create Date:</th>
                <td><asp:label id="CreateDate" runat="server" /></td>
            </tr>
            <tr>
                <th>Begin Date:</th>
                <td><asp:TextBox id="BeginDate" type="date" runat="server"  Enabled="false"/></td>
            </tr>
            <tr>
                <th>Begin Status:</th>
                <td>
                    <select id="BeginFlag" runat="server" aria-readonly="true">
                        <option value="0">Afternoon</option>
                        <option value="1" selected="selected">Morning</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>End Date:</th>
                <td><asp:TextBox id="EndDate" type="date" runat="server"  Enabled="false"/></td>
            </tr>
            <tr>
                <th>End Status:</th>
                <td>
                    <select id="EndFlag" runat="server" aria-readonly="true">
                        <option value="0">Morning</option>
                        <option value="1" selected="selected">Aftermoon</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>Travel Days</th>
                <td>
                    <asp:Label ID="TravelDays" Text="0.0" runat="server" Width="50px"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label Text="Bring Num of CL:" runat="server"></asp:Label>
                    &nbsp;
                    <asp:Label ID="txDays" Text="0.0" runat="server" Width="50px"></asp:Label>
                    <%--&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnTravelDays" runat="server" Text="Calculation" OnClick="btnTravelDays_Click" />--%>
                </td>
            </tr>
            <tr>
                <th>Destination:</th>
                <td><textarea id="Place" runat="server" maxlength="500" rows="3" readonly="readonly"></textarea></td>
            </tr>
            <tr>
                <th>Reason:</th>
                <td><textarea id="Reason" runat="server" maxlength="500" rows="3" readonly="readonly"></textarea></td>
            </tr>
            <tr>
                <th>Remark:</th>
                <td><textarea id="Remark" runat="server" maxlength="500" rows="3" readonly="readonly"></textarea></td>
            </tr>
            <tr>
                <th>Attachment:</th>
                <td>
                     <input type="file" id="FilesAdd" name="FilesAdd" runat="server"  aria-readonly="true"/>
                     <asp:Button ID="btn_submit" Text="Upload" runat="server" onclick="btn_submit_Click"  Enabled="false"/>
                </td>
            </tr>
           <tr id="DownFiles" runat="server">
                <th>Download Attachment:</th>
                <td>
                     <asp:Button ID="btn_down" Text="Download" runat="server" onclick="btn_down_Click" />
                </td>
            </tr>
            <tr>
                <th>Approver Remark:</th>
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
