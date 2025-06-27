namespace ShoesStore.Domain.Entities.Data;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int BrandId { get; set; }

    public byte[]? MainImageUrl { get; set; }

    public string? ImageRenderedUrl =>
            MainImageUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(MainImageUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
