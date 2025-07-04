// Hiển thị chi tiết
function showDetails(id) {
    $.ajax({
        url: '/Brands/Index?handler=DetailsPartial&id=' + id,
        type: 'GET',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (result) {
            $('#detailsModalContent').html(result);
            setTimeout(function () {
                $('#detailsModal').modal('show');
            }, 500);
        },
        error: function (error) {
            alert('Đã xảy ra lỗi khi tải dữ liệu!');
        }
    });
}


// Hiển thị xác nhận xóa
function showDeleteConfirm(id) {
    $('#detailsModal').modal('hide');

    $.ajax({
        url: '/Brands/Index?handler=DeletePartial&id=' + id,
        type: 'GET',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (result) {
            $('#detailsModalContent').html(result);
            setTimeout(function () {
                $('#detailsModal').modal('show');
            }, 500);
        },
        error: function (error) {
            alert('Đã xảy ra lỗi khi tải dữ liệu!');
        }
    });
}


// Thực hiện xóa
function deleteBrand(id) {
    $.ajax({
        url: '/Brands/Index?handler=DeleteBrand',
        type: 'POST',
        data: { id: id },
        headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            $('#detailsModal').modal('hide');

            if (result.success) {
                // Hủy DataTable hiện tại
                var table = $('#myTable').DataTable();
                table.destroy();

                // Xóa hàng khỏi DOM
                $('#myTable tbody tr').each(function () {
                    if ($(this).find('a[onclick*="showDetails(' + id + ')"]').length > 0) {
                        $(this).remove();
                    }
                });

                // Khởi tạo lại DataTable
                initDataTable("#myTable", {
                    order: [[1, "asc"]],
                    columnDefs: [
                        { targets: 0, className: "text-center" },
                        { targets: 3, className: "text-center" }
                    ]
                });

                // Hiển thị thông báo thành công
                toastr.options = {
                    "closeButton": true,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "timeOut": "2000"
                };
                toastr.success('Xóa dữ liệu thành công!');
            } else {
                toastr.error(result.message || 'Đã xảy ra lỗi khi xóa!');
            }
        },
        error: function (error) {
            $('#detailsModal').modal('hide');
            toastr.error('Đã xảy ra lỗi khi xóa!');
            console.log(error);
        }
    });
}