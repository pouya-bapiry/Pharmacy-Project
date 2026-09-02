using Pharmacy.Application.Services.Implementation;
using Pharmacy.Application.Services.Implementations;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.IRepository;
using Pharmacy.Infrastructure.Repository;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Pharmacy.Web.DI
{
    public static class DIRegister
    {


        public static void RegisterService(this IServiceCollection services)
        {


            #region Repository

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            #region General Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ISiteImagesService, SiteImagesService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDiscountService, ProductDiscountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBlogService, BlogService>();



            #endregion

            #region Common Services

            services.AddHttpContextAccessor();
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            //services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            services.AddScoped<IAuthHelper, AuthHelper>();

            #endregion

        }

    }
}
