<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Icons_List.aspx.cs" Inherits="RM.Web.RMBase.SysMenu.Icons_List" %>

<%@ Register Src="/UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单图标</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize();
            $(".divicons").click(function () {
                top.Menu_Form.Get_Menu_Img($(this).attr('title'));
            }).dblclick(function () {
                OpenClose();
            });

        })
        /**自应表格高度**/
        function divresize() {
            resizeU();
            $(window).resize(resizeU);
            function resizeU() {
                $(".div-body").css("height", $(window).height() - 59);
            }
        }
    </script>
    <style type="text/css">
        .divicons
        {
            float: left;
            border: solid 1px #ccc;
            margin: 5px;
            padding: 5px;
            text-align: center;
            cursor: pointer;
        }
        .divicons:hover
        {
            color: #FFF;
            border: solid 1px #3399dd;
            background: #2288cc;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#33bbee', endColorstr='#2288cc');
            background: linear-gradient(top, #33bbee, #2288cc);
            background: -moz-linear-gradient(top, #33bbee, #2288cc);
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#33bbee), to(#2288cc));
            text-shadow: -1px -1px 1px #1c6a9e;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hidden_Size" type="hidden" runat="server" />
    <div class="btnbartitle">
        <div>
            系统图标全取
        </div>
    </div>
    <div class="div-body">
        <%=strImg.ToString() %>
    </div>
    <uc1:PageControl ID="PageControl1" runat="server" />
    </form>
</body>
</html>
