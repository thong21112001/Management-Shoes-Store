using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Sizes.Commands.UpdateSize
{
    public class UpdateSizeCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        [Display(Name = "Kích thước")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public string Value { get; set; } = string.Empty;

        [Display(Name = "Loại giày")]
        public string? Type { get; set; }
    }
}
