<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SQLQuery.aspx.cs" Inherits="RM.Web.RMBase.SysDataCenter.SQLQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新建查询</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                var obtnSearch = document.getElementById("ExeOuter");
                obtnSearch.focus(); //让另一个控件获得焦点就等于让文本输入框失去焦点
                obtnSearch.click();
            }
        }
        //初始化
        $(function () {
            divresize();
            $("#txtSql").css("height", "152").css("width", $(window).width() - 7);
        })
        /**自应表格高度**/
        function divresize() {
            resizeU();
            $(window).resize(resizeU);
            function resizeU() {
                $("#divGrid").css("height", $(window).height() - 221);
                $("#divtxt").css("height", "155");
            }
        }
        //返回
        function Back() {
            var Table_Name = '<%=_Table_Name %>';
            var url = "DataCenter_Conten.aspx?Table_Name=" + escape(Table_Name);
            Urlhref(url);
        }

    </script>
    <style type="text/css">
        .tableval tbody tr td
        {
            text-align: left;
            border-bottom: 1px solid #ccc;
            border-right: 1px dotted #ccc;
            padding: 1px 2px;
            width: 1px;
            word-break: keep-all; /* for ie */
            white-space: nowrap; /* for chrome */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            后台SQL执行 &nbsp;&nbsp;<span style="color: Red;">注：SQL 查询（最大显示500条）</span>
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="float: left; padding-top: 2px;">
            执行类型：<select id="Execute_Type" class="select" runat="server">
                <option value="1">查询列表</option>
                <option value="2">更新与删除</option>
            </select>
        </div>
        <div style="text-align: right;">
            <asp:LinkButton ID="ExeOuter" runat="server" class="button green" OnClick="ExeOuter_Click"><span
                class="icon-botton" style="background: url('/Themes/images/lightning.png') no-repeat scroll 0px 4px;">
            </span>执 行</asp:LinkButton>
            <a href="javascript:void(0)" title="返 回" onclick="Back()" class="button green"><span
                class="icon-botton" style="background: url('/Themes/images/16/back.png') no-repeat scroll 0px 4px;">
            </span>返 回</a>
        </div>
    </div>
    <div id="divtxt" class="div-body" style="border-top: 1px solid #ccc;">
        <textarea id="txtSql" runat="server" style="border: 0px solid #A8A8A8;"></textarea>
    </div>
    <div id="divGrid" class="div-body" style="padding-top: 1px;">
        <asp:GridView ID="Grid" CssClass="tableval" runat="server" BorderStyle="None" CellPadding="0"
            GridLines="None">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
