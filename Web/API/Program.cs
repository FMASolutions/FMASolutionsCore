using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using FMASolutionsCore.BusinessServices.AppLoggerExtension;
using FMASolutionsCore.BusinessServices.AppConfigExtension;
namespace FMASolutionsCore.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RegisterServices();
            LoggerService.WriteToProcessLog("Register Services Complete");
            LoggerService.WriteToProcessLog("StartingWebHost With Args: " + args.ToString());
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        public static IAppLoggerExtension LoggerService = new AppLoggerExtension();
        public static IAppConfigExtension ConfigService = new AppConfigExtension(C.SettingKeys);

        private static void RegisterServices()
        {
            try
            {

                LoggerService.Register();

                LoggerService.WriteToProcessLog("Registering Config Service");

                ConfigService.Register();

                LoggerService.WriteToProcessLog("Config Service Registration Complete");
            }
            catch (Exception ex)
            {
                LoggerService.WriteToErrorLog("Problem during InitSystems, Exception Message := " + ex.Message
                    + " Inner Message := " + ex.InnerException.Message, ex.Source.ToString());
            }

        }

    }
}
