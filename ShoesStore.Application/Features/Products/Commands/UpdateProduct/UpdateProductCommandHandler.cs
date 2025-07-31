using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Lấy sản phẩm từ cơ sở dữ liệu theo Id
            var productRepo = _unitOfWork.Repository<Product>();

            // Lấy các bảng ghi có liên quan đến sản phẩm
            var productToUpdate = await productRepo
                                        .GetQueryable()
                                        .Include(p => p.ProductVariants)
                                        .Include(p => p.ProductCategories)
                                        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (productToUpdate == null)
            {
                // Xử lý trường hợp không tìm thấy sản phẩm
                throw new Exception($"Không tìm thấy sản phẩm với Id {request.Id}");
            }

            // Bắt đầu một transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // 2. Cập nhật thông tin chính của Product
                productToUpdate.Name = request.Name;
                productToUpdate.Description = request.Description;
                productToUpdate.BrandId = request.BrandId;
                productToUpdate.UpdatedAt = DateTime.UtcNow;
                if (request.MainImageUrl != null)
                {
                    productToUpdate.MainImageUrl = request.MainImageUrl;
                }

                // 3. Cập nhật Categories (Xóa cũ, thêm mới)
                productToUpdate.ProductCategories.Clear();
                foreach (var categoryId in request.CategoryIds)
                {
                    productToUpdate.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = productToUpdate.Id });
                }

                // 4. Xử lý các biến thể
                var submittedVariantIds = request.Variants.Select(v => v.Id).ToHashSet();
                var variantRepo = _unitOfWork.Repository<ProductVariant>();

                // 4a. Xóa các biến thể không còn trong danh sách gửi lên
                var variantsToDelete = productToUpdate.ProductVariants
                    .Where(v => !submittedVariantIds.Contains(v.Id)).ToList();
                if (variantsToDelete.Any())
                {
                    await variantRepo.DeleteRangeAsync(variantsToDelete, cancellationToken);
                }

                // 4b. Cập nhật hoặc Thêm mới các biến thể
                foreach (var variantDto in request.Variants)
                {
                    if (variantDto.Id > 0) // ID > 0 => Biến thể cũ -> Cập nhật
                    {
                        var existingVariant = productToUpdate.ProductVariants.FirstOrDefault(v => v.Id == variantDto.Id);

                        if (existingVariant != null)
                        {
                            existingVariant.ColorId = variantDto.ColorId;
                            existingVariant.SizeId = variantDto.SizeId;
                            existingVariant.Sku = $"{productToUpdate.Name.ToUpper()}-{variantDto.ColorId}-{variantDto.SizeId}";
                            existingVariant.Price = variantDto.Price;
                            existingVariant.StockQuantity = variantDto.StockQuantity;
                            existingVariant.UpdatedAt = DateTime.UtcNow;
                            if (variantDto.ImageUrl != null)
                            {
                                existingVariant.ImageUrl = variantDto.ImageUrl;
                            }

                            await variantRepo.UpdateAsync(existingVariant, cancellationToken);
                        }
                    }
                    else // ID = 0 => Biến thể mới -> Thêm
                    {
                        var newVariant = new ProductVariant
                        {
                            ProductId = request.Id,
                            ColorId = variantDto.ColorId,
                            SizeId = variantDto.SizeId,
                            Sku = $"{productToUpdate.Name.ToUpper()}-{variantDto.ColorId}-{variantDto.SizeId}",
                            Price = variantDto.Price,
                            StockQuantity = variantDto.StockQuantity,
                            ImageUrl = variantDto.ImageUrl,
                            CreatedAt = DateTime.UtcNow
                        };

                        await variantRepo.AddAsync(newVariant, cancellationToken);
                    }
                }

                // 5. Lưu tất cả thay đổi vào DB và commit transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Cập nhập sản phẩm thành công với Id: {ProductId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Lỗi khi cập nhập sản phẩm với Id: {ProductId}", request.Id);
                throw new ApplicationException("Lỗi khi cập nhập sản phẩm");
            }
        }
    }
}
