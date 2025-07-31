using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;


        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }



        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Tạo product từ request
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    BrandId = request.BrandId,
                    MainImageUrl = request.MainImageUrl,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                // 2. Thêm các category vào product
                if (request.CategoryIds != null && request.CategoryIds.Any())
                {
                    foreach (var categoryId in request.CategoryIds)
                    {
                        product.ProductCategories.Add(new ProductCategory
                        {
                            CategoryId = categoryId
                        });
                    }
                }

                // 3. Thêm product vào database
                await _unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);

                // 4. Hoàn thành lưu Product để sinh ra Product.Id
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Tạo sản phẩm thành công với Id: {ProductId}", product.Id);

                // 5. Thêm các biến thể vào product
                foreach (var variant in request.Variants)
                {
                    var productVariant = new ProductVariant
                    {
                        ProductId = product.Id, // Lấy Id từ product vừa được lưu
                        ColorId = variant.ColorId,
                        SizeId = variant.SizeId,
                        Price = variant.Price,
                        StockQuantity = variant.StockQuantity,
                        Sku = $"{product.Name.ToUpper()}-{variant.ColorId}-{variant.SizeId}", // Tự động tạo SKU
                        ImageUrl = variant.ImageUrl,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.Repository<ProductVariant>().AddAsync(productVariant);
                }

                // 6. Lưu các biến thể vào database
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Thêm các biến thể cho sản phẩm Id: {ProductId} thành công", product.Id);

                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo sản phẩm");
                throw new ApplicationException("Lỗi khi tạo sản phẩm", ex);
            }
        }
    }
}
