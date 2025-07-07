using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Categories.Queries.Shared
{
    public class CategoriesListDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;
    }
}
