using MediatR;
using ShoesStore.Application.Features.Brands.Queries.Shared;

namespace ShoesStore.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQuery : IRequest<List<BrandListDto>> { }
}