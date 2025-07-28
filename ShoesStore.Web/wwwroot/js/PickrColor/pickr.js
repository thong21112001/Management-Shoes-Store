document.addEventListener("DOMContentLoaded", function () {
    let nearest;

    // Load file tên màu từ file JSON tự copy :v
    fetch('/data/colornames.json')
        .then(response => response.json())
        .then(data => {
            const colorList = {};
            data.forEach(c => {
                colorList[c.name] = c.hex;
            });
            nearest = window.nearestColor.from(colorList);
        });

    const pickr = Pickr.create({
        el: '#color-picker-btn',
        theme: 'classic', // hoặc 'nano', 'monolith'
        default: '#42445A',
        i18n: {
            'btn:save': 'Lưu'
        },
        components: {
            preview: true,
            opacity: true,
            hue: true,

            interaction: {
                hex: true,
                input: true,
                save: true
            }
        }
    });

    // Khi bấm nút "Lưu"
    pickr.on('save', (color, instance) => {
        const hexColor = color.toHEXA().toString().toLowerCase();

        // Tìm tên gần đúng nhất
        const nearestColor = nearest(hexColor);
        const colorName = nearestColor.name;

        // Gán mã màu vào input form (asp-for)
        document.querySelector('input[name="ColorCommand.Hexcode"]').value = hexColor;
        document.querySelector('input[name="ColorCommand.Name"]').value = colorName;
    });
});