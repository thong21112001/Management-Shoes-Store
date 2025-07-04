using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Domain.Entities.Data;

public partial class Brand
{
    public int Id { get; set; }

    [Display(Name = "Tên thương hiệu")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; } = null!;

    [Display(Name = "Thông tin")]
    public string? Description { get; set; }

    [Display(Name = "Logo")]
    public byte[]? LogoUrl { get; set; }

    [Display(Name = "Logo")]
    public string? ImageRenderedUrl =>
            LogoUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(LogoUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";

    [Display(Name = "Ngày tạo"), DataType(DataType.Date)]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhập"), DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
