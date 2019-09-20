//var globalTimeout;

$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        debug: false,
        progressBar: true,
        positionClass: "toast-top-right",
        onclick: null,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "5000",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    }

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

    getNotificationCountList();

    initCustomSelect("div-customSelectMonth");

});

function initCustomSelect(selectClassName) {
    var x, i, j, selElmnt, a, b, c;
    /*look for any elements with the class "custom-select":*/
    x = document.getElementsByClassName(selectClassName);
    for (i = 0; i < x.length; i++) {
        selElmnt = x[i].getElementsByTagName("select")[0];
        /*for each element, create a new DIV that will act as the selected item:*/
        a = document.createElement("DIV");
        a.setAttribute("class", "select-selected");
        a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
        x[i].appendChild(a);
        /*for each element, create a new DIV that will contain the option list:*/
        b = document.createElement("DIV");
        b.setAttribute("class", "select-items select-hide");
        for (j = 1; j < selElmnt.length; j++) {
            /*for each option in the original select element,
            create a new DIV that will act as an option item:*/
            c = document.createElement("DIV");
            c.innerHTML = selElmnt.options[j].innerHTML;
            c.addEventListener("click", function (e) {
                /*when an item is clicked, update the original select box,
                and the selected item:*/
                var y, i, k, s, h;
                s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                h = this.parentNode.previousSibling;
                for (i = 0; i < s.length; i++) {
                    if (s.options[i].innerHTML == this.innerHTML) {
                        s.selectedIndex = i;
                        h.innerHTML = this.innerHTML;
                        y = this.parentNode.getElementsByClassName("same-as-selected");
                        for (k = 0; k < y.length; k++) {
                            y[k].removeAttribute("class");
                        }
                        this.setAttribute("class", "same-as-selected");
                        break;
                    }
                }
                h.click();
            });
            b.appendChild(c);
        }
        x[i].appendChild(b);
        a.addEventListener("click", function (e) {
            /*when the select box is clicked, close any other select boxes,
            and open/close the current select box:*/
            e.stopPropagation();
            closeAllSelect(this);
            this.nextSibling.classList.toggle("select-hide");
            this.classList.toggle("select-arrow-active");
        });
    }
    function closeAllSelect(elmnt) {
        /*a function that will close all select boxes in the document,
        except the current select box:*/
        var x, y, i, arrNo = [];
        x = document.getElementsByClassName("select-items");
        y = document.getElementsByClassName("select-selected");
        for (i = 0; i < y.length; i++) {
            if (elmnt == y[i]) {
                arrNo.push(i)
            } else {
                y[i].classList.remove("select-arrow-active");
            }
        }
        for (i = 0; i < x.length; i++) {
            if (arrNo.indexOf(i)) {
                x[i].classList.add("select-hide");
            }
        }
    }
    /*if the user clicks anywhere outside the select box,
    then close all select boxes:*/
    document.addEventListener("click", closeAllSelect);
}

function getNotificationCountList() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetNotificationCountList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                setScanNotificationCount(data.countList[0]);
                setCallNotificationCount(data.countList[1]);
                setCafeCommentNotificationCount(data.countList[2]);
                setMenuCommentNotificationCount(data.countList[3]);
            }
            else if (data.result == 200) {
                toastr.error("Site ayarları ile ilgili bir hata oluştu.", "Hata Kodu: 200.");
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

$("#a-logout").click(function () {

    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/Logout",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Yönlendiriliyorsunuz.", "Başarılı.");
                setTimeout(function () {
                    window.location.href = '/Admin/Login';
                }, 500);

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

function validateNumber(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}



function setScanNotificationCount(count) {

    if (count != -1) {
        $(".li-scanNotification").show();
        if (count == 0) {
            $(".span-scanNotificationCount").hide();
        }
        else {
            $(".span-scanNotificationCount").html(count);
            $(".span-scanNotificationCount").show();
        }
    }
    else {
        $(".li-scanNotification").hide();
    }
}

function setCallNotificationCount(count) {

    if (count != -1) {
        $(".li-callNotification").show();
        if (count == 0) {
            $(".span-callNotificationCount").hide();
        }
        else {
            $(".span-callNotificationCount").html(count);
            $(".span-callNotificationCount").show();
        }
    }
    else {
        $(".li-callNotification").hide();
    }
}

function setCafeCommentNotificationCount(count) {
    if (count != -1) {
        $(".li-cafeCommentNotification").show();
        if (count == 0) {
            $(".span-cafeCommentNotificationCount").hide();
        }
        else {
            $(".span-cafeCommentNotificationCount").html(count);
            $(".span-cafeCommentNotificationCount").show();
        }
    }
    else {
        $(".li-cafeCommentNotification").hide();
    }
}

function setMenuCommentNotificationCount(count) {
    if (count != -1) {
        $(".li-menuCommentNotification").show();
        if (count == 0) {
            $(".span-menuCommentNotificationCount").hide();
        }
        else {
            $(".span-menuCommentNotificationCount").html(count);
            $(".span-menuCommentNotificationCount").show();
        }
    }
    else {
        $(".li-menuCommentNotification").hide();
    }
}

function getScanNotificationList() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetScanNotificationList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                if (data.scanList.length == 0) {
                    html += '<div class="notify-arrow notify-arrow-terques"></div>';
                    html += '<li><p class="terques">Tarama bildirimi yok.</p></li>';
                }
                else {
                    html += '<div class="notify-arrow notify-arrow-terques"></div>';
                    html += '<li><p class="terques">Tarama bildirimi sayısı : ' + data.scanList.length + '</p></li>';
                    for (var i = 0; i < data.scanList.length; i++) {
                        html += '<li>';
                        html += '<a href="#" onclick="deleteScanNotification(' + data.scanList[i].ID + ')">';
                        html += '<span class="label badge-terques"><i class="fa fa-trash-o"></i></span>';
                        html += 'Masa Numarası : ' + data.scanList[i].TableNo;
                        html += '<span class="time span-dateTimeRelative">' + data.scanList[i].HelperDateTimeRelative + '</span>';
                        html += '</a>';
                        html += '</li>';
                    }
                    html += '<li>';
                    html += '<a href="/Admin/ScanList">Tüm tarama listesi</a>';
                    html += '</li>';
                }
                $(".ul-scanNotificationReference").html(html);
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

function getCallNotificationList() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetCallNotificationList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                if (data.callList.length == 0) {
                    html += '<div class="notify-arrow notify-arrow-red"></div>';
                    html += '<li><p class="red">Çağırma bildirimi yok.</p></li>';
                }
                else {
                    html += '<div class="notify-arrow notify-arrow-red"></div>';
                    html += '<li><p class="red">Çağırma bildirimi sayısı : ' + data.callList.length + '</p></li>';
                    for (var i = 0; i < data.callList.length; i++) {
                        html += '<li>';
                        html += '<a href="#" onclick="deleteCallNotification(' + data.callList[i].ID + ')">';
                        html += '<span class="label label-danger"><i class="fa fa-trash-o"></i></span>';
                        html += 'Masa Numarası : ' + data.callList[i].TableNo;
                        html += '<span class="time span-dateTimeRelative">' + data.callList[i].HelperDateTimeRelative + '</span>';
                        html += '</a>';
                        html += '</li>';
                    }
                    html += '<li>';
                    html += '<a href="/Admin/CallList">Tüm çağırma listesi</a>';
                    html += '</li>';
                }
                $(".ul-callNotificationReference").html(html);
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

function getCafeCommentNotificationList() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetCafeCommentNotificationList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                if (data.cafeCommentList.length == 0) {
                    html += '<div class="notify-arrow notify-arrow-yellow"></div>';
                    html += '<li><p class="yellow">Kafe yorum bildirimi yok.</p></li>';
                }
                else {
                    html += '<div class="notify-arrow notify-arrow-yellow"></div>';
                    html += '<li><p class="yellow">Yeni Kafe Yorumu Sayısı : ' + data.cafeCommentList.length + '</p></li>';
                    for (var i = 0; i < data.cafeCommentList.length; i++) {
                        html += '<li>';
                        html += '<a>';
                        html += '<span class="label badge-yellow" onclick="deleteCafeCommentNotification(' + data.cafeCommentList[i].ID + ')"><i class="fa fa-trash-o"></i></span>';
                        html += '<span class="label badge-yellow" onclick="rotateEditCafeComment(' + data.cafeCommentList[i].ID + ')"><i class="fa fa-external-link"></i></span>';
                        html += data.cafeCommentList[i].CommentText;
                        html += '<span class="time span-dateTimeRelative">' + data.cafeCommentList[i].HelperDateTimeRelative + '</span>';
                        html += '</a>';
                        html += '</li>';
                    }
                    html += '<li>';
                    html += '<a href="/Admin/CafeCommentList">Tüm cafe yorumları</a>';
                    html += '</li>';
                }
                $(".ul-cafeCommetNotificationReference").html(html);
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

function getMenuCommentNotificationList() {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/GetMenuCommentNotificationList",
        data: JSON.stringify({}),
        success: function (data) {
            if (data.result == 100) {
                var html = "";
                if (data.menuCommentList.length == 0) {
                    html += '<div class="notify-arrow notify-arrow-blue"></div>';
                    html += '<li><p class="blue">Menü yorum bildirimi yok.</p></li>';
                }
                else {
                    html += '<div class="notify-arrow notify-arrow-blue"></div>';
                    html += '<li><p class="blue">Yeni Menü Yorumu Sayısı : ' + data.menuCommentList.length + '</p></li>';
                    for (var i = 0; i < data.menuCommentList.length; i++) {
                        html += '<li>';
                        //html += '<a href="#" onclick="deleteMenuCommentNotification(' + data.menuCommentList[i].ID + ')">';
                        html += '<a>';
                        html += '<span class="label badge-blue" onclick="deleteMenuCommentNotification(' + data.menuCommentList[i].ID + ')"><i class="fa fa-trash-o"></i></span>';
                        html += '<span class="label badge-blue" onclick="rotateEditMenuComment(' + data.menuCommentList[i].ID + ')"><i class="fa fa-external-link"></i></span>';
                        html += data.menuCommentList[i].CommentText;
                        html += '<span class="time span-dateTimeRelative">' + data.menuCommentList[i].HelperDateTimeRelative + '</span>';
                        html += '<span class="time span-dateTimeRelative"><br/>' + data.menuCommentList[i].HelperMenuName + '</span>';
                        html += '</a>';
                        html += '</li>';
                    }
                    html += '<li>';
                    html += '<a href="/Admin/MenuCommentList">Tüm menü yorumları</a>';
                    html += '</li>';
                }
                $(".ul-menuCommetNotificationReference").html(html);
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

function deleteScanNotification(ID) {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/ChangeScanIsNew",
        data: JSON.stringify({ ID: ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Tarama bildirimi güncellendi.", "Başarılı.");
                getNotificationCountList();
                //getScanNotificationList();
                $(".a-scanNotification").click();
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

function deleteCallNotification(ID) {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/ChangeCallIsNew",
        data: JSON.stringify({ ID: ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Çağırma bildirimi güncellendi.", "Başarılı.");
                getNotificationCountList();
                //getCallNotificationList();
                $(".a-callNotification").click();
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

function deleteCafeCommentNotification(ID) {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/ChangeCafeCommentIsNew",
        data: JSON.stringify({ ID: ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Kafe yorumu bildirimi güncellendi.", "Başarılı.");
                getNotificationCountList();
                //getCafeCommentNotificationList();
                $(".a-cafeCommentNotification").click();
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

function rotateEditCafeComment(ID) {
    window.location = "/Admin/EditCafeComment?ID=" + ID;
}

function deleteMenuCommentNotification(ID) {
    $.ajax({
        type: "POST",
        contentType: "application/jsonrequest; charset=utf-8",
        url: "/Admin/ChangeMenuCommentIsNew",
        data: JSON.stringify({ ID: ID }),
        success: function (data) {
            if (data.result == 100) {
                toastr.success("Menü yorumu bildirimi güncellendi.", "Başarılı.");
                getNotificationCountList();
                //getMenuCommentNotificationList();
                $(".a-menuCommentNotification").click();
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

function rotateEditMenuComment(ID) {
    window.location = "/Admin/EditMenuComment?ID=" + ID;
}

