// Write your Javascript code.
$(document).ready(function () {
    $('input[type="submit"]').on('click', function () {
        $(this).prop("disabled", true);
    });

    $('input[type="button"]').on('click', function () {
        $(this).prop("disabled", true);
    });

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
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#img-upload').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imgInp").change(function () {
        readURL(this);
    });

    $(function () {

        $.ajaxSetup({ cache: false });

        $("a[data-modal]").on("click", function (e) {

            // hide dropdown if any
            $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');


            $('#myModalContent').load(this.href, function () {


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

    $('#table').DataTable({
        "dom": 't<"panel-footer"p>',
        "language": {
            "zeroRecords": "Nie znaleziono pasujących rekordów",
            "emptyTable": "Brak rekordów",
            "paginate": {
                "first": "Pierwsza",
                "last": "Ostatnia",
                "next": "Następna",
                "previous": "Poprzednia"
            }
        }
    });

    $('#search').keyup(function () {
        $('#table').DataTable().search($(this).val()).draw();
    })
});
