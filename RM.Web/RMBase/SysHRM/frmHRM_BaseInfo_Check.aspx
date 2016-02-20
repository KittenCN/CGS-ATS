<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmHRM_BaseInfo_Check.aspx.cs" Inherits="RM.Web.RMBase.SysHRM.frmHRM_BaseInfo_Check" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>组织机构部门表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 211px;
        }
    </style>
    </head>
<%--    <script language="JavaScript">
        var a;
        function CheckValuePiece() {
            if (window.document.form1.GoPage.value == "") {
                alert("请输入跳转的页码！");
                window.document.form1.GoPage.focus();
                return false;
            }
            return true;
        }
        function CheckAll() {
            if (a == 1) {
                for (var i = 0; i < window.document.form1.elements.length; i++) {
                    var e = form1.elements[i];
                    e.checked = false;
                }
                a = 0;
            }
            else {
                for (var i = 0; i < window.document.form1.elements.length; i++) {
                    var e = form1.elements[i];
                    e.checked = true;
                }
                a = 1;
            }
        }
        function CheckDel() {
            var number = 0;
            for (var i = 0; i < window.document.form1.elements.length; i++) {
                var e = form1.elements[i];
                if (e.Name != "CheckBoxAll") {
                    if (e.checked == true) {
                        number = number + 1;
                    }
                }
            }
            if (number == 0) {
                alert("请选择需要删除的项！");
                return false;
            }
            if (window.confirm("你确认删除吗？")) {
                return true;
            }
            else {
                return false;
            }
        }

        function CheckModify() {
            var Modifynumber = 0;
            for (var i = 0; i < window.document.form1.elements.length; i++) {
                var e = form1.elements[i];
                if (e.Name != "CheckBoxAll") {
                    if (e.checked == true) {
                        Modifynumber = Modifynumber + 1;
                    }
                }
            }
            if (Modifynumber == 0) {
                alert("请至少选择一项！");
                return false;
            }
            if (Modifynumber > 1) {
                alert("只允许选择一项！");
                return false;
            }

            return true;
        }

</script>--%>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                &nbsp;</th>
            <td>
                &nbsp;</td>
            <th class="auto-style1">
                &nbsp;</th>
            <td dir="rtl">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            </td>
        </tr>
        <tr>
            <th>
                编号:
            </th>
            <td>
                <input id="id" maxlength="7" runat="server" type="text" class="txt"
                    datacol="yes" checkexpession="NotNull" style="width: 200px" />
            </td>
            <th class="auto-style1">
                名称:
            </th>
            <td>
                <input id="name" runat="server" type="text" class="txt" datacol="yes"
                    checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                &nbsp;</th>
            <td colspan="3">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" style="margin-left: 0px" Width="1405px">
                    <Columns>
                <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LabVisible" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ID")%>'
                                    Visible="False"></asp:Label><asp:CheckBox ID="CheckSelect" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <%--<input id="CheckBoxAll" onclick="CheckAll()" type="checkbox" />--%>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                        <asp:BoundField HeaderText="员工工号" DataField="id" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="姓名" DataField="name" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="生日" DataField="birthday" DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="入职前社会工龄" DataField="Sage_b" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="入职日期" DataField="join_date" DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="当前社会工龄" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="离职日期" DataField="out_date" DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="工作状态" DataField="work_flag" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="直属上司" DataField="Boss_id" >
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                    </tr>
<%--        <tr>
            <td style="border-top: #000000 1px solid; border-bottom: #000000 1px solid" aria-hidden="True">
                <asp:ImageButton ID="BtnFirst" runat="server" CommandName="First" ImageUrl="../images/Button/First.jpg"
                    OnClick="PagerButtonClick" />
                <asp:ImageButton ID="BtnPre" runat="server" CommandName="Pre" ImageUrl="../images/Button/Pre.jpg"
                    OnClick="PagerButtonClick" />
                <asp:ImageButton ID="BtnNext" runat="server" CommandName="Next" ImageUrl="../images/Button/Next.jpg"
                    OnClick="PagerButtonClick" />
                <asp:ImageButton ID="BtnLast" runat="server" CommandName="Last" ImageUrl="../images/Button/Last.jpg"
                    OnClick="PagerButtonClick" />
                &nbsp;第<asp:Label ID="LabCurrentPage" runat="server" Text="Label"></asp:Label>页&nbsp;
                共<asp:Label ID="LabPageSum" runat="server" Text="Label"></asp:Label>页&nbsp;
                <asp:TextBox ID="TxtPageSize" runat="server" CssClass="TextBoxCssUnder2" Height="20px"
                    Width="35px">15</asp:TextBox>
                行每页 &nbsp; 转到第<asp:TextBox ID="GoPage" runat="server" CssClass="TextBoxCssUnder2"
                    Height="20px" Width="33px"></asp:TextBox>
                页&nbsp;
                <asp:ImageButton ID="ButtonGo" runat="server" OnClientClick="javascript:return CheckValuePiece();"
                    ImageUrl="../images/Button/Jump.jpg" OnClick="ButtonGo_Click" />
            </td>
        </tr>--%>
<%--            </td>
        </tr>--%>
    </table>
    <div class="frmbottom">
    </div>
    </form>
</body>
</html>

