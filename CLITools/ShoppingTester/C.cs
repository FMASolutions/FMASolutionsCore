namespace FMASolutionsCore.CLITools.ShoppingTester
{
    internal enum AppSettings
    {
        DBConnectionString,
        DBType

    }
    public static class C
    {
        public static string[] SettingsKeys = System.Enum.GetNames(typeof(AppSettings));
    }
}