using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Colors.Commands.UpdateColor;
using ShoesStore.Application.Features.Colors.Queries.GetColorByIdQuery;

namespace ShoesStore.Web.Pages.Colors
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdateColorCommand ColorCommand { get; set; } = new();


        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var colorDto = await _mediator.Send(new GetColorByIdQuery { Id = id.Value });

            if (colorDto == null)
            {
                return NotFound();
            }

            ColorCommand = new UpdateColorCommand
            {
                Id = colorDto.Id,
                Name = colorDto.Name,
                Hexcode = colorDto.HexCode
            };

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
                AlertMessage.Success("Cập nhật thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
