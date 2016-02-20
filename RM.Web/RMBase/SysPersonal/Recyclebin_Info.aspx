<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recyclebin_Info.aspx.cs"
    Inherits="RM.Web.RMBase.SysPersonal.Recyclebin_Info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回收站数据详细信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            Setform();
        })
        //表单赋值
        function Setform() {
            var strArray = new Array();
            var strArray1 = new Array();
            var item_value = '<%=_ItemValue %>'; //后台返回值
            strArray = item_value.split('∮');
            for (var i = 0; i < strArray.length; i++) {
                var item_value1 = strArray[i];
                strArray1 = item_value1.split('∫');
                $("#" + strArray1[0]).text(strArray1[1]);
            }
        }
    </script>
    <style type="text/css">
        .frm th
        {
            border-right: 1px solid #ccc;
            padding-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            【<%=_Recyclebin_Name%>】- 相关信息
        </div>
    </div>
    <div class="btnbarcontetn">
        <div style="text-align: right">
            <a href="javascript:void(0)" title="返 回" onclick="back();" class="button green"><span
                class="icon-botton" style="background: url('/Themes/images/16/back.png') no-repeat scroll 0px 4px;">
            </span>返 回</a>
        </div>
    </div>
    <div class="div-body" style="padding-top: 1px;">
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <%=str_OutputHtml.ToString()%>
        </table>
    </div>
    </form>
</body>
</html>
