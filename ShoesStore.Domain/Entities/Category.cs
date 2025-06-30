using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Domain.Entities.Data;

public partial class Category
{
    public int Id { get; set; }

    [Display(Name = "Tên danh mục")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; } = null!;

    [Display(Name = "Thông tin")]
    public string? Description { get; set; }

    [Display(Name = "Hình ảnh")]
    public byte[]? ImageUrl { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? ImageRenderedUrl =>
            ImageUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(ImageUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";


    [Display(Name = "Ngày tạo")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhập")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
