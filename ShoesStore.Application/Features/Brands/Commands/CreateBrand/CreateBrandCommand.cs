using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Brands.Commands.CreateBrand
{
    // IRequest<int> nghĩa là command này khi thực thi sẽ trả về một giá trị kiểu int (Id của brand mới)
    //Giống như tạo mới một brand và trả về Id của brand đó
    public class CreateBrandCommand : IRequest<int>
    {
        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Tên thương hiệu")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public byte[]? LogoUrl { get; set; }
    }
}
