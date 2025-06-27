namespace ShoesStore.Domain.Entities.Data;

public partial class Size
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
