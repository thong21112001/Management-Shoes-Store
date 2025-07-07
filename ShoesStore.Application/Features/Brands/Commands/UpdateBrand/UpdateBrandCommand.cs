using MediatR;

namespace ShoesStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommand : IRequest<Unit> //Unit là không trả về gì cả
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public byte[]? LogoUrl { get; set; }

        public string? ImageRenderedUrl =>
            LogoUrl != null
            ? $"data:image;base64,{Convert.ToBase64String(LogoUrl)}"
            : "https://placehold.co/650x350/E8F5E8/7cb342?text=Featured+Article";
    }
}
