using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShoesStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký auto-mapper để ánh xạ các đối tượng tại đây luôn không cần ở Web
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            // Tự động tìm và đăng ký tất cả các Handler trong assembly này
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
