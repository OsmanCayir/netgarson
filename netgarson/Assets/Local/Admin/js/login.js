var Script = function () {

    //var d = document;
    //var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
    //var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
    //onload = function () {
    //    var body = gebtn(d, 'body')[0];
    //    body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

    //    if (!d.getElementById || !d.createTextNode) return;
    //    var ls = gebtn(d, 'label');
    //    for (var i = 0; i < ls.length; i++) {
    //        var l = ls[i];
    //        if (l.className.indexOf('label_') == -1) continue;
    //        var inp = gebtn(l, 'input')[0];
    //        if (l.className == 'label_check') {
    //            l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
    //            l.onclick = check_it;
    //        };
    //    };
    //};
    //var check_it = function () {
    //    var inp = gebtn(this, 'input')[0];
    //    if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
    //        this.className = 'label_check c_on';
    //        if (safari) inp.click();
    //    } else {
    //        this.className = 'label_check c_off';
    //        if (safari) inp.click();
    //    };
    //};

}();

$("#button-login").click(function () {

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/LoginUser",
        data: JSON.stringify({ mail: $('#input-mail').val(), password: $('#input-password').val(), rememberMe: $('#input-rememberMe').closest(".label_check").hasClass("c_off") ? false : true }),
        success: function (data) {

            if (data.result == 100) {
                toastr.success("Yönlendiriliyorsunuz.", "Başarılı.");
                setTimeout(function () {
                    window.location.href = '/Admin/Index';
                }, 500);
            }
            else if (data.result == 200) {
                toastr.error("Geçersiz e-posta adresi veya şifre.", "Hata Kodu: 200.");
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

$("#button-sendRepasswordMail").click(function () {
    $.ajax({
        url: "/Admin/SendRepasswordMail",
        type: "POST",
        dataType: "json",
        data: { mail: $('#input-sendRepasswordMail').val() },
        success: function (data) {

            if (data.result == 100) {
                toastr.success("E-posta gönderildi, gelen kutunuzu kontrol edin.", "Başarılı.");
            }
            else if (data.result == 200) {
                toastr.error("Geçersiz e-posta adresi.", "Hata Kodu: 200.");
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