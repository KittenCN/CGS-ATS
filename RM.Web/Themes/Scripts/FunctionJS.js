//*后台管理页JS函数，Jquery扩展
//*作者：xiaoshe
//*时间：2013年01月08日
$.ajaxSetup({
    cache: false
});
$(function () {
    $('.aspNetHidden').hide();
    Loading(false);
    publicobjcss();
    $("#txt_Search").focus().select(); //搜索框默认焦点
})
//=============================切换验证码======================================
function ToggleCode(obj, codeurl) {
    $("#txtCode").val("");
    $("#" + obj).attr("src", codeurl + "?time=" + Math.random());
}
//回调
function windowload() {
    rePage();
}
/**
刷新页面
**/
function rePage() {
    Loading(true);
    window.location.href = window.location.href.replace('#', '');
    return false;
}
/**
* 返回上一级
*/
function back() {
    window.history.back(-1);
    Loading(true)
}
//跳转页面
function Urlhref(url) {
    Loading(true);
    window.location.href = url;
    return false;
}
/**
中间加载对话窗
**/
function Loading(bool) {
    if (bool) {
        top.$("#loading").show();
    } else {
        setInterval(loadinghide, 800);
    }
}
function loadinghide() {
    top.$("#loading").hide();
}
/**
Top 加载对话窗
msg:提示信息
time：停留时间ms
**/
function TopLoading(msg, time) {
    var _time = 1000;
    if (IsNullOrEmpty(time)) {
        _time = time;
    }
    top.$("#Toploading").show().html(msg);
    top.$('#Toploading').css("left", ($(window).width() - $("#Toploading").width()) / 2);
    setInterval(Toploadinghide, _time);
    
}
function Toploadinghide() {
    top.$("#Toploading").hide();
}
function BeautifulGreetings() {
    var now = new Date();
    var hour = now.getHours();
    if (hour < 3) { return ("夜深了,早点休息吧！") }
    else if (hour < 9) { return ("早上好！") }
    else if (hour < 12) { return ("上午好！") }
    else if (hour < 14) { return ("中午好！") }
    else if (hour < 18) { return ("下午好！") }
    else if (hour < 23) { return ("晚上好！") }
    else { return ("夜深了,早点休息吧！") }
}
/**
短暂提示
msg: 显示消息
time：停留时间ms
type：类型 4：成功，5：失败，3：警告
**/
function showTipsMsg(msg, time, type) {
    top.ZENG.msgbox.show(msg, type, time);
}

function showFaceMsg(msg) {
    top.art.dialog({
        id: 'faceId',
        title: '温馨提醒',
        content: msg,
        icon: 'face-smile',
        time: 10,
        background: '#000',
        opacity: 0.1,
        lock: true,
        okVal: '关闭',
        ok: true
    });
}
function showWarningMsg(msg) {
    top.art.dialog({
        id: 'warningId',
        title: '系统提示',
        content: msg,
        icon: 'warning',
        time: 10,
        background: '#000',
        opacity: 0.1,
        lock: true,
        okVal: '关闭',
        ok: true
    });
}
/**
警告提示
msg: 显示消息
callBack：函数
**/
function showConfirmMsg(msg, callBack) {
    top.art.dialog({
        id: 'confirmId',
        title: '系统提示',
        content: msg,
        icon: 'warning',
        background: '#000000',
        opacity: 0.1,
        lock: true,
        button: [{
            name: '确定',
            callback: function () {
                callBack(true);
            },
            focus: true
        }, {
            name: '取消',
            callback: function () {
                this.close();
                return false;
            }
        }]
    });
}
/*弹出网页
/*url:          表示请求路径
/*_id:          ID
/*_title:       标题名称
/*width:        宽度
/*height:       高度
---------------------------------------------------*/
function openDialog(url, _id, _title, _width, _height, left, top) {
    art.dialog.open(url, {
        id: _id,
        title: _title,
        width: _width,
        height: _height,
        left: left + '%',
        top: top + '%',
        background: '#000000',
        opacity: 0.1,
        lock: true,
        resize: false,
        close: function () { }
    }, false);
}
//窗口关闭
function OpenClose() {
    art.dialog.close();
}
/*验证
/*id:        表示请求
--------------------------------------------------*/
function IsEditdata(id) {
    var isOK = true;
    if (id == undefined || id == "") {
        isOK = false;
        showWarningMsg("未选中任何一行");
    } else if (id.split(",").length > 1) {
        isOK = false;
        showFaceMsg("一次只能选择一条记录");
    }
    return isOK;
}
function IsDelData(id) {
    var isOK = true;
    if (id == undefined || id == "") {
        isOK = false;
        showWarningMsg("未选中任何一行");
    }
    return isOK;
}
function IsNullOrEmpty(str) {
    var isOK = true;
    if (str == undefined || str == "") {
        isOK = false;
    }
    return isOK;
}
/*数据放入回收站
/*url:        表示请求路径
/*parm：      条件参数
--------------------------------------------------*/
function delConfig(url, parm) {
    showConfirmMsg('注：您确认要把此数据放入回收站吗？', function (r) {
        if (r) {
            getAjax(url, parm, function (rs) {
                if (parseInt(rs) > 0) {
                    showTipsMsg("删除成功！", 2000, 4);
                    windowload();
                } else if (parseInt(rs) == 0) {
                    showTipsMsg("删除失败，0 行受影响！", 3000, 3);
                }
                else {
                    showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                }
            });
        }
    });
}
/*删除数据
/*url:        表示请求路径
/*parm：      条件参数
--------------------------------------------------*/
function DeleteData(url, parm) {
    showConfirmMsg("此操作不可恢复，您确定要删除吗？", function (r) {
        if (r) {
            getAjax(url, parm, function (rs) {
                if (parseInt(rs) > 0) {
                    showTipsMsg("删除成功！", 2000, 4);
                    windowload();
                } else if (parseInt(rs) == 0) {
                    showTipsMsg("删除失败，0 行受影响！", 3000, 3);
                }
                else {
                    showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                }
            });
        }
    });
}
/*验证数据是否存在
/*url:        表示请求路径
/*parm：      条件参数
--------------------------------------------------*/
function IsExist_Data(url, parm) {
    var num = 0;
    getAjax(url, parm, function (rs) {
        num = parseInt(rs);
    });
    return num;
}
/* 请求Ajax 带返回值，并弹出提示框提醒
--------------------------------------------------*/
function getAjax(url, parm, callBack) {
    $.ajax({
        type: 'post',
        dataType: "text",
        url: url,
        data: parm,
        cache: false,
        async: false,
        success: function (msg) {
            callBack(msg);
        }
    });
}
/**
数据验证完整性
**/
function CheckDataValid(id) {
    if (!JudgeValidate(id)) {
        return false;
    } else {
        return true;
    }
}
/**
文本框，下拉框输入事件
作用是，如果没有对表单值更新，就不提交到数据库
**/
var haveinputValue = "";
function Haveinput() {
    $("textarea,input[type='text']").keydown(function () {
        haveinputValue = 1;
    })
    $("select").change(function () {
        haveinputValue = 1;
    });
}
/********
接收地址栏参数
key:参数名称
**********/
function GetQuery(key) {
    var search = location.search.slice(1); //得到get方式提交的查询字符串
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == key) {
            return ar[1];
        }
    }
}
/**
文本框只允许输入数字
**/
function Keypress(obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}
/**
获取选中复选框值
值：1,2,3,4
**/
function CheckboxValue() {
    var reVal = '';
    $('[type = checkbox]').each(function () {
        if ($(this).attr("checked")) {
            reVal += $(this).val() + ",";
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    return reVal;
}
/**
Table固定表头
gv:             table id
scrollHeight:   高度
**/
function FixedTableHeader(gv, scrollHeight) {
    var gvn = $(gv).clone(true).removeAttr("id");
    $(gvn).find("tr:not(:first)").remove();
    $(gv).before(gvn);
    $(gv).find("tr:first").remove();
    $(gv).wrap("<div id='FixedTable' style='width:auto;height:" + scrollHeight + "px;overflow-y: auto; overflow-x: hidden;scrollbar-face-color: #e4e4e4;scrollbar-heightlight-color: #f0f0f0;scrollbar-3dlight-color: #d6d6d6;scrollbar-arrow-color: #240024;scrollbar-darkshadow-color: #d6d6d6; padding: 0;margin: 0;'></div>");
    var lie = $(gvn).find('thead').find("td").length - 1;
    var arr = $(gvn).find('thead').find("td");
    if ($("#FixedTable").height() > scrollHeight) {
        var lastwidth = $(arr[lie]).width() + 17;
        $(arr[lie]).attr('style', 'width:' + lastwidth + 'px;border-right: 0px');
    } else {
        $(arr[lie]).attr('style', 'border-right: 0px')
    }
}
/**.div-body 自应表格高度**/
function divresize(height) {
    resizeU();
    $(window).resize(resizeU);
    function resizeU() {
        $(".div-body").css("height", $(window).height() - height);
    }
}
//Tab标签切换
function GetTabClick(e) {
    Loading(true);
    $("#menutab div").each(function () {
        this.className = "Tabremovesel";
    });
    e.className = "Tabsel";
    Loading(false);
}
/**
初始化样式
**/
function publicobjcss() {
    /*****************普通表格********************************/
    $('.grid tr').hover(function () {
        $(this).addClass("trhover");
    }, function () {
        $(this).removeClass("trhover");
    });
    $('.grid tbody tr:odd').addClass('alt');
    if ($('.grid').attr('singleselect') == 'true') {
        $('.grid tr td').click(function (e) {
            if ($(this).parents('tr').find("td").hasClass('selected')) {
                $('.grid tr td').parents('tr').find("td").removeClass('selected');
                $('.grid tr td').parents('tr').find('input[type="checkbox"]').removeAttr('checked');
            } else {
                $('.grid tr td').parents('tr').find("td").removeClass('selected');
                $('.grid tr td').parents('tr').find('input[type="checkbox"]').removeAttr('checked');
                $(this).parents('tr').find("td").addClass('selected');
                $(this).parents('tr').find('input[type="checkbox"]').attr('checked', 'checked');
            }
        });
    } else {
        $('.grid tr td').click(function (e) {
            if (!$(this).hasClass('oper')) {
                if ($(this).parents('tr').find("td").hasClass('selected')) {
                    $(this).parents('tr').find("td").removeClass('selected');
                    $(this).parents('tr').find('input[type="checkbox"]').removeAttr('checked');
                } else {
                    $(this).parents('tr').find("td").addClass('selected');
                    $(this).parents('tr').find('input[type="checkbox"]').attr('checked', 'checked');
                }
            }
        });
    }
    /*****************树表格********************************/
    $('#dnd-example tbody tr:odd').addClass('alt');
    $("#dnd-example tr").click(function () {
        $('#dnd-example tr').removeClass("selected");
        $(this).addClass("selected"); //添加选中样式   
    })
    /*****************按钮********************************/
    $(".l-btn").hover(function () {
        $(this).addClass("l-btnhover");
        $(this).find('span').addClass("l-btn-lefthover");
    }, function () {
        $(this).removeClass("l-btnhover");
        $(this).find('span').removeClass("l-btn-lefthover");
    });
}
/*****************树形样式********************************/
function treeAttrCss() {
    $('.strTree').lightTreeview({
        collapse: true,
        line: true,
        nodeEvent: false,
        unique: false,
        style: 'black',
        animate: 100,
        fileico: false,
        folderico: true
    });
    treeCss();
}
function treeCss() {
    $(".strTree li div").css("cursor", "pointer");
    $(".strTree li div").click(function () {
        if ($(this).attr('class') == "" || $(this).attr('class') == undefined) {
            $(".strTree li div").removeClass("collapsableselected");
            $(this).addClass("collapsableselected"); //添加选中样式
        }
    })
}
/**********复选框 全选/反选**************/
var CheckAllLinestatus = 0;
function CheckAllLine() {
    if (CheckAllLinestatus == 0) {
        CheckAllLinestatus = 1;
        $("#checkAllOff").attr('title', '反选');
        $("#checkAllOff").attr('id', 'checkAllOn');
        $(".grid :checkbox").attr("checked", true);
        $('.grid tr').find('td').addClass('selected');
        $("#dnd-example :checkbox").attr("checked", true);
        $('#dnd-example tr').find('td').addClass('selected');
    } else {
        CheckAllLinestatus = 0;
        $("#checkAllOn").attr('title', '全选');
        $("#checkAllOn").attr('id', 'checkAllOff');
        $(".grid :checkbox").attr("checked", false);
        $('.grid tr').find('td').removeClass('selected');
        $("#dnd-example :checkbox").attr("checked", false);
        $('#dnd-example tr').find('td').removeClass('selected');
    }
}
///防止重复提交
function SubmitCheckForRC() {
    $("#Save .l-btn-left").html('<img src="/Themes/Images/loading1.gif" alt="" />正在提交');
    $("#Save").attr('disabled', "true");
    $("#Close").hide();
}
///清空防止重复提交
function SubmitCheckEmpty() {
    $("#Save").removeAttr("disabled")
    $("#Save .l-btn-left").html('<img src="/Themes/Images/disk.png" alt="" />保 存');
    $("#Close").show();
}
//树表格复选框，点击子，把父也打勾
function ckbValueObj(e) {
    var item_id = '';
    var arry = new Array();
    arry = e.split('-');
    for (var i = 0; i < arry.length - 1; i++) {
        item_id += arry[i] + '-';
    }
    item_id = item_id.substr(0, item_id.length - 1);
    if (item_id != "") {
        $("#" + item_id).attr("checked", true);
        ckbValueObj(item_id);
    }
}