using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Brands.Commands.CreateBrand;

namespace ShoesStore.Web.Pages.Brands
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public CreateBrandCommand BrandCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public IActionResult OnGet()
        {
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);
                    BrandCommand.LogoUrl = memoryStream.ToArray();
                }
            }

            // Gửi command đến MediatR để xử lý thêm mới
            await _mediator.Send(BrandCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
