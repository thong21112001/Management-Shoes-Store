using Microsoft.EntityFrameworkCore;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;
using ShoesStore.Infrastructure.Persistence.Data;

namespace ShoesStore.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Index categories by Id, Name, and Description
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // sử dụng AsNoTracking để tránh việc theo dõi các thay đổi của thực thể, giúp cải thiện hiệu suất khi chỉ cần đọc dữ liệu
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        // Get category by Id
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstAsync(c => c.Id == id);
        }

        // Add a new category
        public async Task<Category> AddAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = null;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Update an existing category
        public async Task UpdateAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete a category by Id
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}