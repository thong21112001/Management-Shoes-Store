using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Categories.Commands.CreateCategory;

namespace ShoesStore.Web.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public CreateCategoryCommand CategoryCommand { get; set; } = new();

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
                    CategoryCommand.ImageUrl = memoryStream.ToArray();
                }
            }

            await _mediator.Send(CategoryCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công"),
                AlertMessage.Info("Bạn có thể tiếp tục thêm")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);


            return RedirectToPage("./Index");
        }
    }
}
