using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Features.Categories.Commands.DeleteCategory;
using ShoesStore.Application.Features.Categories.Queries.GetAllCates;
using ShoesStore.Application.Features.Categories.Queries.GetCatesByIdQuery;
using ShoesStore.Application.Features.Categories.Queries.Shared;

namespace ShoesStore.Web.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<CategoriesListDto> Categories { get; set; } = new();


        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            Categories = await _mediator.Send(new GetAllCatesQuery());
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var cateDto = await _mediator.Send(new GetCatesByIdQuery { Id = id });

            if (cateDto == null)
                return NotFound();

            return Partial("_DetailsPartial", cateDto);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var cateDto = await _mediator.Send(new GetCatesByIdQuery { Id = id });

            if (cateDto == null)
                return NotFound();

            return Partial("_DeletePartial", cateDto);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteCategoryAsync(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCategoryCommand { Id = id });

                return new JsonResult(new { success = true });
            }
            catch (Exception ex) // Bắt các lỗi có thể xảy ra từ lớp Application
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
