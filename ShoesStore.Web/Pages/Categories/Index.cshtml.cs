using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;

        public IndexModel(ICategoryRepository category)
        {
            categoryRepository = category;
        }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public async Task OnGetAsync()
        {
            Categories = await categoryRepository.GetAllAsync();
        }
    }
}
