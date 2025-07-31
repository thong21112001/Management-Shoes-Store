using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Products.Queries.Shared.ProductVariant
{
    public class ProductVarDetailsDto
    {
        public int Id { get; set; }

        [Display(Name = "Màu sắc")]
        public string ColorName { get; set; } = string.Empty;

        [Display(Name = "Mã màu (Hex)")]
        public string ColorHexCode { get; set; } = string.Empty;

        [Display(Name = "Kích cỡ")]
        public string SizeValue { get; set; } = string.Empty;

        [Display(Name = "SKU")]
        public string Sku { get; set; } = string.Empty;

        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; }

        [Display(Name = "Ảnh biến thể")]
        public byte[]? ImageUrl { get; set; }
    }
}
