(function ($) {
    $.fn.PullBox = function (options) {
        var defaults = {
            //最外层元素
            dv: '',
            //被选择对象以数组形式
            obj: '',
            //选中的样式
            selectClass: 'selected',
            //窗口宽度
            windowidth: document.documentElement.clientWidth,
            //窗口高度
            windowheight: document.documentElement.clientHeight,
            // 记录鼠标按下时的坐标
            downX: 0,
            downY: 0,
            // 记录鼠标抬起时候的坐标
            mouseX2: 0,
            mouseY2: 0,
            //是否全屏拉框
            Isscreen: true,
            //创建选择框DIV
            rect: $("<div id='rect'></div>")
        };
        var opts = $.extend({}, defaults, options);
        return this.each(function () {

            var J = $(this);
            //阻止mouseup后触发click事件
            var IsMove = false;
            if (opts.Isscreen) {
                $("body div").first().height(opts.windowheight);
                $("body div").first().width(opts.windowidth);
            }
            J.append(opts.rect);

            J.mousedown(function (event) {
                IsMove = false;
                down(event, opts);
                $(document).mousemove(function (event) {

                    IsMove = true;
                    move(event, opts);

                }).mouseup(function (event) {
                    up(event, opts);
                    $(document).unbind('mousemove').unbind('mouseup');

                });
            }).click(function (e) {
                //取消所有选择
                if (!IsMove) {
                    onselect(opts);
                };
                IsMove = false; //

            });

        });

    };
    // 是否需要(允许)处理鼠标的移动事件,默认识不处理
    var select = false;
    var down = function (event, options) {

        // 鼠标按下时才允许处理鼠标的移动事仿
        select = true;
        // 取得鼠标按下时的坐标位置
        options.downX = event.clientX > options.windowidth ? options.windowidth : event.clientX;
        options.downY = event.clientY > options.windowheight ? options.windowheight : event.clientY;

    };
    var up = function (event, options) {
        //鼠标抬起初始化所有值
        options.rect.width(0);
        options.rect.height(0);
        options.rect.css({
            "left": 0,
            "top": 0
        });
        //鼠标抬起,就不允许在处理鼠标移动事仿
        select = false;
        //取消捕获范围
        if (!window.captureEvents) {
            options.dv[0].releaseCapture();
        } else if (window.captureEvents) {
            window.mousemove = null;
            window.mouseup = null;
        }
        //隐藏图层
        options.rect.css({
            "visibility": "hidden"
        });
    };
    var move = function (event, options) {

        // 取得鼠标移动时的坐标位置
        options.mouseX2 = event.clientX > options.windowidth ? options.windowidth : event.clientX;
        options.mouseY2 = event.clientY > options.windowheight ? options.windowheight : event.clientY;
        options.downX = options.downX;
        options.downY = options.downY
        if (select) {
            //捕获区域外的鼠标事件
            if (!window.captureEvents) {
                //IE
                options.dv[0].setCapture();
            } else {
                //IE9 or FF or chrome
                $(window).mousemove(function (event) {
                    move(event, options);
                });
                $(window).mouseup(function (event) {
                    up(event, options);
                });
            }
            // 设置拉框的大尿
            options.rect.width(Math.abs(options.mouseX2 - options.downX - 2));
            options.rect.height(Math.abs(options.mouseY2 - options.downY - 2));

            /*

            这个部分,根据你鼠标按下的位置,和你拉框时鼠标松开的位置关糿可以把区域分为四个部刿根据四个部分的不吿
            我们可以分别来画桿否则的话,就只能向一个方向画桿也就是点的右下方画框.

            */

            options.rect.css({
                "visibility": "visible"
            });

            // A part
            if (options.mouseX2 < options.downX && options.mouseY2 < options.downY) {

                options.rect.css({
                    "left": options.mouseX2,
                    "top": options.mouseY2
                });
            }

            // B part
            if (options.mouseX2 > options.downX && options.mouseY2 < options.downY) {
                options.rect.css({
                    "left": options.downX,
                    "top": options.mouseY2
                });
            }

            // C part
            if (options.mouseX2 < options.downX && options.mouseY2 > options.downY) {
                options.rect.css({
                    "left": options.mouseX2,
                    "top": options.downY
                });
            }

            // D part
            if (options.mouseX2 > options.downX && options.mouseY2 > options.downY) {
                options.rect.css({
                    "left": options.downX,
                    "top": options.downY
                });
            }
            //选择对象
            onselect(options);
        }

        /*
        这两句代码是最重要的时倿没有这两句代砿你的拉框,就只能拉桿在鼠标松开的时倿
        拉框停止,但是不能相应鼠标的mouseup事件.那么你想做的处理就不能进衿
        这两句的作用是使当前的鼠标事件不在冒泿也就是说,不向其父窗口传逿所以才可以相应鼠标抬起事件,
        这个部分我也理解的不是特别的清楚,如果你需要的诿你可以查资料.但是这两句话确实最核心的部刿
        为了这两句话,为了实现鼠标拉框,我搞了几天的时间.
        */
        window.event.cancelBubble = true;
        window.event.returnValue = false;
    };

    var onselect = function (options) {
        var select = options.rect;
        var tr = options.obj;
        for (var k = 1; k < tr.length; k++) {
            //判断对象是否在框选中
            var item = tr[k],
			sr = innerSerlect(select, item);
            if (sr && item.className.indexOf(options.selectClass) == -1) {
                item.className = item.className + " " + options.selectClass;
                var ck = item.getElementsByTagName("input");
                for (var k = 0; k < ck.length; k++) {
                    if (ck[k].type == "checkbox")
                        ck[k].checked = "checked";
                }
            } else if (!sr && item.className.indexOf(options.selectClass) != -1) {
                item.className = item.className.replace(options.selectClass, "");
                var ck = item.getElementsByTagName("input");
                for (var k = 0; k < ck.length; k++) {
                    if (ck[k].type == "checkbox")
                        ck[k].checked = "";
                }
            }
        }
    };

    var innerSerlect = function (selDiv, region) {
        var position = selDiv.position();
        //选择框的TOP位置
        var s_top = parseInt(position.top);
        //选择框的LEFT位置
        var s_left = parseInt(position.left);
        //选择框RIGHT位置
        var s_right = s_left + parseInt(selDiv.outerWidth());
        //选择器BOTTOM位置
        var s_bottom = s_top + parseInt(selDiv.outerHeight());

        var offs = $(region).offset();
        var r_top = parseInt(offs.top);
        var r_left = parseInt(offs.left);
        var r_right = r_left + parseInt(region.offsetWidth);
        var r_bottom = r_top + parseInt(region.offsetHeight);

        var t = Math.max(s_top, r_top);
        var r = Math.min(s_right, r_right);
        var b = Math.min(s_bottom, r_bottom);
        var l = Math.max(s_left, r_left);

        if (b > t + 5 && r > l + 5) {
            return region;
        } else {
            return null;
        }

    };

})($);
