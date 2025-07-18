using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoesStore.Domain.Entities.Data;

namespace ShoesStore.Infrastructure.Persistence.Data
{
    public static class DataSeeder
    {
        // Làm await tránh deadlock trong quá trình khởi tạo dữ liệu
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();
                await CreateDataBase(context);
            }
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
    }
}
