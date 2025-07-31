using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Products.Queries.Shared.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Thương hiệu giày")]
        public string Brand { get; set; } = string.Empty;

        [Display(Name = "Danh mục giày")]
        public string CategoryIds { get; set; } = string.Empty;

        [Display(Name = "Số lượng biến thể")]
        public int AnyProductVariant { get; set; }
    }
}
