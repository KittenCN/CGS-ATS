<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataCenter_Conten.aspx.cs"
    Inherits="RM.Web.RMBase.SysDataCenter.DataCenter_Conten" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>表结构描述</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 107);
            
        })
        //新建查询
        function addSQLQuery() {
            var Table_Name = '<%=_Table_Name %>';
            var url = "SQLQuery.aspx?Table_Name=" + escape(Table_Name);
            Urlhref(url);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            【<%=_Table_Name.ToString() %>】表结构描述
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right;">
            <a href="javascript:void(0)" title="新建查询" onclick="addSQLQuery();" class="button green">
                <span class="icon-botton" style="background: url('/Themes/images/201208010567.png') no-repeat scroll 0px 4px;">
                </span>新建查询</a>
        </div>
    </div>
    <div class="div-body">
        <table id="table1" class="grid" singleselect="true">
            <thead>
                <tr>
                    <td style="width: 40px; text-align: center;">
                        序号
                    </td>
                    <td style="width: 200px;">
                        字段名
                    </td>
                    <td style="width: 100px; text-align: center;">
                        数据类型
                    </td>
                    <td style="width: 50px; text-align: center;">
                        长度
                    </td>
                    <td style="width: 50px; text-align: center;">
                        允许空
                    </td>
                    <td style="width: 100px; text-align: center;">
                        默认值
                    </td>
                    <td>
                        字段说明
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rp_Item" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 40px; text-align: center;">
                                <%# Container.ItemIndex + 1%>
                            </td>
                            <td style="width: 200px;">
                                <%#Eval("列名")%>
                            </td>
                            <td style="width: 100px; text-align: center;">
                                <%#Eval("数据类型")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("长度")%>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <%#Eval("是否为空")%>
                            </td>
                            <td style="width: 100px; text-align: center;">
                                <%#Eval("默认值")%>
                            </td>
                            <td>
                                <%#Eval("说明")%>
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
