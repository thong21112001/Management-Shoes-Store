using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Brands.Queries.Shared
{
    public class BrandDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên thương hiệu")]
        public string Name { get; set; } = null!;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public byte[]? LogoUrl { get; set; }

        public string? ImageRenderedUrl =>
            LogoUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(LogoUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

        [Display(Name = "Ngày tạo"), DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhập"), DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }
    }
}
