using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Sizes.Commands.UpdateSize
{
    public class UpdateSizeCommandHandler : IRequestHandler<UpdateSizeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateSizeCommandHandler> _logger;

        public UpdateSizeCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSizeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sizeDb = await _unitOfWork.Repository<Size>().GetByIdAsync(request.Id, cancellationToken);

                if (sizeDb == null)
                {
                    throw new KeyNotFoundException($"Size with ID {request.Id} not found.");
                }

                sizeDb.Value = request.Value;
                sizeDb.Type = request.Type;
                sizeDb.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Size>().UpdateAsync(sizeDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Cập nhập kích thước giày thành công với Id: {Id}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật kích thước giày với Id: {Id}", request.Id);
                throw new ApplicationException("Lỗi khi cập nhật kích thước giày", ex);
            }
        }
    }
}
