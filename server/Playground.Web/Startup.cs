using System.Linq;
using System.Runtime.Versioning;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Playground.Core.Banner;
using Playground.Core.Extensions;
using Playground.Core.Logging;
using Playground.Core.Upload;
using Playground.Data;
using Playground.Identity;
using Playground.Identity.Mock;
using Playground.Office;

[assembly:SupportedOSPlatform("windows")]
namespace Playground.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public LogProvider Logger { get; }
        public OfficeConfig OfficeConfig { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Logger = new LogProvider
            {
                LogDirectory = Configuration.GetValue<string>("LogDirectory")
                    ?? $@"{Environment.WebRootPath}\logs"
            };

            OfficeConfig = new OfficeConfig
            {
                Directory = Configuration.GetValue<string>("OfficeDirectory")
                    ?? $@"{Environment.WebRootPath}\office"
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(Configuration.GetConnectionString("Project"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Playground", Version="v1"});
            });

            services.AddSignalR();

            services.AddSingleton(new BannerConfig
            {
                Label = Configuration.GetValue<string>("AppBannerLabel"),
                Background = Configuration.GetValue<string>("AppBannerBackground"),
                Color = Configuration.GetValue<string>("AppBannerColor")
            });

            services.AddSingleton(OfficeConfig);

            if (Environment.IsDevelopment())
            {
                services.AddSingleton(new UploadConfig
                {
                    DirectoryBasePath = $@"{Environment.ContentRootPath}/wwwroot/files/",
                    UrlBasePath = "/files/"
                });

                services
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

                services.AddScoped<IUserProvider, MockProvider>();
            }
            else
            {
                services.AddSingleton(new UploadConfig
                {
                    DirectoryBasePath = Configuration.GetValue<string>("AppDirectoryBasePath"),
                    UrlBasePath = Configuration.GetValue<string>("AppUrlBasePath")
                });

                services.AddScoped<IUserProvider, AdUserProvider>();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseAuthentication();
                app.UseMockMiddleware();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Playground v1"));
            }
            else
            {
                app.UseAdMiddleware();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Logger.LogDirectory),
                RequestPath = "/logs"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Logger.LogDirectory),
                RequestPath = "/logs"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(OfficeConfig.Directory),
                RequestPath = "/office"
            });

            app.UseExceptionHandler(err => err.HandleError(Logger));

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.WithOrigins(GetConfigArray("CorsOrigins"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        string[] GetConfigArray(string section) => Configuration.GetSection(section)
            .GetChildren()
            .Select(x => x.Value)
            .ToArray();
    }
}
