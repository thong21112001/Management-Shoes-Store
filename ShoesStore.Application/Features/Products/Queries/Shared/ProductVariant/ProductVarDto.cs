using Microsoft.AspNetCore.Http;

namespace ShoesStore.Application.Features.Products.Queries.Shared.ProductVariant
{
    public class ProductVarDto
    {
        public int Id { get; set; } // Id > 0 là biến thể cũ, Id = 0 là biến thể mới

        public int ProductId { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public byte[]? ImageUrl { get; set; }

        //Sử dụng để bắt file ảnh khi tạo biến thể sản phẩm lúc upload lên
        public IFormFile? ImageFile { get; set; }
    }
}
