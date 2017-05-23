using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using netcoredocker.web.Models;

namespace netcoredocker.web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public ILogger Logger { get; set; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Logger = loggerFactory.CreateLogger<Startup>();
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // change server=localhost before running 
            // update-database
            //var connection = "Server=localhost,1433;Database=netcoredocker;User ID=sa;Password=P2ssw0rd;";
            var connection = GetConfigure("ConnectionStrings:MSSQL");
            services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connection));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private string GetConfigure(string configName)
        {
            string varName = configName.Replace(":", "_").ToUpper();

            string varValue = Environment.GetEnvironmentVariable(varName);

            if (varValue != null && varValue != string.Empty)
            {
                Logger.LogInformation("Using Environment Variable [" + varName + "]=" + varValue);
                return varValue;
            }

            return Configuration.GetSection(configName).Value;
        }
    }
}
