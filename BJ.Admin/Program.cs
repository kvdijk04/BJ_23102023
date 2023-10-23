using BJ.ApiConnection.Services;
using BJ.Contract.Config;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

var secretKey = builder.Configuration["AppSettings:SecretKey"];


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/dang-nhap.html";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.ExpireTimeSpan = TimeSpan.FromSeconds(20);

    });
builder.Services.AddSession();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(1800);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductServiceConnection, ProductServiceConnection>();
builder.Services.AddScoped<ISizeServiceConnection, SizeServiceConnection>();

builder.Services.AddScoped<ICategoryServiceConnection, CategoryServiceConnection>();
builder.Services.AddScoped<ISubCategoryServiceConnection, SubCategoryServiceConnection>();
builder.Services.AddScoped<IConfigProductServiceConnection, ConfigProductServiceConnection>();
builder.Services.AddScoped<ILoginServiceConnection, LoginServiceConnection>();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
