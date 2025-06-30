using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<IList<CategoriesDto>> GetAllAsync();
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
