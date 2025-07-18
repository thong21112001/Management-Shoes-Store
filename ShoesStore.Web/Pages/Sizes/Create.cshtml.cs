using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Sizes.Commands.CreateSize;

namespace ShoesStore.Web.Pages.Sizes
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public CreateSizeCommand SizeCommand { get; set; } = new();


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

            await _mediator.Send(SizeCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công")
            };

            return RedirectToPage("./Index");
        }
    }
}
