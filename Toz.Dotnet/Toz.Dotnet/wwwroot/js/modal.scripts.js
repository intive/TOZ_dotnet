$(document).ready(function () {
    var image;
    var canvas;
    var jcropApi;
    var context;
    var prefSize;
    var cropMaxWidth = 400;
    var cropMaxHeight = 400;

    var form = $('.form-horizontal')
        .removeData("validator")
        .removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse(form);

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
        $("#view").empty();
        $("#view").append("<canvas id=\"canvas\">");
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
});