using MediatR;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;


        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.Repository<Product>();

            // Lấy sản phẩm cần xóa. Không cần Include vì EF Core sẽ xử lý cascade delete.
            var productToDelete = await productRepo.GetByIdAsync(request.Id, cancellationToken);

            if (productToDelete == null)
            {
                // Trả về false nếu không tìm thấy sản phẩm
                return false;
            }

            // Bắt đầu transaction để đảm bảo toàn vẹn
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Đánh dấu sản phẩm để xóa.
                // EF Core sẽ tự động tìm và xóa các bản ghi liên quan trong
                // ProductVariants và ProductCategories nhờ vào Cascade Delete.
                await productRepo.DeleteAsync(productToDelete, cancellationToken);

                // Lưu thay đổi và commit transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return true;
            }
            catch
            {
                // Nếu có lỗi, rollback lại tất cả thay đổi
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw; // Ném lại exception để tầng trên xử lý
            }
        }
    }
}
