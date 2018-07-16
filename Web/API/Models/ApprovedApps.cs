using FMASolutionsCore.DataServices.DataRepository.Auth;
using FMASolutionsCore.DataServices.DataRepository;
using FMASolutionsCore.DataServices.CryptoHelper;
using FMASolutionsCore.BusinessServices.SQLAppConfigTypes;

namespace FMASolutionsCore.Web.API.Models
{
    public class ApprovedApps
    {

        private string _appKey;
        private string _appPassword;

        private ApprovedApps()
        {
        }
        public ApprovedApps(string appKey, string appPassword)
        {
            _appKey = appKey;
            _appPassword = appPassword;
        }
        public string AppKey { get => _appKey; set => _appKey = value; }
        public string AppPassword { get => _appPassword; set => _appPassword = value; }

        public bool IsApprovedApp()
        {
            SQLFactory fact = new SQLFactoryStandard();
            SQLAppConfigTypes dbType = (SQLAppConfigTypes)int.Parse(Program.ConfigService.GetSetting(AppSettings.AuthDBServerType.ToString()));
            string conString = Program.ConfigService.GetSetting(AppSettings.AuthDBConnectionString.ToString());
            System.Data.IDbConnection con = fact.CreateDBConnection(dbType, conString);
            ApprovedAppsRepo appsRepo = new ApprovedAppsRepo(con);
            ApprovedAppsEntity appRecord = appsRepo.GetByAppKey(AppKey);


            string salt = Program.ConfigService.GetSetting(AppSettings.CryptoSaltValue.ToString());
            string recAppPassEnc = CryptoService.Encrypt(AppPassword, AppKey, salt);

            if (recAppPassEnc == appRecord.AppPassword)
                return true;

            return false;
        }

    }
}