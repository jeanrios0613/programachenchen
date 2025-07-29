using System.Globalization;
using FormChenchen.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var esCulture = new CultureInfo("es-ES");

var usNumberFormatCulture = (CultureInfo)esCulture.Clone();
usNumberFormatCulture.NumberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;


var supportedCultures = new [] { usNumberFormatCulture };

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(options => {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });


builder.Services.AddDbContext<ToyoNoToyContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(usNumberFormatCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToyoNoToy}/{action=Create}/{id?}");

app.Run();
