using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Sizes.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Sizes.Queries.GetSizeByIdQuery
{
    public class GetSizeByIdQueryHandler : IRequestHandler<GetSizeByIdQuery, SizeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetSizeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<SizeDto> Handle(GetSizeByIdQuery request, CancellationToken cancellationToken)
        {
            var sizeDb = await _unitOfWork.Repository<Size>().GetByIdAsync(request.Id, cancellationToken);

            if (sizeDb == null)
            {
                throw new KeyNotFoundException($"Size with ID {request.Id} not found.");
            }

            return _mapper.Map<SizeDto>(sizeDb);
        }
    }
}
