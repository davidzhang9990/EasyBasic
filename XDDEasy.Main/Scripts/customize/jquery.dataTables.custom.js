$.fn.dGotoPage = function(oSettings) {

    // Configura a exibição do campo
    $("#gotoPage").remove();
    $(".dataTables_wrapper .dataTables_paginate:visible")
      .append("<span id='gotoPage' style='margin-left:20px;'><input style='width:30px; text-align: center;' value='" +
      $(".paginate_active").text() + "'/><input type='button' value='" + oSettings.oLanguage.sGoToPage + "' class='button blue-gradient' style='margin-left:10px;'/></span>");

    // Configura o evento
    $("#gotoPage input:button").unbind('click').bind('click', function () {
        if (parseInt($(".paginate_active").text()) === parseInt($("#gotoPage input").val())) return;
        if (parseInt($("#gotoPage input").val()) > Math.ceil(oSettings._iRecordsDisplay / oSettings._iDisplayLength)) return;
        // Set new page
        var iPage = (parseInt($("#gotoPage input").val()) - 1) * oSettings._iDisplayLength;
        oSettings._iDisplayStart = iPage;

        // Redraw table
        $(oSettings.oInstance).trigger('page', oSettings);
        oSettings.oApi._fnCalculateEnd(oSettings);
        oSettings.oApi._fnDraw(oSettings);
    });

    $("#gotoPage input:text").unbind('keyup').bind('keyup', function (e) {
        if (e.keyCode == 13) {
            $("#gotoPage input:button").click();
        }
    });

}
