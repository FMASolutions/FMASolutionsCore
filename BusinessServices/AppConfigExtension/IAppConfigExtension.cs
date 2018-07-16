using FMASolutionsCore.BusinessServices.BusinessCore.Extensions;
namespace FMASolutionsCore.BusinessServices.AppConfigExtension
{
    public interface IAppConfigExtension : IAppExtension
    {
        string GetSetting(string settingKey);
        void WriteSetting(string settingKey, string settingValue);
    }
}