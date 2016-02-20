<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppendProperty_Function.aspx.cs"
    Inherits="RM.Web.RMBase.SysAppend.AppendProperty_Function" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>附加属性所属功能</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
        })
        //添加
        function add() {
            var url = "/RMBase/SysAppend/AppendProperty_Function_Form.aspx";
            top.openDialog(url, 'AppendProperty_Function_Form', '附加属性所属功能 - 添加', 350, 105, 50, 50);
        }
        //修改
        function edit() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(CheckboxValue())) {
                var url = "/RMBase/SysAppend/AppendProperty_Function_Form.aspx?key=" + key;
                top.openDialog(url, 'AppendProperty_Function_Form', '附加属性所属功能 - 编辑', 350, 105, 50, 50);
            }
        }
        //删除
        function Delete() {
            var arrt = new Array();
            arrt = CheckboxValue().split('|');
            var key = arrt[0];
            if (IsEditdata(CheckboxValue())) {
                var isExistparm = 'action=IsExist&tableName=Base_AppendProperty&pkName=Property_Function&pkVal=' + escape(arrt[1]);
                if (IsExist_Data('/Ajax/Common_Ajax.ashx', isExistparm) > 1) {
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
            附加属性设置
        </div>
    </div>
    <div class="btnbarcontetn" style="margin-bottom: 1px;">
        <div style="float: left;">
            <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="menutab" style="vertical-align: bottom;">
                        <div id="tab0" class="Tabremovesel" onclick="GetTabClick(this);Urlhref('AppendProperty_Index.aspx');">
                            附加资料</div>
                        <div id="tab1" class="Tabsel" onclick="GetTabClick(this);Urlhref('AppendProperty_Function.aspx');">
                            所属功能</div>
                    </td>
                </tr>
            </table>
        </div>
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
                        所属功能
                    </td>
                    <td>
                        功能路径
                    </td>
                    <td style="width: 120px;">
                        创建用户
                    </td>
                    <td style="width: 120px;">
                        创建时间
                    </td>
                    <td style="width: 120px;">
                        修改用户
                    </td>
                    <td style="width: 120px;">
                        修改时间
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20px; text-align: left;">
                                <input type="checkbox" value="<%#Eval("Property_ID")%>|<%#Eval("Property_Function")%>"
                                    name="checkbox" />
                            </td>
                            <td style="width: 150px;">
                                <%#Eval("Property_Function")%>
                            </td>
                            <td>
                                <%#Eval("Property_FunctionUrl")%>
                            </td>
                            <td style="width: 120px;">
                                <%#Eval("CreateUserName")%>
                            </td>
                            <td style="width: 120px;">
                                <%#Eval("CreateDate", "{0:yyyy-MM-dd HH:mm}")%>
                            </td>
                            <td style="width: 120px;">
                                <%#Eval("ModifyUserName")%>
                            </td>
                            <td style="width: 120px;">
                                <%#Eval("ModifyDate", "{0:yyyy-MM-dd HH:mm}")%>
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
<script language="javascript" type="text/javascript">
    $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
</script>
