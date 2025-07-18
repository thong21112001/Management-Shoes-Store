using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Sizes.Queries.Shared
{
    public class SizesListDto
    {
        public int Id { get; set; }

        [Display(Name = "Kích cỡ")]
        public string? Value { get; set; }

        [Display(Name = "Loại giày")]
        public string? Type { get; set; }
    }
}
