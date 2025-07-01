using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Common.Models;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = new();

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
                    Category.ImageUrl = memoryStream.ToArray();
                }
            }

            TempData.Clear(); // nếu cần reset
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công"),
                AlertMessage.Info("Bạn có thể tiếp tục thêm")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            await _categoryRepository.AddAsync(Category);
            return RedirectToPage("./Index");
        }
    }
}
