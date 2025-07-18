using MediatR;
using ShoesStore.Application.Features.Sizes.Queries.Shared;

namespace ShoesStore.Application.Features.Sizes.Queries.GetAllSizes
{
    public class GetAllSizesQuery : IRequest<List<SizesListDto>> { }
}
