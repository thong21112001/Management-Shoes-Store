using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Categories.Commands.UpdateCategory;
using ShoesStore.Application.Features.Categories.Queries.GetCatesByIdQuery;

namespace ShoesStore.Web.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdateCategoryCommand CategoryCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }



        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var cateDb = await _mediator.Send(new GetCatesByIdQuery { Id = id.Value });

            if (cateDb == null)
            {
                return NotFound();
            }

            CategoryCommand.Id = cateDb.Id;
            CategoryCommand.Name = cateDb.Name;
            CategoryCommand.Description = cateDb.Description ?? string.Empty;
            CategoryCommand.ImageUrl = cateDb.ImageUrl;

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
                    CategoryCommand.ImageUrl = memoryStream.ToArray();
                }
            }

            await _mediator.Send(CategoryCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Cập nhật thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
