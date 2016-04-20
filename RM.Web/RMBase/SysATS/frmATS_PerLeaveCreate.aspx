<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_PerLeaveCreate.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_PerLeaveCreate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>Create Leave</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
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
               <th>Effective AL:</th>
               <td><asp:Label ID="njDays" runat="server"></asp:Label></td>
           </tr>
           <tr>
               <th>Effective CL:</th>
               <td><asp:Label ID="txDays" runat="server"></asp:Label></td>
           </tr>                                                                    
            <tr>
                <th>Create Date:</th>
                <td><asp:label id="CreateDate" runat="server" /></td>
            </tr>
            <tr>
                <th>Type:</th>
                <td>
                    <asp:DropDownList ID="LeaveID" runat="server" OnSelectedIndexChanged="LeaveID_SelectedIndexChanged" AutoPostBack="true">

                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Begin Date:</th>
                <td><asp:TextBox id="BeginDate" type="date" runat="server" /></td>
            </tr>
            <tr>
                <th>Begin Status:</th>
                <td>
                    <select id="BeginFlag" runat="server">
                        <option value="1" selected="selected">Morning</option>
                        <option value="0">Afternoon</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>End Date:</th>
                <td><asp:TextBox id="EndDate" type="date" runat="server" /></td>
            </tr>
            <tr>
                <th>End Status:</th>
                <td>
                    <select id="EndFlag" runat="server">
                        <option value="0">Morning</option>
                        <option value="1" selected="selected">Afternoon</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>Leave Days</th>
                <td>
                    <asp:Label ID="LeaveDays" runat="server"></asp:Label>
                    <asp:Button ID="btnLeaveDays" runat="server" OnClick="btnLeaveDays_Click" />
                </td>
            </tr>
            <tr>
                <th>Remark:</th>
                <td><textarea id="Remark" runat="server" maxlength="500" rows="3"></textarea></td>
            </tr>
            <tr>
                <th>Attachment:</th>
                <td>
                     <input type="file" id="FilesAdd" name="FilesAdd" runat="server" />
                     <asp:Button ID="btn_submit" Text="Upload" runat="server" onclick="btn_submit_Click" />
                </td>
            </tr>
        </table>
        <table id="CJform" runat="server" class="frm">
            <tr>
                <th>Difficult labor:</th>
                <td>
                    <asp:CheckBox ID="cbNC" Text="Difficult labor" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Multiple births:</th>
                <td>

                    <asp:CheckBox ID="cbDBT" Text="Multiple births:" runat="server" OnCheckedChanged="DBT_CheckedChanged" AutoPostBack="true" />
&nbsp;
                    <asp:TextBox ID="DBT" runat="server" Width="16px" Enabled="false">0</asp:TextBox>
                    <asp:Label ID="lbDBT" runat="server" Text="Babies"></asp:Label>

                </td>
            </tr>           
        </table>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />Save</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />Close</span></a>
    </div>
    </form>
</body>
</html>
