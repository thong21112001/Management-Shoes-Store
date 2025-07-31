// Hàm load select2 và mutiple choice
$(function () {
    $('#productCategoryIds').select2({
        placeholder: "Chọn danh mục",
        allowClear: true,
        width: '100%'
    });

    $('#productBrandId').select2({
        theme: 'bootstrap-5', // Áp dụng theme Bootstrap 5
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: 'Chọn thương hiệu', // Ví dụ thêm placeholder
        allowClear: true, // Cho phép xóa lựa chọn
        language: 'vi'
    });
});

// Hàm preview ảnh chung
function previewImage(input, previewId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById(previewId).setAttribute('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}