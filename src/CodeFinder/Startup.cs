namespace CodeFinder
{
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.Logging;
    using Services;
    using Services.Contracts;

    public class Startup
    {
        public Startup(IApplicationEnvironment appEnv)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ICodeFinderFactory, CodeFinderFactory>();
            services.AddScoped<IFileCodeProcessor, FileCodeProcessor>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute("default",
                        "{controller=Search}/{action=MatchingFiles}/{id?}");
                });
        }
    }
}
