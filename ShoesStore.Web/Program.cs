using ShoesStore.Application.Common.Mapping;
using ShoesStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Gọi phương thức mở rộng để đăng ký DI từ các project khác
builder.Services.AddInfrastructure(builder.Configuration);

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

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
