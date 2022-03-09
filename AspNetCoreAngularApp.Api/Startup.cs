using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Mappers;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.Repositories;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "AspNetCoreAngularApp.UI/ClientApp/dist";
            });

            // Add Swagger for documentation
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetAngular API", Version = "v1" });
                                   });

            
            //Add AutoMapper
            var config = new MapperConfiguration(cfg =>
                                                 {
                                                     cfg.AddProfile(new MainProfile());
                                                 });
            services.AddSingleton(sp => config.CreateMapper());
            
            //For In-Memory Caching
            services.AddMemoryCache();
            
            //Add repository
            services.AddScoped<IVendorRepository, VendorRepository>();
            
            services.AddScoped<VendorFactory>();
    
            services.AddScoped<NetflixStockQuoteService>()
                    .AddScoped<IStockQuoteService, NetflixStockQuoteService>(s => s.GetService<NetflixStockQuoteService>());
                
            services.AddScoped<AppleStockQuoteService>()
                    .AddScoped<IStockQuoteService, AppleStockQuoteService>(s => s.GetService<AppleStockQuoteService>());
        
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetAngular API V1");
                             });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "AspNetCoreAngularApp.UI/ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
