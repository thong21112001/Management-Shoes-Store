const preview = document.getElementById("preview");
const defaultImage = "https://placehold.co/300x200/E8F5E8/7cb342?text=Chưa+chọn+ảnh";

function previewImage(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function resetImage() {
    const input = document.querySelector("input[type='file'][name='Upload']");
    input.value = ""; // reset input file
    preview.src = defaultImage;
}