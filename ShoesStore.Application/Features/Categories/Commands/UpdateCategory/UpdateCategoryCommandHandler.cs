using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cateDb = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, cancellationToken);

                if (cateDb == null)
                {
                    throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
                }

                cateDb.Name = request.Name;
                cateDb.Description = request.Description;
                cateDb.ImageUrl = request.ImageUrl;
                cateDb.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Category>().UpdateAsync(cateDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Cập nhật danh mục giày thành công với Id: {Id}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật danh mục giày với Id: {Id}", request.Id);
                throw new ApplicationException("Lỗi khi cập nhật danh mục giày", ex);
            }
        }
    }
}
