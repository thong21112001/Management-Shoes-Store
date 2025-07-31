using MediatR;
using ShoesStore.Application.Features.Products.Queries.Shared.Product;

namespace ShoesStore.Application.Features.Products.Queries.GetProductByIdQuery
{
    public class GetProductByIdQuery : IRequest<ProductDetailsDto?>
    {
        public int Id { get; set; }
    }
}
