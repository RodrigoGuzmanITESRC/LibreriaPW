using LibreriaPW.Models;
using LibreriaPW.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.LoginPath = "/Home/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddTransient<MenuServices>();
builder.Services.AddDbContext<librosContext>(
    optionsBuilder => optionsBuilder.UseMySql("server=libreria.sistemas19.com;password=sistemas19_;user=sistem21_rodrigo;database=sistem21_libros", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"))
    );
builder.Services.AddMvc();
var app = builder.Build();

app.UseFileServer();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(x =>
{
    x.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
    x.MapDefaultControllerRoute();
});

app.Run();
