using MediatR;

namespace ShoesStore.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Unit> // Unit sẽ không trả về giá trị nào chỉ thực thi lệnh
    {
        public int Id { get; set; }
    }
}
