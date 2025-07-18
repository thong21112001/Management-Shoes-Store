using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Sizes.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Sizes.Queries.GetAllSizes
{
    public class GetAllSizesQueryHandler : IRequestHandler<GetAllSizesQuery, List<SizesListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSizesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SizesListDto>> Handle(GetAllSizesQuery request, CancellationToken cancellationToken)
        {
            var sizes = await _unitOfWork.Repository<Size>().GetAllAsync(cancellationToken);

            return _mapper.Map<List<SizesListDto>>(sizes);
        }
    }
}
