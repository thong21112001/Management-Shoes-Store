namespace ShoesStore.Domain.Entities.Data;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? LogoUrl { get; set; }

    public string? ImageRenderedUrl =>
            LogoUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(LogoUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
