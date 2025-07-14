using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Brands.Commands.UpdateBrand;
using ShoesStore.Application.Features.Brands.Queries.GetBrandByIdQuery;

namespace ShoesStore.Web.Pages.Brands
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdateBrandCommand BrandCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var brandDto = await _mediator.Send(new GetBrandByIdQuery { Id = id.Value });

            if (brandDto == null)
            {
                return NotFound();
            }

            BrandCommand = new UpdateBrandCommand
            {
                Id = brandDto.Id,
                Name = brandDto.Name,
                Description = brandDto.Description ?? string.Empty,
                LogoUrl = brandDto.LogoUrl
            };

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

            //Gửi Command đã được bind từ form đi để xử lý cập nhật
            await _mediator.Send(BrandCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Cập nhật thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
