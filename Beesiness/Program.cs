using Beesiness;
using Beesiness.Controllers;
using Beesiness.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/LoginIn";
        options.LogoutPath = "/Auth/LogOut";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
builder.Services.AddSession();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
// Agregar el DbContext al contenedor de inyección de dependencias
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Agregado servicio para las tareas en segundo plano
builder.Services.AddHostedService<TareasSP>();

var supportedCultures = new[] { CultureInfo.InvariantCulture };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options = localizationOptions;
});

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);


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
app.UseAuthorization(); //Middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Agenda}/{action=Iniciar}/{id?}");


app.Run();
