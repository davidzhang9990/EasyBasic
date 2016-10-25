var touch = 'ontouchend' in document,
    debugMode = false;


var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};

var EventUtil = {
    getEvent: function (event) {
        return event ? event : window.event;
    },
    addHandler: function (element, type, handler) {
        if (element.addEventListener) {
            element.addEventListener(type, handler, false);
        } else if (element.attachEvent) {
            element.attachEvent("on" + type, handler);
        } else {
            element["on" + type] = handler;
        }
    }
};

function getQueryParamValue(sParam) {
    var sPageUrl = window.location.search.substring(1);
    var sUrlVariables = sPageUrl.split('&');
    for (var i = 0; i < sUrlVariables.length; i++) {
        var sParameterName = sUrlVariables[i].split('=');
        if (sParameterName[0] === sParam) {
            return sParameterName[1];
        }
    }
    return null;
}

function GoLogout() {
    var data = { msgType: "Logout" };
    parent.postMessage(data, "*");
    //清除
    sessionStorage.clear();
    window.location.href = "/Account/LogOut";
}

function sendToParentInfo(sendtype, sid) {
    var data = { msgType: sendtype, sessionId: sid, isMobile: false, sessionIds: null };
    if (isMobile.Android() || isMobile.iOS()) {
        data.isMobile = true;
    }
    parent.postMessage(data, "*");
}

(function ($) {
    $.fn.getStyleObject = function () {
        var dom = this.get(0);
        var style;
        var returns = {};
        if (window.getComputedStyle) {
            var camelize = function (a, b) {
                return b.toUpperCase();
            }
            style = window.getComputedStyle(dom, null);
            for (var i = 0; i < style.length; i++) {
                var prop = style[i];
                var camel = prop.replace(/\-([a-z])/g, camelize);
                var val = style.getPropertyValue(prop);
                returns[camel] = val;
            }
            return returns;
        }
        if (dom.currentStyle) {
            style = dom.currentStyle;
            for (var prop in style) {
                returns[prop] = style[prop];
            }
            return returns;
        }
        return this.css();
    }
})(jQuery);

String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};

function IsURL(url) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?"
        + "(([0-9]{1,3}\.){3}[0-9]{1,3}"
        + "|"
        + "([0-9a-z_!~*'()-]+\.)*"
        + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\."
        + "[a-z]{2,6})"
        + "(:[0-9]{1,4})?"
        + "((/?)|"
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    if (re.test(url.toLowerCase())) {
        return true;
    } else {
        return false;
    }
}

function simulateDblClickTouchEvent(oo) {
    var $oo = !oo ? {} : $(oo);
    if (!$oo[0]) { return false; }
    $oo[0].__taps = 0;
    $oo.bind('touchend', function (e) {
        var ot = this;
        ++ot.__taps;
        //$d(ot.__taps);
        if (!ot.__tabstm) /* don't start it twice */ {
            ot.__tabstm = setTimeout(function () {
                if (ot.__taps >= 2) {
                    ot.__taps = 0;
                    $(ot).trigger('dblclick');
                    if (ot.__tabstm)
                        clearTimeout(ot.__tabstm); // clearTimeout, not cleartimeout..
                }
                ot.__tabstm = 0;
                ot.__taps = 0;
            }, 800);
        }
    });
    return true;
}

function simulateTapHold(oo, second, callback) {
    var $oo = !oo ? {} : $(oo);
    var startTime, endTime;
    var time = !second ? 2 : second;
    //var gbMove = false;

    $oo.bind('touchstart', function () {
        startTime = new Date().getTime();
    });

    //$oo.bind('touchmove', function () {
    //    gbMove = true;
    //});

    $oo.bind('touchend', function () {
        endTime = new Date().getTime();
        if ((endTime - startTime) / 1000 > time)
            if (callback) callback();
    });
}

function touchHandler(event) {
    var touches = event.changedTouches,
        first = touches[0],
        type = "";
    switch (event.type) {
        case "touchstart":
            type = "mousedown";
            break;
        case "touchmove":
            type = "mousemove";
            break;
        case "touchend":
            type = "mouseup";
            break;
        default:
            return;
    }

    var simulatedEvent = document.createEvent("MouseEvent");
    simulatedEvent.initMouseEvent(type, true, true, window, 1,
        first.screenX, first.screenY,
        first.clientX, first.clientY, false,
        false, false, false, 0/*left*/, null);

    first.target.dispatchEvent(simulatedEvent);
    event.preventDefault();
}

function initTouch() {
    if (document.addEventListener) {
        document.addEventListener("touchstart", touchHandler, true);
        document.addEventListener("touchmove", touchHandler, true);
        document.addEventListener("touchend", touchHandler, true);
        document.addEventListener("touchcancel", touchHandler, true);
    } else if (document.attachEvent) {
        document.attachEvent("ontouchstart", touchHandler);
        document.attachEvent("ontouchmove", touchHandler);
        document.attachEvent("ontouchend", touchHandler);
        document.attachEvent("ontouchcancel", touchHandler);
    }
}

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

// *** Call a wrapped object
function AjaxPostInvoke(url, data, callback, error, bare) {
    // *** Convert input data into JSON - REQUIRES Json2.js        
    var json = JSON.stringify(data);

    $.ajax({
        url: url,
        data: json,
        type: "POST",
        processData: false,
        contentType: "application/json",
        timeout: 10000,
        dataType: "text",  // not "json" we'll parse
        async: false,
        success:
                function (res) {
                    if (!callback) return;

                    // *** Use json library so we can fix up MS AJAX dates
                    var result = $.parseJSON(res); // JSON.parse(res);

                    // *** Bare message IS result                        
                    //if (bare)
                    if (true)
                    { callback(result); return; }

                    // *** Wrapped message contains top level object node
                    // *** strip it off                        
                    for (var property in result) {
                        callback(result[property]);
                        break;
                    }
                },
        error: function (xhr) {
            if (!error) return;
            if (xhr.responseText) {
                var err = JSON.parse(xhr.responseText);
                if (err) {
                    error(err);
                } else {
                    error({ Message: "Unknown server error." });
                }
            }
            return;
        }
    });
};

function msgModal(title, msgContentHtml, callback) {
    $.modal({
        title: title,
        isShowTopBtns: false,
        content: msgContentHtml,
        actionCloseCaption: EqlResource.UI_Action_Close,
        buttons: {
            'Close': {
                caption: EqlResource.UI_Action_OK,
                classes: 'btns btn-blue',
                click: function (modal) {
                    if (callback) {
                        callback();
                    }
                    modal.closeModal();
                }
            }
        }
    });
}

function msgModal3(msgContent, callback) {
    msgModal2(EqlResource.UI_MsgTitle_Warning, msgContent, callback);
}

function msgModal2(title, msgContent, callback) {
    var msg = "<div style='margin-top:5px; padding:35px 55px;'><span style='font:bolder;'>" + msgContent + "</span></div>";
    msgModal(title, msg, callback);
}

function msgModalWithoutBtns(title, content) {
    $.modal({
        title: title,
        content: content,
        closeOnBlur: true,
        buttons: false,
        draggable: true
    });
}

function confirmNoMsgModal(title, btnTitle, msgContent, callBackFunc) {
    var cancelCallBack = arguments[3] ? arguments[3] : function () { };
    var confirmParameter = arguments[4] ? arguments[4] : "";
    var msg = "<div style='margin-top:5px; padding:15px;'><span style='font:bolder;'>" + msgContent + "</span></div>";
    $.modal({
        title: title,
        width: "200px",
        height: "200px",
        content: msg,
        resizable: false,
        buttonsAlign: "center",
        isShowTopBtns: true,
        actionCloseCaption: EqlResource.UI_Action_Close,
        buttons: {
            'OK': {
                caption: btnTitle ? btnTitle : EqlResource.UI_Action_Yes,
                isfocus: true,
                classes: 'btns btn-blue',
                click: function (modal) {
                    callBackFunc(confirmParameter);
                    modal.closeModal();
                }
            }
        }
    });
}

function confirmMsgModal(title, msgContent, callBackFunc) {
    var cancelCallBack = arguments[3] ? arguments[3] : function () { };
    var confirmParameter = arguments[4] ? arguments[4] : "";
    var msg = "<div style='margin-top:5px; padding:15px;'><span style='font:bolder;'>" + msgContent + "</span></div>";
    $.modal({
        title: title,
        width: "200px",
        height: "80px",
        content: msg,
        //closeOnBlur: true,
        resizable: false,
        buttonsAlign: "center",
        isShowTopBtns: false,
        actionCloseCaption: EqlResource.UI_Action_Close,
        buttons: {
            'OK': {
                caption: EqlResource.UI_Action_Yes,
                isfocus: true,
                classes: 'btns btn-red',
                click: function (modal) {
                    callBackFunc(confirmParameter);
                    modal.closeModal();
                }
            },
            'Cancel': {
                caption: EqlResource.UI_Action_No,
                classes: 'btns btn-blue',
                click: function (modal) {
                    cancelCallBack();
                    modal.closeModal();
                }
            }
        }
    });
}

//function confirmMsgModal2(title, msgContent, callBackFunc) {
//    var confirmParameter = arguments[4] ? arguments[4] : "";
//    var msg = "<div style='margin-top:5px; padding:15px;'><span style='font:bolder;'>" + msgContent + "</span></div>";
//    $.modal({
//        title: title,
//        /*width: "200px",
//        height: "80px",*/
//        content: msg,
//        //closeOnBlur: false,
//        resizable: false,
//        cancelCallback: function () { callBackFunc(confirmParameter); },
//        actionCloseCaption: EqlResource.UI_Action_Close,
//        buttons: {
//            'OK': {
//                caption: EqlResource.UI_Action_Yes,
//                isfocus: true,
//                classes: 'btns btn-red',
//                click: function (modal) {
//                    callBackFunc(confirmParameter);
//                    modal.closeModal();
//                }
//            }
//        }
//    });
//}

function confirmMsgModal3(title, msgContent, callBackFunc, okCaption, cancelCaption) {
    var cancelCallBack = arguments[5] ? arguments[5] : function () { };
    var confirmParameter = arguments[6] ? arguments[6] : "";
    var msg = "<div style='margin-top:5px; padding:15px;'><span style='font:bolder;'>" + msgContent + "</span></div>";
    $.modal({
        title: title,
        width: "200px",
        height: "80px",
        content: msg,
        //closeOnBlur: true,
        resizable: false,
        buttonsAlign: "center",
        isShowTopBtns: false,
        actionCloseCaption: EqlResource.UI_Action_Close,
        buttons: {
            'OK': {
                caption: okCaption ? okCaption : EqlResource.UI_Action_Yes,
                isfocus: true,
                classes: 'btns btn-red',
                click: function (modal) {
                    callBackFunc(confirmParameter);
                    modal.closeModal();
                }
            },
            'Cancel': {
                caption: cancelCaption ? cancelCaption : EqlResource.UI_Action_No,
                classes: 'btns btn-blue',
                click: function (modal) {
                    cancelCallBack();
                    modal.closeModal();
                }
            }
        }
    });
}



function popWin(url, caption, width, height, btns) {
    //var modal = $.modal({
    //    title: caption,
    //    useIframe: true,
    //    scrolling: "no",
    //    width: width,
    //    height: height,
    //    url: url,
    //    actionCloseCaption: EqlResource.UI_Tip_Hide,
    //    isShowMaximize: true,
    //    buttons: btns ? btns : {},
    //    ajax: {
    //        success: function () {
    //            modal.centerModal();
    //        }
    //    }
    //});
    //return modal;
    return popNonCloseWin(url, caption, width, height, btns);
}

function popNonCloseWin(url, caption, width, height, btns, isShow) {
    var modal = $.modal({
        title: caption,
        useIframe: true,
        scrolling: "no",
        width: width,
        height: height,
        url: url,
        actionCloseCaption: EqlResource.UI_Tip_Hide,
        isShowTopBtns: isShow,
        buttons: btns ? btns : {},
        isShowMaximize: true,
        buttons: btns ? btns : {},
        ajax: {
            success: function () {
                modal.centerModal();
            }
        }
    });
    return modal;
}


Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

function FormatDate(d, fmt) {
    var dmp = new parseISO8601(d);
    return dmp.Format(fmt);
};

function parseISO8601(str) {
    var dtemp1 = str.split("T")[0].split("-");
    var dtemp2 = str.split("T")[1].split(":");

    var date1 = new Date();
    date1.setUTCFullYear(dtemp1[0], dtemp1[1] - 1, dtemp1[2]);
    date1.setUTCHours(dtemp2[0], dtemp2[1], 0, 0);

    return date1;
}

function convertContentType(questionType) {
    switch (questionType) {
        case "SingleChoice":
            return EqlResource.ContentType_SingleChoice;
        case "MultipleChoice":
            return EqlResource.ContentType_MultipleChoice;
        case "PaintIn":
            return EqlResource.ContentType_PaintIn;
        case "PaintOut":
            return EqlResource.ContentType_PaintOut;
        case "FillBlank":
            return EqlResource.ContentType_FillBlank;
            //case "CustomedCar":
            //    return "Customed Car";
            //case "CustomedCoin":
            //    return "Customed Coin";
            //case "CustomedShape":
            //    return "Customed Shape";
            //case "CustomedFruit":
            //    return "Customed Fruit";
        case "EBook":
            return EqlResource.ContentType_EBook;
        case "Video":
            return EqlResource.ContentType_Video;
        case "WebPage":
            return EqlResource.ContentType_WebResource;
        case "Documentation":
            return EqlResource.ContentType_Documents;
        case "Game":
            return EqlResource.ContentType_Game;
    }
    return "Customed";
}



/******************************************************************************/
/*Spinner loading when call api get data
/******************************************************************************/
function spinnerloadingInit() {
    var spinner = new Spinner({
        lines: 12,
        length: 10,
        width: 2,
        radius: 5,
        color: '#333',
        speed: 1,
        trail: 38,
        shadow: false,
        hwaccel: true,
        className: 'spinner',
        top: 'auto',
        left: 'auto'
    });
    $('.w-load').show();
    var target = $('.w-load .spin')[0];
    spinner.spin(target);
    return spinner;
}

/******************************************************************************/
/*Stop spinner loading after get data
/******************************************************************************/
function spinnerLoadingDestroy(spinner) {
    spinner.stop();
    $('.w-load').hide();
}

function htmlEncode(value) {
    if (value) {
        return jQuery('<div />').text(value).html();
    } else {
        return '';
    }
}

function htmlDecode(value) {
    if (value) {
        return $('<div />').html(value).text();
    } else {
        return '';
    }
}

function substringWithSuffix(str) {
    var len = arguments[1] ? arguments[1] : 50;
    return str.length > len ? str.substring(0, len) + "..." : str;
}

//check user name, for example: use name can't have "!"
function checkUserName(userName) {
    var usernameRegex = /^[a-zA-Z][a-zA-Z0-9_]*$/;
    return userName.match(usernameRegex);
}

// Get url parameters
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function getClientTimezone() {
    var oDate = new Date();
    var nTimezone = -oDate.getTimezoneOffset() / 60;
    return nTimezone.toFixed(2);
}

//filterHtml
function filterHtmlContent(str) {
    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
    str.value = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
    str = str.replace(/\n[\s| | ]*\r/g, '\n'); //去除多余空行
    return str;
}

var entityMap = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': '&quot;',
    "'": '&#39;',
    "/": '&#x2F;'
};
// format html to display
function escapeHtml(string) {
    return String(string).replace(/[&<>"'\/]/g, function (s) {
        return entityMap[s];
    });
}

function extractDomain(url) {
    var domain;
    //find & remove protocol (http, ftp, etc.) and get domain
    if (url.indexOf("://") > -1) {
        domain = url.split('/')[2];
    }
    else {
        domain = url.split('/')[0];
    }

    //find & remove port number
    domain = domain.split(':')[0];

    return domain;
}

function getBaseUrl() {
    var url = window.location.href;
    var arr = url.split("/");
    return arr[0] + "//" + arr[2];
}

var app = function () { };
app.ajax = function () { };
app.ajax.getSync = function (url, successCallback) {
    app.ajax.get(url, false, successCallback);
};

app.ajax.get = function (url, async, successCallback) {
    async = (typeof async === "undefined") ? true : async;
    $.ajax(
    {
        type: 'GET',
        url: url,
        async: async,
        success: function (result) {
            if (successCallback) {
                successCallback(result);
            }
        },
        error: function (a) {
            app.log(a);
        }
    });
};

app.ajax.post = function (url, data, async, successCallback) {
    async = (typeof async === "undefined") ? true : async;
    $.ajax(
    {
        type: 'POST',
        url: url,
        async: async,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (result) {
            if (successCallback) {
                successCallback(result);
            }
        },
        error: function (a) {
            app.log(a);
        }
    });
};
app.ajax.put = function (url, data, async, successCallback) {
    async = (typeof async === "undefined") ? true : async;
    $.ajax(
    {
        type: 'PUT',
        url: url,
        async: async,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (result) {
            if (successCallback) {
                successCallback(result);
            }
        }
    });
};

app.ajax.delete = function (url, async, successCallback) {
    async = (typeof async === "undefined") ? true : async;
    $.ajax(
    {
        type: 'DELETE',
        url: url,
        async: async,
        success: function (result) {
            if (successCallback) {
                successCallback(result);
            }
        }
    });
};

app.log = function (msg) {
    if (!debugMode) return;
    if (typeof msg === "object")
        console.log(JSON.stringify(msg));
    else
        console.log(msg);
};

if (!EqlResource)
    var EqlResource = {};

app.question = function () { };
app.question.questionType = {
    SingleChoice: {
        id: 1,
        name: "SingleChoice",
        tabId: 1,
        title: EqlResource.ContentType_SingleChoice,
        span_class: "t6",
        partialView: "SingleChoicePartialView",
        actionName: "SingleChoice",
        childActionName: "ESingleChoice",
        templatePath: "/template/_singlechoice.html"
    },
    MultipleChoice: {
        id: 2,
        name: "MultipleChoice",
        tabId: 2,
        title: EqlResource.ContentType_MultipleChoice,
        span_class: "t6",
        partialView: "MultipleChoicePartialView",
        actionName: "MultipleChoice",
        childActionName: "EMultipleChoice",
        templatePath: "/template/_multiplechoice.html"
    },
    PaintIn: {
        id: 3,
        name: "PaintIn",
        tabId: 3,
        title: EqlResource.ContentType_PaintIn,
        span_class: "t4",
        partialView: "PaintInPartialView",
        actionName: "PaintIn",
        childActionName: "EPaintIn",
        templatePath: "/template/_paintin.html"
    },
    PaintOut: {
        id: 4,
        name: "PaintOut",
        tabId: 4,
        title: EqlResource.ContentType_PaintOut,
        span_class: "t3",
        partialView: "PaintOutPartialView",
        actionName: "PaintOut",
        childActionName: "EPaintOut",
        templatePath: "/template/_paintout.html"
    },
    FillBlank: {
        id: 5,
        name: "FillBlank",
        tabId: 5,
        title: EqlResource.ContentType_FillBlank,
        span_class: "t2",
        partialView: "FillBlank",
        actionName: "FillBlank",
        childActionName: "EFillBlank",
        templatePath: "/template/_fillblank.html"
    },
    CustomedCar: {
        id: 6,
        name: "CustomedCar",
        tabId: 6,
        title: "Customed",
        span_class: "t6",
        partialView: "CustomedPartialView",
        actionName: "Customed",
        childActionName: "ECustomed"
    },
    CustomedCoin: {
        id: 7,
        name: "CustomedCoin",
        tabId: 6,
        title: "Customed",
        span_class: "t6",
        partialView: "CustomedPartialView",
        actionName: "Customed",
        childActionName: "ECustomed"
    },
    CustomedShape: {
        id: 8,
        name: "CustomedShape",
        tabId: 6,
        title: "Customed",
        span_class: "t6",
        partialView: "CustomedPartialView",
        actionName: "Customed",
        childActionName: "ECustomed"
    },
    CustomedFruit: {
        id: 9,
        name: "CustomedFruit",
        tabId: 6,
        title: "Customed",
        span_class: "t6",
        partialView: "CustomedPartialView",
        actionName: "Customed",
        childActionName: "ECustomed"
    },
    EBook: {
        id: 10,
        name: "EBook",
        tabId: 7,
        title: EqlResource.ContentType_EBook,
        span_class: "t1",
        partialView: "EBookPartialView",
        actionName: "EBook",
        templatePath: "/template/_ebook.html"
    },
    Video: {
        id: 11,
        name: "Video",
        tabId: 8,
        title: EqlResource.ContentType_Video,
        span_class: "t8",
        partialView: "VideoPartialView",
        actionName: "Video"
    },
    WebPage: {
        id: 12,
        name: "WebPage",
        tabId: 9,
        title: EqlResource.ContentType_WebResource,
        span_class: "t9",
        partialView: "WebPagePartialView",
        actionName: "WebPage"
    },
    Documentation: {
        id: 13,
        name: "Documentation",
        tabId: 10,
        title: EqlResource.ContentType_Documents,
        span_class: "t10",
        partialView: "DocumentationPartialView",
        actionName: "Documentation"
    },
    Game: {
        id: 14,
        name: "Game",
        tabId: 11,
        title: EqlResource.ContentType_Game,
        span_class: "t11",
        partialView: "GamePartialView",
        actionName: "Game"
    }
};

app.bootstrap = {
    modalDefault: {
        selector: '.modal',
        title: '',
        msg: '',
        closeCallback: null,
        height: '300',
        width: '400'
    },
    modalDialog: function (o) {
        var options = $.extend({}, app.bootstrap.modalDefault, o);
        $(options.selector + ' .modal-dialog').width(options.width);
        $(options.selector + ' .modal-dialog').height(options.height);
        $(options.selector + ' .modal-title').text(options.title);
        $(options.selector + ' .modal-body').html(options.msg);
        $(options.selector).modal('show');
        $(options.selector).on('hidden.bs.modal', function () {
            if (options.closeCallback)
                options.closeCallback();
        });
    }
}

app.showHelp = function (url) {
    var widthP,
        heightP;
    if ($(window).width() > 1024) {
        widthP = 930;
        heightP = 640;
    } else {
        widthP = 860;
        heightP = 590;
    }
    $.modal({
        contentAlign: 'center',
        url: url,
        resizable: false,
        useIframe: true,
        width: widthP,
        height: heightP,
        buttons: {}
    });
}


var adjustModalParentScroll = function () {
    if (!window.self.frameElement) return;
    //var h = window.self.frameElement.contentWindow.document.documentElement.scrollHeight;
    //var vh = window.self.frameElement.height;

    //if (parseInt(h) > parseInt(vh)) {
    var div = window.self.frameElement.contentWindow.document.body;
    div.style.height = window.self.frameElement.height - 20 + "px";
    div.style.overflow = 'auto';
    //}
}
app.reload = function (currentWindow) {
    if (currentWindow) {
        location.reload();
    } else if (window.parent !== window) {
        window.parent.location.reload();
    } else {
        location.reload();
    }
};
app.setCookie = function (name, value, expiredays) {
    var exdate = null;

    if (expiredays != null) {
        exdate = new Date();
        exdate.setDate(exdate.getDate() + expiredays);
    }
    document.cookie = name + "=" + escape(value) + ((exdate == null) ? "" : ";expires=" + exdate.toGMTString()) + "; path=/";
};

app.getCookie = function (objName) {
    var arrStr = document.cookie.split("; ");
    for (var i = 0; i < arrStr.length; i++) {
        var temp = arrStr[i].split("=");
        if (temp[0] === objName) {
            return unescape(temp[1]);
        }
    }
};

app.removeCookie = function (name) {
    var exp = new Date();
    exp.setTime(exp.getTime() + (-1 * 24 * 60 * 60 * 1000));
    var cval = this.getCookie(name);
    document.cookie = name + "=" + cval + "; expires=" + exp.toGMTString() + "; path=/";
};

app.setLanguae = function (key, value, isReload) {
    this.setCookie(key, value);

    if (isReload) {
        this.reload(false);
    }
};

app.equalBoolean = function(value) {
    if (typeof value === "string" && value) {
        if (value.toUpperCase() === "TRUE") {
            return true;
        } else if (value.toUpperCase() === "FALSE") {
            return false;
        }
    }
    return !!value;
};


