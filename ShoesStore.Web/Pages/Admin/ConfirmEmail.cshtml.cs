using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ShoesStore.Domain.Entities;
using System.Text;

namespace ShoesStore.Web.Pages.Admin
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;


        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                StatusMessage = $"Lỗi: Không tìm thấy người dùng.";
                return Page();
            }

            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (result.Succeeded)
                {
                    StatusMessage = "Cảm ơn bạn đã xác thực email. Bây giờ bạn có thể đăng nhập.";
                    IsSuccess = true;
                }
                else
                {
                    StatusMessage = "Lỗi: Không thể xác thực email.";
                }
            }
            catch (FormatException)
            {
                StatusMessage = "Lỗi: Token không hợp lệ.";
            }

            return Page();
        }
    }
}
