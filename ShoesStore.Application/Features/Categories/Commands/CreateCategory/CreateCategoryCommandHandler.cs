using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newCategory = new Category
                {
                    Name = request.Name,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                await _unitOfWork.Repository<Category>().AddAsync(newCategory, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Tạo mới danh mục giày thành công với tên: {Name}", request.Name);

                return newCategory.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới danh mục giày với tên: {Name}", request.Name);
                throw new ApplicationException("Lỗi khi tạo mới danh mục giày", ex);
            }
        }
    }
}
