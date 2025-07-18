using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Features.Sizes.Commands.DeleteSize;
using ShoesStore.Application.Features.Sizes.Queries.GetAllSizes;
using ShoesStore.Application.Features.Sizes.Queries.GetSizeByIdQuery;
using ShoesStore.Application.Features.Sizes.Queries.Shared;

namespace ShoesStore.Web.Pages.Sizes
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<SizesListDto> Sizes { get; set; } = new();


        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            Sizes = await _mediator.Send(new GetAllSizesQuery());
        }


        #region Phần Chi tiết và Xóa
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var sizeDto = await _mediator.Send(new GetSizeByIdQuery { Id = id });

            if (sizeDto == null)
                return NotFound();

            return Partial("_DetailsPartial", sizeDto);
        }

        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var sizeDto = await _mediator.Send(new GetSizeByIdQuery { Id = id });

            if (sizeDto == null)
                return NotFound();

            return Partial("_DeletePartial", sizeDto);
        }

        public async Task<IActionResult> OnPostDeleteSizeAsync(int id)
        {
            try
            {
                await _mediator.Send(new DeleteSizeCommand { Id = id });

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
