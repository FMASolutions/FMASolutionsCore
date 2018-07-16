namespace FMASolutionsCore.Web.FMASolutionsWebsite.FMAWebsite
{
    public enum AppSettings
    {
        AppID,
        AppPassword
    }

    public static class C
    {        
        public static string[] SettingKeys = System.Enum.GetNames(typeof(AppSettings));
    }
}