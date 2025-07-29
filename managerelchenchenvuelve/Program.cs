using managerelchenchenvuelve.Models;
using managerelchenchenvuelve.Services;
using managerelchenchenvuelve.Filters;
using managerelchenchenvuelve.Controllers; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AuthorizeFilter = Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddScoped<AuthorizeFilter>();

builder.Services.AddControllersWithViews(/*options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}*/);



builder.Services.AddTransient<DatabaseConnection>(); 

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});


builder.Services.AddDbContext<ToyoNoToyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
        });
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ToyoNoToyContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied"; 
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();
