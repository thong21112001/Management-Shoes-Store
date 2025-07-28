using MediatR;
using ShoesStore.Application.Features.Colors.Queries.Shared;

namespace ShoesStore.Application.Features.Colors.Queries.GetAllColors
{
    public class GetAllColorsQuery : IRequest<List<ColorListDto>> { }
}
