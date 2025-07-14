using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBrandCommandHandler> _logger;

        public UpdateBrandCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateBrandCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            try
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

                _logger.LogInformation("Cập nhập brand thành công với Id: {BrandId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhập brand với Id: {BrandId}", request.Id);
                throw new ApplicationException("Lỗi khi cập nhập brand");
            }
        }
    }
}
