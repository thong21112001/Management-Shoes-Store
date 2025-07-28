using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Colors.Queries.Shared
{
    public class ColorListDto
    {
        public int Id { get; set; }

        [Display(Name = "Tên màu")]
        public string? Name { get; set; }

        [Display(Name = "Mã màu")]
        public string? HexCode { get; set; }
    }
}
