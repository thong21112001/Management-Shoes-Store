using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Common.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<IList<BrandsDto>> GetAllBrandsAsync();
        Task<Brand> AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(int id);
    }
}
