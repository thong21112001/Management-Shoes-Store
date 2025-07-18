using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Sizes.Commands.UpdateSize;
using ShoesStore.Application.Features.Sizes.Queries.GetSizeByIdQuery;

namespace ShoesStore.Web.Pages.Sizes
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdateSizeCommand SizeCommand { get; set; } = new();


        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeDb = await _mediator.Send(new GetSizeByIdQuery { Id = id.Value });

            if (sizeDb == null)
            {
                return NotFound();
            }

            SizeCommand.Id = sizeDb.Id;
            SizeCommand.Value = sizeDb.Value;
            SizeCommand.Type = sizeDb.Type;

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
                AlertMessage.Success("Cập nhật thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }
    }
}
