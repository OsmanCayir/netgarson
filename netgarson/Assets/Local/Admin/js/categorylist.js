var handleCategory_ID = 0;

$(document).ready(function () {

    var nCloneTh = document.createElement('th'); nCloneTh.style.width = '50px';
    var nCloneTd = document.createElement('td'); nCloneTd.style.width = '50px';
    nCloneTd.innerHTML = '<img src="../Assets/FlatLab/img/details_open.png">';
    nCloneTd.className = "center";

    $('#table-categoryList thead tr').each(function () {
        this.insertBefore(nCloneTh, this.childNodes[0]);
    });

    $('#table-categoryList tbody tr').each(function () {
        this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
    });


    var oTable = $('#table-categoryList').dataTable({
        "aoColumnDefs": [
            { "bSortable": false, "aTargets": [0, 5] }
        ],
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
        "aaSorting": [[1, 'asc']]
    });


    $(document).on('click', '#table-categoryList tbody td img', function () {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
            /* This row is already open - close it */
            this.src = "../Assets/FlatLab/img/details_open.png";
            oTable.fnClose(nTr);
        }
        else {
            /* Open this row */
            this.src = "../Assets/FlatLab/img/details_close.png";
            var category_ID = nTr.id.replace("tr-", "");
            var sOut = "";

            $.ajax({
                type: "POST",
                contentType: "application/jsonrequest; charset=utf-8",
                url: "/Admin/AjaxGetMenuFromCategory",
                data: JSON.stringify({ category_ID: category_ID }),
                success: function (data) {
                    if (data.result == 100) {
                        if (data.menuList.length == 0) {
                            sOut += '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
                            sOut += '<tr>';
                            sOut += '<td>Bu kategori altında menü bulunmamakta.</td>';
                            sOut += '</tr>';
                            sOut += '</table>';
                        }
                        else {

                            sOut += '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
                            sOut += '<tr><td><b>Sıra</b></td><td><b>Menü Adı</b></td><td><b>Fiyat</b></td><td><b>Durum</b></td></tr>'
                            for (var i = 0; i < data.menuList.length; i++) {
                                sOut += '<tr>';
                                sOut += '<td>' + (i + 1) + '</td><td>' + data.menuList[i].Name + '</td><td>' + data.menuList[i].Price + '</td>';

                                if (data.menuList[i].Active == true) {
                                    sOut += '<td><span class="badge badge-info label-mini">Aktif</span></td>';
                                } else {
                                    sOut += '<td><span class="badge badge-danger label-mini">Pasif</span></td>';
                                }
                                sOut += '</tr>';
                            }
                            sOut += '</table>';
                        }
                        oTable.fnOpen(nTr, sOut, 'details');
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
    });
});

function deleteCategoryBefore(ID) {
    handleCategory_ID = ID;
}

function deleteCategory() {


    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/DeleteCategory",
        data: JSON.stringify({ category_ID: handleCategory_ID }),
        success: function (data) {
            
            if (data.result == 100) {
                toastr.success("Kayıt silindi.", "Başarılı.");
                $('#modalDeleteCategory').modal('toggle');
                $('#table-categoryList').dataTable().fnDeleteRow($('tr#tr-' + handleCategory_ID)[0]);
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
