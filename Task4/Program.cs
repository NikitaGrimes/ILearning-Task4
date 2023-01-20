using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequiredLength = 1;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddTransient<ApplicationSeeder>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IApplicationRepository, ApplicaionRepository>();

var app = builder.Build();

var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var seeder = scope.ServiceProvider.GetService<ApplicationSeeder>();
    seeder.Seed();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
