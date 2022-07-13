//$(document).ready(function () {

//    //option A
//    $("#registerform").submit(function (e) {
//        e.preventDefault();
//    });

//});

function validateForm() {
    var cansubmit = true;
    
    $("input.myClassvalidation").each(function () {
        inputVal = $(this).val();
        if (inputVal == "") {
            //set error
            cansubmit = false;
            return setErrorFor($(this), $(this).attr('placeholder') + ' is required.');
        }
        else {
            //set success
            setSuccessFor($(this));
        }
    });
    var password = $("input.Password").val();
    var ConfirmPassword = $("input.ConfirmPassword").val();
    if (password != ConfirmPassword && ConfirmPassword != "") {
        return setErrorFor($("input.ConfirmPassword"), $("input.ConfirmPassword").attr('placeholder') + ' is not match with password.');
    }

    return cansubmit;
}

function validateFormJs() {
    var cansubmit = true;
    $("input.validate").each(function () {
        inputVal = $(this).val();
        if (inputVal == "") {
            //set error
            cansubmit = false;

            setErrorForJs($(this), $(this).attr('id') + ' is required.');
        }
    });
    return cansubmit;
}


function setErrorForJs(input, message) {
    var small = '<small id="errinput" class="text-danger">' + message + '<small>';
    

    $("#errinput").remove();

    input.after(small);

    $("#errinput").slideUp(10000)
}


function setErrorFor(input, message) {
    const formControl = input[0].parentElement;
    const small = formControl.querySelector('small');
    formControl.classList.add("error");
    small.innerText = message;
    return false;
}

function setSuccessFor(input) {
    const formControl = input[0].parentElement;
    formControl.classList.add("success");
}