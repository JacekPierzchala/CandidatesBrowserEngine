using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.DbInitializers;
using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Services;
using CandidatesBrowserEngine.Utilities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine
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
            #region postgress
   
            #endregion

            #region sqlServer
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("DefaultConnectionProd").Value);
            });
            #endregion

            services.AddIdentity<ApplicationUser, IdentityRole>
            (options=>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(opt =>
            {

                opt.AppId = Configuration.GetSection("FacebookAppId").Value;
                opt.AppSecret = Configuration.GetSection("FacebookAppSecret").Value;
                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
            })
            .AddGoogle(opt =>
            {

                opt.ClientId = Configuration.GetSection("GoogleClientId").Value; 
                opt.ClientSecret = Configuration.GetSection("GoogleClientSecret").Value;
                opt.ClaimActions.MapJsonKey("image", "picture", "url");
                opt.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");

            });
           



            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddTransient<ICandidateService, CandidateService>();
            services.AddTransient<IProjectService,ProjectService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICandidateCompanyService, CandidateCompanyService>();
            services.AddTransient<ICandidateProjectService, CandidateProjectService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailSender, MailJetEmailSender>();

            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
       
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");

            });


            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                opt.Lockout.MaxFailedAccessAttempts = 5;

            });

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
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

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            dbInitializer.Initialize(Configuration.GetSection("AppAdminL").Value, Configuration.GetSection("AppAdminP").Value); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
