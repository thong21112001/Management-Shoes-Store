using MediatR;

namespace ShoesStore.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest<Unit> // Unit sẽ không trả về giá trị nào chỉ thực thi lệnh
    {
        public int Id { get; set; }
    }
}
