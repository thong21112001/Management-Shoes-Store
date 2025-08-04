using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var brandDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(request.Id);
            var productRepo = _unitOfWork.Repository<Product>();

            // 1. Kiểm tra xem brand có sản phẩm nào không
            var isBrandHasProducts = await productRepo.GetQueryable().AnyAsync(p => p.BrandId == request.Id, cancellationToken);

            if (isBrandHasProducts)
            {
                // Nếu đang được sử dụng, ném ra một ngoại lệ với thông báo rõ ràng.
                // Khối try-catch trong PageModel sẽ bắt lỗi này.
                throw new InvalidOperationException("Không thể xóa thương hiệu này vì đang có sản phẩm sử dụng.");
            }

            if (brandDb == null)
            {
                throw new KeyNotFoundException($"Brand với id {request.Id} không tồn tại.");
            }

            await _unitOfWork.Repository<Brand>().DeleteAsync(brandDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Xóa brand thành công với Id: {BrandId}", request.Id);

            return Unit.Value;
        }
    }
}
