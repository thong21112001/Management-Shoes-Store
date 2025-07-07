using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brandDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(request.Id);

            if (brandDb == null)
            {
                throw new KeyNotFoundException($"Brand with ID {request.Id} not found.");
            }

            await _unitOfWork.Repository<Brand>().DeleteAsync(brandDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
