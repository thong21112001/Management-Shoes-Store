using AutoMapper;
using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Features.Categories.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Categories.Queries.GetAllCates
{
    public class GetAllCatesQueryHandler : IRequestHandler<GetAllCatesQuery, List<CategoriesListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCatesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoriesListDto>> Handle(GetAllCatesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync(cancellationToken);

            return _mapper.Map<List<CategoriesListDto>>(categories);
        }
    }
}
