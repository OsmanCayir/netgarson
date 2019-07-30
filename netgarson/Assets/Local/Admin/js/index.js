$(document).ready(function () {

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetDashboardTotalCountList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                setScanTotalCount(data.countList[0]);
                setCallTotalCount(data.countList[1]);
                setCafeCommentTotalCount(data.countList[2]);
                setMenuCommentTotalCount(data.countList[3]);
            }
            else if (data.result == 99) {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
            } else {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
            }
        },
        error: function (request) {
            alert(request.responseText);
        },
        beforeSend: function () {
            $('#div-globalLoading').show(0);
        },
        complete: function () {
            $('#div-globalLoading').hide(0);
        }
    });

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetDashboardChartYearSelect",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                for (var i = 0; i < data.yearList.count; i++) {
                    html += "<option>" + data.yearList[i].Value + "</option>";
                }
                $(".select-year").html(html);
            }
            else if (data.result == 200) {
                toastr.error("Veritabanı boş tablo hatası.", "Hata Kodu: 200.");
            }
            else if (data.result == 99) {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
            } else {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
            }
        },
        error: function (request) {
            alert(request.responseText);
        },
        beforeSend: function () {
            $('#div-globalLoading').show(0);
        },
        complete: function () {
            $('#div-globalLoading').hide(0);
        }
    });

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetDashboardChartYearDecadeSelect",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                for (var i = 0; i < data.yearDecadeList.count; i++) {
                    html += "<option>" + data.yearDecadeList[i].BeginValue - data.yearDecadeList[i].EndValue + "</option>";
                }
                $(".select-yearDecade").html(html);
            }
            else if (data.result == 200) {
                toastr.error("Veritabanı boş tablo hatası.", "Hata Kodu: 200.");
            }
            else if (data.result == 99) {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
            } else {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
            }
        },
        error: function (request) {
            alert(request.responseText);
        },
        beforeSend: function () {
            $('#div-globalLoading').show(0);
        },
        complete: function () {
            $('#div-globalLoading').hide(0);
        }
    });

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetDashboardChart",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                if (data.chartCookieList[0] == "daily") {

                    $(".a-scanChartDaily").addClass("active");
                    $(".a-scanChartMonthly").removeClass("active");
                    $(".a-scanChartYearly").removeClass("active");

                    $("#div-scanMonthSelect").show();
                    $("#div-scanYearSelect").show();
                    $("#div-scanYearDecadeSelect").hide();
                }
                else if (data.chartCookieList[0] == "monthly") {

                    $(".a-scanChartDaily").removeClass("active");
                    $(".a-scanChartMonthly").addClass("active");
                    $(".a-scanChartYearly").removeClass("active");

                    $("#div-scanMonthSelect").hide();
                    $("#div-scanYearSelect").show();
                    $("#div-scanYearDecadeSelect").hide();
                }
                else if (data.chartCookieList[0] == "yearly") {

                    $(".a-scanChartDaily").removeClass("active");
                    $(".a-scanChartMonthly").removeClass("active");
                    $(".a-scanChartYearly").addClass("active");

                    $("#div-scanMonthSelect").hide();
                    $("#div-scanYearSelect").hide();
                    $("#div-scanYearDecadeSelect").show();
                }

                if (data.chartCookieList[3] == "daily") {

                    $(".a-callChartDaily").addClass("active");
                    $(".a-callChartMonthly").removeClass("active");
                    $(".a-callChartYearly").removeClass("active");

                    $("#div-callMonthSelect").show();
                    $("#div-callYearSelect").show();
                    $("#div-callYearDecadeSelect").hide();
                }
                else if (data.chartCookieList[3] == "monthly") {

                    $(".a-callChartDaily").removeClass("active");
                    $(".a-callChartMonthly").addClass("active");
                    $(".a-callChartYearly").removeClass("active");

                    $("#div-callMonthSelect").hide();
                    $("#div-callYearSelect").show();
                    $("#div-callYearDecadeSelect").hide();
                }
                else if (data.chartCookieList[3] == "yearly") {

                    $(".a-callChartDaily").removeClass("active");
                    $(".a-callChartMonthly").removeClass("active");
                    $(".a-callChartYearly").addClass("active");

                    $("#div-callMonthSelect").hide();
                    $("#div-callYearSelect").hide();
                    $("#div-callYearDecadeSelect").show();
                }
            }
            else if (data.result == 99) {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
            } else {
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
            }
        },
        error: function (request) {
            alert(request.responseText);
        },
        beforeSend: function () {
            $('#div-globalLoading').show(0);
        },
        complete: function () {
            $('#div-globalLoading').hide(0);
        }
    });

    $(".barchart").sparkline([15, 3, 6, 7, 5, 6, 6, 5, 7, 6, 5, 4, 7, 4], {
        type: 'bar',
        height: '65',
        barWidth: 8,
        barSpacing: 5,
        barColor: '#fff',
        tooltipFormat: 'test'
        //tooltipFormat: '<span style="display:block; padding:0px 10px 12px 0px;">' +
        //    '<span style="color: {{color}}">&#9679;</span> {{offset:names}}  ({{percent.1}}%)</span>'

    });



});


function setScanTotalCount(count) {
    var div_by = 100,
        speed = Math.round(count / div_by),
        $display = $('.count'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function () {
        if (run_count < div_by) {
            $display.text(speed * run_count);
            run_count++;
        } else if (parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

function setCallTotalCount(count) {
    var div_by = 100,
        speed = Math.round(count / div_by),
        $display = $('.count2'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function () {
        if (run_count < div_by) {
            $display.text(speed * run_count);
            run_count++;
        } else if (parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

function setCafeCommentTotalCount(count) {
    var div_by = 100,
        speed = Math.round(count / div_by),
        $display = $('.count3'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function () {
        if (run_count < div_by) {
            $display.text(speed * run_count);
            run_count++;
        } else if (parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

function setMenuCommentTotalCount(count) {
    var div_by = 100,
        speed = Math.round(count / div_by),
        $display = $('.count4'),
        run_count = 1,
        int_speed = 24;

    var int = setInterval(function () {
        if (run_count < div_by) {
            $display.text(speed * run_count);
            run_count++;
        } else if (parseInt($display.text()) < count) {
            var curr_count = parseInt($display.text()) + 1;
            $display.text(curr_count);
        } else {
            clearInterval(int);
        }
    }, int_speed);
}

function changeCallCookieType(type) {

}