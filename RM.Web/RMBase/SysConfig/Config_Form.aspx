<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config_Form.aspx.cs" Inherits="RM.Web.RMBase.SysConfig.Config_Form" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统配置信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(62);
            Setform();
            $("#table1 tr td").dblclick(function () {
                var obj = $(this);
                var tdwidth = obj.width() - 20;
                //取出当前td的文本内容保存起来
                var oldText = $(this).text();
                //建立一个文本框，设置文本框的值为保存的值   
                var input = $("<input class=\"txt\" datacol=\"yes\" err=\"\" checkexpession=\"NotNull\" style=\"width: " + tdwidth + "px;\" type='text' value='" + oldText + "'/>");
                obj.html(input);
                //设置文本框的点击事件失效
                input.click(function () {
                    return false;
                });
                //当文本框得到焦点时触发全选事件  
                input.trigger("focus").trigger("select");
                input.blur(function () {
                    var input_blur = $(this);
                    //保存当前文本框的内容
                    var newText = input_blur.val();
                    obj.html(newText);
                });
            })
        })
        //附加信息表单赋值
        function Setform() {
            var strArray = new Array();
            var strArray1 = new Array();
            var item_value = $("#AppendProperty_value").val(); //后台返回值
            strArray = item_value.split('∮');
            for (var i = 0; i < strArray.length; i++) {
                var item_value1 = strArray[i];
                strArray1 = item_value1.split('∫');
                $("#" + strArray1[0]).text(strArray1[1]);
            }
        }
        //点击切换面板
        function panel(obj) {
            if (obj == 1) {
                $('#table1').show();
                $('#table2').hide();
            } else if (obj == 2) {
                $('#table1').hide();
                $("#table2").show();
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
    <%--    获取附加信息值--%>
    <input id="AppendProperty_value" type="hidden" runat="server" />
    <div class="btnbartitle">
        <div>
            系统配置信息
        </div>
    </div>
    <div class="btnbarcontetn">
        <div>
            <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="menutab" style="vertical-align: bottom;">
                        <div id="tab0" class="Tabsel" onclick="GetTabClick(this);panel(1)">
                            系统信息</div>
                    </td>
                </tr>
            </table>
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
