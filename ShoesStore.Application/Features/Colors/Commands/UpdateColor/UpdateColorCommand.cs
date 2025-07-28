using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Colors.Commands.UpdateColor
{
    public class UpdateColorCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Tên màu")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Mã màu")]
        public string Hexcode { get; set; } = string.Empty;
    }
}
