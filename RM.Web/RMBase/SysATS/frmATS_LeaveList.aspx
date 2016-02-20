<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmATS_LeaveList.aspx.cs" Inherits="RM.Web.RMBase.SysATS.frmATS_LeaveList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>休假设置明细</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
 <%--   <script type="text/javascript">
                    function YYYYMMDDstart()
                    {
                        MonHead = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                        //先给年下拉框赋内容
                        var y   = new Date().getFullYear();
                        for (var i = (y-30); i < (y+30); i++) //以今年为准，前30年，后30年
                            document.form1.YYYY.options.add(new Option(" "+ i +" 年", i));

                        //赋月份的下拉框
                        for (var i = 1; i < 13; i++)
                            document.form1.MM.options.add(new Option(" " + i + " 月", i));

                        document.form1.YYYY.value = y;
                        document.form1.MM.value = new Date().getMonth() + 1;
                        var n = MonHead[new Date().getMonth()];
                        if (new Date().getMonth() ==1 && IsPinYear(YYYYvalue)) n++;
                            writeDay(n); //赋日期下拉框
                        document.form1.DD.value = new Date().getDate();
                    }
                    if(document.attachEvent)
                        window.attachEvent("onload", YYYYMMDDstart);
                    else
                        window.addEventListener('load', YYYYMMDDstart, false);
                    function YYYYDD(str) //年发生变化时日期发生变化(主要是判断闰平年)
                    {
                        var MMvalue = document.form1.MM.options[document.form1.MM.selectedIndex].value;
                        if (MMvalue == ""){ var e = document.form1.DD; optionsClear(e); return;}
                        var n = MonHead[MMvalue - 1];
                        if (MMvalue ==2 && IsPinYear(str)) n++;
                            writeDay(n)
                    }
                    function MMDD(str)  //月发生变化时日期联动
                    {
                        var YYYYvalue = document.form1.YYYY.options[document.form1.YYYY.selectedIndex].value;
                        if (YYYYvalue == ""){ var e = document.form1.DD; optionsClear(e); return;}
                        var n = MonHead[str - 1];
                        if (str ==2 && IsPinYear(YYYYvalue)) n++;
                            writeDay(n)
                    }
                    function writeDay(n)  //据条件写日期的下拉框
                    {
                        var e = document.form1.DD; optionsClear(e);
                        for (var i=1; i<(n+1); i++)
                            e.options.add(new Option(" "+ i + " 日", i));
                    }
                    function IsPinYear(year)//判断是否闰平年
                    {  
                        return(0 == year%4 && (year%100 !=0 || year%400 == 0));
                    }
                    function optionsClear(e)
                    {
                        e.options.length = 1;
                    }
           </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <table class="frm">
        <tr>
            <th>休假名称:</th>
            <td><input id="LeaveName" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>每次申请最多天数:</th>
            <td><input id="MaxPerTime" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>每年申请最多天数:</th>
            <td><input id="MaxPerYear" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>是否必须附件:</th>
            <td>
                <select id="MustFile" name="MustFile" runat="server">
                    <option value="0" selected="selected">否</option>
                    <option value="1">是</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>每次折算小时数:</th>
            <td><input id="DisHour" type="text" class="txt" runat="server" /></td>
        </tr>
    </table>
    <table id="NJfrm" class="frm" runat="server" visible="false">
        <tr>
            <th>年假默认天数:</th>
            <td><input id="NJday" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>每两年增加天数:</th>
            <td><input id="NJadd" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>普通员工年假上限:</th>
            <td><input id="NJnorMax" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>管理层年假上限:</th>
            <td><input id="NJmanMax" type="text" class="txt" runat="server" /></td>
        </tr>
        <tr>
            <th>年假转结余日期:</th>
            <td>
                <asp:DropDownList ID="jyYYYY" runat="server" Visible="false" >
                </asp:DropDownList>
                <asp:DropDownList ID="jyMM" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="jyDD" runat="server">
                </asp:DropDownList>
            
            </td>
        </tr>
        <tr>
            <th>结余清零日期:</th>
            <td>
                <asp:DropDownList ID="zeroYYYY" runat="server" Visible="false">
                </asp:DropDownList>
                <asp:DropDownList ID="zeroMM" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="zeroDD" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
     <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
    </form>
</body>
</html>
