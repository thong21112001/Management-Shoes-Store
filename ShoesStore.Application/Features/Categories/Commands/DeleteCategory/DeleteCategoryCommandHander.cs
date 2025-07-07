using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHander : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHander(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryDb = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id);

            if (categoryDb == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            await _unitOfWork.Repository<Category>().DeleteAsync(categoryDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
