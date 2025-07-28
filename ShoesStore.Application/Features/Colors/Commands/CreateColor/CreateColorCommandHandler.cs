using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Colors.Commands.CreateColor
{
    public class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateColorCommandHandler> _logger;


        public CreateColorCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateColorCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<int> Handle(CreateColorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newColor = new Color
                {
                    Name = request.Name,
                    HexCode = request.Hexcode,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                await _unitOfWork.Repository<Color>().AddAsync(newColor, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Thêm màu mới thành công: {ColorName}", request.Name);

                return newColor.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm màu mới: {ColorName}", request.Name);
                throw new ApplicationException("Lỗi khi thêm màu mới");
            }
        }
    }
}
