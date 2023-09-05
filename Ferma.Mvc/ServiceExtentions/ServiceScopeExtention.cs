using Ferma.Core.IUnitOfWork;
using Ferma.Core.Repositories;
using Ferma.Data.Repositories;
using Ferma.Data.UnitOfWork;
using Ferma.Service.HelperService.Implementations;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Implementations;
using Ferma.Service.Services.Implementations.Area;
using Ferma.Service.Services.Implementations.Area.RoleManagers;
using Ferma.Service.Services.Implementations.Area.UserManagers;
using Ferma.Service.Services.Implementations.User;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using Ferma.Service.Services.Interfaces.Area.RoleManagers;
using Ferma.Service.Services.Interfaces.Area.UserManagers;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ServiceExtentions
{
    public static class ServiceScopeExtention
    {
        public static void AddServiceScopeExtention(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPosterRepository, PosterRepository>();
            services.AddScoped<IPosterFeaturesRepository, PosterFeaturesRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IImageSettingRepository, ImageSettingRepository>();
            services.AddScoped<IPosterFeaturesRepository, PosterFeaturesRepository>();
            services.AddScoped<IPosterImageRepository, PosterImageRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWishItemRepository, WishItemRepository>();
            services.AddScoped<IUserTermRepository, UserTermRepository>();


            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IPosterCreateServices, PosterCreateServices>();
            services.AddScoped<IManageImageHelper, ManageImageHelper>();
            services.AddScoped<IImageValue, ImageValue>();
            services.AddScoped<ILayoutServices, LayoutServices>();
            services.AddScoped<IAnaSehifeIndexServices, AnaSehifeIndexServices>();
            services.AddScoped<IPosterCreateIndexServices, PosterCreateIndexServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IPosterCreateValueCheckServices, PosterCreateValueCheckServices>();
            services.AddScoped<IPaymentCreateServices, PaymentCreateServices>();
            services.AddScoped<IProfileLoginServices, ProfileLoginServices>();
            services.AddScoped<IPhoneNumberServices, PhoneNumberServices>();
            services.AddScoped<IProfileEditServices, ProfileEditServices>();
            services.AddScoped<IPosterEditServices, PosterEditServices>();
            services.AddScoped<IBalanceIncreaseServices, BalanceIncreaseServices>();
            services.AddScoped<IPosterWishlistAddServices, PosterWishlistAddServices>();
            services.AddScoped<IPosterWishlistDeleteServices, PosterWishlistDeleteServices>();
            services.AddScoped<IPosterSearchServices, PosterSearchServices>();
            services.AddScoped<IContactUsServices, ContactUsServices>();
            services.AddScoped<IUserPostersServices, UserPostersServices>();
            services.AddScoped<IPosterDetailIndexServices, PosterDetailIndexServices>();
            services.AddScoped<IAdminLoginServices, AdminLoginServices>();
            services.AddScoped<IAdminPosterDetailIndexServices, AdminPosterDetailIndexServices>();
            services.AddScoped<IAdminPosterIndexServices, AdminPosterIndexServices>();
            services.AddScoped<IAdminPosterEditServices, AdminPosterEditServices>();
            services.AddScoped<IBalancePaymentIndexServices, BalancePaymentIndexServices>();
            services.AddScoped<IPosterPaymentIndexServices, PosterPaymentIndexServices>();
            services.AddScoped<IAdminCategoryServices, AdminCategoryServices>();
            services.AddScoped<IAdminSubCategoryServices, AdminSubCategoryServices>();
            services.AddScoped<IAdminCityServices, AdminCityServices>();
            services.AddScoped<IAdminServiceDurationServices, AdminServiceDurationServices>();
            services.AddScoped<ISettingEditServices, SettingEditServices>();
            services.AddScoped<ISettingIndexServices, SettingIndexServices>();
            services.AddScoped<IContactUsDeleteServices, ContactUsDeleteServices>();
            services.AddScoped<IContactUsIndexServices, ContactUsIndexServices>();
            services.AddScoped<IContactRespondServices, ContactRespondServices>();
            services.AddScoped<IPosterDeleteServices, PosterDeleteServices>();
            services.AddScoped<IAdminUserTermServices, AdminUserTermServices>();
            services.AddScoped<IUserTermIndexServices, UserTermIndexServices>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddScoped<IProfileIndexServices, ProfileIndexServices>();
            services.AddScoped<ISmsSenderServices, SmsSenderServices>();

            services.AddScoped<IAdminRoleManagerCreateServices, AdminRoleManagerCreateServices>();
            services.AddScoped<IAdminRoleManagerEditServices, AdminRoleManagerEditServices>();
            services.AddScoped<IAdminRoleManagerDeleteServices, AdminRoleManagerDeleteServices>();
            services.AddScoped<IAdminRoleManagerIndexServices, AdminRoleManagerIndexServices>();

            services.AddScoped<IAdminUserManagerCreateServices, AdminUserManagerCreateServices>();
            services.AddScoped<IAdminUserManagerEditServices, AdminUserManagerEditServices>();
            services.AddScoped<IAdminUserManagerDeleteServices, AdminUserManagerDeleteServices>();
            services.AddScoped<IAdminUserManagerIndexServices, AdminUserManagerIndexServices>();

        }
    }
}
