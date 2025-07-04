using AutoMapper;
using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoriesDto>();
            CreateMap<Brand, BrandsDto>();
        }
    }
}
