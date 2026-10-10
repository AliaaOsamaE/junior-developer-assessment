function showToast(type, message) {
    if (!message) {
        return;
    }

    var color = type === 'success' ? 'bg-success' : 'bg-danger';

    var toast = document.createElement('div');
    toast.className = 'toast align-items-center border-0 text-white ' + color;
    toast.setAttribute('role', 'alert');

    var row = document.createElement('div');
    row.className = 'd-flex';

    var body = document.createElement('div');
    body.className = 'toast-body';
    body.textContent = message;

    var closeButton = document.createElement('button');
    closeButton.type = 'button';
    closeButton.className = 'btn-close btn-close-white me-2 m-auto';
    closeButton.setAttribute('data-bs-dismiss', 'toast');

    row.appendChild(body);
    row.appendChild(closeButton);
    toast.appendChild(row);
    document.getElementById('toastContainer').appendChild(toast);

    toast.addEventListener('hidden.bs.toast', function () {
        toast.remove();
    });

    new bootstrap.Toast(toast, { delay: 4000 }).show();
}

document.addEventListener('DOMContentLoaded', function () {
    var container = document.getElementById('toastContainer');
    if (container) {
        showToast('success', container.dataset.success);
        showToast('error', container.dataset.error);
    }
});
