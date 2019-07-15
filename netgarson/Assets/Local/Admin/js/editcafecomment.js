var Script = function () {

    var d = document;
    var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
    var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
    onload = function () {
        var body = gebtn(d, 'body')[0];
        body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

        if (!d.getElementById || !d.createTextNode) return;
        var ls = gebtn(d, 'label');
        for (var i = 0; i < ls.length; i++) {
            var l = ls[i];
            if (l.className.indexOf('label_') == -1) continue;
            var inp = gebtn(l, 'input')[0];
            if (l.className == 'label_check') {
                l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
                l.onclick = check_it;
            };
        };
    };
    var check_it = function () {
        var inp = gebtn(this, 'input')[0];
        if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
            this.className = 'label_check c_on';
            if (safari) inp.click();
        } else {
            this.className = 'label_check c_off';
            if (safari) inp.click();
        };
    };

}();

$("#button-save").click(function () {

    var cafeComment = {
        ID: $('#input-ID').val(),
        CommentText: $('#textarea-commentText').val(),
        Plus: $('#input-plus').val(),
        Cons: $('#input-cons').val(),
        IsNew: $('#input-isNew').closest(".label_check").hasClass("c_off") ? false : true,
        Active: $('#input-active').closest(".label_check").hasClass("c_off") ? false : true
    };

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/UpdateCafeComment",
        data: JSON.stringify({ cafeComment: cafeComment }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Yönlendiriliyorsunuz.", "Başarılı.");
            }
            else if (data.result == 200) {
                toastr.error("Yorum boş bırakılamaz.", "Hata Kodu: 200.");
            }
            else if (data.result == 300) {
                toastr.error("Artı sayısı 0 - 999999999 arasında olmalı.", "Hata Kodu: 300.");
            }
            else if (data.result == 400) {
                toastr.error("Eksi sayısı 0 - 999999999 arasında olmalı.", "Hata Kodu: 300.");
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

});
