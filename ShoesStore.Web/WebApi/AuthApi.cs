using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.Domain.Entities;

namespace ShoesStore.Web.WebApi
{
    public static class AuthApi
    {
        public static RouteGroupBuilder MapAuthApi(this RouteGroupBuilder group)
        {
            // /api/auth/login
            group.MapPost("/login", async ([FromBody] LoginRequest model, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
            {
                // Bước 1: Kiểm tra xem người dùng có tồn tại không
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    // Trả về lỗi 400 Bad Request với thông báo cụ thể
                    return Results.Problem("Email này không tồn tại trong hệ thống.", statusCode: 400);
                }

                // Bước 2: Nếu người dùng tồn tại, tiến hành đăng nhập bằng mật khẩu
                // Tham số lockoutOnFailure: true sẽ tự động xử lý việc khóa tài khoản nếu nhập sai nhiều lần
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

                // Bước 3: Xử lý các kết quả đăng nhập thất bại
                if (result.IsLockedOut)
                {
                    return Results.Problem("Tài khoản đã bị khóa do đăng nhập sai nhiều lần. Vui lòng thử lại sau.", statusCode: 400);
                }

                if (result.IsNotAllowed)
                {
                    return Results.Problem("Tài khoản của bạn chưa được xác thực. Vui lòng kiểm tra email.", statusCode: 400);
                }

                if (!result.Succeeded)
                {
                    // Đây là trường hợp cuối cùng: sai mật khẩu
                    return Results.Problem("Mật khẩu không chính xác.", statusCode: 400);
                }

                // Nếu tất cả đều thành công, trả về 200 OK.
                // Cookie đã được tự động tạo bởi SignInManager.
                return Results.Ok();
            });

            return group;
        }

        public record LoginRequest(string Email, string Password, bool RememberMe);
    }
}
