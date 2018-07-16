namespace FMASolutionsCore.Web.API
{
    public enum AppSettings
    {
        CryptoSaltValue,
        AuthDBConnectionString,
        AuthDBServerType
    }
    public static class C
    {
        public static string[] SettingKeys = System.Enum.GetNames(typeof(AppSettings));
        public const string ProtocolUsername = "Username:";
        public const string ProtocolPassword = "Password:";
        public const string ProtocolApplicationID = "ApplicationID:";
        public const string ProtocolApplicationPassword = "ApplicationPassword:";
        public const string AuthSuccessMessage = "Auth Success TokenForUse=";
        public const string AuthFailMessage = "Auth Failed";
        public const string QueryAppPassword = "SELECT AppPassword FROM ApprovedApps Where AppKey= @AppKey";
        public const string QueryUserKey = "SELECT UserKey FROM Users Where Username= @Username";
        public const string QueryUserPassword = "SELECT Password FROM Users Where Username= @Username";
        public const string ParameterAppKey = "@AppKey";
        public const string ParameterUsername = "@Username";


    }

}