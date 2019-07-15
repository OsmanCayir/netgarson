$(document).ready(function () {

    $('#table-callList').dataTable({
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
        "aaSorting": [[0, 'asc']],
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": [3] }
        ]
    });

});

function changeCallState(ID) {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/ChangeCallIsNew",
        data: JSON.stringify({ ID: ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Durum değiştirildi.", "Başarılı.");
                if ($('span#span-' + ID).text() == "Yeni") {
                    $('span#span-' + ID).text("-");
                    $('span#span-' + ID).removeClass("badge-info");
                    $('span#span-' + ID).addClass("badge-danger");
                }
                else {
                    $('span#span-' + ID).text("Yeni");
                    $('span#span-' + ID).removeClass("badge-danger");
                    $('span#span-' + ID).addClass("badge-info");
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
}