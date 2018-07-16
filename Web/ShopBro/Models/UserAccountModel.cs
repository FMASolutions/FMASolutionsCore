using System;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class UserAccountModel
    {
        public UserAccountModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }
        public string AuthMessage { get; set; }

        public bool HasValidCredentials()
        {
            try
            {
                /*
                string appID = Program.ConfigService.GetSetting(FMAWebsite.AppSettings.AppID.ToString());
                string appPass = Program.ConfigService.GetSetting(FMAWebsite.AppSettings.AppPassword.ToString());
                AuthRequest authRequest = new AuthRequest(Username, Password, appID, appPass);

                string returnMessage = WebCrawler.APIPostCall("https://www.fmasolutionsauthservice.co.uk/", "User/TryAuth", authRequest.EncodeAndFormatToBase64());
                if (returnMessage.Contains("Auth Success"))
                {
                    Program.LoggerService.WriteToProcessLog("Successful auth message for user: " + Username + " received");
                    AuthMessage = returnMessage;
                    return true;
                }
                else if (returnMessage.Contains("Auth Failed"))
                {
                    Program.LoggerService.WriteToProcessLog("Auth Failed message for user: " + Username + " received");
                    AuthMessage = returnMessage;
                    return false;
                }
                else
                {
                    Program.LoggerService.WriteToProcessLog("No reply received for user: " + Username);
                    AuthMessage = "Non standard Response Received";
                    return false;
                }
                */
                return false;
            }
            catch (Exception ex)
            {
                Program.loggerExtension.WriteToErrorLog(ex.Message, ex.Source.ToCharArray());
                AuthMessage = "Error occured during auth, ERROR=: " + ex.Message;
                return false;
            }
        }
    }

}