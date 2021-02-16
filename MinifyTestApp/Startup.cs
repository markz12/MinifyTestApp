using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMarkupMin.AspNetCore3;  //-- use WebMarkupMin that we install from nuget packages

namespace MinifyTestApp
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

            //ToString Minify CSHTML file we will be using WebMarkupMin.AspNetCore3  - Mark Vanz
            //Start  - Mark Vanz
            services.AddWebMarkupMin(
                option =>
                {
                    option.AllowMinificationInDevelopmentEnvironment = true;
                    option.AllowCompressionInDevelopmentEnvironment = true;
                })
                .AddHtmlMinification(
                option =>
                {
                    option.MinificationSettings.RemoveRedundantAttributes = true;
                    option.MinificationSettings.RemoveHttpProtocolFromAttributes = true; // Http Protocol attributes  - Mark Vanz
                    option.MinificationSettings.RemoveHttpsProtocolFromAttributes = true; // Https Protocol attributes  - Mark Vanz
                })
                .AddHttpCompression();
            //END  - Mark Vanz
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

            app.UseWebMarkupMin(); // Use WebMarkup during run time. - Mark Vanz

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
