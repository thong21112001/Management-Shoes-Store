using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoesStore.Domain.Entities.Data;
using IdentityApplicationUser = ShoesStore.Domain.Entities.ApplicationUser;

namespace ShoesStore.Infrastructure.Persistence.Data
{
    public static class DataSeeder
    {
        // Làm await tránh deadlock trong quá trình khởi tạo dữ liệu
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;

            var db = sp.GetRequiredService<ApplicationDbContext>();

            // DÙNG MIGRATIONS thay vì EnsureCreated (đúng bài EF Core)
            await db.Database.MigrateAsync();

            // seed data cơ bản
            await CreateDataBase(db);
        }

        private static async Task CreateDataBase(ApplicationDbContext context)
        {
            // Seed Brands
            if (!await context.Brands.AnyAsync())
            {
                var brands = new List<Brand>
                {
                    new() { Name = "Nike", Description = "Just Do It" },
                    new() { Name = "Adidas", Description = "Impossible is Nothing" },
                    new() { Name = "Puma", Description = "Forever Faster" }
                };
                await context.Brands.AddRangeAsync(brands);
            }

            // Seed Categories
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new() { Name = "Running Shoes", Description = "For running enthusiasts" },
                    new() { Name = "Basketball Shoes", Description = "Dominate the court" },
                    new() { Name = "Lifestyle", Description = "Everyday comfort and style" }
                };
                await context.Categories.AddRangeAsync(categories);
            }

            // Seed Sizes
            if (!await context.Sizes.AnyAsync())
            {
                var sizes = new List<Size>
                {
                    new() { Value = "38", Type = "EU" },
                    new() { Value = "39", Type = "EU" },
                    new() { Value = "40", Type = "EU" },
                    new() { Value = "41", Type = "EU" },
                    new() { Value = "42", Type = "EU" },
                    new() { Value = "9", Type = "US" },
                    new() { Value = "10", Type = "US" }
                };
                await context.Sizes.AddRangeAsync(sizes);
            }

            // Seed Colors
            if (!await context.Colors.AnyAsync())
            {
                var colors = new List<Color>
                {
                    new() { Name = "Black", HexCode = "#000000" },
                    new() { Name = "White", HexCode = "#FFFFFF" },
                    new() { Name = "Red", HexCode = "#FF0000" },
                    new() { Name = "Blue", HexCode = "#0000FF" }
                };
                await context.Colors.AddRangeAsync(colors);
            }

            // Lưu tất cả các thay đổi vào DB
            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var cfg = sp.GetRequiredService<IConfiguration>();
            var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>(); // ✅ GUID
            var userMgr = sp.GetRequiredService<UserManager<IdentityApplicationUser>>();    // ✅ ApplicationUser đúng loại


            // Seed Roles
            string[] roleNames = { "SuperAdmin", "Admin" };
            foreach (var role in roleNames)
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole<Guid>(role));

            // Admin defaults (có thể lấy từ appsettings: Auth:Seed:*)
            var adminEmail = cfg["Auth:Seed:AdminEmail"] ?? "thongzuka2000@gmail.com";
            var adminPass = cfg["Auth:Seed:AdminPassword"] ?? "Admin123@";
            var adminName = cfg["Auth:Seed:AdminFullName"] ?? "System Admin";

            // Seed Admin User
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new IdentityApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = adminName
                };
                var res = await userMgr.CreateAsync(admin, adminPass);
                if (!res.Succeeded)
                    throw new Exception("Create admin failed: " + string.Join(";", res.Errors.Select(e => e.Description)));
            }

            // Add Admin to Roles
            var roles = await userMgr.GetRolesAsync(admin);
            if (!roles.Contains("SuperAdmin"))
                await userMgr.AddToRolesAsync(admin, new[] { "SuperAdmin" });
        }
    }
}
