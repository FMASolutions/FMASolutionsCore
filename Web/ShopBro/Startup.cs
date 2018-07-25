using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies; //CookieAuthenticationDefaults
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.SQLAppConfigTypes;

namespace FMASolutionsCore.Web.ShopBro
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            MvcServiceCollectionExtensions.AddMvc(services);

            /*MvcServiceCollectionExtensions.AddMvc(services).AddMvcOptions(options =>
            {
                options.Filters.Add(new ValidateAntiForgeryTokenAttribute());
            });
            */
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/UserAccount/AccessDenied";
                    options.LoginPath = "/UserAccount/Login";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", p => p.RequireAuthenticatedUser().RequireRole("Admin"));
            });            

            string shopDBConnectionString = Program.configExtension.GetSetting(AppSettings.ShopBroDBConnectionString.ToString());
            SQLAppConfigTypes shopSQLDBType = (SQLAppConfigTypes)int.Parse(Program.configExtension.GetSetting(AppSettings.ShopBroDBType.ToString()));
            
            services.AddTransient<IProductGroupService>( s => new ProductGroupService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ISubGroupService>(s => new SubGroupService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<IItemService>(s => new ItemService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ICustomerTypeService>(s => new CustomerTypeService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ICountryService>(s => new CountryService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ICityService>(s => new CityService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ICityAreaService>(s => new CityAreaService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<IPostCodeService>(s => new PostCodeService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<IAddressLocationService>(s => new AddressLocationService(shopDBConnectionString,shopSQLDBType));
            services.AddTransient<ICustomerService>(s => new CustomerService(shopDBConnectionString,shopSQLDBType));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            StaticFileExtensions.UseStaticFiles(app);            

            app.UseAuthentication();

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
