using System;
using System.Text;

namespace FMASolutionsCore.BusinessServices.BusinessCore.Auth
{
    //Expect data format received to be "AuthProtocol:Username:testuser:Password:testpass:ApplicationID:appID:ApplicationPassword:applicationPassword" and base 64 encoded...
    //Example String = "AuthProtocol:Username:ExampleUsername:Password:ExamplePassword:ApplicationID:EXAMPLEAPPLICATIONIDWITHPADDIN32:ApplicationPassword:ExampleApplicationPassword"
    //Example Base64 = "QXV0aFByb3RvY29sOlVzZXJuYW1lOkV4YW1wbGVVc2VybmFtZTpQYXNzd29yZDpFeGFtcGxlUGFzc3dvcmQ6QXBwbGljYXRpb25JRDpFWEFNUExFQVBQTElDQVRJT05JRFdJVEhQQURESU4zMjpBcHBsaWNhdGlvblBhc3N3b3JkOkV4YW1wbGVBcHBsaWNhdGlvblBhc3N3b3Jk"
    public class AuthRequest
    {
        private const string _protocolUsername = "Username:";
        private const string _protocolPassword = "Password:";
        private const string _protocolApplicationID = "ApplicationID:";
        private const string _protocolApplicationPassword = "ApplicationPassword:";
        private string _username;
        private string _password;
        private string _applicationID;
        private string _applicationPassword;

        public AuthRequest(string username, string password, string applicationID, string applicationPassword)
        {
            _username = username;
            _password = password;
            _applicationID = applicationID;
            _applicationPassword = applicationPassword;
        }

        public string Username { get => _username; set => _username = value; }

        public string Password { get => _password; set => _password = value; }

        public string ApplicationID { get => _applicationID; set => _applicationID = value; }

        public string ApplicationPassword { get => _applicationPassword; set => _applicationPassword = value; }

        public string EncodeAndFormatToBase64() =>
            AuthRequest.EncodeAndFormatToBase64(Username, Password, ApplicationID, ApplicationPassword);

        public static string EncodeAndFormatToBase64(string username, string password, string applicationID, string applicationPassword) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(
                "AuthProtocol:"
                + "Username:" + username + ":"
                + "Password:" + password + ":"
                + "ApplicationID:" + applicationID + ":"
                + "ApplicationPassword:" + applicationPassword)
            );

        public static AuthRequest DecodeFromFormattedBase64(string base64EncodedString) =>
            new AuthRequest(
                AuthRequest.GetUsername(Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedString)))
                , AuthRequest.GetUserPassword(Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedString)))
                , AuthRequest.GetAppID(Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedString)))
                , AuthRequest.GetAppPassword(Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedString)))
            );

        private static string GetUsername(string base64DecodedString) =>
            base64DecodedString.Substring(
                base64DecodedString.IndexOf("Username:") + "Username:".Length
                , base64DecodedString.IndexOf("Password:") - (base64DecodedString.IndexOf("Username:") + "Username:".Length + 1)
            );

        private static string GetUserPassword(string base64DecodedString) =>
            base64DecodedString.Substring(base64DecodedString.IndexOf("Password:") + "Password:".Length
                , base64DecodedString.IndexOf("ApplicationID:") - (base64DecodedString.IndexOf("Password:") + "Password:".Length + 1));

        private static string GetAppID(string base64DecodedString) =>
            base64DecodedString.Substring(base64DecodedString.IndexOf("ApplicationID:") + "ApplicationID:".Length
                , base64DecodedString.IndexOf("ApplicationPassword:") - (base64DecodedString.IndexOf("ApplicationID:") + "ApplicationID:".Length + 1));

        private static string GetAppPassword(string base64DecodedString) =>
            base64DecodedString.Substring(base64DecodedString.IndexOf("ApplicationPassword:") + "ApplicationPassword:".Length);
    }
}
