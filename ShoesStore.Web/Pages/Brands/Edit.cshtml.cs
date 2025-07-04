using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Brands
{
    public class EditModel : PageModel
    {
        private readonly IBrandRepository _brandRepository;

        public EditModel(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [BindProperty]
        public Brand Brand { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var br = await _brandRepository.GetBrandByIdAsync(id.Value);
            if (br == null) return NotFound();

            Brand = br;
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
                    Brand.LogoUrl = memoryStream.ToArray();
                }
            }

            await _brandRepository.UpdateAsync(Brand);

            return RedirectToPage("./Index");
        }
    }
}
