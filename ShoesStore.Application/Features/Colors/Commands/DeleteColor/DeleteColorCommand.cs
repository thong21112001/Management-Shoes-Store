using MediatR;

namespace ShoesStore.Application.Features.Colors.Commands.DeleteColor
{
    public class DeleteColorCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
