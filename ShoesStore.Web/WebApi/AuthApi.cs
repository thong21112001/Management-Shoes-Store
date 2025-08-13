using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ShoesStore.Domain.Entities;
using System.Text;

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


            // /api/auth/logout
            group.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization(); // Yêu cầu phải đăng nhập mới được logout


            // /api/auth/register
            group.MapPost("/register", async (
                [FromBody] RegisterRequest model,
                UserManager<ApplicationUser> userManager,
                // Sử dụng đúng IEmailSender<ApplicationUser>
                IEmailSender<ApplicationUser> emailSender,
                LinkGenerator linkGenerator,
                IHttpContextAccessor httpContextAccessor) =>
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.Email
                };

                // Tạo người dùng với mật khẩu đã cung cấp
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    // Nếu có lỗi (email trùng, mật khẩu yếu...), trả về chi tiết lỗi
                    return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
                }

                //Gán role "Admin" cho người dùng mới
                await userManager.AddToRoleAsync(user, "Admin");

                // Gửi email xác thực
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); // Mã hóa token cho URL

                var request = httpContextAccessor.HttpContext!.Request;
                var confirmationLink = linkGenerator.GetUriByPage(
                    httpContextAccessor.HttpContext,
                    "/Admin/ConfirmEmail", // Trang sẽ xử lý việc xác thực
                    handler: null,
                    values: new { userId = user.Id, token = token });

                if (confirmationLink != null)
                {
                    // Gọi đúng phương thức SendConfirmationLinkAsync từ MailKitEmailSender
                    await emailSender.SendConfirmationLinkAsync(user, model.Email, confirmationLink);
                }

                return Results.Ok();
            });

            return group;
        }

        public record LoginRequest(string Email, string Password, bool RememberMe);

        public record RegisterRequest(string Email, string Password);
    }
}
