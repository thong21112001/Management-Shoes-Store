using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Colors.Commands.CreateColor;

namespace ShoesStore.Web.Pages.Colors
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public CreateColorCommand ColorCommand { get; set; } = new();


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

            await _mediator.Send(ColorCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Tạo mới thành công")
            };

            return RedirectToPage("./Index");
        }
    }
}
