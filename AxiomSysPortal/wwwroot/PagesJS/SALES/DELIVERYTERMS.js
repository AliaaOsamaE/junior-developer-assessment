$(function () {
    $('#dataTable').DataTable({
        bFilter: true,
        sDom: 'ftlpi',
        language: window.datatableTexts.language,
        order: [[0, 'asc']],
        columnDefs: [
            { type: 'num', targets: [0, 3] },
            { orderable: false, targets: 4 }
        ],
        initComplete: function () {
            $('.dataTables_filter').appendTo('#tableSearch');
        }
    });

    $('.needs-validation').on('submit', function (event) {
        var form = this;

        if (!form.checkValidity()) {
            event.preventDefault();
            form.classList.add('was-validated');
            return;
        }

        if (form.dataset.sent === 'true') {
            event.preventDefault();
            return;
        }
        form.dataset.sent = 'true';
    });

    $('#addModal').on('show.bs.modal', function () {
        var form = this.querySelector('form');
        form.reset();
        form.classList.remove('was-validated');
    });

    $('#addModal').on('shown.bs.modal', function () {
        $('#addCode').trigger('focus');
    });

    $('#editModal').on('hidden.bs.modal', function () {
        this.querySelector('form').classList.remove('was-validated');
    });

    $('#dataTable').on('click', '.edit-button', function () {
        var code = $(this).data('code');

        $.getJSON('?handler=DeliveryTerm&code=' + code)
            .done(function (term) {
                $('#editCode').val(term.code);
                $('#editSName').val(term.sName);
                $('#editBName').val(term.bName);
                $('#editDays').val(term.days);
                $('#editActive').prop('checked', term.active);
                bootstrap.Modal.getOrCreateInstance(document.getElementById('editModal')).show();
            })
            .fail(function () {
                showToast('error', window.appTexts.unexpectedError);
            });
    });

    $('#dataTable').on('click', '.delete-button', function () {
        var code = $(this).data('code');

        Swal.fire({
            title: window.appTexts.confirmDeleteTitle,
            text: window.appTexts.confirmDeleteText,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: window.appTexts.yes,
            cancelButtonText: window.appTexts.no,
            confirmButtonColor: '#dc3545'
        }).then(function (result) {
            if (result.isConfirmed) {
                $('#deleteCode').val(code);
                $('#deleteForm').submit();
            }
        });
    });
});
