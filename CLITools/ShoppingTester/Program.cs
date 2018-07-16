using FMASolutionsCore.BusinessServices.AppConfigExtension;
using System;

namespace FMASolutionsCore.CLITools.ShoppingTester
{
    class Program
    {
        static void Main(string[] args)
        {
            appConfig.Register();
            ShoppingTester tester = new ShoppingTester();            
            tester.Run();
        }

        internal static IAppConfigExtension appConfig = new AppConfigExtension(C.SettingsKeys);
    }
}
