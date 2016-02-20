<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppendProperty_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysAppend.AppendProperty_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>附加属性信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(65);
            FixedTableHeader("#table1", $(window).height() - 93); //固定表头
        })
        //添加
        function add() {
            var Function_Name = '<%=_Function %>';
            if (Function_Name != "未选择") {
                var url = "/RMBase/SysAppend/AppendProperty_Form.aspx?Function=" + escape(Function_Name);
                top.openDialog(url, 'AppendProperty_Form', '【' + Function_Name + '】 - 添加', 700, 360, 50, 50);
            } else {
                showWarningMsg("请选择左边所属功能一项！");
            }
        }
        //修改
        function edit() {
            var Function_Name = '<%=_Function %>';
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(key)) {
                var url = "/RMBase/SysAppend/AppendProperty_Form.aspx?key=" + key;
                top.openDialog(url, 'AppendProperty_Form', '【' + Function_Name + '】 - 编辑', 700, 360, 50, 50);
            }
        }
        //删除
        function Delete() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(CheckboxValue())) {
                var isExistparm = 'action=IsExist&tableName=Base_AppendPropertyInstance&pkName=Property_Control_ID&pkVal=' + escape(arrt[1]);
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 0) {
                    showWarningMsg("该数据被关联,0 行受影响！");
                    return false;
                }
                var delparm = 'action=Virtualdelete&module=附加属性&tableName=Base_AppendProperty&pkName=Property_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
            }
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            所属功能【<%=_Function.ToString()%>】&nbsp;&nbsp;<span style="color: Red;">注：该功能谨慎使用！</span>
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
                    <td style="width: 150px;">
                        属性名称
                    </td>
                    <td style="width: 150px;">
                        控件ID
                    </td>
                    <td style="width: 50px; text-align: center;">
                        控件类型
                    </td>
                    <td style="width: 50px; text-align: center;">
                        控件样式
                    </td>
                    <td style="width: 50px; text-align: center;">
                        控件长度
                    </td>
                    <td style="width: 50px; text-align: center;">
                        显示顺序
                    </td>
                    <td>
                        对象资源
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Property_ID")%>| <%#Eval("Property_Control_ID")%>"
                                    name="checkbox" />
                            </td>
                            <td style="width: 150px;">
                                <%#Eval("Property_Name")%>
                            </td>
                            <td style="width: 150px;">
                                <%#Eval("Property_Control_ID")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <asp:Label ID="lblProperty_Control_Type" runat="server" Text='<%#Eval("Property_Control_Type")%>'></asp:Label>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("Property_Control_Style")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("Property_Control_Length")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("SortCode")%>
                            </td>
                            <td>
                                <%#Eval("Property_Control_DataSource")%>
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
