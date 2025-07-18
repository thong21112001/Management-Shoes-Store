using MediatR;

namespace ShoesStore.Application.Features.Sizes.Commands.DeleteSize
{
    public class DeleteSizeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
