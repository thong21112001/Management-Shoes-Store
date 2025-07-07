using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Features.Brands.Commands.DeleteBrand;
using ShoesStore.Application.Features.Brands.Queries.GetAllBrands;
using ShoesStore.Application.Features.Brands.Queries.GetBrandByIdQuery;
using ShoesStore.Application.Features.Brands.Queries.Shared;

namespace ShoesStore.Web.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<BrandListDto> Brands { get; set; } = new();


        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            // Gửi Query để lấy toàn bộ danh sách Brands
            Brands = await _mediator.Send(new GetAllBrandsQuery());
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var brandDto = await _mediator.Send(new GetBrandByIdQuery { Id = id });

            if (brandDto == null)
                return NotFound();

            // Truyền DTO vào Partial View
            return Partial("_DetailsPartial", brandDto);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var brandDto = await _mediator.Send(new GetBrandByIdQuery { Id = id });

            if (brandDto == null)
                return NotFound();

            // Truyền DTO vào Partial View
            return Partial("_DeletePartial", brandDto);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteBrandAsync(int id)
        {
            try
            {
                // Tạo và gửi command, không cần logic kiểm tra tồn tại ở đây
                // vì logic đó đã nằm trong Command Handler
                await _mediator.Send(new DeleteBrandCommand { Id = id });

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