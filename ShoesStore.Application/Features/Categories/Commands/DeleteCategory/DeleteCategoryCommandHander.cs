using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHander : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryCommandHander> _logger;

        public DeleteCategoryCommandHander(IUnitOfWork unitOfWork, ILogger<DeleteCategoryCommandHander> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryDb = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id);
            var productsRepo = _unitOfWork.Repository<Product>();

            var isHadproductsRepo = await productsRepo.GetQueryable()
                                    .AnyAsync(p => p.ProductCategories.Any(pc => pc.CategoryId == request.Id), cancellationToken);

            if (isHadproductsRepo)
            {
                // Nếu đang được sử dụng, ném ra một ngoại lệ với thông báo rõ ràng.
                throw new InvalidOperationException("Không thể xóa danh mục này vì đang có sản phẩm sử dụng.");
            }

            if (categoryDb == null)
            {
                throw new KeyNotFoundException($"Danh mục với id {request.Id} không tồn tại.");
            }

            await _unitOfWork.Repository<Category>().DeleteAsync(categoryDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Xóa danh mục giày thành công với Id: {Id}", request.Id);

            return Unit.Value;
        }
    }
}
