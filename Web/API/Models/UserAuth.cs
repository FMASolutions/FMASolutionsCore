using FMASolutionsCore.DataServices.DataRepository.Auth;
using FMASolutionsCore.DataServices.DataRepository;
using FMASolutionsCore.BusinessServices.BusinessCore.Auth;
using FMASolutionsCore.DataServices.CryptoHelper;
using FMASolutionsCore.BusinessServices.SQLAppConfigTypes;
using System;

namespace FMASolutionsCore.Web.API.Models
{
    public class UserAuth
    {

        private UserAuth()
        {

        }
        public UserAuth(AuthRequest authRequest)
        {
            _rawInputString = AuthRequest.EncodeAndFormatToBase64(authRequest.Username, authRequest.Password, authRequest.ApplicationID, authRequest.ApplicationPassword);
            _authRequest = authRequest;
            _appModel = new ApprovedApps(_authRequest.ApplicationID, _authRequest.ApplicationPassword);

            SQLFactory fact = new SQLFactoryStandard();
            SQLAppConfigTypes dbType = (SQLAppConfigTypes)int.Parse(Program.ConfigService.GetSetting(AppSettings.AuthDBServerType.ToString()));
            string conString = Program.ConfigService.GetSetting(AppSettings.AuthDBConnectionString.ToString());
            System.Data.IDbConnection con = fact.CreateDBConnection(dbType, conString);

            _userRepo = new UserAuthRepo(con);
            _userAuthDBEntity = _userRepo.GetByUsername(authRequest.Username);
        }

        private UserAuthRepo _userRepo;
        private UserAuthEntity _userAuthDBEntity;
        private AuthRequest _authRequest;
        private ApprovedApps _appModel;
        private string _rawInputString;

        public AuthResponse PerformAuth()
        {
            AuthResponse resp = new AuthResponse(); //Default to Fail;                
            resp.Status = AuthResponse.StatusCode.Fail;
            resp.Message = "Auth Failed";
            resp.Token = string.Empty;
            try
            {
                Program.LoggerService.WriteToProcessLog("@" + System.DateTime.Now.ToString() + " Process Attempt With String: " + _rawInputString);
                string salt = Program.ConfigService.GetSetting(AppSettings.CryptoSaltValue.ToString());

                if (_appModel.IsApprovedApp())
                {
                    if (CryptoService.Encrypt(_authRequest.Password, _userAuthDBEntity.UserKey, salt) == _userAuthDBEntity.Password)
                    {
                        string token = System.Guid.NewGuid().ToString();
                        resp.Status = AuthResponse.StatusCode.Success;
                        resp.Message = "Auth Success";
                        resp.Token = System.Guid.NewGuid().ToString();
                        Program.LoggerService.WriteToProcessLog("@" + System.DateTime.Now.ToString() + " Successful Login for: " + _authRequest.Username + " Token: " + resp.Token);
                        return resp;
                    }
                    else
                    {
                        Program.LoggerService.WriteToProcessLog("@" + System.DateTime.Now.ToString() + " Incorrect Password for: " + _authRequest.Username + " Full input string was: " + _rawInputString);
                        return resp;
                    }
                }
                Program.LoggerService.WriteToProcessLog("@" + System.DateTime.Now.ToString() + " Unapproved or non-existant APP when processing request for: " + _authRequest.Username + " Full input string was: " + _rawInputString);
                return resp;
            }
            catch (Exception ex)
            {
                Program.LoggerService.WriteToErrorLog(ex.Message, ex.Source.ToString());
                return resp;
            }
        }
    }
}