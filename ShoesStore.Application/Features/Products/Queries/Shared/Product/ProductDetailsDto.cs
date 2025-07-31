using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Products.Queries.Shared.Product
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Thương hiệu")]
        public string BrandName { get; set; } = string.Empty;

        [Display(Name = "Ảnh chính")]
        public byte[]? MainImageUrl { get; set; }

        public string? ImageRenderedUrl =>
            MainImageUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(MainImageUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

        [Display(Name = "Ngày tạo"), DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhập"), DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Các danh mục")]
        public string Categories { get; set; } = string.Empty;

        [Display(Name = "Số lượng biến thể")]
        public int AnyProductVar { get; set; }

        //[Display(Name = "Các biến thể")]
        //public List<ProductVarDetailsDto> Variants { get; set; } = new();
    }
}
