using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Features.Colors.Commands.DeleteColor;
using ShoesStore.Application.Features.Colors.Queries.GetAllColors;
using ShoesStore.Application.Features.Colors.Queries.GetColorByIdQuery;
using ShoesStore.Application.Features.Colors.Queries.Shared;

namespace ShoesStore.Web.Pages.Colors
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<ColorListDto> Colors { get; set; } = new();


        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            Colors = await _mediator.Send(new GetAllColorsQuery());
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var colorDto = await _mediator.Send(new GetColorByIdQuery { Id = id });

            if (colorDto == null)
                return NotFound();

            return Partial("_DetailsPartial", colorDto);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var colorDto = await _mediator.Send(new GetColorByIdQuery { Id = id });

            if (colorDto == null)
                return NotFound();

            return Partial("_DeletePartial", colorDto);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteColorAsync(int id)
        {
            try
            {
                await _mediator.Send(new DeleteColorCommand { Id = id });

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
