using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Products.Queries.Shared.Product;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Products.Queries.GetProductByIdQuery
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto?>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ProductDetailsDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetQueryable()
                .AsNoTracking() // Tối ưu cho truy vấn chỉ đọc
                .Where(p => p.Id == request.Id)
                // Dùng .Select() (projection) để định hình dữ liệu ngay tại database
                .Select(p => new ProductDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BrandName = p.Brand.Name,
                    MainImageUrl = p.MainImageUrl,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Categories = string.Join(", ", p.ProductCategories.Select(pc => pc.Category.Name)),
                    AnyProductVar = p.ProductVariants.Count()
                    //Không nhất thiết phải lấy hết các biến thể:
                    //Variants = p.ProductVariants.Select(v => new ProductVarDetailsDto
                    //{
                    //    Id = v.Id,
                    //    ColorName = v.Color.Name,
                    //    ColorHexCode = v.Color.HexCode ?? string.Empty,
                    //    SizeValue = v.Size.Value,
                    //    Sku = v.Sku,
                    //    Price = v.Price,
                    //    StockQuantity = v.StockQuantity,
                    //    ImageUrl = v.ImageUrl
                    //}).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return product;
        }
    }
}
