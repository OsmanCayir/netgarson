$(document).ready(function () {
    $("#select-categoryMenuRelList").select2();
});

$("#button-deleteImage").click(function () {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/DeleteMenuImageBeforeUpdate",
        data: JSON.stringify({ imagePath: $('#input-imagePath').val() }),
        success: function (data) {
            if (data.result == 100) {
                toastr.info("Görsel silindi.", "Başarılı.");
                var htmlImageUploader = '<input id="input-imagePath" type="hidden" /><div id="fileuploader" class="btn btn-success btn-sm"></div><div id="extrabutton" class="btn btn-info btn-sm" style="display:none"><i class="fa fa-upload"></i> Yükle</div>';
                $("#div-imageUploader").html(htmlImageUploader);
                loadImageUploader();
            } else if (resp.errorCode == 99) {
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

function loadImageUploader() {
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
}

$("#button-update").click(function () {

    var menu = {
        ID: $('#input-ID').val(),
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
        url: "/Admin/UpdateMenu",
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
                toastr.error("Beklenmedik bir hata oluştu.", "Hata Kodu: 600.");
            }
            else if (data.result == 700) {
                toastr.error("Aynı isimde menu kaydedilemez.", "Hata Kodu: 700.");
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
