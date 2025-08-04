using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Colors.Commands.DeleteColor
{
    public class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteColorCommandHandler> _logger;

        public DeleteColorCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteColorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
        {
            var colorDb = await _unitOfWork.Repository<Color>().GetByIdAsync(request.Id);
            var prodVarRepo = _unitOfWork.Repository<ProductVariant>();

            var isHadProdVar = await prodVarRepo.GetQueryable()
                .AnyAsync(pv => pv.ColorId == request.Id, cancellationToken);

            if (isHadProdVar)
            {
                // Nếu đang được sử dụng, ném ra một ngoại lệ với thông báo rõ ràng.
                throw new InvalidOperationException("Không thể xóa màu này vì đang có sản phẩm sử dụng.");
            }

            if (colorDb == null)
            {
                throw new KeyNotFoundException($"Màu với id {request.Id} không tồn tại.");
            }

            await _unitOfWork.Repository<Color>().DeleteAsync(colorDb, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Xóa màu thành công với Id: {ColorId}", request.Id);

            return Unit.Value;
        }
    }
}
