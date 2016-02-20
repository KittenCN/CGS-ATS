<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_OriDataIn.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_OriDataIn" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>考勤原始数据导入</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
    <div>
        <div style="text-align: left;" >
            <input type="file" id="OriDataFile" name="OriDataFile" runat="server" />
            <%--<input type="submit" id="Sub_Submit" name="sub_Submit" value="Upload" runat="server" onclick="Sub_Submit_ServerClick" />--%>
            <asp:Button ID="btn_submit" Text="Upload" runat="server" onclick="btn_submit_Click" />
        </div>
        <div id="GV_div" runat="server" visible="false">
            <asp:GridView ID="GV_OriData" runat="server">
            </asp:GridView>
        </div>
    </div>
    <div style="overflow-x: auto; overflow-y: scroll;height:auto!important; width:auto!important;">
        <table id="table1" class="grid" singleselect="true">
            <thead>
                <tr>
                    <td style="width: 20px; text-align: left;">
                        <label id="checkAllOff">
                            &nbsp;</label>
                    </td>
                    <td style="width: 100px; text-align: center;">
                        部门
                    </td>
                    <td style="width: 80px; text-align: center;">
                        编号
                    </td>
                    <td style="width: 100px; text-align: center;">
                        姓名
                    </td>
                    <td style="width: 80px; text-align: center;">
                        登记号码
                    </td>
                    <td style="width: 80px; text-align: center;">
                        设备号
                    </td>
                    <td style="width: 80px; text-align: center;">
                        位置
                    </td>
                    <td style="width: 80px; text-align: center;">
                        日期
                    </td>
                    <td style="width: 80px; text-align: center;">
                        时间
                    </td>
                    <td style="width: 80px; text-align: center;">
                        考勤类型
                    </td>
                    <td style="width: auto!important; text-align: center;">
                        备注
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound" >
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("id")%>" name="checkbox" />
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("Dept")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("OriID")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("Name")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("RegID")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("DeviceID")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("Posion")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("OriData","{0:d}")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("OriTime","{0:T}")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("ATSstatus")%>
                            </td>
                            <td>
                                <%#Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='8' style='color:red;text-align:center'>没有数据！</td></tr>");
                               }
                           } %>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
        <uc1:PageControl ID="PageControl1" runat="server" />
    </form>
</body>
</html>
