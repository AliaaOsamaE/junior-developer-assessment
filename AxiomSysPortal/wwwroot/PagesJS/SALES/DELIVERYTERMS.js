$(function () {
    $('#dataTable').DataTable({
        bFilter: true,
        sDom: 'ftlpi',
        language: window.datatableTexts.language,
        columnDefs: [{ orderable: false, targets: 5 }],
        initComplete: function () {
            $('.dataTables_filter').appendTo('#tableSearch');
        }
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
