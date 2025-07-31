using MediatR;
using ShoesStore.Application.Features.Products.Queries.Shared.Product;

namespace ShoesStore.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductListDto>> { }
}