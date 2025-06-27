using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Infrastructure.Persistence.Data;
using ShoesStore.Infrastructure.Persistence.Repositories;

namespace ShoesStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext với chuỗi kết nối từ cấu hình và các Interfaces (Application) + Repositories (Infrastructure)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();


            return services;
        }
    }
}
