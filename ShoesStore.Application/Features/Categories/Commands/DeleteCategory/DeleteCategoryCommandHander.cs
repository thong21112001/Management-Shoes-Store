using MediatR;
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
            try
            {
                var categoryDb = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id);

                if (categoryDb == null)
                {
                    throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
                }

                await _unitOfWork.Repository<Category>().DeleteAsync(categoryDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Xóa danh mục giày thành công với Id: {Id}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa danh mục giày với Id: {Id}", request.Id);
                throw new ApplicationException("Lỗi khi xóa danh mục giày", ex);
            }
        }
    }
}
