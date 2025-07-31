using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Brands.Queries.GetAllBrands;
using ShoesStore.Application.Features.Categories.Queries.GetAllCates;
using ShoesStore.Application.Features.Colors.Queries.GetAllColors;
using ShoesStore.Application.Features.Products.Commands.CreateProduct;
using ShoesStore.Application.Features.Sizes.Queries.GetAllSizes;

namespace ShoesStore.Web.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public CreateProductCommand ProductCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? MainImageUpload { get; set; }

        // Dữ liệu cho các dropdowns
        public SelectList Brands { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Categories { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Colors { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Sizes { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());



        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task OnGetAsync()
        {
            // Lấy dữ liệu để đổ vào các select list
            var brands = await _mediator.Send(new GetAllBrandsQuery());
            var categories = await _mediator.Send(new GetAllCatesQuery());
            var colors = await _mediator.Send(new GetAllColorsQuery());
            var sizes = await _mediator.Send(new GetAllSizesQuery());

            Brands = new SelectList(brands, "Id", "Name");
            Categories = new SelectList(categories, "Id", "Name");
            Colors = new SelectList(colors, "Id", "Name");
            Sizes = new SelectList(sizes, "Id", "Value");
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Xử lý ảnh chính
            if (MainImageUpload != null)
            {
                using var memoryStream = new MemoryStream();
                await MainImageUpload.CopyToAsync(memoryStream);
                ProductCommand.MainImageUrl = memoryStream.ToArray();
            }

            // Xử lý ảnh cho từng biến thể một cách an toàn
            foreach (var variant in ProductCommand.Variants)
            {
                if (variant.ImageFile != null && variant.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await variant.ImageFile.CopyToAsync(memoryStream);
                    variant.ImageUrl = memoryStream.ToArray();
                }
            }

            // Gửi command đến handler để xử lý logic
            await _mediator.Send(ProductCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
