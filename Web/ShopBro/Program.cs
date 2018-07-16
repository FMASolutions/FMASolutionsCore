using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using FMASolutionsCore.BusinessServices.AppLoggerExtension;
using FMASolutionsCore.BusinessServices.AppConfigExtension;

namespace FMASolutionsCore.Web.ShopBro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RegisterServices();            
            loggerExtension.WriteToProcessLog("RegisterServices Complete");

            loggerExtension.WriteToProcessLog("Run the host with args:  " + args.ToString());
            WebHostExtensions.Run(Program.BuildWebHost(args));
            loggerExtension.WriteToProcessLog("Run host complete");
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHostBuilderExtensions.UseStartup<Startup>(WebHost.CreateDefaultBuilder(args)).Build();
        }

        internal static IAppLoggerExtension loggerExtension = new AppLoggerExtension();
        internal static IAppConfigExtension configExtension = new AppConfigExtension(ShopBro.C.SettingsKeys);
        
        private static void InitSystems()
        {
            
        }

        private static void RegisterServices()
        {
            try
            {
                loggerExtension.Register();

                loggerExtension.WriteToProcessLog("Registering Config Service");

                configExtension.Register();               

                loggerExtension.WriteToProcessLog("Config Service Registration Complete");
            }
            catch (Exception ex)
            {
                loggerExtension.WriteToErrorLog("Problem during InitSystems, Exception Message := " + ex.Message
                    + " Inner Message := " + ex.InnerException.Message, ex.Source.ToString());
            }

        }
    }
}