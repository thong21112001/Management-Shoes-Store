using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using ShoesStore.Application;
using ShoesStore.Infrastructure;
using ShoesStore.Infrastructure.Persistence.Data;
using ShoesStore.Web.Services;
using System.Threading.RateLimiting;

// DÙNG đúng ApplicationUser kế thừa IdentityUser<Guid>
using IdentityAppUser = ShoesStore.Domain.Entities.ApplicationUser;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Khởi tạo ứng dụng ShoesStore.Web");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Cấu hình Serilog
    builder.Host.UseSerilog((ctx, sv, cfg) => cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(sv)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Email sender (generic đúng loại)
    builder.Services.AddTransient<IEmailSender<IdentityAppUser>, MailKitEmailSender>();

    // Bắt buộc để IdentityCookie hoạt động chính xác
    builder.Services.AddHttpContextAccessor();

    // Cấu hình Identity và API Endpoints TRƯỚC
    builder.Services.AddIdentityApiEndpoints<IdentityAppUser>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

    // Cấu hình Authentication, để login google
    var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
    var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
    {
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = googleClientId;
                options.ClientSecret = googleClientSecret;
            });
        Log.Information("Google Authentication được kích hoạt.");
    }
    else
    {
        builder.Services.AddAuthentication();
        Log.Warning("Google Authentication bị vô hiệu hóa do thiếu ClientId/ClientSecret.");
    }

    // Cấu hình lại Cookie đã được Identity đăng ký
    builder.Services.ConfigureApplicationCookie(options =>
    {
        // Chỉ định đường dẫn login cho các trang Razor
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = "/Admin/Logout";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.ReturnUrlParameter = "returnUrl";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

    builder.Services.AddAuthorization();

    // Cấu hình Rate Limiting chống spam
    builder.Services.AddRateLimiter(o =>
    {
        o.AddFixedWindowLimiter("fixed", x =>
        {
            x.PermitLimit = 3;
            x.Window = TimeSpan.FromMinutes(1);
            x.QueueLimit = 2;
            x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        });
    });

    // Cấu hình RazorPages với Authorization phù hợp
    builder.Services.AddRazorPages(options =>
    {
        // Bảo vệ tất cả các trang NGOẠI TRỪ những trang được chỉ định
        options.Conventions.AuthorizeFolder("/");

        // Cho phép truy cập anonymous đến các trang auth
        options.Conventions.AllowAnonymousToPage("/Admin/Login");
        options.Conventions.AllowAnonymousToPage("/Admin/Register");
        options.Conventions.AllowAnonymousToPage("/Admin/ForgotPassword");
        options.Conventions.AllowAnonymousToPage("/Admin/ResetPassword");
        options.Conventions.AllowAnonymousToPage("/Error");
    });

    var app = builder.Build();

    // Seed dữ liệu vào DB
    using (var scope = app.Services.CreateScope())
    {
        var sv = scope.ServiceProvider;
        try
        {
            await DataSeeder.SeedAsync(sv);
            await DataSeeder.SeedRolesAndAdminAsync(sv);
        }
        catch (Exception ex)
        {
            sv.GetRequiredService<ILogger<Program>>().LogError(ex, "Có lỗi xảy ra trong quá trình seed data.");
        }
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();

    // Map Identity API (dùng CÙNG loại ApplicationUser)
    app.MapGroup("/api/auth").RequireRateLimiting("fixed")
       .MapIdentityApi<IdentityAppUser>();

    app.MapRazorPages();

    app.MapGet("/", context =>
    {
        if (!(context.User?.Identity?.IsAuthenticated ?? false))
        {
            context.Response.Redirect("/Admin/Login");
        }
        else
        {
            context.Response.Redirect("/Index");
        }
        return Task.CompletedTask;
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng không thể khởi động");
}
finally
{
    Log.Information("Tắt ứng dụng");
    Log.CloseAndFlush();
}
