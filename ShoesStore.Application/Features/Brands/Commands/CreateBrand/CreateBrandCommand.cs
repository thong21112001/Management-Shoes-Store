using MediatR;

namespace ShoesStore.Application.Features.Brands.Commands.CreateBrand
{
    // IRequest<int> nghĩa là command này khi thực thi sẽ trả về một giá trị kiểu int (Id của brand mới)
    //Giống như tạo mới một brand và trả về Id của brand đó
    public class CreateBrandCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? LogoUrl { get; set; }
    }
}
