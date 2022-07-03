
function b_file_ajax(url, type, data, callbackfunction, message) {

    if (type === null) {
        type = 'POST'
    }

    $.ajax({
        url: url,
        type: type,
        error: function (code) {
            if (code == null || code == undefined)
                unknown_error();
            errorInAjax(code);
        },
        data: data,
        enctype: 'multipart/form-data',
        processData: false,
        contentType: false,
        success: function (data) {
            ResetInputs();
            if (callbackfunction != null)
                callbackfunction(data);
            if (message != null) {
                if (message == "ok" || message == "Ok") {
                    Success_ajax("Success");

                }
                else {
                    Success_ajax(message);
                }
            }

        }
    });
}
function b_ajax(url, type, data, callbackfunction, message) {
    if (type === null) {
        type = 'POST'
    }
    $.ajax({
        url: url,
        data: data,
        type: type,
        error: function (code) {
            if (code == null || code == undefined)
                unknown_error();
            errorInAjax(code);
        },
        success: function (data) {
            ResetInputs();
            if (callbackfunction != null)
                callbackfunction(data);
            if (message != null) {
                if (message == "ok" || message == "Ok") {
                    Success_ajax("Success");

                }
                else {
                    Success_ajax(message);
                }
            }

        }
    });
}

function confirm_ajax(title, text, icon, callbackfunction) {

    swal({
        title: title,
        text: text,
        icon: icon,
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            callbackfunction();
        }
    });
}


function errorInAjax(code) {
    if (code.status == 501) {
        swal("You cannot delete this item", {
            icon: "error",
            buttons: false,
            timer: 3000,
        });
        return;
    }
    if (code.status == 502) {
        swal("Size of File is not valid", {
            icon: "error",
            buttons: false,
            timer: 3000,
        });
        return;
    }

    if (code.status == 509) {
        swal("Information is already exist", {
            icon: "error",
            buttons: false,
            timer: 3000,
        });
        return;
    }



    if (code.status == 400) {
        swal("Undifiend request", {
            icon: "error",
            buttons: false,
            timer: 3000,
        });
        return;
    }
    if (code.status == 404) {
        swal("No Info", {
            icon: "error",
            buttons: false,
            timer: 3000,
        });
        return;
    }

}

function Success_ajax(message) {
    swal(message, {
        icon: "success",
        buttons: false,
        timer: 3000,
    });
}
function unknown_error() {
    iziToast.error({
        title: 'error',
        message: 'error',
        position: 'topRight'
    });
}

function error_ajax(message) {
    swal(message, {
        icon: "error",
        buttons: false,
        timer: 3000,
    });
    return;

}

function search_tbl(inputcontent, tblbodycontent) {

    $("#" + inputcontent).keyup(function () {
        var value = $(this).val().toLowerCase();
        $("#" + tblbodycontent + " tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
}

var skip = 10;
var pagenumber = 1;

function tbl_paging(content_paging, data) {

    if (data[0] != undefined) {

        var str_pg = "";
        var total;
        total = (Math.floor(data[0].total) / skip) + 1;

        for (var j = 1; j < total; j++) {
            if (j == pagenumber) {
                str_pg += "<li><button class='btn btn-success' onclick='_page(" + j + ")'>" + j + "</button></li>";
            }
            else {
                str_pg += "<li><button class='btn btn-default' onclick='_page(" + j + ")'>" + j + "</button></li>";
            }
        }

        content_paging.innerHTML = str_pg;
    }
}


function tbl_paging2(content_paging, data) {
    if (data != undefined) {

        var str_pg = "";
        var total;
        total = (Math.floor(data) / skip) + 1;

        for (var j = 1; j < total; j++) {
            if (j == pagenumber) {
                str_pg += "<li><button class='btn btn-success' onclick='FormPage(" + j + ")'>" + j + "</button></li>";
            }
            else {
                str_pg += "<li><button class='btn btn-default' onclick='FormPage(" + j + ")'>" + j + "</button></li>";
            }
        }

        content_paging.innerHTML = str_pg;
    }
}

$('.star-rate').hover(function () {
    var Id = $(this).attr("id");

    switch (Id) {
        case "1":
            $(this).nextAll('.star-rate').removeClass('filled');
            $(this).nextAll('.star-rate').removeClass('star-active');
            break;
        case "2":
            $(this).addClass('filled');
            $(this).prevAll('.star-rate').addClass('filled');
            $(this).nextAll('.star-rate').removeClass('filled');

            $(this).addClass('star-active');
            $(this).prevAll('.star-rate').addClass('star-active');
            $(this).nextAll('.star-rate').removeClass('star-active');
            break;
        case "3":
            $(this).addClass('filled');
            $(this).prevAll('.star-rate').addClass('filled');
            $(this).nextAll('.star-rate').removeClass('filled');

            $(this).addClass('star-active');
            $(this).prevAll('.star-rate').addClass('star-active');
            $(this).nextAll('.star-rate').removeClass('star-active');
            break;
        case "4":
            $(this).addClass('filled');
            $(this).prevAll('.star-rate').addClass('filled');
            $(this).nextAll('.star-rate').removeClass('filled');

            $(this).addClass('star-active');
            $(this).prevAll('.star-rate').addClass('star-active');
            $(this).nextAll('.star-rate').removeClass('star-active');
            break;
        case "5":
            $(this).addClass('filled');
            $(this).prevAll('.star-rate').addClass('filled');

            $(this).addClass('star-active');
            $(this).prevAll('.star-rate').addClass('star-active');
            break;
        default:
            break;
    }


});


function SkipAjx(url, callbackfunction) {

    $("#skip").change(function () {
        var _skip = $(this).val();
        skip = _skip;
        b_ajax(url, "Post", { skip: _skip }, function (data) {
            callbackfunction(data)
        }, null);
    });
}
function PageAjx(_pagenumber, url, callbackfunction) {
    pagenumber = _pagenumber;
    b_ajax(url, "Post", { pagenumber: pagenumber }, function (data) {
        callbackfunction(data)
    }, null);
}


function FormPageAction(action, pagenumber) {
    var form = document.createElement("form");
    var element1 = document.createElement("input");
    form.method = "Get";
    form.action = action;
    element1.value = pagenumber;
    element1.name = "pagenumber";
    form.appendChild(element1);
    document.body.appendChild(form);
    form.submit();
}

function FormPage(pagenumber) {
    var form = document.createElement("form");
    var element1 = document.createElement("input");
    form.method = "Get";
    form.action = "Index";
    element1.value = pagenumber;
    element1.name = "pagenumber";
    form.appendChild(element1);
    document.body.appendChild(form);
    form.submit();
}


function FormSkip(action, skip) {
    // var _skip = $(this).val();
    // skip = _skip;
    var form = document.createElement("form");
    var element1 = document.createElement("input");
    form.method = "Get";
    form.action = action;
    element1.value = skip;
    element1.name = "skip";
    form.appendChild(element1);
    document.body.appendChild(form);
    form.submit();
}

function Showimg(componentid, fileinputid, content) {
    $(componentid).change(function () {
        content.src = URL.createObjectURL(fileinputid.files[0]);
    });
}

function ResetImageInput(content) {
    content.src = "";
}

function ResetInputs() {
    var elements = document.getElementsByTagName("input");
    var textareaelements = document.getElementsByTagName("textarea");

    for (var ii = 0; ii < elements.length; ii++) {

        elements[ii].value = "";

    }
    for (var j = 0; j < textareaelements.length; j++) {

        textareaelements[j].value = "";

    }
}
function SetPictures(inputtarget, target, type) {
    var myURL = window.URL || window.webkitURL;
    var result = "";
    var tag = "";
    var _File = document.getElementById("" + inputtarget + "").files;
    for (var i = 0; i < _File.length; i++) {
        var fileURL = myURL.createObjectURL(_File[i]);


        tag = "<img src='" + fileURL + "' style='width:80px;height:60px;'>";

        result += tag;
    }

    $('#' + target + '').html(result);
}
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}

function SetInputFilter(targets) {
    for (var i = 0; i < targets.length; i++) {
        setInputFilter(document.getElementById(targets[i]), function (value) {
            return /^\d*\.?\d*$/.test(value);
        });
    }
}
function b_file_ajaxWithoutreset(url, type, data, callbackfunction, message) {

    if (type === null) {
        type = 'POST'
    }

    $.ajax({
        url: url,
        type: type,
        error: function (code) {
            if (code == null || code == undefined)
                unknown_error();
            errorInAjax(code);
        },
        data: data,
        enctype: 'multipart/form-data',
        processData: false,
        contentType: false,
        success: function (data) {

            if (callbackfunction != null)
                callbackfunction(data);
            if (message != null) {
                if (message == "ok" || message == "Ok") {
                    Success_ajax("Success");

                }
                else {
                    Success_ajax(message);
                }
            }

        }
    });
}