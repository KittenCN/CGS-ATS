<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotButton_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysMenu.AllotButton_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分配按钮</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#allButton div").dblclick(function () {
                var IsExist = true;
                var all = $(this).attr('title');
                $('#selectedButton div').each(function (i) {
                    if ($(this).attr('title') == all) {
                        IsExist = false;
                        return false;
                    }
                })
                if (IsExist == true) {
                    //$("#selectedButton").append("<div id=" + $(this).attr('id') + ";add" + " onclick='selectedButton(this)' ondblclick='$(this).remove()' title='" + $(this).attr('title') + "' class=\"shortcuticons\">" + $(this).html() + "</div>");
                    var ParentId = '<%=_ParentId %>'
                    var parm = 'action=addButton&ParentId=' + ParentId + '&key=' + $(this).attr('id');
                    getAjax('Menu_List.ashx', parm, function (rs) {
                        if (parseInt(rs) > 0) {
                            showTipsMsg("添加成功！", 2000, 4);
                            windowload();
                        }
                        else {
                            showTipsMsg("操作失败，请稍后重试！", 4000, 5);
                        }
                    });
                } else {
                    showWarningMsg("【" + all + "】按钮已经存在");
                }
            });
            $("#Buttonlist").height(358);
            $(".div-overflow").height(318);
        })
        //移除按钮
        function removeButton(id) {
            var delparm = 'action=Delete&tableName=Base_SysMenu&pkName=Menu_Id&pkVal=' + id;
            DeleteData('/Ajax/Common_Ajax.ashx', delparm)
        }
        function selectedButton(e) {
            $('.shortcuticons').removeClass("selected");
            $(e).addClass("selected"); //添加选中样式
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            var Button_ID = "";
            $('#selectedButton div').each(function (i) {
                Button_ID += $(this).attr('id') + ",";
            })
            $("#Button_ID_Hidden").val(Button_ID);
            if (!confirm('注：确认要保存此操作吗？')) {
                return false;
            }
        }
    </script>
    <style type="text/css">
        .shortcuticons
        {
            float: left;
            border: solid 1px #ccc;
            width: 47px;
            height: 35px;
            margin: 5px;
            padding: 3px;
            cursor: pointer;
            vertical-align: middle;
            text-align: center;
            word-break: keep-all;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .shortcuticons:hover
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
        .selected
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
    <input id="Button_ID_Hidden" type="hidden" runat="server" />
    <div id="Buttonlist">
        <div id="Buttonlistleft" style="float: left; width: 48.6%; border: 1px solid #ccc;
            margin-right: 5px;">
            <div class="box-title" style="height: 25px;">
                所有按钮;<span style="color: Blue;">双击添加</span>
            </div>
            <div class="div-overflow" id="allButton" style="overflow-x: hidden; overflow-y: scroll;
                margin-bottom: 5px;">
                <%=ButtonList.ToString() %>
            </div>
        </div>
        <div id="Buttonlistright" style="float: left; width: 48.6%; border: 1px solid #ccc;">
            <div class="box-title" style="height: 25px;">
                已选按钮;<span style="color: Red;">双击移除</span>
            </div>
            <div class="div-overflow" id="selectedButton" style="overflow-x: hidden; overflow-y: scroll;
                margin-bottom: 5px;">
                <%=selectedButtonList.ToString() %>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
