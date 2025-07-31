using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.Common.Models;
using ShoesStore.Application.Features.Brands.Queries.GetAllBrands;
using ShoesStore.Application.Features.Categories.Queries.GetAllCates;
using ShoesStore.Application.Features.Colors.Queries.GetAllColors;
using ShoesStore.Application.Features.Products.Commands.UpdateProduct;
using ShoesStore.Application.Features.Products.Queries.Shared.ProductVariant;
using ShoesStore.Application.Features.Sizes.Queries.GetAllSizes;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork; // Dùng để đọc dữ liệu

        [BindProperty]
        public UpdateProductCommand ProductCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? MainImageUpload { get; set; }

        // Dữ liệu cho các dropdowns
        public SelectList Brands { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Categories { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Colors { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Sizes { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());


        public EditModel(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Lấy dữ liệu sản phẩm hiện tại
            var product = await _unitOfWork.Repository<Product>()
                .GetQueryable()
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductVariants)
                .AsNoTracking() // Dùng AsNoTracking để không bị lỗi tracking khi update
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductCommand = new UpdateProductCommand
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BrandId = product.BrandId,
                MainImageUrl = product.MainImageUrl,
                CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                Variants = product.ProductVariants.Select(v => new ProductVarDto
                {
                    Id = v.Id,
                    ProductId = product.Id,
                    ColorId = v.ColorId,
                    SizeId = v.SizeId,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    ImageUrl = v.ImageUrl
                }).ToList()
            };

            await LoadDropdownDataAsync();
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownDataAsync();
                return Page();
            }

            // Xử lý ảnh chính
            if (MainImageUpload != null)
            {
                using var memoryStream = new MemoryStream();
                await MainImageUpload.CopyToAsync(memoryStream);
                ProductCommand.MainImageUrl = memoryStream.ToArray();
            }

            // Xử lý ảnh cho từng biến thể một cách an toàn
            foreach (var variant in ProductCommand.Variants)
            {
                if (variant.ImageFile != null && variant.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await variant.ImageFile.CopyToAsync(memoryStream);
                    variant.ImageUrl = memoryStream.ToArray();
                }
            }

            await _mediator.Send(ProductCommand);

            TempData.Clear();
            var list = new List<AlertMessage> {
                AlertMessage.Success("Cập nhật thành công")
            };
            TempData["Alerts"] = JsonConvert.SerializeObject(list);

            return RedirectToPage("./Index");
        }



        private async Task LoadDropdownDataAsync()
        {
            // Lấy dữ liệu để đổ vào các select list
            var brands = await _mediator.Send(new GetAllBrandsQuery());
            var categories = await _mediator.Send(new GetAllCatesQuery());
            var colors = await _mediator.Send(new GetAllColorsQuery());
            var sizes = await _mediator.Send(new GetAllSizesQuery());

            Brands = new SelectList(brands, "Id", "Name", ProductCommand.BrandId);
            Categories = new SelectList(categories, "Id", "Name", ProductCommand.CategoryIds);
            Colors = new SelectList(colors, "Id", "Name");
            Sizes = new SelectList(sizes, "Id", "Value");
        }
    }
}
