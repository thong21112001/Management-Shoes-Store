using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Colors.Queries.Shared
{
    public class ColorDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên màu")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mã màu")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string HexCode { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo"), DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhập"), DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }
    }
}
