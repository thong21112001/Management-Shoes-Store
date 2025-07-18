using ShoesStore.Application;
using ShoesStore.Infrastructure;
using ShoesStore.Infrastructure.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Gọi phương thức mở rộng để đăng ký DI từ các project khác
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

//Seed dữ liệu mặc định vào cơ sở dữ liệu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await DataSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Có lỗi khi tạo dữ liệu mặc định: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
