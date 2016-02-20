<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotAuthority_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysRole.AllotAuthority_Form" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色分配权限</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 94);
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        })
        //返回
        function back() {
            Urlhref('/RMBase/SysRole/Role_List.aspx');
        }
        //保存
        function SaveForm() {
            showConfirmMsg('注：您确认要保存此操作吗？', function (r) {
                if (r) {
                    var item = CheckboxValue();
                    $("#item_hidden").val(item);
                    document.getElementById("<%=Save.ClientID%>").click();
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="item_hidden" type="hidden" runat="server" />
    <div class="btnbartitle">
        <div>
            所属角色【<%=_Roles_Name.ToString()%>】 &nbsp;&nbsp;<span style="color: Red;">注：分配权限 - 该功能谨慎使用！</span>
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right">
            <asp:Button id="Save" runat="server" OnClick="Save_Click" style="display:none" />
            <uc1:LoadButton ID="LoadButton1" runat="server" />
        </div>
    </div>
    <div class="div-body">
        <table class="example" id="dnd-example">
            <thead>
                <tr>
                    <td style="width: 203px; padding-left: 20px;">
                        URL菜单权限
                    </td>
                    <td style="width: 30px; text-align: center;">
                        图标
                    </td>
                    <td style="width: 20px; text-align: center;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td>
                        操作按钮权限
                    </td>
                </tr>
            </thead>
            <tbody>
                <%=StrTree_Menu.ToString()%>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
