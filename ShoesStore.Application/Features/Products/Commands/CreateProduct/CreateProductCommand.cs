using MediatR;
using ShoesStore.Application.Features.Products.Queries.Shared.ProductVariant;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = null!;

        [Display(Name = "Thông tin sản phẩm")]
        public string? Description { get; set; }

        [Display(Name = "Thương hiệu giày")]
        public int BrandId { get; set; }

        public byte[]? MainImageUrl { get; set; }

        public bool? IsActive { get; set; }

        [Display(Name = "Danh mục giày")]
        public List<int> CategoryIds { get; set; } = new(); // Danh sách Id của các category

        //Danh sách các biến thể được gửi từ UI
        public List<ProductVarDto> Variants { get; set; } = new();
    }
}
