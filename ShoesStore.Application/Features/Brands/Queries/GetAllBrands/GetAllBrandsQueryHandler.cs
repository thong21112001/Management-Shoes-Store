using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Brands.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, List<BrandListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBrandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BrandListDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _unitOfWork.Repository<Brand>().GetAllAsync(cancellationToken);

            return _mapper.Map<List<BrandListDto>>(brands);
        }
    }
}
