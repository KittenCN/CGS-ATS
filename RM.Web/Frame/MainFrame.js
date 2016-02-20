/**初始化**/
$(document).ready(function () {
    writeDateInfo();
    readyIndex();
    iframeresize();
});
//样式
function readyIndex() {
    $("#Sidebar li div").click(function () {
        $("#Sidebar li div").removeClass('leftselected');
        $(this).addClass('leftselected');
    });
    $(".navPanel div").click(function () {
        $('.navPanel div').removeClass("selected")
        $(this).addClass("selected");
    }).hover(function () {
        $(this).addClass("navHover");
    }, function () {
        $(this).removeClass("navHover");
    });
    $(".navPanelMini div").click(function () {
        $('.navPanelMini div').removeClass("selected")
        $(this).addClass("selected");
    }).hover(function () {
        $(this).addClass("navHover");
    }, function () {
        $(this).removeClass("navHover");
    });
    $(".navSelect div").click(function () {
        $('.navSelect div').removeClass("selected")
        $(this).addClass("selected");
    }).hover(function () {
        $(this).addClass("navHover");
    }, function () {
        $(this).removeClass("navHover");
    });


    $("#toolbar img").hover(function () {
        $(this).addClass("pageBase_Div");
    }, function () {
        $(this).removeClass("pageBase_Div");
    });
}
/**自应高度**/
function iframeresize() {
    resizeU();
    $(window).resize(resizeU);
    function resizeU() {
        var divkuangH = $(window).height();
        $("#MainContent").height(divkuangH - 98);
        //导航
        var navigationheight = $(".navigation").height();
        var navPanelheight = $("#htmlMenuPanel").height();
        var navSelectheight = $(".navSelect").height();
        $(".navPanelMini").css("height", navigationheight - navSelectheight - 49);
        $(".navPanel").css("height", navigationheight - navSelectheight - 29);
        $("#sidebarTree").css("height", divkuangH - 125);
        $("#Sidebar").css("height", divkuangH - 125);
    }
}
//链接内框架frames
function NavMenu(url, title) {
    Loading(true);
    if (url != "") {
        $("#titleInfo").empty();
        var info = "&nbsp;>>&nbsp;<a class=\"subtitle\" onclick=\"NavMenuUrl('" + url + "');\">" + title + "</a>";
        $("#titleInfo").html(info);
        NavMenuUrl(url);
    }
}
function NavMenuUrl(url) {
    $("#main").attr("src", url);
    return false;
}
/**安全退出**/
function IndexOut() {
    top.showConfirmMsg('确定要安全退出吗？', function (r) {
        if (r) {
            window.location.href = '../Index.htm';
        }
    });
}
//当前日期
function writeDateInfo() {
    var day = "";
    var month = "";
    var ampm = "";
    var ampmhour = "";
    var myweekday = "";
    var year = "";
    mydate = new Date();
    myweekday = mydate.getDay();
    mymonth = mydate.getMonth() + 1;
    myday = mydate.getDate();
    myyear = mydate.getYear();
    year = (myyear > 200) ? myyear : 1900 + myyear;
    if (myweekday == 0)
        weekday = " 星期日";
    else if (myweekday == 1)
        weekday = " 星期一";
    else if (myweekday == 2)
        weekday = " 星期二";
    else if (myweekday == 3)
        weekday = " 星期三";
    else if (myweekday == 4)
        weekday = " 星期四";
    else if (myweekday == 5)
        weekday = " 星期五";
    else if (myweekday == 6)
        weekday = " 星期六";
    $("#datetime").text(year + "年" + mymonth + "月" + myday + "日 " + weekday);
}
var Contentheight = "";
var Contentwidth = "";
var FixedTableHeight = "";
//最大化
function Maximize() {
    $("#Content").addClass("ContentMaximize");
    $(".ContentMaximize").css("height", $(window).height());
    $(".ContentMaximize").css("width", $(window).width());
    $("#fullreturn").show();
    FixedTableHeight = main.$("#FixedTable").height();
    main.window.$("#FixedTable").height(FixedTableHeight + 98);
    TopLoading('注意：要还原，请单击右上角的红色按钮。', 3000)
}
//还原
function Fullrestore() {
    $("#Content").removeClass("ContentMaximize");
    $("#Content").css("height", Contentheight);
    $("#Content").css("width", Contentwidth);
    $("#fullreturn").hide();
    main.window.$("#FixedTable").height(FixedTableHeight);
}
