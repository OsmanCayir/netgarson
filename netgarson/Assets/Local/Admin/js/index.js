$(document).ready(function () {

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/DashboardGetTotalCountList",
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
