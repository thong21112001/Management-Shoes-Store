using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Tạo mới Brand
        public async Task<int> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
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

            return newBrand.Id;
        }
    }
}
