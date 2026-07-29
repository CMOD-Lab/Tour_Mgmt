// Tour Management - Site JavaScript
(function () {
    'use strict';

    // Auto-dismiss alerts after 5 seconds
    window.addEventListener('load', function () {
        var alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            setTimeout(function () {
                var bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000);
        });
    });
})();
