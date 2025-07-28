using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Colors.Commands.UpdateColor
{
    public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateColorCommandHandler> _logger;

        public UpdateColorCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateColorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var colorDb = await _unitOfWork.Repository<Color>().GetByIdAsync(request.Id, cancellationToken);

                if (colorDb == null)
                {
                    throw new KeyNotFoundException($"Color with ID {request.Id} not found.");
                }


                colorDb.Name = request.Name;
                colorDb.HexCode = request.Hexcode;
                colorDb.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Color>().UpdateAsync(colorDb, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Cập nhập màu thành công với Id: {ColorId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhập màu với Id: {ColorId}", request.Id);
                throw new ApplicationException("Lỗi khi cập nhập màu");
            }
        }
    }
}
