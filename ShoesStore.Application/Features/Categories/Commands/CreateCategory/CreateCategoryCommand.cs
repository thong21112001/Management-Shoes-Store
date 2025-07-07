using MediatR;

namespace ShoesStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? ImageUrl { get; set; }
    }
}
