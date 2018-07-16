using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FMASolutionsCore.BusinessServices.ControllerTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHostExtensions.Run(Program.BuildWebHost(args));
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHostBuilderExtensions.UseStartup<Startup>(WebHost.CreateDefaultBuilder(args)).Build();
        }
    }
}
