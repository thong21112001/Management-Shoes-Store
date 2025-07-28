using MediatR;
using ShoesStore.Application.Features.Colors.Queries.Shared;

namespace ShoesStore.Application.Features.Colors.Queries.GetColorByIdQuery
{
    public class GetColorByIdQuery : IRequest<ColorDto>
    {
        public int Id { get; set; }
    }
}
