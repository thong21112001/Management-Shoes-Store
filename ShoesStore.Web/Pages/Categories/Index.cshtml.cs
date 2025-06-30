using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.DTOs;

namespace ShoesStore.Web.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository category)
        {
            _categoryRepository = category;
        }

        public IList<CategoriesDto> Categories { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_categoryRepository != null)
            {
                Categories = await _categoryRepository.GetAllAsync();
            }
            else
            {
                Categories = new List<CategoriesDto>();
            }
        }
    }
}
