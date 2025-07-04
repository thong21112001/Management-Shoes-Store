using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Common.Models;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Brands
{
    public class CreateModel : PageModel
    {
        private readonly IBrandRepository _brandRepository;

        public CreateModel(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Brand Brand { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }


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
                    Brand.LogoUrl = memoryStream.ToArray();
                }
            }

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công"),
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            await _brandRepository.AddAsync(Brand);
            return RedirectToPage("./Index");
        }
    }
}
