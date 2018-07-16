namespace FMASolutionsCore.Web.ShopBro
{
    public enum AppSettings
    {
        AppID,
        AppPassword,
        ShopBroDBConnectionString,
        ShopBroDBType
    }

    public static class C
    {
        public static string[] SettingsKeys = System.Enum.GetNames(typeof(AppSettings));
    }
}