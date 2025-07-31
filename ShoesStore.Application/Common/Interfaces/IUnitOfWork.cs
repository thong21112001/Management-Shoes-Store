namespace ShoesStore.Application.Common.Interfaces
{
    //Triển khai IUnitOfWork để quản lý các thao tác với cơ sở dữ liệu
    // sử dụng IRepository để truy cập các thực thể
    public interface IUnitOfWork : IDisposable
    {
        // Lấy ra một repository cho một entity cụ thể
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        // Lưu các thay đổi vào cơ sở dữ liệu
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // THÊM MỚI: Các phương thức quản lý Transaction
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
