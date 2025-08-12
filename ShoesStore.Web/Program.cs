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

    builder.Host.UseSerilog((ctx, sv, cfg) => cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(sv)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddAuthorization();

    // ⬇️ Identity API endpoints + Roles (GUID). KHÔNG thêm AddIdentityCookies.
    builder.Services.AddIdentityApiEndpoints<IdentityAppUser>(opt =>
    {
        opt.SignIn.RequireConfirmedEmail = true;
        opt.User.RequireUniqueEmail = true;
        opt.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

    // Email sender (generic đúng loại)
    builder.Services.AddTransient<IEmailSender<IdentityAppUser>, MailKitEmailSender>();

    // Google chỉ khi có key (KHÔNG gọi AddIdentityCookies)
    var gid = builder.Configuration["Authentication:Google:ClientId"];
    var gsec = builder.Configuration["Authentication:Google:ClientSecret"];
    if (!string.IsNullOrWhiteSpace(gid) && !string.IsNullOrWhiteSpace(gsec))
    {
        builder.Services.AddAuthentication().AddGoogle(o =>
        {
            o.ClientId = gid!;
            o.ClientSecret = gsec!;
            o.SignInScheme = IdentityConstants.ExternalScheme; // external flow
        });
        Log.Information("Google OAuth enabled.");
    }
    else
    {
        Log.Warning("Google OAuth disabled (missing ClientId/ClientSecret).");
    }

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

    builder.Services.AddRazorPages();

    var app = builder.Build();

    // Seed (đã fix generic ở bước B dưới)
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
