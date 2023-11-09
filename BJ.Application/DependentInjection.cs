using BJ.Application.Email;
using BJ.Application.Mapping;
using BJ.Application.Service;
using BJ.Contract.Config;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BJ.Application
{
    public static class DependentInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BJContext>(options => options.UseSqlServer(configuration.GetConnectionString("BJConnection")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISizeService, SizeService>();
            services.AddScoped<IConfigProductService, ConfigProductService>();
            services.AddScoped<IStoreLocationService, StoreLocationService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);
            services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            services.AddAutoMapper(typeof(SizeMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);
            services.AddAutoMapper(typeof(StoreLocationMappingProfile).Assembly);
            services.AddAutoMapper(typeof(LanguageMappingProfile).Assembly);
            services.AddAutoMapper(typeof(BlogMappingProfile).Assembly);
            services.AddAutoMapper(typeof(NewsMappingProfile).Assembly);


            //Config AppSetting
            services.Configure<AppSetting>(configuration.GetSection("AppSettings"));

            var secretKey = configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero,

                        ValidateLifetime = false,
                    };
                });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });



            return services;
        }
    }
}
