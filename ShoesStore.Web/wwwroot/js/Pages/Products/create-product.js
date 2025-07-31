document.addEventListener('DOMContentLoaded', function () {
    const addVariantBtn = document.getElementById('add-variant-btn');
    const variantContainer = document.getElementById('variant-container');
    const variantTemplate = document.getElementById('variant-template');
    let variantIndex = 0;

    addVariantBtn.addEventListener('click', function () {
        // Sao chép nội dung từ template
        const newVariantRow = variantTemplate.firstElementChild.cloneNode(true);

        // Cập nhật các thuộc tính 'name' và 'id' với chỉ số mới
        updateAttributes(newVariantRow, variantIndex);

        // Thêm dòng mới vào container
        variantContainer.appendChild(newVariantRow);

        // Tăng chỉ số cho lần thêm tiếp theo
        variantIndex++;
    });

    // Hàm để xóa một dòng biến thể
    window.removeVariant = function (button) {
        button.closest('.variant-row').remove();
        // Sau khi xóa, cập nhật lại chỉ số của tất cả các dòng còn lại
        reindexVariants();
    };

    // Hàm cập nhật lại chỉ số (quan trọng cho model binding)
    function reindexVariants() {
        const allVariantRows = variantContainer.querySelectorAll('.variant-row');
        variantIndex = 0; // Reset lại chỉ số
        allVariantRows.forEach(row => {
            updateAttributes(row, variantIndex);
            variantIndex++;
        });
    }

    // Hàm helper để cập nhật các thuộc tính
    function updateAttributes(element, index) {
        element.querySelectorAll('[name]').forEach(input => {
            input.name = input.name.replace('__INDEX__', index);
        });
        element.querySelectorAll('[id*="__INDEX__"]').forEach(el => {
            el.id = el.id.replace('__INDEX__', index);
        });
        element.querySelectorAll('[onchange*="__INDEX__"]').forEach(el => {
            el.setAttribute('onchange', el.getAttribute('onchange').replace('__INDEX__', index));
        });
    }
});