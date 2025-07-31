using Microsoft.EntityFrameworkCore;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Infrastructure.Persistence.Data;

namespace ShoesStore.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            // Chưa SaveChanges() ở đây để tuân thủ Unit of Work
            return entity;
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        // THÊM MỚI: Triển khai DeleteRangeAsync
        public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        // THÊM MỚI: Triển khai GetQueryable
        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>();
        }
    }
}
