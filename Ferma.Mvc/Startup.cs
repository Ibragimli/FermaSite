using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using Ferma.Data.Repositories;
using Ferma.Data.UnitOfWork;
using Ferma.Mvc.ServiceExtentions;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Implementations;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Implementations;
using Ferma.Service.Services.Implementations.Area;
using Ferma.Service.Services.Implementations.User;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using Ferma.Service.Services.Interfaces.User;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<PosterCreateDto>());

            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddIdentityServiceExtention();
            services.AddServiceScopeExtention();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.AddExceptionHandlerService();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=dashboard}/{action=index}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=AnaSehife}/{action=Index}/{id?}");
            });
        }
    }
}
