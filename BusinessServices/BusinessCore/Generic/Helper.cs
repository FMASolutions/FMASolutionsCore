using System.Reflection;
namespace FMASolutionsCore.BusinessServices.BusinessCore.Generic
{
    public class Helper
    {
        public static string GetCurrentAppName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}