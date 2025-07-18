using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Sizes.Queries.Shared
{
    public class SizeDto
    {
        public int Id { get; set; }

        [Display(Name = "Kích cỡ")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Value { get; set; } = string.Empty;

        [Display(Name = "Loại giày")]
        public string? Type { get; set; }

        [Display(Name = "Ngày tạo"), DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhập"), DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }
    }
}
