using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetBook.Options;
using Microsoft.OpenApi.Models;
using TweetBook.IInstalers;

namespace TweetBook
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
            var classesImplementingIInstaler = typeof(Startup).Assembly.ExportedTypes.Where(o => 
                typeof(IInstaller).IsAssignableFrom(o) && !o.IsInterface && !o.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            classesImplementingIInstaler.ForEach(o=> o.InstallServices(services,Configuration));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            else
            {
                
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options=> { options.RouteTemplate = swaggerOptions.JsonRoute;});

            app.UseSwaggerUI(options=> {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint,swaggerOptions.Description);
                });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseAuthentication();
            
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
