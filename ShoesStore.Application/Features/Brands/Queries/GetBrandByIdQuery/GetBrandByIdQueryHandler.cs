using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Brands.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Queries.GetBrandByIdQuery
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBrandByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brandDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(request.Id, cancellationToken);

            if (brandDb == null)
            {
                throw new KeyNotFoundException($"Brand with ID {request.Id} not found.");
            }

            return _mapper.Map<BrandDto>(brandDb);
        }
    }
}
