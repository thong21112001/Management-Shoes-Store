// wwwroot/js/alert/AlertHelper.js
(function () {
    document.addEventListener("DOMContentLoaded", function () {
        const el = document.getElementById("alert-data");
        if (!el) return;

        let alerts;
        try {
            alerts = JSON.parse(el.dataset.alerts);
        } catch (e) {
            console.warn("AlertHelper: JSON.parse error", e);
            return;
        }

        if (!Array.isArray(alerts) || alerts.length === 0) return;

        // Tạo một promise khởi đầu
        let sequence = Promise.resolve();

        alerts.forEach(a => {
            if (a && a.Type !== undefined && a.Message) {
                sequence = sequence.then(() => {
                    return Swal.fire({
                        icon: mapAlertType(a.Type),
                        title: a.Message,
                        timer: 3000,
                        showConfirmButton: false,
                        allowOutsideClick: false,
                        allowEscapeKey: false
                    });
                });
            }
        });
    });

    function mapAlertType(n) {
        return {
            0: 'success',
            1: 'error',
            2: 'warning',
            3: 'info'
        }[n] || 'info';
    }
})();