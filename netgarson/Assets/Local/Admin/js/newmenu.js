$(document).ready(function () {
    var extraObj = $("#fileuploader").uploadFile({
        url: "../Admin/UploadMenuImage",
        statusBarWidth: 'auto',
        dragdropWidth: 'auto',
        showDelete: true,
        autoSubmit: false,
        showProgress: true,
        maxFileCount: 1,
        allowedTypes: "jpg,jpeg,png,gif",
        multiple: false,
        dragDrop: false,
        showPreview: true,
        previewHeight: "100px",
        previewWidth: "auto",
        showError: false,
        uploadStr: "<i class='fa fa-plus'></i> Dosya Seç",
        cancelStr: "<i class='fa fa-ban'></i> İptal",
        abortStr: "<i class='fa fa-ban'></i> İptal",
        deleteStr: "<i class='fa fa-trash-o'></i> Sil",
        deleteCallback: function (data, pd) {
            $.post("/Admin/DeleteMenuImage?url=" + data.url,
                function (resp, textStatus, jqXHR) {
                    if (resp.errorCode == 100) {
                        toastr.info("Görsel silindi.", "Başarılı.");
                    } else if (resp.errorCode == 99) {
                        toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
                    } else {
                        toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
                    }

                    $("#extrabutton").hide();
                    $("#fileuploader").show();
                });

        },
        afterUploadAll: function (obj) {
            if (obj.responses.length != 0) {
                if (obj.responses[0].errorCode == 100) {
                    toastr.info("Görsel yüklendi.", "Başarılı.");
                    $("#extrabutton").hide();
                    $("#fileuploader").hide();
                    $('#input-imagePath').val(obj.responses[0].filenameDB);
                } else if (obj.responses[0].errorCode == 99) {
                    toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 99.");
                    obj.reset();
                    $("#extrabutton").hide();
                    $("#fileuploader").show();
                } else {
                    toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: Bilinmiyor.");
                    obj.reset();
                    $("#extrabutton").hide();
                    $("#fileuploader").show();
                }
            } else {
                $("#extrabutton").hide();
                $("#fileuploader").show();
                obj.reset();
            }
        },
        onCancel: function (files, pd) {
            $("#extrabutton").hide();
            $("#fileuploader").show();
        },
        onSelect: function (files) {
            $("#extrabutton").show();
            $("#fileuploader").hide();
        }
    });
    $("#extrabutton").click(function () {
        extraObj.startUpload();
    });
    $("#select-categoryMenuRelList").select2();
});

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
    var menu = {
        Name: $('#input-name').val(),
        Description: $('#textarea-description').val(),
        Price: parseFloat($('#input-priceTL').val() + $('#input-priceKR').val()),
        ImagePath: $('#input-imagePath').val(),
        ShowComment: $('#input-showComment').closest(".label_check").hasClass("c_off") ? false : true,
        Active: $('#input-active').closest(".label_check").hasClass("c_off") ? false : true
    };

    var categoryMenuRelList = [];
    var categoryMenuRel = {};
    $.each($("#select-categoryMenuRelList").val(), function (index, value) {
        categoryMenuRel = {
            Category_ID: value
        };
        categoryMenuRelList.push(categoryMenuRel);
    });
    

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/SaveMenu",
        data: JSON.stringify({ menu: menu, categoryMenuRelList: categoryMenuRelList }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Yönlendiriliyorsunuz.", "Başarılı.");
            }
            else if (data.result == 200) {
                toastr.error("Menu ismi boş bırakılamaz.", "Hata Kodu: 200.");
            }
            else if (data.result == 300) {
                toastr.error("Görsel eklemeden kayıt yapılamaz.", "Hata Kodu: 300.");
            }
            else if (data.result == 400) {
                toastr.error("Menu fiyatı 1 TL - 9999 TL arasında olmalı.", "Hata Kodu: 400.");
            }
            else if (data.result == 500) {
                toastr.error("Görsel yüklemeden kayıt yapılamaz.", "Hata Kodu: 500.");
            }
            else if (data.result == 600) {
                toastr.error("Aynı isimde menu kaydedilemez.", "Hata Kodu: 600.");
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
