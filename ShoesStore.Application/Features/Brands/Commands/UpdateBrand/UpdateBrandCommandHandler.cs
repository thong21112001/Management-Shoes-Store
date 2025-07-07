using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(request.Id, cancellationToken);

            if (brandDb == null)
            {
                throw new KeyNotFoundException($"Brand with ID {request.Id} not found.");
            }

            brandDb.Name = request.Name;
            brandDb.Description = request.Description;
            brandDb.LogoUrl = request.LogoUrl;
            brandDb.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Brand>().UpdateAsync(brandDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
