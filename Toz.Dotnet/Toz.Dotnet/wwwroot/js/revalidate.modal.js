$(document).ready(function () {
    var form = $('.form-horizontal')
        .removeData("validator")
        .removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse(form);
});