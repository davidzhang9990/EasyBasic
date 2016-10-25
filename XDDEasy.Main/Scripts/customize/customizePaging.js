var pageIndex = 0;
var pageSize = 10;
var pagingControl;

/******************************************************************************/
/*params: arg[0]:total records  
/*        arg[1]:function for build get page api url
/*        arg[2]:result build function 
/*        arg[3]:function berfore call data api function 
/*        arg[4]:function after call data api function 
/******************************************************************************/
function pagingInitSearch(totalRecords, buildQueryUrlFunc,buildResultTableFunc) {
    var pageGetResultCallBerfore = arguments[3] ? arguments[3] : function () { };
    var pageGetResultCallBack = arguments[4] ? arguments[4] : function () { };
    pagingControl = $("#pagination").pagination(totalRecords, {
        callback: pageCallback,
        prev_text: "« {0}".format(EqlResource.UI_Paging_Previous),
        next_text: "{0} »".format(EqlResource.UI_Paging_Next),
        items_per_page: pageSize,
        num_edge_entries: 2,
        num_display_entries: 4,
        current_page: pageIndex,
    });

    function pageCallback(index, jq) {
        pageGetResultCallBerfore();
        getPageData(index, buildQueryUrlFunc, buildResultTableFunc, pageGetResultCallBack);
    }
}

/******************************************************************************/
/*params: arg[0]:total records 
/*        arg[1]:function for build get page api url
/*        arg[2]:result build function 
/*        arg[3]:function berfore call data api function 
/*        arg[4]:function after call data api function 
/******************************************************************************/
function pagingInit(totalRecords,buildQueryUrlFunc, buildResultTableFunc) {
    var pageGetResultCallBerfore = arguments[3] ? arguments[3] : function () { };
    var pageGetResultCallBack = arguments[4] ? arguments[4] : function () { };
    getPageData(0, buildQueryUrlFunc, buildResultTableFunc, pageGetResultCallBack);
    pagingInitSearch(totalRecords,buildQueryUrlFunc, buildResultTableFunc, pageGetResultCallBerfore, pageGetResultCallBack);
}

/******************************************************************************/
/*params: arg[0]:current page index 
/*        arg[1]:function for build get page api url
/*        arg[2]:result build function 
/*        arg[3]:function after call result build function 
/*        arg[4]:function for init search condition for re-init pagination plug
/******************************************************************************/
function getPageData(pIndex, buildQueryUrlFunc,buildResultTableFunc) {
    var spinner;
    var url = buildQueryUrlFunc(pIndex, pageSize);
    var callback = arguments[3] ? arguments[3] : function () { };//
    var callbackSearchInit = arguments[4] ? arguments[4] : function () { };//
    pageIndex = pIndex;
    $.ajax(
    {
        type: 'GET',
        url: url,
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            spinner = buildSpinnerloading();
        },
        success: function (result) {
            buildResultTableFunc(result.items);
            callback();
            callbackSearchInit(result.count);
        },
        complete: function (jqXHR, textStatus, errorThrown) {
            adjustModalParentScroll();
            stopSpinnerLoading(spinner);
        }
    });
}

/******************************************************************************/
/*Spinner loading when call api get data
/******************************************************************************/
function buildSpinnerloading() {
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
function stopSpinnerLoading(spinner) {
    spinner.stop();
    $('.w-load').hide();
}