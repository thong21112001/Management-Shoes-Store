using MediatR;
using ShoesStore.Application.Features.Brands.Queries.Shared;

namespace ShoesStore.Application.Features.Brands.Queries.GetBrandByIdQuery
{
    public class GetBrandByIdQuery : IRequest<BrandDto>
    {
        public int Id { get; set; }
    }
}