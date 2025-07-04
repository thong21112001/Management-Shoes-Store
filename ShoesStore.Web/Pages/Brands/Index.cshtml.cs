using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Web.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly IBrandRepository _brandRepository;

        public IndexModel(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public IList<BrandsDto> Brand { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_brandRepository != null)
            {
                Brand = await GetDataAsync();
            }
            else
            {
                Brand = new List<BrandsDto>();
            }
        }



        #region Phần Chi tiết và Xóa
        //Lấy thông tin đẩy lên partial: _DetailsPartial
        public async Task<IActionResult> OnGetDetailsPartial(int id)
        {
            var br = await GetBrandById(id);

            if (br == null)
                return NotFound();

            return Partial("_DetailsPartial", br);
        }


        //Lấy thông tin đẩy lên partial: _DeletePartial
        public async Task<IActionResult> OnGetDeletePartial(int id)
        {
            var br = await GetBrandById(id);

            if (br == null)
                return NotFound();

            return Partial("_DeletePartial", br);
        }


        //Tiến hành kiểm tra và xóa
        public async Task<IActionResult> OnPostDeleteBrandAsync(int id)
        {
            var br = await GetBrandById(id);

            if (br == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy dữ liệu cần xóa!" });
            }

            await _brandRepository.DeleteAsync(id);

            return new JsonResult(new { success = true });
        }
        #endregion



        #region Viết phương thức xử lý
        private async Task<Brand?> GetBrandById(int id)
        {
            return await _brandRepository.GetBrandByIdAsync(id);
        }

        private async Task<IList<BrandsDto>> GetDataAsync()
        {
            return await _brandRepository.GetAllBrandsAsync();
        }
        #endregion
    }
}
