using AspNetCoreHero.ToastNotification;
using BJ.ApiConnection.Services;
using BJ.App.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var cultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("vi"),
};
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("*")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews().AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
{
    // When using all the culture providers, the localization process will
    // check all available culture providers in order to detect the request culture.
    // If the request culture is found it will stop checking and do localization accordingly.
    // If the request culture is not found it will check the next provider by order.
    // If no culture is detected the default culture will be used.

    // Checking order for request culture:
    // 1) RouteSegmentCultureProvider
    //      e.g. http://localhost:1234/tr
    // 2) QueryStringCultureProvider
    //      e.g. http://localhost:1234/?culture=tr
    // 3) CookieCultureProvider
    //      Determines the culture information for a request via the value of a cookie.
    // 4) AcceptedLanguageHeaderRequestCultureProvider
    //      Determines the culture information for a request via the value of the Accept-Language header.
    //      See the browsers language settings

    // Uncomment and set to true to use only route culture provider
    ops.UseAllCultureProviders = false;
    ops.ResourcesPath = "LocalizationResources";
    ops.RequestLocalizationOptions = o =>
    {
        o.SupportedCultures = cultures;
        o.SupportedUICultures = cultures;
        o.DefaultRequestCulture = new RequestCulture("vi");
    };
});



builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddScoped<IProductServiceConnection, ProductServiceConnection>();
builder.Services.AddScoped<ICategoryServiceConnection, CategoryServiceConnection>();
builder.Services.AddScoped<ISizeServiceConnection, SizeServiceConnection>();
builder.Services.AddScoped<IStoreLocationServiceConnection, StoreLocationServiceConnection>();
builder.Services.AddScoped<IBlogServiceConnection, BlogServiceConnection>();
builder.Services.AddScoped<INewsServiceConnection, NewsServiceConnection>();
builder.Services.AddScoped<IEmailServiceConnection, EmailServiceConnection>();
builder.Services.AddScoped<ILanguageServiceConnection, LanguageServiceConnection>();
builder.Services.AddScoped<IVisitorCounterServiceConnection, VisitorCounterServiceConnection>();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });





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
app.UseRouting();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseRequestLocalization();
app.UseMiddleware(typeof(VisitorCounterMiddleware));
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/robots.txt"))
    {
        var robotsTxtPath = Path.Combine(app.Environment.ContentRootPath, "robots.txt");
        string output = "User-agent: *  \nDisallow: /";
        if (File.Exists(robotsTxtPath))
        {
            output = await File.ReadAllTextAsync(robotsTxtPath);
        }
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(output);
    }
    else await next();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Product List En",
    pattern: "{culture}/drinks", new
    {
        controller = "Product",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Product List Vi",
    pattern: "{culture}/thuc-uong", new
    {
        controller = "Product",
        action = "Index"
    });
//app.MapControllerRoute(
//    name: "Product Detail En",
//    pattern: "{culture}/drinks/{Alias}", new
//    {
//        controller = "Product",
//        action = "Detail"
//    });
//app.MapControllerRoute(
//    name: "Product List Vi",
//    pattern: "{culture}/thuc-uong/{Alias}", new
//    {
//        controller = "Product",
//        action = "Detail"
//    });
app.MapControllerRoute(
    name: "Store List En",
    pattern: "{culture}/stores", new
    {
        controller = "Store",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Store List Vi",
    pattern: "{culture}/cua-hang", new
    {
        controller = "Store",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Wellbeing List En",
    pattern: "{culture}/wellbeing", new
    {
        controller = "Blog",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Wellbeing List Vi",
    pattern: "{culture}/song-khoe", new
    {
        controller = "Blog",
        action = "Index"
    });
app.MapControllerRoute(
    name: "WellBeing Detail En",
    pattern: "{culture}/wellbeing/{id}/{Alias}", new
    {
        controller = "Blog",
        action = "Detail"
    });
app.MapControllerRoute(
    name: "Wellbeing Detail Vi",
    pattern: "{culture}/song-khoe/{id}/{Alias}", new
    {
        controller = "Blog",
        action = "Detail"
    });
app.MapControllerRoute(
    name: "News List En",
    pattern: "{culture}/news", new
    {
        controller = "News",
        action = "Index"
    });
app.MapControllerRoute(
    name: "News List Vi",
    pattern: "{culture}/tin-tuc", new
    {
        controller = "News",
        action = "Index"
    });
app.MapControllerRoute(
    name: "News Detail En",
    pattern: "{culture}/news/{id}/{Alias}", new
    {
        controller = "News",
        action = "Detail"
    });
app.MapControllerRoute(
    name: "News Detail Vi",
    pattern: "{culture}/tin-tuc/{id}/{Alias}", new
    {
        controller = "News",
        action = "Detail"
    });
app.MapControllerRoute(
    name: "Contact En",
    pattern: "{culture}/contact", new
    {
        controller = "Home",
        action = "Contact"
    });
app.MapControllerRoute(
    name: "Contact Vi",
    pattern: "{culture}/lien-he", new
    {
        controller = "Home",
        action = "Contact"
    });
app.MapControllerRoute(
    name: "About En",
    pattern: "{culture}/about", new
    {
        controller = "Home",
        action = "About"
    });
app.MapControllerRoute(
    name: "About Vi",
    pattern: "{culture}/ve-boost-juice", new
    {
        controller = "Home",
        action = "About"
    });
app.MapControllerRoute(
    name: "Privacy En",
    pattern: "{culture}/privacy-policy", new
    {
        controller = "Home",
        action = "Privacy"
    });
app.MapControllerRoute(
    name: "About Vi",
    pattern: "{culture}/chinh-sach-bao-mat", new
    {
        controller = "Home",
        action = "Privacy"
    });
app.MapControllerRoute(
    name: "UsePolicy En",
    pattern: "{culture}/acceptable-use-policy", new
    {
        controller = "Home",
        action = "UsePolicy"
    });
app.MapControllerRoute(
    name: "UsePolicy Vi",
    pattern: "{culture}/chinh-sach-su-dung-duoc-chap-nhan", new
    {
        controller = "Home",
        action = "UsePolicy"
    });
app.MapControllerRoute(
    name: "Delivery En",
    pattern: "{culture}/delivery-return-policy", new
    {
        controller = "Home",
        action = "Delivery"
    });
app.MapControllerRoute(
    name: "Delivery Vi",
    pattern: "{culture}/giao-hang-hoan-tra", new
    {
        controller = "Home",
        action = "Delivery"
    });
app.MapControllerRoute(
    name: "TermOfUse En",
    pattern: "{culture}/terms-of-use", new
    {
        controller = "Home",
        action = "TermOfUse"
    });
app.MapControllerRoute(
    name: "TermOfUse Vi",
    pattern: "{culture}/dieu-khoan-thoa-thuan", new
    {
        controller = "Home",
        action = "TermOfUse"
    });
app.MapControllerRoute(
    name: "Promotion En",
    pattern: "{culture}/promotion", new
    {
        controller = "Promotion",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Promotion Vi",
    pattern: "{culture}/khuyen-mai", new
    {
        controller = "Promotion",
        action = "Index"
    });
app.MapControllerRoute(
    name: "Promotion Detail En",
    pattern: "{culture}/promotion/{id}/{Alias}", new
    {
        controller = "Promotion",
        action = "Detail"
    });
app.MapControllerRoute(
    name: "Promotion Detail Vi",
    pattern: "{culture}/khuyen-mai/{id}/{Alias}", new
    {
        controller = "Promotion",
        action = "Detail"
    });
app.Run();
