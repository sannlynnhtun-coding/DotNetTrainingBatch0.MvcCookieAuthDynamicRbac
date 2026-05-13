using DotNetTrainingBatch0.Database;
using DotNetTrainingBatch0.Database.AppDbContextModels;
using DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Auth;
using DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Product;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register AppDbContext with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MvcCookieAuthDynamicDb"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.TblRoles.Any())
    {
        var adminRole = new AppRole { Id = 1, RoleName = "Admin" };
        var staffRole = new AppRole { Id = 2, RoleName = "Staff" };
        context.TblRoles.AddRange(adminRole, staffRole);

        var viewPerm = new AppPermission { Id = 1, PermissionName = "Product.View" };
        var createPerm = new AppPermission { Id = 2, PermissionName = "Product.Create" };
        var updatePerm = new AppPermission { Id = 3, PermissionName = "Product.Update" };
        var deletePerm = new AppPermission { Id = 4, PermissionName = "Product.Delete" };
        context.TblPermissions.AddRange(viewPerm, createPerm, updatePerm, deletePerm);

        context.TblRolePermissions.AddRange(
            new AppRolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
            new AppRolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
            new AppRolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
            new AppRolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
            new AppRolePermission { Id = 5, RoleId = 2, PermissionId = 1 }
        );

        context.TblUsers.AddRange(
            new AppUser { Id = 1, Username = "admin", Password = "123", RoleId = 1 },
            new AppUser { Id = 2, Username = "staff", Password = "123", RoleId = 2 }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
