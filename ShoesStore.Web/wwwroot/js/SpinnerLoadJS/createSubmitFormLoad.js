$(function () {
    // Gắn sự kiện submit cho form
    $('#createForm').on('submit', function (event) {
        const $form = $(this);

        if ($form.valid()) {
            event.preventDefault(); // Ngăn submit tạm thời để hiển thị loader

            // Disable nút và đổi text
            $('#submitBtn').prop('disabled', true).val('Đang xử lý...');

            // Hiển thị overlay loader
            $('#loaderOverlay').css('display', 'flex');

            // Chờ 3 giây rồi mới submit thật
            setTimeout(function () {
                $form.off('submit'); // Gỡ handler để tránh lặp vô hạn
                $form[0].submit();  // Submit lại form thật sự
            }, 3000);
        }
        // Nếu không hợp lệ → jQuery Validation tự xử lý, không cần làm gì thêm
    });

    // Khi có lỗi server, kích hoạt lại nút
    if ($('.text-danger').length > 0) {
        $('#submitBtn').prop('disabled', false).val('Tạo mới');
    }
});