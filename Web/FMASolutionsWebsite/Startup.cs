using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http; //HttpRequestMessage
using Microsoft.AspNetCore.Http; //Pathstring
using System.Net.Http.Headers; //MediaType
using System.Security.Claims; //ClaimTypes
using Microsoft.AspNetCore.Authentication; //MapJsonKey
using Microsoft.AspNetCore.Authentication.Cookies; //CookieAuthenticationDefaults
using Microsoft.AspNetCore.Authentication.OAuth; //OAuthEvents
using Newtonsoft.Json.Linq; //JObject

namespace FMASolutionsCore.Web.FMASolutionsWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            MvcServiceCollectionExtensions.AddMvc(services);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "FMASolutions";
            })
            .AddCookie()
            .AddOAuth("FMASolutions", options =>
            {
                options.ClientId = Program.ConfigService.GetSetting(FMAWebsite.AppSettings.AppID.ToString());
                options.ClientSecret = Program.ConfigService.GetSetting(FMAWebsite.AppSettings.AppPassword.ToString());
                options.CallbackPath = new PathString("/UserAccount/LoginCallBack");

                options.AuthorizationEndpoint = "https://FMASolutionsAuthService.co.uk/User/TryAuth/";
                options.TokenEndpoint = "https://FMASolutionsAuthService.co.uk/User/Tokens";
                options.UserInformationEndpoint = "https://FMASolutionsAuthService.co.uk/User/Profile";

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "Name");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                        
                        context.RunClaimActions(user);
                    }                            
                };

            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            StaticFileExtensions.UseStaticFiles(app);
            
            app.UseAuthentication();

            if (HostingEnvironmentExtensions.IsDevelopment(env))
                DeveloperExceptionPageExtensions.UseDeveloperExceptionPage(app);
            MvcApplicationBuilderExtensions.UseMvc(app, (System.Action<IRouteBuilder>)(
                    routes => MapRouteRouteBuilderExtensions.MapRoute(
                        routes, "default", "{controller=Home}/{action=Index}/{id?}"
                    )
            ));
        }
    }
}