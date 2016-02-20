<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Backup_List.aspx.cs" Inherits="RM.Web.RMBase.SysDataCenter.Backup_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据库备份</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 92); //固定表头
        })
        //数据库备份
        function backups() {
            var url = "/RMBase/SysDataCenter/Backup_Confirm.aspx?type=0";
            top.openDialog(url, 'Backup_Confirm', '数据库 - 备份', 400, 165, 50, 50);
        }
        //执行备份
        function okbackups(Backup_Restore_Memo) {
            $("#Backup_Restore_Memo").val(Backup_Restore_Memo)
            document.getElementById("<%=bntbackups.ClientID%>").click();
        }
        //数据库恢复
        function recover() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            if (IsEditdata(CheckboxValue())) {
                if (arrt[0] == "备份") {
                    $("#Backup_Restore_File").val(arrt[1]);
                    var url = "/RMBase/SysDataCenter/Backup_Confirm.aspx?type=1";
                    top.openDialog(url, 'Backup_Confirm', '数据库 - 恢复', 400, 165, 50, 50);
                } else {
                    showWarningMsg("请选择类型为【备份】!");
                }
            }
        }
        //执行恢复
        function okrecover(Backup_Restore_Memo) {
            $("#Backup_Restore_Memo").val(Backup_Restore_Memo)
            document.getElementById("<%=btnrecover.ClientID%>").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="Backup_Restore_Memo" runat="server" />
    <input type="hidden" id="Backup_Restore_File" runat="server" />
    <div class="btnbartitle">
        <div>
            数据库【备份/恢复】记录
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right;">
            <uc1:LoadButton ID="LoadButton1" runat="server" />
            <asp:Button ID="bntbackups" runat="server" OnClick="bntbackups_Click" Style="display: none" />
            <asp:Button ID="btnrecover" runat="server" OnClick="btnrecover_Click" Style="display: none" />
        </div>
    </div>
    <div class="div-body">
        <table id="table1" class="grid" singleselect="true">
            <thead>
                <tr>
                    <td style="width: 20px; text-align: left;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style="width: 35px; text-align: center;">
                        序号
                    </td>
                    <td style="width: 50px; text-align: center;">
                        类型
                    </td>
                    <td style="width: 100px;">
                        数据库
                    </td>
                    <td style="width: 150px;">
                        文件名
                    </td>
                    <td style="width: 80px; text-align: center;">
                        文件大小
                    </td>
                    <td style="width: 120px; text-align: center;">
                        操作用户
                    </td>
                    <td style="width: 130px; text-align: center;">
                        操作时间
                    </td>
                    <td>
                        说明
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Backup_Restore_Type")%>|<%#Eval("Backup_Restore_File")%>"
                                    name="checkbox" />
                            </td>
                            <td style="width: 35px; text-align: center;">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <asp:Label ID="lblBackup_Restore_Type" runat="server" Text='<%#Eval("Backup_Restore_Type")%>'></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <%#Eval("Backup_Restore_DB")%>
                            </td>
                            <td style="width: 150px;">
                                <%#Eval("Backup_Restore_File")%>
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <%#Eval("Backup_Restore_Size")%>
                            </td>
                            <td style="width: 120px; text-align: center;">
                                <%#Eval("CreateUserName")%>
                            </td>
                            <td style="width: 130px; text-align: center;">
                                <%#Eval("CreateDate")%>
                            </td>
                            <td>
                                <%#Eval("Backup_Restore_Memo")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='8' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
                               }
                           } %>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
