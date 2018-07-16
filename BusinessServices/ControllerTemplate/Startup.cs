using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FMASolutionsCore.BusinessServices.ControllerTemplate
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            MvcServiceCollectionExtensions.AddMvc(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            StaticFileExtensions.UseStaticFiles(app);
            if (HostingEnvironmentExtensions.IsDevelopment(env))
                DeveloperExceptionPageExtensions.UseDeveloperExceptionPage(app);
            MvcApplicationBuilderExtensions.UseMvc(app
                , (System.Action<IRouteBuilder>)(routes =>
                 {
                     routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                     //routes.MapRoute("defaultProductGroup", "{controller=ProductGroup}/{action=Search}/{id?}");
                 })
            );
        }
    }
}
