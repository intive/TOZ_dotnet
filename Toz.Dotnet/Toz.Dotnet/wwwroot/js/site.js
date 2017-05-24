// Write your Javascript code.
$(document).ready(function () {
    var image;
    var canvas;
    var jcropApi;
    var context;
    var prefSize;
    var cropMaxWidth = 600;
    var cropMaxHeight = 600;

    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [label]);
    });

    $('.btn-file :file').on('fileselect', function (event, label) {
        var input = $(this).parents('.input-group').find(':text'),
            log = label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }
    });

    function loadImage(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            canvas = null;
            reader.onload = function (e) {
                image = new Image();
                image.onload = validateImage();
                image.src = e.target.result;
                //$('#img-upload').attr('src', e.target.result);
                //$('#img-upload').attr('onload', validateImage);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }

    function validateImage() {
        if (canvas != null) {
            image = new Image();
            image.onload = restartJcrop;
            image.src = canvas.toDataURL('image/png');
        } else restartJcrop();
    }

    function restartJcrop() {
        if (jcropApi != null) {
            jcropApi.destroy();
        }
        $("#img-upload").empty();
        $("#img-upload").append("<canvas id=\"canvas\">");
        canvas = $("#canvas")[0];
        context = canvas.getContext("2d");
        canvas.width = image.width;
        canvas.height = image.height;
        context.drawImage(image, 0, 0);
        $("#canvas").Jcrop({
            onSelect: selectcanvas,
            onRelease: clearcanvas,
            boxWidth: cropMaxWidth,
            boxHeight: cropMaxHeight
        }, function () {
            jcropApi = this;
        });
        clearcanvas();
    }

    function clearcanvas() {
        prefSize = {
            x: 0,
            y: 0,
            w: canvas.width,
            h: canvas.height
        };
    }

    function selectcanvas(coords) {
        prefSize = {
            x: Math.round(coords.x),
            y: Math.round(coords.y),
            w: Math.round(coords.w),
            h: Math.round(coords.h)
        };
    }

    $("#imgInp").change(function () {
        loadImage(this);
    });

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

    $('#search').keyup(function() {
        $('#table').DataTable().search($(this).val()).draw();
    });

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
});