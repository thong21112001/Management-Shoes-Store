using MediatR;
using Microsoft.Extensions.Logging;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Sizes.Commands.CreateSize
{
    public class CreateSizeCommandHandler : IRequestHandler<CreateSizeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSizeCommandHandler> _logger;


        public CreateSizeCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateSizeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<int> Handle(CreateSizeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newSize = new Size
                {
                    Value = request.Value,
                    Type = request.Type,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                await _unitOfWork.Repository<Size>().AddAsync(newSize, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Tạo mới kích thước giày thành công: {Value}", request.Value);

                return newSize.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới kích thước với tên: {Value}", request.Value);
                throw new ApplicationException("Lỗi khi tạo mới kích thước: ", ex);
            }
        }
    }
}
