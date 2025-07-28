using MediatR;
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
            try
            {
                var colorDb = await _unitOfWork.Repository<Color>().GetByIdAsync(request.Id);

                if (colorDb == null)
                {
                    throw new KeyNotFoundException($"Color with ID {request.Id} not found.");
                }

                await _unitOfWork.Repository<Color>().DeleteAsync(colorDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Xóa màu thành công với Id: {ColorId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa màu với Id: {ColorId}", request.Id);
                throw new ApplicationException("Lỗi khi xóa màu");
            }
        }
    }
}
