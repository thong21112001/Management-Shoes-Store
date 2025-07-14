using MediatR;
using ShoesStore.Application.Features.Categories.Queries.Shared;

namespace ShoesStore.Application.Features.Categories.Queries.GetAllCates
{
    public class GetAllCatesQuery : IRequest<List<CategoriesListDto>> { }
}