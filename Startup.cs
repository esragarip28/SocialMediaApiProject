
using DemoApplication1.Models.DataAccess;
using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.DataAccess.Concretes;
using DemoApplication1.Models.Services.Abstracts;
using DemoApplication1.Models.Services.Concretes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DemoApplication1.Extensions;

namespace DemoApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //Add Cors
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            //Json Serializer
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.Configure<CookiePolicyOptions>(options => {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                
            });
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options =>
              options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddControllers();
            services.AddSwaggerDocument();
            services.AddScoped<IInstagramClientService,InstagramClientService>();
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IAccessTokenDal, AccessTokenDal>();
            services.AddScoped<IPostDal,PostDal>();
            services.AddScoped<ILoginService, LoginService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IInstagramBusinessInfoDal, InstagramBusinessInfoDal>();
            services.AddSingleton<ILog, LogNLog>();
            services.AddScoped<ICommentNotificationDal, CommentNotificationDal>();
            services.AddScoped<IInstagramUserProfileDal, InstagramUserProfileDal>();
            services.AddScoped<IProfileStatisticDal, ProfileStatisticDal>();
            services.AddScoped<ICommentNotificationDal, CommentNotificationDal>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILog logger)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseCookiePolicy(
            ); 
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                 name: "instagram_oauth",
                 pattern: "{controller=Facebook}/{action=facebook_oauth}");
            });
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.ConfigureExceptionHandler(logger);
            
        }
    }
}
