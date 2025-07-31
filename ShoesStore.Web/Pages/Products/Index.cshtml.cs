using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Features.Products.Commands.DeleteProduct;
using ShoesStore.Application.Features.Products.Queries.GetAllProducts;
using ShoesStore.Application.Features.Products.Queries.GetProductByIdQuery;
using ShoesStore.Application.Features.Products.Queries.Shared.Product;

namespace ShoesStore.Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<ProductListDto> Products { get; set; } = new();


        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            Products = await _mediator.Send(new GetAllProductsQuery());
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var prodDto = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (prodDto == null)
                return NotFound();

            // Truyền DTO vào Partial View
            return Partial("_DetailsPartial", prodDto);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var prodDto = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (prodDto == null)
                return NotFound();

            return Partial("_DeletePartial", prodDto);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteProductAsync(int id)
        {
            try
            {
                await _mediator.Send(new DeleteProductCommand { Id = id });

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
