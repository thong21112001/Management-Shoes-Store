using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBrandCommandHandler> _logger;

        public CreateBrandCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBrandCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //Tạo mới Brand
        public async Task<int> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newBrand = new Brand
                {
                    Name = request.Name,
                    Description = request.Description,
                    LogoUrl = request.LogoUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                await _unitOfWork.Repository<Brand>().AddAsync(newBrand, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Thêm brand mới thành công: {BrandName}", request.Name);

                return newBrand.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm brand mới: {BrandName}", request.Name);
                throw new ApplicationException("Lỗi khi thêm brand mới");
            }
        }
    }
}
