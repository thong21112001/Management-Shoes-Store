using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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

            return newCategory.Id;
        }
    }
}
