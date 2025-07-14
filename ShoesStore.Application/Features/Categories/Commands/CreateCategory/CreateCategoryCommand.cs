using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {

        [Required(ErrorMessage = "{0} không được để trống."), Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public byte[]? ImageUrl { get; set; }
    }
}
