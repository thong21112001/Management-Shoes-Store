using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Brands.Queries.Shared
{
    public class BrandListDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên thương hiệu")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;
    }
}
