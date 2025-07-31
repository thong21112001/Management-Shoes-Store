using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Products.Queries.Shared.Product;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductListDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.Repository<Product>();

            var products = await productRepo.GetQueryable()
                .AsNoTracking()
                .Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand.Name,
                    CategoryIds = string.Join(", ", p.ProductCategories.Select(pc => pc.Category.Name)),
                    AnyProductVariant = p.ProductVariants.Count()
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
