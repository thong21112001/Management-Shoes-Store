﻿document.addEventListener("DOMContentLoaded", function () {
    // --- Helper adjust DataTables: cải thiện và tối ưu hóa ---
    function debounce(func, wait = 200) {
        let timeout;
        return function () {
            clearTimeout(timeout);
            timeout = setTimeout(func, wait);
        };
    }

    function adjustTables() {
        $('.dataTable').each(function () {
            const table = $(this).DataTable();
            if (table && $.fn.DataTable.isDataTable(this)) {
                // Force recalculation với timeout để đảm bảo DOM đã render xong
                setTimeout(() => {
                    table.columns.adjust();
                    table.draw();

                    // Responsive recalc nếu có
                    if (table.responsive && typeof table.responsive.recalc === 'function') {
                        table.responsive.recalc();
                    }
                }, 100);
            }
        });
    }

    const adjustTablesDebounced = debounce(adjustTables, 150);

    // Event listeners cho resize tables
    window.addEventListener('resize', adjustTablesDebounced);
    window.addEventListener('orientationchange', adjustTablesDebounced);

    // Custom event khi sidebar toggle
    document.addEventListener('sidebarToggled', () => {
        setTimeout(adjustTables, 350); // Đợi animation hoàn thành
    });

    // --- KHỞI TẠO CÁC THƯ VIỆN ---
    // 1. Select2
    $(".select2-single").select2({
        theme: "bootstrap-5",
        dropdownParent: $("#addProductModal"),
    });

    // --- LOGIC GIAO DIỆN ---
    const body = document.body;
    const htmlEl = document.documentElement;
    const sidebar = document.getElementById("sidebar");
    const themeToggle = document.getElementById("theme-toggle");
    const sunIcon = document.getElementById("theme-icon-sun");
    const moonIcon = document.getElementById("theme-icon-moon");

    // 1. Chuyển đổi theme Sáng/Tối
    if (themeToggle && sunIcon && moonIcon) {
        const applyTheme = (theme) => {
            htmlEl.setAttribute("data-bs-theme", theme);
            sunIcon.classList.toggle("d-none", theme === "dark");
            moonIcon.classList.toggle("d-none", theme !== "dark");
        };

        const savedTheme = localStorage.getItem("theme") || "light";
        applyTheme(savedTheme);

        themeToggle.addEventListener("click", () => {
            const currentTheme = htmlEl.getAttribute("data-bs-theme");
            const newTheme = currentTheme === "dark" ? "light" : "dark";
            localStorage.setItem("theme", newTheme);
            applyTheme(newTheme);
        });
    }

    // 2. Mobile Sidebar Backdrop
    const backdrop = document.createElement("div");
    backdrop.className = "sidebar-backdrop";
    body.appendChild(backdrop);

    const hideMobileSidebar = () => {
        if (sidebar) sidebar.classList.remove("active");
        backdrop.classList.remove("show");

        // Dispatch custom event
        document.dispatchEvent(new CustomEvent('sidebarToggled'));
    };

    backdrop.addEventListener("click", hideMobileSidebar);

    // 3. Toggle sidebar trên mobile
    const mobileSidebarToggle = document.getElementById("mobile-sidebar-toggle");
    if (mobileSidebarToggle) {
        mobileSidebarToggle.addEventListener("click", () => {
            if (sidebar) sidebar.classList.toggle("active");
            backdrop.classList.toggle("show");

            // Dispatch custom event
            document.dispatchEvent(new CustomEvent('sidebarToggled'));
        });
    }

    // 4. Thu gọn/mở rộng sidebar trên desktop - CẢI THIỆN MƯỢT MÀ
    const desktopSidebarToggle = document.getElementById("desktop-sidebar-toggle");

    const applySidebarState = (state) => {
        // Thêm class transition trước khi thay đổi
        body.classList.add('sidebar-transitioning');

        if (state === "collapsed") {
            body.classList.add("sidebar-collapsed");
        } else {
            body.classList.remove("sidebar-collapsed");
        }

        // Dispatch resize event để các component khác biết sidebar đã thay đổi
        window.dispatchEvent(new Event("resize"));

        // Dispatch custom event cho DataTables
        document.dispatchEvent(new CustomEvent('sidebarToggled'));

        // Remove transition class sau khi animation hoàn thành
        setTimeout(() => {
            body.classList.remove('sidebar-transitioning');
        }, 300);
    };

    const savedSidebarState = localStorage.getItem("sidebarState");
    if (savedSidebarState) {
        applySidebarState(savedSidebarState);
    }

    if (desktopSidebarToggle) {
        desktopSidebarToggle.addEventListener("click", () => {
            const isCollapsed = body.classList.contains("sidebar-collapsed");
            const newState = isCollapsed ? "expanded" : "collapsed";
            localStorage.setItem("sidebarState", newState);
            applySidebarState(newState);
        });
    }

    // 5. REMOVED: Logic cũ để mở rộng sidebar đã được loại bỏ
    //    để thay thế bằng hiệu ứng pop-out menu trong CSS.

    // 6. Xử lý trạng thái active của menu khi click
    const navLinks = document.querySelectorAll(
        '.sidebar .nav-link:not([data-bs-toggle="collapse"])'
    );

    navLinks.forEach((link) => {
        link.addEventListener("click", function () {
            // Chỉ xử lý khi click vào link con trong menu popout khi sidebar thu gọn
            if (body.classList.contains('sidebar-collapsed') && this.closest('.collapse')) {
                // Không cần làm gì thêm, để link điều hướng bình thường
            } else {
                // Xử lý active cho các trường hợp khác
                document
                    .querySelectorAll(".sidebar .nav-link.active")
                    .forEach((l) => l.classList.remove("active"));
                this.classList.add("active");

                const parentCollapse = this.closest(".collapse");
                if (parentCollapse) {
                    const parentLink = document.querySelector(
                        `a[href="#${parentCollapse.id}"]`
                    );
                    if (parentLink) parentLink.classList.add("active");
                }
            }


            if (window.innerWidth < 992) {
                hideMobileSidebar();
            }
        });
    });

    // 7. Tự động highlight menu theo URL được xử lý bởi server-side (Razor) nên không cần ở đây.

    // --- 8. Chuyển trang mượt ---
    document.body.classList.add("fade-in");

    document.querySelectorAll("a[href]:not([target]):not([data-bs-toggle]):not([href^='#']):not([href^='http'])").forEach(link => {
        const href = link.getAttribute("href");
        if (href && href !== '#' && !href.startsWith("javascript:")) {
            link.addEventListener("click", function (e) {
                try {
                    const url = new URL(href, window.location.origin);
                    if (url.origin === window.location.origin && url.pathname !== window.location.pathname) {
                        e.preventDefault();
                        document.body.classList.remove("fade-in");
                        document.body.classList.add("fade-out");
                        setTimeout(() => {
                            window.location.href = href;
                        }, 250);
                    }
                } catch (error) {
                    console.warn('Could not parse URL:', href);
                }
            });
        }
    });

    // --- 9. Khởi tạo lại tables khi cần thiết ---
    const tableObserver = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.type === 'childList') {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1) { // Element node
                        const tables = node.querySelectorAll?.('.dataTable') ||
                            (node.classList?.contains('dataTable') ? [node] : []);
                        if (tables.length > 0) {
                            setTimeout(adjustTables, 100);
                        }
                    }
                });
            }
        });
    });

    tableObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    // --- 10. Cleanup khi trang unload ---
    window.addEventListener('beforeunload', () => {
        tableObserver.disconnect();
    });

    // --- Final adjustment sau khi DOM hoàn toàn ready ---
    setTimeout(() => {
        adjustTables();
        document.body.classList.remove("page-transitioning", "fade-out");
        if (!document.body.classList.contains("fade-in")) {
            document.body.classList.add("fade-in");
        }
    }, 100);
});
