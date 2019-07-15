var handleMenuComment_ID = 0;

$(document).ready(function () {

    $('#table-menuCommentList').dataTable({
        "oLanguage": {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        },
        "aaSorting": [[0, 'desc']],
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": [7] }
        ]
    });

});

function deleteMenuCommentBefore(ID) {
    handleMenuComment_ID = ID;
}

function deleteMenuComment() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/DeleteMenuComment",
        data: JSON.stringify({ ID: handleMenuComment_ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Kayıt silindi.", "Başarılı.");
                $('#modalDeleteMenuComment').modal('toggle');
                $('#table-menuCommentList').dataTable().fnDeleteRow($('tr#tr-' + handleMenuComment_ID)[0]);
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
}