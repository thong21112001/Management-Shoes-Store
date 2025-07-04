using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;

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
                Categories = await GetDataAsync();
            }
            else
            {
                Categories = new List<CategoriesDto>();
            }
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var cate = await GetCategoryById(id);

            if (cate == null)
                return NotFound();

            return Partial("_DetailsPartial", cate);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var cate = await GetCategoryById(id);

            if (cate == null)
                return NotFound();

            return Partial("_DeletePartial", cate);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteCategoryAsync(int id)
        {
            var cate = await GetCategoryById(id);

            if (cate == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy dữ liệu cần xóa!" });
            }

            await _categoryRepository.DeleteAsync(id);

            return new JsonResult(new { success = true });
        }
        #endregion



        #region Viết phương thức xử lý
        private async Task<Category?> GetCategoryById(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        private async Task<IList<CategoriesDto>> GetDataAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
        #endregion
    }
}
