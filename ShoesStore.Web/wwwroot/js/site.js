document.addEventListener("DOMContentLoaded", function () {
    // --- KHỞI TẠO CÁC THƯ VIỆN ---
    // 1. DataTables
    $("#productsTable").DataTable({
        language: {
            url: "https://cdn.datatables.net/plug-ins/2.0.8/i18n/vi.json",
        },
        responsive: true,
    });

    // 2. Select2
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
    };

    backdrop.addEventListener("click", hideMobileSidebar);

    // 3. Toggle sidebar trên mobile
    const mobileSidebarToggle = document.getElementById("mobile-sidebar-toggle");
    if (mobileSidebarToggle) {
        mobileSidebarToggle.addEventListener("click", () => {
            if (sidebar) sidebar.classList.toggle("active");
            backdrop.classList.toggle("show");
        });
    }

    // 4. Thu gọn/mở rộng sidebar trên desktop
    const desktopSidebarToggle = document.getElementById("desktop-sidebar-toggle");
    const applySidebarState = (state) => {
        if (state === "collapsed") {
            body.classList.add("sidebar-collapsed");
        } else {
            body.classList.remove("sidebar-collapsed");
        }
        window.dispatchEvent(new Event("resize"));
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

    // 5. Tự động mở rộng Sidebar thu gọn khi nhấp vào menu đa cấp
    const collapseLinks = document.querySelectorAll(
        '.sidebar .nav-link[data-bs-toggle="collapse"]'
    );
    collapseLinks.forEach((link) => {
        link.addEventListener("click", function (e) {
            if (body.classList.contains("sidebar-collapsed")) {
                e.stopImmediatePropagation();

                localStorage.setItem("sidebarState", "expanded");
                applySidebarState("expanded");

                const targetCollapse = document.querySelector(
                    this.getAttribute("href")
                );
                setTimeout(() => {
                    if (targetCollapse) {
                        new bootstrap.Collapse(targetCollapse, {
                            toggle: true,
                        });
                    }
                }, 300);
            }
        });
    });

    // 6. Xử lý trạng thái active của menu
    const navLinks = document.querySelectorAll(
        '.sidebar .nav-link:not([data-bs-toggle="collapse"])'
    );

    navLinks.forEach((link) => {
        link.addEventListener("click", function () {
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

            if (window.innerWidth < 992) {
                hideMobileSidebar();
            }
        });
    });

    // 7. Tự động highlight menu theo URL thực tế
    const currentPath = window.location.pathname.toLowerCase();

    document.querySelectorAll('.sidebar .nav-link[href]').forEach((link) => {
        const href = link.getAttribute('href').toLowerCase();
        if (href && currentPath.includes(href)) {
            // Xóa active cũ
            document.querySelectorAll('.sidebar .nav-link.active')
                .forEach((l) => l.classList.remove('active'));

            // Thêm active cho link hiện tại
            link.classList.add('active');

            // Thêm active cho menu cha nếu có
            const parentCollapse = link.closest('.collapse');
            if (parentCollapse) {
                const parentLink = document.querySelector(
                    `a[href="#${parentCollapse.id}"]`
                );
                if (parentLink) parentLink.classList.add('active');

                // Mở submenu nếu đang collapsed
                parentCollapse.classList.add('show');
            }
        }
    });
});