using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Sizes.Commands.CreateSize
{
    public class CreateSizeCommand : IRequest<int>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Kích cỡ")]
        public string Value { get; set; } = string.Empty;

        [Display(Name = "Loại giày")]
        public string? Type { get; set; }
    }
}
