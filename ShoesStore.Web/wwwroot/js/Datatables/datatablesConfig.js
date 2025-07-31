// datatablesconfig.js
(function (window, $) {
    $.fn.DataTable.ext.pager.numbers_length = 4;

    window.initDataTable = function (selector, userOpts = {}) {
        const $t = $(selector);

        const defaults = {
            scrollX: true,
            scrollCollapse: true,
            pagingType: "full_numbers",
            language: {
                url: "/data/i18n/vndatatables.json",
                paginate: {
                    first: '<i class="fa fa-angle-double-left"></i>',
                    previous: '<i class="fa fa-angle-left"></i>',
                    next: '<i class="fa fa-angle-right"></i>',
                    last: '<i class="fa fa-angle-double-right"></i>'
                },
                processing:
                    '<div class="spinner-border spinner-border-sm text-primary me-2" role="status"></div>Đang xử lý...'
            },
            dom:
                `<"dt-header d-flex flex-wrap justify-content-between align-items-center mb-3"
                <"dt-length"l>
                <"dt-filter"f>
             >
             rt
             <"dt-footer d-flex flex-wrap justify-content-between align-items-center"
                <"dt-info"i>
                <"dt-paginate"p>
             >`,
            responsive: false,
            autoWidth: false,
            deferRender: true,
            processing: true,
            order: [],
            lengthMenu: [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "Tất cả"]
            ],
            pageLength: 10,
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "stt", orderable: false }
            ],
            initComplete: function () {
                // show table
                $t.removeClass("d-none").addClass("table-striped");
                // adjust once
                this.api().columns.adjust();
            },
            drawCallback: function () {
                // update STT
                const api = this.api(),
                    info = api.page.info();
                api
                    .column(".stt", { search: "applied", order: "applied" })
                    .nodes()
                    .each((cell, i) => {
                        cell.innerHTML = info.start + i + 1;
                    });
            }
        };

        const cfg = $.extend(true, {}, defaults, userOpts);
        cfg.destroy = true;

        return $t.DataTable(cfg);
    };
})(window, jQuery);