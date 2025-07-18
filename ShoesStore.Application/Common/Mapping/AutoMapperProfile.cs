using AutoMapper;
using ShoesStore.Application.Features.Brands.Queries.Shared;
using ShoesStore.Application.Features.Categories.Queries.Shared;
using ShoesStore.Application.Features.Sizes.Queries.Shared;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Brand
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, BrandListDto>();// dành cho list index

            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoriesListDto>();// dành cho list index

            // Size
            CreateMap<Size, SizeDto>().ReverseMap();
            CreateMap<Size, SizesListDto>();// dành cho list index
        }
    }
}
