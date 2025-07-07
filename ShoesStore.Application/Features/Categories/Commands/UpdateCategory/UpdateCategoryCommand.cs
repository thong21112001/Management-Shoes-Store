using MediatR;

namespace ShoesStore.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public byte[]? ImageUrl { get; set; }

        public string? ImageRenderedUrl =>
            ImageUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(ImageUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";
    }
}
