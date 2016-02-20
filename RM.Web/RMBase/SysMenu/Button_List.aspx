<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Button_List.aspx.cs" Inherits="RM.Web.RMBase.SysMenu.Button_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>按钮设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 91);
        })
        function add() {
            var url = "/RMBase/SysMenu/Button_Form.aspx";
            top.openDialog(url, 'Menu_Form', '菜单按钮信息 - 添加', 450, 325, 50, 50);
        }
        function edit() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(CheckboxValue())) {
                var url = "/RMBase/SysMenu/Button_Form.aspx?key=" + key;
                top.openDialog(url, 'Menu_Form', '菜单按钮信息 - 编辑', 450, 325, 50, 50);
            }
        }
        //删除
        function Delete() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(CheckboxValue())) {
                var isExistparm = 'action=IsExist&tableName=Base_SysMenu&pkName=Menu_Name&pkVal=' + escape(arrt[1]);
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=操作按钮&tableName=Base_Button&pkName=Button_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            按钮信息列表
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right;">
            <uc1:LoadButton ID="LoadButton1" runat="server" />
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
                        图标
                    </td>
                    <td style="width: 80px; text-align: center;">
                        名称
                    </td>
                    <td style="width: 60px; text-align: center;">
                        位置
                    </td>
                    <td style="width: 120px;">
                        事件
                    </td>
                    <td style="width: 50px; text-align: center;">
                        显示顺序
                    </td>
                    <td style="width: 120px; text-align: center;">
                        创建用户
                    </td>
                    <td style="width: 120px; text-align: center;">
                        创建时间
                    </td>
                    <td style="width: 120px; text-align: center;">
                        修改用户
                    </td>
                    <td style="width: 120px; text-align: center;">
                        修改时间
                    </td>
                    <td>
                        说明
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Button_ID")%>|<%#Eval("Button_Name")%>" name="checkbox" />
                            </td>
                            <td style="width: 35px; text-align: center;">
                                <img src='/Themes/images/16/<%#Eval("Button_Img")%>' style='width: 16px; height: 16px;
                                    vertical-align: middle;' alt='' />
                            </td>
                            <td style="width: 80px; text-align: center;">
                                <%#Eval("Button_Name")%>
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <%#Eval("Button_Type")%>
                            </td>
                            <td style="width: 120px;">
                                <%#Eval("Button_Code")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("SortCode")%>
                            </td>
                            <td style="width: 120px; text-align: center;">
                                <%#Eval("CreateUserName")%>
                            </td>
                            <td style="width: 120px; text-align: center;">
                                <%#Eval("CreateDate", "{0:yyyy-MM-dd HH:mm}")%>
                            </td>
                            <td style="width: 120px; text-align: center;">
                                <%#Eval("ModifyUserName")%>
                            </td>
                            <td style="width: 120px; text-align: center;">
                                <%#Eval("ModifyDate", "{0:yyyy-MM-dd HH:mm}")%>
                            </td>
                            <td>
                                <%#Eval("Button_Remak")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='11' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
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
