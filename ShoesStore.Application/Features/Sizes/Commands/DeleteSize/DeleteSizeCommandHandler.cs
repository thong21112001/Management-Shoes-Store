using MediatR;
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
            try
            {
                var sizeDb = await _unitOfWork.Repository<Size>().GetByIdAsync(request.Id);

                if (sizeDb == null)
                {
                    throw new KeyNotFoundException($"Size with ID {request.Id} not found.");
                }

                await _unitOfWork.Repository<Size>().DeleteAsync(sizeDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Xóa kích thước thành công với id: {Id}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa kích thước với id: {Id}", request.Id);
                throw new ApplicationException("Lỗi khi xóa kích thước", ex);
            }
        }
    }
}
