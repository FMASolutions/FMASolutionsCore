using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using FMASolutionsCore.BusinessServices.AppConfigExtension;
using FMASolutionsCore.BusinessServices.AppLoggerExtension;
namespace FMASolutionsCore.Web.FMASolutionsWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RegisterServices();
            WebHostExtensions.Run(Program.BuildWebHost(args));
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHostBuilderExtensions.UseStartup<Startup>(WebHost.CreateDefaultBuilder(args)).Build();
        }

        internal static IAppLoggerExtension LoggerService = new AppLoggerExtension();
        internal static IAppConfigExtension ConfigService = new AppConfigExtension(FMAWebsite.C.SettingKeys);

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