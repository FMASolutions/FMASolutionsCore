namespace FMASolutionsCore.BusinessServices.BusinessCore.Auth
{
    public class AuthResponse
    {
        private StatusCode _status;
        private string _message;
        private string _token;

        public enum StatusCode
        {
            Fail,
            Success,
        }

        public StatusCode Status { get => _status; set => _status = value; }
        public string Message { get => _message; set => _message = value; }
        public string Token { get => _token; set => _token = value; }
    }
}
