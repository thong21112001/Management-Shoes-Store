using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public EditModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null) return NotFound();

            Category = category;
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
                    Category.ImageUrl = memoryStream.ToArray();
                }
            }

            await _categoryRepository.UpdateAsync(Category);

            return RedirectToPage("./Index");
        }
    }
}
