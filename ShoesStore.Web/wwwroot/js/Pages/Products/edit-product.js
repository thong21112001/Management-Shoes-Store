document.addEventListener('DOMContentLoaded', function () {
    const addVariantBtn = document.getElementById('add-variant-btn');
    const variantContainer = document.getElementById('variant-container');
    const variantTemplate = document.getElementById('variant-template');

    // QUAN TRỌNG: Bắt đầu chỉ số từ số lượng biến thể đã có
    let variantIndex = variantContainer.querySelectorAll('.variant-row').length;

    addVariantBtn.addEventListener('click', function () {
        const newVariantRow = variantTemplate.firstElementChild.cloneNode(true);
        updateAttributes(newVariantRow, variantIndex);
        variantContainer.appendChild(newVariantRow);
        variantIndex++;
    });

    window.removeVariant = function (button) {
        button.closest('.variant-row').remove();
        reindexVariants();
    };

    function reindexVariants() {
        const allVariantRows = variantContainer.querySelectorAll('.variant-row');
        let currentIndex = 0;
        allVariantRows.forEach(row => {
            updateAttributes(row, currentIndex);
            currentIndex++;
        });
        variantIndex = currentIndex; // Cập nhật lại chỉ số toàn cục
    }

    function updateAttributes(element, index) {
        // Cập nhật thuộc tính 'name' cho tất cả input/select
        element.querySelectorAll('[name*="[__INDEX__]"]').forEach(input => {
            input.name = input.name.replace('__INDEX__', index);
        });
        element.querySelectorAll('[name*="[i]"]').forEach(input => {
            input.name = input.name.replace('[i]', `[${index}]`);
        });

        // Cập nhật 'id' và 'onchange' cho preview ảnh
        element.querySelectorAll('[id*="__INDEX__"]').forEach(el => {
            el.id = el.id.replace('__INDEX__', index);
        });
        element.querySelectorAll('[onchange*="__INDEX__"]').forEach(el => {
            el.setAttribute('onchange', el.getAttribute('onchange').replace('__INDEX__', index));
        });
    }
});