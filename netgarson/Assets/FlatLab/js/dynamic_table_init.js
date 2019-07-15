//function fnFormatDetails(oTable, nTr) {
//    var aData = oTable.fnGetData(nTr);
//    var sOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
//    sOut += '<tr><td>Rendering engine:</td><td>' + aData[1] + ' ' + aData[4] + '</td></tr>';
//    sOut += '<tr><td>Link to source:</td><td>Could provide a link here</td></tr>';
//    sOut += '<tr><td>Extra info:</td><td>And any further details here (img etc)</td></tr>';
//    sOut += '</table>';

//    return sOut;
//}

//$(document).ready(function () {

//    $('#dynamic-table').dataTable({
//        "aaSorting": [[4, "desc"]]
//    });

//    /*
//     * Insert a 'details' column to the table
//     */
//    var nCloneTh = document.createElement('th'); nCloneTh.style.width = '50px';
//    var nCloneTd = document.createElement('td'); nCloneTd.style.width = '50px';
//    nCloneTd.innerHTML = '<img src="../Assets/FlatLab/img/details_open.png">';
//    nCloneTd.className = "center";

//    $('#hidden-table-info thead tr').each(function () {
//        this.insertBefore(nCloneTh, this.childNodes[0]);
//    });

//    $('#hidden-table-info tbody tr').each(function () {
//        this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
//    });

//    /*
//     * Initialse DataTables, with no sorting on the 'details' column
//     */
//    var oTable = $('#hidden-table-info').dataTable({
//        "aoColumnDefs": [
//            { "bSortable": false, "aTargets": [0, 5] }
//        ],
//        "oLanguage": {
//            "sDecimal": ",",
//            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
//            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
//            "sInfoEmpty": "Kayıt yok",
//            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
//            "sInfoPostFix": "",
//            "sInfoThousands": ".",
//            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
//            "sLoadingRecords": "Yükleniyor...",
//            "sProcessing": "İşleniyor...",
//            "sSearch": "Ara:",
//            "sZeroRecords": "Eşleşen kayıt bulunamadı",
//            "oPaginate": {
//                "sFirst": "İlk",
//                "sLast": "Son",
//                "sNext": "Sonraki",
//                "sPrevious": "Önceki"
//            },
//            "oAria": {
//                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
//                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
//            },
//            "select": {
//                "rows": {
//                    "_": "%d kayıt seçildi",
//                    "0": "",
//                    "1": "1 kayıt seçildi"
//                }
//            }
//        },
//        "aaSorting": [[1, 'asc']]
//    });

//    /* Add event listener for opening and closing details
//     * Note that the indicator for showing which row is open is not controlled by DataTables,
//     * rather it is done here
//     */
//    $(document).on('click', '#hidden-table-info tbody td img', function () {
//        var nTr = $(this).parents('tr')[0];
//        if (oTable.fnIsOpen(nTr)) {
//            /* This row is already open - close it */
//            this.src = "../Assets/FlatLab/img/details_open.png";
//            oTable.fnClose(nTr);
//        }
//        else {
//            /* Open this row */
//            this.src = "../Assets/FlatLab/img/details_close.png";
//            oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
//        }
//    });
//});