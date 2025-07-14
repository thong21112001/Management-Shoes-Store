using MediatR;
using ShoesStore.Application.Features.Categories.Queries.Shared;

namespace ShoesStore.Application.Features.Categories.Queries.GetCatesByIdQuery
{
    public class GetCatesByIdQuery : IRequest<CategoryDto>
    {
        public int Id { get; set; }
    }
}
