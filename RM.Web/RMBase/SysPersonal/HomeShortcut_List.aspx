<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeShortcut_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.HomeShortcut_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>首页快捷功能列表</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 92);
        })
        //新增
        function add() {
            var url = "/RMBase/SysPersonal/HomeShortcut_Form.aspx";
            top.openDialog(url, 'Menu_Form', '首页快捷功能信息 - 添加', 450, 300, 50, 50);
        }
        //编辑
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/RMBase/SysPersonal/HomeShortcut_Form.aspx?key=" + key;
                top.openDialog(url, 'Menu_Form', '首页快捷功能信息 - 编辑', 450, 300, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var delparm = 'action=Delete&tableName=Base_O_A_Setup&pkName=Setup_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            首页快捷功能
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right">
            <uc2:LoadButton ID="LoadButton1" runat="server" />
        </div>
    </div>
    <div class="div-body">
        <table id="table1" class="grid">
            <thead>
                <tr>
                    <td style="width: 20px; text-align: left;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style="width: 30px; text-align: center;">
                        图标
                    </td>
                    <td style="width: 100px;">
                        快捷功能
                    </td>
                    <td style="width: 60px; text-align: center;">
                        连接目标
                    </td>
                    <td>
                        连接地址
                    </td>
                    <td style="width: 60px; text-align: center;">
                        显示顺序
                    </td>
                    <td>
                        描述
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Setup_ID")%>" name="checkbox" />
                            </td>
                            <td style="width: 30px; text-align: center;">
                                <img src='/Themes/images/32/<%#Eval("Setup_Img")%>' style='width: 16px; height: 16px;
                                    vertical-align: middle;' alt='' />
                            </td>
                            <td style="width: 100px;">
                                <%#Eval("Setup_IName")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("Target")%>
                            </td>
                            <td>
                                <%#Eval("NavigateUrl")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("SortCode")%>
                            </td>
                            <td>
                                <%#Eval("Setup_Remak")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='7' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
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
