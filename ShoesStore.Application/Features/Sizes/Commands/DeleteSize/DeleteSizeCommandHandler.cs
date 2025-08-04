using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Sizes.Commands.DeleteSize
{
    public class DeleteSizeCommandHandler : IRequestHandler<DeleteSizeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSizeCommandHandler> _logger;

        public DeleteSizeCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSizeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<Unit> Handle(DeleteSizeCommand request, CancellationToken cancellationToken)
        {
            var sizeDb = await _unitOfWork.Repository<Size>().GetByIdAsync(request.Id);
            var prodVarRepo = _unitOfWork.Repository<ProductVariant>();

            var isHadProdVar = await prodVarRepo.GetQueryable()
                .AnyAsync(pv => pv.SizeId == request.Id, cancellationToken);

            if (isHadProdVar)
            {
                // Nếu đang được sử dụng, ném ra một ngoại lệ với thông báo rõ ràng.
                throw new InvalidOperationException("Không thể xóa size này vì đang có sản phẩm sử dụng.");
            }

            if (sizeDb == null)
            {
                throw new KeyNotFoundException($"Size với id {request.Id} không tồn tại.");
            }

            await _unitOfWork.Repository<Size>().DeleteAsync(sizeDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Xóa kích thước thành công với id: {Id}", request.Id);

            return Unit.Value;
        }
    }
}
