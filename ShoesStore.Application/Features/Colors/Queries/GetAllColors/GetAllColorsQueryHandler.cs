using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Colors.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Colors.Queries.GetAllColors
{
    public class GetAllColorsQueryHandler : IRequestHandler<GetAllColorsQuery, List<ColorListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllColorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<ColorListDto>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            var colors = await _unitOfWork.Repository<Color>().GetAllAsync(cancellationToken);

            return _mapper.Map<List<ColorListDto>>(colors);
        }
    }
}
