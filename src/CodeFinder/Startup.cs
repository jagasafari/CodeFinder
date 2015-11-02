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
        public Startup(IHostingEnvironment env,
            IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ICodeFounderFactory, CodeFounderFactory>();
            services.AddScoped<IFileCodeProcessor, FileCodeProcessor>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            //To-Do Uncomment when Microsoft.Framework.Logging.NLog is available
            //loggerFactory.AddNLog(new global::NLog.LogFactory());

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
