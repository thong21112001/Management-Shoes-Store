using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
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

            return Unit.Value;
        }
    }
}
