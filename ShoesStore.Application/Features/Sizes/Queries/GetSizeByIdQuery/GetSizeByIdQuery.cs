using MediatR;
using ShoesStore.Application.Features.Sizes.Queries.Shared;

namespace ShoesStore.Application.Features.Sizes.Queries.GetSizeByIdQuery
{
    public class GetSizeByIdQuery : IRequest<SizeDto>
    {
        public int Id { get; set; }
    }
}
