// Write your Javascript code.
$(function () {

    $.ajaxSetup({
        cache: false
    });

    $("a[class*=data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');


        $('#myModalContent').load(this.href, function () {
            $.validator.unobtrusive.parse(this);

            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;
    });


});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}

$('#search').keyup(function () {
    $('#table').DataTable().search($(this).val()).draw();
})

jQuery.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).addClass(errorClass).removeClass(validClass);
        } else {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            $(element).closest('.form-control-feedback').removeClass('glyphicon-ok').addClass('glyphicon-remove');
            $(element).nextAll('.glyphicon').removeClass('hidden');
            $(element).nextAll('.glyphicon').removeClass('glyphicon-ok').addClass('glyphicon-remove');
            $(element).nextAll('.glyphicon').removeClass('has-success').addClass('has-error');
        }
    },
    unhighlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).removeClass(errorClass).addClass(validClass);
        } else {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
            $(element).nextAll('.glyphicon').removeClass('hidden');
            $(element).nextAll('.glyphicon').removeClass('glyphicon-remove').addClass('glyphicon-ok');
            $(element).nextAll('.glyphicon').removeClass('has-error').addClass('has-success');
        }
    }
});

$(function () {

    $("span.field-validation-valid, span.field-validation-error").addClass('help-block');
    $("div.form-group").has("span.field-validation-error").addClass('has-error');
    $("div.validation-summary-errors").has("li:visible").addClass("alert");
});
