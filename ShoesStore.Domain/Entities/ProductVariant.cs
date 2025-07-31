namespace ShoesStore.Domain.Entities.Data;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ColorId { get; set; }

    public int SizeId { get; set; }

    public string Sku { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public byte[]? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
