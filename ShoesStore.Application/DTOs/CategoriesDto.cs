using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.DTOs
{
    public class CategoriesDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = null!;

        [Display(Name = "Thông tin")]
        public string? Description { get; set; }
    }
}
