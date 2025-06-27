namespace ShoesStore.Domain.Entities.Data;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? ImageUrl { get; set; }

    public string? ImageRenderedUrl =>
            ImageUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(ImageUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
