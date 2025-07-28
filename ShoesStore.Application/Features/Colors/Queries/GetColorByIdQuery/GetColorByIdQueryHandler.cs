using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Colors.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Colors.Queries.GetColorByIdQuery
{
    public class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, ColorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetColorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ColorDto> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
        {
            var colorDb = await _unitOfWork.Repository<Color>().GetByIdAsync(request.Id, cancellationToken);

            if (colorDb == null)
            {
                throw new KeyNotFoundException($"Color with ID {request.Id} not found.");
            }

            return _mapper.Map<ColorDto>(colorDb);
        }
    }
}
