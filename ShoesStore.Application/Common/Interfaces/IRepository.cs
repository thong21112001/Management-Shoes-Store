namespace ShoesStore.Application.Common.Interfaces
{
    // TEntity là một class (entity trong Domain) và có property Id
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        // THÊM MỚI: Cung cấp khả năng xóa nhiều bản ghi
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        // THÊM MỚI: Cung cấp khả năng truy vấn linh hoạt
        IQueryable<TEntity> GetQueryable();
    }
}
