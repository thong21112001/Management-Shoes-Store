using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBrandCommandHandler> _logger;

        public DeleteBrandCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteBrandCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var brandDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(request.Id);

                if (brandDb == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {request.Id} not found.");
                }

                await _unitOfWork.Repository<Brand>().DeleteAsync(brandDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Xóa brand thành công với Id: {BrandId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa brand với Id: {BrandId}", request.Id);
                throw new ApplicationException("Lỗi khi xóa brand");
            }
        }
    }
}
