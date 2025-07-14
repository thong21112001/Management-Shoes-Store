using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Categories.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Queries.GetCatesByIdQuery
{
    public class GetCatesByIdQueryHandler : IRequestHandler<GetCatesByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetCatesByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCatesByIdQuery request, CancellationToken cancellationToken)
        {
            var categoryDb = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, cancellationToken);

            if (categoryDb == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            return _mapper.Map<CategoryDto>(categoryDb);
        }
    }
}
